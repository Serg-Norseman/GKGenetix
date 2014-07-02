using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Genetic_Genealogy_Kit
{
    public partial class MitoMapFrm : Form
    {
        char[] RSRS = null;
        string kit = null;
        Dictionary<int, string[]> nucleotides = new Dictionary<int, string[]>();

        SortedDictionary<int, List<string>> kit_mutations = new SortedDictionary<int, List<string>>();
        SortedDictionary<int, List<string>> kit_insertions = new SortedDictionary<int, List<string>>();

        string mutations = null;

        int initial_value = -1;

        public MitoMapFrm(string kit)
        {
            InitializeComponent();
            this.kit = kit;
            label1.Text = " " + kit + " (" + GGKUtilLib.getKitName(kit) + ")";
        }

        private void MitoMapFrm_Load(object sender, EventArgs e)
        {
            string csv = Genetic_Genealogy_Kit.Properties.Resources.mtdna_map;
            StreamReader reader = new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(csv)));
            string line = null;
            string[] data = null;
            Series series = series = mtdna_chart.Series[0];
            //Map Locus	 Starting	 Ending	bp Length	 Shorthand	 Description
            dgvmtdna.Rows.Clear();
            while((line = reader.ReadLine())!=null)
            {
                data=line.Split(new char[]{','});
                DataPoint dp=new DataPoint();
                dp.IsVisibleInLegend = false;
                dp.Label = data[0];
                dp.YValues=new double[]{int.Parse(data[3])};
                dp.CustomProperties = "PieLineColor=Black, PieLabelStyle=Outside, Exploded=True";
                series.Points.Add(dp);
                //
                dgvmtdna.Rows.Add(new object[] { data[0], data[1], data[2], data[3], data[4], data[5] });                
            }
            reader.Close();
            //
            RSRS = GGKUtilLib.getRSRS();
            for (int i = 0; i < RSRS.Length; i++)
                nucleotides.Add(i + 1, new string[] {(i + 1).ToString(), RSRS[i].ToString(),  RSRS[i].ToString() });

            mutations = GGKUtilLib.queryValue("kit_mtdna", new string[] { "mutations" }, "where kit_no='" + kit + "'");
            dgvNucleotides.Columns[2].HeaderText = GGKUtilLib.getKitName(kit) + " (" + kit + ")";

            dgvmtdna.Columns[3].Visible = false;

            loadKitMutations();
        }

        private void dgvmtdna_SelectionChanged(object sender, EventArgs e)
        {
            int start = int.Parse(dgvmtdna.SelectedRows[0].Cells[1].Value.ToString());
            int end = int.Parse(dgvmtdna.SelectedRows[0].Cells[2].Value.ToString());
            string title = dgvmtdna.SelectedRows[0].Cells[0].Value.ToString();

            foreach (DataPoint dp in mtdna_chart.Series[0].Points)
            {
                if(dp.Label==title)
                    dp.LabelBackColor = Color.LightBlue;
                else
                    dp.LabelBackColor = Color.White;
            }

            tabControl2.TabPages[0].Text = "Nucleotides - "+title;
            dgvNucleotides.Rows.Clear();
            int end_tmp = end;
            if (end < start)
                end_tmp = 16569;
            for (int i = start; i <= end_tmp; i++)
                dgvNucleotides.Rows.Add(nucleotides[i]);
            if(end<start)
            {
                for (int i = 1; i <= end; i++)
                    dgvNucleotides.Rows.Add(nucleotides[i]);
            }

            DataGridViewRowCollection dgvrc=dgvNucleotides.Rows;

            DataGridViewCellStyle style_point = new DataGridViewCellStyle();
            style_point.BackColor = Color.Blue;
            style_point.ForeColor = Color.White;
            DataGridViewCellStyle style_insert = new DataGridViewCellStyle();
            style_insert.BackColor = Color.Green;
            style_insert.ForeColor = Color.White;

            end_tmp = end;
            if (end < start)
                end_tmp = 16569;

            foreach(KeyValuePair<int,List<string>> a in kit_mutations)
            {
                if ((a.Key >= start && a.Key <= end_tmp) || (end < start && a.Key <= end))
                {
                    for (int i = 0; i < dgvrc.Count; i++)
                    {
                        if (int.Parse(dgvrc[i].Cells[0].Value.ToString()) == a.Key)
                        {
                            dgvNucleotides.Rows[i].Cells[2].Value = a.Value[0];
                            dgvNucleotides.Rows[i].DefaultCellStyle = style_point;
                        }
                    }
                }
            }

            foreach (KeyValuePair<int, List<string>> a in kit_insertions)
            {
                if ((a.Key >= start && a.Key <= end_tmp) || (end < start && a.Key <= end))
                {
                    dgvrc = dgvNucleotides.Rows;
                    for (int i = 0; i < dgvrc.Count; i++)
                    {
                        if (int.Parse(dgvrc[i].Cells[0].Value.ToString()) == a.Key+1)
                        {
                            foreach (string v in a.Value)
                            {                                
                                dgvNucleotides.Rows.Insert(i, new object[] { a.Key, "", v });
                                dgvNucleotides.Rows[i].DefaultCellStyle = style_insert;                                
                            }
                            break;
                        }
                    }
                }
            }
            populateFASTA(title, start,end);
        }

        private void loadKitMutations()
        {
           string mut = null;
           string allele = null;
           int pos = -1;
           List<string> alleles = null;
           foreach(string mutation in mutations.Split(new char[]{','}))
           {
               mut = mutation.Trim();
               if(mut.IndexOf(".")!=-1)
               {
                   // insert
                   allele = mut[mut.Length - 1].ToString();
                   pos = int.Parse(mut.Substring(0, mut.IndexOf(".")));
                   if (!kit_insertions.ContainsKey(pos))
                       alleles = new List<string>();
                   else
                       alleles = kit_insertions[pos];
                   alleles.Add(allele);
                   kit_insertions.Remove(pos);
                   kit_insertions.Add(pos, alleles);
               }
               else
               {
                   allele = mut[mut.Length - 1].ToString();
                   pos = int.Parse(mut.Substring(1, mut.Length - 2));
                   if (!kit_mutations.ContainsKey(pos))
                       alleles = new List<string>();
                   else
                       alleles = kit_mutations[pos];
                   alleles.Add(allele);
                   kit_mutations.Remove(pos);
                   kit_mutations.Add(pos, alleles);
               }
           }
        }

        private void mtdna_chart_MouseDown(object sender, MouseEventArgs e)
        {
            initial_value = e.Y;
        }

        private void mtdna_chart_MouseMove(object sender, MouseEventArgs e)
        {
            if(initial_value!=-1)
            {
                if (e.Button == MouseButtons.Left)
                {
                    int new_y = e.Y - initial_value;

                    int new_degree = new_y * (90) / 1200;

                    new_degree = new_degree + mtdna_chart.ChartAreas[0].Area3DStyle.Inclination;

                    if (new_degree < -90)
                        new_degree = 90 + (new_degree + 90);

                    if (new_degree > 90)
                        new_degree = -90 + (new_degree - 90);

                    mtdna_chart.ChartAreas[0].Area3DStyle.Inclination = new_degree;
                }
                if (e.Button == MouseButtons.Right)
                {
                    int new_y = e.Y - initial_value;

                    int new_degree = new_y * 180 / 1200;

                    string tmp = mtdna_chart.Series[0].CustomProperties;
                    int start_pos=tmp.IndexOf("PieStartAngle=") + "PieStartAngle=".Length;
                    tmp = tmp.Substring(start_pos);
                    start_pos = tmp.IndexOf(",");
                    if(start_pos!=-1)
                        tmp = tmp.Substring(0,start_pos);
                    int angle = int.Parse(tmp.Trim());

                    new_degree = new_degree + angle;

                    if (new_degree < -180)
                        new_degree = 180 + (new_degree + 180);

                    if (new_degree > 180)
                        new_degree = -180 + (new_degree - 180);
                    //PieLineColor=Black, CollectedSliceExploded=True, DoughnutRadius=5, PieDrawingStyle=SoftEdge, PieLabelStyle=Outside, PieStartAngle=300
                    mtdna_chart.Series[0].CustomProperties = "PieLineColor=Black, CollectedSliceExploded=True, DoughnutRadius=5, PieDrawingStyle=SoftEdge, PieLabelStyle=Outside, PieStartAngle=" + new_degree;
                }
            }
        }

        private void mtdna_chart_MouseUp(object sender, MouseEventArgs e)
        {
            initial_value = -1;
        }

        private void mtdna_chart_MouseClick(object sender, MouseEventArgs e)
        {
            // Call Hit Test Method
            HitTestResult result = mtdna_chart.HitTest(e.X, e.Y);
           
            if (result.ChartElementType == ChartElementType.DataPoint || result.ChartElementType == ChartElementType.DataPointLabel)
            {
                foreach(DataPoint dp2 in mtdna_chart.Series[0].Points)
                {
                    dp2.LabelBackColor = Color.White;
                    dp2.LabelForeColor = Color.Black;
                }
                
                DataPoint dp = (DataPoint)result.Object;
                dp.LabelBackColor = Color.LightBlue;
                GGKUtilLib.setStatus("Selected " + dp.Label);

                foreach(DataGridViewRow row in dgvmtdna.Rows)
                {
                    if (row.Cells[0].Value.ToString() == dp.Label)
                    {
                        row.Selected = true;
                        break;
                    }
                }                
            }
        }

        private void populateFASTA(string locus, int start,int end)
        {
            StringBuilder sb = new StringBuilder();
            int width = 0;
            Dictionary<int, Color> list = new Dictionary<int, Color>();
            sb.Append(">" + kit + "|" + locus + "|"+start+"-"+end+"\r\n");
           foreach(DataGridViewRow row in dgvNucleotides.Rows)
           {
               if(width%50==0 && width!=0)
                   sb.Append("\r\n");
               if (row.DefaultCellStyle.BackColor == Color.Blue || row.DefaultCellStyle.BackColor == Color.Green)
               {
                   sb.Append(row.Cells[2].Value.ToString());
                   list.Add(sb.Length - 1, row.DefaultCellStyle.BackColor);
               }
               else
                   sb.Append(row.Cells[2].Value.ToString());
               width++;               

           }
           rtbFASTA.Text =  sb.ToString();
           foreach(KeyValuePair<int,Color> a in list)
           {
               rtbFASTA.SelectionStart = a.Key;
               rtbFASTA.SelectionLength = 1;
               rtbFASTA.SelectionColor = a.Value;
           }
           rtbFASTA.SelectionStart = 0;
           rtbFASTA.SelectionLength = 0;
           rtbFASTA.SelectedText = "";
        }
    }
}

/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using GGKit.Core;
using GKGenetix.Core.Model;
using GKGenetix.UI;

namespace GGKit.Forms
{
    public partial class MitoMapFrm : GKWidget
    {
        /*internal class MDNucleotide
        {
            public int Pos { get; }
            public string A1 { get; }
            public string A2 { get; }

            public bool Mut;
            public bool Ins;

            public MDNucleotide(int pos, string a1, string a2)
            {
                Pos = pos;
                A1 = a1;
                A2 = a2;
            }
        }*/

        private string kit = null;
        private readonly SortedDictionary<int, List<string>> kitMutations = new SortedDictionary<int, List<string>>();
        private readonly SortedDictionary<int, List<string>> kitInsertions = new SortedDictionary<int, List<string>>();
        private int initialValue = -1;
        private DataGridViewCellStyle styleMut, styleIns;


        public static bool CanBeUsed(IList<KitDTO> selectedKits)
        {
            return (selectedKits != null && selectedKits.Count == 1 && !selectedKits[0].Disabled && GKSqlFuncs.ExistsMtDna(selectedKits[0].KitNo));
        }


        public MitoMapFrm(IKitHost host, IList<KitDTO> selectedKits) : this(host, selectedKits[0].KitNo)
        {
        }

        public MitoMapFrm(IKitHost host, string kit) : base(host)
        {
            InitializeComponent();

            styleMut = new DataGridViewCellStyle { BackColor = Color.Blue, ForeColor = Color.White };
            styleIns = new DataGridViewCellStyle { BackColor = Color.Green, ForeColor = Color.White };

            UIHelper.FixGridView(dgvMtDna);
            dgvMtDna.AddColumn("MapLocus", "Map Locus");
            dgvMtDna.AddColumn("Starting", "Start Position");
            dgvMtDna.AddColumn("Ending", "End Position");
            //dgvmtdna.AddColumn("bpLength", "Total Nucleotides");
            dgvMtDna.AddColumn("Shorthand", "Shorthand");
            dgvMtDna.AddColumn("Description", "Description");

            UIHelper.FixGridView(dgvNucleotides);
            dgvNucleotides.AddColumn("pos", "Position");
            dgvNucleotides.AddColumn("rsrsname", "RSRS");
            dgvNucleotides.AddColumn("ckit", "Kit");

            this.kit = kit;
        }

        public override void SetKit(IList<KitDTO> selectedKits)
        {
            if (CanBeUsed(selectedKits)) {
                this.kit = selectedKits[0].KitNo;
                ReloadData();
            }
        }

        private void ReloadData()
        {
            this.Text = $"Mito Map : {kit} ({GKSqlFuncs.GetKitName(kit)})";

            Series series = mtdna_chart.Series[0];

            series.Points.Clear();
            var mtdna_map = GKData.MtDnaMap;
            foreach (var mdm in mtdna_map) {
                DataPoint dp = new DataPoint();
                dp.IsVisibleInLegend = false;
                dp.Label = mdm.MapLocus;
                dp.YValues = new double[] { int.Parse(mdm.bpLength) };
                dp.CustomProperties = "PieLineColor=Black, PieLabelStyle=Outside, Exploded=True";
                series.Points.Add(dp);
            }
            dgvMtDna.DataSource = mtdna_map;

            Task.Factory.StartNew(() => {
                GKGenFuncs.GetMtDNA(kit, out string mutations, out _, kitMutations, kitInsertions);

                this.Invoke(new MethodInvoker(delegate {
                    dgvNucleotides.Columns[2].HeaderText = $"{kit} ({GKSqlFuncs.GetKitName(kit)})";
                }));
            });
        }

        private void MitoMapFrm_Load(object sender, EventArgs e)
        {
            ReloadData();
        }

        private void dgvMtDna_SelectionChanged(object sender, EventArgs e)
        {
            var selRow = dgvMtDna.GetSelectedObj<MDMapRow>();
            if (selRow == null) return;

            int start = int.Parse(selRow.Starting);
            int end = int.Parse(selRow.Ending);
            string title = selRow.MapLocus;

            foreach (DataPoint dp in mtdna_chart.Series[0].Points) {
                dp.LabelBackColor = (dp.Label == title) ? Color.LightBlue : Color.White;
            }

            tabControl2.TabPages[0].Text = "Nucleotides - " + title;

            int end_tmp = end;
            if (end < start)
                end_tmp = 16569;

            PopulateNucleotides(start, end, end_tmp);
            PopulateFASTA(title, start, end);
        }

        private void PopulateNucleotides(int start, int end, int end_tmp)
        {
            var nucleotides = new Dictionary<int, string[]>();
            var RSRS = GKData.RSRS;
            for (int i = 0; i < RSRS.Length; i++)
                nucleotides.Add(i + 1, new string[] { (i + 1).ToString(), RSRS[i].ToString(), RSRS[i].ToString() });

            DataGridViewRowCollection dgvRows = dgvNucleotides.Rows;
            dgvRows.Clear();

            for (int i = start; i <= end_tmp; i++)
                dgvRows.Add(nucleotides[i]);

            if (end < start) {
                for (int i = 1; i <= end; i++)
                    dgvRows.Add(nucleotides[i]);
            }

            foreach (KeyValuePair<int, List<string>> a in kitMutations) {
                if ((a.Key >= start && a.Key <= end_tmp) || (end < start && a.Key <= end)) {
                    string akey = a.Key.ToString();
                    for (int i = 0; i < dgvRows.Count; i++) {
                        if (dgvRows[i].Cells[0].Value.ToString() == akey) {
                            dgvRows[i].Cells[2].Value = a.Value[0];
                            dgvRows[i].DefaultCellStyle = styleMut;
                            break;
                        }
                    }
                }
            }

            foreach (KeyValuePair<int, List<string>> a in kitInsertions) {
                if ((a.Key >= start && a.Key <= end_tmp) || (end < start && a.Key <= end)) {
                    string akey1 = (a.Key + 1).ToString();
                    for (int i = 0; i < dgvRows.Count; i++) {
                        if (dgvRows[i].Cells[0].Value.ToString() == akey1) {
                            foreach (string v in a.Value) {
                                dgvRows.Insert(i, new object[] { a.Key, "", v });
                                dgvRows[i].DefaultCellStyle = styleIns;
                            }
                            break;
                        }
                    }
                }
            }
        }

        private void dgvNucleotides_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            /*var row = tblAlleles[e.RowIndex];
            var cellVal = row.Match.ToString();

            if (cellVal == "-")
                e.CellStyle.BackColor = Color.LightGray;
            else if (cellVal == "")
                e.CellStyle.BackColor = Color.OrangeRed;*/
        }

        private void mtdna_chart_MouseDown(object sender, MouseEventArgs e)
        {
            initialValue = e.Y;
        }

        private void mtdna_chart_MouseMove(object sender, MouseEventArgs e)
        {
            if (initialValue != -1) {
                if (e.Button == MouseButtons.Left) {
                    int new_y = e.Y - initialValue;
                    int new_degree = new_y * (90) / 1200;
                    new_degree += mtdna_chart.ChartAreas[0].Area3DStyle.Inclination;

                    if (new_degree < -90)
                        new_degree = 90 + (new_degree + 90);

                    if (new_degree > 90)
                        new_degree = -90 + (new_degree - 90);

                    mtdna_chart.ChartAreas[0].Area3DStyle.Inclination = new_degree;
                }

                if (e.Button == MouseButtons.Right) {
                    int new_y = e.Y - initialValue;
                    int new_degree = new_y * 180 / 1200;

                    string tmp = mtdna_chart.Series[0].CustomProperties;
                    int start_pos = tmp.IndexOf("PieStartAngle=") + "PieStartAngle=".Length;
                    tmp = tmp.Substring(start_pos);
                    start_pos = tmp.IndexOf(",");
                    if (start_pos != -1)
                        tmp = tmp.Substring(0, start_pos);

                    int angle = int.Parse(tmp.Trim());
                    new_degree += angle;

                    if (new_degree < -180)
                        new_degree = 180 + (new_degree + 180);

                    if (new_degree > 180)
                        new_degree = -180 + (new_degree - 180);

                    mtdna_chart.Series[0].CustomProperties = "PieLineColor=Black, CollectedSliceExploded=True, DoughnutRadius=5, PieDrawingStyle=SoftEdge, PieLabelStyle=Outside, PieStartAngle=" + new_degree;
                }
            }
        }

        private void mtdna_chart_MouseUp(object sender, MouseEventArgs e)
        {
            initialValue = -1;
        }

        private void mtdna_chart_MouseClick(object sender, MouseEventArgs e)
        {
            // Call Hit Test Method
            HitTestResult result = mtdna_chart.HitTest(e.X, e.Y);

            if (result.ChartElementType == ChartElementType.DataPoint || result.ChartElementType == ChartElementType.DataPointLabel) {
                foreach (DataPoint dp2 in mtdna_chart.Series[0].Points) {
                    dp2.LabelBackColor = Color.White;
                    dp2.LabelForeColor = Color.Black;
                }

                DataPoint dp = (DataPoint)result.Object;
                dp.LabelBackColor = Color.LightBlue;
                _host.SetStatus("Selected " + dp.Label);

                foreach (DataGridViewRow dgvRow in dgvMtDna.Rows) {
                    var row = (MDMapRow)dgvRow.DataBoundItem;
                    if (row.MapLocus == dp.Label) {
                        dgvRow.Selected = true;
                        break;
                    }
                }
            }
        }

        private void PopulateFASTA(string locus, int start, int end)
        {
            StringBuilder sb = new StringBuilder();
            int width = 0;
            Dictionary<int, Color> list = new Dictionary<int, Color>();
            sb.Append(">" + kit + "|" + locus + "|" + start + "-" + end + "\r\n");

            foreach (DataGridViewRow row in dgvNucleotides.Rows) {
                string val = row.Cells[2].Value.ToString();

                if (width % 50 == 0 && width != 0)
                    sb.Append("\r\n");
                if (row.DefaultCellStyle.BackColor == Color.Blue || row.DefaultCellStyle.BackColor == Color.Green) {
                    sb.Append(val);
                    list.Add(sb.Length - 1, row.DefaultCellStyle.BackColor);
                } else
                    sb.Append(val);

                width++;
            }

            rtbFASTA.Text = sb.ToString();
            foreach (KeyValuePair<int, Color> a in list) {
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

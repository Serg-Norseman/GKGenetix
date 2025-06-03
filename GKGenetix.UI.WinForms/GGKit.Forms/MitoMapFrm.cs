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
        private string kit = null;
        private readonly SortedDictionary<int, List<string>> kitMutations = new SortedDictionary<int, List<string>>();
        private readonly SortedDictionary<int, List<string>> kitInsertions = new SortedDictionary<int, List<string>>();
        private int initialValue = -1;


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

            UIHelper.FixGridView(dgvMtDna);
            dgvMtDna.AddColumn("MapLocus", "Map Locus");
            dgvMtDna.AddColumn("Starting", "Start Position");
            dgvMtDna.AddColumn("Ending", "End Position");
            //dgvmtdna.AddColumn("bpLength", "Total Nucleotides");
            dgvMtDna.AddColumn("Shorthand", "Shorthand");
            dgvMtDna.AddColumn("Description", "Description");

            UIHelper.FixGridView(dgvNucleotides);
            dgvNucleotides.AddColumn("Pos", "Position");
            dgvNucleotides.AddColumn("RSRS", "RSRS");
            dgvNucleotides.AddColumn("Kit", "Kit");

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

            string title = selRow.MapLocus;
            foreach (DataPoint dp in mtdna_chart.Series[0].Points) {
                dp.LabelBackColor = (dp.Label == title) ? Color.LightBlue : Color.White;
            }
            tabControl2.TabPages[0].Text = "Nucleotides - " + title;

            int start = int.Parse(selRow.Starting);
            int end = int.Parse(selRow.Ending);
            dgvNucleotides.DataSource = GKGenFuncs.PopulateMtDnaNucleotides(start, end, kitMutations, kitInsertions);
            PopulateFASTA(title, start, end);
        }

        private void dgvNucleotides_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var nucleotides = (List<MDNucleotide>)dgvNucleotides.DataSource;
            var row = nucleotides[e.RowIndex];

            if (row.Mut) {
                e.CellStyle.BackColor = Color.Blue;
                e.CellStyle.ForeColor = Color.White;
            } else if (row.Ins) {
                e.CellStyle.BackColor = Color.Green;
                e.CellStyle.ForeColor = Color.White;
            }
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
            sb.Append(">" + kit + "|" + locus + "|" + start + "-" + end + "\r");
            // with \r\n sb.Length has wrong value for format

            var nucleotides = (List<MDNucleotide>)dgvNucleotides.DataSource;
            foreach (var row in nucleotides) {
                string val = row.Kit;

                if (width % 30 == 0 && width != 0)
                    sb.Append("\r");

                if (row.Mut) {
                    sb.Append(val);
                    list.Add(sb.Length - 1, Color.Blue);
                } else if (row.Ins) {
                    sb.Append(val);
                    list.Add(sb.Length - 1, Color.Green);
                } else {
                    sb.Append(val);
                }

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

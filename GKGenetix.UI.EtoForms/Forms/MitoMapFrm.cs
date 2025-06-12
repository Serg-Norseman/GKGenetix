/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eto.Drawing;
using Eto.Forms;
using Eto.Serialization.Xaml;
using GKGenetix.Core;
using GKGenetix.Core.Database;
using GKGenetix.Core.Model;
using GKGenetix.Core.Reference;

namespace GKGenetix.UI.Forms
{
    public partial class MitoMapFrm : GKWidget
    {
        #region Design components
#pragma warning disable CS0169, CS0649, IDE0044, IDE0051

        //private DataVisualization.Charting.Chart mtdna_chart;
        private TabPage mtMapTab;
        private TabPage details;
        private GridView dgvMtDna;
        private TabPage rsrs;
        private GridView dgvNucleotides;
        private TabPage tabFASTA;
        private RichTextArea rtbFASTA;

#pragma warning restore CS0169, CS0649, IDE0044, IDE0051
        #endregion


        private string kit = null;
        private readonly SortedDictionary<int, List<string>> kitMutations = new SortedDictionary<int, List<string>>();
        private readonly SortedDictionary<int, List<string>> kitInsertions = new SortedDictionary<int, List<string>>();
        private float initialValue = -1;


        public static bool CanBeUsed(IList<TestRecord> selectedKits)
        {
            return (selectedKits != null && selectedKits.Count == 1 && !selectedKits[0].Disabled && GKSqlFuncs.ExistsMtDNA(selectedKits[0].KitNo));
        }


        public MitoMapFrm(IKitHost host, IList<TestRecord> selectedKits) : this(host, selectedKits[0].KitNo)
        {
        }

        public MitoMapFrm(IKitHost host, string kit) : base(host)
        {
            XamlReader.Load(this);

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

        public override void SetKit(IList<TestRecord> selectedKits)
        {
            if (CanBeUsed(selectedKits)) {
                this.kit = selectedKits[0].KitNo;
                ReloadData();
            }
        }

        private void ReloadData()
        {
            this.Text = $"Mito Map : {kit} ({GKSqlFuncs.GetKitName(kit)})";

            var mtdna_map = RefData.MtDnaMap;

            /*Series series = mtdna_chart.Series[0];
            series.Points.Clear();
            foreach (var mdm in mtdna_map) {
                DataPoint dp = new DataPoint();
                dp.IsVisibleInLegend = false;
                dp.Label = mdm.MapLocus;
                dp.YValues = new double[] { int.Parse(mdm.bpLength) };
                dp.CustomProperties = "PieLineColor=Black, PieLabelStyle=Outside, Exploded=True";
                series.Points.Add(dp);
            }*/

            dgvMtDna.DataStore = mtdna_map;

            Task.Factory.StartNew(() => {
                GKGenFuncs.GetMtDNA(kit, out string mutations, out _, kitMutations, kitInsertions);

                Application.Instance.Invoke(new Action(delegate {
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
            var selRow = dgvMtDna.GetSelectedObj<MtDNAMapItem>();
            if (selRow == null) return;

            string title = selRow.MapLocus;
            /*foreach (DataPoint dp in mtdna_chart.Series[0].Points) {
                dp.LabelBackColor = (dp.Label == title) ? Colors.LightBlue : Colors.White;
            }*/
            rsrs.Text = "Nucleotides - " + title;

            int start = int.Parse(selRow.Starting);
            int end = int.Parse(selRow.Ending);
            dgvNucleotides.DataStore = GKGenFuncs.PopulateMtDnaNucleotides(start, end, kitMutations, kitInsertions);
            PopulateFASTA(title, start, end);
        }

        private void dgvNucleotides_CellFormatting(object sender, GridCellFormatEventArgs e)
        {
            var nucleotides = (List<MtDNANucleotide>)dgvNucleotides.DataStore;
            var row = nucleotides[e.Row];

            if (row.Mut) {
                e.BackgroundColor = Colors.Blue;
                e.ForegroundColor = Colors.White;
            } else if (row.Ins) {
                e.BackgroundColor = Colors.Green;
                e.ForegroundColor = Colors.White;
            }
        }

        private void mtdna_chart_MouseDown(object sender, MouseEventArgs e)
        {
            var mPt = e.Location;
            initialValue = mPt.Y;
        }

        private void mtdna_chart_MouseMove(object sender, MouseEventArgs e)
        {
            /*var mPt = e.Location;
            if (initialValue != -1) {
                if (e.Buttons == MouseButtons.Primary) {
                    int new_y = mPt.Y - initialValue;
                    int new_degree = new_y * (90) / 1200;
                    new_degree += mtdna_chart.ChartAreas[0].Area3DStyle.Inclination;

                    if (new_degree < -90)
                        new_degree = 90 + (new_degree + 90);

                    if (new_degree > 90)
                        new_degree = -90 + (new_degree - 90);

                    mtdna_chart.ChartAreas[0].Area3DStyle.Inclination = new_degree;
                }

                if (e.Buttons == MouseButtons.Alternate) {
                    int new_y = mPt.Y - initialValue;
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
            }*/
        }

        private void mtdna_chart_MouseUp(object sender, MouseEventArgs e)
        {
            initialValue = -1;
        }

        private void mtdna_chart_MouseClick(object sender, MouseEventArgs e)
        {
            // Call Hit Test Method
            /*var mPt = e.Location;
            HitTestResult result = mtdna_chart.HitTest(mPt.X, mPt.Y);

            if (result.ChartElementType == ChartElementType.DataPoint || result.ChartElementType == ChartElementType.DataPointLabel) {
                foreach (DataPoint dp2 in mtdna_chart.Series[0].Points) {
                    dp2.LabelBackColor = Colors.White;
                    dp2.LabelForeColor = Colors.Black;
                }

                DataPoint dp = (DataPoint)result.Object;
                dp.LabelBackColor = Colors.LightBlue;
                _host.SetStatus("Selected " + dp.Label);

                foreach (DataGridViewRow dgvRow in dgvMtDna.Rows) {
                    var row = (MtDNAMapItem)dgvRow.DataBoundItem;
                    if (row.MapLocus == dp.Label) {
                        dgvRow.Selected = true;
                        break;
                    }
                }
            }*/
        }

        private void PopulateFASTA(string locus, int start, int end)
        {
            var nucleotides = (List<MtDNANucleotide>)dgvNucleotides.DataStore;

            var fastaHgl = GKGenFuncs.GetFastaHighlights(kit, locus, start, end, nucleotides, out string fasta);

            rtbFASTA.Text = fasta;
            foreach (var a in fastaHgl) {
                rtbFASTA.Selection = new Range<int>(a.Start, a.Start);
                rtbFASTA.SelectionForeground = (a.State == GKGenFuncs.HGS_MB) ? Colors.Blue : ((a.State == GKGenFuncs.HGS_MG) ? Colors.Green : Colors.Black);
            }
            rtbFASTA.Selection = new Range<int>(0, 0);
            rtbFASTA.SelectedText = "";
        }
    }
}

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
using OxyPlot;
using OxyPlot.Eto;
using OxyPlot.Series;

namespace GKGenetix.UI.Forms
{
    public partial class MitoMapFrm : GKWidget
    {
        #region Design components
#pragma warning disable CS0169, CS0649, IDE0044, IDE0051

        private PlotView mtdna_chart;
        private TabPage mtMapTab;
        private TabPage details;
        private GridView dgvMtDna;
        private TabPage rsrs;
        private GridView dgvNucleotides;
        private TabPage tabFASTA;
        private RichTextArea rtbFASTA;

#pragma warning restore CS0169, CS0649, IDE0044, IDE0051
        #endregion


        private PlotModel plotModel;
        private string kit = null;
        private readonly SortedDictionary<int, List<string>> kitMutations = new SortedDictionary<int, List<string>>();
        private readonly SortedDictionary<int, List<string>> kitInsertions = new SortedDictionary<int, List<string>>();
        private List<MtDNAMapItem> mtdna_map;


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

            plotModel = new PlotModel();
            mtdna_chart.Model = plotModel;
            mtdna_chart.MouseDown += mtdna_chart_MouseClick;

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

            mtdna_map = RefData.MtDnaMap;

            plotModel.Series.Clear();
            plotModel.Title = "Mito Map";
            var series = new PieSeries {
                InsideLabelPosition = 0.9,
                InnerDiameter = 0.95,
            };
            plotModel.Series.Add(series);

            series.Slices.Clear();
            foreach (var mdm in mtdna_map) {
                var slice = new PieSlice(mdm.MapLocus, int.Parse(mdm.bpLength));
                series.Slices.Add(slice);
            }
            mtdna_chart.Invalidate();

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

        private void mtdna_chart_MouseClick(object sender, MouseEventArgs e)
        {
            var series = plotModel.Series[0] as PieSeries;
            if (series != null) {
                var point = new ScreenPoint(e.Location.X, e.Location.Y);
                var nearestSlice = series.GetNearestPoint(point, true);
                if (nearestSlice != null) {
                    _host.SetStatus("Selected " + nearestSlice.Text);
                    dgvMtDna.SelectedRow = (int)nearestSlice.Index;
                }
            }
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

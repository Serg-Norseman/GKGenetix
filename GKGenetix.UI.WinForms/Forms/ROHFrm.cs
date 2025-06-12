/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using GKGenetix.Core;
using GKGenetix.Core.Database;
using GKGenetix.Core.Model;

namespace GKGenetix.UI.Forms
{
    public partial class ROHFrm : GKWidget
    {
        private string kit = null;
        private IList<ROHSegment> roh_results;


        public static bool CanBeUsed(IList<TestRecord> selectedKits)
        {
            return (selectedKits != null && selectedKits.Count == 1 && !selectedKits[0].Disabled);
        }


        public ROHFrm(IKitHost host, IList<TestRecord> selectedKits) : this(host, selectedKits[0].KitNo)
        {
        }

        public ROHFrm(IKitHost host, string kit) : base(host)
        {
            InitializeComponent();
            this.kit = kit;

            UIHelper.FixGridView(dgvSegmentIdx);
            UIHelper.FixGridView(dgvMatching);

            dgvSegmentIdx.AddColumn("Chromosome", "Chromosome");
            dgvSegmentIdx.AddColumn("StartPosition", "Start Position");
            dgvSegmentIdx.AddColumn("EndPosition", "End Position");
            dgvSegmentIdx.AddColumn("SegmentLength_cm", "Segment Length (cM)", "#0.00");
            dgvSegmentIdx.AddColumn("SNPCount", "SNP Count");

            dgvMatching.AddColumn("RSID", "RSID");
            dgvMatching.AddColumn("Chromosome", "Chromosome");
            dgvMatching.AddColumn("Position", "Position");
            dgvMatching.AddColumn("Genotype", "Genotype");
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
            _host.SetStatus("Calculating ROH ...");
            this.Text = $"Runs of Homozygosity - {kit} ({GKSqlFuncs.GetKitName(kit)})";
            dgvMatching.DataSource = null;

            Task.Factory.StartNew(() => {
                roh_results = GKGenFuncs.ROH(kit, false);

                this.Invoke(new MethodInvoker(delegate {
                    UpdateView();
                }));
            });
        }

        private void ROHFrm_Load(object sender, EventArgs e)
        {
            ReloadData();
        }

        private void UpdateView()
        {
            dgvSegmentIdx.DataSource = roh_results;

            var segmentStats = SegmentStats.CalculateSegmentStats(roh_results);
            lblTotalSegments.Text = $"{segmentStats.Total:#0.00} cM";
            lblTotalXSegments.Text = $"{segmentStats.XTotal:#0.00} cM";
            lblLongestSegment.Text = $"{segmentStats.Longest:#0.00} cM";
            lblLongestXSegment.Text = $"{segmentStats.XLongest:#0.00} cM";
            lblMRCA.Text = segmentStats.GetMRCAText(true);

            _host.SetStatus("Done.");
        }

        private void dgvSegmentIdx_SelectionChanged(object sender, EventArgs e)
        {
            var selRow = dgvSegmentIdx.GetSelectedObj<ROHSegment>();
            if (selRow != null)
                dgvMatching.DataSource = selRow.Rows;
        }

        private void dgvMatching_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int index = dgvSegmentIdx.CurrentRow.Index;
            var segRow = roh_results[index].Rows;
            SNP row = segRow[e.RowIndex];

            if (row.Genotype.A1 == '-')
                e.CellStyle.BackColor = Color.LightGray;
            else if (row.Genotype.A1 == '0')
                e.CellStyle.BackColor = Color.OrangeRed;
        }
    }
}

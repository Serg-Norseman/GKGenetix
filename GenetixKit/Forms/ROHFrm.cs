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
using GenetixKit.Core;
using GenetixKit.Core.Model;

namespace GenetixKit.Forms
{
    public partial class ROHFrm : Form
    {
        private readonly string kit = null;
        private IList<ROHSegment> roh_results;

        public ROHFrm(string kit)
        {
            InitializeComponent();
            this.kit = kit;

            GKUIFuncs.FixGridView(dgvSegmentIdx);
            GKUIFuncs.FixGridView(dgvMatching);

            dgvSegmentIdx.AddColumn("Chromosome", "Chromosome");
            dgvSegmentIdx.AddColumn("StartPosition", "Start Position");
            dgvSegmentIdx.AddColumn("EndPosition", "End Position");
            dgvSegmentIdx.AddColumn("SegmentLength_cm", "Segment Length (cM)");
            dgvSegmentIdx.AddColumn("SNPCount", "SNP Count");

            dgvMatching.AddColumn("RSID", "RSID");
            dgvMatching.AddColumn("Chromosome", "Chromosome");
            dgvMatching.AddColumn("Position", "Position");
            dgvMatching.AddColumn("Genotype", "Genotype");
        }

        private void ROHFrm_Load(object sender, EventArgs e)
        {
            Program.KitInstance.SetStatus("Calculating ROH ...");
            this.Text = $"Runs of Homozygosity - {kit} ({GKSqlFuncs.GetKitName(kit)})";

            Task.Factory.StartNew(() => {
                roh_results = GKGenFuncs.ROH(kit);

                this.Invoke(new MethodInvoker(delegate {
                    UpdateView();
                }));
            });
        }

        private void UpdateView()
        {
            dgvSegmentIdx.DataSource = roh_results;

            var segmentStats = SegmentStats.CalculateSegmentStats(roh_results);
            lblTotalSegments.Text = segmentStats.Total.ToString() + " cM";
            lblTotalXSegments.Text = segmentStats.XTotal.ToString() + " cM";
            lblLongestSegment.Text = segmentStats.Longest.ToString() + " cM";
            lblLongestXSegment.Text = segmentStats.XLongest.ToString() + " cM";
            lblMRCA.Text = segmentStats.GetMRCAText(true);

            Program.KitInstance.SetStatus("Done.");
        }

        private void dgvSegmentIdx_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSegmentIdx.CurrentRow == null)
                return;

            int index = dgvSegmentIdx.CurrentRow.Index;
            dgvMatching.DataSource = roh_results[index].Rows;
        }

        private void dgvMatching_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int index = dgvSegmentIdx.CurrentRow.Index;
            var segRow = roh_results[index].Rows;
            SingleSNP row = segRow[e.RowIndex];

            if (row.Genotype == "-")
                e.CellStyle.BackColor = Color.LightGray;
            else if (row.Genotype == "")
                e.CellStyle.BackColor = Color.OrangeRed;
        }
    }
}

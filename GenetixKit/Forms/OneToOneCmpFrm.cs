﻿/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using GenetixKit.Core;
using GenetixKit.Core.Model;

namespace GenetixKit.Forms
{
    public partial class OneToOneCmpFrm : Form
    {
        private readonly string kit1 = null;
        private readonly string kit2 = null;
        private bool phased = false;
        private IList<CmpSegment> segmentsRes;

        public OneToOneCmpFrm(string kit1, string kit2)
        {
            InitializeComponent();

            GKUIFuncs.FixGridView(dgvSegmentIdx);
            dgvSegmentIdx.AddColumn("Chromosome", "Chromosome");
            dgvSegmentIdx.AddColumn("StartPosition", "Start Position");
            dgvSegmentIdx.AddColumn("EndPosition", "End Position");
            dgvSegmentIdx.AddColumn("SegmentLength_cm", "Segment Length (cM)", "#0.00");
            dgvSegmentIdx.AddColumn("SNPCount", "SNP Count");

            this.kit1 = kit1;
            this.kit2 = kit2;
            Program.KitInstance.SetStatus($"Comparing kits {kit1} and {kit2} ...");

            Task.Factory.StartNew(() => {
                phased = GKSqlFuncs.IsPhased(kit1) || GKSqlFuncs.IsPhased(kit2);
                segmentsRes = GKGenFuncs.CompareOneToOne(kit1, kit2, null, false, false);

                this.Invoke(new MethodInvoker(delegate {
                    UpdateView();
                }));
            });
        }

        private void UpdateView()
        {
            dgvSegmentIdx.DataSource = segmentsRes;

            var segmentStats = SegmentStats.CalculateSegmentStats(segmentsRes);
            lblTotalSegments.Text = segmentStats.Total.ToString("#0.00") + " cM";
            lblTotalXSegments.Text = segmentStats.XTotal.ToString("#0.00") + " cM";
            lblLongestSegment.Text = segmentStats.Longest.ToString("#0.00") + " cM";
            lblLongestXSegment.Text = segmentStats.XLongest.ToString("#0.00") + " cM";
            lblMRCA.Text = segmentStats.GetMRCAText(false);

            Program.KitInstance.SetStatus("Done.");
        }

        private void dgvSegmentIdx_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var selRow = dgvSegmentIdx.GetSelectedObj<CmpSegment>();
            if (phased && selRow != null) {
                string chr = selRow.Chromosome;
                string startPos = selRow.StartPosition.ToString();
                string endPos = selRow.EndPosition.ToString();

                Program.KitInstance.ShowPhasedSegmentVisualizer(kit1, kit2, chr, startPos, endPos);
            }
        }
    }
}

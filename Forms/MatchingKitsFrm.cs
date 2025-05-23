/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using GenetixKit.Core;
using GenetixKit.Core.Model;

namespace GenetixKit.Forms
{
    public partial class MatchingKitsFrm : Form
    {
        private readonly string kit = null;
        private string phasedKit = null;
        private string unphasedKit = null;
        private bool phased = false;
        private IList<CmpSegment> tblSegments = null;
        private IList<CmpSegmentRow> tblAlleles = null;

        public MatchingKitsFrm(string kit)
        {
            InitializeComponent();

            GKUIFuncs.FixGridView(dgvMatches);
            dgvMatches.AddColumn("Kit", "Kit No");
            dgvMatches.AddColumn("Name", "Name");
            dgvMatches.AddColumn("Longest", "Autosomal Longest", "N2");
            dgvMatches.AddColumn("Total", "Autosomal Total", "N2");
            dgvMatches.AddColumn("XLongest", "X Longest", "N2");
            dgvMatches.AddColumn("XTotal", "X Total", "N2");
            dgvMatches.AddColumn("Mrca", "MRCA");

            GKUIFuncs.FixGridView(dgvSegments);
            dgvSegments.AddColumn("Chromosome", "Chromosome");
            dgvSegments.AddColumn("StartPosition", "Start Position");
            dgvSegments.AddColumn("EndPosition", "End Position");
            dgvSegments.AddColumn("SegmentLength_cm", "Segment Length (cM)", "N2");
            dgvSegments.AddColumn("SNPCount", "SNP Count");

            GKUIFuncs.FixGridView(dgvAlleles);
            dgvAlleles.AddColumn("RSID", "RSID");
            dgvAlleles.AddColumn("Position", "Position");
            dgvAlleles.AddColumn("Kit1Genotype", "");
            dgvAlleles.AddColumn("Kit2Genotype", "");
            dgvAlleles.AddColumn("Match", "Match");

            this.kit = kit;
        }

        private void MatchingKitsFrm_Load(object sender, EventArgs e)
        {
            btnKit.Text = kit;
            lblName.Text = GKSqlFuncs.GetKitName(kit);

            var dt = GKSqlFuncs.GetMatchingKits(kit);
            dgvMatches.DataSource = dt;
        }

        private void dgvMatches_SelectionChanged(object sender, EventArgs e)
        {
            var selMatchRow = dgvMatches.GetSelectedObj<MatchingKit>();
            if (selMatchRow == null) return;

            dgvAlleles.Columns[2].HeaderText = $"{kit} ({lblName.Text})";
            dgvAlleles.Columns[3].HeaderText = $"{selMatchRow.Kit} ({selMatchRow.Name})";

            BackgroundWorker bWorker = new BackgroundWorker();
            bWorker.DoWork += bWorker_DoWork;
            bWorker.RunWorkerCompleted += bWorker_RunWorkerCompleted;
            bWorker.RunWorkerAsync(selMatchRow);
        }

        private void bWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var o = (MatchingKit)e.Argument;

            tblSegments = GKSqlFuncs.GetAutosomalCmp(o.CmpId);

            if (GKSqlFuncs.IsPhased(kit)) {
                phasedKit = kit;
                unphasedKit = o.Kit;
                phased = true;
            } else if (GKSqlFuncs.IsPhased(o.Kit)) {
                phasedKit = o.Kit;
                unphasedKit = kit;
                phased = true;
            } else
                phased = false;

            e.Result = o;
        }

        private void bWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (tblSegments == null) return;

            var o = (MatchingKit)e.Result;
            lblSegLabel.Text = $"List of matching segments for kit {o.Kit} ({o.Name})";

            dgvSegments.DataSource = tblSegments;
            tblSegments = null;
        }

        private void dgvSegments_SelectionChanged(object sender, EventArgs e)
        {
            var segment = dgvSegments.GetSelectedObj<CmpSegment>();
            if (segment == null) return;

            BackgroundWorker bWorker2 = new BackgroundWorker();
            bWorker2.DoWork += bWorker2_DoWork;
            bWorker2.RunWorkerCompleted += bWorker2_RunWorkerCompleted;
            bWorker2.RunWorkerAsync(segment);
        }

        private void bWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            var seg = (CmpSegment)e.Argument;
            tblAlleles = GKSqlFuncs.GetCmpSeg(seg.SegmentId);
        }

        private void bWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dgvAlleles.DataSource = tblAlleles;
        }

        private void dgvAlleles_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var row = tblAlleles[e.RowIndex];
            var cellVal = row.Match.ToString();

            if (cellVal == "-")
                e.CellStyle.BackColor = Color.LightGray;
            else if (cellVal == "")
                e.CellStyle.BackColor = Color.OrangeRed;
        }

        private void btnKit_Click(object sender, EventArgs e)
        {
            Program.KitInstance.SelectOper(UIOperation.SELECT_ONE_TO_MANY);
        }

        private void dgvSegments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!phased) return;

            var segment = dgvSegments.GetSelectedObj<CmpSegment>();
            if (segment == null) return;

            string chr = segment.Chromosome.ToString();
            string startPos = segment.StartPosition.ToString();
            string endPos = segment.EndPosition.ToString();

            Program.KitInstance.ShowPhasedSegmentVisualizer(phasedKit, unphasedKit, chr, startPos, endPos);
        }
    }
}

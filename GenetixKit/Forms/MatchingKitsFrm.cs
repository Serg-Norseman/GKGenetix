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
using GKGenetix.Core.Model;

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

            Task.Factory.StartNew((object obj) => {
                var o = (MatchingKit)obj;

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

                this.Invoke(new MethodInvoker(delegate {
                    if (tblSegments == null) return;

                    lblSegLabel.Text = $"List of matching segments for kit {o.Kit} ({o.Name})";

                    dgvSegments.DataSource = tblSegments;
                    tblSegments = null;
                }));
            }, selMatchRow);
        }

        private void dgvSegments_SelectionChanged(object sender, EventArgs e)
        {
            var segment = dgvSegments.GetSelectedObj<CmpSegment>();
            if (segment == null) return;

            Task.Factory.StartNew((object obj) => {
                var seg = (CmpSegment)obj;
                tblAlleles = GKSqlFuncs.GetCmpSeg(seg.SegmentId);

                this.Invoke(new MethodInvoker(delegate {
                    dgvAlleles.DataSource = tblAlleles;
                }));
            }, segment);
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

            Program.KitInstance.ShowPhasedSegmentVisualizer(phasedKit, unphasedKit, segment.Chromosome, segment.StartPosition, segment.EndPosition);
        }
    }
}

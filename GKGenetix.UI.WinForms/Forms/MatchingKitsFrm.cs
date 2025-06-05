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
    public partial class MatchingKitsFrm : GKWidget
    {
        private string kit = null;
        private string phasedKit = null;
        private string unphasedKit = null;
        private bool phased = false;
        private IList<CmpSegment> tblSegments = null;
        private IList<SNPMatch> tblAlleles = null;


        public static bool CanBeUsed(IList<TestRecord> selectedKits)
        {
            return (selectedKits != null && selectedKits.Count == 1 && !selectedKits[0].Disabled);
        }


        public MatchingKitsFrm(IKitHost host, IList<TestRecord> selectedKits) : this(host, selectedKits[0].KitNo)
        {
        }

        public MatchingKitsFrm(IKitHost host, string kit) : base(host)
        {
            InitializeComponent();

            UIHelper.FixGridView(dgvMatches);
            dgvMatches.AddColumn("Kit", "Kit No");
            dgvMatches.AddColumn("Name", "Name");
            dgvMatches.AddColumn("Longest", "Autosomal Longest", "N2");
            dgvMatches.AddColumn("Total", "Autosomal Total", "N2");
            dgvMatches.AddColumn("XLongest", "X Longest", "N2");
            dgvMatches.AddColumn("XTotal", "X Total", "N2");
            dgvMatches.AddColumn("Mrca", "MRCA");

            UIHelper.FixGridView(dgvSegments);
            dgvSegments.AddColumn("Chromosome", "Chromosome");
            dgvSegments.AddColumn("StartPosition", "Start Position");
            dgvSegments.AddColumn("EndPosition", "End Position");
            dgvSegments.AddColumn("SegmentLength_cm", "Segment Length (cM)", "N2");
            dgvSegments.AddColumn("SNPCount", "SNP Count");

            UIHelper.FixGridView(dgvAlleles);
            dgvAlleles.AddColumn("RSID", "RSID");
            dgvAlleles.AddColumn("Position", "Position");
            dgvAlleles.AddColumn("Genotype1", "");
            dgvAlleles.AddColumn("Genotype2", "");
            dgvAlleles.AddColumn("Match", "Match");

            this.kit = kit;
        }

        public override void SetKit(IList<TestRecord> selectedKits)
        {
            if (CanBeUsed(selectedKits)) {
                this.kit = selectedKits[0].KitNo;
                UpdateView();
            }
        }

        private void MatchingKitsFrm_Load(object sender, EventArgs e)
        {
            UpdateView();
        }

        private void UpdateView()
        {
            this.Text = $"Matching Kits : {kit} ({GKSqlFuncs.GetKitName(kit)})";

            var dt = GKSqlFuncs.GetMatchingKits(kit);
            dgvMatches.DataSource = dt;
            dgvSegments.DataSource = null;
            dgvAlleles.DataSource = null;
        }

        private void dgvMatches_SelectionChanged(object sender, EventArgs e)
        {
            var selMatchRow = dgvMatches.GetSelectedObj<MatchingKit>();
            if (selMatchRow == null) return;

            dgvAlleles.Columns[2].HeaderText = $"{kit} ({GKSqlFuncs.GetKitName(kit)})";
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

        private void dgvSegments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var segment = dgvSegments.GetSelectedObj<CmpSegment>();
            if (segment != null && phased) {
                _host.ShowPhasedSegmentVisualizer(phasedKit, unphasedKit, segment.Chromosome, segment.StartPosition, segment.EndPosition);
            }
        }
    }
}

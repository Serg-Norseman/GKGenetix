/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using GenetixKit.Core;

namespace GenetixKit.Forms
{
    public partial class MatchingKitsFrm : Form
    {
        private readonly string kit = null;
        private string phasedKit = null;
        private string unphasedKit = null;
        private bool phased = false;
        private DataTable tblSegments = null;
        private DataTable tblAlleles = null;

        public MatchingKitsFrm(string kit)
        {
            InitializeComponent();

            GKUIFuncs.FixGridView(dgvMatches);
            dgvMatches.AddColumn("kit", "Kit No");
            dgvMatches.AddColumn("name", "Name");
            dgvMatches.AddColumn("at_longest", "Autosomal Longest", "N2");
            dgvMatches.AddColumn("at_total", "Autosomal Total", "N2");
            dgvMatches.AddColumn("x_longest", "X Longest", "N2");
            dgvMatches.AddColumn("x_total", "X Total", "N2");
            dgvMatches.AddColumn("mrca", "MRCA");

            GKUIFuncs.FixGridView(dgvSegments);
            dgvSegments.AddColumn("chromosome", "Chromosome");
            dgvSegments.AddColumn("start_position", "Start Position");
            dgvSegments.AddColumn("end_position", "End Position");
            dgvSegments.AddColumn("segment_length_cm", "Segment Length (cM)", "N2");
            dgvSegments.AddColumn("snp_count", "SNP Count");
            dgvSegments.AddColumn("segment_id", "segment_id", "", false);

            GKUIFuncs.FixGridView(dgvAlleles);
            dgvAlleles.AddColumn("rsid", "RSID");
            dgvAlleles.AddColumn("position", "Position");
            dgvAlleles.AddColumn("kit1_genotype", "");
            dgvAlleles.AddColumn("kit2_genotype", "");
            dgvAlleles.AddColumn("match", "Match");

            this.kit = kit;
        }

        private void MatchingKitsFrm_Load(object sender, EventArgs e)
        {
            lblKit.Text = kit;
            lblName.Text = GKSqlFuncs.GetKitName(kit);

            DataTable dt = GKSqlFuncs.QueryTable("SELECT cmp_id, kit, name, at_longest, at_total, x_longest, x_total, mrca FROM (SELECT a.cmp_id,a.kit1'kit',b.name,a.at_longest,a.at_total,a.x_longest,a.x_total,a.mrca FROM cmp_status a,kit_master b WHERE a.at_total!=0 AND a.kit1!='" + kit + "' AND a.kit2='" + kit + "' AND a.status_autosomal=1 AND b.kit_no=a.kit1 AND b.disabled=0 UNION SELECT a.cmp_id,a.kit2'kit',b.name,a.at_longest,a.at_total,a.x_longest,a.x_total,a.mrca FROM cmp_status a,kit_master b WHERE a.at_total!=0 AND a.kit2!='" + kit + "' AND a.kit1='" + kit + "' AND a.status_autosomal=1 AND b.kit_no=a.kit2 AND b.disabled=0) ORDER BY at_longest DESC,at_total DESC");
            dgvMatches.DataSource = dt;
        }

        private void dgvMatches_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMatches.SelectedRows.Count > 0) {
                var selRowCells = dgvMatches.SelectedRows[0].Cells;

                string cmp_id = selRowCells[0].Value.ToString();
                string kit2 = selRowCells[1].Value.ToString();
                string name2 = selRowCells[2].Value.ToString();

                BackgroundWorker bWorker = new BackgroundWorker();
                bWorker.DoWork += bWorker_DoWork;
                bWorker.RunWorkerCompleted += bWorker_RunWorkerCompleted;
                bWorker.RunWorkerAsync(new string[] { cmp_id, kit2, name2 });
            }
        }

        private void bWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] o = (string[])e.Argument;
            string cmp_id = o[0];
            string kit2 = o[1];
            string name2 = o[2];
            tblSegments = GKSqlFuncs.QueryTable("select chromosome, start_position, end_position, segment_length_cm, snp_count, segment_id from cmp_autosomal where cmp_id='" + cmp_id + "'");

            if (GKSqlFuncs.IsPhased(kit)) {
                phasedKit = kit;
                unphasedKit = kit2;
                phased = true;
            } else if (GKSqlFuncs.IsPhased(kit2)) {
                phasedKit = kit2;
                unphasedKit = kit;
                phased = true;
            } else
                phased = false;
            e.Result = new string[] { kit2, name2 };
        }

        private void bWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (tblSegments != null) {
                string[] o = (string[])e.Result;
                lblSegLabel.Text = "List of matching segments for kit " + o[0] + " (" + o[1] + ")";

                dgvSegments.DataSource = tblSegments;
                tblSegments = null;
            }
        }

        private void dgvSegments_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSegments.SelectedRows.Count > 0) {
                BackgroundWorker bWorker2 = new BackgroundWorker();
                bWorker2.DoWork += bWorker2_DoWork;
                bWorker2.RunWorkerCompleted += bWorker2_RunWorkerCompleted;

                string segmentId = dgvSegments.SelectedRows[0].Cells[5].Value.ToString();

                DataGridViewCellCollection selRowCells = dgvMatches.SelectedRows[0].Cells;
                dgvAlleles.Columns[2].HeaderText = kit + " (" + lblName.Text + ")";
                dgvAlleles.Columns[3].HeaderText = selRowCells[0].Value.ToString() + " (" + selRowCells[1].Value.ToString() + ")";

                bWorker2.RunWorkerAsync(segmentId);
            }
        }

        private void bWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            string sId = (string)e.Argument;
            tblAlleles = GKSqlFuncs.QueryTable("select rsid, position, kit1_genotype, kit2_genotype, match from cmp_mrca where segment_id='" + sId + "'");
        }

        private void bWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (tblAlleles != null) {
                dgvAlleles.DataSource = tblAlleles;
            }
        }

        private void dgvAlleles_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var row = tblAlleles.Rows[e.RowIndex];
            var cellVal = row["match"].ToString();

            if (cellVal == "-")
                e.CellStyle.BackColor = Color.LightGray;
            else if (cellVal == "")
                e.CellStyle.BackColor = Color.OrangeRed;
        }

        private void lblKit_Click(object sender, EventArgs e)
        {
            Program.KitInstance.SelectOper(UIOperation.SELECT_ONE_TO_MANY);
        }

        private void dgvSegments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (phased) {
                DataGridViewRow selRow = dgvSegments.SelectedRows[0];

                string chr = selRow.Cells[0].Value.ToString();
                string startPos = selRow.Cells[1].Value.ToString();
                string endPos = selRow.Cells[2].Value.ToString();

                Program.KitInstance.ShowPhasedSegmentVisualizer(phasedKit, unphasedKit, chr, startPos, endPos);
            }
        }
    }
}

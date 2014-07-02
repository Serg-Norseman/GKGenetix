using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Genetic_Genealogy_Kit
{
    public partial class MatchingKitsFrm : Form
    {
        string kit = null;
        string phased_kit = null;
        string unphased_kit = null;
        bool phased = false;
        DataTable segment_dt = null;
        DataTable dt_alleles = null;
        
        public MatchingKitsFrm(string kit)
        {
            InitializeComponent();
            this.kit = kit;
        }

        private void MatchingKitsFrm_Load(object sender, EventArgs e)
        {
            lblKit.Text = kit;
            lblName.Text = GGKUtilLib.queryDatabase("kit_master", new string[] { "name" },"WHERE kit_no='"+kit+"'").Rows[0].ItemArray[0].ToString();
            DataTable dt = GGKUtilLib.QueryDB("SELECT cmp_id,kit'Kit No',name'Name',at_longest'Autosomal Longest',at_total'Autosomal Total',x_longest'X Longest',x_total'X Total',mrca'MRCA' FROM (SELECT a.cmp_id,a.kit1'kit',b.name,a.at_longest,a.at_total,a.x_longest,a.x_total,a.mrca FROM cmp_status a,kit_master b WHERE a.at_total!=0 AND a.kit1!='" + kit + "' AND a.kit2='" + kit + "' AND a.status_autosomal=1 AND b.kit_no=a.kit1 AND b.disabled=0 UNION SELECT a.cmp_id,a.kit2'kit',b.name,a.at_longest,a.at_total,a.x_longest,a.x_total,a.mrca FROM cmp_status a,kit_master b WHERE a.at_total!=0 AND a.kit2!='" + kit + "' AND a.kit1='" + kit + "' AND a.status_autosomal=1 AND b.kit_no=a.kit2 AND b.disabled=0) ORDER BY at_longest DESC,at_total DESC");
            dgvMatches.Columns.Clear();
            dgvMatches.DataSource = dt;
            dgvMatches.Columns[0].Visible = false;

            dgvMatches.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvMatches.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvMatches.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvMatches.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvMatches.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvMatches.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvMatches.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            DataGridViewCellStyle style= new DataGridViewCellStyle();
            style.Format="N2";

            dgvMatches.Columns[3].DefaultCellStyle = style;
            dgvMatches.Columns[4].DefaultCellStyle = style;
            dgvMatches.Columns[5].DefaultCellStyle = style;
            dgvMatches.Columns[6].DefaultCellStyle = style;
           
        }

        private void dgvMatches_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMatches.SelectedRows.Count > 0)
            {
                string cmp_id = dgvMatches.SelectedRows[0].Cells[0].Value.ToString();
                string kit2 = dgvMatches.SelectedRows[0].Cells[1].Value.ToString();
                string name2 = dgvMatches.SelectedRows[0].Cells[2].Value.ToString();   
                BackgroundWorker bWorker = new BackgroundWorker();
                bWorker.DoWork += bWorker_DoWork;
                bWorker.RunWorkerCompleted += bWorker_RunWorkerCompleted;
                bWorker.RunWorkerAsync(new string[] { cmp_id, kit2 ,name2});
            }
        }

        void bWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] o = (string[])e.Argument;
            string cmp_id = o[0];
            string kit2 = o[1];
            string name2 = o[2];
            segment_dt = GGKUtilLib.QueryDB("select chromosome'Chromosome',start_position'Start Position',end_position'End Position',segment_length_cm'Segment Length (cM)',snp_count'SNP Count',segment_id from cmp_autosomal where cmp_id='" + cmp_id + "'");

            if (GGKUtilLib.isPhased(kit))
            {
                phased_kit = kit;
                unphased_kit = kit2;
                phased = true;
            }
            else if (GGKUtilLib.isPhased(kit2))
            {
                phased_kit = kit2;
                unphased_kit = kit;
                phased = true;
            }
            else
                phased = false;
            e.Result = new string[]{kit2,name2};
        }

        void bWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (segment_dt != null)
            {
                string[] o = (string[])e.Result;
                lblSegLabel.Text = "List of matching segments for kit " + o[0] + " (" + o[1] + ")";                
                dgvSegments.Columns.Clear();         
                dgvSegments.DataSource = segment_dt;
                DataGridViewCellStyle style = new DataGridViewCellStyle();
                style.Format = "N2";

                dgvSegments.Columns[3].DefaultCellStyle = style;

                dgvSegments.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvSegments.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvSegments.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvSegments.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvSegments.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dgvSegments.Columns[5].Visible = false;                
            }
            
        }

        private void dgvSegments_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSegments.SelectedRows.Count > 0)
            {
                BackgroundWorker bWorker2 = new BackgroundWorker();
                bWorker2.DoWork += bWorker2_DoWork;
                bWorker2.RunWorkerCompleted += bWorker2_RunWorkerCompleted;
                string segment_id = dgvSegments.SelectedRows[0].Cells[5].Value.ToString();

                //lblSegLabel.Text = "List of matching segments for kit " + dgvMatches.SelectedRows[0].Cells[1].Value.ToString() + " (" + dgvMatches.SelectedRows[0].Cells[2].Value.ToString() + ")";
                dgvAlleles.Columns.Clear();
                bWorker2.RunWorkerAsync(new string[]{kit,lblName.Text,dgvMatches.SelectedRows[0].Cells[1].Value.ToString(),dgvMatches.SelectedRows[0].Cells[2].Value.ToString(),segment_id});
            }
        }

        void bWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] o= (object[])e.Argument;
            dt_alleles = GGKUtilLib.QueryDB("select rsid'RSID',position'Position',kit1_genotype'" + o[0] + " (" + o[1] + ")',kit2_genotype'" + o[2] + " (" + o[3] + ")',match'Match' from cmp_mrca where segment_id='" + o[4] + "'");
        }

        void bWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (dt_alleles != null)
            {
                dgvAlleles.Columns.Clear();
                dgvAlleles.DataSource = dt_alleles;

                dgvAlleles.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvAlleles.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvAlleles.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvAlleles.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvAlleles.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                foreach (DataGridViewRow row in dgvAlleles.Rows)
                {
                    if (row.Cells[4].Value.ToString() == "-")
                        row.DefaultCellStyle.BackColor = Color.LightGray;
                    else if (row.Cells[4].Value.ToString() == "")
                        row.DefaultCellStyle.BackColor = Color.OrangeRed;
                }
            }
        }

        private void lblKit_Click(object sender, EventArgs e)
        {
            SelectKitFrm open = new SelectKitFrm(SelectKitFrm.SELECT_ONE_TO_MANY);
            open.ShowDialog(this);
        }

        private void dgvSegments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (phased)
            {
                string chr = dgvSegments.SelectedRows[0].Cells[0].Value.ToString();
                string start_pos = dgvSegments.SelectedRows[0].Cells[1].Value.ToString();
                string end_pos = dgvSegments.SelectedRows[0].Cells[2].Value.ToString();
                PhasedSegmentVisualizerFrm frm = new PhasedSegmentVisualizerFrm(phased_kit, unphased_kit, chr, start_pos, end_pos);
                frm.ShowDialog(Program.GGKitFrmMainInst);
                frm.Dispose();
            }
        }
    }
}

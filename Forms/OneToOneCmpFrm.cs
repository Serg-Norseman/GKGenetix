using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Genetic_Genealogy_Kit
{
    public partial class OneToOneCmpFrm : Form
    {
        string kit1 = null;
        string kit2 = null;
        string name1 = null;
        string name2 = null;
        bool phased = false;
        string phased_kit = null;
        string unphased_kit = null;

        DataTable segment_idx = new DataTable();
        List<DataTable> segments = new List<DataTable>();
        int p_idx = -1;
        List<DataGridViewRow> timer_jobs = new List<DataGridViewRow>();

        public OneToOneCmpFrm(string kit1,string kit2)
        {
            InitializeComponent();
            //
            this.kit1 = kit1;
            this.kit2 = kit2;
            this.name1 = GGKUtilLib.getKitName(kit1);
            this.name2 = GGKUtilLib.getKitName(kit2);
            dgvMatching.Columns[3].HeaderText = name1;
            dgvMatching.Columns[4].HeaderText = name2;
            GGKUtilLib.setStatus("Comparing kits "+kit1+" and "+kit2+" ...");
            bwCompare.RunWorkerAsync();
        }

        private void bwCompare_DoWork(object sender, DoWorkEventArgs e)
        {            
            if (GGKUtilLib.isPhased(kit1))
            {
                phased_kit = kit1;
                unphased_kit = kit2;
                phased = true;
            }
            else if (GGKUtilLib.isPhased(kit2))
            {
                phased_kit = kit2;
                unphased_kit = kit1;
                phased = true;
            }
            else
                phased = false;
            object[] cmp_results = GGKUtilLib.compareOneToOne(kit1, kit2);
            segment_idx = (DataTable)cmp_results[0];
            segments = (List<DataTable>)cmp_results[1];
        }

        private void bwCompare_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {            
            double total = 0;
            double longest = 0;
            double x_total = 0;
            double x_longest = 0;
            int mrca = 0;
            object[] obj=null;
            double seg_len=0;
            foreach(DataRow row in segment_idx.Rows)
            {
                obj = row.ItemArray;
                seg_len = double.Parse(obj[3].ToString());
                if(obj[0].ToString()=="X")
                {
                    x_total += seg_len;
                    if (x_longest < seg_len)
                        x_longest = seg_len;
                }
                else
                {
                    total += seg_len;
                    if (longest < seg_len)
                        longest = seg_len;
                }
            }
            ///
            lblTotalSegments.Text = total.ToString() + " cM";
            lblTotalXSegments.Text = x_total.ToString() + " cM";
            lblLongestSegment.Text = longest.ToString() + " cM";
            lblLongestXSegment.Text = x_longest.ToString() + " cM";

            double shared = 0;
            double range_begin = 0;
            double range_end = 0;
            for (int gen = 0; gen < 10;gen++ )
            {
                shared=3600/Math.Pow(2,gen);
                range_begin = shared - shared / 4;
                range_end = shared + shared / 4;
                if (total < range_end && total > range_begin)
                    mrca = gen + 1;
            }
            if (mrca > 0)
            {
                if (mrca == 1)
                    lblMRCA.Text = mrca.ToString() + " generation back";
                else
                    lblMRCA.Text = mrca.ToString() + " generations back";
            }
            else
            {
                if(total>0 || x_total>0)
                    lblMRCA.Text = "Distantly related.";
                else
                    lblMRCA.Text = "Not related";
            }

                dgvSegmentIdx.Rows.Clear();
                dgvSegmentIdx.Columns.Clear();            
                dgvSegmentIdx.DataSource = segment_idx;
                if (dgvSegmentIdx.Columns.Count > 0)
                {
                    dgvSegmentIdx.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvSegmentIdx.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvSegmentIdx.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvSegmentIdx.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvSegmentIdx.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvSegmentIdx.Columns[0].ReadOnly = true;
                    dgvSegmentIdx.Columns[1].ReadOnly = true;
                    dgvSegmentIdx.Columns[2].ReadOnly = true;
                    dgvSegmentIdx.Columns[3].ReadOnly = true;
                    dgvSegmentIdx.Columns[4].ReadOnly = true;
                    dgvSegmentIdx.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgvSegmentIdx.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgvSegmentIdx.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgvSegmentIdx.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgvSegmentIdx.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                    //
                    if (dgvMatching.Columns.Count > 0)
                    {
                        foreach (DataGridViewRow row in dgvMatching.Rows)
                        {
                            if (row.Cells[5].Value.ToString() == "-")
                                row.DefaultCellStyle.BackColor = Color.LightGray;
                            else if (row.Cells[5].Value.ToString() == "")
                                row.DefaultCellStyle.BackColor = Color.OrangeRed;
                        }
                    }
                }
            GGKUtilLib.setStatus("Done.");
        }

        private void dgvSegmentIdx_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSegmentIdx.CurrentRow != null)
            {
                if (p_idx != dgvSegmentIdx.CurrentRow.Index && segments.Count>0)
                {
                    dgvMatching.Columns.Clear();
                    dgvMatching.DataSource = segments[dgvSegmentIdx.CurrentRow.Index];

                    if (dgvMatching.Columns.Count > 0)
                    {
                        dgvMatching.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dgvMatching.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dgvMatching.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dgvMatching.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dgvMatching.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dgvMatching.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                        dgvMatching.Columns[0].ReadOnly = true;
                        dgvMatching.Columns[1].ReadOnly = true;
                        dgvMatching.Columns[2].ReadOnly = true;
                        dgvMatching.Columns[3].ReadOnly = true;
                        dgvMatching.Columns[4].ReadOnly = true;
                        dgvMatching.Columns[5].ReadOnly = true;

                        dgvMatching.Columns[3].HeaderText = name1;
                        dgvMatching.Columns[4].HeaderText = name2;

                        foreach (DataGridViewRow row in dgvMatching.Rows)
                        {
                            if (row.Cells[5].Value.ToString() == "-")
                                row.DefaultCellStyle.BackColor = Color.LightGray;
                            else if (row.Cells[5].Value.ToString() == "")
                                row.DefaultCellStyle.BackColor = Color.OrangeRed;
                        }
                    }                    
                }
                p_idx = dgvSegmentIdx.CurrentRow.Index;
            }
            
        }

        private void dgvMatching_Sorted(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvMatching.Rows)
            {
                if (row.Cells[5].Value.ToString() == "-")
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                else if (row.Cells[5].Value.ToString() == "")
                    row.DefaultCellStyle.BackColor = Color.OrangeRed;
            }
        }

        private void OneToOneCmpFrm_Load(object sender, EventArgs e)
        {
            GGKUtilLib.enableSave();
        }

        public void  Save()
        {
            if(saveFileDlg.ShowDialog(this)==DialogResult.OK)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("RSID,CHROMOSOME,POSITION,RESULT\r\n");
                object[] data = null;
                foreach(DataTable dt in segments)                
                    foreach(DataRow row in dt.Rows)
                    {
                        data = row.ItemArray;
                        if (data[5].ToString() == "--")
                            continue;
                        if (data[5].ToString().Length==2)
                            sb.Append(("\""+data[0] + "\",\"" + data[1] + "\",\"" + data[2] + "\",\"" + data[5] + "\"\r\n"));                        
                        else if (data[5].ToString().Length==1)
                            sb.Append(("\""+data[0] + "\",\"" + data[1] + "\",\"" + data[2] + "\",\"" + data[5] + data[5]+"\"\r\n"));        
                    }
                File.WriteAllText(saveFileDlg.FileName, sb.ToString());
                sb.Length = 0;
                GGKUtilLib.setStatus("CA Profile saved.");
            }
        }

        private void OneToOneCmpFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            GGKUtilLib.disableSave();
        }

        private void dgvSegmentIdx_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (phased)
            {
                string chr = dgvSegmentIdx.SelectedRows[0].Cells[0].Value.ToString();
                string start_pos = dgvSegmentIdx.SelectedRows[0].Cells[1].Value.ToString();
                string end_pos = dgvSegmentIdx.SelectedRows[0].Cells[2].Value.ToString();
                PhasedSegmentVisualizerFrm frm = new PhasedSegmentVisualizerFrm(kit1, kit2, chr, start_pos, end_pos);
                frm.ShowDialog(Program.GGKitFrmMainInst);
                frm.Dispose();
            }
        }
       
    }
}

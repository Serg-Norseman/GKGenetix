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
    public partial class ROHFrm : Form
    {
        string kit = null;
        DataTable segment_idx = new DataTable();
        List<DataTable> segments = new List<DataTable>();
        //bool data_exits_in_db = false;
        int p_idx = -1;

        public ROHFrm(string kit)
        {
            InitializeComponent();
            this.kit = kit;
        }

        private void ROHFrm_Load(object sender, EventArgs e)
        {
            GGKUtilLib.setStatus("Calculating ROH ...");
            this.Text = "Runs of Homozygosity - " + kit+" ("+GGKUtilLib.getKitName(kit)+")";
            bwROH.RunWorkerAsync(kit);
        }

        private void bwROH_DoWork(object sender, DoWorkEventArgs e)
        {
            string kit = e.Argument.ToString();
            object[] roh_results = GGKUtilLib.ROH(kit);
            segment_idx = (DataTable)roh_results[0];
            segments = (List<DataTable>)roh_results[1];
        }

        private void bwROH_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {            
            double total = 0;
            double longest = 0;
            double x_total = 0;
            double x_longest = 0;
            int mrca = 0;
            object[] obj = null;
            double seg_len = 0;
            foreach (DataRow row in segment_idx.Rows)
            {
                obj = row.ItemArray;
                seg_len = double.Parse(obj[3].ToString());
                if (obj[0].ToString() == "X")
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
            for (int gen = 0; gen < 10; gen++)
            {
                shared = 3600 / Math.Pow(2, gen);
                range_begin = shared - shared / 4;
                range_end = shared + shared / 4;
                if (total < range_end && total > range_begin)
                    mrca = gen + 1;
            }

            //adjusting mrca for RoH specific
            if (mrca > 0)
            {
                mrca = mrca - 1;

                if (mrca == 1)
                    lblMRCA.Text = mrca.ToString() + " generation back";
                else
                    lblMRCA.Text = mrca.ToString() + " generations back";
            }
            else
                lblMRCA.Text = "Not Related";


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

                if (dgvMatching.Columns.Count > 0)
                {
                    foreach (DataGridViewRow row in dgvMatching.Rows)
                    {
                        if (row.Cells[3].Value.ToString() == "-")
                            row.DefaultCellStyle.BackColor = Color.LightGray;
                        else if (row.Cells[3].Value.ToString() == "")
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
                if (p_idx != dgvSegmentIdx.CurrentRow.Index)
                {
                    dgvMatching.Columns.Clear();
                    dgvMatching.DataSource = segments[dgvSegmentIdx.CurrentRow.Index];

                    if (dgvMatching.Columns.Count > 0)
                    {
                        dgvMatching.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dgvMatching.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dgvMatching.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dgvMatching.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                        dgvMatching.Columns[0].ReadOnly = true;
                        dgvMatching.Columns[1].ReadOnly = true;
                        dgvMatching.Columns[2].ReadOnly = true;
                        dgvMatching.Columns[3].ReadOnly = true;

                        foreach (DataGridViewRow row in dgvMatching.Rows)
                        {
                            if (row.Cells[3].Value.ToString() == "-")
                                row.DefaultCellStyle.BackColor = Color.LightGray;
                            else if (row.Cells[3].Value.ToString() == "")
                                row.DefaultCellStyle.BackColor = Color.OrangeRed;
                        }
                    }
                }
                p_idx = dgvSegmentIdx.CurrentRow.Index;
            }
        }
    }
}

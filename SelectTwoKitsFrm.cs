using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Genetic_Genealogy_Kit
{
    public partial class SelectTwoKitsFrm : Form
    {
        string selected_kit_one = null;
        string selected_kit_two = null;

        int working = 0;
        int selected_operation = -1;
        string select_sql = null;

        public const int SELECT_ADMIXTURE = 0;

        public SelectTwoKitsFrm(int operation)
        {
            InitializeComponent();

            selected_operation = operation;
            string hide_ref = GGKSettings.getParameterValue("Admixture.ReferencePopulations.Hide");
            switch (selected_operation)
            {
                case SELECT_ADMIXTURE:
                    if (hide_ref == "1")
                        select_sql = @"SELECT kit_no 'Kit#',name 'Name' FROM kit_master WHERE disabled=0 AND reference=0 order by name ASC";
                    else
                        select_sql = @"SELECT kit_no 'Kit#',name 'Name' FROM kit_master WHERE disabled=0 order by name ASC";
                    break;
                default:
                    select_sql = @"SELECT kit_no 'Kit#',name 'Name' FROM kit_master WHERE disabled=0 order by name ASC";
                    break;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            SQLiteConnection cnn = GGKUtilLib.getDBConnection();
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            SQLiteCommand query = new SQLiteCommand(select_sql, cnn);
            SQLiteDataReader reader = query.ExecuteReader();
            
            DataTable dt1 = null;
            DataTable dt2 = null;

            dt1 = new DataTable();
            dt2 = new DataTable();
            dt1.Load(reader);
            reader = query.ExecuteReader();
            dt2.Load(reader);
            query.Dispose();
            cnn.Dispose();

            dataGridView1.DataSource = dt1;
            dataGridView2.DataSource = dt2;

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView2.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView2.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void SelectTwoKitsFrm_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            string kit1 = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            string kit2 = dataGridView2.SelectedRows[0].Cells[0].Value.ToString();

            if(kit1==kit2)
            {
                MessageBox.Show("Please select different kits to compare.","One-to-One Compare",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            // open mdi child
            switch (selected_operation)
            {
                case SELECT_ADMIXTURE:
                    GGKUtilLib.hideAllMdiChildren();
                    OneToOneCmpFrm cmp = new OneToOneCmpFrm(kit1,kit2);
                    cmp.MdiParent = Program.GGKitFrmMainInst;
                    cmp.Visible = true;
                    cmp.WindowState = FormWindowState.Maximized;
                    this.Close();
                    break;                

                default:
                    break;
            }

            
            //
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewCellStyle style_no = new DataGridViewCellStyle();
            style_no.ForeColor = Color.LightGray;
            DataGridViewCellStyle style_yes = new DataGridViewCellStyle();
            style_yes.ForeColor = Color.Black;

            if (dataGridView1.SelectedRows.Count > 0)
            {
                selected_kit_one = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[0].Value.ToString() != selected_kit_two)
                        row.DefaultCellStyle = style_yes;
                }
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (row.Cells[0].Value.ToString() == selected_kit_one)
                        row.DefaultCellStyle = style_no;
                    else
                        row.DefaultCellStyle = style_yes;
                    if (row.Cells[0].Value.ToString() == selected_kit_two)
                        row.DefaultCellStyle = style_yes;
                }
            }
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewCellStyle style_no = new DataGridViewCellStyle();
            style_no.ForeColor = Color.LightGray;
            DataGridViewCellStyle style_yes = new DataGridViewCellStyle();
            style_yes.ForeColor = Color.Black;

            if (dataGridView2.SelectedRows.Count > 0)
            {
                selected_kit_two = dataGridView2.SelectedRows[0].Cells[0].Value.ToString();
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (row.Cells[0].Value.ToString() != selected_kit_one)
                        row.DefaultCellStyle = style_yes;
                }
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[0].Value.ToString() == selected_kit_two)
                        row.DefaultCellStyle = style_no;
                    else
                        row.DefaultCellStyle = style_yes;
                    if (row.Cells[0].Value.ToString() == selected_kit_one)
                        row.DefaultCellStyle = style_yes;
                }
            }
        }
    }
}

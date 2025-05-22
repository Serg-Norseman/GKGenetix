/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using GenetixKit.Core;

namespace GenetixKit.Forms
{
    public partial class SelectTwoKitsFrm : Form
    {
        private string selectedKit1 = null;
        private string selectedKit2 = null;
        private readonly UIOperation selectedOperation;

        public SelectTwoKitsFrm(UIOperation operation)
        {
            InitializeComponent();

            GKUIFuncs.FixGridView(dataGridView1);
            dataGridView1.AddColumn("kit_no", "Kit#");
            dataGridView1.AddColumn("name", "Name");

            GKUIFuncs.FixGridView(dataGridView2);
            dataGridView2.AddColumn("kit_no", "Kit#");
            dataGridView2.AddColumn("name", "Name");

            selectedOperation = operation;
        }

        private void SelectTwoKitsFrm_Load(object sender, EventArgs e)
        {
            string selectSql;
            switch (selectedOperation) {
                case UIOperation.SELECT_ADMIXTURE:
                    selectSql = @"SELECT kit_no, name FROM kit_master WHERE disabled=0 order by name ASC";
                    break;
                default:
                    selectSql = @"SELECT kit_no, name FROM kit_master WHERE disabled=0 order by name ASC";
                    break;
            }

            var dt1 = GKSqlFuncs.QueryTable(selectSql);
            var dt2 = GKSqlFuncs.QueryTable(selectSql);

            dataGridView1.DataSource = dt1;
            dataGridView2.DataSource = dt2;
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            string kit1 = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            string kit2 = dataGridView2.SelectedRows[0].Cells[0].Value.ToString();

            if (kit1 == kit2) {
                MessageBox.Show("Please select different kits to compare.", "One-to-One Compare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            switch (selectedOperation) {
                case UIOperation.SELECT_ADMIXTURE:
                    Program.KitInstance.ShowOneToOneCmp(kit1, kit2);
                    this.Close();
                    break;

                default:
                    break;
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0) {
                selectedKit1 = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                dataGridView2.Invalidate(false);
            }
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0) {
                selectedKit2 = dataGridView2.SelectedRows[0].Cells[0].Value.ToString();
                dataGridView1.Invalidate(false);
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var row = ((DataTable)dataGridView1.DataSource).Rows[e.RowIndex];
            string val = row[0].ToString();
            e.CellStyle.ForeColor = (val != selectedKit2) ? Color.Black : Color.LightGray;
        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var row = ((DataTable)dataGridView2.DataSource).Rows[e.RowIndex];
            string val = row[0].ToString();
            e.CellStyle.ForeColor = (val != selectedKit1) ? Color.Black : Color.LightGray;
        }
    }
}

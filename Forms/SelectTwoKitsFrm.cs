/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GenetixKit.Core;
using GenetixKit.Core.Model;

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
            dataGridView1.AddColumn("KitNo", "Kit#");
            dataGridView1.AddColumn("Name", "Name");

            GKUIFuncs.FixGridView(dataGridView2);
            dataGridView2.AddColumn("KitNo", "Kit#");
            dataGridView2.AddColumn("Name", "Name");

            selectedOperation = operation;
        }

        private void SelectTwoKitsFrm_Load(object sender, EventArgs e)
        {
            IList<KitDTO> dt1, dt2;

            switch (selectedOperation) {
                case UIOperation.SELECT_ADMIXTURE:
                    dt1 = GKSqlFuncs.QueryKits(true, "", "order by name asc");
                    dt2 = GKSqlFuncs.QueryKits(true, "", "order by name asc");
                    break;
                default:
                    return;
            }

            dataGridView1.DataSource = dt1;
            dataGridView2.DataSource = dt2;
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            string kit1 = dataGridView1.GetSelectedObj<KitDTO>()?.KitNo;
            string kit2 = dataGridView2.GetSelectedObj<KitDTO>()?.KitNo;

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
            selectedKit1 = dataGridView1.GetSelectedObj<KitDTO>()?.KitNo;
            dataGridView2.Invalidate(false);
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            selectedKit2 = dataGridView2.GetSelectedObj<KitDTO>().KitNo;
            dataGridView1.Invalidate(false);
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var row = ((IList<KitDTO>)dataGridView1.DataSource)[e.RowIndex];
            e.CellStyle.ForeColor = (row.KitNo != selectedKit2) ? Color.Black : Color.LightGray;
        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var row = ((IList<KitDTO>)dataGridView2.DataSource)[e.RowIndex];
            e.CellStyle.ForeColor = (row.KitNo != selectedKit1) ? Color.Black : Color.LightGray;
        }
    }
}

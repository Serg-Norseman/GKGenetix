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
using GKGenetix.Core.Model;
using GKGenetix.UI;

namespace GenetixKit.Forms
{
    public partial class SelectKitFrm : Form
    {
        private string kit = "Unknown";
        private IList<KitDTO> tbl;

        public string GetSelectedKit()
        {
            return kit;
        }

        public SelectKitFrm()
        {
            InitializeComponent();

            UIHelper.FixGridView(dgvKits);
            dgvKits.AddColumn("KitNo", "Kit #");
            dgvKits.AddColumn("Name", "Name");
            dgvKits.AddCheckedColumn("Disabled", "Disabled");
            dgvKits.AddColumn("LastModified", "Last Modified");
        }

        private void OpenKitFrm_Load(object sender, EventArgs e)
        {
            btnOpen.Text = "Select";

            tbl = GKSqlFuncs.QueryKits(false, true);
            dgvKits.DataSource = tbl;

            if (tbl.Count == 0) {
                MessageBox.Show("There are no kits available to open.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
            }
        }

        private void dgvKits_SelectionChanged(object sender, EventArgs e)
        {
            var value = dgvKits.GetSelectedObj<KitDTO>()?.KitNo;
            kitLbl.Text = value != null ? value.ToString() : string.Empty;
        }

        private void dgvKits_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var row = tbl[e.RowIndex];
            e.CellStyle.ForeColor = (!row.Disabled) ? Color.Black : Color.LightGray;
        }

        private void dgvKits_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectKit(kitLbl.Text);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            SelectKit(kitLbl.Text);
        }

        private void SelectKit(string kit)
        {
            this.kit = kit;
            this.Close();
        }
    }
}

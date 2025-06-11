/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using Eto.Drawing;
using Eto.Forms;
using Eto.Serialization.Xaml;
using GKGenetix.Core.Database;

namespace GKGenetix.UI.Forms
{
    public partial class SelectKitFrm : Dialog
    {
        #region Design components
#pragma warning disable CS0169, CS0649, IDE0044, IDE0051

        private Button btnOpen;
        private GridView dgvKits;
        private Label kitLbl;

#pragma warning restore CS0169, CS0649, IDE0044, IDE0051
        #endregion


        private string kit = "Unknown";
        private IList<TestRecord> tbl;
        private char requestedSex;

        public string GetSelectedKit()
        {
            return kit;
        }

        public SelectKitFrm(char sex)
        {
            XamlReader.Load(this);

            UIHelper.FixGridView(dgvKits);
            dgvKits.AddColumn("KitNo", "Kit #");
            dgvKits.AddColumn("Name", "Name");
            dgvKits.AddCheckedColumn("Disabled", "Disabled");
            dgvKits.AddColumn("LastModified", "Last Modified");

            requestedSex = sex;
        }

        private void OpenKitFrm_Load(object sender, EventArgs e)
        {
            btnOpen.Text = "Select";

            tbl = GKSqlFuncs.QueryKits(false, true, requestedSex);
            dgvKits.DataStore = tbl;

            if (tbl.Count == 0) {
                MessageBox.Show("There are no kits available to open.", "", MessageBoxButtons.OK, MessageBoxType.Warning);
                this.Close();
            }
        }

        private void dgvKits_SelectionChanged(object sender, EventArgs e)
        {
            var value = dgvKits.GetSelectedObj<TestRecord>()?.KitNo;
            kitLbl.Text = value != null ? value.ToString() : string.Empty;
        }

        private void dgvKits_CellFormatting(object sender, GridCellFormatEventArgs e)
        {
            var row = tbl[e.Row];
            e.ForegroundColor = (!row.Disabled) ? Colors.Black : Colors.LightGrey;
        }

        private void dgvKits_CellContentDoubleClick(object sender, GridCellMouseEventArgs e)
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

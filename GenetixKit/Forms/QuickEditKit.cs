/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GenetixKit.Core;
using GKGenetix.Core.Model;

namespace GenetixKit.Forms
{
    public partial class QuickEditKit : Form
    {
        private IList<KitDTO> tblKits;

        public QuickEditKit()
        {
            InitializeComponent();

            dgvEditKit.AutoGenerateColumns = false;
            dgvEditKit.AddColumn("KitNo", "Kit#");
            dgvEditKit.AddColumn("Name", "Name", "", true, false);
            dgvEditKit.AddComboColumn("Sex", "Sex", new object[] { "Unknown", "Male", "Female" });
            dgvEditKit.AddCheckedColumn("Disabled", "Disabled");
            dgvEditKit.AddButtonColumn("Location", "Location");
        }

        private void QuickEditKit_Load(object sender, EventArgs e)
        {
            Program.KitInstance.EnableSave();
            Program.KitInstance.EnableDelete();

            tblKits = GKSqlFuncs.QueryKits();
            dgvEditKit.DataSource = tblKits;
        }

        private void QuickEditKit_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.KitInstance.DisableSave();
            Program.KitInstance.DisableDelete();
        }

        public void Save()
        {
            Program.KitInstance.SetStatus("Saving ...");

            foreach (var row in tblKits) {
                string location = row.Location;
                string x, y;
                if (location == "Unknown") {
                    x = "0";
                    y = "0";
                } else {
                    x = location.Split(new char[] { ':' })[0];
                    y = location.Split(new char[] { ':' })[1];
                }

                GKSqlFuncs.SaveKit(row.KitNo, row.Name, row.Sex, row.Disabled, x, y);
            }

            Program.KitInstance.SetStatus("Saved.");
        }

        public void Delete()
        {
            var rowsToDelete = dgvEditKit.SelectedRows.Cast<DataGridViewRow>().Select(x => { return ((KitDTO)x.DataBoundItem); }).ToList();

            int selRowsCount = rowsToDelete.Count;
            if (MessageBox.Show($"You had selected {selRowsCount} kits to be deleted. Are you sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                Program.KitInstance.SetStatus($"Deleting {selRowsCount} kit(s) and all it's associated data ...");
                this.Enabled = false;
                Program.KitInstance.DisableToolbar();

                Task.Factory.StartNew((object obj) => {
                    var rows2Del = (List<KitDTO>)obj;
                    foreach (var row in rows2Del) {
                        GKSqlFuncs.DeleteKit(row.KitNo);
                        tblKits.Remove(row);
                    }

                    this.Invoke(new MethodInvoker(delegate {
                        dgvEditKit.DataSource = tblKits;

                        Program.KitInstance.SetStatus("Deleted.");
                        this.Enabled = true;
                        Program.KitInstance.EnableToolbar();
                    }));
                }, rowsToDelete);
            }
        }

        private void dgvEditKit_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0) {
                var kitRow = tblKits[e.RowIndex];
                string location = kitRow.Location;
                int x = 0;
                int y = 0;
                if (location != "Unknown") {
                    var parts = location.Split(new char[] { ':' });
                    x = int.Parse(parts[0]);
                    y = int.Parse(parts[1]);
                }
                Program.KitInstance.SelectLocation(ref x, ref y);
                kitRow.Location = x.ToString() + ":" + y.ToString();

                senderGrid.Invalidate();
            }
        }
    }
}

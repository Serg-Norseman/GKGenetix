/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GKGenetix.Core;
using GKGenetix.Core.Database;

namespace GKGenetix.UI.Forms
{
    public partial class KitsExplorer : GKWidget
    {
        private IList<TestRecord> tblKits;


        public IList<TestRecord> SelectedKits
        {
            get {
                var selectedRows = dgvEditKit.SelectedRows.Cast<DataGridViewRow>().Select(x => { return ((TestRecord)x.DataBoundItem); }).ToList();
                return selectedRows;
            }
        }

        public event EventHandler SelectionChanged;


        public KitsExplorer() : base(null)
        {
            InitializeComponent();

            dgvEditKit.SelectionChanged += new EventHandler(dgvEditKit_SelectionChanged);
            dgvEditKit.AutoGenerateColumns = false;
            dgvEditKit.AddColumn("KitNo", "Kit#");
            dgvEditKit.AddColumn("Name", "Name", "", true, false);
            dgvEditKit.AddComboColumn("Sex", "Sex", new object[] { "Unknown", "Male", "Female" });
            dgvEditKit.AddCheckedColumn("Disabled", "Disabled");
            dgvEditKit.AddButtonColumn("Location", "Location");
            dgvEditKit.AddColumn("LastModified", "Last Modified");
        }

        public void SetHost(IKitHost host)
        {
            _host = host;
        }

        private void QuickEditKit_Load(object sender, EventArgs e)
        {
            if (this.DesignMode) return;

            _host.EnableSave();
            _host.EnableDelete();

            UpdateView();
        }

        private void QuickEditKit_FormClosing(object sender, EventArgs e)
        {
            _host.DisableSave();
            _host.DisableDelete();
        }

        private void dgvEditKit_SelectionChanged(object sender, EventArgs e)
        {
            SelectionChanged?.Invoke(this, e);
        }

        public void UpdateView()
        {
            tblKits = GKSqlFuncs.QueryKits();
            foreach (var kit in tblKits) kit.PrepareValues();

            dgvEditKit.DataSource = tblKits;
        }

        public void Save()
        {
            _host.SetStatus("Saving ...");

            foreach (var row in tblKits) {
                string location = row.Location;
                string lng, lat;
                if (location == "Unknown") {
                    lng = "0";
                    lat = "0";
                } else {
                    lng = location.Split(new char[] { ':' })[0];
                    lat = location.Split(new char[] { ':' })[1];
                }
                GKSqlFuncs.SaveKit(row.KitNo, row.Name, row.Sex, row.Disabled, lng, lat);
            }

            _host.SetStatus("Saved.");
        }

        public void Delete()
        {
            var rowsToDelete = this.SelectedKits;

            int selRowsCount = rowsToDelete.Count;
            if (MessageBox.Show($"You had selected {selRowsCount} kits to be deleted. Are you sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                _host.SetStatus($"Deleting {selRowsCount} kit(s) and all it's associated data ...");
                this.Enabled = false;
                _host.DisableToolbar();

                Task.Factory.StartNew((object obj) => {
                    var rows2Del = (List<TestRecord>)obj;
                    foreach (var row in rows2Del) {
                        GKSqlFuncs.DeleteKit(row.KitNo);
                        tblKits.Remove(row);
                    }

                    this.Invoke(new MethodInvoker(delegate {
                        dgvEditKit.DataSource = tblKits;

                        _host.SetStatus("Deleted.");
                        this.Enabled = true;
                        _host.EnableToolbar();
                    }));
                }, rowsToDelete);
            }
        }

        private void dgvEditKit_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var row = tblKits[e.RowIndex];
            e.CellStyle.ForeColor = (!row.Disabled) ? Color.Black : Color.LightGray;
        }

        private void dgvEditKit_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvEditKit.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0) {
                var kitRow = tblKits[e.RowIndex];
                string location = kitRow.Location;
                double lng = 0;
                double lat = 0;
                if (location != "Unknown") {
                    var parts = location.Split(new char[] { ':' });
                    lng = int.Parse(parts[0]);
                    lat = int.Parse(parts[1]);
                }
                _host.SelectLocation(ref lng, ref lat);
                kitRow.Location = lng.ToString() + ":" + lat.ToString();

                dgvEditKit.Invalidate();
            }
        }

        private void dgvEditKit_DoubleClick(object sender, EventArgs e)
        {
            _host?.ChangeKits(this.SelectedKits);
        }
    }
}

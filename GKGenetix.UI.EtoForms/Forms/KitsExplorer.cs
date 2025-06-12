/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eto.Drawing;
using Eto.Forms;
using GKGenetix.Core;
using GKGenetix.Core.Database;

namespace GKGenetix.UI.Forms
{
    public partial class KitsExplorer : GKWidget
    {
        private GridView dgvEditKit;


        private IList<TestRecord> tblKits;


        public IList<TestRecord> SelectedKits
        {
            get {
                var selectedRows = dgvEditKit.SelectedItems.Cast<TestRecord>().ToList();
                return selectedRows;
            }
        }

        public event EventHandler SelectionChanged;


        public KitsExplorer() : base(null)
        {
            Load += QuickEditKit_Load;
            Closing += QuickEditKit_FormClosing;

            dgvEditKit = new GridView();
            dgvEditKit.SelectionChanged += dgvEditKit_SelectionChanged;

            dgvEditKit.AddColumn("KitNo", "Kit#");
            dgvEditKit.AddColumn("Name", "Name", "", true, false);
            dgvEditKit.AddComboColumn("Sex", "Sex", new object[] { "Unknown", "Male", "Female" });
            dgvEditKit.AddCheckedColumn("Disabled", "Disabled", false);
            dgvEditKit.AddButtonColumn("Location", "Location");
            dgvEditKit.AddColumn("LastModified", "Last Modified");
            dgvEditKit.CellFormatting += dgvEditKit_CellFormatting;
            dgvEditKit.CellClick += dgvEditKit_CellContentClick;
            dgvEditKit.CellDoubleClick += dgvEditKit_DoubleClick;
            dgvEditKit.AllowMultipleSelection = true;
            dgvEditKit.GridLines = GridLines.Both;
            Content = dgvEditKit;
        }

        public void SetHost(IKitHost host)
        {
            _host = host;
        }

        private void QuickEditKit_Load(object sender, EventArgs e)
        {
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

            dgvEditKit.DataStore = tblKits;
        }

        public void Save()
        {
            _host.SetStatus("Saving ...");

            foreach (var row in tblKits) {
                GKSqlFuncs.SaveKit(row);
            }

            _host.SetStatus("Saved.");
        }

        public void Delete()
        {
            var rowsToDelete = this.SelectedKits;

            int selRowsCount = rowsToDelete.Count;
            if (MessageBox.Show($"You had selected {selRowsCount} kits to be deleted. Are you sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxType.Question) == DialogResult.Yes) {
                _host.SetStatus($"Deleting {selRowsCount} kit(s) and all it's associated data ...");
                this.Enabled = false;
                _host.DisableToolbar();

                Task.Factory.StartNew((object obj) => {
                    var rows2Del = (List<TestRecord>)obj;
                    foreach (var row in rows2Del) {
                        GKSqlFuncs.DeleteKit(row.KitNo);
                        tblKits.Remove(row);
                    }

                    Application.Instance.Invoke(new Action(delegate {
                        dgvEditKit.DataStore = tblKits;

                        _host.SetStatus("Deleted.");
                        this.Enabled = true;
                        _host.EnableToolbar();
                    }));
                }, rowsToDelete);
            }
        }

        private void dgvEditKit_CellFormatting(object sender, GridCellFormatEventArgs e)
        {
            var row = tblKits[e.Row];
            e.ForegroundColor = (!row.Disabled) ? Colors.Black : Colors.LightGrey;
        }

        private void dgvEditKit_CellContentClick(object sender, GridCellMouseEventArgs e)
        {
            if (e.Column == 4 && e.Row >= 0) {
                var kitRow = tblKits[e.Row];
                _host.SelectLocation(kitRow);

                dgvEditKit.Invalidate();
            }
        }

        private void dgvEditKit_DoubleClick(object sender, EventArgs e)
        {
            _host?.ChangeKits(this.SelectedKits);
        }
    }
}

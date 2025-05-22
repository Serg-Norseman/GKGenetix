/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using GenetixKit.Core;

namespace GenetixKit.Forms
{
    public partial class SelectKitFrm : Form
    {
        private readonly List<string> disabled = new List<string>();
        private readonly UIOperation selectedOperation = 0;
        private string kit = "Unknown";

        public string GetSelectedKit()
        {
            return kit;
        }

        public SelectKitFrm(UIOperation operation)
        {
            InitializeComponent();

            GKUIFuncs.FixGridView(dgvKits);
            dgvKits.AddColumn("kit_no", "Kit #");
            dgvKits.AddColumn("name", "Name");
            dgvKits.AddCheckedColumn("disabled", "Disabled");
            dgvKits.AddColumn("last_modified", "Last Modified");

            selectedOperation = operation;
        }

        private void OpenKitFrm_Load(object sender, EventArgs e)
        {
            const string defaultSql = @"SELECT kit_no,name,disabled,last_modified FROM kit_master order by last_modified DESC";

            string selectSql;
            switch (selectedOperation) {
                case UIOperation.OPEN_KIT:
                    btnOpen.Text = "Open";
                    selectSql = defaultSql;
                    break;
                case UIOperation.SELECT_ONE_TO_MANY:
                    btnOpen.Text = "Select";
                    selectSql = defaultSql;
                    break;
                case UIOperation.SELECT_ADMIXTURE:
                    btnOpen.Text = "Select";
                    selectSql = @"SELECT kit_no,name,disabled,last_modified FROM kit_master WHERE reference=0 order by last_modified DESC";
                    break;
                case UIOperation.SELECT_ROH:
                    btnOpen.Text = "Select";
                    selectSql = defaultSql;
                    break;
                case UIOperation.SELECT_KIT:
                    btnOpen.Text = "Select";
                    selectSql = defaultSql;
                    break;
                case UIOperation.SELECT_MTPHYLOGENY:
                    btnOpen.Text = "Select";
                    selectSql = @"select kit_no,name,disabled,last_modified from kit_master where kit_no in (select distinct kit_no from kit_mtdna) order by last_modified DESC";
                    break;
                case UIOperation.SELECT_MITOMAP:
                    btnOpen.Text = "Select";
                    selectSql = @"select kit_no,name,disabled,last_modified from kit_master where kit_no in (select distinct kit_no from kit_mtdna) order by last_modified DESC";
                    break;
                case UIOperation.SELECT_ISOGGYTREE:
                    btnOpen.Text = "Select";
                    selectSql = @"select kit_no,name,disabled,last_modified from kit_master where kit_no in (select distinct kit_no from kit_ysnps) order by last_modified DESC";
                    break;
                default:
                    selectSql = defaultSql;
                    break;
            }

            var tbl = GKSqlFuncs.QueryTable(selectSql);
            dgvKits.DataSource = tbl;

            disabled.Clear();
            foreach (DataRow row in tbl.Rows) {
                if (Convert.ToInt16(row[2]) == 1) {
                    disabled.Add(Convert.ToString(row[0]));
                }
            }

            if (dgvKits.Rows.Count == 0) {
                MessageBox.Show("There are no kits available to open.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
            }
        }

        private void dgvKits_SelectionChanged(object sender, EventArgs e)
        {
            try {
                if (dgvKits.SelectedRows.Count > 0) {
                    var value = dgvKits.SelectedRows[0].Cells[0].Value;
                    kitLbl.Text = value != null ? value.ToString() : string.Empty;
                }
            } catch (Exception) {
                // ignore
            }
        }

        private void dgvKits_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var row = ((DataTable)dgvKits.DataSource).Rows[e.RowIndex];
            short val = Convert.ToInt16(row[2]);
            e.CellStyle.ForeColor = (val != 1) ? Color.Black : Color.LightGray;
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
            switch (selectedOperation) {
                case UIOperation.OPEN_KIT:
                    Program.KitInstance.NewKit(kit, disabled.Contains(kit));
                    break;
                case UIOperation.SELECT_ONE_TO_MANY:
                    Program.KitInstance.ShowMatchingKits(kit);
                    break;
                case UIOperation.SELECT_ADMIXTURE:
                    Program.KitInstance.ShowAdmixture(kit);
                    break;
                case UIOperation.SELECT_ROH:
                    Program.KitInstance.ShowROH(kit);
                    break;
                case UIOperation.SELECT_KIT:
                    this.kit = kit;
                    this.Visible = false;
                    return;
                case UIOperation.SELECT_MTPHYLOGENY:
                    Program.KitInstance.ShowMtPhylogeny(kit);
                    break;
                case UIOperation.SELECT_MITOMAP:
                    Program.KitInstance.ShowMitoMap(kit);
                    break;
                case UIOperation.SELECT_ISOGGYTREE:
                    Program.KitInstance.ShowIsoggYTree(kit);
                    break;
                default:
                    break;
            }
            this.Close();
        }
    }
}

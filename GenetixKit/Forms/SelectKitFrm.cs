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
    public partial class SelectKitFrm : Form
    {
        private readonly List<string> disabled = new List<string>();
        private readonly UIOperation selectedOperation = 0;
        private string kit = "Unknown";
        private IList<KitDTO> tbl;

        public string GetSelectedKit()
        {
            return kit;
        }

        public SelectKitFrm(UIOperation operation)
        {
            InitializeComponent();

            GKUIFuncs.FixGridView(dgvKits);
            dgvKits.AddColumn("KitNo", "Kit #");
            dgvKits.AddColumn("Name", "Name");
            dgvKits.AddCheckedColumn("Disabled", "Disabled");
            dgvKits.AddColumn("LastModified", "Last Modified");

            selectedOperation = operation;
        }

        private void OpenKitFrm_Load(object sender, EventArgs e)
        {
            string whereSql = " where reference = 0";

            switch (selectedOperation) {
                case UIOperation.OPEN_KIT:
                    btnOpen.Text = "Open";
                    break;
                case UIOperation.SELECT_ONE_TO_MANY:
                case UIOperation.SELECT_ADMIXTURE:
                case UIOperation.SELECT_ROH:
                case UIOperation.SELECT_KIT:
                    btnOpen.Text = "Select";
                    break;
                case UIOperation.SELECT_MTPHYLOGENY:
                case UIOperation.SELECT_MITOMAP:
                    btnOpen.Text = "Select";
                    whereSql = " where kit_no in (select distinct kit_no from kit_mtdna)";
                    break;
                case UIOperation.SELECT_ISOGGYTREE:
                    btnOpen.Text = "Select";
                    whereSql = " where kit_no in (select distinct kit_no from kit_ysnps)";
                    break;
                default:
                    break;
            }

            tbl = GKSqlFuncs.QueryKits(false, whereSql, "");
            dgvKits.DataSource = tbl;

            disabled.Clear();
            foreach (var row in tbl) {
                if (row.Disabled) {
                    disabled.Add(Convert.ToString(row.KitNo));
                }
            }

            if (tbl.Count == 0) {
                MessageBox.Show("There are no kits available to open.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
            }
        }

        private void dgvKits_SelectionChanged(object sender, EventArgs e)
        {
            try {
                var value = dgvKits.GetSelectedObj<KitDTO>()?.KitNo;
                kitLbl.Text = value != null ? value.ToString() : string.Empty;
            } catch (Exception) {
                // ignore
            }
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

/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;
using GenetixKit.Core;

namespace GenetixKit.Forms
{
    public partial class QuickEditKit : Form
    {
        public QuickEditKit()
        {
            InitializeComponent();
        }

        private void QuickEditKit_Load(object sender, EventArgs e)
        {
            GKUIFuncs.enableSave();
            GKUIFuncs.enableDeleteKitToolbarBtn();
            GKUIFuncs.enable_DisableKitToolbarBtn();
            GKUIFuncs.enable_EnableKitToolbarBtn();
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            DataGridViewCellStyle gray = new DataGridViewCellStyle();
            gray.ForeColor = Color.LightGray;

            SQLiteConnection cnn = GKSqlFuncs.getDBConnection();
            dgvEditKit.Rows.Clear();
            SQLiteCommand query = new SQLiteCommand(@"SELECT kit_no,name,sex,disabled,coalesce(x,0),coalesce(y,0),last_modified FROM kit_master WHERE reference=0 order by last_modified DESC", cnn);
            SQLiteDataReader reader = query.ExecuteReader();
            string sex = "U";
            string xy = null;
            while (reader.Read()) {
                int new_idx = dgvEditKit.Rows.Add();
                DataGridViewRow row = dgvEditKit.Rows[new_idx];
                row.Cells[0].Value = reader.GetString(0);
                row.Cells[1].Value = reader.GetString(1);

                sex = reader.GetString(2);
                if (sex == "U")
                    row.Cells[2].Value = "Unknown";
                else if (sex == "M")
                    row.Cells[2].Value = "Male";
                else if (sex == "F")
                    row.Cells[2].Value = "Female";

                if (reader.GetInt16(3) == 1)
                    row.Cells[3].Value = true;
                else
                    row.Cells[3].Value = false;

                xy = reader.GetInt16(4).ToString() + ":" + reader.GetInt16(5).ToString();
                if (xy == "0:0")
                    xy = "Unknown";
                row.Cells[4].Value = xy;
            }
            query.Dispose();
            cnn.Dispose();
        }

        private void QuickEditKit_FormClosing(object sender, FormClosingEventArgs e)
        {
            GKUIFuncs.disableSave();
            GKUIFuncs.disableDeleteKitToolbarBtn();
            GKUIFuncs.disable_DisableKitToolbarBtn();
            GKUIFuncs.disable_EnableKitToolbarBtn();
        }

        public void Save()
        {
            GKUIFuncs.setStatus("Saving ...");
            SQLiteConnection conn = GKSqlFuncs.getDBConnection();
            string sex = "";
            string location = "";
            using (SQLiteTransaction trans = conn.BeginTransaction()) {
                foreach (DataGridViewRow row in dgvEditKit.Rows) {
                    SQLiteCommand upCmd = new SQLiteCommand("UPDATE kit_master set name=@name, sex=@sex, disabled=@disabled, x=@x, y=@y WHERE kit_no=@kit_no", conn);
                    upCmd.Parameters.AddWithValue("@name", row.Cells[1].Value.ToString());

                    sex = row.Cells[2].Value.ToString();
                    if (sex == "Unknown")
                        upCmd.Parameters.AddWithValue("@sex", "U");
                    else if (sex == "Male")
                        upCmd.Parameters.AddWithValue("@sex", "M");
                    else if (sex == "Female")
                        upCmd.Parameters.AddWithValue("@sex", "F");

                    if (((bool)row.Cells[3].Value))
                        upCmd.Parameters.AddWithValue("@disabled", "1");
                    else
                        upCmd.Parameters.AddWithValue("@disabled", "0");

                    location = row.Cells[4].Value.ToString();
                    if (location == "Unknown") {
                        upCmd.Parameters.AddWithValue("@x", "0");
                        upCmd.Parameters.AddWithValue("@y", "0");
                    } else {
                        upCmd.Parameters.AddWithValue("@x", location.Split(new char[] { ':' })[0]);
                        upCmd.Parameters.AddWithValue("@y", location.Split(new char[] { ':' })[1]);
                    }

                    upCmd.Parameters.AddWithValue("@kit_no", row.Cells[0].Value.ToString());
                    upCmd.ExecuteNonQuery();
                }
                //
                trans.Commit();
            }

            GKUIFuncs.setStatus("Saved.");
        }

        public void Delete()
        {
            if (MessageBox.Show("You had selected " + dgvEditKit.SelectedRows.Count.ToString() + " kits to be deleted. Are you sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                GKUIFuncs.setStatus("Deleting " + dgvEditKit.SelectedRows.Count.ToString() + " kit(s) and all it's associated data ...");
                this.Enabled = false;
                GKUIFuncs.disableMenu();
                GKUIFuncs.disableToolbar();
                bwDelete.RunWorkerAsync(dgvEditKit);
            }
        }

        public void Enable()
        {
            GKUIFuncs.setStatus("Enabling ...");
            SQLiteConnection conn = GKSqlFuncs.getDBConnection();
            using (SQLiteTransaction trans = conn.BeginTransaction()) {
                foreach (DataGridViewRow row in dgvEditKit.SelectedRows) {
                    SQLiteCommand upCmd = new SQLiteCommand("UPDATE kit_master set disabled=@disabled WHERE kit_no=@kit_no", conn);
                    upCmd.Parameters.AddWithValue("@name", row.Cells[1].Value.ToString());
                    upCmd.Parameters.AddWithValue("@disabled", "0");
                    upCmd.Parameters.AddWithValue("@kit_no", row.Cells[0].Value.ToString());
                    upCmd.ExecuteNonQuery();
                    row.Cells[3].Value = false;
                }
                //
                trans.Commit();
            }

            GKUIFuncs.setStatus("Enabled.");
        }

        public void Disable()
        {
            GKUIFuncs.setStatus("Disabling ...");
            SQLiteConnection conn = GKSqlFuncs.getDBConnection();
            using (SQLiteTransaction trans = conn.BeginTransaction()) {
                foreach (DataGridViewRow row in dgvEditKit.SelectedRows) {
                    SQLiteCommand upCmd = new SQLiteCommand("UPDATE kit_master set disabled=@disabled WHERE kit_no=@kit_no", conn);
                    upCmd.Parameters.AddWithValue("@name", row.Cells[1].Value.ToString());
                    upCmd.Parameters.AddWithValue("@disabled", "1");
                    upCmd.Parameters.AddWithValue("@kit_no", row.Cells[0].Value.ToString());
                    upCmd.ExecuteNonQuery();
                    row.Cells[3].Value = true;
                }
                //
                trans.Commit();
            }

            GKUIFuncs.setStatus("Disabled.");
        }

        private void dgvEditKit_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0) {
                string location = dgvEditKit.Rows[e.RowIndex].Cells[4].Value.ToString();
                int x = 0;
                int y = 0;
                if (location != "Unknown") {
                    x = int.Parse(location.Split(new char[] { ':' })[0]);
                    y = int.Parse(location.Split(new char[] { ':' })[1]);
                }
                LocationSelectFrm frm = new LocationSelectFrm(x, y);
                frm.ShowDialog(Program.GGKitFrmMainInst);
                x = frm.X;
                y = frm.Y;
                frm.Dispose();
                dgvEditKit.Rows[e.RowIndex].Cells[4].Value = x.ToString() + ":" + y.ToString();
            }
        }

        private void bwDelete_DoWork(object sender, DoWorkEventArgs e)
        {
            DataGridView dgv = (DataGridView)e.Argument;
            SQLiteConnection conn = GKSqlFuncs.getDBConnection();
            List<DataGridViewRow> rows = new List<DataGridViewRow>();
            using (SQLiteTransaction trans = conn.BeginTransaction()) {

                foreach (DataGridViewRow row in dgv.SelectedRows) {
                    SQLiteCommand upCmd = new SQLiteCommand("DELETE FROM kit_master WHERE kit_no=@kit_no", conn);
                    upCmd.Parameters.AddWithValue("@kit_no", row.Cells[0].Value.ToString());
                    upCmd.ExecuteNonQuery();
                    rows.Add(row);
                }
                //
                trans.Commit();
            }


            this.Invoke(new MethodInvoker(delegate {
                foreach (DataGridViewRow row in rows)
                    dgvEditKit.Rows.Remove(row);
            }));
        }

        private void bwDelete_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            GKUIFuncs.setStatus("Deleted.");

            this.Enabled = true;
            GKUIFuncs.enableMenu();
            GKUIFuncs.enableToolbar();
        }
    }
}

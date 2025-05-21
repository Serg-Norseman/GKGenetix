/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GenetixKit.Core;

namespace GenetixKit.Forms
{
    public partial class SettingsFrm : Form
    {
        SortedDictionary<string, string[]> settings = null;

        public SettingsFrm()
        {
            InitializeComponent();
        }

        private void SettingsFrm_Load(object sender, EventArgs e)
        {
            GKUtilLib.enableSave();
            lbSettings.Items.Clear();
            settings = GKSettings.getSettings();
            lbSettings.Items.AddRange(settings.Keys.ToArray());
            if (lbSettings.Items.Count > 0) {
                lbSettings.SelectedIndex = 0;
                populateForm(lbSettings.Text);
            }
        }

        private void lbSettings_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbSettings.Text != null) {
                populateForm(lbSettings.Text);
            }
        }

        private void populateForm(string kit)
        {
            string[] data = settings[kit];
            tbKey.Text = kit;
            tbValue.Text = data[0];
            tbDesc.Text = data[1];
            lblLastModified.Text = "Last Modified on " + data[3];
            if (data[2] == "1") {
                tbValue.ReadOnly = true;
                btnResetDefault.Enabled = false;
                GKUtilLib.disableSave();
            } else {
                tbValue.ReadOnly = false;
                btnResetDefault.Enabled = true;
                GKUtilLib.enableSave();
            }
        }

        public void Save()
        {
            GKSettings.saveParameterValue(tbKey.Text, tbValue.Text);
            GKUtilLib.setStatus("Value for parameter [" + tbKey.Text + "] saved.");
        }

        private void btnResetDefault_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to reset the parameter value with default value?", "Reset Parameter Value", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                tbValue.Text = GKSettings.getDefaultResetSettings()[tbKey.Text][0];
                Save();
            }
        }

        private void SettingsFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            GKUtilLib.disableSave();
        }
    }
}

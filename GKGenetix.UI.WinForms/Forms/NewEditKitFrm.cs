﻿/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GKGenetix.Core;
using GKGenetix.Core.Database;
using GKGenetix.Core.FileFormats;
using GKGenetix.Core.Model;
using GKGenetix.Core.Reference;

namespace GKGenetix.UI.Forms
{
    public partial class NewEditKitFrm : GKWidget
    {
        private bool saveSuccess = false;
        private string ysnps = null;
        private string mtdna = null;
        private readonly string kit;
        private readonly bool kitDisabled = false;
        private readonly Control[] editableCtrls = null;


        public NewEditKitFrm(IKitHost host, string kit, bool disabled) : base(host)
        {
            InitializeComponent();

            UIHelper.FixGridView(dgvAutosomal);
            dgvAutosomal.AddColumn("RSID", "RSID");
            dgvAutosomal.AddColumn("Chromosome", "Chromosome");
            dgvAutosomal.AddColumn("Position", "Position");
            dgvAutosomal.AddColumn("Genotype", "Genotype");

            SetupYGrid(dgvY12, RefData.ydna12);
            SetupYGrid(dgvY25, RefData.ydna25);
            SetupYGrid(dgvY37, RefData.ydna37);
            SetupYGrid(dgvY67, RefData.ydna67);
            SetupYGrid(dgvY111, RefData.ydna111);
            SetupYGrid(dgvYMisc, null);

            editableCtrls = new Control[] { txtFASTA, txtName, cbSex, dgvAutosomal, txtYDNA, dgvY12, dgvY25, dgvY37, dgvY67, dgvY111, dgvYMisc, txtMtDNA, btnPasteY, btnClearY };
            cbSex.SelectedIndex = 0;

            this.kit = kit;
            kitDisabled = disabled;
        }

        private void NewKitFrm_Load(object sender, EventArgs e)
        {
            if (kit == null) {
                this.Text = "New Kit";
                txtKit.Enabled = true;
                _host.EnableSave();
            } else {
                this.Text = "Edit Kit";
                txtKit.Text = kit;
                txtKit.Enabled = false;
                PopulateFields(kit);
            }
        }

        private void NewKitFrm_FormClosing(object sender, EventArgs e)
        {
            if (bwSave.IsBusy)
                bwSave.CancelAsync();
            if (bwNewKitAutosomalJob.IsBusy)
                bwNewKitAutosomalJob.CancelAsync();
            if (bwPopulate.IsBusy)
                bwPopulate.CancelAsync();

            _host.DisableSave();
            _host.DisableDelete();
            _host.SetStatus("Done.");
        }

        private void tabsNewKit_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabsNewKit.SelectedIndex) {
                case 0:
                    tipLbl.Text = "Tip: Drag and drop any autosomal raw file into the grid below. You can select multiple files. e.g, Autosomal and X.";
                    break;
                case 1:
                    tipLbl.Text = "Tip: Drag and drop Big-Y Export CSV file into the textbox below or you can type the Y-SNPs.";
                    break;
                case 2:
                    tipLbl.Text = "Tip: Drag and drop a FASTA file into the grid below or you can type the mutations.";
                    break;
                default:
                    tipLbl.Text = "";
                    break;
            }
        }

        private void tabsY_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabsY.SelectedIndex) {
                case 0:
                    tipLbl.Text = "Tip: Drag and drop Big-Y Export CSV file into the textbox below or you can type the Y-SNPs.";
                    break;
                case 1:
                    tipLbl.Text = "Tip: Copy markers from FTDNA project page and click on paste icon to populate into fields or you can type the Y-STR values";
                    break;
                default:
                    tipLbl.Text = "";
                    break;
            }
        }

        private void SetControlActivities(bool disabled)
        {
            if (disabled) {
                _host.EnableDelete();
                _host.DisableSave();
            } else {
                _host.EnableDelete();
                _host.EnableSave();
            }

            txtName.ReadOnly = disabled;
            dgvAutosomal.ReadOnly = disabled;
            txtYDNA.ReadOnly = disabled;
            dgvY12.ReadOnly = disabled;
            dgvY25.ReadOnly = disabled;
            dgvY37.ReadOnly = disabled;
            dgvY67.ReadOnly = disabled;
            dgvY111.ReadOnly = disabled;
            dgvYMisc.ReadOnly = disabled;
            txtMtDNA.ReadOnly = disabled;
        }

        private void nekf_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private bool TryGetFilesDrop(DragEventArgs e, out string[] filePaths)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                return true;
            }
            filePaths = Array.Empty<string>();
            return false;
        }

        #region Autosomal

        public void ImportFile(DNATestInfo dnaTest)
        {
            txtName.Text = dnaTest.Name;
            switch (dnaTest.Sex) {
                case 'M':
                    cbSex.SelectedIndex = 1;
                    break;
                case 'F':
                    cbSex.SelectedIndex = 2;
                    break;
                default:
                    cbSex.SelectedIndex = 0;
                    break;
            }

            string[] filePaths = new string[] { dnaTest.FileReference };

            _host.SetStatus("Parsing autosomal file(s) ...");
            if (bwNewKitAutosomalJob.IsBusy)
                bwNewKitAutosomalJob.CancelAsync();
            bwNewKitAutosomalJob.RunWorkerAsync(filePaths);
        }

        private void dgvAutosomal_DragDrop(object sender, DragEventArgs e)
        {
            // autosomal file dropped.
            if (TryGetFilesDrop(e, out string[] filePaths)) {
                _host.SetStatus("Parsing autosomal file(s) ...");
                if (bwNewKitAutosomalJob.IsBusy)
                    bwNewKitAutosomalJob.CancelAsync();
                bwNewKitAutosomalJob.RunWorkerAsync(filePaths);
            }
        }

        private void bwNewKitAutosomalJob_DoWork(object sender, DoWorkEventArgs e)
        {
            try {
                string[] filePaths = (string[])e.Argument;

                foreach (string file_path in filePaths) {
                    DNAData dnaout = FileFormatsHelper.ReadFile(file_path);

                    var atdna = dnaout.SNP;
                    var ysnps_arr = dnaout.ydna;
                    var mtdna_arr = dnaout.mtdna;

                    bwNewKitAutosomalJob.ReportProgress(-1, atdna.Count.ToString() + " SNPs found in " + Path.GetFileName(file_path));

                    if (ysnps_arr.Count != 0) {
                        ysnps_arr = ysnps_arr.Distinct().ToList();
                        var ysnps_str = string.Join(", ", ysnps_arr);

                        if (ysnps == null)
                            ysnps = ysnps_str;
                        else
                            ysnps += ysnps_str;
                    }

                    if (mtdna_arr.Count != 0) {
                        var mtdna_str = string.Join(", ", mtdna_arr);

                        if (mtdna == null)
                            mtdna = mtdna_str;
                        else
                            mtdna += mtdna_str;
                    }

                    this.Invoke(new MethodInvoker(delegate {
                        dgvAutosomal.DataSource = atdna;

                        if (ysnps != null)
                            txtYDNA.Text = ysnps;

                        if (mtdna != null)
                            txtMtDNA.Text = mtdna;
                    }));
                }
            } catch (Exception) {
                // something. The most probable cause is user just exited. so, just cancel the job...
            }
        }

        private void bwNewKitAutosomalJob_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _host.SetProgress(-1);
            _host.SetStatus("Done.");
        }

        private void bwNewKitAutosomalJob_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _host.SetProgress(e.ProgressPercentage);
            _host.SetStatus(e.UserState.ToString());
        }

        private void miClearAllAutosomal_Click(object sender, EventArgs e)
        {
            try {
                dgvAutosomal.Rows.Clear();
            } catch (Exception err) {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void miDeleteRowAutosomal_Click(object sender, EventArgs e)
        {
            try {
                int rowToDelete = dgvAutosomal.Rows.GetFirstRow(DataGridViewElementStates.Selected);
                dgvAutosomal.Rows.RemoveAt(rowToDelete);
                dgvAutosomal.ClearSelection();
            } catch (Exception) { }
        }

        #endregion

        #region Y-DNA

        private void txtYDNA_DragDrop(object sender, DragEventArgs e)
        {
            if (TryGetFilesDrop(e, out string[] filePaths)) {
                _host.SetStatus("Parsing Y-DNA file(s) ...");

                var fileName = filePaths[0];

                Task.Factory.StartNew(() => {
                    try {
                        var snpList = GKGenFuncs.LoadYDNAFile(fileName);
                        ysnps = string.Join(", ", snpList);
                    } catch (Exception) {
                        MessageBox.Show($"Unable to get Y-SNPs from {fileName}");
                    }

                    this.Invoke(new MethodInvoker(delegate {
                        txtYDNA.Text = ysnps;
                        _host.SetStatus("Done.");
                    }));
                });
            }
        }

        private void miDeleteRowYDNAMisc_Click(object sender, EventArgs e)
        {
            try {
                int rowToDelete = dgvYMisc.Rows.GetFirstRow(DataGridViewElementStates.Selected);
                dgvYMisc.Rows.RemoveAt(rowToDelete);
                dgvYMisc.ClearSelection();
            } catch (Exception) { }
        }

        private void miClearAllYMisc_Click(object sender, EventArgs e)
        {
            try {
                dgvYMisc.Rows.Clear();
            } catch (Exception err) {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPasteY_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText()) {
                string stab = "";
                for (int i = 0; i < 111; i++)
                    stab += "\t";
                string[] ystr = (Clipboard.GetText(TextDataFormat.Text) + stab).TrimStart().Split(new char[] { '\t' });

                //YDNA12
                for (int i = 0; i < RefData.ydna12.Length; i++)
                    dgvY12.Rows[i].Cells[1].Value = ystr[i];
                //YDNA25
                for (int i = 0; i < RefData.ydna25.Length; i++)
                    dgvY25.Rows[i].Cells[1].Value = ystr[i + RefData.ydna12.Length];
                //YDNA37
                for (int i = 0; i < RefData.ydna37.Length; i++)
                    dgvY37.Rows[i].Cells[1].Value = ystr[i + RefData.ydna12.Length + RefData.ydna25.Length];
                //YDNA67
                for (int i = 0; i < RefData.ydna67.Length; i++)
                    dgvY67.Rows[i].Cells[1].Value = ystr[i + RefData.ydna12.Length + RefData.ydna25.Length + RefData.ydna37.Length];
                //YDNA111
                for (int i = 0; i < RefData.ydna111.Length; i++)
                    dgvY111.Rows[i].Cells[1].Value = ystr[i + RefData.ydna12.Length + RefData.ydna25.Length + RefData.ydna37.Length + RefData.ydna67.Length];
            }
        }

        private void btnClearY_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < RefData.ydna12.Length; i++)
                dgvY12.Rows[i].Cells[1].Value = "";
            for (int i = 0; i < RefData.ydna25.Length; i++)
                dgvY25.Rows[i].Cells[1].Value = "";
            for (int i = 0; i < RefData.ydna37.Length; i++)
                dgvY37.Rows[i].Cells[1].Value = "";
            for (int i = 0; i < RefData.ydna67.Length; i++)
                dgvY67.Rows[i].Cells[1].Value = "";
            for (int i = 0; i < RefData.ydna111.Length; i++)
                dgvY111.Rows[i].Cells[1].Value = "";

            dgvYMisc.Rows.Clear();
        }

        private void SetupYGrid(DataGridView dgv, string[] markers)
        {
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            dgv.Columns.Clear();
            dgv.AddColumn("marker", "Marker", "", true, true);
            dgv.AddColumn("value", "Value");

            if (markers != null) {
                foreach (string marker in markers)
                    dgv.Rows.Add(new string[] { marker, "" });
            }
        }

        #endregion

        #region Mt-DNA

        private void txtMtDNA_DragDrop(object sender, DragEventArgs e)
        {
            if (TryGetFilesDrop(e, out string[] filePaths)) {
                string fasta_file = filePaths[0];
                try {
                    txtFASTA.Text = File.ReadAllText(fasta_file);
                    txtMtDNA.Text = GKGenFuncs.GetMtDNAMarkers(fasta_file);
                } catch (Exception) {
                    MessageBox.Show("Unable to extract mtDNA mutations from " + fasta_file);
                }
            }
        }

        private void txtFASTA_DragDrop(object sender, DragEventArgs e)
        {
            if (TryGetFilesDrop(e, out string[] filePaths)) {
                string fasta_file = filePaths[0];
                try {
                    txtFASTA.Text = File.ReadAllText(fasta_file);
                    txtMtDNA.Text = GKGenFuncs.GetMtDNAMarkers(fasta_file);
                } catch (Exception) {
                    MessageBox.Show("Unable to extract mtDNA mutations from " + fasta_file);
                }
            }
        }

        #endregion

        #region Saving

        public void Save()
        {
            bool err = false;
            if (txtKit.Text.Trim() == "") {
                errorProvider1.SetError(txtKit, "Must provide a Kit Number. If you don't have one, just enter anything unique to this kit. e.g, KIT01");
                err = true;
            } else
                errorProvider1.SetError(txtKit, "");

            if (txtName.Text.Trim() == "") {
                errorProvider1.SetError(txtName, "Must provide a name.");
                err = true;
            } else
                errorProvider1.SetError(txtName, "");

            if (err)
                return;

            var yRows = Utilities.MergeArrays(true, dgvY12.Rows.GetArray(), dgvY25.Rows.GetArray(), dgvY37.Rows.GetArray(), dgvY67.Rows.GetArray(), dgvY111.Rows.GetArray(), dgvYMisc.Rows.GetArray());
            var ySTRlist = new List<YSTR>(yRows.Length);
            foreach (DataGridViewRow row in yRows) {
                var marker = row.Cells[0].Value.ToString();
                var value = row.Cells[1].Value.ToString();
                if (row.IsNewRow || value.Trim() == "") continue;

                ySTRlist.Add(new YSTR(marker, value));
            }

            object[] args = new object[] {
                txtKit.Text, txtName.Text, dgvAutosomal.DataSource, txtYDNA.Text, txtMtDNA.Text, ySTRlist, cbSex.Text, txtFASTA.Text
            };

            _host.DisableSave();
            _host.SetStatus("Saving ...");
            this.Enabled = false;
            _host.DisableToolbar();
            bwSave.RunWorkerAsync(args);
        }

        private void bwSave_DoWork(object sender, DoWorkEventArgs e)
        {
            saveSuccess = false;
            object[] args = (object[])e.Argument;

            string kit_no = args[0].ToString();
            string name = args[1].ToString();
            var atdna = (List<SNP>)args[2];
            string ysnps_list = args[3].ToString();
            string mutations = args[4].ToString();
            var yRows = (List<YSTR>)args[5];
            string sex = args[6].ToString();
            string fasta = args[7].ToString();

            try {
                string kit_name = GKSqlFuncs.GetKitName(kit_no);
                if (kit_name == "Unknown") {
                    GKSqlFuncs.InsertKit(kit_no, name, sex); // new kit
                } else {
                    GKSqlFuncs.UpdateKit(kit_no, name, sex); // kit exists
                }

                bwSave.ReportProgress(35, "Saving Autosomal data ...");
                GKSqlFuncs.SaveAutosomal(kit_no, atdna);

                if (!string.IsNullOrEmpty(ysnps_list)) {
                    bwSave.ReportProgress(75, "Saving Y-SNPs ...");
                    GKSqlFuncs.SaveYSNPs(kit_no, ysnps_list);
                }

                if (yRows.Count > 0) {
                    bwSave.ReportProgress(80, "Saving Y-STR Values ...");
                    GKSqlFuncs.SaveYSTR(kit_no, yRows);
                }

                if (mutations.Trim() != "" || fasta.Trim() != "") {
                    bwSave.ReportProgress(90, "Saving mtDNA mutations ...");
                    GKSqlFuncs.SaveMtDNA(kit_no, mutations, fasta);
                }

                bwSave.ReportProgress(100, "Saved");
                saveSuccess = true;
            } catch (Exception err) {
                bwSave.ReportProgress(-1, "Not Saved. Tech Details: " + err.Message);
                MessageBox.Show("Not Saved. Techical Details: " + err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bwSave_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _host.SetProgress(e.ProgressPercentage);
            if (e.UserState != null)
                _host.SetStatus(e.UserState.ToString());
        }

        private void bwSave_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _host.SetProgress(-1);
            this.Enabled = true;
            _host.EnableToolbar();
            //if (saveSuccess)
            //this.Close();
        }

        #endregion

        #region Populate

        private void PopulateFields(string kit)
        {
            foreach (Control ctrl in editableCtrls)
                ctrl.Enabled = false;

            _host.SetStatus("Please wait ...");
            bwPopulate.RunWorkerAsync(kit);
        }

        private void bwPopulate_DoWork(object sender, DoWorkEventArgs e)
        {
            string kit = (string)e.Argument;

            // kit master
            var testRec = GKSqlFuncs.GetKit(kit);
            this.Invoke(new MethodInvoker(delegate {
                if (testRec == null) return;
                txtName.Text = testRec.Name;
                if (testRec.Sex == "U")
                    cbSex.SelectedIndex = 0;
                else if (testRec.Sex == "M")
                    cbSex.SelectedIndex = 1;
                else if (testRec.Sex == "F")
                    cbSex.SelectedIndex = 2;
            }));

            // kit autosomal - RSID,Chromosome,Position,Genotypoe
            var dt = GKSqlFuncs.GetAutosomal(kit);
            this.Invoke(new MethodInvoker(delegate {
                dgvAutosomal.DataSource = dt;
            }));

            // kit - ysnp
            ysnps = GKSqlFuncs.GetYSNPs(kit);
            this.Invoke(new MethodInvoker(delegate {
                txtYDNA.Text = ysnps;
            }));

            // kit - ystr
            var ystrList = GKSqlFuncs.GetYSTR(kit);
            Dictionary<string, string> ystr_dict = new Dictionary<string, string>();
            foreach (var str in ystrList) {
                ystr_dict.Add(str.Marker, str.Repeats);
            }
            this.Invoke(new MethodInvoker(delegate {
                PopulateYGrid(RefData.ydna12, ystr_dict, dgvY12);
                PopulateYGrid(RefData.ydna25, ystr_dict, dgvY25);
                PopulateYGrid(RefData.ydna37, ystr_dict, dgvY37);
                PopulateYGrid(RefData.ydna67, ystr_dict, dgvY67);
                PopulateYGrid(RefData.ydna111, ystr_dict, dgvY111);
                dgvYMisc.DataSource = ystrList;
            }));

            // kit - mtdna
            GKSqlFuncs.GetMtDNA(kit, out string mut, out string fasta);
            this.Invoke(new MethodInvoker(delegate {
                txtMtDNA.Text = mut;
                txtFASTA.Text = !string.IsNullOrEmpty(fasta) ? fasta : string.Empty;
            }));
        }

        private void bwPopulate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            foreach (Control ctrl in editableCtrls)
                ctrl.Enabled = true;

            SetControlActivities(kitDisabled);
            _host.SetStatus("Done.");
        }

        private static void PopulateYGrid(string[] ydna, Dictionary<string, string> ystr_dict, DataGridView dgv)
        {
            for (int i = 0; i < ydna.Length; i++) {
                string marker = ydna[i];
                if (ystr_dict.ContainsKey(marker)) {
                    dgv.Rows[i].Cells[1].Value = ystr_dict[marker];
                    ystr_dict.Remove(marker);
                }
            }
        }

        #endregion
    }
}

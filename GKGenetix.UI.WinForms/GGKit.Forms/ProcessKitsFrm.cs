/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using GGKit.Core;
using GKGenetix.Core.Model;

namespace GGKit.Forms
{
    public partial class ProcessKitsFrm : GKWidget
    {
        private string kit1 = null;
        private string kit2 = null;
        private IList<CmpSegment> cmpResults;
        private IList<KitDTO> dt;


        public ProcessKitsFrm(IKitHost host) : base(host)
        {
            InitializeComponent();
        }

        private void ProcessKitsFrm_FormClosing(object sender, EventArgs e)
        {
            if (bwCompare.IsBusy || bwROH.IsBusy || bwPhaseVisualizer.IsBusy) {
                _host.SetStatus("Cancelling...");
                _host.SetProgress(-1);
                btnStart.Text = "Cancelling";
                btnStart.Enabled = false;
                if (bwCompare.IsBusy)
                    bwCompare.CancelAsync();
                if (bwROH.IsBusy)
                    bwROH.CancelAsync();
                if (bwPhaseVisualizer.IsBusy)
                    bwPhaseVisualizer.CancelAsync();
                //e.Cancel = true;
                //this.Close();
            } else {
                _host.SetStatus("Done.");
                _host.SetProgress(-1);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (btnStart.Text == "Start") {
                _host.SetStatus("Processing Kits ...");
                if (bwCompare.IsBusy) {
                    MessageBox.Show("Process is busy!", "Please Wait!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                } else {
                    bwCompare.RunWorkerAsync();
                    btnStart.Text = "Stop";
                }
            } else if (btnStart.Text == "Stop") {
                if (MessageBox.Show("Are you sure you want to cancel the process?", "Cancel?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    _host.SetStatus("Done.");
                    _host.SetProgress(-1);
                    bwCompare.CancelAsync();
                    bwROH.CancelAsync();
                    bwPhaseVisualizer.CancelAsync();
                    btnStart.Text = "Cancelling";
                    btnStart.Enabled = false;
                }
            }
        }

        private void bwCompare_DoWork(object sender, DoWorkEventArgs e)
        {
            var redoAgain = cbDontSkip.Checked;

            if (redoAgain)
                GKSqlFuncs.ClearAllComparisons(false);

            dt = GKSqlFuncs.QueryKits(true);

            int total = 0;
            for (int i = 0; i < dt.Count; i++) {
                for (int j = i; j < dt.Count; j++) {
                    if (dt[i].KitNo != dt[j].KitNo && (dt[i].Reference != 1 || dt[j].Reference != 1))
                        total++;
                }
            }

            int progress = 0;
            int idx = 0;
            for (int i = 0; i < dt.Count; i++) {
                for (int j = i; j < dt.Count; j++) {
                    if (bwCompare.CancellationPending)
                        break;

                    kit1 = dt[i].KitNo;
                    kit2 = dt[j].KitNo;
                    if (kit1 == kit2)
                        continue;

                    int ref1 = dt[i].Reference;
                    int ref2 = dt[j].Reference;
                    if (ref1 == 1 && ref2 == 1)
                        continue;

                    bool reference = (ref1 == 1 || ref2 == 1);

                    string name1 = dt[i].Name;
                    string name2 = dt[j].Name;
                    idx++;

                    if (reference) {
                        WriteStatus($"Comparing Reference {kit1} ({name1}) and {kit2} ({name2})", true);
                    } else {
                        WriteStatus($"Comparing Kits {kit1} ({name1}) and {kit2} ({name2})", true);
                    }

                    progress = idx * 100 / total;

                    cmpResults = GKGenFuncs.CompareOneToOne(kit1, kit2, bwCompare, reference, true);

                    if (bwCompare.CancellationPending)
                        break;

                    if (cmpResults.Count > 0 || redoAgain) {
                        if (!this.IsHandleCreated)
                            break;

                        if (reference)
                            WriteStatus($"{cmpResults.Count} compound segments found.", true);
                        else
                            WriteStatus($"{cmpResults.Count} matching segments found.", true);
                    } else {
                        WriteStatus("Earlier comparison exists. Skipping.", true);
                    }
                    bwCompare.ReportProgress(progress, progress.ToString() + "%");
                }

                if (bwCompare.CancellationPending || !this.IsHandleCreated)
                    break;
            }

            if (!bwCompare.CancellationPending)
                bwCompare.ReportProgress(100, "Done.");
        }

        private void WriteStatus(string msg, bool onlyLocal = false)
        {
            this.Invoke(new MethodInvoker(delegate {
                WriteStatusMsg(msg, onlyLocal);
            }));
        }

        private void WriteStatusMsg(string msg, bool onlyLocal = false)
        {
            if (!onlyLocal) _host.SetStatus(msg);

            lblComparing.Text = msg;

            tbStatus.Text += $"{msg}\r\n";
            tbStatus.Select(tbStatus.Text.Length - 1, 0);
            tbStatus.ScrollToCaret();
        }

        private void bwCompare_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!this.IsHandleCreated)
                return;

            WriteStatusMsg("Comparison Completed.", true);

            bwROH.RunWorkerAsync();
            progressBar.Value = 0;
        }

        private void bwCompare_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _host.SetProgress(e.ProgressPercentage);
            _host.SetStatus(e.UserState.ToString());
            progressBar.Value = e.ProgressPercentage;
        }

        private void bwROH_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < dt.Count; i++) {
                KitDTO row = dt[i];

                if (bwROH.CancellationPending)
                    break;

                string kit = row.KitNo;
                int roh = row.RoH_Status;

                int progress = i * 100 / dt.Count;
                string msg0 = $"Runs of Homozygosity for kit #{kit} ({GKSqlFuncs.GetKitName(kit)})";

                if (roh == 0) {
                    bwROH.ReportProgress(progress, msg0 + " - Processing ...");
                    GKGenFuncs.ROH(kit);
                } else if (roh == 1) {
                    bwROH.ReportProgress(progress, msg0 + " - Already Exists. Skipping..");
                }
            }
        }

        private void bwROH_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string rohStatus = e.UserState.ToString();

            _host.SetProgress(e.ProgressPercentage);
            progressBar.Value = e.ProgressPercentage;

            WriteStatusMsg(rohStatus);
        }

        private void bwROH_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar.Value = 0;
            _host.SetProgress(-1);

            WriteStatusMsg("Runs of Homozygosity Processing Completed.");

            bwPhaseVisualizer.RunWorkerAsync();
        }

        private void bwPhaseVisualizer_DoWork(object sender, DoWorkEventArgs e)
        {
            var redoVisual = cbRedoVisual.Checked;
            GKGenFuncs.DoPhaseVisualizer(redoVisual, bwPhaseVisualizer);
        }

        private void bwPhaseVisualizer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar.Value = 0;
            btnStart.Text = "Start";
            btnStart.Enabled = true;
            _host.SetProgress(-1);

            WriteStatusMsg("Phased Segment Processing Completed.");
        }

        private void bwPhaseVisualizer_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string phStatus = e.UserState.ToString();

            _host.SetProgress(e.ProgressPercentage);
            progressBar.Value = e.ProgressPercentage;

            WriteStatusMsg(phStatus);
        }
    }
}

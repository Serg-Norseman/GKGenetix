/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using GKGenetix.Core;
using GKGenetix.Core.Database;

namespace GKGenetix.UI.Forms
{
    public partial class ProcessKitsFrm : GKWidget
    {
        private IList<TestRecord> dt;
        private bool redoRoH;


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
            _host.EnableExplore();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (btnStart.Text == "Start") {
                _host.SetStatus("Processing Kits ...");
                if (bwCompare.IsBusy) {
                    MessageBox.Show("Process is busy!", "Please Wait!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                } else {
                    _host.DisableExplore();
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
                    _host.EnableExplore();
                }
            }
        }

        private class ProcessItem
        {
            public string Kit1;
            public string Kit2;

            public int Ref1;
            public int Ref2;

            public string Name1;
            public string Name2;
        }

        private void bwCompare_DoWork(object sender, DoWorkEventArgs e)
        {
            var redoAgain = chkDontSkip.Checked;

            if (redoAgain)
                GKSqlFuncs.ClearAllComparisons(false);

            dt = GKSqlFuncs.QueryKits(true);

            var items = new List<ProcessItem>();
            for (int i = 0; i < dt.Count; i++) {
                for (int j = i; j < dt.Count; j++) {
                    if (dt[i].KitNo != dt[j].KitNo && (dt[i].Reference != 1 || dt[j].Reference != 1)) {
                        items.Add(new ProcessItem() {
                            Kit1 = dt[i].KitNo,
                            Ref1 = dt[i].Reference,
                            Name1 = dt[i].Name,
                            Kit2 = dt[j].KitNo,
                            Ref2 = dt[j].Reference,
                            Name2 = dt[j].Name
                        });
                    }
                }
            }

            if (bwCompare.CancellationPending) return;

            for (int i = 0; i < items.Count; i++) {
                var itm = items[i];

                bool reference = (itm.Ref1 == 1 || itm.Ref2 == 1);
                if (reference) {
                    WriteStatus($"Comparing Reference {itm.Kit1} ({itm.Name1}) and {itm.Kit2} ({itm.Name2})", true);
                } else {
                    WriteStatus($"Comparing Kits {itm.Kit1} ({itm.Name1}) and {itm.Kit2} ({itm.Name2})", true);
                }

                var cmpResults = GKGenFuncs.CompareOneToOne(itm.Kit1, itm.Kit2, bwCompare, reference, true);
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

                int progress = i * 100 / items.Count;
                bwCompare.ReportProgress(progress, progress.ToString() + "%");

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

            txtStatus.Text += $"{msg}\r\n";
            txtStatus.Select(txtStatus.Text.Length - 1, 0);
            txtStatus.ScrollToCaret();
        }

        private void bwCompare_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!this.IsHandleCreated)
                return;

            WriteStatusMsg("Comparison Completed.", true);

            redoRoH = chkRedoRoH.Checked;
            bwROH.RunWorkerAsync();
        }

        private void bwCompare_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _host.SetProgress(e.ProgressPercentage);
            _host.SetStatus(e.UserState.ToString());
        }

        private void bwROH_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < dt.Count; i++) {
                if (bwROH.CancellationPending)
                    break;

                TestRecord row = dt[i];
                string kit = row.KitNo;
                int progress = i * 100 / dt.Count;
                string msg0 = $"Runs of Homozygosity for kit #{kit} ({GKSqlFuncs.GetKitName(kit)})";

                if (row.RoH_Status == 1 && !redoRoH) {
                    bwROH.ReportProgress(progress, msg0 + " - Already Exists. Skipping..");
                } else {
                    bwROH.ReportProgress(progress, msg0 + " - Processing ...");
                    GKGenFuncs.ROH(kit, redoRoH);
                }
            }
        }

        private void bwROH_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string rohStatus = e.UserState.ToString();

            _host.SetProgress(e.ProgressPercentage);

            WriteStatusMsg(rohStatus);
        }

        private void bwROH_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _host.SetProgress(-1);

            WriteStatusMsg("Runs of Homozygosity Processing Completed.");

            bwPhaseVisualizer.RunWorkerAsync();
        }

        private void bwPhaseVisualizer_DoWork(object sender, DoWorkEventArgs e)
        {
            GKGenFuncs.DoPhaseVisualizer(false, bwPhaseVisualizer);
        }

        private void bwPhaseVisualizer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnStart.Text = "Start";
            btnStart.Enabled = true;
            _host.SetProgress(-1);

            WriteStatusMsg("Phased Segment Processing Completed.");

            _host.EnableExplore();
        }

        private void bwPhaseVisualizer_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string phStatus = e.UserState.ToString();

            _host.SetProgress(e.ProgressPercentage);

            WriteStatusMsg(phStatus);
        }
    }
}

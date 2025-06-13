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
        private bool redoAgain;
        private bool redoRoH;


        public ProcessKitsFrm(IKitHost host) : base(host)
        {
            InitializeComponent();
        }

        private void ProcessKitsFrm_FormClosing(object sender, EventArgs e)
        {
            if (bwCompare.IsBusy || bwROH.IsBusy) {
                _host.SetStatus("Cancelling...");
                _host.SetProgress(-1);
                btnStart.Text = "Cancelling";
                btnStart.Enabled = false;
                if (bwCompare.IsBusy)
                    bwCompare.CancelAsync();
                if (bwROH.IsBusy)
                    bwROH.CancelAsync();
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
            redoAgain = chkDontSkip.Checked;
            redoRoH = chkRedoRoH.Checked;

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
            if (redoAgain)
                GKSqlFuncs.ClearAllComparisons(false);

            dt = GKSqlFuncs.QueryKits(true);

            var items = new List<ProcessItem>();
            for (int i = 0; i < dt.Count; i++) {
                var dtI = dt[i];

                for (int j = i; j < dt.Count; j++) {
                    var dtJ = dt[j];

                    if (dtI.KitNo != dtJ.KitNo && (dtI.Reference != 1 || dtJ.Reference != 1)) {
                        items.Add(new ProcessItem() {
                            Kit1 = dtI.KitNo,
                            Ref1 = dtI.Reference,
                            Name1 = dtI.Name,
                            Kit2 = dtJ.KitNo,
                            Ref2 = dtJ.Reference,
                            Name2 = dtJ.Name
                        });
                    }
                }
            }

            for (int i = 0; i < items.Count; i++) {
                if (bwCompare.CancellationPending || !this.IsHandleCreated)
                    break;

                var itm = items[i];

                bool reference = (itm.Ref1 == 1 || itm.Ref2 == 1);
                if (reference) {
                    WriteStatus($"Comparing Reference {itm.Kit1} ({itm.Name1}) and {itm.Kit2} ({itm.Name2})", -2, true);
                } else {
                    WriteStatus($"Comparing Kits {itm.Kit1} ({itm.Name1}) and {itm.Kit2} ({itm.Name2})", -2, true);
                }

                var cmpResults = GKGenFuncs.CompareOneToOne(itm.Kit1, itm.Kit2, bwCompare, reference, redoAgain);
                int progress = i * 100 / items.Count;

                if (cmpResults.Count > 0 || redoAgain) {
                    if (!this.IsHandleCreated)
                        break;

                    if (reference)
                        WriteStatus($"{cmpResults.Count} compound segments found.", progress, true);
                    else
                        WriteStatus($"{cmpResults.Count} matching segments found.", progress, true);
                } else {
                    WriteStatus("Earlier comparison exists. Skipping.", progress, true);
                }
            }

            if (!bwCompare.CancellationPending)
                bwCompare.ReportProgress(100, "Done.");
        }

        private void WriteStatus(string msg, int progress, bool onlyLocal = false)
        {
            this.Invoke(new MethodInvoker(delegate {
                if (progress >= -1) {
                    _host.SetProgress(progress);
                    _host.SetStatus($"{progress}%");
                }

                if (!onlyLocal) _host.SetStatus(msg);

                txtStatus.Text += $"{msg}\r\n";
                txtStatus.Select(txtStatus.Text.Length - 1, 0);
                txtStatus.ScrollToCaret();
            }));
        }

        private void bwCompare_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!this.IsHandleCreated)
                return;

            WriteStatus("Comparison Completed.", -1, true);

            bwROH.RunWorkerAsync();
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
            WriteStatus(rohStatus, e.ProgressPercentage);
        }

        private void bwROH_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            WriteStatus("Runs of Homozygosity Processing Completed.", -1);
        }
    }
}

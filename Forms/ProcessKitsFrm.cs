/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using GenetixKit.Core;
using GenetixKit.Core.Model;

namespace GenetixKit.Forms
{
    public partial class ProcessKitsFrm : Form
    {
        private string kit1 = null;
        private string kit2 = null;
        private IList<CmpSegment> cmpResults;
        private bool redoAgain = false;
        private bool redoVisual = false;
        private IList<KitDTO> dt;

        public ProcessKitsFrm()
        {
            InitializeComponent();
        }

        private void bwCompare_DoWork(object sender, DoWorkEventArgs e)
        {
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

                    this.Invoke(new MethodInvoker(delegate {
                        if (reference) {
                            lblComparing.Text = "Comparing Reference " + kit1 + " (" + name1 + ") and " + kit2 + " (" + name2 + ") ... " + progress.ToString() + "%";
                            tbStatus.Text += ("Comparing Reference " + kit1 + " (" + name1 + ") and " + kit2 + " (" + name2 + "): ");
                        } else {
                            lblComparing.Text = "Comparing Kits " + kit1 + " (" + name1 + ") and " + kit2 + " (" + name2 + ") ... " + progress.ToString() + "%";
                            tbStatus.Text += ("Comparing Kits " + kit1 + " (" + name1 + ") and " + kit2 + " (" + name2 + "): ");
                        }
                        tbStatus.Select(tbStatus.Text.Length - 1, 0);
                        tbStatus.ScrollToCaret();
                    }));

                    progress = idx * 100 / total;

                    cmpResults = GKGenFuncs.CompareOneToOne(kit1, kit2, bwCompare, reference, true);

                    if (bwCompare.CancellationPending)
                        break;

                    if (cmpResults.Count > 0 || redoAgain) {
                        if (!this.IsHandleCreated)
                            break;

                        this.Invoke(new MethodInvoker(delegate {
                            if (reference)
                                tbStatus.Text += cmpResults.Count.ToString() + " compound segments found.\r\n";
                            else
                                tbStatus.Text += cmpResults.Count.ToString() + " matching segments found.\r\n";
                            tbStatus.Select(tbStatus.Text.Length - 1, 0);
                            tbStatus.ScrollToCaret();
                        }));
                    } else {
                        this.Invoke(new MethodInvoker(delegate {
                            tbStatus.Text += "Earlier comparison exists. Skipping.\r\n";
                            tbStatus.Select(tbStatus.Text.Length - 1, 0);
                            tbStatus.ScrollToCaret();
                        }));
                    }
                    bwCompare.ReportProgress(progress, progress.ToString() + "%");
                }

                if (bwCompare.CancellationPending || !this.IsHandleCreated)
                    break;
            }

            if (!bwCompare.CancellationPending)
                bwCompare.ReportProgress(100, "Done.");
        }

        private void bwCompare_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!this.IsHandleCreated)
                return;
            lblComparing.Text = "Comparison Completed.";
            tbStatus.Text += "Comparison Completed.\r\n";
            tbStatus.Select(tbStatus.Text.Length - 1, 0);
            tbStatus.ScrollToCaret();

            bwROH.RunWorkerAsync();
            progressBar.Value = 0;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (btnStart.Text == "Start") {
                redoAgain = cbDontSkip.Checked;
                redoVisual = cbRedoVisual.Checked;
                Program.KitInstance.SetStatus("Processing Kits ...");
                if (bwCompare.IsBusy) {
                    MessageBox.Show("Process is busy!", "Please Wait!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                } else {
                    bwCompare.RunWorkerAsync();
                    btnStart.Text = "Stop";
                }
            } else if (btnStart.Text == "Stop") {
                if (MessageBox.Show("Are you sure you want to cancel the process?", "Cancel?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    Program.KitInstance.SetStatus("Done.");
                    Program.KitInstance.SetProgress(-1);
                    bwCompare.CancelAsync();
                    bwROH.CancelAsync();
                    bwPhaseVisualizer.CancelAsync();
                    btnStart.Text = "Cancelling";
                    btnStart.Enabled = false;
                }
            }
        }

        private void bwCompare_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Program.KitInstance.SetProgress(e.ProgressPercentage);
            Program.KitInstance.SetStatus(e.UserState.ToString());
            progressBar.Value = e.ProgressPercentage;
        }

        private void ProcessKitsFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bwCompare.IsBusy || bwROH.IsBusy || bwPhaseVisualizer.IsBusy) {
                Program.KitInstance.SetStatus("Cancelling...");
                Program.KitInstance.SetProgress(-1);
                btnStart.Text = "Cancelling";
                btnStart.Enabled = false;
                if (bwCompare.IsBusy)
                    bwCompare.CancelAsync();
                if (bwROH.IsBusy)
                    bwROH.CancelAsync();
                if (bwPhaseVisualizer.IsBusy)
                    bwPhaseVisualizer.CancelAsync();
                e.Cancel = true;
                this.Close();
            } else {
                Program.KitInstance.SetStatus("Done.");
                Program.KitInstance.SetProgress(-1);
            }
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
            lblComparing.Text = e.UserState.ToString();
            Program.KitInstance.SetProgress(e.ProgressPercentage);
            Program.KitInstance.SetStatus(e.UserState.ToString());
            progressBar.Value = e.ProgressPercentage;

            lblComparing.Text = e.UserState.ToString();
            tbStatus.Text += e.UserState.ToString() + "\r\n";
            tbStatus.Select(tbStatus.Text.Length - 1, 0);
            tbStatus.ScrollToCaret();
        }

        private void bwROH_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar.Value = 0;
            Program.KitInstance.SetProgress(-1);
            Program.KitInstance.SetStatus("Runs of Homozygosity Processing Completed.");
            lblComparing.Text = "Processing Completed.";
            tbStatus.Text += "Runs of Homozygosity Processing Completed.\r\n";
            tbStatus.Select(tbStatus.Text.Length - 1, 0);
            tbStatus.ScrollToCaret();

            bwPhaseVisualizer.RunWorkerAsync();
        }

        private void bwPhaseVisualizer_DoWork(object sender, DoWorkEventArgs e)
        {
            var phasedKits = GKSqlFuncs.GetPhasedKits();
            for (int i = 0; i < phasedKits.Count; i++) {
                string phased_kit = phasedKits[i];

                if (bwPhaseVisualizer.CancellationPending)
                    break;

                int percent = i * 100 / phasedKits.Count;
                bwPhaseVisualizer.ReportProgress(percent, $"Phased Segments for kit #{phased_kit} ({GKSqlFuncs.GetKitName(phased_kit)}) - Processing ...");

                var unphasedSegments = GKSqlFuncs.GetUnphasedSegments(phased_kit);

                foreach (var unphSeg in unphasedSegments) {
                    if (bwPhaseVisualizer.CancellationPending)
                        break;

                    string unphased_kit = unphSeg.UnphasedKit;
                    string chromosome = unphSeg.Chromosome;
                    string start_position = unphSeg.StartPosition.ToString();
                    string end_position = unphSeg.EndPosition.ToString();

                    var exists = GKSqlFuncs.QueryValue(
                        $"select phased_kit from cmp_phased where phased_kit='{phased_kit}' and match_kit='{unphased_kit}' and chromosome='{chromosome}' and start_position={start_position} and end_position={end_position}");

                    if (!string.IsNullOrEmpty(exists)) {
                        //already exists...
                        if (!redoVisual) {
                            bwPhaseVisualizer.ReportProgress(percent, "Segment [" + GKSqlFuncs.GetKitName(phased_kit) + ":" + GKSqlFuncs.GetKitName(unphased_kit) + "] Chr " + chromosome + ": " + start_position + "-" + end_position + ", Already Processed. Skipping ...");
                            continue;
                        } else {
                            GKSqlFuncs.DeletePhasedKit(phased_kit);
                        }
                    }

                    bwPhaseVisualizer.ReportProgress(percent, "Segment [" + GKSqlFuncs.GetKitName(phased_kit) + ":" + GKSqlFuncs.GetKitName(unphased_kit) + "] Chr " + chromosome + ": " + start_position + "-" + end_position + ", Processing ...");

                    /*var dt = GGKSqlFuncs.GetPhaseSegments(unphased_kit, start_position, end_position, chromosome, phased_kit);
                    if (dt.Count > 0) {
                        if (bwPhaseVisualizer.CancellationPending)
                            break;

                        Image img = GGKGenFuncs.GetPhasedSegmentImage(dt, chromosome);
                    }*/
                }
            }
        }

        private void bwPhaseVisualizer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar.Value = 0;
            btnStart.Text = "Start";
            btnStart.Enabled = true;
            Program.KitInstance.SetProgress(-1);
            Program.KitInstance.SetStatus("Phased Segment Processing Completed.");
            lblComparing.Text = "Processing Completed.";
            tbStatus.Text += "Phased Segment Processing Completed.\r\n";
            tbStatus.Select(tbStatus.Text.Length - 1, 0);
            tbStatus.ScrollToCaret();
        }

        private void bwPhaseVisualizer_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblComparing.Text = e.UserState.ToString();
            Program.KitInstance.SetProgress(e.ProgressPercentage);
            Program.KitInstance.SetStatus(e.UserState.ToString());
            progressBar.Value = e.ProgressPercentage;

            lblComparing.Text = e.UserState.ToString();
            tbStatus.Text += e.UserState.ToString() + "\r\n";
            tbStatus.Select(tbStatus.Text.Length - 1, 0);
            tbStatus.ScrollToCaret();
        }
    }
}

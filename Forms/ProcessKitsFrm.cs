/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        public ProcessKitsFrm()
        {
            InitializeComponent();
        }

        private void bwCompare_DoWork(object sender, DoWorkEventArgs e)
        {
            if (redoAgain)
                GKSqlFuncs.ClearAllComparisons(false);

            DataTable dt = GKSqlFuncs.QueryTable("kit_master", new string[] { "kit_no", "reference", "name" }, "where disabled=0");

            int progress = 0;
            int total = 0;
            string ref1 = "0";
            string ref2 = "0";

            for (int i = 0; i < dt.Rows.Count; i++) {
                for (int j = i; j < dt.Rows.Count; j++) {
                    if (bwCompare.CancellationPending)
                        break;

                    kit1 = dt.Rows[i].ItemArray[0].ToString();
                    kit2 = dt.Rows[j].ItemArray[0].ToString();
                    if (kit1 == kit2)
                        continue;

                    ref1 = dt.Rows[i].ItemArray[1].ToString();
                    ref2 = dt.Rows[j].ItemArray[1].ToString();

                    if (ref1 == "1" && ref2 == "1")
                        continue;
                    total++;
                }

                if (bwCompare.CancellationPending)
                    break;
            }

            int idx = 0;
            string name1 = "";
            string name2 = "";
            bool reference = false;
            for (int i = 0; i < dt.Rows.Count; i++) {
                for (int j = i; j < dt.Rows.Count; j++) {
                    if (bwCompare.CancellationPending)
                        break;

                    kit1 = dt.Rows[i].ItemArray[0].ToString();
                    kit2 = dt.Rows[j].ItemArray[0].ToString();
                    if (kit1 == kit2)
                        continue;

                    ref1 = dt.Rows[i].ItemArray[1].ToString();
                    ref2 = dt.Rows[j].ItemArray[1].ToString();

                    if (ref1 == "1" && ref2 == "1")
                        continue;

                    if (ref1 == "1" || ref2 == "1")
                        reference = true;
                    else
                        reference = false;

                    name1 = dt.Rows[i].ItemArray[2].ToString();
                    name2 = dt.Rows[j].ItemArray[2].ToString();
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
            DataTable dt = GKSqlFuncs.QueryTable("select kit_no,roh_status from kit_master where disabled=0");

            foreach (DataRow row in dt.Rows) {
                if (bwROH.CancellationPending)
                    break;

                string kit = row.ItemArray[0].ToString();
                string roh = row.ItemArray[1].ToString();

                if (roh == "0") {
                    bwROH.ReportProgress(dt.Rows.IndexOf(row) * 100 / dt.Rows.Count, "Runs of Homozygosity for kit #" + kit + " (" + GKSqlFuncs.GetKitName(kit) + ") - Processing ...");
                    GKGenFuncs.ROH(kit);
                } else if (roh == "1") {
                    bwROH.ReportProgress(dt.Rows.IndexOf(row) * 100 / dt.Rows.Count, "Runs of Homozygosity for kit #" + kit + " (" + GKSqlFuncs.GetKitName(kit) + ") - Already Exists. Skipping..");
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
            DataTable dt2 = GKSqlFuncs.QueryTable("select distinct kit_no from kit_phased");
            foreach (DataRow row in dt2.Rows) {
                if (bwPhaseVisualizer.CancellationPending)
                    break;

                string phased_kit = row.ItemArray[0].ToString();

                int percent = dt2.Rows.IndexOf(row) * 100 / dt2.Rows.Count;
                bwPhaseVisualizer.ReportProgress(percent, "Phased Segments for kit #" + phased_kit + " (" + GKSqlFuncs.GetKitName(phased_kit) + ") - Processing ...");

                DataTable dt3 = GKSqlFuncs.QueryTable("select unphased_kit,chromosome,start_position,end_position FROM (select kit1'unphased_kit',chromosome,start_position,end_position from cmp_autosomal where kit2='" + phased_kit + "' UNION select kit2'unphased_kit',chromosome,start_position,end_position from cmp_autosomal where kit1='" + phased_kit + "') order by cast(chromosome as integer),start_position");

                foreach (DataRow row3 in dt3.Rows) {
                    if (bwPhaseVisualizer.CancellationPending)
                        break;

                    string unphased_kit = row3.ItemArray[0].ToString();
                    string chromosome = row3.ItemArray[1].ToString();
                    string start_position = row3.ItemArray[2].ToString();
                    string end_position = row3.ItemArray[3].ToString();

                    DataTable exists = GKSqlFuncs.QueryTable("select * from cmp_phased where phased_kit='" + phased_kit + "' and match_kit='" + unphased_kit + "' and chromosome='" + chromosome + "' and start_position=" + start_position + " and end_position=" + end_position);

                    if (exists.Rows.Count > 0) {
                        //already exists...
                        if (!redoVisual) {
                            bwPhaseVisualizer.ReportProgress(percent, "Segment [" + GKSqlFuncs.GetKitName(phased_kit) + ":" + GKSqlFuncs.GetKitName(unphased_kit) + "] Chr " + chromosome + ": " + start_position + "-" + end_position + ", Already Processed. Skipping ...");
                            continue;
                        } else {
                            GKSqlFuncs.UpdateDB("DELETE from cmp_phased where phased_kit='{0}'", phased_kit);
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

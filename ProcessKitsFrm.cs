using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Genetic_Genealogy_Kit
{
    public partial class ProcessKitsFrm : Form
    {
        string kit1 = null;
        string kit2 = null;

        DataTable segment_idx = new DataTable();
        List<DataTable> segments = new List<DataTable>();        

        bool redo_again = false;
        bool no_admixture = false;
        bool redo_visual = false;

        public ProcessKitsFrm()
        {
            InitializeComponent();
        }

        private void bwCompare_DoWork(object sender, DoWorkEventArgs e)
        {

            if (redo_again)
                GGKUtilLib.clearAllComparisions(no_admixture);

            DataTable dt = null;

            if(no_admixture)
                dt = GGKUtilLib.queryDatabase("kit_master", new string[] { "kit_no", "reference", "name" }, "where disabled=0 and reference=0");
            else
                dt = GGKUtilLib.queryDatabase("kit_master", new string[] { "kit_no", "reference" ,"name"}, "where disabled=0");

            int progress = 0;
            int total = 0;
            string ref1 = "0";
            string ref2 = "0";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = i; j < dt.Rows.Count; j++)
                {
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
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = i; j < dt.Rows.Count; j++)
                {
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

                    if(ref1=="1"||ref2=="1")
                        reference = true;
                    else
                        reference = false;

                    name1 = dt.Rows[i].ItemArray[2].ToString();
                    name2 = dt.Rows[j].ItemArray[2].ToString();
                    idx++;
                   
                    this.Invoke(new MethodInvoker(delegate
                    {
                        
                        if (reference)
                        {
                            lblComparing.Text = "Comparing Reference " + kit1 + " (" + name1 + ") and " + kit2 + " (" + name2 + ") ... " + progress.ToString() + "%";
                            tbStatus.Text += ("Comparing Reference " + kit1 + " (" + name1 + ") and " + kit2 + " (" + name2 + "): ");
                        }
                        else
                        {
                            lblComparing.Text = "Comparing Kits " + kit1 + " (" + name1 + ") and " + kit2 + " (" + name2 + ") ... " + progress.ToString() + "%";
                            tbStatus.Text += ("Comparing Kits " + kit1 + " (" + name1 + ") and " + kit2 + " (" + name2 + "): ");
                        }
                        tbStatus.Select(tbStatus.Text.Length - 1, 0);
                        tbStatus.ScrollToCaret();                        
                    }));
                    progress = idx * 100 / total;

                    object[] cmp_results = GGKUtilLib.compareOneToOne(kit1, kit2, bwCompare,reference,true);

                    if (bwCompare.CancellationPending)
                        break;

                    segment_idx = (DataTable)cmp_results[0];
                    segments = (List<DataTable>)cmp_results[1];
                    if (segment_idx.Rows.Count>0)
                    {
                        if (!this.IsHandleCreated)
                            break;

                        this.Invoke(new MethodInvoker(delegate
                        {
                            if(reference)
                                tbStatus.Text += segment_idx.Rows.Count.ToString() + " compound segments found.\r\n";
                            else
                                tbStatus.Text += segment_idx.Rows.Count.ToString() + " matching segments found.\r\n";
                            tbStatus.Select(tbStatus.Text.Length - 1, 0);
                            tbStatus.ScrollToCaret();
                        }));
                    }
                    else
                    {
                        this.Invoke(new MethodInvoker(delegate
                        {
                            tbStatus.Text += "Earlier comparision exists, Skipping.\r\n";
                            tbStatus.Select(tbStatus.Text.Length - 1, 0);
                            tbStatus.ScrollToCaret();
                        }));
                    }
                    bwCompare.ReportProgress(progress, progress.ToString() + "%");
                }
                if (bwCompare.CancellationPending)
                    break;
                if (!this.IsHandleCreated)
                    break;
            }
            if (!bwCompare.CancellationPending)
                bwCompare.ReportProgress(100, "Done.");
        }

        private void bwCompare_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!this.IsHandleCreated)            
                return;
            lblComparing.Text = "Comparision Completed.";
            tbStatus.Text += "Comparision Completed.\r\n";
            tbStatus.Select(tbStatus.Text.Length - 1, 0);
            tbStatus.ScrollToCaret();
            //
            bwROH.RunWorkerAsync();
            progressBar.Value = 0;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (btnStart.Text == "Start")
            {
                redo_again = cbDontSkip.Checked;
                redo_visual = cbRedoVisual.Checked;
                no_admixture = cbNoAdmixture.Checked;
                GGKUtilLib.setStatus("Processing Kits ...");
                if (bwCompare.IsBusy)
                {
                    MessageBox.Show("Process is busy!","Please Wait!",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                }
                else
                {
                    bwCompare.RunWorkerAsync();
                    btnStart.Text = "Stop";
                }
            }
            else if (btnStart.Text == "Stop")
            {
                if (MessageBox.Show("Are you sure you want to cancel the process?", "Cancel?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    GGKUtilLib.setStatus("Done.");
                    GGKUtilLib.setProgress(-1);
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
            GGKUtilLib.setProgress(e.ProgressPercentage);
            GGKUtilLib.setStatus(e.UserState.ToString());
            progressBar.Value = e.ProgressPercentage;
        }

        private void ProcessKitsFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            if (bwCompare.IsBusy||bwROH.IsBusy||bwPhaseVisualizer.IsBusy)
            {
                GGKUtilLib.setStatus("Cancelling...");
                GGKUtilLib.setProgress(-1);
                btnStart.Text = "Cancelling";
                btnStart.Enabled = false;
                if(bwCompare.IsBusy)
                    bwCompare.CancelAsync();
                if (bwROH.IsBusy)
                    bwROH.CancelAsync();
                if(bwPhaseVisualizer.IsBusy)
                    bwPhaseVisualizer.CancelAsync();
                e.Cancel = true;
                timer1.Enabled = true;
            }
            else
            {
                GGKUtilLib.setStatus("Done.");
                GGKUtilLib.setProgress(-1);                
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {            
             this.Close();
        }

        private void bwROH_DoWork(object sender, DoWorkEventArgs e)
        {
            DataTable dt = GGKUtilLib.QueryDB("select kit_no,roh_status from kit_master where reference=0 and disabled=0");
            string kit = null;
            string roh = null;
            foreach(DataRow row in dt.Rows)
            {
                if (bwROH.CancellationPending)
                    break;
                kit = row.ItemArray[0].ToString();
                roh = row.ItemArray[1].ToString();
                if (roh == "0")
                {
                    bwROH.ReportProgress(dt.Rows.IndexOf(row) * 100 / dt.Rows.Count, "Runs of Homozygosity for kit #" + kit + " (" + GGKUtilLib.getKitName(kit) + ") - Processing ...");
                    GGKUtilLib.ROH(kit);
                }
                else if (roh == "1")
                {
                    bwROH.ReportProgress(dt.Rows.IndexOf(row) * 100 / dt.Rows.Count, "Runs of Homozygosity for kit #" + kit + " (" + GGKUtilLib.getKitName(kit) + ") - Already Exists. Skipping..");
                }
            }
            
        }

        private void bwROH_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblComparing.Text = e.UserState.ToString();
            GGKUtilLib.setProgress(e.ProgressPercentage);
            GGKUtilLib.setStatus(e.UserState.ToString());
            progressBar.Value = e.ProgressPercentage;

            lblComparing.Text = e.UserState.ToString();
            tbStatus.Text += e.UserState.ToString()+"\r\n";
            tbStatus.Select(tbStatus.Text.Length - 1, 0);
            tbStatus.ScrollToCaret();
        }

        private void bwROH_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar.Value = 0;
            GGKUtilLib.setProgress(-1);
            GGKUtilLib.setStatus("Runs of Homozygosity Processing Completed.");
            lblComparing.Text = "Processing Completed.";
            tbStatus.Text += "Runs of Homozygosity Processing Completed.\r\n";
            tbStatus.Select(tbStatus.Text.Length - 1, 0);
            tbStatus.ScrollToCaret();

            bwPhaseVisualizer.RunWorkerAsync();
        }

        private void bwPhaseVisualizer_DoWork(object sender, DoWorkEventArgs e)
        {
            DataTable dt2 = GGKUtilLib.QueryDB("select distinct kit_no from kit_phased");
            string phased_kit = null;
            string unphased_kit = null;
            string chromosome = null;
            string start_position = null;
            string end_position = null;
            int percent = 0;
            foreach (DataRow row in dt2.Rows)
            {

                if (bwPhaseVisualizer.CancellationPending)
                    break;
                phased_kit = row.ItemArray[0].ToString();
                percent = dt2.Rows.IndexOf(row) * 100 / dt2.Rows.Count;
                bwPhaseVisualizer.ReportProgress(percent, "Phased Segments for kit #" + phased_kit + " (" + GGKUtilLib.getKitName(phased_kit) + ") - Processing ...");

                DataTable dt3 = GGKUtilLib.QueryDB("select unphased_kit,chromosome,start_position,end_position FROM (select kit1'unphased_kit',chromosome,start_position,end_position from cmp_autosomal where kit2='" + phased_kit + "' UNION select kit2'unphased_kit',chromosome,start_position,end_position from cmp_autosomal where kit1='" + phased_kit + "') order by cast(chromosome as integer),start_position");

                foreach (DataRow row3 in dt3.Rows)
                {
                    if (bwPhaseVisualizer.CancellationPending)
                        break;
                    unphased_kit = row3.ItemArray[0].ToString();
                    chromosome = row3.ItemArray[1].ToString();
                    start_position = row3.ItemArray[2].ToString();
                    end_position = row3.ItemArray[3].ToString();

                    DataTable exists=GGKUtilLib.QueryDB("select * from cmp_phased where phased_kit='"+phased_kit+"' and match_kit='"+unphased_kit+"' and chromosome='"+chromosome+"' and start_position="+start_position+" and end_position="+end_position);

                    if (exists.Rows.Count > 0)
                    {
                        //already exists...
                        if (!redo_visual)
                        {
                            bwPhaseVisualizer.ReportProgress(percent, "Segment [" + GGKUtilLib.getKitName(phased_kit) + ":" + GGKUtilLib.getKitName(unphased_kit) + "] Chr " + chromosome + ": " + start_position + "-" + end_position + ", Already Processed. Skipping ...");                        
                            continue;
                        }
                        else
                        {
                            GGKUtilLib.UpdateDB("DELETE from cmp_phased where phased_kit='"+phased_kit+"'");
                        }
                    }
                    
                    bwPhaseVisualizer.ReportProgress(percent, "Segment [" + GGKUtilLib.getKitName(phased_kit) + ":" + GGKUtilLib.getKitName(unphased_kit) + "] Chr " + chromosome + ": " + start_position + "-" + end_position + ", Processing ...");
                    

                    DataTable dt = GGKUtilLib.QueryDB("select a.position,a.genotype,p.paternal_genotype,p.maternal_genotype from kit_autosomal a,kit_phased p where a.kit_no='" + unphased_kit + "' and a.position>" + start_position + " and a.position<" + end_position + " and a.chromosome='" + chromosome + "' and p.rsid=a.rsid and p.kit_no='" + phased_kit + "' order by a.position");
                    if (dt.Rows.Count > 0)
                    {
                        if (bwPhaseVisualizer.CancellationPending)
                            break;

                        Image img = GGKUtilLib.getPhasedSegmentImage(dt, chromosome);

                        dt.TableName = "cmp_phased";
                        StringBuilder sb = new StringBuilder();
                        StringWriter w = new StringWriter(sb);
                        dt.WriteXml(w, XmlWriteMode.WriteSchema);
                        string segment_xml = sb.ToString();

                        SQLiteConnection conn = GGKUtilLib.getDBConnection();
                        SQLiteCommand cmd = new SQLiteCommand("INSERT INTO cmp_phased(phased_kit,match_kit,chromosome,start_position,end_position,segment_image,segment_xml) VALUES (@phased_kit,@match_kit,@chromosome,@start_position,@end_position,@segment_image,@segment_xml)", conn);
                        cmd.Parameters.AddWithValue("@phased_kit", phased_kit);
                        cmd.Parameters.AddWithValue("@match_kit", unphased_kit);
                        cmd.Parameters.AddWithValue("@chromosome", chromosome);
                        cmd.Parameters.AddWithValue("@start_position", start_position);
                        cmd.Parameters.AddWithValue("@end_position", end_position);
                        byte[] image_bytes = GGKUtilLib.imageToByteArray(img);
                        cmd.Parameters.Add("@segment_image", DbType.Binary, image_bytes.Length).Value = image_bytes;
                        cmd.Parameters.AddWithValue("@segment_xml", segment_xml);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
        }

        private void bwPhaseVisualizer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar.Value = 0;
            btnStart.Text = "Start";
            btnStart.Enabled = true;
            GGKUtilLib.setProgress(-1);
            GGKUtilLib.setStatus("Phased Segment Processing Completed.");
            lblComparing.Text = "Processing Completed.";
            tbStatus.Text += "Phased Segment Processing Completed.\r\n";
            tbStatus.Select(tbStatus.Text.Length - 1, 0);
            tbStatus.ScrollToCaret();
        }

        private void bwPhaseVisualizer_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblComparing.Text = e.UserState.ToString();
            GGKUtilLib.setProgress(e.ProgressPercentage);
            GGKUtilLib.setStatus(e.UserState.ToString());
            progressBar.Value = e.ProgressPercentage;

            lblComparing.Text = e.UserState.ToString();
            tbStatus.Text += e.UserState.ToString() + "\r\n";
            tbStatus.Select(tbStatus.Text.Length - 1, 0);
            tbStatus.ScrollToCaret();
        }
    }
}

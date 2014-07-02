using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Genetic_Genealogy_Kit
{
    public partial class NewEditKitFrm : Form
    {

        string[] ydna12 = new string[] { "DYS393", "DYS390", "DYS19", "DYS391", "DYS385", "DYS426", "DYS388", "DYS439", "DYS389I", "DYS392", "DYS389II" };
        string[] ydna25 = new string[] { "DYS458", "DYS459", "DYS455", "DYS454", "DYS447", "DYS437", "DYS448", "DYS449", "DYS464" };
        string[] ydna37 = new string[] { "DYS460", "Y-GATA-H4", "YCAII", "DYS456", "DYS607", "DYS576", "DYS570", "CDY", "DYS442", "DYS438" };
        string[] ydna67 = new string[] { "DYS531", "DYS578", "DYF395S1", "DYS590", "DYS537", "DYS641", "DYS472", "DYF406S1", "DYS511", "DYS425", "DYS413", "DYS557", "DYS594", "DYS436", "DYS490", "DYS534", "DYS450", "DYS444", "DYS481", "DYS520", "DYS446", "DYS617", "DYS568", "DYS487", "DYS572", "DYS640", "DYS492", "DYS565" };
        string[] ydna111 = new string[] { "DYS710", "DYS485", "DYS632", "DYS495", "DYS540", "DYS714", "DYS716", "DYS717", "DYS505", "DYS556", "DYS549", "DYS589", "DYS522", "DYS494", "DYS533", "DYS636", "DYS575", "DYS638", "DYS462", "DYS452", "DYS445", "Y-GATA-A10", "DYS463", "DYS441", "Y-GGAAT-1B07", "DYS525", "DYS712", "DYS593", "DYS650", "DYS532", "DYS715", "DYS504", "DYS513", "DYS561", "DYS552", "DYS726", "DYS635", "DYS587", "DYS643", "DYS497", "DYS510", "DYS434", "DYS461", "DYS435" };

        bool save_success = false;        

        Dictionary<string, string[]> ymap = GGKUtilLib.getYMap();
        string ysnps = null;
        string mtdna = null;
        bool kit_disabled = false;
        Control[] editableCtrls = null;
        public NewEditKitFrm(string kit,bool disabled)
        {
            InitializeComponent();
            editableCtrls = new Control[] {tbFASTA, txtName, cbSex, dataGridViewAutosomal, textBoxYDNA, dgvy12, dgvy25, dgvy37, dgvy67, dgvy111, dgvymisc, textBoxMtDNA, btnPaste, btnClear };
            if (kit == null)
            {
                this.Text = "New Kit";
                txtKit.Enabled = true;
                GGKUtilLib.enableSave();
            }
            else
            {
                kit_disabled = disabled;
                this.Text = "Edit Kit";
                txtKit.Text = kit;
                txtKit.Enabled = false;
                populateFields(kit);                
            }
        }

        private void dataGridViewAutosomal_DragDrop(object sender, DragEventArgs e)
        {
            // autosomal file dropped.
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] filePaths = (string[])(e.Data.GetData(DataFormats.FileDrop));
                GGKUtilLib.setStatus("Parsing autosomal file(s) ...");
                if (bwNewKitAutosomalJob.IsBusy)                
                    bwNewKitAutosomalJob.CancelAsync();                                   
                bwNewKitAutosomalJob.RunWorkerAsync(filePaths);
            }
        }

        private void dataGridViewAutosomal_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void bwNewKitAutosomalJob_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add("RSID");
                table.Columns.Add("Chromosome");
                table.Columns.Add("Position");
                table.Columns.Add("Genotype");


                string[] filePaths = (string[])e.Argument;

                foreach (string file_path in filePaths)
                {
                    Object[] dnaout = GGKUtilLib.getAutosomalDNAList(file_path);
                    ArrayList rows = (ArrayList)dnaout[0];
                    List<string> ysnps_arr = (List<string>)dnaout[1];
                    ArrayList mtdna_arr = (ArrayList)dnaout[2];
                    bwNewKitAutosomalJob.ReportProgress(-1, rows.Count.ToString() + " SNPs found in "+Path.GetFileName(file_path));
                    int count = 0;
                    int percent = 0;
                    int ppercent = 0;

                    foreach (string[] row in rows)
                    {
                        percent = (count * 100) / rows.Count;
                        if (ppercent != percent)
                        {
                            bwNewKitAutosomalJob.ReportProgress(percent, " " + percent.ToString() + "%");
                            ppercent = percent;
                        }

                        table.Rows.Add(row);
                        count++;
                    }

                    //
                    if (ysnps_arr.Count != 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        ysnps_arr = ysnps_arr.Distinct().ToList();
                        foreach (string snp in ysnps_arr)
                        {
                            sb.Append(snp + ", ");
                        }
                        if(ysnps == null)
                            ysnps = sb.ToString().Trim();
                        else
                            ysnps = ysnps + sb.ToString().Trim();
                        if (ysnps.Length > 0)
                            ysnps = ysnps.Substring(0, ysnps.Length - 1);
                    }

                    if (mtdna_arr.Count != 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (string mut in mtdna_arr)
                        {
                            sb.Append(mut + ", ");
                        }
                       
                        if (mtdna == null)
                            mtdna = sb.ToString().Trim();
                        else
                            mtdna = mtdna + sb.ToString().Trim();
                        if (mtdna.Length > 0)
                            mtdna = mtdna.Substring(0, mtdna.Length - 1);
                    }
                }


                this.Invoke(new MethodInvoker(delegate
                {
                    dataGridViewAutosomal.Columns.Clear();
                    dataGridViewAutosomal.DataSource = table;
                    foreach (DataGridViewColumn col in dataGridViewAutosomal.Columns)
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }));
                
            }
            catch (Exception)
            {
                // something. The most probable cause is user just exited. so, just cancel the job...
            }
        }

        private void bwNewKitAutosomalJob_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            textBoxYDNA.Text = ysnps;
            textBoxMtDNA.Text = mtdna;  

            GGKUtilLib.setProgress(-1);
            GGKUtilLib.setStatus("Done.");
        }

        private void bwNewKitAutosomalJob_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            GGKUtilLib.setProgress(e.ProgressPercentage);
            GGKUtilLib.setStatus(e.UserState.ToString());            
        }

        private void NewKitFrm_Load(object sender, EventArgs e)
        {
            //YDNA12
             foreach (string marker in ydna12)
                dgvy12.Rows.Add(new string[] { marker, "" });
            //YDNA25
             foreach (string marker in ydna25)
                dgvy25.Rows.Add(new string[] { marker, "" });
            //YDNA37
             foreach (string marker in ydna37)
                dgvy37.Rows.Add(new string[] { marker, "" });
            //YDNA67
             foreach (string marker in ydna67)
                dgvy67.Rows.Add(new string[] { marker, "" });
            //YDNA111
             foreach (string marker in ydna111)
                dgvy111.Rows.Add(new string[] { marker, "" });

             cbSex.SelectedIndex = 0;
        }

        private void textBoxYDNA_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void textBoxYDNA_DragDrop(object sender, DragEventArgs e)
        {
            // file dropped.
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] filePaths = (string[])(e.Data.GetData(DataFormats.FileDrop));
                GGKUtilLib.setStatus("Parsing Y-DNA file(s) ...");
                bwNewKitYDNAJob.RunWorkerAsync(filePaths[0]);
            }
        }


        private void bwNewKitYDNAJob_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {            
            string[] lines = File.ReadAllLines(e.Argument.ToString());
            StringBuilder sb = new StringBuilder();
            string[] data = null;
            string[] snp = null;
            foreach (string line in lines)
            {
                data = line.Replace("\"", "").Split(new char[] { ',' });
                //"Type","Position","SNPName","Derived","OnTree","Reference","Genotype","Confidence"
                if (data[0] == "Known SNP")
                {
                    if (data[3] == "Yes(+)")
                    {
                        sb.Append(data[2] + "+, ");
                    }
                    else if (data[3] == "No(-)")
                    {
                        sb.Append(data[2] + "-, ");
                    }
                }
                else if (data[0] == "Novel Variant")
                {
                    if (ymap.ContainsKey(data[1]))
                    {
                        snp = GGKUtilLib.getYSNP(data[1], data[6]);
                        if(snp[0].IndexOf(";")==-1)
                            sb.Append(snp[0]+snp[1]+", ");
                        else
                             sb.Append(snp[0].Substring(0,snp[0].IndexOf(";"))+snp[1]+", ");
                    }
                }
            }
            //
            ysnps = sb.ToString().Trim();
            if (ysnps.Length>0)
                ysnps = ysnps.Substring(0, ysnps.Length - 1);
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to get Y-SNPs from " + e.Argument.ToString());
            }
        }


        

        private void bwNewKitYDNAJob_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            textBoxYDNA.Text = ysnps; 
            GGKUtilLib.setStatus("Done.");
        }

        private void textBoxMtDNA_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void textBoxMtDNA_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] filePaths = (string[])(e.Data.GetData(DataFormats.FileDrop));
                string fasta_file = filePaths[0];
                //
                try
                {
                    tbFASTA.Text = File.ReadAllText(fasta_file);
                    textBoxMtDNA.Text = GGKUtilLib.getMtDNAMarkers(fasta_file);
                }
                catch (Exception)
                {
                    MessageBox.Show("Unable to extract mtDNA mutations from " + fasta_file);
                }
                
            }
        }

        private void tabControlNewKit_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControlNewKit.SelectedIndex)
            {
                case 0:
                    tipLbl.Text = "Tip: Drag and drop any autosomal raw file into the grid below.";
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

        private void btnPaste_Click(object sender, EventArgs e)
        {
            if(Clipboard.ContainsText())
            {
                string stab="";
                for (int i = 0; i < 111; i++)
                    stab += "\t";
                string[] ystr = (Clipboard.GetText(TextDataFormat.Text) + stab).TrimStart().Split(new char[] { '\t' });

                //YDNA12
                for (int i = 0; i < ydna12.Length; i++)
                    dgvy12.Rows[i].Cells[1].Value = ystr[i];
                //YDNA25
                for (int i = 0; i < ydna25.Length; i++)
                    dgvy25.Rows[i].Cells[1].Value = ystr[i + ydna12.Length];
                //YDNA37
                for (int i = 0; i < ydna37.Length; i++)
                    dgvy37.Rows[i].Cells[1].Value = ystr[i + ydna12.Length + ydna25.Length];
                //YDNA67
                for (int i = 0; i < ydna67.Length; i++)
                    dgvy67.Rows[i].Cells[1].Value = ystr[i + ydna12.Length + ydna25.Length + ydna37.Length];
                //YDNA111
                for (int i = 0; i < ydna111.Length; i++)
                    dgvy111.Rows[i].Cells[1].Value = ystr[i + ydna12.Length + ydna25.Length + ydna37.Length + ydna67.Length];
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            //YDNA12
            for (int i = 0; i < ydna12.Length; i++)
                dgvy12.Rows[i].Cells[1].Value = "";
            //YDNA25
            for (int i = 0; i < ydna25.Length; i++)
                dgvy25.Rows[i].Cells[1].Value = "";
            //YDNA37
            for (int i = 0; i < ydna37.Length; i++)
                dgvy37.Rows[i].Cells[1].Value = "";
            //YDNA67
            for (int i = 0; i < ydna67.Length; i++)
                dgvy67.Rows[i].Cells[1].Value = "";
            //YDNA111
            for (int i = 0; i < ydna111.Length; i++)
                dgvy111.Rows[i].Cells[1].Value = "";
            //YDNA MISC
            dgvymisc.Rows.Clear();            
        }

        private void NewKitFrm_Activated(object sender, EventArgs e)
        {
            
        }

        private void NewKitFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bwDelete.IsBusy)
                bwDelete.CancelAsync();
            if (bwSave.IsBusy)
                bwSave.CancelAsync();
            if (bwNewKitAutosomalJob.IsBusy)
                bwNewKitAutosomalJob.CancelAsync();
            if (bwNewKitYDNAJob.IsBusy)
                bwNewKitYDNAJob.CancelAsync();
            if (bwPopuate.IsBusy)
                bwPopuate.CancelAsync();            
            GGKUtilLib.disableSave();
            GGKUtilLib.disable_DisableKitToolbarBtn();
            GGKUtilLib.disable_EnableKitToolbarBtn();
            GGKUtilLib.disableDeleteKitToolbarBtn();
            GGKUtilLib.setStatus("Done.");
        }

        public void Save()
        {
            bool err = false;
            if (txtKit.Text.Trim() == "")
            {
                errorProvider1.SetError(txtKit, "Must provide a Kit Number. If you don't have one, just enter anything unique to this kit. e.g, KIT01");
                err = true;
            }
            else
                errorProvider1.SetError(txtKit, "");

            if (txtName.Text.Trim() == "")
            {
                errorProvider1.SetError(txtName, "Must provide a name.");
                err = true;
            }
            else            
                errorProvider1.SetError(txtName, "");            


            if (err)
                return;
            
            Object[] args = new Object[] { txtKit.Text, txtName.Text, dataGridViewAutosomal.Rows, textBoxYDNA.Text, textBoxMtDNA.Text, new DataGridViewRowCollection[] { dgvy12.Rows, dgvy25.Rows, dgvy37.Rows, dgvy67.Rows, dgvy111.Rows, dgvymisc.Rows } ,cbSex.Text,tbFASTA.Text};
            GGKUtilLib.disableSave();
            GGKUtilLib.setStatus("Saving ...");
            this.Enabled = false;
            GGKUtilLib.disableMenu();
            GGKUtilLib.disableToolbar();
            bwSave.RunWorkerAsync(args);
        }

        private void bwSave_DoWork(object sender, DoWorkEventArgs e)
        {
            save_success = false;
            Object[] args = (Object[])e.Argument;
            string kit_no = args[0].ToString();
            string name = args[1].ToString();
            DataGridViewRowCollection rows = (DataGridViewRowCollection)args[2];
            string ysnps_list = args[3].ToString();
            string mutations = args[4].ToString();
            DataGridViewRowCollection[] dgvy_rows = (DataGridViewRowCollection[])args[5];
            string sex = args[6].ToString();
            string fasta = args[7].ToString();

            SQLiteConnection cnn = GGKUtilLib.getDBConnection();

            try
            {
                SQLiteCommand upCmd = null;
                //kit master
                string kit_name = GGKUtilLib.getKitName(kit_no);
                if (kit_name == "Unknown")
                {
                    // new kit
                    upCmd = new SQLiteCommand(@"INSERT OR REPLACE INTO kit_master(kit_no, name, sex)values(@kit_no,@name,@sex)", cnn);
                    upCmd.Parameters.AddWithValue("@kit_no", kit_no);
                    upCmd.Parameters.AddWithValue("@name", name);
                    upCmd.Parameters.AddWithValue("@sex", sex[0].ToString());
                    upCmd.ExecuteNonQuery();
                }
                else
                {
                    // kit exists
                    upCmd = new SQLiteCommand(@"UPDATE kit_master SET name=@name, sex=@sex WHERE kit_no=@kit_no", cnn);                    
                    upCmd.Parameters.AddWithValue("@name", name);
                    upCmd.Parameters.AddWithValue("@sex", sex[0].ToString());
                    upCmd.Parameters.AddWithValue("@kit_no", kit_no);
                    upCmd.ExecuteNonQuery();
                }
                upCmd.Dispose();
                bwSave.ReportProgress(35,"Saving Autosomal data ...");


                upCmd = new SQLiteCommand(@"DELETE from kit_autosomal where kit_no=@kit_no", cnn);
                upCmd.Parameters.AddWithValue("@kit_no", kit_no);
                upCmd.ExecuteNonQuery();
                
                //kit autosomal
                upCmd = new SQLiteCommand(@"INSERT OR REPLACE INTO kit_autosomal(kit_no, rsid,chromosome,position,genotype)values(@kit_no,@rsid,@chromosome,@position,@genotype)", cnn);
                
                using (var transaction = cnn.BeginTransaction())
                {
                    bool incomplete = false;
                    foreach (DataGridViewRow row in rows)
                    {
                        if (row.IsNewRow)
                            continue;
                        incomplete = false;
                        for (int c = 0; c < row.Cells.Count; c++)
                            if (row.Cells[c].Value == null)
                            {
                                incomplete = true;
                                break;
                            }
                            else if (row.Cells[c].Value.ToString().Trim() == "")
                            {
                                incomplete = true;
                                break;
                            }

                        if (incomplete)
                            continue;

                        upCmd.Parameters.AddWithValue("@kit_no", kit_no);
                        upCmd.Parameters.AddWithValue("@rsid", row.Cells[0].Value.ToString());
                        upCmd.Parameters.AddWithValue("@chromosome", row.Cells[1].Value.ToString());
                        upCmd.Parameters.AddWithValue("@position", row.Cells[2].Value.ToString());
                        upCmd.Parameters.AddWithValue("@genotype", row.Cells[3].Value.ToString());
                        upCmd.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                
                upCmd.Dispose();
                
                
                bwSave.ReportProgress(75,"Saving Y-SNPs ...");


                //kit ysnps
                upCmd = new SQLiteCommand(@"INSERT OR REPLACE INTO kit_ysnps(kit_no, ysnps) values (@kit_no,@ysnps)", cnn);
                upCmd.Parameters.AddWithValue("@kit_no", kit_no);
                upCmd.Parameters.AddWithValue("@ysnps", ysnps_list);
                upCmd.ExecuteNonQuery();
                upCmd.Dispose();
                bwSave.ReportProgress(80,"Saving Y-STR Values ...");

                upCmd = new SQLiteCommand(@"DELETE from kit_ystr where kit_no=@kit_no", cnn);
                upCmd.Parameters.AddWithValue("@kit_no", kit_no);
                upCmd.ExecuteNonQuery();
                //kit ystr
                upCmd = new SQLiteCommand(@"INSERT OR REPLACE INTO kit_ystr(kit_no, marker, value)values(@kit_no,@marker,@value)", cnn);
                using (var transaction = cnn.BeginTransaction())
                {
                    foreach (DataGridViewRowCollection row_collection in dgvy_rows)
                    {
                        foreach (DataGridViewRow row in row_collection)
                        {
                            if (row.IsNewRow)
                                continue;
                            if (row.Cells[1].Value.ToString().Trim() == "")
                                continue;
                            upCmd.Parameters.AddWithValue("@kit_no", kit_no);
                            upCmd.Parameters.AddWithValue("@marker", row.Cells[0].Value.ToString());
                            upCmd.Parameters.AddWithValue("@value", row.Cells[1].Value.ToString());
                            upCmd.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
                upCmd.Dispose();

                bwSave.ReportProgress(90,"Saving mtDNA mutations ...");

                upCmd = new SQLiteCommand(@"DELETE from kit_mtdna where kit_no=@kit_no", cnn);
                upCmd.Parameters.AddWithValue("@kit_no", kit_no);
                upCmd.ExecuteNonQuery();
                //kit mtdna
                upCmd = new SQLiteCommand(@"INSERT OR REPLACE INTO kit_mtdna(kit_no, mutations,fasta)values(@kit_no,@mutations,@fasta)", cnn);
                upCmd.Parameters.AddWithValue("@kit_no", kit_no);
                upCmd.Parameters.AddWithValue("@mutations", mutations);
                upCmd.Parameters.AddWithValue("@fasta", fasta);
                upCmd.ExecuteNonQuery();

                bwSave.ReportProgress(100,"Saved");
                save_success = true;
            }
            catch (Exception err)
            {               
                bwSave.ReportProgress(-1, "Not Saved. Tech Details: " + err.Message);
                MessageBox.Show("Not Saved. Techical Details: " + err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            cnn.Dispose();
        }

        private void bwSave_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            GGKUtilLib.setProgress(e.ProgressPercentage);
            if (e.UserState != null)
                GGKUtilLib.setStatus(e.UserState.ToString());
        }

        private void bwSave_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {            
            GGKUtilLib.setProgress(-1);
            this.Enabled = true;
            GGKUtilLib.enableMenu();
            GGKUtilLib.enableToolbar();
            if (save_success) 
                this.Close();
        }

        private void tabControlY_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControlY.SelectedIndex)
            {
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

        private void populateFields(string kit)
        {
            foreach (Control ctrl in editableCtrls)
                ctrl.Enabled = false;  

            GGKUtilLib.setStatus("Please wait ...");
            bwPopuate.RunWorkerAsync(kit);
        }

        private void bwPopuate_DoWork(object sender, DoWorkEventArgs e)
        {
            string kit = (string)e.Argument;
            SQLiteConnection cnn = GGKUtilLib.getDBConnection();
            //kit master
            SQLiteCommand query = new SQLiteCommand(@"SELECT name, sex from kit_master where kit_no=@kit_no", cnn);
            query.Parameters.AddWithValue("@kit_no", kit);
            SQLiteDataReader reader = query.ExecuteReader();
            string sex = "U";
            if (reader.Read())
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    txtName.Text = reader.GetString(0);
                    sex = reader.GetString(1);
                    if (sex == "U")
                        cbSex.SelectedIndex = 0;
                    else if (sex == "M")
                        cbSex.SelectedIndex = 1;
                    else if (sex == "F")
                        cbSex.SelectedIndex = 2;
                }));
            }
            reader.Close();
            query.Dispose();

            // kit autosomal - RSID,Chromosome,Position,Genotypoe
            query = new SQLiteCommand(@"SELECT rsid 'RSID',chromosome 'Chromosome',position 'Position',genotype 'Genotype' from kit_autosomal where kit_no=@kit_no order by chromosome,position", cnn);
            query.Parameters.AddWithValue("@kit_no", kit);
            reader = query.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            this.Invoke(new MethodInvoker(delegate
            {
                dataGridViewAutosomal.Rows.Clear();
                dataGridViewAutosomal.Columns.Clear();
                dataGridViewAutosomal.DataSource = dt;
                dataGridViewAutosomal.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridViewAutosomal.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridViewAutosomal.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridViewAutosomal.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                ((DataGridViewTextBoxColumn)dataGridViewAutosomal.Columns[0]).MaxInputLength = 20;
                ((DataGridViewTextBoxColumn)dataGridViewAutosomal.Columns[1]).MaxInputLength = 2;
                ((DataGridViewTextBoxColumn)dataGridViewAutosomal.Columns[2]).MaxInputLength = 15;
                ((DataGridViewTextBoxColumn)dataGridViewAutosomal.Columns[3]).MaxInputLength = 2;
            }));
            reader.Close();
            query.Dispose();


            // kit - ysnp
            query = new SQLiteCommand(@"SELECT ysnps from kit_ysnps where kit_no=@kit_no", cnn);
            query.Parameters.AddWithValue("@kit_no", kit);
            reader = query.ExecuteReader();
            if (reader.Read())
            {
                ysnps = reader.GetString(0);
            }
            reader.Close();
            query.Dispose();
            this.Invoke(new MethodInvoker(delegate
            {
                textBoxYDNA.Text = ysnps;
            }));

            // kit - ystr
            query = new SQLiteCommand(@"SELECT marker,value from kit_ystr where kit_no=@kit_no", cnn);
            query.Parameters.AddWithValue("@kit_no", kit);
            reader = query.ExecuteReader();
            string marker = null;
            string value = null;
            Dictionary<string, string> ystr_dict = new Dictionary<string, string>();
            while (reader.Read())
            {
                marker = reader.GetString(0);
                value = reader.GetString(1);
                ystr_dict.Add(marker, value);
            }
            reader.Close();
            query.Dispose();
            this.Invoke(new MethodInvoker(delegate
            {
                //
                //YDNA12
                for (int i = 0; i < ydna12.Length; i++)
                {
                    if (ystr_dict.ContainsKey(ydna12[i]))
                    {
                        dgvy12.Rows[i].Cells[1].Value = ystr_dict[ydna12[i]];
                        ystr_dict.Remove(ydna12[i]);
                    }
                }
                //YDNA25
                for (int i = 0; i < ydna25.Length; i++)
                {
                    if (ystr_dict.ContainsKey(ydna25[i]))
                    {
                        dgvy25.Rows[i].Cells[1].Value = ystr_dict[ydna25[i]];
                        ystr_dict.Remove(ydna25[i]);
                    }
                }
                //YDNA37
                for (int i = 0; i < ydna37.Length; i++)
                {
                    if (ystr_dict.ContainsKey(ydna37[i]))
                    {
                        dgvy37.Rows[i].Cells[1].Value = ystr_dict[ydna37[i]];
                        ystr_dict.Remove(ydna37[i]);
                    }
                }
                //YDNA67
                for (int i = 0; i < ydna67.Length; i++)
                {
                    if (ystr_dict.ContainsKey(ydna67[i]))
                    {
                        dgvy67.Rows[i].Cells[1].Value = ystr_dict[ydna67[i]];
                        ystr_dict.Remove(ydna67[i]);
                    }
                }
                //YDNA111
                for (int i = 0; i < ydna111.Length; i++)
                {
                    if (ystr_dict.ContainsKey(ydna111[i]))
                    {
                        dgvy111.Rows[i].Cells[1].Value = ystr_dict[ydna111[i]];
                        ystr_dict.Remove(ydna111[i]);
                    }
                }
                //YDNA MISC
                dgvymisc.Rows.Clear();
                foreach (string str in ystr_dict.Keys)
                {
                    dgvymisc.Rows.Add(new string[] { str, ystr_dict[str] });
                }
            }));


            // kit - mtdna
            query = new SQLiteCommand(@"SELECT mutations,fasta from kit_mtdna where kit_no=@kit_no", cnn);
            query.Parameters.AddWithValue("@kit_no", kit);
            reader = query.ExecuteReader();
            
            if (reader.Read())
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    textBoxMtDNA.Text = reader.GetString(0);
                    tbFASTA.Text = reader.GetString(1);
                }));
            }
            reader.Close();
            query.Dispose();
        }

        private void bwPopuate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {            
            foreach(Control ctrl in editableCtrls)
                ctrl.Enabled = true;      
            //
            doControlActivities(kit_disabled);
            GGKUtilLib.setStatus("Done.");
        }

        private void doControlActivities(bool disabled)
        {
            if (disabled)
            {
                GGKUtilLib.enable_EnableKitToolbarBtn();
                GGKUtilLib.disable_DisableKitToolbarBtn();
                GGKUtilLib.enableDeleteKitToolbarBtn();
                GGKUtilLib.disableSave();

                txtName.ReadOnly = true;
                dataGridViewAutosomal.ReadOnly = true;
                textBoxYDNA.ReadOnly = true;
                dgvy12.ReadOnly = true;
                dgvy25.ReadOnly = true;
                dgvy37.ReadOnly = true;
                dgvy67.ReadOnly = true;
                dgvy111.ReadOnly = true;
                dgvymisc.ReadOnly = true;
                textBoxMtDNA.ReadOnly = true;
            }
            else
            {
                GGKUtilLib.disable_EnableKitToolbarBtn();
                GGKUtilLib.enable_DisableKitToolbarBtn();
                GGKUtilLib.enableDeleteKitToolbarBtn();
                GGKUtilLib.enableSave();
                
                txtName.ReadOnly = false;
                dataGridViewAutosomal.ReadOnly = false;
                textBoxYDNA.ReadOnly = false;
                dgvy12.ReadOnly = false;
                dgvy25.ReadOnly = false;
                dgvy37.ReadOnly = false;
                dgvy67.ReadOnly = false;
                dgvy111.ReadOnly = false;
                dgvymisc.ReadOnly = false;
                textBoxMtDNA.ReadOnly = false;
            }
        }

        public void Disable()
        {
            string kit_no = txtKit.Text;
            SQLiteConnection cnn = GGKUtilLib.getDBConnection();
            using (SQLiteTransaction dbTrans = cnn.BeginTransaction())
            {
                try
                {
                    SQLiteCommand upCmd = new SQLiteCommand(@"UPDATE kit_master set disabled=1 where kit_no=@kit_no", cnn);
                    upCmd.Parameters.AddWithValue("@kit_no", kit_no);
                    upCmd.ExecuteNonQuery();
                    dbTrans.Commit();
                    doControlActivities(true);
                }
                catch (Exception err)
                {
                    dbTrans.Rollback();
                    MessageBox.Show("Unable to disable kit " + kit_no+"\r\nTechnical Error: "+err.Message);
                    GGKUtilLib.setStatus("Unable to disable kit " + kit_no);
                }               
            }
            
        }

        public void Enable()
        {
            string kit_no = txtKit.Text;
            SQLiteConnection cnn = GGKUtilLib.getDBConnection();
            using (SQLiteTransaction dbTrans = cnn.BeginTransaction())
            {
                try
                {
                    SQLiteCommand upCmd = new SQLiteCommand(@"UPDATE kit_master set disabled=0 where kit_no=@kit_no", cnn);
                    upCmd.Parameters.AddWithValue("@kit_no", kit_no);
                    upCmd.ExecuteNonQuery();
                    dbTrans.Commit();
                    doControlActivities(false);
                }
                catch (Exception err)
                {
                    dbTrans.Rollback();
                    MessageBox.Show("Unable to enable kit " + kit_no + "\r\nTechnical Error: " + err.Message);
                    GGKUtilLib.setStatus("Unable to enable kit " + kit_no);
                }
            }
            
        }

        public void Delete()
        {
            string kit_no = txtKit.Text;
            if (MessageBox.Show("Are you sure to delete kit " + kit_no + "?", "Delete?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                GGKUtilLib.setStatus("Deleting " + kit_no + "...");
                doControlActivities(true);
                GGKUtilLib.disableToolbar();
                bwDelete.RunWorkerAsync(kit_no);
            }
        }

        private void dataGridViewAutosomal_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Data Error. Technical Details: "+e.Exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            GGKUtilLib.setStatus("Data Error. Technical Details: " + e.Exception.Message);
        }

        private void dgvy_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Data Error. Technical Details: " + e.Exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            GGKUtilLib.setStatus("Data Error. Technical Details: " + e.Exception.Message);
        }

        private void dgvymisc_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                try
                {
                    var hti = dgvymisc.HitTest(e.X, e.Y);
                    dgvymisc.ClearSelection();
                    dgvymisc.Rows[hti.RowIndex].Selected = true;
                }
                catch (Exception)
                { }                
            }
        }

        private void DeleteRowYDNAMisc_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 rowToDelete = dgvymisc.Rows.GetFirstRow(DataGridViewElementStates.Selected);
                dgvymisc.Rows.RemoveAt(rowToDelete);
                dgvymisc.ClearSelection();
            }
            catch (Exception)
            { }            
        }

        private void dataGridViewAutosomal_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                try
                {
                    var hti = dataGridViewAutosomal.HitTest(e.X, e.Y);
                    dataGridViewAutosomal.ClearSelection();
                    dataGridViewAutosomal.Rows[hti.RowIndex].Selected = true;
                }
                catch (Exception)
                { }
                
            }
        }

        private void deleteRowToolStripAutosomal_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 rowToDelete = dataGridViewAutosomal.Rows.GetFirstRow(DataGridViewElementStates.Selected);
                dataGridViewAutosomal.Rows.RemoveAt(rowToDelete);
                dataGridViewAutosomal.ClearSelection();
            }
            catch (Exception)
            {}
            
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                dgvymisc.Rows.Clear();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } 
        }

        private void clearAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridViewAutosomal.Rows.Clear();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void bwDelete_DoWork(object sender, DoWorkEventArgs e)
        {
           
            string kit_no = (string)e.Argument;
            SQLiteConnection cnn = GGKUtilLib.getDBConnection();
            try
            {
                SQLiteCommand upCmd = new SQLiteCommand(@"DELETE from kit_master where kit_no=@kit_no", cnn);
                upCmd.Parameters.AddWithValue("@kit_no", kit_no);
                upCmd.ExecuteNonQuery();
                this.Invoke(new MethodInvoker(delegate { this.Close(); }));
            }
            catch (Exception err)
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    MessageBox.Show("Unable to delete kit " + kit_no + "\r\nTechnical Error: " + err.Message);
                    GGKUtilLib.setStatus("Unable to delete kit " + kit_no);
                }));
            }
            
        }

        private void bwDelete_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            GGKUtilLib.enableToolbar();
            GGKUtilLib.setStatus("Done.");
        }

        private void tbFASTA_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void tbFASTA_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] filePaths = (string[])(e.Data.GetData(DataFormats.FileDrop));
                string fasta_file = filePaths[0];
                //
                try
                {
                    tbFASTA.Text = File.ReadAllText(fasta_file);
                    textBoxMtDNA.Text = GGKUtilLib.getMtDNAMarkers(fasta_file);
                }
                catch (Exception)
                {
                    MessageBox.Show("Unable to extract mtDNA mutations from " + fasta_file);
                }

            }
        }
    }
}

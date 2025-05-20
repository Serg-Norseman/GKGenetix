using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Genetic_Genealogy_Kit
{
    public partial class SelectKitFrm : Form
    {
        List<string> disabled = new List<string>();

        public const int OPEN_KIT = 0;
        public const int EXPORT_KIT = 1;
        public const int SELECT_ONE_TO_MANY = 2;
        public const int SELECT_ADMIXTURE = 3;
        public const int SELECT_ROH = 4;
        public const int SELECT_KIT = 5;
        public const int SELECT_MTPHYLOGENY = 6;
        public const int SELECT_MITOMAP = 7;
        public const int SELECT_ISOGGYTREE = 8;

        int selected_operation = 0;
        string kit = "Unknown";
        string select_sql = null;

        public string getSelectedKit()
        {
            return kit;
        }
        
        public SelectKitFrm(int operation)
        {
            InitializeComponent();
            //
            selected_operation = operation;
            string hide_ref = GGKSettings.getParameterValue("Admixture.ReferencePopulations.Hide");
            switch (selected_operation)
            {
                case OPEN_KIT:
                    btnOpen.Text = "Open";
                    if (hide_ref == "1")
                        select_sql = @"SELECT kit_no,name,disabled,last_modified FROM kit_master WHERE reference=0 order by last_modified DESC";
                    else
                        select_sql = @"SELECT kit_no,name,disabled,last_modified FROM kit_master order by last_modified DESC";
                    break;
                case EXPORT_KIT:
                    btnOpen.Text = "Export";
                    if(hide_ref=="1")
                        select_sql = @"SELECT kit_no,name,disabled,last_modified FROM kit_master WHERE reference=0 order by last_modified DESC";
                    else
                        select_sql = @"SELECT kit_no,name,disabled,last_modified FROM kit_master order by last_modified DESC";
                    break;
                case SELECT_ONE_TO_MANY:
                    //hide parameter has no effect.
                    btnOpen.Text = "Select";
                    select_sql = @"SELECT kit_no,name,disabled,last_modified FROM kit_master WHERE reference=0 order by last_modified DESC";
                    break;
                case SELECT_ADMIXTURE:
                    //hide parameter has no effect.
                    btnOpen.Text = "Select";
                    select_sql = @"SELECT kit_no,name,disabled,last_modified FROM kit_master WHERE reference=0 order by last_modified DESC";
                    break;
                case SELECT_ROH:
                    //hide parameter has no effect.
                    btnOpen.Text = "Select";
                    select_sql = @"SELECT kit_no,name,disabled,last_modified FROM kit_master WHERE reference=0  order by last_modified DESC";
                    break;
                case SELECT_KIT:
                    btnOpen.Text = "Select";
                    if (hide_ref == "1")
                        select_sql = @"SELECT kit_no,name,disabled,last_modified FROM kit_master WHERE reference=0 order by last_modified DESC";
                    else
                        select_sql = @"SELECT kit_no,name,disabled,last_modified FROM kit_master order by last_modified DESC";
                    break;
                case SELECT_MTPHYLOGENY:
                    btnOpen.Text = "Select";
                    if (hide_ref == "1")
                        select_sql = @"select kit_no,name,disabled,last_modified from kit_master where kit_no in (select distinct kit_no from kit_mtdna) and reference=0 order by last_modified DESC";
                    else
                        select_sql = @"select kit_no,name,disabled,last_modified from kit_master where kit_no in (select distinct kit_no from kit_mtdna) order by last_modified DESC";
                    break;
                case SELECT_MITOMAP:
                    btnOpen.Text = "Select";
                    if (hide_ref == "1")
                        select_sql = @"select kit_no,name,disabled,last_modified from kit_master where kit_no in (select distinct kit_no from kit_mtdna) and reference=0 order by last_modified DESC";
                    else
                        select_sql = @"select kit_no,name,disabled,last_modified from kit_master where kit_no in (select distinct kit_no from kit_mtdna) order by last_modified DESC";
                    break;
                case SELECT_ISOGGYTREE:
                    btnOpen.Text = "Select";
                    if (hide_ref == "1")
                        select_sql = @"select kit_no,name,disabled,last_modified from kit_master where kit_no in (select distinct kit_no from kit_ysnps) and reference=0 order by last_modified DESC";
                    else
                        select_sql = @"select kit_no,name,disabled,last_modified from kit_master where kit_no in (select distinct kit_no from kit_ysnps) order by last_modified DESC";
                    break;
                default:
                    if (hide_ref == "1")
                        select_sql = @"SELECT kit_no,name,disabled,last_modified FROM kit_master WHERE reference=0 order by last_modified DESC";
                    else
                        select_sql = @"SELECT kit_no,name,disabled,last_modified FROM kit_master order by last_modified DESC";
                    break;
            }
        }

        private void OpenKitFrm_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            
        }

        private void dataGridViewOpenKit_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewOpenKit.SelectedRows.Count > 0)
                    kitLbl.Text = dataGridViewOpenKit.SelectedRows[0].Cells[0].Value.ToString(); 
            }
            catch (Exception)
            {
               //ignore               
            }
                       
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            disabled.Clear();
            DataGridViewCellStyle gray = new DataGridViewCellStyle();
            gray.ForeColor=Color.LightGray;

            SQLiteConnection cnn = GGKUtilLib.getDBConnection();            
            dataGridViewOpenKit.Rows.Clear();
            string hide = GGKSettings.getParameterValue("Admixture.ReferencePopulations.Hide");
            SQLiteCommand query = new SQLiteCommand(select_sql, cnn);
            SQLiteDataReader reader = query.ExecuteReader();
            while(reader.Read())
            {
                int new_idx = dataGridViewOpenKit.Rows.Add();
                DataGridViewRow row = dataGridViewOpenKit.Rows[new_idx];                
                row.Cells[0].Value =reader.GetString(0);
                row.Cells[1].Value =reader.GetString(1);
                row.Cells[2].Value =reader.GetString(3);
                if (reader.GetInt16(2) == 1)
                {
                    row.DefaultCellStyle = gray;
                    disabled.Add(reader.GetString(0));
                }
                
            }
            query.Dispose();
            cnn.Dispose();

            if (dataGridViewOpenKit.Rows.Count > 0)
            {
                dataGridViewOpenKit.CurrentCell = dataGridViewOpenKit.Rows[0].Cells[0];
                kitLbl.Text = dataGridViewOpenKit.SelectedRows[0].Cells[0].Value.ToString();
            }
            if(dataGridViewOpenKit.Rows.Count==0)
            {
                MessageBox.Show("There are no kits available to open.","",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                this.Close();
            }

        }

        private void dataGridViewOpenKit_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectKit(kitLbl.Text);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            SelectKit(kitLbl.Text);
        }

        private void SelectKit(string kit)
        {            
            switch (selected_operation)
            {
                case OPEN_KIT:
                    GGKUtilLib.hideAllMdiChildren();
                    NewEditKitFrm newKitFrm = Program.GGKitFrmMainInst.getNewEditKitFrm();
                    if (newKitFrm == null)
                        newKitFrm = new NewEditKitFrm(kit, disabled.Contains(kit));
                    if (newKitFrm.IsDisposed)
                        newKitFrm = new NewEditKitFrm(kit, disabled.Contains(kit));
                    newKitFrm.MdiParent = Program.GGKitFrmMainInst;
                    newKitFrm.Visible = true;
                    newKitFrm.WindowState = FormWindowState.Maximized;
                    break;
                case EXPORT_KIT:
                    //ToDo:
                    GGKUtilLib.hideAllMdiChildren();
                    if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        btnOpen.Text = "Exporting ...";
                        btnOpen.Enabled = false;
                        GGKUtilLib.setStatus("Exporting. Please wait ...");
                        bwExport.RunWorkerAsync(new string[] { kit, saveFileDialog.FileName, saveFileDialog.FilterIndex.ToString()});
                    }
                    return;
                case SELECT_ONE_TO_MANY:
                    GGKUtilLib.hideAllMdiChildren();
                    MatchingKitsFrm frm = new MatchingKitsFrm(kit);
                    frm.MdiParent = Program.GGKitFrmMainInst;
                    frm.Visible = true;
                    frm.WindowState = FormWindowState.Maximized;
                    break;
                case SELECT_ADMIXTURE:
                    GGKUtilLib.hideAllMdiChildren();
                    AdmixtureFrm afrm = new AdmixtureFrm(kit);
                    afrm.MdiParent = Program.GGKitFrmMainInst;
                    afrm.Visible = true;
                    afrm.WindowState = FormWindowState.Maximized;
                    break;
                case SELECT_ROH:
                    GGKUtilLib.hideAllMdiChildren();
                    ROHFrm rohfrm = new ROHFrm(kit);
                    rohfrm.MdiParent = Program.GGKitFrmMainInst;
                    rohfrm.Visible = true;
                    rohfrm.WindowState = FormWindowState.Maximized;
                    break;
                case SELECT_KIT:
                    this.kit = kit;
                    this.Visible = false;
                    return;
                case SELECT_MTPHYLOGENY:
                    GGKUtilLib.hideAllMdiChildren();
                    MtPhylogenyFrm mtFrm = new MtPhylogenyFrm(kit);
                    mtFrm.MdiParent = Program.GGKitFrmMainInst;
                    mtFrm.Visible = true;
                    mtFrm.WindowState = FormWindowState.Maximized;
                    break;
                case SELECT_MITOMAP:
                    GGKUtilLib.hideAllMdiChildren();
                    MitoMapFrm mpFrm = new MitoMapFrm(kit);
                    mpFrm.MdiParent = Program.GGKitFrmMainInst;
                    mpFrm.Visible = true;
                    mpFrm.WindowState = FormWindowState.Maximized;
                    break;
                case SELECT_ISOGGYTREE:
                    GGKUtilLib.hideAllMdiChildren();
                    IsoggYTreeFrm yFrm = new IsoggYTreeFrm(kit);
                    yFrm.MdiParent = Program.GGKitFrmMainInst;
                    yFrm.Visible = true;
                    yFrm.WindowState = FormWindowState.Maximized;
                    break;
                default:
                    break;
            }
            this.Close();
        }

        private void bwExport_DoWork(object sender, DoWorkEventArgs e)
        {
            string kit_no = ((string[])e.Argument)[0];
            string filename = ((string[])e.Argument)[1];
            int option = int.Parse(((string[])e.Argument)[2]);
            //
            GGKUtilLib.exportKit(kit_no, filename, option);
        }

        private void bwExport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            GGKUtilLib.setStatus("Done.");
            this.Close();
        }

        private void SelectKitFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bwExport.IsBusy)
            {
                if (MessageBox.Show("Exporting kit .. Do you want to cancel it and close this window?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    GGKUtilLib.setStatus("Export Cancelled.");
                    bwExport.CancelAsync();
                }
                else
                    e.Cancel = true;
            }
        }
    }
}

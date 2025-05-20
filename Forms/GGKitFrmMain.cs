using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Genetic_Genealogy_Kit
{
    public partial class GGKitFrmMain : Form
    {
        AboutGGKFrm aboutFrm = null;
        LicenseFrm licFrm = null;
        NewEditKitFrm newKitFrm = null;
        SettingsFrm settingsFrm = null;

        public GGKitFrmMain()
        {
            InitializeComponent();
        }

        public NewEditKitFrm getNewEditKitFrm()
        {
            return newKitFrm;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hideAllChildren("AboutGGKFrm");
            if (aboutFrm == null)
                aboutFrm = new AboutGGKFrm();
            if (aboutFrm.IsDisposed)
                aboutFrm = new AboutGGKFrm();
            aboutFrm.MdiParent = this;
            aboutFrm.Visible = true;
            aboutFrm.WindowState = FormWindowState.Maximized;
        }

        private void licenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hideAllChildren("LicenseFrm");
            if(licFrm == null)
                licFrm = new LicenseFrm();
            if (licFrm.IsDisposed)
                licFrm = new LicenseFrm();
            licFrm.MdiParent=this;
            licFrm.Visible = true;            
            licFrm.WindowState = FormWindowState.Maximized;
        }

        public void hideAllChildren(string exceptFrm)
        {
            foreach(Form frm in this.MdiChildren)
            {
                if (frm.Name != exceptFrm)
                    frm.Dispose();
            }
        }

        public void setStatusMessage(string message)
        {
            statusLbl.Text = message;
        }

        public void setProgress(int percent)
        {
            if (percent == -1|| percent==100)
                progressBar.Visible = false;
            else
            {
                progressBar.Visible = true;
                progressBar.Value = percent;
            }
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            newToolStripMenuItem.PerformClick();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hideAllChildren("");
            if (newKitFrm == null)
                newKitFrm = new NewEditKitFrm(null,false);
            if (newKitFrm.IsDisposed)
                newKitFrm = new NewEditKitFrm(null, false);
            newKitFrm.MdiParent = this;
            newKitFrm.Visible = true;
            newKitFrm.WindowState = FormWindowState.Maximized;
        }

        public void enableSave()
        {
            saveToolStripButton.Enabled = true;
            saveToolStripMenuItem.Enabled = true;
        }

        public void disableSave()
        {
            saveToolStripButton.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            GGKUtilLib.SaveInfoFromActiveMdiChild();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GGKUtilLib.SaveInfoFromActiveMdiChild();
        }

        public void enableToolbar()
        {
            toolStripGGK.Enabled = true;
            menuStripGGK.Enabled = true;
        }

        public void disableToolbar()
        {
            toolStripGGK.Enabled = false;
            menuStripGGK.Enabled = false;
        }

        public void enable_EnableKitToolbarBtn()
        {
            toolStripEnableIcon.Enabled = true;
            enableToolStripMenuItem.Enabled = true;
        }

        public void disable_EnableKitToolbarBtn()
        {
            toolStripEnableIcon.Enabled = false;
            enableToolStripMenuItem.Enabled = false;
        }

        public void enableDeleteKitToolbarBtn()
        {
            toolStripDeleteBtn.Enabled = true;
            deleteToolStripMenuItem.Enabled = true;
        }

        public void disableDeleteKitToolbarBtn()
        {
            toolStripDeleteBtn.Enabled = false;
            deleteToolStripMenuItem.Enabled = false;
        }


        public void enable_DisableKitToolbarBtn()
        {
            toolStripDisableBtn.Enabled = true;
            disableToolStripMenuItem.Enabled = true;
        }

        public void disable_DisableKitToolbarBtn()
        {
            toolStripDisableBtn.Enabled = false;
            disableToolStripMenuItem.Enabled = false;
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            SelectKitFrm open = new SelectKitFrm(SelectKitFrm.OPEN_KIT);
            open.ShowDialog(this);
        }

        private void toolStripDisableBtn_Click(object sender, EventArgs e)
        {
            GGKUtilLib.disableKit();
        }

        private void toolStripEnableIcon_Click(object sender, EventArgs e)
        {
            GGKUtilLib.enableKit();
        }

        private void toolStripDeleteBtn_Click(object sender, EventArgs e)
        {
            GGKUtilLib.deleteKit();
        }

        private void GGKitFrmMain_Load(object sender, EventArgs e)
        {
            setStatusMessage("Checking Integrity of DB ...");
            this.Enabled = false;
            bwIChkAndFix.RunWorkerAsync();
            this.Text = Application.ProductName + " v" + Application.ProductVersion.ToString();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectKitFrm open = new SelectKitFrm(SelectKitFrm.OPEN_KIT);
            open.ShowDialog(this);
        }


        private void enableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GGKUtilLib.enableKit();
        }

        private void disableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GGKUtilLib.disableKit();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GGKUtilLib.deleteKit();
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openFileDialogGGK.ShowDialog(this)==DialogResult.OK)
            {
                disableToolbar();
                setStatusMessage("Importing "+Path.GetFileName(openFileDialogGGK.FileName));
                bwImport.RunWorkerAsync(openFileDialogGGK.FileName);
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectKitFrm open = new SelectKitFrm(SelectKitFrm.EXPORT_KIT);
            open.ShowDialog(this);
        }

        private void bwImport_DoWork(object sender, DoWorkEventArgs e)
        {
            GGKUtilLib.importKit(e.Argument.ToString());
        }

        private void bwImport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            setStatusMessage("Done.");
            setProgress(-1);
            enableToolbar();
        }

        private void onetoOneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectTwoKitsFrm frm = new SelectTwoKitsFrm(SelectTwoKitsFrm.SELECT_ADMIXTURE);
            frm.ShowDialog(this);
        }

        private void onetoManyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectKitFrm open = new SelectKitFrm(SelectKitFrm.SELECT_ONE_TO_MANY);
            open.ShowDialog(this);
        }

        private void factoryResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to delete everything and reset to factory defaults?", "Factory Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                GGKUtilLib.FactoryReset();
            }
        }

        private void bwIChkAndFix_DoWork(object sender, DoWorkEventArgs e)
        {
            GGKUtilLib.integrityCheckAndFix();
            DataTable dt = GGKUtilLib.QueryDB("select * from kit_master where reference=1");
            if (dt.Rows.Count == 0)
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    admixtureToolStripMenuItem.Enabled = false;
                }));
            }

        }

        private void bwIChkAndFix_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            setStatusMessage("Done.");
            this.Enabled = true;
        }

        private void processKitsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hideAllChildren("");
            ProcessKitsFrm pkfrm = new ProcessKitsFrm();
            pkfrm.MdiParent = this;
            pkfrm.Visible = true;
            pkfrm.WindowState = FormWindowState.Maximized;
        }

        private void admixtureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectKitFrm open = new SelectKitFrm(SelectKitFrm.SELECT_ADMIXTURE);
            open.ShowDialog(this);
        }

        private void quickEditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hideAllChildren("");
            QuickEditKit qefrm = new QuickEditKit();
            qefrm.MdiParent = this;
            qefrm.Visible = true;
            qefrm.WindowState = FormWindowState.Maximized;
        }

        private void runsOfHomozygosityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectKitFrm open = new SelectKitFrm(SelectKitFrm.SELECT_ROH);
            open.ShowDialog(this);
        }

        private void phasingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hideAllChildren("");
            PhasingFrm pufrm = new PhasingFrm();
            pufrm.MdiParent = this;
            pufrm.Visible = true;
            pufrm.WindowState = FormWindowState.Maximized;
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hideAllChildren("SettingsFrm");
            if (settingsFrm == null)
                settingsFrm = new SettingsFrm();
            if (settingsFrm.IsDisposed)
                settingsFrm = new SettingsFrm();
            settingsFrm.MdiParent = this;
            settingsFrm.Visible = true;
            settingsFrm.WindowState = FormWindowState.Maximized;
        }

        private void mtDnaPhylogenyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectKitFrm open = new SelectKitFrm(SelectKitFrm.SELECT_MTPHYLOGENY);
            open.ShowDialog(this);
        }

        private void mitoMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectKitFrm open = new SelectKitFrm(SelectKitFrm.SELECT_MITOMAP);
            open.ShowDialog(this);
        }

        private void iSOGGYTreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectKitFrm open = new SelectKitFrm(SelectKitFrm.SELECT_ISOGGYTREE);
            open.ShowDialog(this);
        }
    }
}

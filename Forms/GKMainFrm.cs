/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using GenetixKit.Core;

namespace GenetixKit.Forms
{
    public partial class GKMainFrm : Form, IKitHost
    {
        private NewEditKitFrm newKitFrm = null;

        public GKMainFrm()
        {
            InitializeComponent();
        }

        #region Handlers

        private void GGKitFrmMain_Load(object sender, EventArgs e)
        {
            SetStatus("Checking Integrity of DB ...");
            this.Enabled = false;
            this.Text = Application.ProductName + " v" + Application.ProductVersion.ToString();

            // bwIChkAndFix.RunWorkerAsync();
            Task.Factory.StartNew(() => {
                GKSqlFuncs.CheckIntegrity();

                this.Invoke(new MethodInvoker(delegate {
                    SetStatus("Done.");
                    this.Enabled = true;
                }));
            });
        }

        private void miExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void miNew_Click(object sender, EventArgs e)
        {
            NewKit(null, false);
        }

        private void miSave_Click(object sender, EventArgs e)
        {
            Form mdifrm = this.ActiveMdiChild;
            if (mdifrm.Name == "NewEditKitFrm")
                ((NewEditKitFrm)mdifrm).Save();
            else if (mdifrm.Name == "QuickEditKit")
                ((QuickEditKit)mdifrm).Save();
        }

        private void miOpen_Click(object sender, EventArgs e)
        {
            SelectOper(UIOperation.OPEN_KIT);
        }

        private void miDelete_Click(object sender, EventArgs e)
        {
            DeleteKit();
        }

        private void miOneToOne_Click(object sender, EventArgs e)
        {
            using (var frm = new SelectTwoKitsFrm(UIOperation.SELECT_ADMIXTURE)) {
                frm.ShowDialog(this);
            }
        }

        private void miOneToMany_Click(object sender, EventArgs e)
        {
            SelectOper(UIOperation.SELECT_ONE_TO_MANY);
        }

        private void miProcessKits_Click(object sender, EventArgs e)
        {
            ShowProcessKits();
        }

        private void miAdmixture_Click(object sender, EventArgs e)
        {
            SelectKitFrm open = new SelectKitFrm(UIOperation.SELECT_ADMIXTURE);
            open.ShowDialog(this);
        }

        private void miQuickEdit_Click(object sender, EventArgs e)
        {
            ShowQuickEdit();
        }

        private void miRunsOfHomozygosity_Click(object sender, EventArgs e)
        {
            SelectOper(UIOperation.SELECT_ROH);
        }

        private void miMtDnaPhylogeny_Click(object sender, EventArgs e)
        {
            SelectOper(UIOperation.SELECT_MTPHYLOGENY);
        }

        private void miMitoMap_Click(object sender, EventArgs e)
        {
            SelectOper(UIOperation.SELECT_MITOMAP);
        }

        private void miISOGGYTree_Click(object sender, EventArgs e)
        {
            SelectOper(UIOperation.SELECT_ISOGGYTREE);
        }

        private void miPhasing_Click(object sender, EventArgs e)
        {
            ShowMdiChild(new PhasingFrm());
        }

        #endregion

        private void ShowMdiChild(Form frm)
        {
            foreach (Form fm in MdiChildren) fm.Dispose();

            frm.MdiParent = this;
            frm.Visible = true;
            frm.WindowState = FormWindowState.Maximized;
        }

        public void DeleteKit()
        {
            Form mdifrm = this.ActiveMdiChild;
            if (mdifrm.Name == "QuickEditKit")
                ((QuickEditKit)mdifrm).Delete();
        }

        public void SetStatus(string message)
        {
            statusLbl.Text = message;
        }

        public void SetProgress(int percent)
        {
            if (percent == -1 || percent == 100)
                progressBar.Visible = false;
            else {
                progressBar.Visible = true;
                progressBar.Value = percent;
            }
        }

        public void NewKit(string kit, bool disabled)
        {
            if (newKitFrm == null || newKitFrm.IsDisposed)
                newKitFrm = new NewEditKitFrm(kit, disabled);
            ShowMdiChild(newKitFrm);
        }

        public void EnableSave()
        {
            miSave.Enabled = true;
        }

        public void DisableSave()
        {
            miSave.Enabled = false;
        }

        public void EnableToolbar()
        {
            menuStripGGK.Enabled = true;
        }

        public void DisableToolbar()
        {
            menuStripGGK.Enabled = false;
        }

        public void EnableDelete()
        {
            miDelete.Enabled = true;
        }

        public void DisableDelete()
        {
            miDelete.Enabled = false;
        }

        public void ShowAdmixture(string kit)
        {
            ShowMdiChild(new AdmixtureFrm(kit));
        }

        public void ShowProcessKits()
        {
            ShowMdiChild(new ProcessKitsFrm());
        }

        public void ShowQuickEdit()
        {
            ShowMdiChild(new QuickEditKit());
        }

        public void ShowPhasedSegmentVisualizer(string kit1, string kit2, string chr, string start_pos, string end_pos)
        {
            using (var frm = new PhasedSegmentFrm(kit1, kit2, chr, start_pos, end_pos))
                frm.ShowDialog(this);
        }

        public void SelectOper(UIOperation operation)
        {
            using (SelectKitFrm open = new SelectKitFrm(operation))
                open.ShowDialog(this);
        }

        public string SelectKit()
        {
            using (var open = new SelectKitFrm(UIOperation.SELECT_KIT)) {
                open.ShowDialog(this);
                return open.GetSelectedKit();
            }
        }

        public void ShowMatchingKits(string kit)
        {
            ShowMdiChild(new MatchingKitsFrm(kit));
        }

        public void ShowROH(string kit)
        {
            ShowMdiChild(new ROHFrm(kit));
        }

        public void ShowMtPhylogeny(string kit)
        {
            ShowMdiChild(new MtPhylogenyFrm(kit));
        }

        public void ShowMitoMap(string kit)
        {
            ShowMdiChild(new MitoMapFrm(kit));
        }

        public void ShowIsoggYTree(string kit)
        {
            ShowMdiChild(new IsoggYTreeFrm(kit));
        }

        public void ShowOneToOneCmp(string kit1, string kit2)
        {
            ShowMdiChild(new OneToOneCmpFrm(kit1, kit2));
        }

        public void SelectLocation(ref int x, ref int y)
        {
            using (var frm = new LocationSelectFrm(x, y)) {
                frm.ShowDialog(this);
                x = frm.X;
                y = frm.Y;
            }
        }
    }
}

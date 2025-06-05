/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using GKGenetix.Core;
using GKGenetix.Core.Database;

namespace GKGenetix.UI.Forms
{
    public partial class GKMainFrm : Form, IKitHost
    {
        private NewEditKitFrm newKitFrm = null;

        public GKMainFrm()
        {
            InitializeComponent();

            GKSqlFuncs.SetHost(this);

            kitsExplorer.SetHost(this);
            kitsExplorer.SelectionChanged += kitsExplorer_SelectionChanged;
        }

        public void ChangeKits(IList<TestRecord> selectedKits)
        {
            var widget = panWidget.Controls.Count > 0 ? panWidget.Controls[0] as GKWidget : null;
            if (widget != null) {
                widget.SetKit(selectedKits);
                lblWidgetTitle.Text = widget.Text;
            }
        }

        #region Handlers

        private void GKMainFrm_Load(object sender, EventArgs e)
        {
            SetStatus("Checking Integrity of DB ...");
            this.Enabled = false;
            this.Text = Application.ProductName;

            Task.Factory.StartNew(() => {
                GKSqlFuncs.CheckIntegrity();

                this.Invoke(new MethodInvoker(delegate {
                    SetStatus("Done.");
                    this.Enabled = true;
                }));
            });
        }

        private void kitsExplorer_SelectionChanged(object sender, EventArgs e)
        {
            var selKits = kitsExplorer.SelectedKits;

            miOpen.Enabled = selKits != null && selKits.Count == 1;
            miDelete.Enabled = selKits != null && selKits.Count > 0;

            miAdmixture.Enabled = AdmixtureFrm.CanBeUsed(selKits);
            miISOGGYTree.Enabled = IsoggYTreeFrm.CanBeUsed(selKits);
            miOneToMany.Enabled = MatchingKitsFrm.CanBeUsed(selKits);
            miMitoMap.Enabled = MitoMapFrm.CanBeUsed(selKits);
            miMtDnaPhylogeny.Enabled = MtPhylogenyFrm.CanBeUsed(selKits);
            miOneToOne.Enabled = OneToOneCmpFrm.CanBeUsed(selKits);
            miRunsOfHomozygosity.Enabled = ROHFrm.CanBeUsed(selKits);
        }

        private void miExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void miNew_Click(object sender, EventArgs e)
        {
            NewKit();
        }

        private void miSave_Click(object sender, EventArgs e)
        {
            var widget = (panWidget.Controls.Count > 0) ? panWidget.Controls[0] : null;

            if (widget.Name == "NewEditKitFrm")
                ((NewEditKitFrm)widget).Save();
            else
                kitsExplorer.Save();
        }

        private void miOpen_Click(object sender, EventArgs e)
        {
            var selKit = kitsExplorer.SelectedKits[0];
            OpenKit(selKit.KitNo, selKit.Disabled);
        }

        private void miImport_Click(object sender, EventArgs e)
        {
            ImportTest();
        }

        private void miDelete_Click(object sender, EventArgs e)
        {
            DeleteKit();
        }

        private void miOneToOne_Click(object sender, EventArgs e)
        {
            ShowWidget(new OneToOneCmpFrm(this, kitsExplorer.SelectedKits));
        }

        private void miOneToMany_Click(object sender, EventArgs e)
        {
            ShowWidget(new MatchingKitsFrm(this, kitsExplorer.SelectedKits));
        }

        private void miProcessKits_Click(object sender, EventArgs e)
        {
            ShowWidget(new ProcessKitsFrm(this));
        }

        private void miAdmixture_Click(object sender, EventArgs e)
        {
            ShowWidget(new AdmixtureFrm(this, kitsExplorer.SelectedKits));
        }

        private void miRunsOfHomozygosity_Click(object sender, EventArgs e)
        {
            ShowWidget(new ROHFrm(this, kitsExplorer.SelectedKits));
        }

        private void miMtDnaPhylogeny_Click(object sender, EventArgs e)
        {
            ShowWidget(new MtPhylogenyFrm(this, kitsExplorer.SelectedKits));
        }

        private void miMitoMap_Click(object sender, EventArgs e)
        {
            ShowWidget(new MitoMapFrm(this, kitsExplorer.SelectedKits));
        }

        private void miISOGGYTree_Click(object sender, EventArgs e)
        {
            ShowWidget(new IsoggYTreeFrm(this, kitsExplorer.SelectedKits));
        }

        private void miPhasing_Click(object sender, EventArgs e)
        {
            ShowWidget(new PhasingFrm(this));
        }

        private void btnWidgetClose_Click(object sender, EventArgs e)
        {
            CloseWidgets();
        }

        private void miDevConsole_Click(object sender, EventArgs e)
        {
            ShowWidget(new DevConsole());
        }

        #endregion

        private void CloseWidgets()
        {
            lblWidgetTitle.Text = "...";
            foreach (Control fm in panWidget.Controls) fm.Dispose();
        }

        private void ShowWidget(GKWidget frm)
        {
            CloseWidgets();

            frm.Dock = DockStyle.Fill;
            panWidget.Controls.Add(frm);

            lblWidgetTitle.Text = frm.Text;
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

        public void NewKit()
        {
            if (newKitFrm == null || newKitFrm.IsDisposed)
                newKitFrm = new NewEditKitFrm(this, null, false);
            ShowWidget(newKitFrm);
        }

        public void OpenKit(string kit, bool disabled)
        {
            if (newKitFrm == null || newKitFrm.IsDisposed)
                newKitFrm = new NewEditKitFrm(this, kit, disabled);
            ShowWidget(newKitFrm);
        }

        public void DeleteKit()
        {
            kitsExplorer.Delete();
        }

        public void ImportTest()
        {
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

        public void EnableExplore()
        {
            kitsExplorer.Enabled = true;
            menuStripGGK.Enabled = true;
            btnWidgetClose.Enabled = true;
        }

        public void DisableExplore()
        {
            kitsExplorer.Enabled = false;
            menuStripGGK.Enabled = false;
            btnWidgetClose.Enabled = false;
        }

        public void ShowPhasedSegmentVisualizer(string kit1, string kit2, byte chr, int startPos, int endPos)
        {
            using (var frm = new PhasedSegmentFrm(kit1, kit2, chr, startPos, endPos))
                frm.ShowDialog(this);
        }

        public string SelectKit(char sex)
        {
            using (var open = new SelectKitFrm(sex)) {
                open.ShowDialog(this);
                return open.GetSelectedKit();
            }
        }

        public void SelectLocation(ref int lng, ref int lat)
        {
            using (var frm = new LocationSelectFrm(lng, lat)) {
                frm.ShowDialog(this);
                lng = frm.Longitude;
                lat = frm.Latitude;
            }
        }

        public void Exit()
        {
            Application.Exit();
        }

        public void ShowMessage(string msg)
        {
            MessageBox.Show(msg);
        }

        public bool ShowQuestion(string msg)
        {
            return (MessageBox.Show(msg, "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes);
        }
    }
}

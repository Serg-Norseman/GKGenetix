/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eto.Forms;
using Eto.Serialization.Xaml;
using GKGenetix.Core;
using GKGenetix.Core.Database;

namespace GKGenetix.UI.Forms
{
    public partial class GKMainFrm : Form, IKitHost
    {
        #region Design components
#pragma warning disable CS0169, CS0649, IDE0044, IDE0051

        private ButtonMenuItem miFile;
        private ButtonMenuItem miExit;
        private Label statusLbl;
        private ButtonMenuItem miNew;
        private ButtonMenuItem miOpen;
        private ButtonMenuItem miSave;
        private ProgressBar progressBar;
        private ButtonMenuItem miTest;
        private ButtonMenuItem miDelete;
        private ButtonMenuItem miDNA;
        private ButtonMenuItem miOneToOne;
        private ButtonMenuItem miOneToMany;
        private ButtonMenuItem miProcessKits;
        private ButtonMenuItem miAdmixture;
        private ButtonMenuItem miRunsOfHomozygosity;
        private ButtonMenuItem miMtDnaPhylogeny;
        private ButtonMenuItem miMitoMap;
        private ButtonMenuItem miISOGGYTree;
        private ButtonMenuItem miPhasing;
        private Button btnWidgetClose;
        private Label lblWidgetTitle;
        private Panel panWidget;
        private ButtonMenuItem miImport;
        private ButtonMenuItem miDevConsole;
        private KitsExplorer kitsExplorer;

#pragma warning restore CS0169, CS0649, IDE0044, IDE0051
        #endregion


        private NewEditKitFrm newKitFrm = null;
        private ITestProvider testProvider;

        public GKMainFrm()
        {
            XamlReader.Load(this);

            GKSqlFuncs.SetHost(this);

            kitsExplorer.SetHost(this);
            kitsExplorer.SelectionChanged += kitsExplorer_SelectionChanged;
        }

        public void ChangeKits(IList<TestRecord> selectedKits)
        {
            var widget = panWidget.Content as GKWidget;
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

            Task.Factory.StartNew(() => {
                GKSqlFuncs.CheckIntegrity();

                Application.Instance.Invoke(new Action(delegate {
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
            miImport.Enabled = (testProvider != null);

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
            Close();
        }

        private void miNew_Click(object sender, EventArgs e)
        {
            NewKit();
        }

        private void miSave_Click(object sender, EventArgs e)
        {
            var widget = panWidget.Content as GKWidget;
            if (widget == null) return;

            if (widget is NewEditKitFrm editFrm)
                editFrm.Save();
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
            ShowWidget(new DevConsole(this));
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

            this.SuspendLayout();
            panWidget.Content = frm;
            this.ResumeLayout();

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
            if (testProvider == null) return;

            var availableTests = testProvider.RequestTests();

            using (var frm = new ImportTestFrm(availableTests)) {
                frm.ShowModal(this);

                var test = frm.GetSelectedTest();

                if (newKitFrm == null || newKitFrm.IsDisposed)
                    newKitFrm = new NewEditKitFrm(this, null, false);
                ShowWidget(newKitFrm);

                newKitFrm.ImportFile(test);
            }
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
            miFile.Enabled = true;
            miTest.Enabled = true;
            miDNA.Enabled = true;
        }

        public void DisableToolbar()
        {
            miFile.Enabled = false;
            miTest.Enabled = false;
            miDNA.Enabled = false;
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
            EnableToolbar();
            btnWidgetClose.Enabled = true;
        }

        public void DisableExplore()
        {
            kitsExplorer.Enabled = false;
            DisableToolbar();
            btnWidgetClose.Enabled = false;
        }

        public void ShowPhasedSegmentVisualizer(string kit1, string kit2, byte chr, int startPos, int endPos)
        {
            using (var frm = new PhasedSegmentFrm(kit1, kit2, chr, startPos, endPos))
                frm.ShowModal(this);
        }

        public string SelectKit(char sex)
        {
            using (var open = new SelectKitFrm(sex)) {
                open.ShowModal(this);
                return open.GetSelectedKit();
            }
        }

        public void SelectLocation(ref double lng, ref double lat)
        {
            using (var frm = new LocationSelectFrm(lng, lat)) {
                frm.ShowModal(this);
                lng = frm.Longitude;
                lat = frm.Latitude;
            }
        }

        public void Exit()
        {
            Application.Instance.Quit();
        }

        public void ShowMessage(string msg)
        {
            MessageBox.Show(msg);
        }

        public bool ShowQuestion(string msg)
        {
            return (MessageBox.Show(msg, "Error", MessageBoxButtons.YesNo, MessageBoxType.Error) == DialogResult.Yes);
        }

        public void SetTestProvider(ITestProvider testProvider)
        {
            this.testProvider = testProvider;
        }

        public void SetAppDataPath(string path)
        {
            GKSqlFuncs.SetAppDataPath(path);
        }
    }
}

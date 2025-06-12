/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eto.Drawing;
using Eto.Forms;
using Eto.Serialization.Xaml;
using GKGenetix.Core;
using GKGenetix.Core.Database;
using GKGenetix.Core.Reference;

namespace GKGenetix.UI.Forms
{
    public partial class IsoggYTreeFrm : GKWidget
    {
        #region Design components
#pragma warning disable CS0169, CS0649, IDE0044, IDE0051

        private Label label1;
        private TreeView treeView1;
        private Label lblKitName;
        private TextArea txtSNPs;
        private Label lblyhg;
        private RichTextArea snpTextBox;

#pragma warning restore CS0169, CS0649, IDE0044, IDE0051
        #endregion


        private string kit = null;
        private IList<string> snpArray = null;


        public static bool CanBeUsed(IList<TestRecord> selectedKits)
        {
            return (selectedKits != null && selectedKits.Count == 1 && !selectedKits[0].Disabled && GKSqlFuncs.ExistsYSNPs(selectedKits[0].KitNo));
        }


        public IsoggYTreeFrm(IKitHost host, IList<TestRecord> selectedKits) : this(host, selectedKits[0].KitNo)
        {
        }

        public IsoggYTreeFrm(IKitHost host, string kit) : base(host)
        {
            XamlReader.Load(this);
            this.kit = kit;
        }

        public override void SetKit(IList<TestRecord> selectedKits)
        {
            if (CanBeUsed(selectedKits)) {
                this.kit = selectedKits[0].KitNo;
                ReloadData();
            }
        }

        private void ReloadData()
        {
            this.Text = "ISOGG Y-Tree : " + GKSqlFuncs.GetKitName(kit);
            lblKitName.Text = GKSqlFuncs.GetKitName(kit);
            _host.SetStatus("Plotting on ISOGG Y-Tree ...");

            var isoggYTree = RefData.ISOGGYTree;

            string kitSNPs = GKSqlFuncs.GetYSNPs(kit);
            txtSNPs.Text = kitSNPs;
            snpArray = GKGenFuncs.FilterSNPsOnYTree(kitSNPs);

            Task.Factory.StartNew(() => {
                var hg_maxpath = GKGenFuncs.FindYHaplogroup(isoggYTree, snpArray);

                Application.Instance.Invoke(new Action(delegate {
                    var snpMap = new List<TreeItem>();
                    //treeView1.BeginUpdate();
                    var root = new TreeNode("");
                    var rootAdam = new TreeNode("Adam");
                    root.Children.Add(rootAdam);
                    BuildTree(rootAdam, isoggYTree);
                    treeView1.DataStore = root;
                    //treeView1.CollapseAll();

                    if (hg_maxpath != null) {
                        var tn = treeView1.FindByTag(root, hg_maxpath);
                        //tn.EnsureVisible();
                        treeView1.SelectedItem = tn;
                        lblyhg.Text = tn.Text;
                    }
                    //treeView1.EndUpdate();

                    _host.SetStatus("Done.");
                }));
            });
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            ReloadData();
        }

        private void BuildTree(TreeItem treeNode, ISOGGYTreeNode pnNode)
        {
            treeNode.Tag = pnNode;

            /*switch (pnNode.Status) {
                case GKGenFuncs.HGS_DG:
                    treeNode.ForeColor = Colors.White;
                    treeNode.BackColor = Colors.DarkGreen;
                    break;

                case GKGenFuncs.HGS_LG:
                    treeNode.ForeColor = Colors.Orange;
                    treeNode.BackColor = Colors.LightGreen;
                    break;

                case GKGenFuncs.HGS_R:
                    treeNode.ForeColor = Colors.Yellow;
                    treeNode.BackColor = Colors.Red;
                    break;
            }*/

            foreach (var subnode in pnNode.Children) {
                var tn = new TreeNode(subnode.Name);
                treeNode.Children.Add(tn);
                BuildTree(tn, subnode);
            }
        }

        private void treeView1_AfterSelect(object sender, EventArgs e)
        {
            var phNode = (ISOGGYTreeNode)((TreeNode)treeView1.SelectedItem).Tag;
            string phMarkers = " " + phNode.Markers.Replace(",", ", ") + " ";

            label1.Text = "Defining SNPs for " + phNode.Name;

            snpTextBox.Text = phMarkers;
            snpTextBox.SelectAll();
            snpTextBox.SelectionForeground = Colors.Gray;

            var pnHgl = GKGenFuncs.GetYHighlights(phMarkers, snpArray);

            foreach (var hgl in pnHgl) {
                snpTextBox.Selection = new Range<int>(hgl.Start, hgl.Start + hgl.Length - 1);
                if (hgl.State == GKGenFuncs.HGS_R) {
                    snpTextBox.SelectionBackground = Colors.Red;
                    snpTextBox.SelectionForeground = Colors.Yellow;
                } else if (hgl.State == GKGenFuncs.HGS_DG) {
                    snpTextBox.SelectionBackground = Colors.DarkGreen;
                    snpTextBox.SelectionForeground = Colors.White;
                }
            }
        }
    }
}

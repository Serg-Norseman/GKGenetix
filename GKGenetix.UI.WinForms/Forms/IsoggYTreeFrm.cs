/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using GKGenetix.Core;
using GKGenetix.Core.Database;
using GKGenetix.Core.Reference;

namespace GKGenetix.UI.Forms
{
    public partial class IsoggYTreeFrm : GKWidget
    {
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
            InitializeComponent();
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

                this.Invoke(new MethodInvoker(delegate {
                    var snpMap = new List<TreeNode>();
                    treeView1.BeginUpdate();
                    var root = new TreeNode("Adam");
                    treeView1.Nodes.Add(root);
                    BuildTree(treeView1, root, isoggYTree);
                    treeView1.CollapseAll();

                    if (hg_maxpath != null) {
                        var tn = treeView1.FindByTag(root, hg_maxpath);
                        tn.EnsureVisible();
                        treeView1.SelectedNode = tn;
                        lblyhg.Text = tn.Text;
                    }
                    treeView1.EndUpdate();

                    _host.SetStatus("Done.");
                }));
            });
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            ReloadData();
        }

        private void BuildTree(TreeView treeView, TreeNode treeNode, ISOGGYTreeNode pnNode)
        {
            treeNode.Tag = pnNode;

            switch (pnNode.Status) {
                case GKGenFuncs.HGS_DG:
                    treeNode.ForeColor = Color.White;
                    treeNode.BackColor = Color.DarkGreen;
                    break;

                case GKGenFuncs.HGS_LG:
                    treeNode.ForeColor = Color.Orange;
                    treeNode.BackColor = Color.LightGreen;
                    break;

                case GKGenFuncs.HGS_R:
                    treeNode.ForeColor = Color.Yellow;
                    treeNode.BackColor = Color.Red;
                    break;
            }

            foreach (var subnode in pnNode.Children) {
                var tn = new TreeNode(subnode.Name);
                treeNode.Nodes.Add(tn);
                BuildTree(treeView, tn, subnode);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var phNode = (ISOGGYTreeNode)treeView1.SelectedNode.Tag;
            string phMarkers = " " + phNode.Markers.Replace(",", ", ") + " ";
            var pnHgl = GKGenFuncs.GetYHighlights(phMarkers, snpArray);

            label1.Text = "Defining SNPs for " + phNode.Name;
            snpTextBox.Text = phMarkers;
            snpTextBox.SelectAll();
            snpTextBox.SelectionColor = Color.Gray;

            foreach (var hgl in pnHgl) {
                snpTextBox.Select(hgl.Start, hgl.Length);
                if (hgl.State == GKGenFuncs.HGS_R) {
                    snpTextBox.SelectionBackColor = Color.Red;
                    snpTextBox.SelectionColor = Color.Yellow;
                } else if (hgl.State == GKGenFuncs.HGS_DG) {
                    snpTextBox.SelectionBackColor = Color.DarkGreen;
                    snpTextBox.SelectionColor = Color.White;
                }
            }
        }
    }
}

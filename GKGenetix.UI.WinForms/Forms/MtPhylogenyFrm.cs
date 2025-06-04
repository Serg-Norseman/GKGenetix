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
    public partial class MtPhylogenyFrm : GKWidget
    {
        private string kit = null;


        public static bool CanBeUsed(IList<TestRecord> selectedKits)
        {
            return (selectedKits != null && selectedKits.Count == 1 && !selectedKits[0].Disabled && GKSqlFuncs.ExistsMtDNA(selectedKits[0].KitNo));
        }


        public MtPhylogenyFrm(IKitHost host, IList<TestRecord> selectedKits) : this(host, selectedKits[0].KitNo)
        {
        }

        public MtPhylogenyFrm(IKitHost host, string kit) : base(host)
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
            lblKit.Text = $"{kit} ({GKSqlFuncs.GetKitName(kit)})";
            this.Text = $"Mitocondrial Phylogeny - {lblKit.Text}";

            var mtTree = RefData.MtTree;
            GKSqlFuncs.GetMtDNA(kit, out string mutations, out _);
            txtSNPs.Text = mutations;

            Task.Factory.StartNew(() => {
                var name_maxpath = GKGenFuncs.FindMtHaplogroup(mtTree, mutations, out string firstBest, out string secondBest);

                this.Invoke(new MethodInvoker(delegate {
                    lblFirstHG.Text = firstBest;
                    lblSecondHGs.Text = secondBest;

                    treeView1.BeginUpdate();
                    var root = new TreeNode("Eve");
                    treeView1.Nodes.Add(root);
                    BuildTree(treeView1, root, mtTree);
                    treeView1.CollapseAll();
                    var tnode = treeView1.FindByTag(root, name_maxpath);
                    if (tnode != null) {
                        tnode.EnsureVisible();
                        treeView1.SelectedNode = tnode;
                    }
                    treeView1.EndUpdate();
                }));
            });
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            ReloadData();
        }

        private void BuildTree(TreeView treeView, TreeNode treeNode, MtDNAPhylogenyNode pnNode)
        {
            treeNode.Tag = pnNode;

            if (pnNode.Status != GKGenFuncs.HGS_DG) {
                treeNode.ForeColor = Color.LightGray;
            } else {
                treeNode.ForeColor = Color.White;
                treeNode.BackColor = Color.DarkGreen;
            }

            foreach (var subnode in pnNode.Children) {
                var tn = new TreeNode(subnode.Name);
                treeNode.Nodes.Add(tn);
                BuildTree(treeView, tn, subnode);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = treeView1.SelectedNode;
            var markers = ((MtDNAPhylogenyNode)node.Tag).Markers;

            snpTextBox.Text = markers;
            string[] snps = txtSNPs.Text.Split(new char[] { ',' });
            foreach (string mutation in snps) {
                int loc = snpTextBox.Find(mutation.Trim());
                if (loc != -1) {
                    snpTextBox.SelectionStart = loc;
                    snpTextBox.SelectionLength = mutation.Trim().Length;
                    snpTextBox.SelectionBackColor = Color.DarkGreen;
                    snpTextBox.SelectionColor = Color.White;
                }
            }
        }
    }
}

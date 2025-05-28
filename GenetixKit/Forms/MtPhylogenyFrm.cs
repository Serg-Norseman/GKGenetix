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
using GenetixKit.Core;
using GKGenetix.Core.Model;

namespace GenetixKit.Forms
{
    public partial class MtPhylogenyFrm : GKWidget
    {
        private readonly string kit = null;


        public static bool CanBeUsed(IList<KitDTO> selectedKits)
        {
            return (selectedKits != null && selectedKits.Count == 1 && !selectedKits[0].Disabled && GKSqlFuncs.ExistsMtDna(selectedKits[0].KitNo));
        }


        public MtPhylogenyFrm(IList<KitDTO> selectedKits) : this(selectedKits[0].KitNo)
        {
        }

        public MtPhylogenyFrm(string kit)
        {
            InitializeComponent();
            this.kit = kit;
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            lblKit.Text = $"{kit} ({GKSqlFuncs.GetKitName(kit)})";
            this.Text = $"Mitocondrial Phylogeny - {lblKit.Text}";

            treeView1.BeginUpdate();
            var mtTree = GKData.MtTree;
            var root = new TreeNode("Eve");
            treeView1.Nodes.Add(root);
            var mutationsMap = new Dictionary<TreeNode, string>();
            BuildTree(treeView1, root, mtTree, mutationsMap);
            treeView1.CollapseAll();
            treeView1.EndUpdate();

            GKSqlFuncs.GetMtDNA(kit, out string mutations, out _);
            txtSNPs.Text = mutations;

            Task.Factory.StartNew(() => {
                var name_maxpath = GKGenFuncs.FindMtHaplogroup(mtTree, mutations, out string firstBest, out string secondBest);

                this.Invoke(new MethodInvoker(delegate {
                    treeView1.BeginUpdate();
                    lblFirstHG.Text = firstBest;
                    lblSecondHGs.Text = secondBest;
                    foreach (TreeNode node in mutationsMap.Keys) {
                        var pnNode = (MtDNAPhylogenyNode)node.Tag;
                        if (pnNode.Status != GKGenFuncs.HGS_DG) {
                            node.ForeColor = Color.LightGray;
                        } else {
                            node.ForeColor = Color.White;
                            node.BackColor = Color.DarkGreen;
                        }
                    }
                    var tnode = treeView1.FindByTag(root, name_maxpath);
                    if (tnode != null) {
                        tnode.EnsureVisible();
                        treeView1.SelectedNode = tnode;
                    }
                    treeView1.EndUpdate();
                }));
            });
        }

        private void BuildTree(TreeView treeView, TreeNode treeNode, MtDNAPhylogenyNode pnNode, Dictionary<TreeNode, string> mutationsMap)
        {
            treeNode.Tag = pnNode;
            treeNode.ForeColor = Color.Black;
            foreach (var subnode in pnNode.Children) {
                var tn = new TreeNode(subnode.Name);
                treeNode.Nodes.Add(tn);
                BuildTree(treeView, tn, subnode, mutationsMap);
            }
            mutationsMap.Add(treeNode, pnNode.Markers);
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

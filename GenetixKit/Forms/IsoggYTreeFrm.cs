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
    public partial class IsoggYTreeFrm : GKWidget
    {
        private readonly string kit = null;
        private IList<string> snpArray = null;


        public static bool CanBeUsed(IList<KitDTO> selectedKits)
        {
            return (selectedKits != null && selectedKits.Count == 1 && !selectedKits[0].Disabled && GKSqlFuncs.ExistsYDna(selectedKits[0].KitNo));
        }


        public IsoggYTreeFrm(IList<KitDTO> selectedKits) : this(selectedKits[0].KitNo)
        {
        }

        public IsoggYTreeFrm(string kit)
        {
            InitializeComponent();
            this.kit = kit;
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            lblKitName.Text = GKSqlFuncs.GetKitName(kit);
            Program.KitInstance.SetStatus("Plotting on ISOGG Y-Tree ...");

            var snpMap = new List<TreeNode>();
            treeView1.BeginUpdate();
            var isoggYTree = GKData.ISOGGYTree;
            var root = new TreeNode("Adam");
            treeView1.Nodes.Add(root);
            BuildTree(treeView1, root, isoggYTree, snpMap);
            treeView1.CollapseAll();
            treeView1.EndUpdate();

            string kitSNPs = GKSqlFuncs.GetYSNPs(kit);
            txtSNPs.Text = kitSNPs;
            snpArray = GKGenFuncs.FilterSNPsOnYTree(kitSNPs);

            Task.Factory.StartNew(() => {
                var hg_maxpath = GKGenFuncs.FindYHaplogroup(isoggYTree, snpArray);

                this.Invoke(new MethodInvoker(delegate {
                    treeView1.BeginUpdate();
                    foreach (TreeNode node in snpMap) {
                        var pnNode = ((ISOGGYTreeNode)node.Tag);
                        switch (pnNode.Status) {
                            case GKGenFuncs.HGS_DG:
                                node.ForeColor = Color.White;
                                node.BackColor = Color.DarkGreen;
                                break;

                            case GKGenFuncs.HGS_LG:
                                node.ForeColor = Color.Orange;
                                node.BackColor = Color.LightGreen;
                                break;

                            case GKGenFuncs.HGS_R:
                                node.ForeColor = Color.Yellow;
                                node.BackColor = Color.Red;
                                break;
                        }
                    }
                    if (hg_maxpath != null) {
                        var tn = treeView1.FindByTag(root, hg_maxpath);
                        tn.EnsureVisible();
                        treeView1.SelectedNode = tn;
                        lblyhg.Text = tn.Text;
                    }
                    treeView1.EndUpdate();

                    Program.KitInstance.SetStatus("Done.");
                }));
            });
        }

        private void BuildTree(TreeView treeView, TreeNode treeNode, ISOGGYTreeNode pnNode, List<TreeNode> snpMap)
        {
            treeNode.Tag = pnNode;
            foreach (var subnode in pnNode.Children) {
                var tn = new TreeNode(subnode.Name);
                treeNode.Nodes.Add(tn);
                BuildTree(treeView, tn, subnode, snpMap);
            }
            snpMap.Add(treeNode);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = treeView1.SelectedNode;
            label1.Text = "Defining SNPs for " + node.Text;

            snpTextBox.Text = " " + ((ISOGGYTreeNode)node.Tag).Markers.Replace(",", ", ") + " ";
            snpTextBox.SelectAll();
            snpTextBox.SelectionColor = Color.Gray;

            string[] begin = new string[] { " ", "/" };
            string[] end = new string[] { " ", "/", "," };
            foreach (string snp in snpArray) {
                if (snp.Equals("")) continue;

                foreach (string b1 in begin)
                    foreach (string e1 in end) {
                        string search_term = b1 + snp.Substring(0, snp.Length - 1) + e1;
                        int start = snpTextBox.Find(search_term);
                        if (start != -1) {
                            snpTextBox.Select(start + 1, snp.Length - 1);
                            if (snp.EndsWith("-")) {
                                snpTextBox.SelectionBackColor = Color.Red;
                                snpTextBox.SelectionColor = Color.Yellow;
                            } else if (snp.EndsWith("+")) {
                                snpTextBox.SelectionBackColor = Color.DarkGreen;
                                snpTextBox.SelectionColor = Color.White;
                            }
                        }
                    }
            }
        }
    }
}

/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GenetixKit.Core;
using GKGenetix.Core.Model;

namespace GenetixKit.Forms
{
    public partial class IsoggYTreeFrm : Form
    {
        private readonly string kit = null;
        private readonly List<TreeNode> snpMap = new List<TreeNode>();
        private IList<string> snpArray = null;

        public IsoggYTreeFrm(string kit)
        {
            InitializeComponent();
            this.kit = kit;
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            lblKitName.Text = GKSqlFuncs.GetKitName(kit);
            Program.KitInstance.SetStatus("Plotting on ISOGG Y-Tree ...");

            snpMap.Clear();
            treeView1.BeginUpdate();
            var isoggYTree = GKData.ISOGGYTree;
            TreeNode root = new TreeNode("Adam");
            treeView1.Nodes.Add(root);
            BuildTree(treeView1, root, isoggYTree);
            treeView1.CollapseAll();
            treeView1.EndUpdate();

            string kitSNPs = GKSqlFuncs.GetYSNPs(kit);
            txtSNPs.Text = kitSNPs;
            snpArray = FilterSNPsOnTree(kitSNPs);

            timer2.Enabled = true;
        }

        private void BuildTree(TreeView treeView, TreeNode treeNode, ISOGGYTreeNode pnNode)
        {
            treeNode.Tag = pnNode;
            foreach (var subnode in pnNode.Children) {
                var tn = new TreeNode(subnode.Name);
                treeNode.Nodes.Add(tn);
                BuildTree(treeView, tn, subnode);
            }
            snpMap.Add(treeNode);
        }

        private void MarkOnTree()
        {
            treeView1.BeginUpdate();

            foreach (TreeNode node in snpMap) {
                node.ForeColor = Color.Gray;
                node.BackColor = Color.White;
            }

            TreeNode hg_maxpath = null;
            foreach (TreeNode key in snpMap) {
                string hg_snps = ((ISOGGYTreeNode)key.Tag).Markers.Trim();
                if (hg_snps.Equals("-"))
                    continue;

                foreach (string hg_snp in hg_snps.Replace(" ", "").Split(new char[] { ',', '/' })) {
                    foreach (string snp in snpArray) {
                        string snp_ss = snp.Substring(0, snp.Length - 1).Trim();
                        if (hg_snp.Trim() != snp_ss) continue;

                        if (snp.EndsWith("-")) {
                            SetNodeVisual(key, '-');
                        } else if (snp.EndsWith("+")) {
                            SetNodeVisual(key, '+');

                            if (hg_maxpath == null) {
                                hg_maxpath = key;
                            } else {
                                if (key.FullPath.LastIndexOf('\\') > hg_maxpath.FullPath.LastIndexOf('\\') && key.Parent.BackColor != Color.Red)
                                    hg_maxpath = key;
                            }
                        }
                    }
                }
            }

            foreach (TreeNode node in snpMap) {
                if (node.BackColor != Color.DarkGreen && node.BackColor != Color.Red && node.BackColor != Color.LightGreen) {
                    node.ForeColor = Color.Gray;
                    node.BackColor = Color.White;
                }
            }

            if (hg_maxpath != null) {
                hg_maxpath.NodeFont = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Underline);
                hg_maxpath.EnsureVisible();
                treeView1.SelectedNode = hg_maxpath;
                lblyhg.Text = hg_maxpath.Text;
            }

            treeView1.EndUpdate();
        }

        private void SetNodeVisual(TreeNode key, char sign)
        {
            if (sign == '-') {
                if (key.ForeColor == Color.White && key.BackColor == Color.DarkGreen) {
                    key.ForeColor = Color.Orange;
                    key.BackColor = Color.LightGreen;
                } else if (key.ForeColor != Color.Orange && key.BackColor != Color.LightGreen) {
                    key.ForeColor = Color.Yellow;
                    key.BackColor = Color.Red;
                }
            } else if (sign == '+') {
                if (key.ForeColor == Color.Yellow && key.BackColor == Color.Red) {
                    key.ForeColor = Color.Orange;
                    key.BackColor = Color.LightGreen;
                } else if (key.ForeColor != Color.Orange && key.BackColor != Color.LightGreen) {
                    key.ForeColor = Color.White;
                    key.BackColor = Color.DarkGreen;
                }
            }
        }

        private IList<string> FilterSNPsOnTree(string kitSNPs)
        {
            var snpOnTree = new List<string>();
            snpOnTree.AddRange(Properties.Resources.snps_on_tree.Split(new char[] { ',' }));

            string[] entered_snps = kitSNPs.Replace(" ", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var valid_snps = new List<string>();
            foreach (string s in entered_snps) {
                // extract string without ending sign (-)
                string es = s.Substring(0, s.Length - 1);
                if (snpOnTree.Contains(es)) {
                    valid_snps.Add(s);
                }
            }
            return valid_snps;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            MarkOnTree();
            Program.KitInstance.SetStatus("Done.");
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = treeView1.SelectedNode;
            label1.Text = "Defining SNPs for " + node.Text;

            snpTextBox.Text = " " + ((ISOGGYTreeNode)node.Tag).Markers + " ";
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

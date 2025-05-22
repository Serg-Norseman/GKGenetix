/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using GenetixKit.Core;

namespace GenetixKit.Forms
{
    public partial class IsoggYTreeFrm : Form
    {
        private readonly string kit = null;
        private readonly Dictionary<TreeNode, string> snpMap = new Dictionary<TreeNode, string>();
        private readonly List<string> snpOnTree = new List<string>();
        private string my_snp = null;
        private string[] snpArray = null;

        public IsoggYTreeFrm(string kit)
        {
            InitializeComponent();
            this.kit = kit;
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            label2.Text = GKSqlFuncs.GetKitName(kit);
            txtSNPs.Text = GKSqlFuncs.QueryValue("kit_ysnps", new string[] { "ysnps" }, "where kit_no='" + kit + "'");
            Program.KitInstance.SetStatus("Plotting on ISOGG Y-Tree ...");

            XDocument doc = XDocument.Parse(Properties.Resources.ytree);

            treeView1.BeginUpdate();
            TreeNode root = new TreeNode("Adam");
            treeView1.Nodes.Add(root);
            foreach (XElement el in doc.Root.Elements()) {
                BuildTree(root, el);
            }
            root.Expand();
            treeView1.EndUpdate();

            snpOnTree.AddRange(Properties.Resources.snps_on_tree.Split(new char[] { ',' }));

            my_snp = txtSNPs.Text;
            snpArray = FilterSNPsOnTree(my_snp);

            timer2.Enabled = true;
        }

        private void BuildTree(TreeNode parent, XElement elmt)
        {
            TreeNode tn = null;
            XAttribute attrib_name = elmt.Attribute("name");
            XAttribute attrib_markers = elmt.Attribute("markers");
            if (attrib_name != null) {
                string attrib_value = attrib_name.Value.Trim();
                string value = attrib_markers.Value.Trim();
                value = value.Replace(",", ", ");

                if (attrib_value.IndexOf(',') != -1)
                    attrib_value = GKUtils.RemoveDuplicates(attrib_value);

                if (attrib_value != "") {
                    tn = new TreeNode(attrib_value);
                    snpMap.Add(tn, value);
                    parent.Nodes.Add(tn);
                }

                foreach (XElement el in elmt.Elements()) {
                    if (tn != null)
                        BuildTree(tn, el);
                    else
                        BuildTree(parent, el);
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            timer3.Enabled = true;
        }

        private void MarkOnTree()
        {
            foreach (TreeNode node in snpMap.Keys) {
                node.ForeColor = Color.Gray;
                node.BackColor = Color.White;
            }
            treeView1.CollapseAll();
            TreeNode hg_maxpath = null;

            string hg_snps = "";

            foreach (TreeNode key in snpMap.Keys) {
                hg_snps = (string)snpMap[key];
                if (hg_snps.Trim().Equals("-"))
                    continue;
                foreach (string hg_snp in hg_snps.Replace(" ", "").Split(new char[] { ',', '/' })) {
                    foreach (string snp in snpArray) {
                        if (snp.EndsWith("-")) {
                            if (hg_snp.Trim() == snp.Substring(0, snp.Length - 1).Trim()) {
                                if (key.ForeColor == Color.White && key.BackColor == Color.DarkGreen) {
                                    key.ForeColor = Color.Orange;
                                    key.BackColor = Color.LightGreen;
                                } else if (key.ForeColor != Color.Orange && key.BackColor != Color.LightGreen) {
                                    key.ForeColor = Color.Yellow;
                                    key.BackColor = Color.Red;
                                }
                                key.Collapse();
                                key.EnsureVisible();
                            }
                        } else if (snp.EndsWith("+")) {
                            if (hg_snp.Trim() == snp.Substring(0, snp.Length - 1).Trim()) {
                                if (key.ForeColor == Color.Yellow && key.BackColor == Color.Red) {
                                    key.ForeColor = Color.Orange;
                                    key.BackColor = Color.LightGreen;
                                } else if (key.ForeColor != Color.Orange && key.BackColor != Color.LightGreen) {
                                    key.ForeColor = Color.White;
                                    key.BackColor = Color.DarkGreen;
                                }
                                key.Expand();
                                key.EnsureVisible();
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
            }
            foreach (TreeNode node in snpMap.Keys) {
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
                BeautifyTree(hg_maxpath);
            }
        }

        private void BeautifyTree(TreeNode node)
        {
            if (node.Parent == null)
                return;
            foreach (TreeNode sibnode in node.Parent.Nodes) {
                if (sibnode == node)
                    continue;
                sibnode.Collapse();
            }
            BeautifyTree(node.Parent);
        }

        private string[] FilterSNPsOnTree(string my_snp)
        {
            string[] entered_snps = my_snp.Replace(" ", "").Split(new char[] { ',' });
            List<string> valid_snps = new List<string>();
            foreach (string s in entered_snps) {
                if (snpOnTree.Contains(s.Substring(0, s.Length - 1))) {
                    valid_snps.Add(s);
                }
            }
            return valid_snps.ToArray();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            MarkOnTree();
            Program.KitInstance.SetStatus("Done.");
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            timer3.Enabled = false;
            TreeNode node = treeView1.SelectedNode;
            snpTextBox.Text = " " + (string)snpMap[node] + " ";
            snpTextBox.SelectAll();
            snpTextBox.SelectionColor = Color.Gray;
            label1.Text = "Defining SNPs for " + node.Text;

            int start = -1;
            string[] begin = new string[] { " ", "/" };
            string[] end = new string[] { " ", "/", "," };
            string search_term = null;
            foreach (string snp in snpArray) {
                if (snp.Equals(""))
                    continue;
                foreach (string b1 in begin)
                    foreach (string e1 in end) {
                        search_term = b1 + snp.Substring(0, snp.Length - 1) + e1;
                        start = snpTextBox.Find(search_term);
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

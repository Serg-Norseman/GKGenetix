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
using GGKit.Core;
using GKGenetix.Core.Model;
using GKGenetix.UI;

namespace GGKit.Forms
{
    public partial class IsoggYTreeFrm : GKWidget
    {
        private string kit = null;
        private IList<string> snpArray = null;


        public static bool CanBeUsed(IList<KitDTO> selectedKits)
        {
            return (selectedKits != null && selectedKits.Count == 1 && !selectedKits[0].Disabled && GKSqlFuncs.ExistsYDna(selectedKits[0].KitNo));
        }


        public IsoggYTreeFrm(IKitHost host, IList<KitDTO> selectedKits) : this(host, selectedKits[0].KitNo)
        {
        }

        public IsoggYTreeFrm(IKitHost host, string kit) : base(host)
        {
            InitializeComponent();
            this.kit = kit;
        }

        public override void SetKit(IList<KitDTO> selectedKits)
        {
            if (CanBeUsed(selectedKits)) {
                this.kit = selectedKits[0].KitNo;
                ReloadData();
            }
        }

        private void ReloadData()
        {
            lblKitName.Text = GKSqlFuncs.GetKitName(kit);
            _host.SetStatus("Plotting on ISOGG Y-Tree ...");

            var isoggYTree = GKData.ISOGGYTree;

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

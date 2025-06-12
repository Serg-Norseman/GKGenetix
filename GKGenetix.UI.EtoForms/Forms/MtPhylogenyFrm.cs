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
    public partial class MtPhylogenyFrm : GKWidget
    {
        #region Design components
#pragma warning disable CS0169, CS0649, IDE0044, IDE0051

        private TreeView treeView1;
        private TextArea txtSNPs;
        private Label lblFirstHG;
        private Label lblSecondHGs;
        private RichTextArea snpTextBox;
        private Label lblKit;

#pragma warning restore CS0169, CS0649, IDE0044, IDE0051
        #endregion


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
            lblKit.Text = $"{kit} ({GKSqlFuncs.GetKitName(kit)})";
            this.Text = $"Mitocondrial Phylogeny - {lblKit.Text}";

            var mtTree = RefData.MtTree;
            GKSqlFuncs.GetMtDNA(kit, out string mutations, out _);
            txtSNPs.Text = mutations;

            Task.Factory.StartNew(() => {
                var name_maxpath = GKGenFuncs.FindMtHaplogroup(mtTree, mutations, out string firstBest, out string secondBest);

                Application.Instance.Invoke(new Action(delegate {
                    lblFirstHG.Text = firstBest;
                    lblSecondHGs.Text = secondBest;

                    //treeView1.BeginUpdate();
                    var root = new TreeNode("root");
                    var rootEve = new TreeNode("Eve");
                    root.Children.Add(rootEve);
                    BuildTree(rootEve, mtTree);
                    treeView1.DataStore = root;
                    //treeView1.CollapseAll();

                    var tnode = treeView1.FindByTag(root, name_maxpath);
                    if (tnode != null) {
                        //tnode.EnsureVisible();
                        treeView1.SelectedItem = tnode;
                    }
                    //treeView1.EndUpdate();
                }));
            });
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            ReloadData();
        }

        private void BuildTree(TreeNode treeNode, MtDNAPhylogenyNode pnNode)
        {
            treeNode.Tag = pnNode;

            /*if (pnNode.Status != GKGenFuncs.HGS_DG) {
                treeNode.ForeColor = Colors.LightSlateGray;
            } else {
                treeNode.ForeColor = Colors.White;
                treeNode.BackColor = Colors.DarkGreen;
            }*/

            foreach (var subnode in pnNode.Children) {
                var tn = new TreeNode(subnode.Name);
                treeNode.Children.Add(tn);
                BuildTree(tn, subnode);
            }
        }

        private void treeView1_AfterSelect(object sender, EventArgs e)
        {
            TreeNode node = treeView1.SelectedItem as TreeNode;
            var markers = ((MtDNAPhylogenyNode)node.Tag).Markers;
            string[] snps = txtSNPs.Text.Split(new char[] { ',' });

            var mtHgl = GKGenFuncs.GetMtHighlights(markers, snps);

            snpTextBox.Text = markers;
            foreach (var hgl in mtHgl) {
                snpTextBox.Selection = new Range<int>(hgl.Start, hgl.Start + hgl.Length - 1);
                snpTextBox.SelectionBackground = Colors.DarkGreen;
                snpTextBox.SelectionForeground = Colors.White;
            }
        }
    }
}

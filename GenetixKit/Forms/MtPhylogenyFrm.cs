/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GenetixKit.Core;
using GKGenetix.Core.Model;

namespace GenetixKit.Forms
{
    public partial class MtPhylogenyFrm : GKWidget
    {
        private readonly Dictionary<TreeNode, string> mutationsMap = new Dictionary<TreeNode, string>();
        private readonly SortedDictionary<int, List<TreeNode>> sorted_hg_readjustment = new SortedDictionary<int, List<TreeNode>>();
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
            lblKit.Text = kit + " - " + GKSqlFuncs.GetKitName(kit);
            this.Text = $"Mitocondrial Phylogeny - {kit} ({GKSqlFuncs.GetKitName(kit)})";

            treeView1.BeginUpdate();
            var mtTree = GKData.MtTree;
            TreeNode root = new TreeNode("Eve");
            treeView1.Nodes.Add(root);
            BuildTree(treeView1, root, mtTree);
            treeView1.CollapseAll();
            treeView1.EndUpdate();

            GKSqlFuncs.GetMtDNA(kit, out string mutations, out _);
            txtSNPs.Text = mutations;

            MarkOnTree();
        }

        private void BuildTree(TreeView treeView, TreeNode treeNode, MtDNAPhylogenyNode pnNode)
        {
            treeNode.Tag = pnNode;
            treeNode.ForeColor = Color.Black;

            foreach (var subnode in pnNode.Children) {
                var tn = new TreeNode(subnode.Name);
                treeNode.Nodes.Add(tn);
                BuildTree(treeView, tn, subnode);
            }

            mutationsMap.Add(treeNode, pnNode.Markers);
        }

        public void MarkOnTree()
        {
            treeView1.CollapseAll();
            TreeNode name_maxpath = null;

            string kitMarkers = txtSNPs.Text;
            // dirty but ok for now
            kitMarkers = kitMarkers.Replace(",", " ").Replace("\t", " ").Replace("\r", " ").Replace("\n", " ");

            Regex regex = new Regex(@"[ ]{2,}", RegexOptions.None);
            kitMarkers = regex.Replace(kitMarkers, @" ");
            kitMarkers = kitMarkers.Replace(" ", ",");

            string[] marker_array = kitMarkers.Split(",".ToCharArray());

            var list = new List<TreeNode>();
            foreach (TreeNode key in mutationsMap.Keys) {
                string marker_names = mutationsMap[key];
                string[] marker_names_on_hg = marker_names.Split(",".ToCharArray());
                foreach (string marker_name in marker_names_on_hg) {
                    string m_name = marker_name.Replace("(", "").Replace(")", "").Replace("!", "").Trim();

                    foreach (string marker in marker_array) {
                        if (marker.Trim().IndexOf(m_name) != -1 || m_name == marker.Trim()) {
                            if (!list.Contains(key))
                                list.Add(key);

                            key.ForeColor = Color.White;
                            key.BackColor = Color.DarkGreen;
                            key.EnsureVisible();
                        }
                    }
                }
            }

            // go through all terminals with matches and count the number of matching parents. now, that's the score.
            var best_score = new SortedList<int, List<TreeNode>>();
            foreach (TreeNode key in list) {
                int pm = GetParentMatches(key);
                var alist = best_score.ContainsKey(pm) ? best_score[pm] : new List<TreeNode>();
                alist.Add(key);
                best_score.Remove(pm);
                best_score.Add(pm, alist);
            }
            var desc = best_score.Reverse();

            bool found_first = false;
            bool found_second = false;

            sorted_hg_readjustment.Clear();
            foreach (KeyValuePair<int, List<TreeNode>> kvp in desc) {
                var mlist = kvp.Value;
                string str = "";
                if (!found_first) {
                    foreach (TreeNode tn in mlist) {
                        if (IsMatchingAll(tn, marker_array)) {
                            name_maxpath = tn;
                            str = str + " " + tn.Text;
                            found_first = true;
                        }
                    }
                    lblFirstHG.Text = str.Trim().Replace(" ", ", ");
                    if (found_first)
                        continue;
                } else if (!found_second) {
                    foreach (TreeNode tn in mlist) {
                        if (IsMatchingAll(tn, marker_array)) {
                            //name_maxpath = tn;
                            str = str + " " + tn.Text;
                            found_second = true;
                        }
                    }
                    lblSecondHGs.Text = str.Trim().Replace(" ", ", ");
                    if (found_second)
                        break;
                }
            }

            // --  final readjustment .. it's dirty
            found_first = false;
            found_second = false;
            string mstr = "";
            foreach (KeyValuePair<int, List<TreeNode>> hg in sorted_hg_readjustment) {
                var m_list = hg.Value;
                foreach (TreeNode mhg in m_list)
                    mstr = mstr + " " + mhg.Text;

                var mstr_tr = mstr.Trim().Replace(" ", ", ");

                if (!found_first) {
                    name_maxpath = m_list[0];
                    lblFirstHG.Text = mstr_tr;
                    found_first = true;
                    mstr = "";
                } else if (!found_second) {
                    lblSecondHGs.Text = mstr_tr;
                    break;
                }
            }

            foreach (TreeNode node in mutationsMap.Keys) {
                if (node.BackColor != Color.DarkGreen) {
                    node.ForeColor = Color.LightGray;
                }
            }

            if (name_maxpath != null) {
                name_maxpath.EnsureVisible();
                treeView1.SelectedNode = name_maxpath;
            }
        }

        private bool IsMatchingAll(TreeNode key, string[] kitMarkers)
        {
            var hgs = GetAllMatchingInParent(key);

            var hgs_found = new List<string>();
            var mismatches = new List<string>();
            var matches = new List<string>();

            foreach (string u_marker in kitMarkers) {
                bool ignore_found = false;
                foreach (string i_marker in GKData.MtIgnoreList) {
                    if (u_marker == i_marker || u_marker.StartsWith(i_marker) || u_marker.Substring(1).StartsWith(i_marker)) {
                        ignore_found = true;
                        break;
                    }
                }
                if (ignore_found) continue;

                //check u_marker in haplogroups
                bool found = false;
                string hg_found = "";
                foreach (string markers in hgs) {
                    string parent_markers = markers.Substring(markers.IndexOf(":") + 1);
                    string parent_hg = markers.Substring(0, markers.IndexOf(":"));
                    if (parent_markers.IndexOf(u_marker) != -1) {
                        found = true;
                        hg_found = markers;
                        break;
                    }
                }

                if (found) {
                    if (!hgs_found.Contains(hg_found))
                        hgs_found.Add(hg_found);

                    matches.Add(u_marker);
                } else
                    mismatches.Add(u_marker);
            }

            if (hgs.Count == hgs_found.Count) {
                var tnList = new List<TreeNode>();
                if (sorted_hg_readjustment.ContainsKey(mismatches.Count))
                    tnList = sorted_hg_readjustment[mismatches.Count];
                tnList.Add(key);
                sorted_hg_readjustment.Remove(mismatches.Count);
                sorted_hg_readjustment.Add(mismatches.Count, tnList);

                return true;
            } else
                return false;
        }

        private List<string> GetAllMatchingInParent(TreeNode key)
        {
            var list = new List<string>();
            list.Add(key.Text + ":" + mutationsMap[key]);

            TreeNode parent = key.Parent;
            while (true) {
                if (parent.Text == "Eve")
                    break;
                if (parent.BackColor == Color.DarkGreen) {
                    list.Add(parent.Text + ":" + mutationsMap[parent]);
                }
                parent = parent.Parent;
            }
            return list;
        }

        private int GetParentMatches(TreeNode key)
        {
            int match = key.BackColor == Color.DarkGreen ? 1 : 0;
            if (key.Parent != null)
                return match + GetParentMatches(key.Parent);
            else
                return match;
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

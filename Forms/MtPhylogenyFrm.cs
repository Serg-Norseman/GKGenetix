/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using GenetixKit.Core;

namespace GenetixKit.Forms
{
    public partial class MtPhylogenyFrm : Form
    {
        private readonly Dictionary<TreeNode, string> mutationsMap = new Dictionary<TreeNode, string>();
        private readonly Dictionary<TreeNode, string> matchMap = new Dictionary<TreeNode, string>();
        private readonly string[] ignoreList = new string[] { "309.1C", "315.1C", "515", "516", "517", "518", "519", "520", "521", "522", "16182C", "16183C", "16193.1C", "16519" };
        private readonly StringBuilder report = new StringBuilder();
        private readonly SortedDictionary<int, List<string>> sorted_report_array = new SortedDictionary<int, List<string>>();
        private readonly SortedDictionary<int, List<TreeNode>> sorted_hg_readjustment = new SortedDictionary<int, List<TreeNode>>();
        private readonly string kit = null;
        private string xmlPhylogeny = null;

        public MtPhylogenyFrm(string kit)
        {
            InitializeComponent();
            this.kit = kit;
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            label2.Text = kit + " - " + GKSqlFuncs.GetKitName(kit);
            this.Text = $"Mitocondrial Phylogeny - {kit} ({GKSqlFuncs.GetKitName(kit)})";
            xmlPhylogeny = Properties.Resources.mtDNAPhylogeny;

            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            Program.KitInstance.EnableSave();
            XDocument doc = XDocument.Parse(xmlPhylogeny);

            treeView1.BeginUpdate();
            TreeNode root = new TreeNode("Eve");
            treeView1.Nodes.Add(root);
            foreach (XElement el in doc.Root.Elements()) {
                BuildTree(root, el);
            }
            root.Expand();
            treeView1.EndUpdate();

            GKSqlFuncs.GetMtDNA(kit, out string mutations, out _);
            txtSNPs.Text = mutations;

            MarkOnTree();
        }

        private void BuildTree(TreeNode parent, XElement elmt)
        {
            TreeNode tn = null;
            XAttribute attrib = elmt.Attribute("Id");
            string attrib_value = attrib.Value.Trim();
            XAttribute attrib_val = elmt.Attribute("HG");
            string value = attrib_val.Value.Trim();

            if (attrib_value != "") {
                tn = new TreeNode(attrib_value);
                mutationsMap.Add(tn, value);
                parent.Nodes.Add(tn);
            }

            foreach (XElement el in elmt.Elements()) {
                if (tn != null)
                    BuildTree(tn, el);
                else
                    BuildTree(parent, el);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = treeView1.SelectedNode;
            snpTextBox.Clear();
            snpTextBox.Text = (string)mutationsMap[node];
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

        public void MarkOnTree()
        {
            matchMap.Clear();
            foreach (TreeNode node in mutationsMap.Keys) {
                node.ForeColor = Color.Black;
            }
            treeView1.CollapseAll();
            TreeNode name_maxpath = null;

            string my_marker = txtSNPs.Text;
            //dirty but ok for now
            my_marker = my_marker.Replace(",", " ").Replace("\t", " ").Replace("\r", " ").Replace("\n", " ");
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex(@"[ ]{2,}", options);
            my_marker = regex.Replace(my_marker, @" ");
            my_marker = my_marker.Replace(" ", ",");
            string[] marker_array = my_marker.Split(",".ToCharArray());
            string marker_names = "";
            string m_name = "";

            string[] marker_names_on_hg = null;
            var list = new List<TreeNode>();
            foreach (TreeNode key in mutationsMap.Keys) {
                marker_names = (string)mutationsMap[key];
                marker_names_on_hg = marker_names.Split(",".ToCharArray());
                foreach (string marker_name in marker_names_on_hg) {
                    m_name = marker_name.Replace("(", "").Replace(")", "").Replace("!", "");

                    foreach (string marker in marker_array) {
                        if (marker.Trim().IndexOf(m_name.Trim()) != -1 || m_name.Trim() == marker.Trim()) {
                            //if (key.GetNodeCount(true) == 0)
                            if (!list.Contains(key))
                                list.Add(key);
                            if (!matchMap.ContainsKey(key))
                                matchMap.Add(key, m_name);
                            key.ForeColor = Color.White;
                            key.BackColor = Color.DarkGreen;
                            key.EnsureVisible();
                        }
                    }
                }
            }

            // go through all terminals with matches and count the number of matching parents. now, that's the score.
            var best_score = new SortedList<int, List<TreeNode>>();
            int pm = 0;
            var alist = new List<TreeNode>();
            foreach (TreeNode key in list) {
                pm = GetParentMatches(key);
                alist = best_score.ContainsKey(pm) ? best_score[pm] : new List<TreeNode>();
                alist.Add(key);
                best_score.Remove(pm);
                best_score.Add(pm, alist);
            }

            var desc = best_score.Reverse();

            List<TreeNode> mlist = null;
            bool found_first = false;
            bool found_second = false;

            report.Clear();
            report.Append("<html><head><title>mtDNA Report for " + GKSqlFuncs.GetKitName(kit) + " (" + kit + ")</title></head><body>");
            report.Append("<h1>mtDNA Report for " + GKSqlFuncs.GetKitName(kit) + " (" + kit + ")</h2>");
            report.Append("<b>User Entered Markers: </b>" + my_marker);

            sorted_report_array.Clear();
            sorted_hg_readjustment.Clear();
            foreach (var item in desc) {
                KeyValuePair<int, List<TreeNode>> kvp = item;

                mlist = kvp.Value;
                string str = "";
                if (!found_first) {
                    foreach (TreeNode tn in mlist) {
                        if (IsMatchingAll(tn, marker_array)) {
                            name_maxpath = tn;
                            str = str + " " + tn.Text;
                            found_first = true;
                        }
                    }
                    lblyhg.Text = str.Trim().Replace(" ", ", ");
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
                    lblSb.Text = str.Trim().Replace(" ", ", ");
                    if (found_second)
                        break;
                }
            }

            // --  final readjustment .. it's dirty
            found_first = false;
            found_second = false;
            string mstr = "";
            List<TreeNode> m_list = null;
            foreach (KeyValuePair<int, List<TreeNode>> hg in sorted_hg_readjustment) {
                m_list = hg.Value;
                foreach (TreeNode mhg in m_list)
                    mstr = mstr + " " + mhg.Text;

                if (!found_first) {
                    name_maxpath = (TreeNode)m_list[0];
                    lblyhg.Text = mstr.Trim().Replace(" ", ", ");
                    found_first = true;
                    mstr = "";
                } else if (!found_second) {
                    lblSb.Text = mstr.Trim().Replace(" ", ", ");
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

        private bool IsMatchingAll(TreeNode key, string[] user_markers)
        {
            var hgs = GetAllMatchingInParent(key);
            var hgs_found = new List<string>();
            var report_stmt = new Dictionary<string, string>();
            StringBuilder report = new StringBuilder(); //local variable
            var mismatches = new List<string>();
            var matches = new List<string>();
            foreach (string u_marker in user_markers) {
                bool ignore_found = false;
                foreach (string i_marker in ignoreList) {
                    if (u_marker == i_marker || u_marker.StartsWith(i_marker) || u_marker.Substring(1).StartsWith(i_marker)) {
                        ignore_found = true;
                        break;
                    }
                }

                if (ignore_found) {
                    continue;
                }

                bool found = false;
                //check u_marker in haplogroups
                string hg_found = "";
                foreach (string markers in hgs) {
                    string parent_markers = markers.Substring(markers.IndexOf(":") + 1);
                    string parent_hg = markers.Substring(0, markers.IndexOf(":"));
                    if (parent_markers.IndexOf(u_marker) != -1) {
                        found = true;
                        hg_found = markers;
                        string rpt_markers = report_stmt.ContainsKey(parent_hg) ? (string)report_stmt[parent_hg] : parent_markers;
                        rpt_markers = rpt_markers.Replace(u_marker, "<font color='darkgreen'><b>" + u_marker + "</b></font>");
                        report_stmt.Remove(parent_hg);
                        report_stmt.Add(parent_hg, rpt_markers);
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
                report.Append("<h3>Haplogroup " + key.Text + "</h3>");
                var lines = new List<string>();
                if (key.Text == "NoLabel")
                    lines.Add(" ⇨ (" + report_stmt[key.Text] + ")");
                else
                    lines.Add(" ⇨ <b>" + key.Text + "</b> (" + report_stmt[key.Text] + ")");
                TreeNode parent = key.Parent;
                while (true) {
                    if (parent.Text == "Eve")
                        break;
                    if (parent.BackColor == Color.DarkGreen) {
                        if (parent.Text == "NoLabel")
                            lines.Add(" ⇨ (" + report_stmt[parent.Text] + ")");
                        else
                            lines.Add(" ⇨ <b>" + parent.Text + "</b> (" + report_stmt[parent.Text] + ")");
                    }
                    parent = parent.Parent;
                }
                lines.Add("♀ Eve");
                int count = 1;
                for (int i = lines.Count - 1; i >= 0; i--) {
                    for (int j = 0; j < count * 2; j++)
                        report.Append("&nbsp;");
                    report.Append(lines[i]);
                    report.Append("<br>");
                    count++;
                }

                report.Append("Matches on Tree (" + matches.Count.ToString() + ") : ");
                foreach (string m in matches) {
                    report.Append("<font color='darkgreen'>" + m + "</font>");
                    report.Append(" ");
                }
                report.Append("<br>");
                report.Append("Mismatches/Extras (" + mismatches.Count.ToString() + ") : ");
                foreach (string m in mismatches) {
                    report.Append("<font color='red'>" + m + "</font>");
                    report.Append(" ");
                }
                report.Append("<br>");

                List<string> list = new List<string>();
                if (sorted_report_array.ContainsKey(mismatches.Count))
                    list = sorted_report_array[mismatches.Count];
                list.Add(report.ToString());
                sorted_report_array.Remove(mismatches.Count);
                sorted_report_array.Add(mismatches.Count, list);

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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Genetic Genealogy Kit (GGK) will try to use the internet and connect to mtdnacommunity.org to fetch the latest Human mtDNA Phylogeny XML. Are you sure you want to do this?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                try {
                    string url = GKSettings.Phylogeny_mtDNA_URL;
                    Program.KitInstance.SetStatus("Fetching.. " + url);
                    backgroundWorker1.RunWorkerAsync(url);
                } catch (Exception ee) {
                    MessageBox.Show("Technical Details: " + ee.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string url = e.Argument.ToString();
            WebClient client = new WebClient();
            xmlPhylogeny = client.DownloadString(url);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            treeView1.Nodes.Clear();
            lblSb.Text = "";
            lblyhg.Text = "";
            mutationsMap.Clear();
            matchMap.Clear();
            Program.KitInstance.SetStatus("Done.");
            timer1.Enabled = true;
        }
    }
}

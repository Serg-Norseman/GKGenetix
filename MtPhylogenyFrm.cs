using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Net;
using System.Windows.Forms.DataVisualization.Charting;

namespace Genetic_Genealogy_Kit
{
    public partial class MtPhylogenyFrm : Form
    {
        Hashtable mutations_map = new Hashtable();
        Hashtable match_map = new Hashtable();
        string[] ignore_list = new string[] { "309.1C", "315.1C", 
            "515","516","517","518","519","520","521","522", "16182C", "16183C", "16193.1C","16519"};
        StringBuilder report = new StringBuilder();
        SortedDictionary<int, ArrayList> sorted_report_array = new SortedDictionary<int, ArrayList>();

        SortedDictionary<int, ArrayList> sorted_hg_readjustment = new SortedDictionary<int, ArrayList>();
        string kit = null;
        string xml_phylogeny = null;        

        public MtPhylogenyFrm(string kit)
        {
            InitializeComponent();
            this.kit = kit;
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            label2.Text = kit + " - " + GGKUtilLib.getKitName(kit);
            this.Text = "Mitocondrial Phylogeny - (" + kit + ")" + GGKUtilLib.getKitName(kit);
            xml_phylogeny = Genetic_Genealogy_Kit.Properties.Resources.mtDNAPhylogeny;
            timer1.Enabled = true;             
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            GGKUtilLib.enableSave();
            //
            XDocument doc = XDocument.Parse(xml_phylogeny);

            TreeNode root = new TreeNode("Eve");
            treeView1.Nodes.Add(root);

            foreach (XElement el in doc.Root.Elements())
            {
                buildTree(root,el);    
            }
            root.Expand();
            //
            string mutations = GGKUtilLib.queryValue("kit_mtdna", new string[] { "mutations" }, "where kit_no='" + kit + "'");
            txtSNPs.Text = mutations;
            //
            markOnTree();
        }


        private void buildTree(TreeNode parent,XElement elmt)
        {
            TreeNode tn = null;
            XAttribute attrib = null;
            XAttribute attrib_val = null;
            attrib = elmt.Attribute("Id");
            string attrib_value = attrib.Value.Trim();
            attrib_val = elmt.Attribute("HG");
            string value = attrib_val.Value.Trim();
            //string value = elmt.FirstNode.ToString().Replace("\r", "").Replace("\n", "").Replace("\t", "");
            //value = value.ToUpper().Trim().Replace(" ", ", ");
            if (attrib_value != "")
            {
                tn = new TreeNode(attrib_value);
                //if (parent.Text!="Eve")
                //    value = mutations_map[parent] +", "+ value;
                mutations_map.Add(tn, value);
                parent.Nodes.Add(tn);
            }
             
            //
            foreach (XElement el in elmt.Elements())
            {
                if (tn != null)
                    buildTree(tn, el);
                else
                    buildTree(parent, el);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = treeView1.SelectedNode;
            snpTextBox.Clear();
            snpTextBox.Text = (string)mutations_map[node];            
            string[] snps = txtSNPs.Text.Split(new char[]{','});
            int loc = 0;
            foreach(string mutation in snps)
            {
                loc = snpTextBox.Find(mutation.Trim());
                if(loc!=-1)
                {
                    snpTextBox.SelectionStart = loc;
                    snpTextBox.SelectionLength = mutation.Trim().Length;
                    snpTextBox.SelectionBackColor = Color.DarkGreen;
                    snpTextBox.SelectionColor = Color.White;
                }
            }

        }

        public void markOnTree()
        {
            match_map.Clear();
            foreach (TreeNode node in mutations_map.Keys)
            {
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
            ArrayList list = new ArrayList();
            foreach (TreeNode key in mutations_map.Keys)
            {
                marker_names= (string)mutations_map[key];
                marker_names_on_hg=marker_names.Split(",".ToCharArray());
                foreach (string marker_name in marker_names_on_hg)
                {
                    m_name = marker_name.Replace("(", "").Replace(")", "").Replace("!", "");
                    
                    foreach (string marker in marker_array)
                    {
                        // if on ignore list, just ignore
                        //foreach (string ignore in ignore_list)
                        //{
                        //    if (marker.IndexOf(ignore) != -1)
                        //        continue;
                        //}

                        //if (m_name.Trim() == marker.Trim())
                        if (marker.Trim().IndexOf(m_name.Trim()) != -1 || m_name.Trim() == marker.Trim())
                        {
                            //if (key.GetNodeCount(true) == 0)
                            if(!list.Contains(key))
                                list.Add(key);
                            if (!match_map.ContainsKey(key))
                                match_map.Add(key, m_name);
                            key.ForeColor = Color.White;
                            key.BackColor = Color.DarkGreen;
                            key.EnsureVisible();
                            //break;
                        }
                    }                    
                }
            }

            //
            // go through all terminals with matches and count the number of matching parents. now, that's the score.
            SortedList<int, ArrayList> best_score = new SortedList<int, ArrayList>();
            int pm = 0;
            ArrayList alist = new ArrayList();
            foreach (TreeNode key in list)
            {
                pm=getParentMatches(key);
                if (best_score.ContainsKey(pm))
                    alist = best_score[pm];
                else
                    alist = new ArrayList();
                alist.Add(key);
                best_score.Remove(pm);
                best_score.Add(pm, alist);
            }

            var desc = best_score.Reverse();
            //     
            //int count = 1;
            ArrayList mlist = null;
            bool found_first = false;
            bool found_second = false;
            report.Clear();            
            //
            report.Append("<html><head><title>mtDNA Report for "+GGKUtilLib.getKitName(kit)+" ("+kit+")</title></head><body>");
            report.Append("<h1>mtDNA Report for " + GGKUtilLib.getKitName(kit) + " (" + kit + ")</h2>");
            report.Append("<b>User Entered Markers: </b>" + my_marker);

            sorted_report_array.Clear();
            sorted_hg_readjustment.Clear();
            foreach (var item in desc)
            {                
                KeyValuePair<int, ArrayList> kvp = item;
                //
                mlist = (ArrayList)kvp.Value;
                string str = "";
                if (!found_first)
                {
                    foreach (TreeNode tn in mlist)
                    {
                        if (isMatchingAll(tn, marker_array))
                        {
                            name_maxpath = tn;
                            str = str + " " + tn.Text;
                            found_first = true;
                        }
                    }
                    lblyhg.Text = str.Trim().Replace(" ", ", ");
                    if (found_first)
                        continue;
                }
                else if (!found_second)
                {
                    foreach (TreeNode tn in mlist)
                    {
                        if (isMatchingAll(tn, marker_array))
                        {
                            //name_maxpath = tn;
                            str = str + " " + tn.Text;
                            found_second = true;
                        }
                    }
                    lblSb.Text = str.Trim().Replace(" ", ", ");
                    if (found_second)
                        break;
                }
                //
                //if (count == 1)
                //{
                //    mlist = (ArrayList)kvp.Value;
                //    string str = "";
                //    foreach (TreeNode tn in mlist)
                //    {
                //        if (isMatchingAll(tn, marker_array))
                //        {
                //            name_maxpath = tn;
                //            str = str + " " + tn.Text;
                //        }
                //    }
                //    lblyhg.Text = str.Trim().Replace(" ", ", ");
                //}
                //else if (count == 2)
                //{
                //    mlist = (ArrayList)kvp.Value;
                //    string str = "";
                //    foreach (TreeNode tn in mlist)
                //    {
                //        if (isMatchingAll(tn, marker_array))
                //        {
                //            str = str + " " + tn.Text;
                //        }
                //    }
                //    lblSb.Text = str.Trim().Replace(" ",", ");
                //}
                //else if(count>2)
                //    break;
                //count++;
            }

            // --  final readjustment .. it's dirty
            found_first = false;
            found_second = false;
            string mstr = "";
            ArrayList m_list = null;
            foreach (KeyValuePair<int, ArrayList> hg in sorted_hg_readjustment)
            {
                m_list = hg.Value;
                foreach (TreeNode mhg in m_list)
                    mstr = mstr + " " + mhg.Text;

                if (!found_first)
                {
                    name_maxpath = (TreeNode)m_list[0];
                    lblyhg.Text = mstr.Trim().Replace(" ", ", ");
                    found_first = true;
                    mstr = "";
                }
                else if (!found_second)
                {
                    lblSb.Text = mstr.Trim().Replace(" ", ", ");
                    break;
                }
            }
            //


            foreach (TreeNode node in mutations_map.Keys)
            {
                if (node.BackColor != Color.DarkGreen)
                {
                    node.ForeColor = Color.LightGray;
                }
            }
            if (name_maxpath != null)
            {
                //name_maxpath.NodeFont = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Underline);
                name_maxpath.EnsureVisible();
                //lblyhg.Text = name_maxpath.Text;
                //name_maxpath
                treeView1.SelectedNode = name_maxpath;
            }

            //TreeNode parent=name_maxpath;
            //mtchart.Series.Clear();
            //while(parent.Text!="Eve")
            //{
            //    Series series = mtchart.Series.Add(parent.Text);

            //    int count = mutations_map[parent].ToString().Split(new char[]{','}).Length;
            //    series.ChartType = SeriesChartType.StackedBar;
            //    series.LabelToolTip = parent.Text+" ("+count.ToString() + " mutations)";
            //    series.Points.AddY(count);
            //    parent = parent.Parent;
            //}
        }

        private bool isMatchingAll(TreeNode key, string[] user_markers)
        {         
            ArrayList hgs = getAllMatchingInParent(key);
            bool found = false;
            bool ignore_found = false;
            string hg_found = null;
            string parent_hg = null;
            string parent_markers = null;
            ArrayList hgs_found = new ArrayList();
            Hashtable report_stmt = new Hashtable();
            String rpt_markers = "";
            StringBuilder report = new StringBuilder(); //local variable
            ArrayList mismatches = new ArrayList();
            ArrayList matches = new ArrayList();
            foreach (string u_marker in user_markers)
            {
                //
                ignore_found = false;
                foreach (string i_marker in ignore_list)
                {
                    if (u_marker == i_marker || u_marker.StartsWith(i_marker) || u_marker.Substring(1).StartsWith(i_marker))
                    {
                        ignore_found = true;
                        break;
                    }
                }

                if (ignore_found)
                {
                    //subrpt.Append("<i>(" + u_marker + " not included in tree)</i><br>");
                    continue;
                }


                found = false;
                //check u_marker in haplogroups
                hg_found = "";
                foreach (string markers in hgs)
                {
                    parent_markers=markers.Substring(markers.IndexOf(":")+1);
                    parent_hg = markers.Substring(0, markers.IndexOf(":"));
                    if (parent_markers.IndexOf(u_marker) != -1)
                    {
                        found = true;
                        hg_found = markers;
                        if (report_stmt.ContainsKey(parent_hg))
                            rpt_markers = (string)report_stmt[parent_hg];
                        else
                            rpt_markers = parent_markers;
                        rpt_markers=rpt_markers.Replace(u_marker, "<font color='darkgreen'><b>" + u_marker + "</b></font>");
                        report_stmt.Remove(parent_hg);
                        report_stmt.Add(parent_hg,rpt_markers);
                        //subrpt.Append("<b>" + parent_hg+"</b> (");
                        //subrpt.Append(parent_markers.Replace(u_marker, "<font color='darkgreen'><b>" + u_marker + "</b></font>") + ")<br>");
                        break;
                    }
                }
                if (found)
                {
                    //hgs.Remove(hg_found);
                    if (!hgs_found.Contains(hg_found))
                        hgs_found.Add(hg_found);
                    matches.Add(u_marker);
                }
                else
                    mismatches.Add(u_marker);
            }
            if (hgs.Count == hgs_found.Count)
            {
                report.Append("<h3>Haplogroup " + key.Text + "</h3>");
                //
                TreeNode parent = null;
                ArrayList lines = new ArrayList();
                if (key.Text == "NoLabel")
                    lines.Add(" ⇨ (" + report_stmt[key.Text] + ")");
                else
                    lines.Add(" ⇨ <b>" + key.Text + "</b> (" + report_stmt[key.Text] + ")");
                parent = key.Parent;
                while (true)
                {
                    if (parent.Text == "Eve")
                        break;
                    if (parent.BackColor == Color.DarkGreen)
                    {
                        if (parent.Text == "NoLabel")
                            lines.Add(" ⇨ (" + report_stmt[parent.Text] + ")");
                        else
                            lines.Add(" ⇨ <b>" + parent.Text + "</b> (" + report_stmt[parent.Text] + ")");
                    }
                    parent = parent.Parent;
                }
                lines.Add("♀ Eve");
                int count = 1;
                for (int i = lines.Count - 1; i >= 0; i--)
                {
                    for (int j = 0; j < count*2; j++)
                        report.Append("&nbsp;");
                    report.Append(lines[i]);
                    report.Append("<br>");
                    count++;
                }

                report.Append("Matches on Tree (" + matches.Count.ToString() + ") : ");
                foreach (string m in matches)
                {
                    report.Append("<font color='darkgreen'>" + m + "</font>");
                    report.Append(" ");
                }
                report.Append("<br>");
                report.Append("Mismatches/Extras (" + mismatches.Count.ToString()+") : ");
                foreach (string m in mismatches)
                {
                    report.Append("<font color='red'>"+m+"</font>");
                    report.Append(" ");
                }
                report.Append("<br>");
                //
                ArrayList list = new ArrayList();
                if (sorted_report_array.ContainsKey(mismatches.Count))
                    list = (ArrayList)sorted_report_array[mismatches.Count];
                list.Add(report.ToString());
                sorted_report_array.Remove(mismatches.Count);
                sorted_report_array.Add(mismatches.Count, list);
                //
                list = new ArrayList();
                if (sorted_hg_readjustment.ContainsKey(mismatches.Count))
                    list = (ArrayList)sorted_hg_readjustment[mismatches.Count];
                list.Add(key);
                sorted_hg_readjustment.Remove(mismatches.Count);
                sorted_hg_readjustment.Add(mismatches.Count, list);
                
                return true;
            }
            else
                return false;
        }

        private ArrayList getAllMatchingInParent(TreeNode key)
        {
            TreeNode parent = null;
            ArrayList list = new ArrayList();
            list.Add(key.Text+":"+mutations_map[key]);
            parent = key.Parent;
            while (true)
            {
                if (parent.Text == "Eve")
                    break;
                if (parent.BackColor == Color.DarkGreen)
                {
                    list.Add(parent.Text+":"+mutations_map[parent]);
                }
                parent = parent.Parent;
            }
            return list;
        }

        private int getParentMatches(TreeNode key)
        {
            int match = 0;
            if (key.BackColor == Color.DarkGreen)
                match = 1;
            else
                match = 0;
            if (key.Parent != null)
                return match + getParentMatches(key.Parent);
            else
                return match;
        }

        public void Save()
        {
            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                bool first = false;
                bool second = false;
                foreach (KeyValuePair<int,ArrayList> kvp in sorted_report_array)
                {
                    if (!first)
                    {
                        report.Append("<hr>");
                        report.Append("<h2>Best Identified Haplogroups</h2>");
                        foreach (string rpt in kvp.Value)
                            report.Append(rpt);
                        first = true;
                    }
                    else if (!second)
                    {
                        report.Append("<hr>");
                        report.Append("<h2>Other Possible Haplogroups</h2>");
                        foreach (string rpt in kvp.Value)
                            report.Append(rpt);
                        report.Append("<hr>");
                        break;
                    }
                }

                report.Append("<br>");
                report.Append("<i>Note: The mutations 309.1C(C), 315.1C, AC indels at 515-522, 16182C, 16183C, 16193.1C(C) and 16519 were not considered for phylogenetic reconstruction and are therefore excluded from the tree.</i><br><br>");
                report.Append("<i>Generated on " + DateTime.Now.ToLongDateString() + " at " + DateTime.Now.ToLongTimeString() + " by <a href='http://www.y-str.org/'>Genetic Genealogy Kit (GGK)</a></i></body></html>");
                File.WriteAllText(saveFileDialog1.FileName, report.ToString(),Encoding.UTF8);
                GGKUtilLib.setStatus("mtDNA Report Saved.");
                if (MessageBox.Show("mtDNA Report Saved. Do you want to open it?", "Open Report", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    Process.Start(saveFileDialog1.FileName);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Genetic Genealogy Kit (GGK) will try to use the internet and connect to mtdnacommunity.org to fetch the latest Human mtDNA Phylogeny XML. Are you sure you want to do this?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    string url = GGKSettings.getParameterValue("Phylogeny.mtDNA.URL");
                    GGKUtilLib.setStatus("Fetching.. " + url);


                    backgroundWorker1.RunWorkerAsync(url);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Technical Details: " + ee.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string url = e.Argument.ToString();
            WebClient client = new WebClient();
            xml_phylogeny = client.DownloadString(url);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            treeView1.Nodes.Clear();
            lblSb.Text = "";
            lblyhg.Text = "";
            mutations_map.Clear();
            match_map.Clear();
            GGKUtilLib.setStatus("Done.");
            timer1.Enabled = true;
        }

        private void lblyhg_Click(object sender, EventArgs e)
        {

        }

        private void mtchart_Click(object sender, EventArgs e)
        {

        }

        private void lblSb_Click(object sender, EventArgs e)
        {

        }    
    }
}

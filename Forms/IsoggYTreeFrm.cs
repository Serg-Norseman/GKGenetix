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

namespace Genetic_Genealogy_Kit
{
    public partial class IsoggYTreeFrm : Form
    {
        string kit = null;
        Hashtable snp_map = new Hashtable();
        List<string> snp_on_tree = new List<string>();
        string my_snp = null;
        string[] snp_array = null;

        public IsoggYTreeFrm(string kit)
        {
            InitializeComponent();
            this.kit = kit;
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {            
            timer1.Enabled = true;             
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            label2.Text = GGKUtilLib.getKitName(kit);
            txtSNPs.Text = GGKUtilLib.queryValue("kit_ysnps", new string[] { "ysnps" }, "where kit_no='"+kit+"'");
            GGKUtilLib.setStatus("Plotting on ISOGG Y-Tree ...");
            //
            XDocument doc = XDocument.Parse(Genetic_Genealogy_Kit.Properties.Resources.ytree);

            TreeNode root = new TreeNode("Adam");
            treeView1.Nodes.Add(root);

            foreach (XElement el in doc.Root.Elements())
            {
                buildTree(root,el);    
            }
            root.Expand();
            //
            snp_on_tree.AddRange(Genetic_Genealogy_Kit.Properties.Resources.snps_on_tree.Split(new char[] { ',' }));

            my_snp = txtSNPs.Text;
            snp_array = filterSNPsOnTree(my_snp);

            timer2.Enabled = true;
        }


        private void buildTree(TreeNode parent,XElement elmt)
        {
            TreeNode tn = null;
            XAttribute attrib_name = elmt.Attribute("name");
            XAttribute attrib_markers = elmt.Attribute("markers");
            if (attrib_name != null)
            {
                string attrib_value = attrib_name.Value.Trim();
                string value = attrib_markers.Value.Trim();
                value = value.Replace(",", ", ");
                //
                if (attrib_value.IndexOf(',') != -1)
                    attrib_value = removeDuplicates(attrib_value);
                //
                if (attrib_value != "")
                {
                    tn = new TreeNode(attrib_value);
                    snp_map.Add(tn, value);
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
        }

        private string removeDuplicates(string value)
        {
            string[] tmp = value.Split(",".ToCharArray()).Distinct().ToArray();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < tmp.Length;i++)
            {
                sb.Append(tmp[i]);
                if(i!=tmp.Length-1)
                    sb.Append(", ");
            }
            return sb.ToString();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            timer3.Enabled = true;   
        }

        private void markOnTree()
        {                        
            foreach (TreeNode node in snp_map.Keys)
            {
                node.ForeColor = Color.Gray;
                node.BackColor = Color.White;
            }
            treeView1.CollapseAll();
            TreeNode hg_maxpath = null;

            string hg_snps = "";            

            foreach (TreeNode key in snp_map.Keys)
            {
                hg_snps= (string)snp_map[key];
                if (hg_snps.Trim().Equals("-"))
                    continue;
                foreach (string hg_snp in hg_snps.Replace(" ","").Split(new char[]{',','/'}))
                {
                    foreach (string snp in snp_array)
                    {
                        if (snp.EndsWith("-"))
                        {
                            if (hg_snp.Trim() == snp.Substring(0, snp.Length - 1).Trim())
                            {
                                if (key.ForeColor == Color.White && key.BackColor==Color.DarkGreen)
                                {
                                    key.ForeColor = Color.Orange;
                                    key.BackColor = Color.LightGreen;
                                }
                                else if (key.ForeColor != Color.Orange && key.BackColor != Color.LightGreen)
                                {
                                    key.ForeColor = Color.Yellow;
                                    key.BackColor = Color.Red;
                                }
                                //key.NodeFont = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold);
                                key.Collapse();
                                key.EnsureVisible();
                                //

                            }
                        }
                        else if (snp.EndsWith("+"))
                        {                            
                            if (hg_snp.Trim() == snp.Substring(0, snp.Length - 1).Trim())
                            {
                                //MessageBox.Show(key.FullPath.LastIndexOf('\\').ToString());
                                if (key.ForeColor == Color.Yellow && key.BackColor == Color.Red)
                                {
                                    key.ForeColor = Color.Orange;
                                    key.BackColor = Color.LightGreen;
                                }
                                else if (key.ForeColor != Color.Orange && key.BackColor != Color.LightGreen)
                                {
                                    key.ForeColor = Color.White;
                                    key.BackColor = Color.DarkGreen;
                                }
                                //key.NodeFont = new Font("Microsoft Sans Serif", 7.8f,FontStyle.Bold);
                                key.Expand();
                                key.EnsureVisible();
                                if (hg_maxpath == null)
                                {
                                    hg_maxpath = key;
                                }
                                else
                                {
                                   
                                    //if (key.FullPath.Length > hg_maxpath.FullPath.Length)
                                    if (key.FullPath.LastIndexOf('\\') > hg_maxpath.FullPath.LastIndexOf('\\') && key.Parent.BackColor != Color.Red)
                                        hg_maxpath = key;
                                }
                            }
                        }
                        else
                        {
                           /*
                            if (hg_snp.Trim() == snp.Trim())
                            {
                                if (key.ForeColor == Color.Yellow && key.BackColor == Color.Red)
                                {
                                    key.ForeColor = Color.Orange;
                                    key.BackColor = Color.LightGreen;
                                }
                                else if (key.ForeColor != Color.Orange && key.BackColor != Color.LightGreen)
                                {
                                    key.ForeColor = Color.White;
                                    key.BackColor = Color.DarkGreen;
                                }
                                //key.NodeFont = new Font("Microsoft Sans Serif", 7.8f,FontStyle.Bold);
                                key.Expand();
                                key.EnsureVisible();
                                if (hg_maxpath == null)
                                {
                                    hg_maxpath = key;
                                }
                                else
                                {
                                    //if (key.FullPath.Length > hg_maxpath.FullPath.Length)
                                    if (key.FullPath.LastIndexOf('\\') > hg_maxpath.FullPath.LastIndexOf('\\') && key.Parent.BackColor != Color.Red)
                                        hg_maxpath = key;
                                }
                            }
                            */
                        }
                    }
                }
            }
            foreach (TreeNode node in snp_map.Keys)
            {
                if (node.BackColor != Color.DarkGreen && node.BackColor != Color.Red && node.BackColor!=Color.LightGreen)
                {
                    node.ForeColor = Color.Gray;
                    node.BackColor = Color.White;
                }
            }
            if (hg_maxpath != null)
            {
                hg_maxpath.NodeFont = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Underline);
                hg_maxpath.EnsureVisible();
                treeView1.SelectedNode = hg_maxpath;
                lblyhg.Text = hg_maxpath.Text;
                beautifyTree(hg_maxpath);
            }
        }

        private void beautifyTree(TreeNode node)
        {
            if (node.Parent == null)
                return;
            foreach(TreeNode sibnode in node.Parent.Nodes)
            {
                if (sibnode == node)
                    continue;
                sibnode.Collapse();
            }
            beautifyTree(node.Parent);
        }

        private string[] filterSNPsOnTree(string my_snp)
        {
            string[] entered_snps = my_snp.Replace(" ", "").Split(new char[]{','});
            List<string> valid_snps = new List<string>();
            foreach(string s in entered_snps)
            {
                if(snp_on_tree.Contains(s.Substring(0,s.Length-1)))
                {
                    valid_snps.Add(s);
                }
            }
            return valid_snps.ToArray();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            markOnTree();
            GGKUtilLib.setStatus("Done.");
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            timer3.Enabled = false;
            TreeNode node = treeView1.SelectedNode;
            snpTextBox.Text = " " + (string)snp_map[node] + " ";
            snpTextBox.SelectAll();
            snpTextBox.SelectionColor = Color.Gray;
            label1.Text = "Defining SNPs for "+node.Text;
            // color:

            int start = -1;
            string[] begin = new string[] { " ", "/" };
            string[] end = new string[] { " ", "/", "," };
            string search_term = null;
            foreach (string snp in snp_array)
            {
                if (snp.Equals(""))
                    continue;
                foreach (string b1 in begin)
                    foreach (string e1 in end)
                    {
                        search_term = b1 + snp.Substring(0, snp.Length - 1) + e1;
                        start = snpTextBox.Find(search_term);
                        if (start != -1)
                        {
                            snpTextBox.Select(start + 1, snp.Length - 1);
                            if (snp.EndsWith("-"))
                            {
                                snpTextBox.SelectionBackColor = Color.Red;
                                snpTextBox.SelectionColor = Color.Yellow;
                            }
                            else if (snp.EndsWith("+"))
                            {
                                snpTextBox.SelectionBackColor = Color.DarkGreen;
                                snpTextBox.SelectionColor = Color.White;
                            }
                        }
                    }
            }
        }      
    }
}

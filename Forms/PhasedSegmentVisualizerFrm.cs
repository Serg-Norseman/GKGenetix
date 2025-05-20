using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Genetic_Genealogy_Kit
{
    public partial class PhasedSegmentVisualizerFrm : Form
    {
        string phased_kit=null;
        string unphased_kit = null;
        string chromosome=null;
        string start_position=null;
        string end_position = null;
        Image original = null;
        DataTable dt = null;

        public PhasedSegmentVisualizerFrm(string phased_kit,string unphased_kit,string chr,string start_pos,string end_pos)
        {
            InitializeComponent();
            //
            this.phased_kit = phased_kit;
            this.unphased_kit = unphased_kit;
            this.chromosome = chr;
            this.start_position = start_pos;
            this.end_position = end_pos;
        }

        private void SegmentVisualizerFrm_Load(object sender, EventArgs e)
        {
            this.Text = "Phased Segment Visualizer : Chr "+chromosome+": "+start_position+"-"+end_position;
            statusLbl.Text = "Loading ...";
            bwPhaseVisualizer.RunWorkerAsync();            
        }

        private void bwPhaseVisualizer_DoWork(object sender, DoWorkEventArgs e)
        {
            DataTable dt_existing = GGKUtilLib.QueryDB("select segment_image,segment_xml from cmp_phased where phased_kit='"+phased_kit+"' and match_kit='"+unphased_kit+"' and chromosome='"+chromosome+"' and start_position="+start_position+" and end_position="+end_position);
            if (dt_existing.Rows.Count > 0)
            {
                object[] o=dt_existing.Rows[0].ItemArray;
                string xml = o[1].ToString();
                dt = new DataTable();
                MemoryStream ms = new MemoryStream(Encoding.ASCII.GetBytes(xml));                
                dt.ReadXml(ms);
                
                
                this.Invoke(new MethodInvoker(delegate
                {
                    dgvSegment.DataSource = dt;

                    dgvSegment.Columns[0].HeaderText = "Position";
                    dgvSegment.Columns[1].HeaderText = GGKUtilLib.sqlSafe(GGKUtilLib.getKitName(unphased_kit));
                    dgvSegment.Columns[2].HeaderText = GGKUtilLib.sqlSafe(GGKUtilLib.getKitName(phased_kit)) + " (Paternal)";
                    dgvSegment.Columns[3].HeaderText = GGKUtilLib.sqlSafe(GGKUtilLib.getKitName(phased_kit)) + " (Maternal)";

                    dgvSegment.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvSegment.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvSegment.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvSegment.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    dgvSegment.Columns[0].ReadOnly = true;
                    dgvSegment.Columns[1].ReadOnly = true;
                    dgvSegment.Columns[2].ReadOnly = true;
                    dgvSegment.Columns[3].ReadOnly = true;
                }));

                byte[] image_array = (byte[])o[0];
                Image img = GGKUtilLib.byteArrayToImage(image_array);
                this.Invoke(new MethodInvoker(delegate
                {
                    original = new Bitmap(img,600,150);
                    pbSegment.Image = original;
                }));
            }
            else
            {
                dt = GGKUtilLib.QueryDB("select a.position,a.genotype,p.paternal_genotype,p.maternal_genotype from kit_autosomal a,kit_phased p where a.kit_no='" + unphased_kit + "' and a.position>" + start_position + " and a.position<" + end_position + " and a.chromosome='" + chromosome + "' and p.rsid=a.rsid and p.kit_no='" + phased_kit + "' order by a.position");
                if (dt.Rows.Count > 0)
                {
                    if (this.IsHandleCreated)
                    {
                        this.Invoke(new MethodInvoker(delegate
                        {
                            dgvSegment.DataSource = dt;

                            dgvSegment.Columns[0].HeaderText = "Position";
                            dgvSegment.Columns[1].HeaderText = GGKUtilLib.sqlSafe(GGKUtilLib.getKitName(unphased_kit));
                            dgvSegment.Columns[2].HeaderText = GGKUtilLib.sqlSafe(GGKUtilLib.getKitName(phased_kit)) + " (Paternal)";
                            dgvSegment.Columns[3].HeaderText = GGKUtilLib.sqlSafe(GGKUtilLib.getKitName(phased_kit)) + " (Maternal)";

                            dgvSegment.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            dgvSegment.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            dgvSegment.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            dgvSegment.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                            dgvSegment.Columns[0].ReadOnly = true;
                            dgvSegment.Columns[1].ReadOnly = true;
                            dgvSegment.Columns[2].ReadOnly = true;
                            dgvSegment.Columns[3].ReadOnly = true;
                        }));
                        Image img = GGKUtilLib.getPhasedSegmentImage(dt, chromosome);
                        this.Invoke(new MethodInvoker(delegate
                        {
                            original = img;
                            pbSegment.Image = img;
                        }));
                    }

                    dt.TableName = "cmp_phased";
                    StringBuilder sb=new StringBuilder();
                    StringWriter w = new StringWriter(sb);
                    dt.WriteXml(w,XmlWriteMode.WriteSchema);
                    string segment_xml = sb.ToString();

                    SQLiteConnection conn = GGKUtilLib.getDBConnection();
                    SQLiteCommand cmd = new SQLiteCommand("INSERT INTO cmp_phased(phased_kit,match_kit,chromosome,start_position,end_position,segment_image,segment_xml) VALUES (@phased_kit,@match_kit,@chromosome,@start_position,@end_position,@segment_image,@segment_xml)", conn);
                    cmd.Parameters.AddWithValue("@phased_kit", phased_kit);
                    cmd.Parameters.AddWithValue("@match_kit", unphased_kit);
                    cmd.Parameters.AddWithValue("@chromosome", chromosome);
                    cmd.Parameters.AddWithValue("@start_position", start_position);
                    cmd.Parameters.AddWithValue("@end_position", end_position);
                    byte[] image_bytes = GGKUtilLib.imageToByteArray(original);
                    cmd.Parameters.Add("@segment_image", DbType.Binary, image_bytes.Length).Value = image_bytes;
                    cmd.Parameters.AddWithValue("@segment_xml", segment_xml);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        private void bwPhaseVisualizer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            statusLbl.Text = "Done.";
            statusStrip1.Visible = false;
            if(dt.Rows.Count==0)
            {
                this.Visible = false;
                MessageBox.Show("The kits are not phased. If any of the kit used for comparing is phased, then Phased Segment Visualizer will show you how the segment matches.","Phased Segment Visualizer",MessageBoxButtons.OK,MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void dgvSegment_SelectionChanged(object sender, EventArgs e)
        {
            if (original != null && dgvSegment.SelectedRows.Count>0)
            {
                int idx = (dgvSegment.SelectedRows[0].Index * 600) / dgvSegment.Rows.Count;
                Image img = (Image)original.Clone();
                Graphics g = Graphics.FromImage(img);
                Pen p1 = new Pen(Color.Black, 1);
                g.DrawLine(p1, idx, 0, idx, 150);
                pbSegment.Image = img;
            }
        }

        private void pbSegment_MouseClick(object sender, MouseEventArgs e)
        {
            if (dt != null && pbSegment.Image != null)
            {
                int idx = e.X * dt.Rows.Count / pbSegment.Image.Width;
                if (idx < dgvSegment.Rows.Count)
                {
                    dgvSegment.ClearSelection();
                    dgvSegment.Rows[idx].Selected = true;
                    dgvSegment.FirstDisplayedScrollingRowIndex = dgvSegment.Rows[idx].Index;
                }
            }
        }

        private void pbSegment_MouseMove(object sender, MouseEventArgs e)
        {
            if (dt != null && pbSegment.Image != null && e.Button==MouseButtons.Left)
            {
                int idx = e.X * dt.Rows.Count / pbSegment.Image.Width;
                if (idx < dgvSegment.Rows.Count)
                {
                    dgvSegment.ClearSelection();
                    dgvSegment.Rows[idx].Selected = true;
                    dgvSegment.FirstDisplayedScrollingRowIndex = dgvSegment.Rows[idx].Index;
                }
            }
        }
    }
}

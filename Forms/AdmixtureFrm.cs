using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Genetic_Genealogy_Kit
{
    public partial class AdmixtureFrm : Form
    {
        string kit = null;
        List<string> plotted = new List<string>();

        public AdmixtureFrm(string kit)
        {
            InitializeComponent();
            this.kit = kit;
        }

        private void AdmixtureFrm_Load(object sender, EventArgs e)
        {
            kitLbl.Text = kit + " (" + GGKUtilLib.getKitName(kit)+")";
            DataTable dt = GGKUtilLib.QueryDB("select name,at_total,at_longest,x,y FROM (select b.name,a.at_total,a.at_longest,b.x,b.y from cmp_status a,kit_master b where a.kit1='" + kit + "' and a.kit2=b.kit_no and a.kit2 like 'HGDP%' and a.status_autosomal=1 and a.at_longest<3 and a.at_total!=0 UNION select b.name,a.at_total,a.at_longest,b.x,b.y from cmp_status a,kit_master b where a.kit2='" + kit + "' and a.kit1=b.kit_no and a.kit1 like 'HGDP%' and a.status_autosomal=1 and a.at_longest<3 and a.at_total!=0) ORDER BY at_total DESC");
            double total = 0.0;
            foreach(DataRow row in dt.Rows)
            {
                total += double.Parse(row.ItemArray[1].ToString());
            }
            
            DataTable adx_table = new DataTable();
            adx_table.Columns.Add("Population");
            adx_table.Columns.Add("Location");
            adx_table.Columns.Add("Total Shared (cM)"); 
            adx_table.Columns.Add("Longest (cM)");
            adx_table.Columns.Add("Percentage");
            adx_table.Columns.Add("X");
            adx_table.Columns.Add("Y");            

            string population = null;
            string location =null;
            double at_total = 0.0;
            string at_longest = null;
            double percentage = 0.0;

            string[] data=null;
            for (int i = 0; i < dt.Rows.Count;i++ )
            {
                data = dt.Rows[i].ItemArray[0].ToString().Replace("_", " ").Split(new char[]{','});
                population=data[0];
                location=data[1];
                at_total = double.Parse(dt.Rows[i].ItemArray[1].ToString());
                at_longest = dt.Rows[i].ItemArray[2].ToString();
                percentage = (at_total * 100 / total);

                adx_table.Rows.Add(new object[] { population, location,at_total,at_longest, percentage.ToString("#0.00"), dt.Rows[i].ItemArray[3], dt.Rows[i].ItemArray[4] });

                chart1.Series[0].Points.AddXY(population + ", " + location + " (" + percentage.ToString("#0.00") + "%)", new object[] { percentage });
            }

            foreach (DataPoint p in chart1.Series[0].Points)
            {                
                p.IsVisibleInLegend = false;
            }
            dgv_Admixture.Columns.Clear();
            dgv_Admixture.DataSource = adx_table;

            dgv_Admixture.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv_Admixture.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv_Admixture.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv_Admixture.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv_Admixture.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv_Admixture.Columns[5].Visible = false;
            dgv_Admixture.Columns[6].Visible = false;

            int percent = 0;
            int x = 0;
            int y = 0;
            Image img = pbWorldMap.Image;
            Graphics g = Graphics.FromImage(img);
            
            plotted.Clear();
            foreach (DataRow row in adx_table.Rows)
            {
                percent = (int)double.Parse(row.ItemArray[4].ToString());
                x = int.Parse(row.ItemArray[5].ToString());
                y = int.Parse(row.ItemArray[6].ToString());
                if (!plotted.Contains(x + ":" + y))
                {
                    if(percent>50) // plotting 100% is too big and ugly.                    
                        percent = 50;                    
                    setHeatMap(g, percent, x, y);
                    plotted.Add(x + ":" + y);
                }                
            }
            g.Save();

            pbWorldMap.Image = img;
        }

        public void setHeatMap(Graphics g, int percent, int x, int y)
        {
            int radius_gap = 2;
            for (int i = 0; i < percent; i++)
            {
                Pen pen1 = new Pen(HeatMapColor(i, percent), 2);
                g.DrawEllipse(pen1, x - 1 - i * radius_gap, y - 1 - i * radius_gap, 2 + i * 2 * radius_gap, 2 + i * 2 * radius_gap);
            }
        }

        public Color HeatMapColor(double percent,double max)
        {
            double val = percent * 255 / max;
            
            int r = 255;
            int g = Convert.ToByte(val);
            int b = Convert.ToByte(val);
            return Color.FromArgb(255,r, g, b);
        }

        private void pbWorldMap_MouseMove(object sender, MouseEventArgs e)
        {
            //GGKUtilLib.setStatus(e.X.ToString() + "," + e.Y.ToString());
        }
    }
}

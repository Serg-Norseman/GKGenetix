using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Genetic_Genealogy_Kit
{
    public partial class LocationSelectFrm : Form
    {
        Image origial = null;

        public int X = 0;
        public int Y = 0;

        public LocationSelectFrm(int X,int Y)
        {
            InitializeComponent();
            this.X = X;
            this.Y = Y;
        }

        private void pbWorldMap_MouseMove(object sender, MouseEventArgs e)
        {
            Image img = (Image)origial.Clone();
            Graphics g = Graphics.FromImage(img);
            int size = 10;
            Pen pen1 = new Pen(Color.Red, 3);
            g.DrawRectangle(pen1, new Rectangle(e.X*2 - size, e.Y*2 - size, size*2, size*2));
            Pen pen2 = new Pen(Color.Red, 1);
            g.DrawLine(pen2, 0, e.Y * 2, 1357, e.Y * 2);
            g.DrawLine(pen2, e.X * 2, 0, e.X * 2, 628);
            g.Save();
            pbWorldMap.Image = img;
        }

        private void LocationSelectFrm_Load(object sender, EventArgs e)
        {
            if (X != 0 && Y != 0)
            {
                Image img = pbWorldMap.Image;
                int green = 0;
                int blue = 0;
                Graphics g = Graphics.FromImage(img);
                
                for (int i = 0; i < 30; i++)
                {
                    green = i * 255 / 30;
                    blue = green;
                    Pen pen1 = new Pen(Color.FromArgb(255,green,blue), 3);
                    g.DrawEllipse(pen1, new Rectangle(X - i, Y - i, i * 2, i * 2));
                }
                g.Save();
                pbWorldMap.Image = img;               
            }
             origial = (Image)pbWorldMap.Image.Clone();
        }

        private void pbWorldMap_MouseClick(object sender, MouseEventArgs e)
        {
            if (MessageBox.Show("Is the selected region displayed in the World Map is where the kit/kit's ancestors are from?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                X = e.X * 2;
                Y = e.Y * 2;
                this.Hide();
            }
        }
    }
}

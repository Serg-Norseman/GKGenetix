/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace GenetixKit.Forms
{
    public partial class LocationSelectFrm : Form
    {
        public int X = 0;
        public int Y = 0;

        private bool preInit = false;
        private Pen pen1;
        private Pen pen2;
        private int mX, mY;

        public LocationSelectFrm(int x, int y)
        {
            InitializeComponent();

            pbWorldMap.Image = Properties.Resources.world_map;
            pen1 = new Pen(Color.Red, 3);
            pen2 = new Pen(Color.Red, 1);

            X = x;
            Y = y;
            if (X != 0 && Y != 0) {
                mX = X / 2;
                mY = Y / 2;
                preInit = true;
            }
        }

        private void LocationSelectFrm_Load(object sender, EventArgs e)
        {
        }

        private void pbWorldMap_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            if (preInit) {
                for (int i = 0; i < 30; i++) {
                    int green = i * 255 / 30;
                    int blue = green;
                    using (var pen1x = new Pen(Color.FromArgb(255, green, blue), 3))
                        g.DrawEllipse(pen1x, new Rectangle(mX - i, mY - i, i * 2, i * 2));
                }
            } else {
                int size = 6;
                g.DrawRectangle(pen1, new Rectangle(mX - size, mY - size, size * 2 - 1, size * 2 - 1));
                g.DrawLine(pen2, 0, mY, 1357, mY);
                g.DrawLine(pen2, mX, 0, mX, 628);
            }
        }

        private void pbWorldMap_MouseMove(object sender, MouseEventArgs e)
        {
            preInit = false;
            mX = e.X;
            mY = e.Y;
            pbWorldMap.Invalidate(false);
        }

        private void pbWorldMap_MouseClick(object sender, MouseEventArgs e)
        {
            if (MessageBox.Show("Is the selected region displayed in the World Map is where the kit/kit's ancestors are from?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                X = e.X * 2;
                Y = e.Y * 2;
                this.Hide();
            }
        }
    }
}

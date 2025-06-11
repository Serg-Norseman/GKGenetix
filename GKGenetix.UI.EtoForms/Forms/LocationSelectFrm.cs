/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using Eto.Drawing;
using Eto.Forms;
using Eto.Serialization.Xaml;

namespace GKGenetix.UI.Forms
{
    public partial class LocationSelectFrm : Dialog
    {
        #region Design components
#pragma warning disable CS0169, CS0649, IDE0044, IDE0051

        private ImageView pbWorldMap;

#pragma warning restore CS0169, CS0649, IDE0044, IDE0051
        #endregion


        public double Longitude { get; set; }
        public double Latitude { get; set; }

        private bool preInit = false;
        private Pen pen1;
        private Pen pen2;
        private float mX, mY;


        public LocationSelectFrm(double lng, double lat)
        {
            XamlReader.Load(this);

            //pbWorldMap.Image = GKGenetix.UI.Properties.Resources.world_map;
            pen1 = new Pen(Colors.Red, 3);
            pen2 = new Pen(Colors.Red, 1);

            Longitude = lng;
            Latitude = lat;
            if (Longitude != 0 && Latitude != 0) {
                mX = (float)(Longitude / 2);
                mY = (float)(Latitude / 2);
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
                        g.DrawEllipse(pen1x, new RectangleF(mX - i, mY - i, i * 2, i * 2));
                }
            } else {
                int size = 6;
                g.DrawRectangle(pen1, new RectangleF(mX - size, mY - size, size * 2 - 1, size * 2 - 1));
                g.DrawLine(pen2, 0, mY, 1357, mY);
                g.DrawLine(pen2, mX, 0, mX, 628);
            }
        }

        private void pbWorldMap_MouseMove(object sender, MouseEventArgs e)
        {
            preInit = false;
            mX = e.Location.X;
            mY = e.Location.Y;
            pbWorldMap.Invalidate(false);
        }

        private void pbWorldMap_MouseClick(object sender, MouseEventArgs e)
        {
            if (MessageBox.Show("Is the selected region displayed in the World Map is where the kit/kit's ancestors are from?", "Confirm", MessageBoxButtons.YesNo, MessageBoxType.Question) == DialogResult.Yes) {
                Longitude = e.Location.X * 2;
                Latitude = e.Location.Y * 2;
                this.Close();
            }
        }
    }
}

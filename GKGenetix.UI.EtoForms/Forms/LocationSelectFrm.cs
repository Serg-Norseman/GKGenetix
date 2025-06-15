/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using Eto.Drawing;
using Eto.Forms;
using Eto.Serialization.Xaml;

namespace GKGenetix.UI.Forms
{
    public partial class LocationSelectFrm : Dialog
    {
        #region Design components
#pragma warning disable CS0169, CS0649, IDE0044, IDE0051

        private GKMapBrowser pbWorldMap;

#pragma warning restore CS0169, CS0649, IDE0044, IDE0051
        #endregion


        public double Longitude { get; set; }
        public double Latitude { get; set; }

        private Pen pen1;
        private Pen pen2;
        private float mX, mY;


        public LocationSelectFrm(double lng, double lat)
        {
            XamlReader.Load(this);

            pbWorldMap.MapControl.MouseDown += pbWorldMap_MouseClick;
            pbWorldMap.MapControl.MouseMove += pbWorldMap_MouseMove;
            pbWorldMap.MapControl.Paint += pbWorldMap_Paint;

            pen1 = new Pen(Colors.Red, 3);
            pen2 = new Pen(Colors.Red, 1);

            Longitude = lng;
            Latitude = lat;
            if (Longitude != 0 && Latitude != 0) {
                pbWorldMap.AddMarker(new GKMap.PointLatLng(Latitude, Longitude), GKMap.MapObjects.GMarkerIconType.blue_small, "");
            }
        }

        private void pbWorldMap_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            int size = 6;
            g.DrawRectangle(pen1, new RectangleF(mX - size, mY - size, size * 2 - 1, size * 2 - 1));
            g.DrawLine(pen2, 0, mY, pbWorldMap.Width - 1, mY);
            g.DrawLine(pen2, mX, 0, mX, pbWorldMap.Height - 1);
        }

        private void pbWorldMap_MouseMove(object sender, MouseEventArgs e)
        {
            mX = e.Location.X;
            mY = e.Location.Y;
            pbWorldMap.MapControl.Invalidate(false);
        }

        private void pbWorldMap_MouseClick(object sender, MouseEventArgs e)
        {
            if (MessageBox.Show("Is the selected region displayed in the World Map is where the kit/kit's ancestors are from?", "Confirm", MessageBoxButtons.YesNo, MessageBoxType.Question) == DialogResult.Yes) {
                Longitude = pbWorldMap.TargetPosition.Lng;
                Latitude = pbWorldMap.TargetPosition.Lat;
                this.Close();
            }
        }
    }
}

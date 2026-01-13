/*
 *  GKGenetix, the simple DNA analysis kit.
 *  Copyright (C) 2022-2026 by Sergey V. Zhdanovskih.
 *
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GKMap;
using GKMap.MapObjects;
using GKMap.MapProviders;
using GKMap.WinForms;

namespace GKGenetix.UI
{
    /// <summary>
    ///
    /// </summary>
    public sealed class GKMapBrowser : UserControl
    {
        private readonly GMapOverlay fObjects = new GMapOverlay("objects");
        private GMapControl fMapControl;
        private PointLatLng fTargetPosition;

        public GMapControl MapControl
        {
            get { return fMapControl; }
        }

        public PointLatLng TargetPosition
        {
            get { return fTargetPosition; }
        }

        public bool SelectMode { get; set; }


        public GKMapBrowser()
        {
            Margin = new Padding(4);

            fMapControl = new GMapControl();
            fMapControl.Dock = DockStyle.Fill;
            fMapControl.Location = new Point(0, 0);
            fMapControl.MinZoom = 0;
            fMapControl.MaxZoom = 24;
            fMapControl.Zoom = 1;
            fMapControl.MapProvider = GMapProviders.GoogleMap;
            fMapControl.Overlays.Add(fObjects);
            fMapControl.MouseMove += MainMap_MouseMove;
            Controls.Add(fMapControl);
        }

        public void Clear()
        {
            fObjects.Clear();
        }

        public void ZoomToBounds()
        {
            fMapControl.ZoomAndCenterMarkers(null);
        }

        public GMapRoute AddRoute(string name, List<PointLatLng> points)
        {
            var route = new GMapRoute(name, points);
            route.IsHitTestVisible = true;
            fObjects.Routes.Add(route);
            return route;
        }

        public void AddMarker(PointLatLng position, GMarkerIconType iconType, string toolTip = "")
        {
            var m = new GMarkerIcon(position, iconType);
            m.ToolTipMode = MarkerTooltipMode.OnMouseOver;
            m.ToolTipText = toolTip;
            fObjects.Markers.Add(m);
        }

        private void MainMap_MouseMove(object sender, MouseEventArgs e)
        {
            Point mpt = e.Location;
            fTargetPosition = fMapControl.FromLocalToLatLng(mpt.X, mpt.Y);
        }
    }
}

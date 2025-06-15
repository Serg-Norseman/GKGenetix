/*
 *  "GKGenetix", the simple DNA analysis kit.
 *  Copyright (C) 2022-2025 by Sergey V. Zhdanovskih.
 *
 *  This file is part of "GKGenetix".
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
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

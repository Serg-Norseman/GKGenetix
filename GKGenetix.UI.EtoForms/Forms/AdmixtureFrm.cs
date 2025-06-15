/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Serialization.Xaml;
using GKGenetix.Core;
using GKGenetix.Core.Database;
using OxyPlot;
using OxyPlot.Eto;
using OxyPlot.Series;

namespace GKGenetix.UI.Forms
{
    public partial class AdmixtureFrm : GKWidget
    {
        #region Design components
#pragma warning disable CS0169, CS0649, IDE0044, IDE0051

        private TabPage tabOrigins;
        private TabPage tabAdmixTable;
        private GridView dgvAdmixture;
        private TabPage tabChart;
        private PlotView chart1;
        private GKMapBrowser pbWorldMap;

#pragma warning restore CS0169, CS0649, IDE0044, IDE0051
        #endregion


        private string kit;
        private PlotModel plotModel;
        private PieSeries pieSeries;


        public static bool CanBeUsed(IList<TestRecord> selectedKits)
        {
            return (selectedKits != null && selectedKits.Count == 1 && !selectedKits[0].Disabled);
        }


        public AdmixtureFrm(IKitHost host, IList<TestRecord> selectedKits) : this(host, selectedKits[0].KitNo)
        {
        }

        public AdmixtureFrm(IKitHost host, string kit) : base(host)
        {
            XamlReader.Load(this);

            UIHelper.FixGridView(dgvAdmixture);
            dgvAdmixture.AddColumn("Population", "Population");
            dgvAdmixture.AddColumn("Location", "Geographic Location");
            dgvAdmixture.AddColumn("AtTotal", "Total Shared (cM)", "#0.00");
            dgvAdmixture.AddColumn("AtLongest", "Longest (cM)", "#0.00");
            dgvAdmixture.AddColumn("Percentage", "Percentage", "#0.00");

            plotModel = new PlotModel();
            pieSeries = new PieSeries();
            plotModel.Series.Add(pieSeries);
            chart1.Model = plotModel;

            this.kit = kit;
        }

        public override void SetKit(IList<TestRecord> selectedKits)
        {
            if (CanBeUsed(selectedKits)) {
                this.kit = selectedKits[0].KitNo;
                ReloadData();
            }
        }

        private void ReloadData()
        {
            this.Text = $"Admixture : {kit} ({GKSqlFuncs.GetKitName(kit)})";

            pbWorldMap.Clear();
            var testRec = GKSqlFuncs.GetKit(kit);
            var hasPt = (testRec.Lng != 0 || testRec.Lat != 0);
            if (hasPt) {
                pbWorldMap.AddMarker(new GKMap.PointLatLng(testRec.Lat, testRec.Lng), GKMap.MapObjects.GMarkerIconType.red_small, "");
            }

            var dt = GKSqlFuncs.GetAdmixture(kit, "> 3");
            foreach (var row in dt) row.PrepareValues();
            AdmixtureRecord.RecalcPercents(dt);
            dgvAdmixture.DataStore = dt;

            pieSeries.Slices.Clear();
            foreach (var row in dt) {
                var slice = new PieSlice($"{row.Population}, {row.Location} ({row.Percentage:#0.00} %)", row.Percentage);
                pieSeries.Slices.Add(slice);
            }
            chart1.Invalidate();

            foreach (var row in dt) {
                if (row.Lng == 0 && row.Lat == 0) continue;

                var perc = $"{row.Percentage:#0.00} %";

                pbWorldMap.AddMarker(new GKMap.PointLatLng(row.Lat, row.Lng), GKMap.MapObjects.GMarkerIconType.green_small, $"{row.Name} {perc}");

                if (hasPt) {
                    pbWorldMap.AddRoute(perc, new List<GKMap.PointLatLng>() { new GKMap.PointLatLng(testRec.Lat, testRec.Lng), new GKMap.PointLatLng(row.Lat, row.Lng) });
                }
            }

            pbWorldMap.Invalidate();
        }

        private void AdmixtureFrm_Load(object sender, EventArgs e)
        {
            ReloadData();
        }
    }
}

/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;
using GGKit.Core;
using GKGenetix.Core.Model;
using GKGenetix.UI;

namespace GGKit.Forms
{
    public partial class AdmixtureFrm : GKWidget
    {
        private string kit;


        public static bool CanBeUsed(IList<KitDTO> selectedKits)
        {
            return (selectedKits != null && selectedKits.Count == 1 && !selectedKits[0].Disabled);
        }


        public AdmixtureFrm(IKitHost host, IList<KitDTO> selectedKits) : this(host, selectedKits[0].KitNo)
        {
        }

        public AdmixtureFrm(IKitHost host, string kit) : base(host)
        {
            InitializeComponent();

            UIHelper.FixGridView(dgvAdmixture);
            dgvAdmixture.AddColumn("Population", "Population");
            dgvAdmixture.AddColumn("Location", "Geographic Location");
            dgvAdmixture.AddColumn("AtTotal", "Total Shared (cM)", "#0.00");
            dgvAdmixture.AddColumn("AtLongest", "Longest (cM)", "#0.00");
            dgvAdmixture.AddColumn("Percentage", "Percentage", "#0.00");

            this.kit = kit;
        }

        public override void SetKit(IList<KitDTO> selectedKits)
        {
            if (CanBeUsed(selectedKits)) {
                this.kit = selectedKits[0].KitNo;
                ReloadData();
            }
        }

        private void ReloadData()
        {
            this.Text = $"Admixture : {kit} ({GKSqlFuncs.GetKitName(kit)})";

            var dt = GKSqlFuncs.GetAdmixture(kit, "> 3");
            foreach (var row in dt) row.PrepareValues();
            AdmixtureRec.RecalcPercents(dt);
            dgvAdmixture.DataSource = dt;

            foreach (var row in dt) {
                chart1.Series[0].Points.AddXY($"{row.Population}, {row.Location} ({row.Percentage:#0.00} %)", row.Percentage);
            }

            foreach (DataPoint p in chart1.Series[0].Points) {
                p.IsVisibleInLegend = false;
            }

            var plotted = new List<string>();
            Image img = (Image)GKGenetix.UI.Properties.Resources.world_map.Clone();
            using (Graphics g = Graphics.FromImage(img)) {
                foreach (var row in dt) {
                    if (row.Longitude == 0 && row.Latitude == 0) continue;

                    string item = row.Longitude + ":" + row.Latitude;
                    if (!plotted.Contains(item)) {
                        SetHeatMap(g, (int)row.Percentage, row.Longitude, row.Latitude);
                        plotted.Add(item);
                    }
                }
            }
            pbWorldMap.Image = img;
        }

        private void AdmixtureFrm_Load(object sender, EventArgs e)
        {
            ReloadData();
        }

        private static void SetHeatMap(Graphics g, int percent, int x, int y)
        {
            percent = Math.Min(50, percent);

            int radius_gap = 2;
            for (int i = 0; i < percent; i++) {
                Pen pen1 = new Pen(UIHelper.HeatMapColor(i, percent), 2);
                g.DrawEllipse(pen1, x - 1 - i * radius_gap, y - 1 - i * radius_gap, 2 + i * 2 * radius_gap, 2 + i * 2 * radius_gap);
            }
        }
    }
}

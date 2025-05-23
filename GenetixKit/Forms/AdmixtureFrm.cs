﻿/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using GenetixKit.Core;
using GenetixKit.Core.Model;

namespace GenetixKit.Forms
{
    public partial class AdmixtureFrm : Form
    {
        private readonly string kit = null;
        private List<string> plotted = new List<string>();

        public AdmixtureFrm(string kit)
        {
            InitializeComponent();

            GKUIFuncs.FixGridView(dgvAdmixture);
            dgvAdmixture.AddColumn("Population", "Population");
            dgvAdmixture.AddColumn("Location", "Geographic Location");
            dgvAdmixture.AddColumn("AtTotal", "Total Shared (cM)", "#0.00");
            dgvAdmixture.AddColumn("AtLongest", "Longest (cM)", "#0.00");
            dgvAdmixture.AddColumn("Percentage", "Percentage", "#0.00");

            this.kit = kit;
        }

        private void AdmixtureFrm_Load(object sender, EventArgs e)
        {
            kitLbl.Text = $"{kit} ({GKSqlFuncs.GetKitName(kit)})";
            var dt = GKSqlFuncs.GetAdmixture(kit, "50");
            AdmixtureRec.RecalcPercents(dt);
            dgvAdmixture.DataSource = dt;

            foreach (var row in dt) {
                chart1.Series[0].Points.AddXY($"{row.Population}, {row.Location} ({row.Percentage:#0.00} %)", new object[] { row.Percentage });
            }

            foreach (DataPoint p in chart1.Series[0].Points) {
                p.IsVisibleInLegend = false;
            }

            Image img = (Image)Properties.Resources.world_map.Clone();
            using (Graphics g = Graphics.FromImage(img)) {
                plotted.Clear();
                foreach (var row in dt) {
                    string item = row.X + ":" + row.Y;
                    if (!plotted.Contains(item)) {
                        SetHeatMap(g, (int)row.Percentage, row.X, row.Y);
                        plotted.Add(item);
                    }
                }
            }
            pbWorldMap.Image = img;
        }

        private static void SetHeatMap(Graphics g, int percent, int x, int y)
        {
            percent = Math.Min(50, percent);

            int radius_gap = 2;
            for (int i = 0; i < percent; i++) {
                Pen pen1 = new Pen(GKUIFuncs.HeatMapColor(i, percent), 2);
                g.DrawEllipse(pen1, x - 1 - i * radius_gap, y - 1 - i * radius_gap, 2 + i * 2 * radius_gap, 2 + i * 2 * radius_gap);
            }
        }
    }
}

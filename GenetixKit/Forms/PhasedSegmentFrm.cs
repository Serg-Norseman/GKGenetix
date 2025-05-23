﻿/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using GenetixKit.Core;
using GenetixKit.Core.Model;

namespace GenetixKit.Forms
{
    public partial class PhasedSegmentFrm : Form
    {
        private readonly string phasedKit = null;
        private readonly string unphasedKit = null;
        private readonly string chromosome = null;
        private readonly string startPosition = null;
        private readonly string endPosition = null;
        private Image original = null;
        private IList<PhaseSegment> tblSegments = null;

        public PhasedSegmentFrm(string phased_kit, string unphased_kit, string chr, string startPos, string endPos)
        {
            InitializeComponent();

            GKUIFuncs.FixGridView(dgvSegment);

            dgvSegment.AddColumn("Position", "Position");
            dgvSegment.AddColumn("Genotype", GKSqlFuncs.GetKitName(unphased_kit));
            dgvSegment.AddColumn("PaternalGenotype", GKSqlFuncs.GetKitName(phased_kit) + " (Paternal)");
            dgvSegment.AddColumn("MaternalGenotype", GKSqlFuncs.GetKitName(phased_kit) + " (Maternal)");

            this.phasedKit = phased_kit;
            this.unphasedKit = unphased_kit;
            this.chromosome = chr;
            this.startPosition = startPos;
            this.endPosition = endPos;
        }

        private void PhasedSegmentFrm_Load(object sender, EventArgs e)
        {
            this.Text = $"Phased Segment : Chr {chromosome}: {startPosition}-{endPosition}";
            statusLbl.Text = "Loading ...";

            Task.Factory.StartNew(() => {
                tblSegments = GKSqlFuncs.GetPhaseSegments(unphasedKit, startPosition, endPosition, chromosome, phasedKit);
                if (tblSegments.Count > 0 && this.IsHandleCreated) {
                    original = GKGenFuncs.GetPhasedSegmentImage(tblSegments, chromosome);
                }

                this.Invoke(new MethodInvoker(delegate {
                    UpdateView();
                }));
            });
        }

        private void UpdateView()
        {
            dgvSegment.DataSource = tblSegments;
            pbSegment.Image = original;

            statusLbl.Text = "Done.";
            statusStrip1.Visible = false;
            if (tblSegments == null || tblSegments.Count == 0) {
                this.Visible = false;
                MessageBox.Show("The kits are not phased. If any of the kit used for comparing is phased, then Phased Segment Visualizer will show you how the segment matches.", "Phased Segment Visualizer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void dgvSegment_SelectionChanged(object sender, EventArgs e)
        {
            if (original != null && dgvSegment.SelectedRows.Count > 0) {
                int idx = (dgvSegment.SelectedRows[0].Index * 600) / dgvSegment.Rows.Count;
                Image img = (Image)original.Clone();
                Graphics g = Graphics.FromImage(img);
                Pen p1 = new Pen(Color.Black, 1);
                g.DrawLine(p1, idx, 0, idx, 150);
                pbSegment.Image = img;
            }
        }

        private void DetectSegmentPos(int mX)
        {
            if (tblSegments != null && pbSegment.Image != null) {
                int idx = mX * tblSegments.Count / pbSegment.Image.Width;
                if (idx < dgvSegment.Rows.Count) {
                    dgvSegment.ClearSelection();
                    dgvSegment.Rows[idx].Selected = true;
                    dgvSegment.FirstDisplayedScrollingRowIndex = dgvSegment.Rows[idx].Index;
                }
            }
        }

        private void pbSegment_MouseClick(object sender, MouseEventArgs e)
        {
            DetectSegmentPos(e.X);
        }

        private void pbSegment_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                DetectSegmentPos(e.X);
        }
    }
}

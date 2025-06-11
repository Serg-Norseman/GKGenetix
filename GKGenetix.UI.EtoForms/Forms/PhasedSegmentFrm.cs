/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eto.Drawing;
using Eto.Forms;
using Eto.Serialization.Xaml;
using GKGenetix.Core;
using GKGenetix.Core.Database;
using GKGenetix.Core.Model;

namespace GKGenetix.UI.Forms
{
    public partial class PhasedSegmentFrm : Dialog
    {
        #region Design components
#pragma warning disable CS0169, CS0649, IDE0044, IDE0051

        private ImageView pbSegment;
        private GridView dgvSegment;
        private Label statusLbl;

#pragma warning restore CS0169, CS0649, IDE0044, IDE0051
        #endregion


        private readonly string phasedKit = null;
        private readonly string unphasedKit = null;
        private readonly byte chromosome = 0;
        private readonly int startPosition;
        private readonly int endPosition;
        private Bitmap original = null;
        private IList<PhaseSegment> tblSegments = null;


        public PhasedSegmentFrm(string phasedKit, string unphasedKit, byte chr, int startPos, int endPos)
        {
            XamlReader.Load(this);

            UIHelper.FixGridView(dgvSegment);

            dgvSegment.AddColumn("Position", "Position");
            dgvSegment.AddColumn("Genotype", GKSqlFuncs.GetKitName(unphasedKit));
            dgvSegment.AddColumn("PaternalGenotype", GKSqlFuncs.GetKitName(phasedKit) + " (Paternal)");
            dgvSegment.AddColumn("MaternalGenotype", GKSqlFuncs.GetKitName(phasedKit) + " (Maternal)");

            this.phasedKit = phasedKit;
            this.unphasedKit = unphasedKit;
            this.chromosome = chr;
            this.startPosition = startPos;
            this.endPosition = endPos;
        }

        private void PhasedSegmentFrm_Load(object sender, EventArgs e)
        {
            this.Title = $"Phased Segment : Chr {chromosome}: {startPosition}-{endPosition}";
            statusLbl.Text = "Loading ...";

            Task.Factory.StartNew(() => {
                tblSegments = GKSqlFuncs.GetPhaseSegments(phasedKit, unphasedKit, chromosome, startPosition, endPosition);
                if (tblSegments.Count > 0) {
                    var xImg = GKGenFuncs.GetPhasedSegmentImage<EFImage>(tblSegments, chromosome);
                    original = xImg.Value;
                }

                Application.Instance.Invoke(new Action(delegate {
                    UpdateView();
                }));
            });
        }

        private void UpdateView()
        {
            dgvSegment.DataStore = tblSegments;
            pbSegment.Image = original;

            statusLbl.Text = "Done.";
            statusLbl.Visible = false;
            if (tblSegments == null || tblSegments.Count == 0) {
                this.Visible = false;
                MessageBox.Show("The kits are not phased. If any of the kit used for comparing is phased, then Phased Segment Visualizer will show you how the segment matches.", "Phased Segment Visualizer", MessageBoxButtons.OK, MessageBoxType.Information);
                this.Close();
            }
        }

        private void dgvSegment_SelectionChanged(object sender, EventArgs e)
        {
            /*if (original != null && dgvSegment.SelectedRows.Count > 0) {
                int idx = (int)((dgvSegment.SelectedRows[0].Index * 600) / dgvSegment.Rows.Count);
                Bitmap img = original.Clone();
                using (var g = new Graphics(img)) {
                    Pen p1 = new Pen(Colors.Black, 1);
                    g.DrawLine(p1, idx, 0, idx, 150);
                }
                pbSegment.Image = img;
            }*/
        }

        private void DetectSegmentPos(float mX)
        {
            /*if (tblSegments != null && pbSegment.Image != null) {
                int idx = (int)(mX * tblSegments.Count / pbSegment.Image.Width);
                if (idx < dgvSegment.Rows.Count) {
                    dgvSegment.ClearSelection();
                    dgvSegment.Rows[idx].Selected = true;
                    dgvSegment.FirstDisplayedScrollingRowIndex = dgvSegment.Rows[idx].Index;
                }
            }*/
        }

        private void pbSegment_MouseClick(object sender, MouseEventArgs e)
        {
            var mPt = e.Location;
            DetectSegmentPos(mPt.X);
        }

        private void pbSegment_MouseMove(object sender, MouseEventArgs e)
        {
            var mPt = e.Location;
            if (e.Buttons == MouseButtons.Primary)
                DetectSegmentPos(mPt.X);
        }
    }
}

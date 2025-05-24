/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using GenetixKit.Core;
using GenetixKit.Core.Model;

namespace GenetixKit.Forms
{
    public partial class PhasingFrm : Form
    {
        private string fatherKit = "Unknown";
        private string motherKit = "Unknown";
        private string childKit = "Unknown";
        private IList<PhaseRow> dt = null;
        private bool male = true;

        public PhasingFrm()
        {
            InitializeComponent();

            GKUIFuncs.FixGridView(dgvPhasing);

            dgvPhasing.AddColumn("RSID", "RSID");
            dgvPhasing.AddColumn("Chromosome", "Chromosome");
            dgvPhasing.AddColumn("Position", "Position");
            dgvPhasing.AddColumn("ChildGenotype", "Child");
            dgvPhasing.AddColumn("PaternalGenotype", "Father");
            dgvPhasing.AddColumn("MaternalGenotype", "Mother");
            dgvPhasing.AddColumn("PhasedPaternal", "Phased Paternal");
            dgvPhasing.AddColumn("PhasedMaternal", "Phased Maternal");
        }

        private void dgvPhasing_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var row = dt[e.RowIndex];

            if (row.Mutated) {
                e.CellStyle.BackColor = Color.Red;
                e.CellStyle.ForeColor = Color.White;
            }
                
            if (row.Ambiguous) {
                e.CellStyle.BackColor = Color.LightGray;
            }
        }

        private void btnFather_Click(object sender, EventArgs e)
        {
            fatherKit = Program.KitInstance.SelectKit();
            UpdateControls();
        }

        private void btnMother_Click(object sender, EventArgs e)
        {
            motherKit = Program.KitInstance.SelectKit();
            UpdateControls();
        }

        private void btnChild_Click(object sender, EventArgs e)
        {
            childKit = Program.KitInstance.SelectKit();
            UpdateControls();
        }

        private void UpdateControls()
        {
            btnFather.Text = GKSqlFuncs.GetKitName(fatherKit);
            btnMother.Text = GKSqlFuncs.GetKitName(motherKit);
            btnChild.Text = GKSqlFuncs.GetKitName(childKit);

            btnPhasing.Enabled = ((fatherKit != "Unknown" || motherKit != "Unknown") && childKit != "Unknown");
        }

        private void btnPhasing_Click(object sender, EventArgs e)
        {
            Program.KitInstance.SetStatus("Phasing...");

            btnPhasing.Enabled = false;
            btnChild.Enabled = false;
            btnFather.Enabled = false;
            btnMother.Enabled = false;
            rbMale.Enabled = false;
            rbFemale.Enabled = false;

            male = rbMale.Checked;

            Task.Factory.StartNew(() => {
                GKGenFuncs.DoPhasing(fatherKit, motherKit, childKit, ref dt, male);

                this.Invoke(new MethodInvoker(delegate {
                    UpdateView();
                }));
            });
        }

        private void UpdateView()
        {
            dgvPhasing.DataSource = dt;

            btnPhasing.Enabled = true;
            btnChild.Enabled = true;
            btnFather.Enabled = true;
            btnMother.Enabled = true;
            rbMale.Enabled = true;
            rbFemale.Enabled = true;

            Program.KitInstance.SetStatus("Done.");
        }
    }
}

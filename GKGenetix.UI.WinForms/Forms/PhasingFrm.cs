/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using GKGenetix.Core;
using GKGenetix.Core.Database;
using GKGenetix.Core.Model;

namespace GKGenetix.UI.Forms
{
    public partial class PhasingFrm : GKWidget
    {
        private string fatherKit = "Unknown";
        private string motherKit = "Unknown";
        private string childKit = "Unknown";
        private IList<PhaseRow> dt = null;
        private string chSex;


        public PhasingFrm(IKitHost host) : base(host)
        {
            InitializeComponent();

            UIHelper.FixGridView(dgvPhasing);

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
            fatherKit = _host.SelectKit('M');
            UpdateControls();
        }

        private void btnMother_Click(object sender, EventArgs e)
        {
            motherKit = _host.SelectKit('F');
            UpdateControls();
        }

        private void btnChild_Click(object sender, EventArgs e)
        {
            childKit = _host.SelectKit('U');
            UpdateControls();
        }

        private void UpdateControls()
        {
            btnFather.Text = GKSqlFuncs.GetKitName(fatherKit);
            btnMother.Text = GKSqlFuncs.GetKitName(motherKit);

            var testRec = GKSqlFuncs.GetKit(childKit);
            btnChild.Text = testRec?.Name;

            btnPhasing.Enabled = ((fatherKit != "Unknown" || motherKit != "Unknown") && childKit != "Unknown");
        }

        private void btnPhasing_Click(object sender, EventArgs e)
        {
            _host.SetStatus("Phasing...");

            btnPhasing.Enabled = false;
            btnChild.Enabled = false;
            btnFather.Enabled = false;
            btnMother.Enabled = false;

            bool male = chSex[0] == 'M';

            Task.Factory.StartNew(() => {
                GKGenFuncs.DoPhasing(_host, fatherKit, motherKit, childKit, ref dt, male);

                this.Invoke(new MethodInvoker(delegate {
                    _host.SetStatus($"Saving Phased Kit {childKit} ...");

                    dgvPhasing.DataSource = dt;

                    btnPhasing.Enabled = true;
                    btnChild.Enabled = true;
                    btnFather.Enabled = true;
                    btnMother.Enabled = true;

                    _host.SetStatus("Done.");
                }));
            });
        }
    }
}

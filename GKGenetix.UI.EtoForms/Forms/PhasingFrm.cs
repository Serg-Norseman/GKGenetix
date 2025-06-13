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
    public partial class PhasingFrm : GKWidget
    {
        #region Design components
#pragma warning disable CS0169, CS0649, IDE0044, IDE0051

        private Button btnFather;
        private Button btnMother;
        private Button btnChild;
        private Button btnPhasing;
        private GridView dgvPhasing;

#pragma warning restore CS0169, CS0649, IDE0044, IDE0051
        #endregion


        private string fatherKit = "Unknown";
        private string motherKit = "Unknown";
        private string childKit = "Unknown";
        private IList<PhaseRow> dt = null;
        private string chSex;


        public PhasingFrm(IKitHost host) : base(host)
        {
            XamlReader.Load(this);

            UIHelper.FixGridView(dgvPhasing);

            dgvPhasing.AddColumn("rsID", "RSID");
            dgvPhasing.AddColumn("ChrStr", "Chromosome");
            dgvPhasing.AddColumn("Position", "Position");
            dgvPhasing.AddColumn("Child", "Child");
            dgvPhasing.AddColumn("Father", "Father");
            dgvPhasing.AddColumn("Mother", "Mother");
            dgvPhasing.AddColumn("PhasedPaternal", "Phased Paternal");
            dgvPhasing.AddColumn("PhasedMaternal", "Phased Maternal");

            Text = "Phasing Utility";
        }

        private void dgvPhasing_CellFormatting(object sender, GridCellFormatEventArgs e)
        {
            var row = dt[e.Row];

            if (row.Mutated) {
                e.BackgroundColor = Colors.Red;
                e.ForegroundColor = Colors.White;
            }
                
            if (row.Ambiguous) {
                e.BackgroundColor = Colors.LightGrey;
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

            GKSqlFuncs.GetKit(childKit, out string chName, out chSex);
            btnChild.Text = chName;

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

                Application.Instance.Invoke(new Action(delegate {
                    _host.SetStatus($"Saving Phased Kit {childKit} ...");

                    dgvPhasing.DataStore = dt;

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

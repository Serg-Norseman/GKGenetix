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

using System;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Serialization.Xaml;
using GKGenetix.Core;

namespace GKGenetix.UI.Forms
{
    public partial class ImportTestFrm : Dialog
    {
        #region Design components
#pragma warning disable CS0169, CS0649, IDE0044, IDE0051

        private Button btnOpen;
        private GridView dgvTests;

#pragma warning restore CS0169, CS0649, IDE0044, IDE0051
        #endregion


        private DNATestInfo fTest = null;
        private IList<DNATestInfo> fTestsList;

        public DNATestInfo GetSelectedTest()
        {
            return fTest;
        }

        public ImportTestFrm(IList<DNATestInfo> availableTests)
        {
            XamlReader.Load(this);

            UIHelper.FixGridView(dgvTests);
            dgvTests.AddColumn("Name", "Name");
            dgvTests.AddColumn("Date", "Date");
            dgvTests.AddColumn("Sex", "Sex");
            dgvTests.AddColumn("FileReference", "File path");

            fTestsList = availableTests;
        }

        private void OpenKitFrm_Load(object sender, EventArgs e)
        {
            btnOpen.Text = "Import";
            dgvTests.DataStore = fTestsList;
            if (fTestsList.Count == 0) {
                MessageBox.Show("There are no tests available to import.", "", MessageBoxButtons.OK, MessageBoxType.Warning);
                this.Close();
            }
        }

        private void dgvTests_CellContentDoubleClick(object sender, EventArgs e)
        {
            SelectTest();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            SelectTest();
        }

        private void SelectTest()
        {
            this.fTest = dgvTests.GetSelectedObj<DNATestInfo>();
            this.Close();
        }
    }
}

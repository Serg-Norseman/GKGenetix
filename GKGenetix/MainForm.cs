/*
 *  "GEDKeeper", the personal genealogical database editor.
 *  Copyright (C) 2009-2022 by Sergey V. Zhdanovskih.
 *
 *  This file is part of "GEDKeeper".
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

using System.Windows.Forms;
using GKGenetix.UI;

namespace GKGenetix
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnDNAAnalysis_Click(object sender, System.EventArgs e)
        {
            using (var dlg = new DNAAnalysis()) {
                dlg.ShowDialog();
            }
        }

        private void btnDNAInheritanceTest_Click(object sender, System.EventArgs e)
        {
            using (var dlg = new DNAInheritanceTest()) {
                dlg.ShowDialog();
            }
        }
    }
}

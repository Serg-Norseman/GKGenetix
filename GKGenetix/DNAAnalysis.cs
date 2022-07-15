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

using System;
using System.IO;
using System.Windows.Forms;
using GKGenetix.Core;

namespace GKGenetix
{
    public partial class DNAAnalysis : Form, IDisplay
    {
        private string fFileName;
        private DNAData fDNA;

        public DNAAnalysis()
        {
            InitializeComponent();
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog()) {
                dlg.Multiselect = false;
                if (dlg.ShowDialog() == DialogResult.OK) {
                    var file = dlg.FileNames[0];

                    fFileName = file;
                    fDNA = FileFormats.ReadAncestryDNAFile(fFileName);
                    fDNA.DetermineSex();

                    WriteLine("File name: " + Path.GetFileName(fFileName));
                    WriteLine("Sex: " + fDNA.Sex.ToString());
                }
            }
        }

        public void WriteLine(string value)
        {
            txtOutput.Text += value;
            txtOutput.Text += "\r\n";
        }
    }
}

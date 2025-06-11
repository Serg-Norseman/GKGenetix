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
using Eto.Drawing;
using Eto.Forms;
using Eto.Serialization.Xaml;
using GKGenetix.Core;
using GKGenetix.Core.FileFormats;
using GKGenetix.Core.Model;

namespace GKGenetix.UI.Forms
{
    public partial class DevConsole : GKWidget, IDisplay
    {
        #region Design components
#pragma warning disable CS0169, CS0649, IDE0044, IDE0051

        private TextArea txtOutput;

#pragma warning restore CS0169, CS0649, IDE0044, IDE0051
        #endregion


        enum ProcessingType
        {
            InheritanceTest,
            DetermineHaplogroupsY,
        }

        private List<DNAData> fFiles;

        public DevConsole(IKitHost host) : base(host)
        {
            XamlReader.Load(this);

            Text = "DNA Analysis Development Console";
            txtOutput.Font = new Font(FontFamilies.Monospace, 9, FontStyle.None);
            fFiles = new List<DNAData>();
        }

        private void btnInheritanceTest_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog()) {
                dlg.MultiSelect = true;
                if (dlg.ShowDialog(this) == DialogResult.Ok) {
                    ProcessFiles(dlg.Filenames, ProcessingType.InheritanceTest);
                }
            }
        }

        private void btnDetermineHaplogroupsY_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog()) {
                dlg.MultiSelect = true;
                if (dlg.ShowDialog(this) == DialogResult.Ok) {
                    ProcessFiles(dlg.Filenames, ProcessingType.DetermineHaplogroupsY);
                }
            }
        }

        private void ProcessFiles(IEnumerable<string> files, ProcessingType processingType)
        {
            fFiles.Clear();
            foreach (var file in files) {
                var dfi = FileFormatsHelper.ReadFile(file);
                fFiles.Add(dfi);
            }

            foreach (var dfi in fFiles) {
                dfi.DetermineSex();
            }

            if (processingType == ProcessingType.InheritanceTest) {
                for (int i = 0; i < fFiles.Count; i++) {
                    var dfi1 = fFiles[i];

                    for (int k = i + 1; k < fFiles.Count; k++) {
                        var dfi2 = fFiles[k];
                        Analytics.Compare(dfi1, dfi2, this);
                    }
                }
            } else if (processingType == ProcessingType.DetermineHaplogroupsY) {
                for (int i = 0; i < fFiles.Count; i++) {
                    var dfi1 = fFiles[i];

                    Analytics.DetermineHaplogroupsY(dfi1.FileName, dfi1, this);
                }
            }
        }

        void IDisplay.WriteLine(string value)
        {
            txtOutput.Text += value;
            txtOutput.Text += "\r\n";
        }
    }
}

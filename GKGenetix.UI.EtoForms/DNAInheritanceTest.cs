/*
 *  "GEDKeeper", the personal genealogical database editor.
 *  Copyright (C) 2009-2023 by Sergey V. Zhdanovskih.
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
using System.Collections.Generic;
using System.IO;
using Eto.Forms;
using Eto.Serialization.Xaml;
using GKGenetix.Core;
using GKGenetix.Core.FileFormats;
using GKUI.Components;

namespace GKGenetix.UI
{
    public partial class DNAInheritanceTest : Form, IDisplay
    {
        #region Design components
#pragma warning disable CS0169, CS0649, IDE0044, IDE0051

        private ButtonToolItem btnLoadFiles;
        private GKListView lvFiles;
        private TextArea txtOutput;

#pragma warning restore CS0169, CS0649, IDE0044, IDE0051
        #endregion


        enum ProcessStage
        {
            FileName, DNALoading, SexDefine, Analysis
        }

        class DNAFileInfo
        {
            public string FileName;
            public ProcessStage Stage;
            public DNAData DNA;
        }

        private List<DNAFileInfo> fFiles;

        public DNAInheritanceTest()
        {
            XamlReader.Load(this);

            fFiles = new List<DNAFileInfo>();
        }

        private void UpdateFiles()
        {
            lvFiles.BeginUpdate();
            lvFiles.Clear();
            lvFiles.AddColumn("File name", 160);

            foreach (DNAFileInfo dfi in fFiles) {
                var item = lvFiles.AddItem(Path.GetFileName(dfi.FileName));

                if (dfi.Stage >= ProcessStage.DNALoading) {
                    if (lvFiles.Columns.Count < 2) {
                        lvFiles.AddColumn("Loaded", 40);
                    }
                    //item.SubItems.Add("ok");
                }

                if (dfi.Stage >= ProcessStage.SexDefine) {
                    if (lvFiles.Columns.Count < 3) {
                        lvFiles.AddColumn("Sex", 40);
                    }
                    //item.SubItems.Add(dfi.DNA.Sex.ToString());
                }

                if (dfi.Stage >= ProcessStage.Analysis) {
                    if (lvFiles.Columns.Count < 4) {
                        lvFiles.AddColumn("Analysis", 40);
                    }
                    //item.SubItems.Add("Done");
                }
            }
            lvFiles.EndUpdate();

            Application.Instance.RunIteration();
        }

        private void btnLoadFiles_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog()) {
                dlg.MultiSelect = true;
                if (dlg.ShowDialog(this) == DialogResult.Ok) {
                    var files = dlg.Filenames;

                    fFiles.Clear();
                    foreach (var file in files) {
                        var dfi = new DNAFileInfo();
                        dfi.FileName = file;
                        fFiles.Add(dfi);
                        UpdateFiles();
                    }

                    foreach (var dfi in fFiles) {
                        dfi.DNA = FileFormatsHelper.ReadFile(dfi.FileName);
                        dfi.Stage = ProcessStage.DNALoading;
                        UpdateFiles();
                    }

                    foreach (var dfi in fFiles) {
                        dfi.DNA.DetermineSex();
                        dfi.Stage = ProcessStage.SexDefine;
                        UpdateFiles();
                    }

                    for (int i = 0; i < fFiles.Count; i++) {
                        var dfi1 = fFiles[i];

                        for (int k = i + 1; k < fFiles.Count; k++) {
                            var dfi2 = fFiles[k];
                            Analytics.Compare(dfi1.DNA, dfi2.DNA, this);
                        }

                        dfi1.Stage = ProcessStage.Analysis;
                        UpdateFiles();
                    }
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

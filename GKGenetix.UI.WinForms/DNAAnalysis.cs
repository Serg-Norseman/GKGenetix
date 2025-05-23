﻿/*
 *  "GEDKeeper", the personal genealogical database editor.
 *  Copyright (C) 2009-2025 by Sergey V. Zhdanovskih.
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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using GKGenetix.Core;
using GKGenetix.Core.FileFormats;

namespace GKGenetix.UI
{
    internal enum ProcessStage
    {
        FileName, DNALoading, SexDefine, Analysis
    }

    internal class DNAFileInfo
    {
        public string FileName;
        public ProcessStage Stage;
        public DNAData DNA;
    }

    public partial class DNAAnalysis : Form, IDisplay
    {
        private List<DNAFileInfo> fFiles;
        private string fFileName;
        private DNAData fDNA;

        public DNAAnalysis()
        {
            InitializeComponent();

            fFiles = new List<DNAFileInfo>();
        }

        private void btnSimpleAnalysis_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog()) {
                dlg.Multiselect = true;
                if (dlg.ShowDialog() == DialogResult.OK) {
                    var files = dlg.FileNames;

                    foreach (var file in files) {
                        fFileName = file;
                        fDNA = FileFormatsHelper.ReadFile(fFileName);
                        fDNA.DetermineSex();

                        WriteLine("File name: " + Path.GetFileName(fFileName));
                        WriteLine("Sex: " + fDNA.Sex.ToString());
                        WriteLine("SNPs: " + fDNA.SNP.Count.ToString());
                        WriteLine("Chromosomes: " + fDNA.Chromosomes.Count.ToString());

                        var haplogroups = Analytics.DetermineHaplogroupsY(fDNA);
                        WriteLine("Y Haplogroups: ");
                        foreach (var h in haplogroups) {
                            string moreSpecific = h.Specific ? "*" : " ";
                            WriteLine("    > " + moreSpecific + "\t" + h.Name);
                        }
                        WriteLine("\r\n");
                        Application.DoEvents();

                        Analytics.DetermineHaplogroupsTree(fDNA, this);
                    }

                    WriteLine("Analysis finished.");
                }
            }
        }

        public void WriteLine(string value)
        {
            txtOutput.Text += value;
            txtOutput.Text += "\r\n";
        }

        private void btnGenImage_Click(object sender, EventArgs e)
        {
            int imageWidth = 1024;
            int imageHeight = fDNA.SNP.Count / imageWidth;
            if (fDNA.SNP.Count % imageWidth != 0)
                imageHeight++;

            var backColor = Color.FromArgb(48, 48, 48);

            var image = new Bitmap(imageWidth, imageHeight);
            var pixelIdx = 0;
            foreach (var nucleotide in fDNA.SNP) {
                Color color;
                switch (nucleotide.Genotype.A2) {
                    case 'A':
                        color = Color.Red;
                        break;
                    case 'T':
                        color = Color.Yellow;
                        break;
                    case 'G':
                        color = Color.Blue;
                        break;
                    case 'C':
                        color = Color.ForestGreen;
                        break;
                    case 'N':
                    case '0':
                        color = backColor;
                        break;
                    default:
                        throw new Exception();
                }
                var row = pixelIdx / imageWidth;
                var column = pixelIdx - row * imageWidth;
                image.SetPixel(column, row, color);
                pixelIdx++;
            }

            string outputFilePath = Path.ChangeExtension(fFileName, ".png");
            image.Save(outputFilePath, ImageFormat.Png);
        }

        private void UpdateFilesIT()
        {
            lvFiles.BeginUpdate();
            lvFiles.Clear();
            lvFiles.Columns.Add("File name", 160);

            foreach (DNAFileInfo dfi in fFiles) {
                var item = lvFiles.Items.Add(Path.GetFileName(dfi.FileName));

                if (dfi.Stage >= ProcessStage.DNALoading) {
                    if (lvFiles.Columns.Count < 2) {
                        lvFiles.Columns.Add("Loaded", 40);
                    }
                    item.SubItems.Add("ok");
                }

                if (dfi.Stage >= ProcessStage.SexDefine) {
                    if (lvFiles.Columns.Count < 3) {
                        lvFiles.Columns.Add("Sex", 40);
                    }
                    item.SubItems.Add(dfi.DNA.Sex.ToString());
                }

                if (dfi.Stage >= ProcessStage.Analysis) {
                    if (lvFiles.Columns.Count < 4) {
                        lvFiles.Columns.Add("Analysis", 40);
                    }
                    item.SubItems.Add("Done");
                }
            }
            lvFiles.EndUpdate();

            Application.DoEvents();
        }

        private void btnInheritanceTest_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog()) {
                dlg.Multiselect = true;
                if (dlg.ShowDialog() == DialogResult.OK) {
                    var files = dlg.FileNames;

                    fFiles.Clear();
                    foreach (var file in files) {
                        var dfi = new DNAFileInfo();
                        dfi.FileName = file;
                        fFiles.Add(dfi);
                        UpdateFilesIT();
                    }

                    foreach (var dfi in fFiles) {
                        dfi.DNA = FileFormatsHelper.ReadFile(dfi.FileName);
                        dfi.Stage = ProcessStage.DNALoading;
                        UpdateFilesIT();
                    }

                    foreach (var dfi in fFiles) {
                        dfi.DNA.DetermineSex();
                        dfi.Stage = ProcessStage.SexDefine;
                        UpdateFilesIT();
                    }

                    for (int i = 0; i < fFiles.Count; i++) {
                        var dfi1 = fFiles[i];

                        for (int k = i + 1; k < fFiles.Count; k++) {
                            var dfi2 = fFiles[k];
                            Analytics.Compare(dfi1.DNA, dfi2.DNA, this);
                        }

                        dfi1.Stage = ProcessStage.Analysis;
                        UpdateFilesIT();
                    }
                }
            }
        }
    }
}

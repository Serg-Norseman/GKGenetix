/*
 *  GKGenetix, the simple DNA analysis kit.
 *  Copyright (C) 2022-2026 by Sergey V. Zhdanovskih.
 *
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using GKGenetix.Core;
using GKGenetix.Core.FileFormats;
using GKGenetix.Core.Model;

namespace GKGenetix.UI.Forms
{
    public partial class DevConsole : GKWidget, IDisplay
    {
        private readonly List<DNAData> fFiles;
        private string fFileName;
        private DNAData fDNA;

        public DevConsole() : base(null)
        {
            InitializeComponent();

            fFiles = new List<DNAData>();
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

        private void btnInheritanceTest_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog()) {
                dlg.Multiselect = true;
                if (dlg.ShowDialog() == DialogResult.OK) {
                    var files = dlg.FileNames;

                    fFiles.Clear();
                    foreach (var file in files) {
                        var dfi = FileFormatsHelper.ReadFile(file);
                        fFiles.Add(dfi);
                    }

                    foreach (var dfi in fFiles) {
                        dfi.DetermineSex();
                    }

                    for (int i = 0; i < fFiles.Count; i++) {
                        var dfi1 = fFiles[i];

                        for (int k = i + 1; k < fFiles.Count; k++) {
                            var dfi2 = fFiles[k];
                            Analytics.Compare(dfi1, dfi2, this);
                        }
                    }
                }
            }
        }
    }
}

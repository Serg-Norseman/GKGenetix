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
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using GKGenetix.Core;
using GKGenetix.Core.FileFormats;

namespace GKGenetix.UI
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
    }
}

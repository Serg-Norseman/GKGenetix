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
using System.IO;
using GGKit.Core;
using GKGenetix.Core.Model;

namespace GKGenetix.Core.FileFormats
{
    public abstract class SNPFileReader : IDisposable
    {
        private const int BufferSize = 16384;

        private bool fCanDisposed;
        private char fFieldSeparator;
        private char fHeaderMark;
        private StreamReader fReader;

        protected SNPFileReader(StreamReader reader)
        {
            fReader = reader;
            fCanDisposed = false;
            SetParsingParameters();
        }

        public void Dispose()
        {
            if (fCanDisposed) {
                fReader.Dispose();
            }
        }

        protected abstract void SetParsingParameters();

        protected void SetParsingParameters(char headerMark, char fieldSeparator)
        {
            fHeaderMark = headerMark;
            fFieldSeparator = fieldSeparator;
        }

        protected virtual void ProcessHeaderLine(string line, DNAData data)
        {
        }

        protected virtual SNP ProcessDataLine(string[] fields)
        {
            return null;
        }

        public DNAData ReadData()
        {
            var result = new DNAData();

            char[] rsrs = GKData.RSRS;

            try {
                int snpIdx = 0;
                SNP prev_snp = null;
                Region curr_chr = null;

                List<string> ysnp = new List<string>();
                List<string> mtdna = new List<string>();

                while (fReader.Peek() != -1) {
                    string line = fReader.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        continue;

                    // header line
                    if (line[0] == fHeaderMark) {
                        ProcessHeaderLine(line, result);
                        continue;
                    }

                    var fields = line.Split(fFieldSeparator);
                    SNP snp = ProcessDataLine(fields);

                    if (snp != null) {
                        result.SNP.Add(snp);

                        // This if statement saves a pointer to the beginning of every chromosome.
                        // Allows comparison of chromosome lengths.
                        if (curr_chr == null) {
                            curr_chr = new Region(snpIdx, 0, false);
                            result.Chromosomes.Add(curr_chr);
                        } else if (snp.Chromosome != prev_snp.Chromosome) {
                            curr_chr.EndPosition = snpIdx - 1;
                            curr_chr = new Region(snpIdx, 0, false);
                            result.Chromosomes.Add(curr_chr);
                        }

                        ProcessSNP(snp, ysnp, mtdna, rsrs);

                        snpIdx++;
                        prev_snp = snp;
                    }
                }

                if (curr_chr != null) {
                    curr_chr.EndPosition = snpIdx - 1;
                }

                result.ydna = ysnp;
                result.mtdna = mtdna;
            } catch (Exception e) {
                Console.WriteLine("The file could not be read: " + e.Message);
            }
            return result;
        }

        private void ProcessSNP(SNP snpx, List<string> ysnp, List<string> mtdna, char[] rsrs)
        {
            var pos = snpx.Position.ToString();
            string genotype = snpx.Genotype.ToString();

            if (snpx.Chromosome == (byte)Chromosome.CHR_Y) {
                if (GKData.YMap.ContainsKey(pos)) {
                    string[] snp = GKGenFuncs.GetYSNP(pos, genotype);
                    if (snp[0].IndexOf(";") == -1)
                        ysnp.Add(snp[0] + snp[1]);
                    else
                        ysnp.Add(snp[0].Substring(0, snp[0].IndexOf(";")) + snp[1]);
                }
            } else if (snpx.Chromosome == (byte)Chromosome.MT) {
                if (rsrs[int.Parse(pos) - 1] != genotype[0] && genotype[0] != '-')
                    mtdna.Add(rsrs[int.Parse(pos) - 1].ToString() + pos + genotype);
            }
        }
    }
}

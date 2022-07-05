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
using System.Collections.Generic;
using System.IO;

namespace GKGenetix.Core
{
    public static class FileFormats
    {
        /// <summary>
        /// Reads a reference file with haplogroups.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static IList<Haplotype> ReadHaplotypeFile(string filePath)
        {
            var result = new List<Haplotype>(1200);

            try {
                using (StreamReader reader = new StreamReader(filePath)) {
                    while (reader.Peek() != -1) {
                        string line = reader.ReadLine();
                        if (!string.IsNullOrEmpty(line) && line[0] != '#' && line[2] != 'i') {
                            var fields = line.Split(fSeparator);

                            var ht = new Haplotype();
                            ht.Group = fields[0];
                            ht.rsID = fields[1];
                            ht.Pos = uint.Parse(fields[2]);
                            ht.Mutation = fields[3][0];
                            result.Add(ht);
                        }
                    }
                }
            } catch (IOException e) {
                Console.WriteLine("The file could not be read: " + e.Message);
            }

            return result;
        }


        private static readonly char[] fSeparator = new char[] { '\t' };


        /// <summary>
        /// Reads the AncestryDNA file (and 23AndMe?) file. Comments begin line with #.
        /// </summary>
        /// <param name="filePath">The file path of DNA.</param>
        public static DNAData ReadAncestryDNAFile(string filePath)
        {
            var result = new DNAData();

            try {
                int snpIdx = 0, chrPtr = 0;
                using (StreamReader reader = new StreamReader(filePath)) {
                    while (reader.Peek() != -1) {
                        string line = reader.ReadLine();
                        // Data validation: if line begins with 'r' then is most likely a SNP
                        if (!string.IsNullOrEmpty(line) && line[0] != '#' && line[2] != 'i') {
                            var fields = line.Split(fSeparator);

                            var snp = new SNP();
                            snp.rsID = fields[0];            // oldout: []
                            snp.Chr = byte.Parse(fields[1]); // oldout: [0]
                            snp.Pos = uint.Parse(fields[2]); // oldout: [3]
                            snp.A1 = fields[3][0];           // oldout: [1]
                            snp.A2 = fields[4][0];           // oldout: [2]
                            result.SNP.Add(snp);

                            // This if statement saves a pointer to the beginning of every chromosome.
                            // Allows comparison of chromosome lengths.
                            if (snpIdx == 0) {
                                result.ChromoPointers[0] = snpIdx;
                            } else if (result.SNP[snpIdx].Chr != result.SNP[snpIdx - 1].Chr) {
                                result.ChromoPointers[++chrPtr] = snpIdx;
                            }
                            snpIdx++;
                        }
                    }
                    result.ChromoPointers[result.ChromoPointers.Length - 1] = snpIdx;
                }
            } catch (IOException e) {
                Console.WriteLine("The file could not be read: " + e.Message);
            }

            return result;
        }
    }
}

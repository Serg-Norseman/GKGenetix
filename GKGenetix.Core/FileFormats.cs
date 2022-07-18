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
        private static readonly char[] TabSeparator = new char[] { '\t' };
        private static readonly char[] CommaSeparator = new char[] { ',' };


        public static string FileFilter_All = "All files (*.*)|*.*";
        public static string FileFilter_23AndMe = "23AndMe data files|*.txt";
        public static string FileFilter_AncestryDNA = "AncestryDNA data files|*.txt";
        public static string FileFilter_deCODEme = "deCODEme data files|*.csv";


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
                            var fields = line.Split(TabSeparator);

                            var poses = fields[2].Split(';');

                            foreach (var po in poses) {
                                var ht = new Haplotype();
                                ht.Group = fields[0];
                                ht.rsID = fields[1];
                                ht.Pos = uint.Parse(po.Trim());
                                ht.Mutation = fields[3][0];
                                result.Add(ht);
                            }
                        }
                    }
                }
            } catch (IOException e) {
                Console.WriteLine("The file could not be read: " + e.Message);
            }

            return result;
        }

        /// <summary>
        /// Reads the file.
        /// </summary>
        /// <param name="filePath">The file path of DNA.</param>
        public static DNAData ReadFile(string filePath)
        {
            string ext = Path.GetExtension(filePath);
            switch (ext) {
                case ".txt":
                    return ReadTabbedTextFile(filePath);
                case ".csv":
                    return ReadCommaSeparatedTextFile(filePath);
                default:
                    throw new Exception("Unknown file format");
            }
        }

        /// <summary>
        /// Reads the AncestryDNA or 23AndMe file. Comments begin line with #.
        /// </summary>
        /// <param name="filePath">The file path of DNA.</param>
        public static DNAData ReadTabbedTextFile(string filePath)
        {
            var result = new DNAData();
            result.PersonalName = Path.GetFileNameWithoutExtension(filePath);

            try {
                var fileFormat = RawDataFormat.rdfUnknown;
                int snpIdx = 0, chrPtr = 0;
                using (StreamReader reader = new StreamReader(filePath)) {
                    while (reader.Peek() != -1) {
                        string line = reader.ReadLine();
                        if (string.IsNullOrEmpty(line))
                            continue;

                        // header line
                        if (line[0] == '#') {
                            if (fileFormat == RawDataFormat.rdfUnknown) {
                                if (line.Contains("AncestryDNA")) {
                                    fileFormat = RawDataFormat.rdfAncestryDNA;
                                } else if (line.Contains("23andMe")) {
                                    fileFormat = RawDataFormat.rdf23AndMe;
                                }
                            }
                            continue;
                        }

                        // Data validation: if line begins with 'r' then is most likely a SNP
                        // AncestryDNA column headers line; starts with "rsid"
                        if (line[2] == 'i') {
                            continue;
                        }

                        var fields = line.Split(TabSeparator);

                        SNP snp = null;
                        switch (fileFormat) {
                            case RawDataFormat.rdfUnknown:
                                break;
                            case RawDataFormat.rdfAncestryDNA:
                                snp = ParseAncestryDNALine(fields);
                                break;
                            case RawDataFormat.rdf23AndMe:
                                snp = Parse23AndMeLine(fields);
                                break;
                        }

                        if (snp != null) {
                            result.SNP.Add(snp);
                            // This if statement saves a pointer to the beginning of every chromosome.
                            // Allows comparison of chromosome lengths.
                            if (snpIdx == 0) {
                                result.ChromoPointers[0] = snpIdx;
                            } else if (snp.Chr != result.SNP[snpIdx - 1].Chr) {
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

        private static SNP ParseAncestryDNALine(string[] fields)
        {
            // AncestryDNA: chromosome numbers from 1 to 25!

            var snp = new SNP();
            snp.rsID = fields[0];
            snp.Chr = (byte)fields[1].ParseChromosome();
            snp.Pos = uint.Parse(fields[2]);
            snp.Orientation = Orientation.Plus;
            snp.A1 = fields[3][0];
            snp.A2 = fields[4][0];
            return snp;
        }

        private static SNP Parse23AndMeLine(string[] fields)
        {
            // 23AndMe: chromosome numbers from 1..22 to X, Y, MT

            var snp = new SNP();
            snp.rsID = fields[0];
            snp.Chr = (byte)fields[1].ParseChromosome();
            snp.Pos = uint.Parse(fields[2]);
            snp.Orientation = Orientation.Plus;
            var genotype = fields[3]; // 23AndMe: CC...--
            if (!string.IsNullOrEmpty(genotype)) {
                snp.A1 = genotype[0];
                if (genotype.Length > 1) {
                    snp.A2 = genotype[1];
                }
            }
            return snp;
        }

        /// <summary>
        /// Reads the deCODEme file. Comments begin line with #.
        /// </summary>
        /// <param name="filePath">The file path of DNA.</param>
        public static DNAData ReadCommaSeparatedTextFile(string filePath)
        {
            var result = new DNAData();
            result.PersonalName = Path.GetFileNameWithoutExtension(filePath);

            try {
                var fileFormat = RawDataFormat.rdfUnknown;
                int snpIdx = 0, chrPtr = 0;
                using (StreamReader reader = new StreamReader(filePath)) {
                    while (reader.Peek() != -1) {
                        string line = reader.ReadLine();
                        if (string.IsNullOrEmpty(line))
                            continue;

                        // header line
                        if (line.StartsWith("Name")) {
                            if (fileFormat == RawDataFormat.rdfUnknown) {
                                fileFormat = RawDataFormat.rdfdeCODEme;
                            }
                            continue;
                        }

                        var fields = line.Split(CommaSeparator);

                        SNP snp = null;
                        switch (fileFormat) {
                            case RawDataFormat.rdfUnknown:
                                break;
                            case RawDataFormat.rdfdeCODEme:
                                snp = ParsedeCODEmeLine(fields);
                                break;
                        }

                        if (snp != null) {
                            result.SNP.Add(snp);
                            // This if statement saves a pointer to the beginning of every chromosome.
                            // Allows comparison of chromosome lengths.
                            if (snpIdx == 0) {
                                result.ChromoPointers[0] = snpIdx;
                            } else if (snp.Chr != result.SNP[snpIdx - 1].Chr) {
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

        private static SNP ParsedeCODEmeLine(string[] fields)
        {
            var snp = new SNP();
            snp.rsID = fields[0];
            // Variation = fields[1];
            snp.Chr = (byte)fields[2].ParseChromosome();
            snp.Pos = uint.Parse(fields[3]);
            snp.Orientation = fields[4].ParseOrientation();
            var genotype = fields[5];
            if (!string.IsNullOrEmpty(genotype)) {
                snp.A1 = genotype[0];
                if (genotype.Length > 1) {
                    snp.A2 = genotype[1];
                }
            }
            return snp;
        }
    }
}

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
using System.IO.Compression;
using GKGenetix.Core.Model;
using GKGenetix.Core.Reference;

namespace GKGenetix.Core.FileFormats
{
    public static class FileFormatsHelper
    {
        private static readonly char[] TabSeparator = new char[] { '\t' };


        public static string FileFilter_All = "All files (*.*)|*.*";
        public static string FileFilter_23AndMe = "23AndMe data files|*.txt";
        public static string FileFilter_AncestryDNA = "AncestryDNA data files|*.txt";
        public static string FileFilter_deCODEme = "deCODEme data files|*.csv";
        public static string FileFilter_VCF = "VCF variations files|*.vcf;*.vcf.gz";


        public static Haplogroup ReadHaplogroupTree(string gzName)
        {
            var gzStream = Utilities.LoadResourceGZipStream(gzName);
            return JsonHelper.DeserializeFromStream<Haplogroup>(gzStream);
        }

        /// <summary>
        /// Reads a reference file with haplogroups.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static IList<HaplogroupMutation> ReadHaplogroupMutations(string gzName)
        {
            var result = new List<HaplogroupMutation>(12000);

            try {
                var gzStream = Utilities.LoadResourceGZipStream(gzName);
                using (StreamReader reader = new StreamReader(gzStream)) {
                    while (reader.Peek() != -1) {
                        string line = reader.ReadLine();
                        if (!string.IsNullOrEmpty(line) && !line.StartsWith("Haplogroup")) {
                            var fields = line.Split(TabSeparator);

                            char oldNuc, newNuc;
                            if (fields[3].Contains("->")) {
                                var mutations = Extensions.ParseMutation(fields[3]);
                                oldNuc = mutations[0];
                                newNuc = mutations[1];
                            } else {
                                oldNuc = '0';
                                newNuc = fields[3][0];
                            }

                            var poses = fields[2].Split(';');
                            foreach (var po in poses) {
                                var ht = new HaplogroupMutation();
                                ht.Haplogroup = fields[0];
                                ht.rsID = fields[1];
                                ht.Position = po.ParsePosition();
                                ht.OldNucleotide = oldNuc;
                                ht.NewNucleotide = newNuc;
                                result.Add(ht);
                            }
                        }
                    }
                }
            } catch (Exception e) {
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
            DNAData result = null;

            string ext = Path.GetExtension(filePath);
            switch (ext) {
                case ".txt":
                    result = ReadTabbedTextFile(filePath);
                    break;

                case ".csv":
                    result = ReadCommaSeparatedTextFile(filePath);
                    break;

                case ".vcf":
                    using (var reader = new VCFFileReader(filePath)) {
                        result = reader.ReadData();
                    }
                    break;

                case ".gz":
                    if (filePath.EndsWith(".vcf.gz")) {
                        using (GZipStream gzInput = new GZipStream(new FileStream(filePath, FileMode.Open), CompressionMode.Decompress)) {
                            using (var reader = new VCFFileReader(gzInput)) {
                                result = reader.ReadData();
                            }
                        }
                    }
                    break;

                default:
                    throw new Exception("Unknown file format");
            }

            if (result != null) {
                result.PersonalName = Path.GetFileNameWithoutExtension(filePath);
            }

            return result;
        }

        /// <summary>
        /// Reads the AncestryDNA or 23AndMe file. Comments begin line with #.
        /// </summary>
        /// <param name="filePath">The file path of DNA.</param>
        public static DNAData ReadTabbedTextFile(string filePath)
        {
            DNAData result = null;

            try {
                SNPFileReader snpReader = null;
                using (StreamReader streamReader = new StreamReader(filePath)) {
                    string line = streamReader.ReadLine();

                    if (!string.IsNullOrEmpty(line) && line[0] == '#') {
                        if (line.Contains("AncestryDNA")) {
                            snpReader = new SNPAncestryDNAFileReader(streamReader);
                        } else if (line.Contains("23andMe")) {
                            snpReader = new SNP23andMeFileReader(streamReader);
                        } else {
                            throw new Exception("Unknown file format");
                        }
                    }

                    result = snpReader.ReadData();
                }
            } catch (Exception e) {
                Console.WriteLine("The file could not be read: " + e.Message);
            }

            return result;
        }

        /// <summary>
        /// Reads the deCODEme file.
        /// </summary>
        /// <param name="filePath">The file path of DNA.</param>
        public static DNAData ReadCommaSeparatedTextFile(string filePath)
        {
            DNAData result = null;

            try {
                SNPFileReader snpReader = null;
                using (StreamReader streamReader = new StreamReader(filePath)) {
                    string line = streamReader.ReadLine();

                    if (!string.IsNullOrEmpty(line)) {
                        if (line.StartsWith("Name")) {
                            snpReader = new SNPdeCODEmeFileReader(streamReader);
                        } else {
                            throw new Exception("Unknown file format");
                        }
                    }

                    result = snpReader.ReadData();
                }
            } catch (Exception e) {
                Console.WriteLine("The file could not be read: " + e.Message);
            }

            return result;
        }
    }
}

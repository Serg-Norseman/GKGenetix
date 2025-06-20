﻿/*
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
using System.Text;
using GKGenetix.Core.Model;
using GKGenetix.Core.Reference;

namespace GKGenetix.Core.FileFormats
{
    internal enum RawFileFormat
    {
        RFF_UNKNOWN,

        RFF_23ANDME,
        RFF_ANCESTRY,
        RFF_DECODEME,
        RFF_FTDNA,
        RFF_GENO2,
        RFF_VCF,
    }


    public static class FileFormatsHelper
    {
        private static readonly char[] TabSeparator = new char[] { '\t' };


        public static string FileFilter_All = "All DNA files (*.*)|*.txt;*.txt.gz;*.csv;*.csv.gz;*.vcf;*.vcf.gz";
        public static string FileFilter_23AndMe = "23AndMe data files|*.txt";
        public static string FileFilter_AncestryDNA = "AncestryDNA data files|*.txt";
        public static string FileFilter_deCODEme = "deCODEme data files|*.csv";
        public static string FileFilter_VCF = "VCF variations files|*.vcf;*.vcf.gz";
        public static string FileFilter_Geno2 = "Geno2 data files|*.csv;*.csv.gz";
        public static string FileFilter_FTDNA = "FTDNA data files|*.csv";


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

            var type = DetectFileType(filePath);

            using (var fileStream = new FileStream(filePath, FileMode.Open)) {
                Stream inputStream;
                if (filePath.EndsWith(".gz")) {
                    inputStream = new GZipStream(fileStream, CompressionMode.Decompress);
                    filePath = filePath.Substring(0, filePath.Length - 3);
                } else {
                    inputStream = fileStream;
                }

                using (inputStream)
                using (var streamReader = new StreamReader(inputStream, Encoding.ASCII)) {
                    SNPFileReader snpReader = null;
                    switch (type) {
                        case RawFileFormat.RFF_UNKNOWN:
                            throw new Exception("Unable to identify file format for " + filePath);

                        case RawFileFormat.RFF_23ANDME:
                            snpReader = new SNP23andMeFileReader(streamReader);
                            break;

                        case RawFileFormat.RFF_ANCESTRY:
                            snpReader = new SNPAncestryDNAFileReader(streamReader);
                            break;

                        case RawFileFormat.RFF_DECODEME:
                            snpReader = new SNPdeCODEmeFileReader(streamReader);
                            break;

                        case RawFileFormat.RFF_FTDNA:
                            snpReader = new SNPFTDNAFileReader(streamReader);
                            break;

                        case RawFileFormat.RFF_GENO2:
                            snpReader = new SNPGeno2FileReader(streamReader);
                            break;

                        case RawFileFormat.RFF_VCF:
                            snpReader = new VCFFileReader(streamReader);
                            break;
                    }

                    if (snpReader != null) {
                        result = snpReader.ReadData();

                        snpReader.Dispose();
                    }
                }
            }

            if (result != null) {
                result.FileName = filePath;
                result.PersonalName = Path.GetFileNameWithoutExtension(filePath);
            }

            return result;
        }

        /// <summary>
        /// Reopening the stream is necessary in cases where we read from a decompression stream,
        /// since in this case it is not possible to return to the beginning if the type definition
        /// has gone into the SNP data zone.
        /// </summary>
        private static RawFileFormat DetectFileType(string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open)) {
                Stream inputStream;
                if (filePath.EndsWith(".gz")) {
                    inputStream = new GZipStream(fileStream, CompressionMode.Decompress);
                    filePath = filePath.Substring(0, filePath.Length - 3);
                } else {
                    inputStream = fileStream;
                }

                string ext = Path.GetExtension(filePath);
                if (ext == ".vcf") {
                    return RawFileFormat.RFF_VCF;
                }

                using (var streamReader = new StreamReader(inputStream, Encoding.ASCII, false, 16 * 1024, true)) {
                    int count = 0;
                    while (streamReader.Peek() != -1) {
                        string line = streamReader.ReadLine();

                        if (line == "RSID,CHROMOSOME,POSITION,RESULT")
                            return RawFileFormat.RFF_FTDNA;
                        if (line == "# rsid\tchromosome\tposition\tgenotype")
                            return RawFileFormat.RFF_23ANDME;
                        if (line == "rsid\tchromosome\tposition\tallele1\tallele2")
                            return RawFileFormat.RFF_ANCESTRY;
                        if (line == "Name,Variation,Chromosome,Position,Strand,YourCode")
                            return RawFileFormat.RFF_DECODEME;
                        if (line == "SNP,Chr,Allele1,Allele2")
                            return RawFileFormat.RFF_GENO2;

                        // if above doesn't work
                        if (line.Split("\t".ToCharArray()).Length == 4)
                            return RawFileFormat.RFF_23ANDME;
                        if (line.Split("\t".ToCharArray()).Length == 5)
                            return RawFileFormat.RFF_ANCESTRY;
                        if (line.Split(",".ToCharArray()).Length == 4)
                            return RawFileFormat.RFF_FTDNA;
                        if (line.Split(",".ToCharArray()).Length == 6)
                            return RawFileFormat.RFF_DECODEME;

                        if (count > 40) {
                            // detection useless... 
                            break;
                        }
                        count++;
                    }

                    return RawFileFormat.RFF_UNKNOWN;
                }
            }
        }
    }
}

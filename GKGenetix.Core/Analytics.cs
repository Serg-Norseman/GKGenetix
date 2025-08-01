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
using System.Linq;
using BSLib;
using GKGenetix.Core.FileFormats;
using GKGenetix.Core.Model;
using GKGenetix.Core.Reference;

namespace GKGenetix.Core
{
    public class PersonalHaplogroup
    {
        public string Name { get; set; }
        public bool Specific { get; set; }
    }

    /// <summary>
    /// Methods for data analysis.
    /// 
    /// Created based on the Isaac Styles code from the DNAInheritanceTest project.
    /// </summary>
    public static class Analytics
    {
        // haplogroups to search for (2016, build 37)
        public static readonly IList<HaplogroupMutation> dbHaplogroupMutationsY = FileFormatsHelper.ReadHaplogroupMutations("ydna_snp_index_2016.txt.gz");

        public static void DetermineHaplogroupsY(string fileName, DNAData dna, IDisplay display)
        {
            display.WriteLine("File name: " + Path.GetFileName(fileName));
            display.WriteLine("Sex: " + dna.Sex.ToString());
            display.WriteLine("SNPs: " + dna.SNP.Count.ToString());
            display.WriteLine("Chromosomes: " + dna.Chromosomes.Count.ToString());

            var haplogroups = DetermineHaplogroupsY(dna);
            display.WriteLine("Y Haplogroups: ");
            foreach (var h in haplogroups) {
                string moreSpecific = h.Specific ? "*" : " ";
                display.WriteLine("    > " + moreSpecific + "\t" + h.Name);
            }
            display.WriteLine("\r\n");

            DetermineHaplogroupsTree(dna, display);
        }

        /// <summary>
        /// Determines the Y haplogroup.
        /// </summary>
        /// <param name="dna">The dna.</param>
        /// <returns></returns>
        public static List<PersonalHaplogroup> DetermineHaplogroupsY(DNAData dna)
        {
            // list of haplogroups that were expressed in Y chromosome
            var hgs = new List<string>();

            // look at Y chromosome
            var chrY = dna.Chromosomes[23];
            for (int i = chrY.StartPosition; i <= chrY.EndPosition; i++) {
                var snp = dna.SNP[i];

                // iterate through the haplogroups
                for (int j = 0; j < dbHaplogroupMutationsY.Count; j++) {
                    var hGroup = dbHaplogroupMutationsY[j];
                    // position matches known haplogroup
                    if (snp.Position == hGroup.Position) {
                        // mutation matches haplogroup
                        if (snp.Genotype.A1 == hGroup.NewNucleotide && !hgs.Contains(hGroup.Haplogroup)) {
                            hgs.Add(hGroup.Haplogroup);
                        }
                    }
                }
            }

            var result = new List<PersonalHaplogroup>(hgs.Count);
            for (int x = 0; x < hgs.Count; x++) {
                result.Add(new PersonalHaplogroup() { Name = hgs[x], Specific = true });
            }

            // compare each pair of haplogroups once
            for (int x = 0; x < hgs.Count; x++) {
                for (int y = x + 1; y < hgs.Count; y++) {
                    // if haplogroup is less specific, or haplogroups have same number of characters
                    if (hgs[x] == null || hgs[y] == null || hgs[x].Length == hgs[y].Length) {
                        // then cannot determine specificity
                        continue;
                    }

                    // if lengths differ, begin looking at characters to see if they belong to same branch
                    if (hgs[x].Length < hgs[y].Length) {
                        // set to false when a character doesn't match, indicating two different branches
                        bool morespecific = true;
                        // iterate the chars, looking for a mismatch
                        for (int i = 0; i < hgs[x].Length; i++) {
                            // if mismatch is found, then the longer haplogroup is not more specific
                            if (hgs[x][i] != hgs[y][i]) {
                                morespecific = false;
                            }
                        }

                        // if mismatch is not found, then the longer haplogroup is more specific
                        // specify the shorter haplogroup
                        if (morespecific == true) {
                            hgs[x] = null;
                            result[x].Specific = false;
                        }
                    } else {
                        // the second haplogroup is longer, compare chars and look for a mismatch
                        bool morespecific = true;
                        for (int i = 0; i < hgs[y].Length; i++) {
                            if (hgs[x][i] != hgs[y][i]) {
                                morespecific = false;
                            }
                        }

                        if (morespecific == true) {
                            hgs[y] = null;
                            result[y].Specific = false;
                        }
                    }
                }
            }

            return result;
        }

        public static void Compare(DNAData d1, DNAData d2, IDisplay display)
        {
            try {
                IList<SNP> d1snp = d1.SNP;
                IList<SNP> d2snp = d2.SNP;

                // alorithm counts chromosome length during file parsing and ensures there are the same number of compares
                // USED TO guarantee that chromosomes are of same length prior to comparison,

                display.WriteLine("Chromosomes 1: " + d1.Chromosomes.Count.ToString());
                display.WriteLine("Chromosomes 2: " + d2.Chromosomes.Count.ToString());

                // check that chromosomes are equilength
                if (d1.Chromosomes.Count != d2.Chromosomes.Count) {
                    throw new Exception("The number of chromosomes does not match");
                }

                bool notAligned = false;
                string message = string.Empty;
                for (int i = 0; i < d1.Chromosomes.Count; i++) {
                    if (d1.Chromosomes[i].EndPosition != d2.Chromosomes[i].EndPosition) {
                        message = "Chromosomes not aligned at chromo " + (i + 1);
                        notAligned = true;
                        display.WriteLine(message);
                    }
                    if (d1.Chromosomes[i].Size != d2.Chromosomes[i].Size) {
                        var szMessage = "Chromosomes has different size at chromo " + (i + 1);
                        display.WriteLine(szMessage);
                    }
                }
                if (notAligned) {
                    throw new Exception(message);
                }

                // holds the scores of all 4 comparisons of each chromosome
                double[,] strandScores = new double[25, 4];

                // compare one strand to the other persons strands, looking for possibility of inheritance
                for (int c = 0; c < 25; c++) {
                    var chReg = d1.Chromosomes[c];

                    // do 4 comparisions for each chromosome
                    for (int comparisonIndex = 0; comparisonIndex < 4; comparisonIndex++) {
                        int match = 0, total = 0;

                        for (int snpIdx = chReg.StartPosition; snpIdx <= chReg.EndPosition; snpIdx++) {
                            var snp1gt = d1snp[snpIdx].Genotype;
                            var snp2gt = d2snp[snpIdx].Genotype;
                            switch (comparisonIndex) {
                                case 0:
                                    // person 1 strand 1
                                    match += snp1gt.CompareStrand(snp2gt, 1);
                                    total++;
                                    break;
                                case 1:
                                    // person 1 strand 2
                                    match += snp1gt.CompareStrand(snp2gt, 2);
                                    total++;
                                    break;
                                case 2:
                                    // person 2 strand 1
                                    match += snp2gt.CompareStrand(snp1gt, 1);
                                    total++;
                                    break;
                                case 3:
                                    // person 2 strand 2
                                    match += snp2gt.CompareStrand(snp1gt, 2);
                                    total++;
                                    break;
                            }
                        }
                        strandScores[c, comparisonIndex] = match / (double)total;
                    }
                }

                // holds the resulting product of each persons probability of inheritance
                double[,] chromoScore = new double[25, 2];

                // multiply probability of inheritance for each persons' pair of chromosomes
                for (int i = 0; i < 25; i++) {
                    chromoScore[i, 0] = strandScores[i, 0] * strandScores[i, 1];
                    chromoScore[i, 1] = strandScores[i, 2] * strandScores[i, 3];
                }

                // holds the percent match for person 1 and person 2
                double[] parentScore = { 1d, 1d };

                // multiply probability of inheritance for each persons' genome
                for (int i = 0; i < 25; i++) {
                    parentScore[0] = parentScore[0] * chromoScore[i, 0];
                    parentScore[1] = parentScore[1] * chromoScore[i, 1];
                }

                // the ratio of correlation of relation
                double correlation;

                // calculation percent different of inheritance scores
                correlation = Math.Abs(parentScore[0] - parentScore[1]);
                correlation /= ((parentScore[0] + parentScore[1]) / 2);

                // Determine sex of parent and child, then output sex chromosomes
                d1.DetermineSex();
                d2.DetermineSex();

                // temp string for parent name
                string parent;
                // temp for child name
                string child;
                // 0-based pointer to person determined to be parent
                int pPointerToParent;
                // display the results
                if (parentScore[0] < parentScore[1]) {
                    parent = string.Format("{0} ({1})", d1.PersonalName, d1.Sex.ToString());
                    child = string.Format("{0} ({1})", d2.PersonalName, d2.Sex.ToString());
                    pPointerToParent = 0;
                } else if (parentScore[0] > parentScore[1]) {
                    parent = string.Format("{0} ({1})", d2.PersonalName, d2.Sex.ToString());
                    child = string.Format("{0} ({1})", d1.PersonalName, d1.Sex.ToString());
                    pPointerToParent = 1;
                } else {
                    // scored the same
                    throw new Exception("Best-fit for parent/child relation not found. Check that two unique individuals are being compared.");
                }

                // Display results
                string diagTextOut = "";
                diagTextOut += "      Parent / Child Relationship:\r\n  Parent:" + parent + "\r\n   Child:" + child + "\r\n";
                diagTextOut += "         Correlation (larger is better): " + correlation;
                diagTextOut += "\r\n              s1+t1  s1+t2  s2+t1  s2+t2";    // display chromosome scores
                for (int i = 0; i < 25; i++) {
                    diagTextOut += "\r\nChromosome" + (i + 1).ToString("00") + ":";
                    for (int j = 0; j < 4; j++) {
                        diagTextOut += Math.Round(strandScores[i, j], 4).ToString("0.000").PadLeft(7, ' ');
                    }
                }

                display.WriteLine(diagTextOut);
                diagTextOut = "";

                // 1-based pointer to non-matched strand of child
                int[] pMissingParentChromo = new int[25];

                // Find pointers to best matched chromosome of child
                if (pPointerToParent == 1) {
                    // person 2 is parent
                    for (int i = 0; i < 25; i++) {
                        if (strandScores[i, 0] > strandScores[i, 1]) {
                            // if P1 chromo 1 best match
                            // chromo 2 is from missing parent
                            pMissingParentChromo[i] = 2;
                        } else {
                            // if P1 chromo 2 best match
                            // chromo 1 is from missing parent
                            pMissingParentChromo[i] = 1;
                        }
                    }
                } else {
                    // person 1 is parent
                    for (int i = 0; i < 25; i++) {
                        if (strandScores[i, 2] > strandScores[i, 3]) {
                            // P2 chromo 1 best match
                            // chromo 2 is best match
                            pMissingParentChromo[i] = 2;
                        } else {
                            // P2 chromo 2 is best match
                            // chromo 1 is best match
                            pMissingParentChromo[i] = 1;
                        }
                    }
                }

                // Begin output of missing parent
                // initialize the missing parent
                // holds the array of chromoNumber and Allele of the extrapolated parent
                var missingParent = new char[d1.SNP.Count];

                // copy child's non-matched strand to missing parent
                if (pPointerToParent == 1) {
                    // person 2 is parent
                    for (int i = 0; i < 23; i++) {
                        var chReg = d1.Chromosomes[i];
                        for (int snpIdx = chReg.StartPosition; snpIdx <= chReg.EndPosition; snpIdx++) {
                            missingParent[snpIdx] = d1snp[snpIdx].Genotype[pMissingParentChromo[i]];
                        }
                    }
                } else {
                    for (int i = 0; i < 23; i++) {
                        var chReg = d1.Chromosomes[i];
                        for (int snpIdx = chReg.StartPosition; snpIdx <= chReg.EndPosition; snpIdx++) {
                            missingParent[snpIdx] = d2snp[snpIdx].Genotype[pMissingParentChromo[i]];
                        }
                    }
                }

                var chrY = d1.Chromosomes[23];
                int x = pMissingParentChromo[23];
                if (pPointerToParent == 1) {
                    // person 2 is parent
                    if (d2.IsFemale) {
                        // parent is female; give missing parent X and look for Y
                        if (d1.IsFemale) {
                            // both parent and child are female, NO KNOWN Y chromosome
                            diagTextOut = "Exporting Missing Parent: No known Y chromosome exists. Defaulting to '0'.";
                            // copy child's Y to missing parent
                            for (int i = chrY.StartPosition; i <= chrY.EndPosition; i++) {
                                missingParent[i] = '0';
                            }
                        } else {
                            // person 2 is female parent, but Y exists in P1
                            // copy child's Y to missing parent
                            for (int i = chrY.StartPosition; i <= chrY.EndPosition; i++) {
                                missingParent[i] = d1snp[i].Genotype[x];
                            }
                        }
                    } else {
                        // person 2 is male parent; give missing parent X & no Y
                        if (d1.IsFemale) {
                            // child is female; give child's chromosome 24
                            for (int i = chrY.StartPosition; i <= chrY.EndPosition; i++) {
                                missingParent[i] = d1snp[i].Genotype[x];
                            }
                        } else {
                            // both parent and child are male, NO KNOWN !Y chromosome
                            diagTextOut = "Exporting Missing Parent: No known !Y chromosome exists. Defaulting to '0'.";
                            for (int i = chrY.StartPosition; i <= chrY.EndPosition; i++) {
                                missingParent[i] = '0';
                            }
                        }
                    }
                } else {
                    // Person 1 is parent
                    if (d1.IsFemale) {
                        // parent is female; look for Y
                        // both parent and child are female, NO KNOWN Y chromosome
                        if (d2.IsFemale) {
                            diagTextOut = "Exporting Missing Parent: No known Y chromosome exists. Defaulting to '0'.";
                            // copy child's Y to missing parent
                            for (int i = chrY.StartPosition; i <= chrY.EndPosition; i++) {
                                missingParent[i] = '0';
                            }
                        } else {
                            // person 1 is female parent, but Y exists in P2
                            // copy child's Y to missing parent
                            for (int i = chrY.StartPosition; i <= chrY.EndPosition; i++) {
                                missingParent[i] = d2snp[i].Genotype[x];
                            }
                        }
                    } else {
                        // person 2 is male parent; give missing parent X & no Y
                        if (d2.IsFemale) {
                            // child is female; give child's chromosome 24
                            for (int i = chrY.StartPosition; i <= chrY.EndPosition; i++) {
                                missingParent[i] = d2snp[i].Genotype[x];
                            }
                        } else {
                            // both parent and child are male, NO KNOWN !Y chromosome
                            diagTextOut = "Exporting Missing Parent: No known !Y chromosome exists. Defaulting to '0'.";
                            for (int i = chrY.StartPosition; i <= chrY.EndPosition; i++) {
                                missingParent[i] = '0';
                            }
                        }
                    }
                }

                display.WriteLine(diagTextOut);

                // copy mitochondrial dna
                var chrMt = d1.Chromosomes[24];
                int m = pMissingParentChromo[24];
                IList<SNP> source;
                if (pPointerToParent == 1) {
                    // person 2 is parent
                    source = d1snp;
                } else {
                    // person 1 is parent
                    source = d2snp;
                }
                for (int i = chrMt.StartPosition; i <= chrMt.EndPosition; i++) {
                    missingParent[i] = source[i].Genotype[m];
                }

                /*display.WriteLine("Output the best guess for missing parent:");
                for (int i = 0; i < missingParent.Length; i++) {
                    display.WriteLine(missingParent[i].ToString());
                }*/

                display.WriteLine("\r\n\r\n");
            } catch (Exception ex) {
                display.WriteLine(ex.Message);
                display.WriteLine(ex.StackTrace.ToString());
            }
        }


        private const float HaplogroupMatchThreshold = 0.5F;

        private class HGMatch
        {
            public int Total;
            public int Exact;
            public float Ratio;
        }

        private static StringNode CreateNode(Haplogroup hg, Dictionary<string, HGMatch> haplogroupMatches)
        {
            StringNode result = null;

            foreach (var child in hg.Children) {
                var node = CreateNode(child, haplogroupMatches);
                if (node != null) {
                    if (result == null) {
                        result = new StringNode();
                    }
                    result.Children.Add(node);
                }
            }

            // if result is not null, then there are significant child nodes

            if (!haplogroupMatches.TryGetValue(hg.Name, out HGMatch match)) {
                if (result != null) {
                    result.Value = hg.Name;
                    return result;
                }
            }

            if ((match == null || match.Exact == 0) && result == null) {
                return result;
            } else {
                if (result == null) {
                    result = new StringNode();
                }

                if (match.Total == 0) {
                    result.Value = hg.Name;
                } else {
                    match.Ratio = match.Exact / (float)match.Total;
                    result.Value = string.Format("{0}: {1:n0}% ({2:n0}/{3:n0})", hg.Name, match.Ratio * 100, match.Exact, match.Total);
                }

                return result;
            }
        }

        public static void DetermineHaplogroupsTree(DNAData dna, IDisplay display)
        {
            try {
                // TODO: dna.Sex -> ydna_tree.json | mtdna_tree.json ?
                var hpTreeRoot = FileFormatsHelper.ReadHaplogroupTree("ydna_tree.json.gz");

                var mutationMatches = new Dictionary<string, HGMatch>();
                for (int i = 0; i < dna.SNP.Count; i++) {
                    var snp = dna.SNP[i];
                    for (int j = 0; j < dbHaplogroupMutationsY.Count; j++) {
                        var hGroup = dbHaplogroupMutationsY[j];
                        if (snp.Position == hGroup.Position) {
                            if (!mutationMatches.TryGetValue(hGroup.Haplogroup, out HGMatch tuple)) {
                                tuple = new HGMatch();
                                mutationMatches.Add(hGroup.Haplogroup, tuple);
                            }

                            tuple.Total += 1;
                            if (snp.Genotype.A1 == hGroup.NewNucleotide) {
                                tuple.Exact += 1;
                            }
                        }
                    }
                }

                var stringTreeRoot = CreateNode(hpTreeRoot, mutationMatches);

                display.WriteLine("Haplogroup Matches");
                display.WriteLine("\r\n");
                display.WriteLine(StringTree.Create(stringTreeRoot, x => x.Value, x => x.Children));

                var haplogroup = mutationMatches
                    .Where(x => x.Value.Ratio >= HaplogroupMatchThreshold)
                    .OrderByDescending(x => x.Value.Ratio)
                    .ThenByDescending(x => x.Key)
                    .Select(x => x.Key)
                    .FirstOrDefault();

                if (haplogroup != null) {
                    display.WriteLine(string.Format("Best match: {0}\r\n", haplogroup));
                }
            } catch (Exception ex) {
                display.WriteLine(ex.Message);
                display.WriteLine(ex.StackTrace.ToString());
            }
        }
    }
}

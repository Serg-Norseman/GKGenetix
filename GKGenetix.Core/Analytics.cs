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

namespace GKGenetix.Core
{
    /// <summary>
    /// Methods for data analysis.
    /// 
    /// Created based on the Isaac Styles code from the DNAInheritanceTest project.
    /// </summary>
    public static class Analytics
    {
        /// <summary>
        /// Determines whether the given DNA is female.
        /// </summary>
        /// <param name="dna">The DNA.</param>
        /// <returns>True if female. False if male.</returns>
        public static bool IsFemale(DNAData dna)
        {
            int count = 0;
            int total = 0;

            for (int i = dna.ChromoPointers[23]; i < dna.ChromoPointers[24]; i++) {
                // chromosome 24 is male Y
                if (dna.SNP[i].A1 == '0') {
                    count++;
                }
                total++;
            }

            // if the majority of Y were 0
            return count / (double)total > 0.9d;
        }

        /// <summary>
        /// Determines the Y haplogroup.
        /// </summary>
        /// <param name="dna">The dna.</param>
        /// <returns></returns>
        public static List<string> DetermineHaplogroup(DNAData dna)
        {
            // haplogroups to search for
            var dbHaplogroups = FileFormats.ReadHaplotypeFile(@"../../../temp/haplogroups.txt");

            // list of haplogroups that were expressed in Y chromosome
            List<string> foundHaplogroups = new List<string>();

            // look at Y chromosome
            for (int i = dna.ChromoPointers[23]; i < dna.ChromoPointers[24]; i++) {
                // iterate through the haplogroups
                for (int j = 0; j < dbHaplogroups.Count; j++) {
                    var hGroup = dbHaplogroups[j];
                    // position matches known haplogroup
                    if (dna.SNP[i].Pos == hGroup.Pos) {
                        // mutation matches haplogroup
                        if (dna.SNP[i].A1 == hGroup.Mutation && !foundHaplogroups.Contains(hGroup.Group)) {
                            foundHaplogroups.Add(hGroup.Group);
                        }
                    }
                }
            }

            // put haplogroups into array to maintain an index
            // list of most specific haplogroups
            var h = foundHaplogroups.ToArray();

            // corresponds to found haplogroups that may be ignored
            var ignore = new bool[h.Length];

            // compare each pair of haplogroups once
            for (int x = 0; x < h.Length; x++) {
                for (int y = x + 1; y < h.Length; y++) {
                    // if haplogroup is less specific, or haplogroups have same number of characters
                    if (h[x] == null || h[y] == null || h[x].Length == h[y].Length) {
                        // then cannot determine specificity
                        continue;
                    }

                    // if lengths differ, begin looking at characters to see if they belong to same branch
                    if (h[x].Length < h[y].Length) {
                        // set to false when a character doesn't match, indicating two different branches
                        bool morespecific = true;
                        // iterate the chars, looking for a mismatch
                        for (int i = 0; i < h[x].Length; i++) {
                            // if mismatch is found, then the longer haplogroup is not more specific
                            if (h[x][i] != h[y][i]) {
                                morespecific = false;
                            }
                        }

                        // if mismatch is not found, then the longer haplogroup is more specific
                        if (morespecific == true) {
                            // ignore the shorter haplogroup
                            h[x] = null;
                        }
                    } else {
                        // the second haplogroup is longer, compare chars and look for a mismatch
                        bool morespecific = true;
                        for (int i = 0; i < h[y].Length; i++) {
                            if (h[x][i] != h[y][i]) {
                                morespecific = false;
                            }

                        }
                        if (morespecific == true) {
                            h[y] = null;
                        }
                    }
                }
            }

            return foundHaplogroups;
        }

        public static void Compare(DNAData d1, DNAData d2, IDisplay display)
        {
            // alorithm counts chromosome length during file parsing and ensures there are the same number of compares
            // USED TO guarantee that chromosomes are of same length prior to comparison,

            // check that chromosomes are equilength
            for (int i = 0; i < 26; i++) {
                if (d1.ChromoPointers[i] != d2.ChromoPointers[i]) {
                    throw new Exception("Chromosomes not aligned at chromo " + (i + 1));
                }
            }

            // holds the scores of all 4 comparisons of each chromosome
            double[,] strandScores = new double[25, 4];

            // compare one strand to the other persons strands, looking for possibility of inheritance
            for (int c = 0; c < 25; c++) {
                // do 4 comparisions for each chromosome
                for (int comparisonIndex = 0; comparisonIndex < 4; comparisonIndex++) {
                    int match = 0, total = 0;

                    for (int i = 0; i < d1.ChromoPointers[c + 1] - d1.ChromoPointers[c]; i++) {
                        if (comparisonIndex == 0) {
                            // person 1 strand 1
                            if (d1.SNP[i + d1.ChromoPointers[c]].A1 == d2.SNP[i + d1.ChromoPointers[c]].A1 || d1.SNP[i + d1.ChromoPointers[c]].A1 == d2.SNP[i + d1.ChromoPointers[c]].A2) {
                                match++;
                            }
                            total++;
                        } else if (comparisonIndex == 1) {
                            // person 1 strand 2
                            if (d1.SNP[i + d1.ChromoPointers[c]].A2 == d2.SNP[i + d1.ChromoPointers[c]].A1 || d1.SNP[i + d1.ChromoPointers[c]].A2 == d2.SNP[i + d1.ChromoPointers[c]].A2) {
                                match++;
                            }
                            total++;
                        } else if (comparisonIndex == 2) {
                            // person 2 strand 1
                            if (d2.SNP[i + d1.ChromoPointers[c]].A1 == d1.SNP[i + d1.ChromoPointers[c]].A1 || d2.SNP[i + d1.ChromoPointers[c]].A1 == d1.SNP[i + d1.ChromoPointers[c]].A2) {
                                match++;
                            }
                            total++;
                        } else if (comparisonIndex == 3) {
                            // person 2 strand 2
                            if (d2.SNP[i + d1.ChromoPointers[c]].A2 == d1.SNP[i + d1.ChromoPointers[c]].A1 || d2.SNP[i + d1.ChromoPointers[c]].A2 == d1.SNP[i + d1.ChromoPointers[c]].A2) {
                                match++;
                            }
                            total++;
                        } else { throw new Exception("Invalid comparison pointer. Valid values are: 0, 1, 2, 3"); }

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
            correlation = correlation / ((parentScore[0] + parentScore[1]) / 2);

            // temp string for parent name
            string parent;
            // temp for child name
            string child;

            // 0-based pointer to person determined to be parent
            int pPointerToParent;

            // display the results
            if (parentScore[0] < parentScore[1]) {
                parent = "Person 1";
                child = "Person 2";
                pPointerToParent = 0;
            } else if (parentScore[0] > parentScore[1]) {
                parent = "Person 2";
                child = "Person 1";
                pPointerToParent = 1;
            } else {
                // scored the same
                throw new Exception("Best-fit for parent/child relation not found. Check that two unique individuals are being compared.");
            }

            // contains main diagnostic output before initializing output file
            string diagTextOut = "";

            // Display results
            diagTextOut += "      Parent / Child Relationship:\r\n  Parent:" + parent + "\r\n   Child:" + child + "\r\n";
            diagTextOut += "         Correlation (larger is better): " + correlation;
            diagTextOut += "\r\n              s1+t1  s1+t2  s2+t1  s2+t2";    // display chromosome scores
            for (int i = 0; i < 25; i++) {
                diagTextOut += "\r\nChromosome" + (i + 1) + ":";
                for (int j = 0; j < 4; j++) {
                    diagTextOut += Math.Round(strandScores[i, j], 4).ToString().PadLeft(7, ' ');

                }
                //diagTextOut += "\r\n";
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
            char[] missingParent;
            missingParent = new char[d1.ChromoPointers[d1.ChromoPointers.GetUpperBound(0)]];

            // copy child's non-matched strand to missing parent
            if (pPointerToParent == 1) {
                // person 2 is parent
                for (int i = 0; i < 23; i++) {
                    for (int j = 0; j < d1.ChromoPointers[i + 1] - d1.ChromoPointers[i]; j++) {
                        missingParent[j + d1.ChromoPointers[i]] = d1.SNP[j + d1.ChromoPointers[i]][pMissingParentChromo[i]];
                    }
                }
            } else {
                for (int i = 0; i < 23; i++) {
                    for (int j = 0; j < d1.ChromoPointers[i + 1] - d1.ChromoPointers[i]; j++) {
                        missingParent[j + d1.ChromoPointers[i]] = d2.SNP[j + d1.ChromoPointers[i]][pMissingParentChromo[i]];
                    }
                }
            }

            bool d1Female, d2Female;

            // Determine sex of parent and child, then output sex chromosomes
            d1Female = Analytics.IsFemale(d1);
            d2Female = Analytics.IsFemale(d2);

            if (pPointerToParent == 1) {
                // person 2 is parent
                if (d2Female) {
                    // parent is female; give missing parent X and look for Y
                    if (d1Female) {
                        // both parent and child are female, NO KNOWN Y chromosome
                        diagTextOut = "Exporting Missing Parent: No known Y chromosome exists. Defaulting to '0'.";
                        // copy child's Y to missing parent
                        for (int i = d1.ChromoPointers[23]; i < d1.ChromoPointers[24]; i++) {
                            missingParent[i] = '0';
                        }
                    } else {
                        // person 2 is female parent, but Y exists in P1
                        // copy child's Y to missing parent
                        for (int i = d1.ChromoPointers[23]; i < d1.ChromoPointers[24]; i++) {
                            missingParent[i] = d1.SNP[i][pMissingParentChromo[i]];
                        }
                    }
                } else {
                    // person 2 is male parent; give missing parent X & no Y
                    if (d1Female) {
                        // child is female; give child's chromosome 24
                        for (int i = d1.ChromoPointers[23]; i < d1.ChromoPointers[24]; i++) {
                            missingParent[i] = d1.SNP[i][pMissingParentChromo[i]];
                        }
                    } else {
                        // both parent and child are male, NO KNOWN !Y chromosome
                        diagTextOut = "Exporting Missing Parent: No known !Y chromosome exists. Defaulting to '0'.";
                        for (int i = d1.ChromoPointers[23]; i < d1.ChromoPointers[24]; i++) {
                            missingParent[i] = '0';
                        }
                    }
                }
            } else {
                // Person 1 is parent
                if (d1Female) {
                    // parent is female; look for Y
                    // both parent and child are female, NO KNOWN Y chromosome
                    if (d2Female) {
                        diagTextOut = "Exporting Missing Parent: No known Y chromosome exists. Defaulting to '0'.";
                        // copy child's Y to missing parent
                        for (int i = d1.ChromoPointers[23]; i < d1.ChromoPointers[24]; i++) {
                            missingParent[i] = '0';
                        }
                    } else {
                        // person 1 is female parent, but Y exists in P2
                        // copy child's Y to missing parent
                        for (int i = d1.ChromoPointers[23]; i < d1.ChromoPointers[24]; i++) {
                            missingParent[i] = d2.SNP[i][pMissingParentChromo[23]];
                        }
                    }
                } else {
                    // person 2 is male parent; give missing parent X & no Y
                    if (d2Female) {
                        // child is female; give child's chromosome 24
                        for (int i = d1.ChromoPointers[23]; i < d1.ChromoPointers[24]; i++) {
                            missingParent[i] = d2.SNP[i][pMissingParentChromo[23]];
                        }
                    } else {
                        // both parent and child are male, NO KNOWN !Y chromosome
                        diagTextOut = "Exporting Missing Parent: No known !Y chromosome exists. Defaulting to '0'.";
                        for (int i = d1.ChromoPointers[23]; i < d1.ChromoPointers[24]; i++) {
                            missingParent[i] = '0';
                        }
                    }
                }
            }

            display.WriteLine(diagTextOut);

            // copy mitochondrial dna
            if (pPointerToParent == 1) {
                // person 2 is parent
                for (int i = d1.ChromoPointers[24]; i < d1.ChromoPointers[25]; i++) {
                    missingParent[i] = d1.SNP[i][pMissingParentChromo[24]];
                }
            } else {
                // person 1 is parent
                for (int i = d1.ChromoPointers[24]; i < d1.ChromoPointers[25]; i++) {
                    missingParent[i] = d2.SNP[i][pMissingParentChromo[25]];
                }
            }

            /*display.WriteLine("Output the best guess for missing parent:");
            for (int i = 0; i <= missingParent.GetUpperBound(0); i++) {
                display.WriteLine(missingParent[i].ToString());
            }*/
        }
    }
}

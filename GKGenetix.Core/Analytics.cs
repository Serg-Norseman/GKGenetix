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

using System.Collections.Generic;

namespace GKGenetix.Core
{
    /// <summary>
    /// Methods for data analysis.
    /// 
    /// Created based on the Isaac Styles code from the DNAinheritanceTest project.
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
    }
}

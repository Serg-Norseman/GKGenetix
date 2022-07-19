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
    public sealed class DNAData
    {
        public string PersonalName { get; set; }

        public IList<SNP> SNP { get; private set; }

        /// <summary>
        /// Pointers to beginning of each chromosome.
        /// </summary>
        public int[] ChromoPointers { get; private set; }

        public GeneticSex Sex { get; private set; }

        public bool IsFemale
        {
            get {
                return (Sex == GeneticSex.Female);
            }
        }

        public DNAData()
        {
            SNP = new List<SNP>(702000);
            ChromoPointers = new int[26];
            Sex = GeneticSex.Unknown;
        }

        /// <summary>
        /// Determines whether the given DNA is female.
        /// </summary>
        public void DetermineSex()
        {
            int count = 0;
            int total = 0;

            for (int i = ChromoPointers[23]; i < ChromoPointers[24]; i++) {
                // chromosome 24 is male Y
                if (SNP[i].Genotype.A1 == '0') {
                    count++;
                }
                total++;
            }

            // if the majority of Y were 0
            Sex = (count / (double)total > 0.9d) ? GeneticSex.Female : GeneticSex.Male;
        }
    }
}

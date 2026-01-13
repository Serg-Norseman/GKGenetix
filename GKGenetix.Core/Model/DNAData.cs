/*
 *  GKGenetix, the simple DNA analysis kit.
 *  Copyright (C) 2022-2026 by Sergey V. Zhdanovskih.
 *
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System.Collections.Generic;

namespace GKGenetix.Core.Model
{
    public enum GeneticSex
    {
        Unknown,
        Female,
        Male
    }


    public sealed class DNAData
    {
        public string FileName { get; set; }

        public string PersonalName { get; set; }

        /// <summary>
        /// Reference human assembly.
        /// </summary>
        public int RHABuild { get; set; }

        public IList<SNP> SNP { get; private set; } // 0, atdna
        public List<string> ydna { get; set; } // 1
        public List<string> mtdna { get; set; } // 2

        /// <summary>
        /// Pointers to beginning of each chromosome.
        /// </summary>
        public IList<Region> Chromosomes { get; private set; }

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
            Chromosomes = new List<Region>(26);
            Sex = GeneticSex.Unknown;
        }

        public DNAData(List<SNP> atdna, List<string> ydna, List<string> mtdna)
        {
            this.SNP = atdna;
            this.ydna = ydna;
            this.mtdna = mtdna;
        }

        /// <summary>
        /// Determines whether the given DNA is female.
        /// </summary>
        public void DetermineSex()
        {
            int count = 0;
            int total = 0;

            // chromosome 24 is male Y
            var chrY = Chromosomes[23];
            for (int i = chrY.StartPosition; i <= chrY.EndPosition; i++) {
                if (SNP[i].Genotype.A1 == Genotype.UnknownAllele) {
                    count++;
                }
                total++;
            }

            // if the majority of Y were 0
            Sex = (count / (double)total > 0.9d) ? GeneticSex.Female : GeneticSex.Male;
        }
    }
}

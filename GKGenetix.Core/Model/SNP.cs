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

using System.Collections.Generic;
using System.Text;

namespace GKGenetix.Core.Model
{
    /// <summary>
    /// Single-nucleotide polymorphism (SNP).
    /// Substitution of a single nucleotide at a specific position in the genome.
    /// </summary>
    public sealed class SNP : IDataRecord
    {
        /// <summary>
        /// The rsID number ("rs#"; "refSNP cluster") is a unique label ("rs" followed by a number) used by researchers and databases to identify a specific SNP.
        /// It stands for Reference SNP cluster ID and is the naming convention used for most SNPs.
        /// Alleles in genotype oriented with respect to the plus strand on the human reference sequence.
        /// </summary>
        public string rsID { get; set; }

        /// <summary>
        /// Chromosome.
        /// </summary>
        public byte Chromosome { get; set; }

        /// <summary>
        /// Basepair position.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Genotype. Allele1 and Allele2.
        /// </summary>
        public Genotype Genotype { get; set; }

        /// <summary>
        /// Logical centiMorgan location of SNP in the chromosome.
        /// </summary>
        public float cM { get; set; }


        public string ChrStr { get { return Chromosome.ToString(); } set { Chromosome = (byte)value.ParseChromosome(); } }
        public string GtStr { get { return Genotype.ToString(); } set { Genotype = new Genotype(value); } }


        public SNP()
        {
        }

        public SNP(string rsid, string chromosome, int position, string genotype)
        {
            rsID = rsid;
            Chromosome = (byte)chromosome.ParseChromosome();
            Position = position;
            Genotype = new Genotype(genotype);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(rsID);
            sb.Append(" ");
            sb.Append(Chromosome);
            sb.Append("@");
            sb.Append(Position);
            sb.Append(" ");
            sb.Append(Genotype.ToString());
            return sb.ToString();
        }
    }

    public class SNPComparer : IComparer<SNP>
    {
        public int Compare(SNP x, SNP y)
        {
            int result = x.Chromosome.CompareTo(y.Chromosome);

            if (result == 0) {
                result = x.Position.CompareTo(y.Position);

                if (result == 0) {
                    result = x.rsID.CompareTo(y.rsID);
                }
            }

            return result;
        }
    }
}

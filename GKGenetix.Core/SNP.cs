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
using System.Text;

namespace GKGenetix.Core
{
    /// <summary>
    /// Single-nucleotide polymorphism (SNP).
    /// Substitution of a single nucleotide at a specific position in the genome.
    /// </summary>
    public sealed class SNP
    {
        /// <summary>
        /// The rsID number ("rs#"; "refSNP cluster") is a unique label ("rs" followed by a number) used by researchers and databases to identify a specific SNP.
        /// It stands for Reference SNP cluster ID and is the naming convention used for most SNPs.
        /// Alleles in genotype oriented with respect to the plus strand on the human reference sequence.
        /// </summary>
        public string rsID;

        /// <summary>
        /// Chromosome.
        /// </summary>
        public byte Chr;

        /// <summary>
        /// Basepair position.
        /// </summary>
        public int Pos;

        /// <summary>
        /// Genotype. Allele1 and Allele2.
        /// </summary>
        public SNPGenotype Genotype;

        /// <summary>
        /// Logical centiMorgan location of SNP in the chromosome.
        /// </summary>
        public float cM;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(rsID);
            sb.Append(" ");
            sb.Append(Chr);
            sb.Append("@");
            sb.Append(Pos);
            sb.Append(" ");
            sb.Append(Genotype.ToString());
            return sb.ToString();
        }
    }

    public class SNPComparer : IComparer<SNP>
    {
        public int Compare(SNP x, SNP y)
        {
            int result = x.Chr.CompareTo(y.Chr);

            if (result == 0) {
                result = x.Pos.CompareTo(y.Pos);

                if (result == 0) {
                    result = x.rsID.CompareTo(y.rsID);
                }
            }

            return result;
        }
    }
}

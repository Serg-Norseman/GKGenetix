/*
 *  GKGenetix, the simple DNA analysis kit.
 *  Copyright (C) 2022-2026 by Sergey V. Zhdanovskih.
 *
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System.Collections.Generic;
using System.Text;
using GKGenetix.Core.Database;

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

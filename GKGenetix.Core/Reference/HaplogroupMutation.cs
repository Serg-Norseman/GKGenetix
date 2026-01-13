/*
 *  GKGenetix, the simple DNA analysis kit.
 *  Copyright (C) 2022-2026 by Sergey V. Zhdanovskih.
 *
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

namespace GKGenetix.Core.Reference
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class HaplogroupMutation
    {
        public string Haplogroup { get; set; }

        public string SNP { get; set; }

        /// <summary>
        /// The rsID number ("rs#"; "refSNP cluster") is a unique label ("rs" followed by a number) used by researchers and databases to identify a specific SNP.
        /// It stands for Reference SNP cluster ID and is the naming convention used for most SNPs.
        /// </summary>
        public string rsID { get; set; }

        /// <summary>
        /// Basepair position.
        /// </summary>
        public int Position { get; set; }

        public char OldNucleotide { get; set; }

        public char NewNucleotide { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: '{1}', {2}, {3}->{4}", Haplogroup, SNP, Position, OldNucleotide, NewNucleotide);
        }
    }
}

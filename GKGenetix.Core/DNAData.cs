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
    /// Single-nucleotide polymorphism (SNP).
    /// Substitution of a single nucleotide at a specific position in the genome.
    /// </summary>
    public struct SNP
    {
        /// <summary>
        /// The rsID number ("rs#"; "refSNP cluster") is a unique label ("rs" followed by a number) used by researchers and databases to identify a specific SNP.
        /// It stands for Reference SNP cluster ID and is the naming convention used for most SNPs.
        /// </summary>
        public string rsID;

        /// <summary>
        /// Chromosome.
        /// </summary>
        public byte Chr;

        /// <summary>
        /// Basepair position.
        /// </summary>
        public uint Pos;

        /// <summary>
        /// Allele1.
        /// </summary>
        public char A1;

        /// <summary>
        /// Allele2.
        /// </summary>
        public char A2;

        public char this[int index]
        {
            get {
                switch (index) {
                    case 1:
                        return A1;
                    case 2:
                        return A2;
                    default:
                        return '0';
                }
            }
        }
    }


    public sealed class DNAData
    {
        public IList<SNP> SNP { get; private set; }

        /// <summary>
        /// Pointers to beginning of each chromosome.
        /// </summary>
        public int[] ChromoPointers { get; private set; }

        public DNAData()
        {
            SNP = new List<SNP>(702000);
            ChromoPointers = new int[26];
        }
    }


    public struct Haplotype
    {
        public string Group;

        /// <summary>
        /// The rsID number ("rs#"; "refSNP cluster") is a unique label ("rs" followed by a number) used by researchers and databases to identify a specific SNP.
        /// It stands for Reference SNP cluster ID and is the naming convention used for most SNPs.
        /// </summary>
        public string rsID;

        /// <summary>
        /// Basepair position.
        /// </summary>
        public uint Pos;

        public char Mutation;
    }
}

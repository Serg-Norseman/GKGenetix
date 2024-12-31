/*
 *  "GEDKeeper", the personal genealogical database editor.
 *  Copyright (C) 2009-2024 by Sergey V. Zhdanovskih.
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

namespace GKGenetix.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class Genotype : IEquatable<Genotype>
    {
        public const char UnknownAllele = '0';

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
                        return UnknownAllele;
                }
            }
        }

        public Genotype(char a1, char a2)
        {
            A1 = a1;
            A2 = a2;
        }

        public Genotype(string field)
        {
            A1 = '0';
            A2 = '0';
            if (!string.IsNullOrEmpty(field)) {
                A1 = field[0];
                if (field.Length > 1) {
                    A2 = field[1];
                }
            }
        }

        public bool Equals(Genotype other)
        {
            return (A1 == other.A1 && A2 == other.A2) ||
                   (A1 == other.A2 && A2 == other.A1);
        }

        public int CompareStrand(Genotype other, int strand)
        {
            if (strand == 1) {
                return (A1 == other.A1 || A1 == other.A2) ? 1 : 0;
            } else if (strand == 2) {
                return (A2 == other.A1 || A2 == other.A2) ? 1 : 0;
            } else {
                return 0;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", A1, A2);
        }

        public Genotype GetComplement()
        {
            return new Genotype(GeneLab.GetComplementaryNucleotide(A1), GeneLab.GetComplementaryNucleotide(A2));
        }

        public static Nucleotide ParseNucleotide(char a)
        {
            Nucleotide result;
            switch (a) {
                case 'A':
                    result = Nucleotide.A;
                    break;
                case 'C':
                    result = Nucleotide.C;
                    break;
                case 'G':
                    result = Nucleotide.G;
                    break;
                case 'T':
                    result = Nucleotide.T;
                    break;
                case 'U':
                    result = Nucleotide.U;
                    break;
                case 'N':
                default:
                    result = Nucleotide.None;
                    break;
            }
            return result;
        }
    }
}

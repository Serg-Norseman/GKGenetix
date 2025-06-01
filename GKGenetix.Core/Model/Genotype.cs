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

using System;
using System.Runtime.InteropServices;

namespace GKGenetix.Core.Model
{
    public enum Orientation : ushort
    {
        Unknown,
        Plus,
        Minus
    }


    /// <summary>
    /// 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Genotype : IEquatable<Genotype>
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

        /// <summary>
        /// Strand orientation.
        /// </summary>
        public Orientation Orientation;


        public Genotype(char a1, char a2, Orientation orientation = Orientation.Unknown)
        {
            A1 = a1;
            A2 = a2;
            Orientation = orientation;
        }

        public Genotype(string field, Orientation orientation = Orientation.Unknown)
        {
            A1 = '0';
            A2 = '0';
            if (!string.IsNullOrEmpty(field)) {
                A1 = field[0];
                if (field.Length > 1) {
                    A2 = field[1];
                }
            }
            Orientation = orientation;
        }

        public static bool IsEmptyOrUnknown(char a)
        {
            return /*a == UnknownAllele ||*/ a == '-' || a == '?';
        }

        public bool IsEmptyOrUnknown()
        {
            return (A1 == UnknownAllele || A1 == '-' || A1 == '?') || (A2 == UnknownAllele || A2 == '-' || A2 == '?');
        }

        public void CheckCompleteness()
        {
            // questionable decision?
            if (A2 == UnknownAllele || A2 == '-' || A2 == '?') {
                A2 = A1;
            }
        }

        public Genotype Reverse()
        {
            return new Genotype(A2, A1);
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

        public Genotype GetComplement(Orientation orientation)
        {
            return new Genotype(GeneLab.GetComplementaryNucleotide(A1), GeneLab.GetComplementaryNucleotide(A2), orientation);
        }

        /// <summary>
        /// Get the genotype oriented for a given strand.
        /// </summary>
        public Genotype GetOrientedGenotype(Orientation targetOrientation)
        {
            if (targetOrientation == Orientation.Unknown || Orientation == Orientation.Unknown) {
                return new Genotype(UnknownAllele, UnknownAllele, Orientation.Unknown);
            } else if (Orientation == targetOrientation) {
                return this;
            } else {
                return GetComplement(targetOrientation);
            }
        }
    }
}

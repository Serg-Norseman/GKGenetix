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
    /// 23andMe: alleles can be "--" and 1 char for X/Y/MT positions.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Genotype : IEquatable<Genotype>
    {
        public static readonly Genotype Empty = new Genotype(UnknownAllele, UnknownAllele);

        public const char UnknownAllele = '0';

        /// <summary>
        /// Allele 1.
        /// </summary>
        public char A1 { get; private set; }

        /// <summary>
        /// Allele 2.
        /// </summary>
        public char A2 { get; private set; }

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
            A1 = CheckAllele(a1);
            A2 = CheckAllele(a2);
            Orientation = orientation;
        }

        public Genotype(string field, Orientation orientation = Orientation.Unknown)
        {
            A1 = UnknownAllele;
            A2 = UnknownAllele;
            if (!string.IsNullOrEmpty(field)) {
                A1 = CheckAllele(field[0]);
                if (field.Length > 1) {
                    A2 = CheckAllele(field[1]);
                }
            }
            Orientation = orientation;
        }

        private static char CheckAllele(char value)
        {
            return (value == '-' || value == '?') ? UnknownAllele : value;
        }

        public void Clear()
        {
            A1 = UnknownAllele;
            A2 = UnknownAllele;
        }

        public bool IsHomozygous()
        {
            return (A1 == A2 && A1 != UnknownAllele);
        }

        public static bool IsEmptyOrUnknown(char a)
        {
            return a == UnknownAllele;
        }

        public bool IsEmptyOrUnknown()
        {
            return (A1 == UnknownAllele || A2 == UnknownAllele);
        }

        public bool IsFullEmpty()
        {
            return (A1 == UnknownAllele && A2 == UnknownAllele);
        }

        public bool Contains(char a)
        {
            return (A1 == a) || (A2 == a);
        }

        public bool ContainsAny(Genotype gt)
        {
            return (A1 == gt.A1 || A1 == gt.A2 || A2 == gt.A1 || A2 == gt.A2);
        }

        public char GetOther(char al)
        {
            if (A1 == al && A2 == al) {
                return UnknownAllele;
            } else if (A1 == al) {
                return A2;
            } else if (A2 == al) {
                return A1;
            } else {
                return UnknownAllele;
            }
        }

        public void CheckCompleteness()
        {
            // questionable decision?
            if (A1 == UnknownAllele) {
                A1 = A2;
            } else if (A2 == UnknownAllele) {
                A2 = A1;
            }
        }

        public Genotype Reverse()
        {
            return new Genotype(A2, A1);
        }

        public static bool operator ==(Genotype gt, Genotype other)
        {
            return (gt.A1 == other.A1 && gt.A2 == other.A2);
        }

        public static bool operator !=(Genotype gt, Genotype other)
        {
            return (gt.A1 != other.A1 && gt.A2 != other.A2);
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

                case '.': // only [VCF] gap?, ??
                case 'N': // only [VCF] any one base?, ??
                case '0': // only [AncestryDNA] "not determined", always "00" (checked!)
                case '-': // only [23andMe] "not determined", always "--" (checked!)
                case '?': // ? [?] "missing or unknown genotypes", "??"
                    result = Nucleotide.NoCall;
                    break;

                default:
                    result = Nucleotide.None;
                    break;
            }
            return result;
        }

        public Genotype GetComplement()
        {
            return new Genotype(GeneLab.GetComplementaryNucleotide(A1), GeneLab.GetComplementaryNucleotide(A2));
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

        /// <summary>
        /// IUPAC symbols. See: https://en.wikipedia.org/wiki/Nucleic_acid_notation
        /// </summary>
        public char GetNucleotideCode()
        {
            /*
                R = A/G (Purine)
                Y = C/T (Pyrimidine)
                S = G/C (Strong)
                W = A/T (Weak)
                K = G/T (Ketone)
                M = A/C (Amino)
                N = (Any one base)
                - = (Gap)
             */

            char bp1 = A1;
            char bp2 = A2;

            if ((bp1 == 'A' && bp2 == 'G') || (bp1 == 'G' && bp2 == 'A'))
                return 'R';
            else if ((bp1 == 'C' && bp2 == 'T') || (bp1 == 'T' && bp2 == 'C'))
                return 'Y';
            else if ((bp1 == 'C' && bp2 == 'G') || (bp1 == 'G' && bp2 == 'C'))
                return 'S';
            else if ((bp1 == 'A' && bp2 == 'T') || (bp1 == 'T' && bp2 == 'A'))
                return 'W';
            else if ((bp1 == 'T' && bp2 == 'G') || (bp1 == 'G' && bp2 == 'T'))
                return 'K';
            else if ((bp1 == 'A' && bp2 == 'C') || (bp1 == 'C' && bp2 == 'A'))
                return 'M';
            else
                return 'N';
        }

        public override bool Equals(object o)
        {
            if (o is Genotype other) {
                return (A1 == other.A1 && A2 == other.A2) ||
                       (A1 == other.A2 && A2 == other.A1);
            } else return false;
        }

        public override int GetHashCode()
        {
            int hashCode = -1506897950;
            hashCode = hashCode * -1521134295 + A1.GetHashCode();
            hashCode = hashCode * -1521134295 + A2.GetHashCode();
            hashCode = hashCode * -1521134295 + Orientation.GetHashCode();
            return hashCode;
        }
    }
}

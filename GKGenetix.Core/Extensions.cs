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
    public static class Extensions
    {
        public static string ToString(this Chromosome chromosome)
        {
            switch (chromosome) {
                case Chromosome.CHR_01:
                case Chromosome.CHR_02:
                case Chromosome.CHR_03:
                case Chromosome.CHR_04:
                case Chromosome.CHR_05:
                case Chromosome.CHR_06:
                case Chromosome.CHR_07:
                case Chromosome.CHR_08:
                case Chromosome.CHR_09:
                case Chromosome.CHR_10:
                case Chromosome.CHR_11:
                case Chromosome.CHR_12:
                case Chromosome.CHR_13:
                case Chromosome.CHR_14:
                case Chromosome.CHR_15:
                case Chromosome.CHR_16:
                case Chromosome.CHR_17:
                case Chromosome.CHR_18:
                case Chromosome.CHR_19:
                case Chromosome.CHR_20:
                case Chromosome.CHR_21:
                case Chromosome.CHR_22:
                    return ((int)chromosome).ToString();
                case Chromosome.CHR_X:
                    return "X";
                case Chromosome.CHR_Y:
                    return "Y";
                case Chromosome.MT:
                    return "MT";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static Chromosome ParseChromosome(this string s)
        {
            switch (s) {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                case "10":
                case "11":
                case "12":
                case "13":
                case "14":
                case "15":
                case "16":
                case "17":
                case "18":
                case "19":
                case "20":
                case "21":
                case "22":
                    int chrNum = int.Parse(s);
                    return (Chromosome)chrNum;

                case "chr1": // [VCF]
                case "chr2": // [VCF]
                case "chr3": // [VCF]
                case "chr4": // [VCF]
                case "chr5": // [VCF]
                case "chr6": // [VCF]
                case "chr7": // [VCF]
                case "chr8": // [VCF]
                case "chr9": // [VCF]
                case "chr10": // [VCF]
                case "chr11": // [VCF]
                case "chr12": // [VCF]
                case "chr13": // [VCF]
                case "chr14": // [VCF]
                case "chr15": // [VCF]
                case "chr16": // [VCF]
                case "chr17": // [VCF]
                case "chr18": // [VCF]
                case "chr19": // [VCF]
                case "chr20": // [VCF]
                case "chr21": // [VCF]
                case "chr22": // [VCF]
                    int chrNumVCF = int.Parse(s.Substring(3));
                    return (Chromosome)chrNumVCF;

                case "23": // [AncestryDNA]
                case "X": // [23AndMe, deCODEme, VCF]
                case "chrX": // [VCF]
                    return Chromosome.CHR_X;

                case "24": // Non-pseudoautosomal portion of the Y chromosome [AncestryDNA]
                case "Y": // [23AndMe, deCODEme, VCF]
                case "chrY": // [VCF]
                    return Chromosome.CHR_Y;

                case "25": // Pseudoautosomal portion of the Y chromosome [AncestryDNA]
                    return Chromosome.CHR_Yp;

                case "26": // Mitochondrial DNA [AncestryDNA]
                case "MT": // [23AndMe]
                case "M": // [deCODEme]
                case "chrM": // [VCF]
                    return Chromosome.MT;

                default:
                    throw new ParseException("Unknown chromosome '{0}'.", s);
            }
        }

        public static Orientation ParseOrientation(this string s)
        {
            s = s.ToLowerInvariant();

            if (s == "-" || s == "minus") {
                return Orientation.Minus;
            } else if (s == "+" || s == "plus") {
                return Orientation.Plus;
            } else {
                return Orientation.Unknown;
            }
        }

        public static int ParsePosition(this string s)
        {
            int x;
            return !int.TryParse(s.Trim(), out x) ? -1 : x;
        }

        public static char[] ParseMutation(string mutation)
        {
            var parts = mutation.Split(new[] { "->" }, StringSplitOptions.RemoveEmptyEntries);

            var nucleotides = new char[parts.Length];
            for (int i = 0; i < parts.Length; i++) {
                string part = parts[i].Trim();
                nucleotides[i] = (part.Length == 1) ? part[0] : '\0';
            }

            return (nucleotides.Length == 2) ? nucleotides : new char[] { '\0', '\0' };
        }
    }
}

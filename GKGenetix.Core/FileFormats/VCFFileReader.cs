﻿/*
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

using System.IO;
using GKGenetix.Core.Model;

namespace GKGenetix.Core.FileFormats
{
    /// <summary>
    /// Variant Call Format (VCF).
    /// </summary>
    public sealed class VCFFileReader : SNPFileReader
    {
        public VCFFileReader(StreamReader reader) : base(reader)
        {
        }

        protected override void SetParsingParameters()
        {
            SetParsingParameters('#', '\t');
        }

        protected override void ProcessHeaderLine(string line, DNAData data)
        {
        }

        protected override SNP ProcessDataLine(string[] fields)
        {
            // 8 mandatory columns: CHROM [0+], POS [1+], ID [2+], REF [3], ALT [4], QUAL [5], FILTER [6], INFO [7]
            // optional columns: FORMAT [8] and other

            string positionText = fields[1];
            if (!int.TryParse(positionText, out int position)) {
                throw new ParseException("Error in VCF raw file. Invalid position '{0}'.", positionText);
            }

            var snp = new SNP();
            snp.rsID = fields[2];
            snp.Chromosome = (byte)fields[0].ParseChromosome();
            snp.Position = position;

            // FIXME: bad, modify later
            string refGenotype = fields[3];
            //string altGenotype = fields[4];

            snp.Genotype = new Genotype(refGenotype, Orientation.Unknown);

            double quality = 0;
            if (fields[5] == ".") {
                quality = double.NaN;
            } else {
                if (!double.TryParse(fields[5], out quality))
                    throw new ParseException("Error in VCF raw file. Invalid quality '{0}'.", fields[5]);
            }

            //string filter = fields[6];

            return snp;
        }

        private Nucleotide[] ParseBase(string field)
        {
            var result = new Nucleotide[field.Length];
            for (int i = 0; i < field.Length; i++) {
                result[i] = Genotype.ParseNucleotide(field[i]);
            }
            return result;
        }
    }
}

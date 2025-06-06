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
    public sealed class SNPdeCODEmeFileReader : SNPFileReader
    {
        public SNPdeCODEmeFileReader(StreamReader reader) : base(reader)
        {
        }

        protected override void SetParsingParameters()
        {
            // TODO: header mark is not yet known
            SetParsingParameters('#', ',');
        }

        protected override void ProcessHeaderLine(string line, DNAData data)
        {
        }

        protected override SNP ProcessDataLine(string[] fields)
        {
            // deCODEme column headers line; starts with "Name"
            if (fields[0] == "Name")
                return null;

            string positionText = fields[3];
            int position = positionText.ParsePosition();
            if (position == -1)
                throw new ParseException("Error in deCODEme raw file. Invalid position '{0}'.", positionText);

            var snp = new SNP();
            snp.rsID = fields[0];
            // Variation = fields[1];
            snp.Chromosome = (byte)fields[2].ParseChromosome();
            snp.Position = position;
            var orientation = fields[4].ParseOrientation();
            snp.Genotype = new Genotype(fields[5], orientation);
            return snp;
        }
    }
}

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

using System.IO;
using GKGenetix.Core.Model;

namespace GKGenetix.Core.FileFormats
{
    public sealed class SNPAncestryDNAFileReader : SNPFileReader
    {
        public SNPAncestryDNAFileReader(string fileName) : base(fileName)
        {
        }

        public SNPAncestryDNAFileReader(Stream stream) : base(stream)
        {
        }

        public SNPAncestryDNAFileReader(StreamReader reader) : base(reader)
        {
        }

        protected override void SetParsingParameters()
        {
            SetParsingParameters('#', '\t');
        }

        protected override void ProcessHeaderLine(string line, DNAData data)
        {
            if (line.Contains("build 37")) {
                data.RHABuild = 37;
            }
        }

        protected override SNP ProcessDataLine(string[] fields)
        {
            // Data validation: if line begins with 'r' then is most likely a SNP
            // AncestryDNA column headers line; starts with "rsid"
            if (fields[0] == "rsid")
                return null;

            // AncestryDNA: chromosome numbers from 1 to 25!

            string positionText = fields[2];
            int position = positionText.ParsePosition();
            if (position == -1)
                throw new ParseException("Error in AncestryDNA raw file. Invalid position '{0}'.", positionText);

            var snp = new SNP();
            snp.rsID = fields[0];
            snp.Chromosome = (byte)fields[1].ParseChromosome();
            snp.Position = position;
            snp.Genotype = new Genotype(fields[3][0], fields[4][0], Orientation.Plus);
            return snp;
        }
    }
}

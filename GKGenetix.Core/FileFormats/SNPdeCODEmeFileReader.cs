/*
 *  GKGenetix, the simple DNA analysis kit.
 *  Copyright (C) 2022-2026 by Sergey V. Zhdanovskih.
 *
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
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

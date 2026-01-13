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
    public sealed class SNPFTDNAFileReader : SNPFileReader
    {
        public SNPFTDNAFileReader(StreamReader reader) : base(reader)
        {
        }

        protected override void SetParsingParameters()
        {
            SetParsingParameters('#', ',');
        }

        protected override SNP ProcessDataLine(string[] fields)
        {
            // FTDNA column headers line; starts with "RSID"
            // there may be quotes in cells!
            if (fields[0] == "RSID")
                return null;

            string positionText = fields[2];
            int position = positionText.ParsePosition();

            string genotypeText = fields[3];

            var snp = new SNP();
            snp.rsID = fields[0];
            snp.Chromosome = (byte)fields[1].ParseChromosome();
            snp.Position = position;
            snp.Genotype = new Genotype(genotypeText);
            return snp;
        }
    }
}

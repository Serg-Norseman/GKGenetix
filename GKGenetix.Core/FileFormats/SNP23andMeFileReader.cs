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
    public sealed class SNP23andMeFileReader : SNPFileReader
    {
        public SNP23andMeFileReader(StreamReader reader) : base(reader)
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
            // 23andMe: chromosome numbers from 1..22 to X, Y, MT

            string positionText = fields[2];
            int position = positionText.ParsePosition();
            if (position == -1)
                throw new ParseException("Error in 23andMe raw file. Invalid position '{0}'.", positionText);

            string genotypeText = fields[3];
            if (genotypeText.Length > 2)
                throw new ParseException("Error in 23andMe raw file. Invalid genotype '{0}'.", genotypeText);

            var snp = new SNP();
            snp.rsID = fields[0];
            snp.Chromosome = (byte)fields[1].ParseChromosome();
            snp.Position = position;
            snp.Genotype = new Genotype(genotypeText, Orientation.Plus); // 23AndMe: CC...--
            return snp;
        }
    }
}

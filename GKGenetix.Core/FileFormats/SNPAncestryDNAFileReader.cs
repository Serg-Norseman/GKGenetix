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
    public sealed class SNPAncestryDNAFileReader : SNPFileReader
    {
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
            // Alleles: can be 0!

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

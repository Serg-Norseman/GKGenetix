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
    public sealed class SNPGeno2FileReader : SNPFileReader
    {
        public SNPGeno2FileReader(StreamReader reader) : base(reader)
        {
        }

        protected override void SetParsingParameters()
        {
            // TODO: header mark is not yet known
            SetParsingParameters('#', ';');
        }

        protected override void ProcessHeaderLine(string line, DNAData data)
        {
        }

        protected override SNP ProcessDataLine(string[] fields)
        {
            // SNP;Chr;Allele1;Allele2
            // Geno2 column headers line; starts with "SNP"
            if (fields[0] == "SNP")
                return null;

            int position = 0; // position is missing

            var snp = new SNP();
            snp.rsID = fields[0];
            snp.Chromosome = (byte)fields[1].ParseChromosome();
            snp.Position = position;
            snp.Genotype = new Genotype(fields[2][0], fields[3][0]); // may contain insertions and deletions (I, D)
            return snp;
        }
    }
}

/*
 *  GKGenetix, the simple DNA analysis kit.
 *  Copyright (C) 2022-2026 by Sergey V. Zhdanovskih.
 *
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using GKGenetix.Core.Database;

namespace GKGenetix.Core.Model
{
    public class SNPMatch : IDataRecord
    {
        public string rsID { get; set; }
        public byte Chromosome { get; set; }
        public int Position { get; set; }
        public Genotype Genotype1 { get; set; }
        public Genotype Genotype2 { get; set; }
        public string Match { get; set; }


        public string ChrStr { get { return Chromosome.ToString(); } set { Chromosome = (byte)value.ParseChromosome(); } }
        public string Gt1Str { get { return Genotype1.ToString(); } set { Genotype1 = new Genotype(value); } }
        public string Gt2Str { get { return Genotype2.ToString(); } set { Genotype2 = new Genotype(value); } }


        public SNPMatch()
        {
        }

        public SNPMatch(string rsid, byte chromosome, int position, Genotype genotype1, Genotype genotype2, string match)
        {
            rsID = rsid;
            Chromosome = chromosome;
            Position = position;
            Genotype1 = genotype1;
            Genotype2 = genotype2;
            Match = match;
        }
    }
}

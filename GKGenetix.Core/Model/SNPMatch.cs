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

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

using System.Collections.Generic;

namespace GKGenetix.Core.Model
{
    public class CmpSegment : ISNPSegment, IDataRecord
    {
        public int SegmentId { get; private set; }
        public byte Chromosome { get; private set; }
        public int StartPosition { get; private set; }
        public int EndPosition { get; private set; }
        public double SegmentLength_cm { get; private set; }
        public int SNPCount { get; private set; }


        public string ChrStr { get { return Chromosome.ToString(); } set { Chromosome = (byte)value.ParseChromosome(); } }


        public IList<CmpSegmentRow> Rows { get; set; }


        public CmpSegment()
        {
        }

        public CmpSegment(byte chromosome, int startPosition, int endPosition, double segmentLength_cm, IList<CmpSegmentRow> rows)
        {
            Chromosome = chromosome;
            StartPosition = startPosition;
            EndPosition = endPosition;
            SegmentLength_cm = segmentLength_cm;
            SNPCount = rows.Count;
            Rows = rows;
        }
    }


    public class CmpSegmentRow : ISNPHeader, IDataRecord
    {
        public string rsID { get; private set; }
        public byte Chromosome { get; private set; }
        public int Position { get; private set; }
        public Genotype Genotype1 { get; private set; }
        public Genotype Genotype2 { get; private set; }
        public string Match { get; private set; }


        public string ChrStr { get { return Chromosome.ToString(); } set { Chromosome = (byte)value.ParseChromosome(); } }
        public string Gt1Str { get { return Genotype1.ToString(); } set { Genotype1 = new Genotype(value); } }
        public string Gt2Str { get { return Genotype2.ToString(); } set { Genotype2 = new Genotype(value); } }


        public CmpSegmentRow()
        {
        }

        public CmpSegmentRow(string rsid, byte chromosome, int position, Genotype genotype1, Genotype genotype2, string match)
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

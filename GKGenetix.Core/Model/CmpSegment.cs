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

using System;
using System.Collections.Generic;
using System.Data;

namespace GKGenetix.Core.Model
{
    public class CmpSegment : ISNPSegment, ITableRow
    {
        public int SegmentId { get; private set; }
        public string Chromosome { get; private set; }
        public int StartPosition { get; private set; }
        public int EndPosition { get; private set; }
        public double SegmentLength_cm { get; private set; }
        public int SNPCount { get; private set; }


        public IList<CmpSegmentRow> Rows { get; set; }


        public CmpSegment()
        {
        }

        public CmpSegment(string chromosome, int startPosition, int endPosition, double segmentLength_cm, IList<CmpSegmentRow> rows)
        {
            Chromosome = chromosome;
            StartPosition = startPosition;
            EndPosition = endPosition;
            SegmentLength_cm = segmentLength_cm;
            SNPCount = rows.Count;
            Rows = rows;
        }

        public void Load(IDataRecord values)
        {
            SegmentId = values.GetInt32(0);
            Chromosome = values.GetString(1);
            StartPosition = values.GetInt32(2);
            EndPosition = values.GetInt32(3);

            //var dec = values.GetDouble(4); // cast's exception?!
            SegmentLength_cm = Convert.ToDouble(values[4]);

            SNPCount = values.GetInt32(5);
        }
    }


    public class CmpSegmentRow : ISNPHeader, ITableRow
    {
        public string RSID { get; private set; }
        public string Chromosome { get; private set; }
        public int Position { get; private set; }
        public string Kit1Genotype { get; private set; }
        public string Kit2Genotype { get; private set; }
        public string Match { get; private set; }


        public CmpSegmentRow()
        {
        }

        public CmpSegmentRow(string rsid, string chromosome, int position, string kit1Genotype, string kit2Genotype, string match)
        {
            RSID = rsid;
            Chromosome = chromosome;
            Position = position;
            Kit1Genotype = kit1Genotype;
            Kit2Genotype = kit2Genotype;
            Match = match;
        }

        public void Load(IDataRecord values)
        {
            RSID = values.GetString(0);
            Chromosome = values.GetString(1);
            Position = values.GetInt32(2);
            Kit1Genotype = values.GetString(3);
            Kit2Genotype = values.GetString(4);
            Match = values.GetString(5);
        }
    }
}

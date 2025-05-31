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
    public class ROHSegment : ISNPSegment, ITableRow
    {
        public string Chromosome { get; private set; }
        public int StartPosition { get; private set; }
        public int EndPosition { get; private set; }
        public double SegmentLength_cm { get; private set; }
        public int SNPCount { get; private set; }


        public IList<SNP> Rows { get; set; }


        public ROHSegment()
        {
        }

        public ROHSegment(byte chromosome, int startPosition, int endPosition, double segmentLength_cm, IList<SNP> rows)
        {
            Chromosome = chromosome.ToString();
            StartPosition = startPosition;
            EndPosition = endPosition;
            SegmentLength_cm = segmentLength_cm;
            SNPCount = rows.Count;
            Rows = rows;
        }

        public void Load(IDataRecord values)
        {
            Chromosome = values.GetString(0);
            StartPosition = values.GetInt32(1);
            EndPosition = values.GetInt32(2);
            //SegmentLength_cm = values.GetFloat(3); // exception
            SegmentLength_cm = Convert.ToDouble(values[3]);
            SNPCount = values.GetInt32(4);
        }
    }
}

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
using GKGenetix.Core.Database;

namespace GKGenetix.Core.Model
{
    public class CmpSegment : SNPSegment, IDataRecord
    {
        public int SegmentId { get; private set; }


        public IList<SNPMatch> Rows { get; set; }


        public CmpSegment()
        {
        }

        public CmpSegment(byte chromosome, int startPosition, int endPosition, double segmentLength_cm, IList<SNPMatch> rows) : base(chromosome, startPosition, endPosition, segmentLength_cm, rows.Count)
        {
            Rows = rows;
        }
    }
}

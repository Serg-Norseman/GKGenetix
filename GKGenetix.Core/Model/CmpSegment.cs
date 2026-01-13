/*
 *  GKGenetix, the simple DNA analysis kit.
 *  Copyright (C) 2022-2026 by Sergey V. Zhdanovskih.
 *
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System.Collections.Generic;
using GKGenetix.Core.Database;

namespace GKGenetix.Core.Model
{
    public class CmpSegment : SNPSegment, IDataRecord
    {
        public int SegmentId { get; set; }


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

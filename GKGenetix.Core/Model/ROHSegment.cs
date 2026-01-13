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
    public class ROHSegment : SNPSegment, IDataRecord
    {
        public IList<SNP> Rows { get; set; }


        public ROHSegment()
        {
        }

        public ROHSegment(byte chromosome, int startPosition, int endPosition, double segmentLength_cm, IList<SNP> rows) : base(chromosome, startPosition, endPosition, segmentLength_cm, rows.Count)
        {
            Rows = rows;
        }
    }
}

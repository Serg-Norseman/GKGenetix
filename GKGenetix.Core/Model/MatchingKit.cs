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
    public class MatchingKit : SegmentStats, IDataRecord
    {
        public int CmpId { get; set; }
        public string Kit {  get; set; }
        public string Name { get; set; }

        public MatchingKit()
        {
        }
    }
}

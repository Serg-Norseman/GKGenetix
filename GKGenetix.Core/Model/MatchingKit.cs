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

using System.Data;

namespace GKGenetix.Core.Model
{
    public class MatchingKit : SegmentStats, ITableRow
    {
        public int CmpId { get; private set; }
        public string Kit {  get; private set; }
        public string Name { get; private set; }

        public MatchingKit()
        {
        }

        // cmp_id, kit, name, at_longest, at_total, x_longest, x_total, mrca
        public void Load(IDataRecord values)
        {
            CmpId = values.GetInt32(0);
            Kit = values.GetString(1);
            Name = values.GetString(2);
            Longest = values.GetDouble(3);
            Total = values.GetDouble(4);
            XLongest = values.GetDouble(5);
            XTotal = values.GetDouble(6);
            Mrca = values.GetInt32(7);
        }
    }
}

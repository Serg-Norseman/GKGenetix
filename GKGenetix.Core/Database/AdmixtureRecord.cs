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
using GKGenetix.Core.Model;

namespace GKGenetix.Core.Database
{
    public class AdmixtureRecord : IDataRecord
    {
        public string Name { get; private set; }
        public string Population { get; private set; }
        public string Location { get; private set; }
        public double AtTotal { get; private set; }
        public double AtLongest { get; private set; }
        public int Lng { get; private set; }
        public int Lat { get; private set; }


        public double Percentage { get; set; }


        public AdmixtureRecord()
        {
        }

        public void PrepareValues()
        {
            string valPL = Name;
            string[] data = valPL.Replace("_", " ").Split(new char[] { ',' });
            Population = data[0];
            Location = (data.Length > 1) ? data[1] : string.Empty;
        }

        public static void RecalcPercents(IList<AdmixtureRecord> items)
        {
            double total = 0.0;
            for (int i = 0; i < items.Count; i++) {
                total += items[i].AtTotal;
            }

            for (int i = 0; i < items.Count; i++) {
                var row = items[i];
                row.Percentage = (row.AtTotal * 100 / total);
            }
        }
    }
}

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
using System.Data;

namespace GKGenetix.Core.Model
{
    public class AdmixtureRec : ITableRow
    {
        public string Population { get; private set; }
        public string Location { get; private set; }
        public double AtTotal { get; private set; }
        public double AtLongest { get; private set; }
        public int Longitude { get; private set; }
        public int Latitude { get; private set; }

        public double Percentage { get; set; }


        public AdmixtureRec()
        {
        }

        public void Load(IDataRecord values)
        {
            string valPL = values.GetString(0);
            string[] data = valPL.Replace("_", " ").Split(new char[] { ',' });
            Population = data[0];
            Location = (data.Length > 1) ? data[1] : string.Empty;

            AtTotal = values.GetDouble(1);
            AtLongest = values.GetDouble(2);
            Longitude = values.GetInt32(3);
            Latitude = values.GetInt32(4);
        }

        public static void RecalcPercents(IList<AdmixtureRec> items)
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

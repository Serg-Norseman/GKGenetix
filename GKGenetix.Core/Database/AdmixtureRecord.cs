/*
 *  GKGenetix, the simple DNA analysis kit.
 *  Copyright (C) 2022-2026 by Sergey V. Zhdanovskih.
 *
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System.Collections.Generic;

namespace GKGenetix.Core.Database
{
    public class AdmixtureRecord : IDataRecord
    {
        public string Name { get; set; }
        public string Population { get; set; }
        public string Location { get; set; }
        public double AtTotal { get; set; }
        public double AtLongest { get; set; }
        public double Lng { get; set; }
        public double Lat { get; set; }


        public double Percentage { get; set; }


        public AdmixtureRecord()
        {
        }

        public void PrepareValues()
        {
            string valPL = Name;
            if (!string.IsNullOrEmpty(valPL)) {
                string[] data = valPL.Replace("_", " ").Split(new char[] { ',' });
                Population = data[0];
                Location = (data.Length > 1) ? data[1] : string.Empty;
            }
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

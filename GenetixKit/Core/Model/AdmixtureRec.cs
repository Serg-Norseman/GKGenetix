using System.Collections.Generic;
using System.Data;

namespace GenetixKit.Core.Model
{
    internal class AdmixtureRec : ITableRow
    {
        public string Population { get; private set; }
        public string Location { get; private set; }
        public double AtTotal { get; private set; }
        public double AtLongest { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

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
            X = values.GetInt32(3);
            Y = values.GetInt32(4);
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

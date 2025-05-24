using System.Data;

namespace GenetixKit.Core.Model
{
    internal class MatchingKit : ITableRow
    {
        public int CmpId { get; private set; }
        public string Kit {  get; private set; }
        public string Name { get; private set; }
        public double Longest { get; private set; }
        public double Total { get; private set; }
        public double XLongest { get; private set; }
        public double XTotal { get; private set; }
        public int Mrca { get; private set; }

        // cmp_id, kit, name, at_longest, at_total, x_longest, x_total, mrca

        public MatchingKit()
        {
        }

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

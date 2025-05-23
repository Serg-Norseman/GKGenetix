using System.Data;

namespace GenetixKit.Core.Model
{
    internal class OTORow : ISNPHeader, ITableRow
    {
        public string RSID { get; private set; }
        public string Chromosome { get; private set; }
        public int Position { get; private set; }
        public string Genotype1 { get; private set; }
        public string Genotype2 { get; private set; }
        public int Count { get; private set; }


        public OTORow()
        {
        }

        public void Load(IDataRecord values)
        {
            RSID = values.GetString(0);
            Chromosome = values.GetString(1);
            Position = values.GetInt32(2);
            Genotype1 = values.GetString(3);
            Genotype2 = values.GetString(4);
            Count = values.GetInt32(5);
        }
    }
}

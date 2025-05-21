using System.Data;

namespace GenetixKit.Core.Model
{
    internal class OTORow : ISNPHeader
    {
        public string RSID { get; }
        public string Chromosome { get; }
        public int Position { get; }
        public string Genotype1 { get; }
        public string Genotype2 { get; }
        public int Count { get; }

        // rsid, chr, pos, gt1, gt2, count(*)
        public OTORow(IDataRecord values)
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

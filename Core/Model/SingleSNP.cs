using System.Data;

namespace GenetixKit.Core.Model
{
    public class SingleSNP : ISNPHeader
    {
        public string RSID { get; }
        public string Chromosome { get; }
        public int Position { get; }
        public string Genotype { get; }

        public SingleSNP(string rsid, string chromosome, int position, string genotype)
        {
            RSID = rsid;
            Chromosome = chromosome;
            Position = position;
            Genotype = genotype;
        }

        public SingleSNP(IDataRecord values)
        {
            RSID = values.GetString(0);
            Chromosome = values.GetString(1);
            Position = values.GetInt32(2);
            Genotype = values.GetString(3);
        }
    }
}

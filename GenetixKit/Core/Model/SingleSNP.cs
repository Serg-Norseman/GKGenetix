using System.Data;

namespace GenetixKit.Core.Model
{
    public class SingleSNP : ISNPHeader, ITableRow
    {
        public string RSID { get; private set; }
        public string Chromosome { get; private set; }
        public int Position { get; private set; }
        public string Genotype { get; private set; }


        public SingleSNP()
        {
        }

        public SingleSNP(string rsid, string chromosome, int position, string genotype)
        {
            RSID = rsid;
            Chromosome = chromosome;
            Position = position;
            Genotype = genotype;
        }

        public void Load(IDataRecord values)
        {
            RSID = values.GetString(0);
            Chromosome = values.GetString(1);
            Position = values.GetInt32(2);
            Genotype = values.GetString(3);
        }
    }
}

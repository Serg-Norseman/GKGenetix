using System.Data;

namespace GenetixKit.Core.Model
{
    internal class PhaseSegment
    {
        public int Position { get; }
        public string Genotype { get; }
        public string PaternalGenotype { get; }
        public string MaternalGenotype { get; }

        public PhaseSegment(IDataRecord values)
        {
            Position = values.GetInt32(0);
            Genotype = values.GetString(1);
            PaternalGenotype = values.GetString(2);
            MaternalGenotype = values.GetString(3);
        }
    }


    internal class PhaseRow : ISNPHeader
    {
        public string RSID { get; } // 0
        public string Chromosome { get; } // 1
        public int Position { get; } // 2

        public string ChildGenotype { get; set; } // 3
        public string PaternalGenotype { get; set; } // 4
        public string MaternalGenotype { get; set; } // 5

        public char PhasedPaternal { get; set; } // 6
        public char PhasedMaternal { get; set; } // 7

        public bool Mutated { get; set; }
        public bool Ambiguous { get; set; }


        public PhaseRow(IDataRecord values)
        {
            RSID = values.GetString(0);
            Chromosome = values.GetString(1);
            Position = values.GetInt32(2);
            ChildGenotype = values.GetString(3);
            PaternalGenotype = values.GetString(4);
            MaternalGenotype = values.GetString(5);
        }
    }
}

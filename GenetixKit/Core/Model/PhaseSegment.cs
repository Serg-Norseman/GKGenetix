using System.Data;

namespace GenetixKit.Core.Model
{
    internal class PhaseSegment : ITableRow
    {
        public int Position { get; private set; }
        public string Genotype { get; private set; }
        public string PaternalGenotype { get; private set; }
        public string MaternalGenotype { get; private set; }


        public PhaseSegment()
        {
        }

        public void Load(IDataRecord values)
        {
            Position = values.GetInt32(0);
            Genotype = values.GetString(1);
            PaternalGenotype = values.GetString(2);
            MaternalGenotype = values.GetString(3);
        }
    }


    internal class PhaseRow : ISNPHeader, ITableRow
    {
        public string RSID { get; private set; } // 0
        public string Chromosome { get; private set; } // 1
        public int Position { get; private set; } // 2

        public string ChildGenotype { get; set; } // 3
        public string PaternalGenotype { get; set; } // 4
        public string MaternalGenotype { get; set; } // 5

        public char PhasedPaternal { get; set; } // 6
        public char PhasedMaternal { get; set; } // 7

        public bool Mutated { get; set; }
        public bool Ambiguous { get; set; }


        public PhaseRow()
        {
        }

        public void Load(IDataRecord values)
        {
            RSID = values.GetString(0);
            Chromosome = values.GetString(1);
            Position = values.GetInt32(2);
            ChildGenotype = values.GetString(3);
            PaternalGenotype = values.GetString(4);
            MaternalGenotype = values.GetString(5);
        }
    }


    internal class UnphasedSegment : ITableRow
    {
        public string UnphasedKit { get; private set; }
        public string Chromosome { get; private set; }
        public int StartPosition { get; private set; }
        public int EndPosition { get; private set; }


        public UnphasedSegment()
        {
        }

        public void Load(IDataRecord values)
        {
            UnphasedKit = values.GetString(0);
            Chromosome = values.GetString(1);
            StartPosition = values.GetInt32(2);
            EndPosition = values.GetInt32(3);
        }
    }
}

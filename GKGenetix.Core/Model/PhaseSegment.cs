/*
 *  GKGenetix, the simple DNA analysis kit.
 *  Copyright (C) 2022-2026 by Sergey V. Zhdanovskih.
 *
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using GKGenetix.Core.Database;

namespace GKGenetix.Core.Model
{
    public class PhaseSegment : IDataRecord
    {
        public int Position { get; private set; }
        public string Genotype { get; private set; }
        public string PaternalGenotype { get; private set; }
        public string MaternalGenotype { get; private set; }


        public PhaseSegment()
        {
        }
    }


    public class PhaseRow : IDataRecord
    {
        public string rsID { get; set; }
        public byte Chromosome { get; set; }
        public int Position { get; set; }

        public Genotype ChildGenotype { get; set; }
        public Genotype PaternalGenotype { get; set; }
        public Genotype MaternalGenotype { get; set; }

        public char PhasedPaternal { get; set; }
        public char PhasedMaternal { get; set; }

        public bool Mutated { get; set; }
        public bool Ambiguous { get; set; }


        public string ChrStr { get { return Chromosome.ToString(); } set { Chromosome = (byte)value.ParseChromosome(); } }
        public string Child { get { return ChildGenotype.ToString(); } set { ChildGenotype = new Genotype(value); } }
        public string Father { get { return PaternalGenotype.ToString(); } set { PaternalGenotype = new Genotype(value); } }
        public string Mother { get { return MaternalGenotype.ToString(); } set { MaternalGenotype = new Genotype(value); } }


        public PhaseRow()
        {
        }
    }


    public class UnphasedSegment : IDataRecord
    {
        public string UnphasedKit { get; private set; }
        public byte Chromosome { get; private set; }
        public int StartPosition { get; private set; }
        public int EndPosition { get; private set; }


        public string ChrStr { get { return Chromosome.ToString(); } set { Chromosome = (byte)value.ParseChromosome(); } }


        public UnphasedSegment()
        {
        }
    }
}

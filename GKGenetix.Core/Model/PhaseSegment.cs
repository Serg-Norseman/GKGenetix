/*
 *  "GKGenetix", the simple DNA analysis kit.
 *  Copyright (C) 2022-2025 by Sergey V. Zhdanovskih.
 *
 *  This file is part of "GKGenetix".
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
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
        public string rsID { get; private set; } // 0
        public byte Chromosome { get; private set; } // 1
        public int Position { get; private set; } // 2

        public Genotype ChildGenotype { get; set; } // 3
        public Genotype PaternalGenotype { get; set; } // 4
        public Genotype MaternalGenotype { get; set; } // 5

        public char PhasedPaternal { get; set; } // 6
        public char PhasedMaternal { get; set; } // 7

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

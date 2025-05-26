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

using System.Data;

namespace GKGenetix.Core.Model
{
    public class PhaseSegment : ITableRow
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


    public class PhaseRow : ISNPHeader, ITableRow
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


    public class UnphasedSegment : ITableRow
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

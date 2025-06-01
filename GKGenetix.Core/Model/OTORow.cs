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
    public class OTORow : ISNPHeader, ITableRow
    {
        public string rsID { get; private set; }
        public byte Chromosome { get; private set; }
        public int Position { get; private set; }
        public Genotype Genotype1 { get; private set; }
        public Genotype Genotype2 { get; private set; }
        public int Count { get; private set; }


        public OTORow()
        {
        }

        public void Load(IDataRecord values)
        {
            rsID = values.GetString(0);
            Chromosome = (byte)values.GetString(1).ParseChromosome();
            Position = values.GetInt32(2);
            Genotype1 = new Genotype(values.GetString(3));
            Genotype2 = new Genotype(values.GetString(4));
            Count = values.GetInt32(5);
        }
    }
}

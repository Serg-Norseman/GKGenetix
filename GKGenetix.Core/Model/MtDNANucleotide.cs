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

namespace GKGenetix.Core.Model
{
    public sealed class MtDNANucleotide
    {
        public int Pos { get; }
        public string RSRS { get; set; }
        public string Kit { get; set; }

        public bool Mut;
        public bool Ins;

        public MtDNANucleotide(int pos, string a1, string a2)
        {
            Pos = pos;
            RSRS = a1;
            Kit = a2;
        }

        public MtDNANucleotide(int pos, string a1, string a2, bool mut, bool ins) : this(pos, a1, a2)
        {
            Mut = mut;
            Ins = ins;
        }
    }
}

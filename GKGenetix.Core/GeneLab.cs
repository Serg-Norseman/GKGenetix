﻿/*
 *  "GEDKeeper", the personal genealogical database editor.
 *  Copyright (C) 2009-2022 by Sergey V. Zhdanovskih.
 *
 *  This file is part of "GEDKeeper".
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

namespace GKGenetix.Core
{
    public static class GeneLab
    {
        public static char GetComplementaryNucleotide(char n)
        {
            if (n == 'A') {
                n = 'T'; // A -> T
            } else if (n == 'T') {
                n = 'A'; // T -> A
            } else if (n == 'C') {
                n = 'G'; // C -> G
            } else if (n == 'G') {
                n = 'C'; // G -> C
            }
            return n;
        }
    }
}

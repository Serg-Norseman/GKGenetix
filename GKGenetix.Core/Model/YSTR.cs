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
    /// <summary>
    /// A short tandem repeat (STR) is a section of DNA which repeats several times in a row on a DNA strand.
    /// The STRs present and the number of times they repeat is used distinguish one DNA sample from another.
    /// </summary>
    public sealed class YSTR : IDataRecord
    {
        public string Marker { get; private set; }
        public string Repeats { get; private set; }


        public YSTR()
        {
        }

        public YSTR(string marker, string value)
        {
            Marker = marker;
            Repeats = value;
        }
    }
}

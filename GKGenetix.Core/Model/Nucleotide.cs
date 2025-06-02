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

using System;

namespace GKGenetix.Core.Model
{
    [Flags]
    public enum Nucleotide : byte
    {
        /// <summary>
        /// ("0")
        /// </summary>
        None = 0,

        /// <summary>
        /// Adenine (complementary pair - T/U)
        /// </summary>
        A = 1,

        /// <summary>
        /// Cytosine (complementary pair - G)
        /// </summary>
        C = 2,

        /// <summary>
        /// Guanine (complementary pair - C)
        /// </summary>
        G = 3,

        /// <summary>
        /// Thymine (DNA only; complementary pair - A)
        /// </summary>
        T = 4,

        /// <summary>
        /// Uracil (RNA only; complementary pair - A)
        /// </summary>
        U = 5,

        /// <summary>
        /// Insertion ("I")
        /// </summary>
        I = 6,

        /// <summary>
        /// Deletion ("D")
        /// </summary>
        D = 7,

        /// <summary>
        /// 
        /// </summary>
        Homozygous = 8,

        /// <summary>
        /// ("-")
        /// </summary>
        NoCall = 9,
    }
}

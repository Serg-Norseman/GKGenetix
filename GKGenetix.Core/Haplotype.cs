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
    /// <summary>
    /// 
    /// </summary>
    public struct Haplotype
    {
        public string Group;

        /// <summary>
        /// The rsID number ("rs#"; "refSNP cluster") is a unique label ("rs" followed by a number) used by researchers and databases to identify a specific SNP.
        /// It stands for Reference SNP cluster ID and is the naming convention used for most SNPs.
        /// </summary>
        public string rsID;

        /// <summary>
        /// Basepair position.
        /// </summary>
        public uint Pos;

        public char Mutation;
    }
}
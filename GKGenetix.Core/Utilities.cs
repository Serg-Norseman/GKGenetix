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

using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection;

namespace GKGenetix.Core
{
    public class Utilities
    {
        public static Stream LoadResourceGZipStream(string gzName)
        {
            var resStream = LoadResourceStream(gzName);
            return new GZipStream(resStream, CompressionMode.Decompress);
        }

        public static Stream LoadResourceStream(string resName)
        {
            Assembly assembly = typeof(Utilities).Assembly;
            Stream resStream = assembly.GetManifestResourceStream("GKGenetix.Core.Resources." + resName);
            return resStream;
        }

        /// <summary>
        /// Opens a browser window at SNPedia for this SNP.
        /// </summary>
        public static void OpenSNPedia(string rsID)
        {
            Process.Start("http://www.snpedia.com/index.php/" + rsID);
        }
    }
}

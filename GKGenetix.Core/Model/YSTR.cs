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

/*
 *  GKGenetix, the simple DNA analysis kit.
 *  Copyright (C) 2022-2026 by Sergey V. Zhdanovskih.
 *
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

namespace GKGenetix.Core.Reference
{
    public sealed class MtDNAMapItem
    {
        public string MapLocus { get; }
        public string Starting { get; }
        public string Ending { get; }
        public string bpLength { get; }
        public string Shorthand { get; }
        public string Description { get; }

        public MtDNAMapItem(string mapLocus, string starting, string ending, string bpLength, string shorthand, string description)
        {
            MapLocus = mapLocus;
            Starting = starting;
            Ending = ending;
            this.bpLength = bpLength;
            Shorthand = shorthand;
            Description = description;
        }
    }
}

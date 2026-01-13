/*
 *  GKGenetix, the simple DNA analysis kit.
 *  Copyright (C) 2022-2026 by Sergey V. Zhdanovskih.
 *
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
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

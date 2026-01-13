/*
 *  GKGenetix, the simple DNA analysis kit.
 *  Copyright (C) 2022-2026 by Sergey V. Zhdanovskih.
 *
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

namespace GKGenetix.Core.Model
{
    public enum Nucleotide : byte
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,

        /// <summary>
        /// Not determined ("0" [AncestryDNA], "-" [23andMe])
        /// </summary>
        NoCall = 1,

        /// <summary>
        /// Adenine (complementary pair - T/U)
        /// </summary>
        A = 2,

        /// <summary>
        /// Cytosine (complementary pair - G)
        /// </summary>
        C = 3,

        /// <summary>
        /// Guanine (complementary pair - C)
        /// </summary>
        G = 4,

        /// <summary>
        /// Thymine (DNA only; complementary pair - A)
        /// </summary>
        T = 5,

        /// <summary>
        /// Uracil (RNA only; complementary pair - A)
        /// </summary>
        U = 6,

        /// <summary>
        /// Insertion ("I")
        /// </summary>
        I = 7,

        /// <summary>
        /// Deletion ("D")
        /// </summary>
        D = 8,
    }
}

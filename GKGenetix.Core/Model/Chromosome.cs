/*
 *  GKGenetix, the simple DNA analysis kit.
 *  Copyright (C) 2022-2026 by Sergey V. Zhdanovskih.
 *
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

namespace GKGenetix.Core.Model
{
    /// <summary>
    /// Chromosomes in the human genome.
    /// </summary>
    public enum Chromosome : byte
    {
        CHR_NN = 00,

        CHR_01 = 01,
        CHR_02 = 02,
        CHR_03 = 03,
        CHR_04 = 04,
        CHR_05 = 05,
        CHR_06 = 06,
        CHR_07 = 07,
        CHR_08 = 08,
        CHR_09 = 09,
        CHR_10 = 10,
        CHR_11 = 11,
        CHR_12 = 12,
        CHR_13 = 13,
        CHR_14 = 14,
        CHR_15 = 15,
        CHR_16 = 16,
        CHR_17 = 17,
        CHR_18 = 18,
        CHR_19 = 19,
        CHR_20 = 20,
        CHR_21 = 21,
        CHR_22 = 22,

        CHR_X  = 23,
        CHR_Y  = 24, // Non-pseudoautosomal portion of the Y chromosome [AncestryDNA]
        CHR_Yp = 25, // Pseudoautosomal portion of the Y chromosome [AncestryDNA]
        MT     = 26,
    }
}

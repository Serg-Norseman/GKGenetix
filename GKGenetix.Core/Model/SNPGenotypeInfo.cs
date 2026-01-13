/*
 *  GKGenetix, the simple DNA analysis kit.
 *  Copyright (C) 2022-2026 by Sergey V. Zhdanovskih.
 *
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System.Collections.Generic;

namespace GKGenetix.Core.Model
{
    public struct SNPGenotypeInfo
    {
        public Genotype Genotype;
        public string Trait;
        public Dictionary<string, float> PopulationFrequencies;
    }
}

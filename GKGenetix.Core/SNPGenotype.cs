/*
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
    public class SNPGenotype : Genotype
    {
        /// <summary>
        /// Strand orientation.
        /// </summary>
        public Orientation Orientation;

        public SNPGenotype(char a1, char a2, Orientation orientation) : base(a1, a2)
        {
            Orientation = orientation;
        }

        public SNPGenotype(string field, Orientation orientation) : base(field)
        {
            Orientation = orientation;
        }

        public SNPGenotype GetComplement(Orientation orientation)
        {
            return new SNPGenotype(GeneLab.GetComplementaryNucleotide(A1), GeneLab.GetComplementaryNucleotide(A2), orientation);
        }

        /// <summary>
        /// Get the genotype oriented for a given strand.
        /// </summary>
        public SNPGenotype GetOrientedGenotype(Orientation targetOrientation)
        {
            if (targetOrientation == Orientation.Unknown || Orientation == Orientation.Unknown) {
                return new SNPGenotype(UnknownAllele, UnknownAllele, Orientation.Unknown);
            } else if (Orientation == targetOrientation) {
                return this;
            } else {
                return GetComplement(targetOrientation);
            }
        }
    }
}

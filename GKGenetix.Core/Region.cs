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

using System;

namespace GKGenetix.Core
{
    public class Region : IEquatable<Region>
    {
        public int StartPosition { get; set; }
        public int EndPosition { get; set; }

        public int Size
        {
            get { return EndPosition - StartPosition + 1; }
        }


        public Region(int startPosition, int endPosition, bool validatePositions = true)
        {
            StartPosition = startPosition;
            EndPosition = endPosition;

            if (!validatePositions)
                return;

            if (StartPosition <= 0 || EndPosition <= 0 || EndPosition < StartPosition)
                throw new ArgumentException(string.Format("Invalid positions {0}-{1}", StartPosition, EndPosition));
        }

        public bool IsValid()
        {
            return EndPosition >= StartPosition && StartPosition > 0;
        }

        public bool ContainsPosition(int position)
        {
            return position >= StartPosition && position <= EndPosition;
        }

        public bool Overlaps(Region otherRegion)
        {
            return ContainsPosition(otherRegion.StartPosition) || ContainsPosition(otherRegion.EndPosition) ||
                   otherRegion.ContainsPosition(StartPosition) || otherRegion.ContainsPosition(EndPosition);
        }

        public bool FullyContains(Region otherRegion)
        {
            return (StartPosition <= otherRegion.StartPosition && EndPosition >= otherRegion.EndPosition);
        }

        public Region Merge(Region otherRegion)
        {
            if (!Overlaps(otherRegion))
                return null;

            return new Region(Math.Min(otherRegion.StartPosition, StartPosition), Math.Max(otherRegion.EndPosition, EndPosition));
        }

        public bool Equals(Region other)
        {
            if (other == null)
                return false;

            return other.StartPosition == StartPosition && other.EndPosition == EndPosition;
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}", StartPosition, EndPosition);
        }
    }
}

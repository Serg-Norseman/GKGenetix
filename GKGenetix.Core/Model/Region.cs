/*
 *  GKGenetix, the simple DNA analysis kit.
 *  Copyright (C) 2022-2026 by Sergey V. Zhdanovskih.
 *
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;

namespace GKGenetix.Core.Model
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

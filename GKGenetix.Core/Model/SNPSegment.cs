/*
 *  GKGenetix, the simple DNA analysis kit.
 *  Copyright (C) 2022-2026 by Sergey V. Zhdanovskih.
 *
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

namespace GKGenetix.Core.Model
{
    public class SNPSegment
    {
        public byte Chromosome { get; set; }
        public int StartPosition { get; set; }
        public int EndPosition { get; set; }
        public double SegmentLength_cm { get; set; }
        public int SNPCount { get; set; }


        public string ChrStr { get { return Chromosome.ToString(); } set { Chromosome = (byte)value.ParseChromosome(); } }


        public SNPSegment()
        {
        }

        public SNPSegment(byte chromosome, int startPosition, int endPosition, double segmentLength_cm, int snpCount)
        {
            Chromosome = chromosome;
            StartPosition = startPosition;
            EndPosition = endPosition;
            SegmentLength_cm = segmentLength_cm;
            SNPCount = snpCount;
        }
    }
}

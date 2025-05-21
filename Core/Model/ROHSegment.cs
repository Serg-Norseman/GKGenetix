using System;
using System.Collections.Generic;
using System.Data;

namespace GenetixKit.Core.Model
{
    internal class ROHSegment : ISNPSegment
    {
        public string Chromosome { get; }
        public int StartPosition { get; }
        public int EndPosition { get; }
        public double SegmentLength_cm { get; }
        public int SNPCount { get; }


        public IList<SingleSNP> Rows { get; set; }


        public ROHSegment(string chromosome, int startPosition, int endPosition, double segmentLength_cm, int snpCount, IList<SingleSNP> rows)
        {
            Chromosome = chromosome;
            StartPosition = startPosition;
            EndPosition = endPosition;
            SegmentLength_cm = segmentLength_cm;
            SNPCount = snpCount;
            Rows = rows;
        }

        public ROHSegment(IDataRecord values)
        {
            Chromosome = values.GetString(0);
            StartPosition = values.GetInt32(1);
            EndPosition = values.GetInt32(2);
            SegmentLength_cm = Convert.ToDouble(values[3]);
            SNPCount = values.GetInt32(4);
        }
    }
}

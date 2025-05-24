using System;
using System.Collections.Generic;
using System.Data;

namespace GenetixKit.Core.Model
{
    internal class ROHSegment : ISNPSegment, ITableRow
    {
        public string Chromosome { get; private set; }
        public int StartPosition { get; private set; }
        public int EndPosition { get; private set; }
        public double SegmentLength_cm { get; private set; }
        public int SNPCount { get; private set; }


        public IList<SingleSNP> Rows { get; set; }


        public ROHSegment()
        {
        }

        public ROHSegment(string chromosome, int startPosition, int endPosition, double segmentLength_cm, IList<SingleSNP> rows)
        {
            Chromosome = chromosome;
            StartPosition = startPosition;
            EndPosition = endPosition;
            SegmentLength_cm = segmentLength_cm;
            SNPCount = rows.Count;
            Rows = rows;
        }

        public void Load(IDataRecord values)
        {
            Chromosome = values.GetString(0);
            StartPosition = values.GetInt32(1);
            EndPosition = values.GetInt32(2);
            SegmentLength_cm = Convert.ToDouble(values[3]);
            SNPCount = values.GetInt32(4);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;

namespace GenetixKit.Core.Model
{
    internal class CmpSegment : ISNPSegment
    {
        public int SegmentId { get; }
        public string Chromosome { get; }
        public int StartPosition { get; }
        public int EndPosition { get; }
        public double SegmentLength_cm { get; }
        public int SNPCount { get; }


        public IList<CmpSegmentRow> Rows { get; set; }


        public CmpSegment(string chromosome, int startPosition, int endPosition, double segmentLength_cm, int snpCount, IList<CmpSegmentRow> rows)
        {
            Chromosome = chromosome;
            StartPosition = startPosition;
            EndPosition = endPosition;
            SegmentLength_cm = segmentLength_cm;
            SNPCount = snpCount;
            Rows = rows;
        }

        public CmpSegment(IDataRecord values)
        {
            SegmentId = values.GetInt32(0);
            Chromosome = values.GetString(1);
            StartPosition = values.GetInt32(2);
            EndPosition = values.GetInt32(3);

            //var dec = values.GetDouble(4); // cast's exception?!
            SegmentLength_cm = Convert.ToDouble(values[4]);

            SNPCount = values.GetInt32(5);
        }
    }


    internal class CmpSegmentRow : ISNPHeader
    {
        public string RSID { get; }
        public string Chromosome { get; }
        public int Position { get; }
        public string Kit1Genotype { get; }
        public string Kit2Genotype { get; }
        public string Match { get; }

        public CmpSegmentRow(string rsid, string chromosome, int position, string kit1Genotype, string kit2Genotype, string match)
        {
            RSID = rsid;
            Chromosome = chromosome;
            Position = position;
            Kit1Genotype = kit1Genotype;
            Kit2Genotype = kit2Genotype;
            Match = match;
        }

        public CmpSegmentRow(IDataRecord values)
        {
            RSID = values.GetString(0);
            Chromosome = values.GetString(1);
            Position = values.GetInt32(2);
            Kit1Genotype = values.GetString(3);
            Kit2Genotype = values.GetString(4);
            Match = values.GetString(5);
        }
    }
}

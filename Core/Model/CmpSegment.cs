using System;
using System.Collections.Generic;
using System.Data;

namespace GenetixKit.Core.Model
{
    internal class CmpSegment : ISNPSegment, ITableRow
    {
        public int SegmentId { get; private set; }
        public string Chromosome { get; private set; }
        public int StartPosition { get; private set; }
        public int EndPosition { get; private set; }
        public double SegmentLength_cm { get; private set; }
        public int SNPCount { get; private set; }


        public IList<CmpSegmentRow> Rows { get; set; }


        public CmpSegment()
        {
        }

        public CmpSegment(string chromosome, int startPosition, int endPosition, double segmentLength_cm, IList<CmpSegmentRow> rows)
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
            SegmentId = values.GetInt32(0);
            Chromosome = values.GetString(1);
            StartPosition = values.GetInt32(2);
            EndPosition = values.GetInt32(3);

            //var dec = values.GetDouble(4); // cast's exception?!
            SegmentLength_cm = Convert.ToDouble(values[4]);

            SNPCount = values.GetInt32(5);
        }
    }


    internal class CmpSegmentRow : ISNPHeader, ITableRow
    {
        public string RSID { get; private set; }
        public string Chromosome { get; private set; }
        public int Position { get; private set; }
        public string Kit1Genotype { get; private set; }
        public string Kit2Genotype { get; private set; }
        public string Match { get; private set; }


        public CmpSegmentRow()
        {
        }

        public CmpSegmentRow(string rsid, string chromosome, int position, string kit1Genotype, string kit2Genotype, string match)
        {
            RSID = rsid;
            Chromosome = chromosome;
            Position = position;
            Kit1Genotype = kit1Genotype;
            Kit2Genotype = kit2Genotype;
            Match = match;
        }

        public void Load(IDataRecord values)
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

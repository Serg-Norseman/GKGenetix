using System;
using System.Collections.Generic;

namespace GenetixKit.Core.Model
{
    internal class SegmentStats
    {
        public double Total { get; }
        public double Longest { get; }
        public double XTotal { get; }
        public double XLongest { get; }
        public int Mrca { get; }

        public SegmentStats(double total, double longest, double xTotal, double xLongest, int mrca)
        {
            Total = total;
            Longest = longest;
            XTotal = xTotal;
            XLongest = xLongest;
            Mrca = mrca;
        }

        public static SegmentStats CalculateSegmentStats(IEnumerable<ISNPSegment> segment_idx)
        {
            double total = 0;
            double longest = 0;
            double x_total = 0;
            double x_longest = 0;
            int mrca = 0;

            foreach (var row in segment_idx) {
                double seg_len = row.SegmentLength_cm;
                if (row.Chromosome == "X") {
                    x_total += seg_len;
                    if (x_longest < seg_len)
                        x_longest = seg_len;
                } else {
                    total += seg_len;
                    if (longest < seg_len)
                        longest = seg_len;
                }
            }

            for (int gen = 0; gen < 10; gen++) {
                double shared = 3600 / Math.Pow(2, gen);
                double range_begin = shared - shared / 4;
                double range_end = shared + shared / 4;
                if (total < range_end && total > range_begin)
                    mrca = gen + 1;
            }

            return new SegmentStats(total, longest, x_total, x_longest, mrca);
        }

        public string GetMRCAText(bool roh)
        {
            string result;

            var mrca = Mrca;
            if (roh) {
                // adjusting mrca for RoH specific
                if (mrca > 0) {
                    mrca--;

                    if (mrca == 1)
                        result = mrca.ToString() + " generation back";
                    else
                        result = mrca.ToString() + " generations back";
                } else
                    result = "Not Related";
            } else {
                if (mrca > 0) {
                    if (mrca == 1)
                        result = mrca.ToString() + " generation back";
                    else
                        result = mrca.ToString() + " generations back";
                } else {
                    if (Total > 0 || XTotal > 0)
                        result = "Distantly related.";
                    else
                        result = "Not related";
                }
            }

            return result;
        }
    }
}

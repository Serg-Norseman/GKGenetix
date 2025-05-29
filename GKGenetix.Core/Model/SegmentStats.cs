/*
 *  "GKGenetix", the simple DNA analysis kit.
 *  Copyright (C) 2022-2025 by Sergey V. Zhdanovskih.
 *
 *  This file is part of "GKGenetix".
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
using System.Collections.Generic;

namespace GKGenetix.Core.Model
{
    public class SegmentStats
    {
        public double Total { get; protected set; }
        public double Longest { get; protected set; }
        public double XTotal { get; protected set; }
        public double XLongest { get; protected set; }
        public int Mrca { get; protected set; }

        public SegmentStats()
        {
        }

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

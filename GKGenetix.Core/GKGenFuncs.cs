/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using GKGenetix.Core.Database;
using GKGenetix.Core.Model;
using GKGenetix.Core.Reference;

namespace GKGenetix.Core
{
    public static class GKGenFuncs
    {
        private static readonly List<string> markers_new = new List<string>();


        public static double GetLength_in_cM(byte chr, int start_pos, int end_pos)
        {
            var chr_s = chr.ToString();
            return GetPosition_in_cM(chr_s, end_pos) - GetPosition_in_cM(chr_s, start_pos);
        }

        public static double GetLength_in_cM(string chr, int start_pos, int end_pos)
        {
            return GetPosition_in_cM(chr, end_pos) - GetPosition_in_cM(chr, start_pos);
        }

        private static double GetPosition_in_cM(string chr, int pos)
        {
            double cm = 0.0;
            int chr_int = (chr == "X") ? 23 : int.Parse(chr);
            if (chr_int - 1 >= RefData.cM_Map.Length) return cm;

            SortedList<int, double> tmap = RefData.cM_Map[chr_int - 1];
            if (tmap.TryGetValue(pos, out cm))
                return cm;

            var positions = tmap.Keys.ToList(); // materialize once
            int index = positions.BinarySearch(pos);

            if (index < 0)
                index = ~index;

            if (index == 0)
                return tmap[positions[0]];

            if (index == positions.Count)
                return tmap[positions.Last()];

            int prevPos = positions[index - 1];
            int nextPos = positions[index];

            int diff = nextPos - prevPos;
            double cm_diff = tmap[nextPos] - tmap[prevPos];
            cm = tmap[prevPos] + cm_diff * (pos - prevPos) / diff;

            return cm;
        }

        public static string[] GetYSNP(string pos, string gt)
        {
            string[] data = RefData.YMap[pos];

            if (data[1].EndsWith("->" + gt))
                data[1] = "+";
            else
                data[1] = "-";

            return data;
        }

        private static Process ExecuteDiff(string file1, string file2, string diff_work_dir)
        {
            File.WriteAllBytes(diff_work_dir + "diff.exe", GKGenetix.Core.Properties.Resources.diff);

            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WorkingDirectory = diff_work_dir;
            p.StartInfo.FileName = diff_work_dir + "diff.exe";
            p.StartInfo.Arguments = file1 + " " + file2;
            p.Start();
            return p;
        }

        public static string GetMarkers(string file)
        {
            string rsrs = new String(RefData.RSRS).ToUpper();
            string user = FastaSeq(file).ToUpper();
            rsrs = Regex.Replace(rsrs, "(.)", "$1\r\n");
            user = Regex.Replace(user, "(.)", "$1\r\n");

            string diff_work_dir = Path.GetTempPath() + "Fasta2Rsrs\\";
            Directory.CreateDirectory(diff_work_dir);

            File.WriteAllText(diff_work_dir + "rsrs.txt", rsrs);
            File.WriteAllText(diff_work_dir + "user.txt", user);

            Process p = ExecuteDiff(diff_work_dir + "rsrs.txt", diff_work_dir + "user.txt", diff_work_dir);

            StringBuilder sb = new StringBuilder();
            while (!p.StandardOutput.EndOfStream) {
                string line = p.StandardOutput.ReadLine();
                //int op = -1;
                string[] rsrs_pos;
                string[] rsrs_a;
                string[] user_a;

                int count;
                if (line.IndexOf("c") != -1) {
                    // change..
                    //op = OP_CHANGE;
                    rsrs_pos = line.Split(new char[] { 'c' })[0].Split(new char[] { ',' });
                    count = rsrs_pos.Length;
                    rsrs_a = new string[count];
                    user_a = new string[count];

                    for (int i = 0; i < count; i++) {
                        rsrs_a[i] = p.StandardOutput.ReadLine();
                        rsrs_a[i] = rsrs_a[i].Substring(rsrs_a[i].Length - 1);
                    }
                    p.StandardOutput.ReadLine();// middleline
                    for (int i = 0; i < count; i++) {
                        user_a[i] = p.StandardOutput.ReadLine();
                        user_a[i] = user_a[i].Substring(user_a[i].Length - 1);

                        if ((rsrs_a[i] == "A" && user_a[i] == "G") ||
                            (rsrs_a[i] == "G" && user_a[i] == "A") ||
                            (rsrs_a[i] == "T" && user_a[i] == "C") ||
                            (rsrs_a[i] == "C" && user_a[i] == "T") ||
                            (rsrs_a[i] == "N" || user_a[i] == "N"))
                            //transition AG TC
                            sb.Append(rsrs_a[i] + rsrs_pos[i] + user_a[i] + " ");
                        else
                            sb.Append(rsrs_a[i] + rsrs_pos[i] + user_a[i].ToLower() + " ");
                    }
                } else if (line.IndexOf("a") != -1) {
                    // insert..
                    //op = OP_INSERT;
                    rsrs_pos = line.Split(new char[] { 'a' })[0].Split(new char[] { ',' });
                    count = rsrs_pos.Length;
                    user_a = new string[count];
                    string pos = rsrs_pos[0];
                    for (int i = 0; i < count; i++) {
                        user_a[i] = p.StandardOutput.ReadLine();
                        user_a[i] = user_a[i].Substring(user_a[i].Length - 1);
                        sb.Append(pos + "." + (i + 1) + user_a[i] + " ");
                    }
                } else if (line.IndexOf("d") != -1) {
                    // delete..
                    //op = OP_DELETE;
                    rsrs_pos = line.Split(new char[] { 'd' })[0].Split(new char[] { ',' });
                    count = rsrs_pos.Length;
                    rsrs_a = new string[count];

                    for (int i = 0; i < count; i++) {
                        rsrs_a[i] = p.StandardOutput.ReadLine();
                        rsrs_a[i] = rsrs_a[i].Substring(rsrs_a[i].Length - 1);
                        sb.Append(rsrs_a[i] + rsrs_pos[i] + "D ");
                    }
                }
            }

            try {
                if (p != null) {
                    if (!p.HasExited)
                        p.Kill();
                    while (!p.HasExited) {
                        Thread.Sleep(100);
                    }
                }
            } catch (Exception) { }

            Directory.Delete(diff_work_dir, true);

            return sb.ToString().Trim().Replace(" ", ", ");
        }

        private static string FastaSeq(string file)
        {
            StringBuilder sb = new StringBuilder();

            using (StreamReader sr = new StreamReader(file)) {
                string line = sr.ReadLine(); // skip first line
                while ((line = sr.ReadLine()) != null) {
                    sb.Append(line);
                }
            }

            return sb.ToString();
        }

        private static string ConvertInsDelToMod(List<string> markers, string file)
        {
            string rsrs = new String(RefData.RSRS).ToUpper();
            string user = FastaSeq(file).ToUpper();
            Stack<int> stack = new Stack<int>();
            Dictionary<int, int> ht = new Dictionary<int, int>();

            foreach (string str in markers) {
                if (str.EndsWith("D")) {
                    int start = int.Parse(str.Substring(1).Replace("D", ""));
                    stack.Push(start);
                } else if (str.Contains(".")) {
                    int end = int.Parse(str.Substring(0, str.IndexOf(".")));
                    if (stack.Count != 0) {
                        int start_paired = stack.Pop();
                        if (stack.Count == 0) {
                            ht.Add(start_paired, end);
                        }
                    }
                }
            }

            var new_mut = new List<string>();
            foreach (var kvp in ht) {
                int start = kvp.Key;
                int end = kvp.Value;

                char[] fasta_char = rsrs.ToCharArray();
                char[] user_char = user.ToCharArray();
                for (int i = start; i <= end; i++) {
                    int offset = i > 3107 ? 1 : 0;
                    char f = fasta_char[i - offset];
                    char u = user_char[i];
                    if (f != u) {
                        if ((f == 'A' && u == 'G') ||
                          (f == 'G' && u == 'A') ||
                          (f == 'T' && u == 'C') ||
                          (f == 'C' && u == 'T'))
                            new_mut.Add(f.ToString() + i.ToString() + u.ToString());
                        else
                            new_mut.Add(f.ToString() + i.ToString() + u.ToString().ToLower());
                    }
                }
            }

            markers_new.Clear();

            foreach (string str in markers) {
                if (!markers_new.Contains(str))
                    markers_new.Add(str);
            }
            foreach (string str in new_mut) {
                if (!markers_new.Contains(str))
                    markers_new.Add(str);
            }

            foreach (string str in markers) {
                foreach (var kvp in ht) {
                    int start = int.Parse(kvp.Key.ToString());
                    int end = int.Parse(kvp.Value.ToString());
                    for (int i = start; i <= end; i++) {
                        if ((str.EndsWith(i + "D") && str.Length == (i + "D").Length + 1) || (str.IndexOf(i + ".") != -1 && str.Length == (i.ToString() + ".").Length + 2)) {
                            markers_new.Remove(str);
                        }
                    }
                }
            }

            return string.Join(",", markers_new);
        }

        public static string GetMtDNAMarkers(string fasta_file)
        {
            string txt = GetMarkers(fasta_file);

            string[] txt2 = txt.Replace(" ", "").Split(new char[] { ',' });
            List<string> markers = new List<string>();
            foreach (string str in txt2) {
                // if (str != "3107.1C" && str != "C3106D" && str != "N3107D")

                //C3106D, 3107.1C
                if (str == "C3106D" || str == "3107.1C" || str == "N3107D" || str == "N523D" || str == "N524D") {
                    // don't add. 
                } else if (str.StartsWith("N523") && !str.EndsWith("D"))
                    markers.Add("522.1" + str.Substring(str.Length - 1));
                else if (str.StartsWith("N524") && !str.EndsWith("D"))
                    markers.Add("522.2" + str.Substring(str.Length - 1));
                else if (str == "573.XC")
                    markers.Add("573.1C");
                else
                    markers.Add(str);
            }

            string markers_str = ConvertInsDelToMod(markers, fasta_file);
            return markers_str;
        }

        public static List<string> LoadYDNAFile(string file)
        {
            var snpList = new List<string>();

            var ymap = RefData.YMap;
            using (StreamReader sr = new StreamReader(file)) {
                string line;
                while ((line = sr.ReadLine()) != null) {
                    string[] data = line.Replace("\"", "").Split(new char[] { ',' });

                    // "Type" 0, "Position" 1, "SNPName" 2, "Derived" 3, "OnTree" 4, "Reference" 5, "Genotype" 6, "Confidence" 7
                    string valType = data[0];
                    string valPos = data[1];
                    string valSNP = data[2];
                    string valDerived = data[3];
                    string valGt = data[6];

                    if (valType == "Known SNP") {
                        if (valDerived == "Yes(+)") {
                            snpList.Add(valSNP + "+");
                        } else if (valDerived == "No(-)") {
                            snpList.Add(valSNP + "-");
                        }
                    } else if (valType == "Novel Variant") {
                        if (ymap.ContainsKey(valPos)) {
                            string[] snp = GetYSNP(valPos, valGt);
                            if (snp[0].IndexOf(";") == -1)
                                snpList.Add(snp[0] + snp[1]);
                            else
                                snpList.Add(snp[0].Substring(0, snp[0].IndexOf(";")) + snp[1]);
                        }
                    }
                }
            }

            return snpList;
        }

        public static void DontMatchProc(int start_pos, int end_pos, byte prev_chr, byte chromosome, IList<CmpSegment> segments_idx, ref List<SNPMatch> tmp, bool reference)
        {
            double cm_len, cm_th;
            int snp_th;
            bool overTh;

            int diffPos = (end_pos - start_pos);

            if (reference) {
                snp_th = RefData.Admixture_Threshold_SNPs;
                cm_th = RefData.Admixture_Threshold_cM;
                overTh = diffPos > 5000;
            } else {
                if (chromosome == (byte)Chromosome.CHR_X) {
                    cm_th = RefData.Compare_X_Threshold_cM;
                    snp_th = RefData.Compare_X_Threshold_SNPs;
                } else {
                    cm_th = RefData.Compare_Autosomal_Threshold_cM;
                    snp_th = RefData.Compare_Autosomal_Threshold_SNPs;
                }
                overTh = diffPos / 1000000.0 > RefData.MB_THRESHOLD;
            }

            if (overTh) {
                cm_len = GetLength_in_cM(chromosome, start_pos, end_pos);

                if (cm_len >= cm_th && tmp.Count >= snp_th) {
                    if (prev_chr != chromosome)
                        segments_idx.Add(new CmpSegment(prev_chr, start_pos, end_pos, cm_len, tmp));
                    else
                        segments_idx.Add(new CmpSegment(chromosome, start_pos, end_pos, cm_len, tmp));
                }
            }

            tmp = new List<SNPMatch>();
        }

        public static void DontMatchProcRoH(int start_pos, int end_pos, byte prev_chr, byte chromosome, IList<ROHSegment> segments_idx, ref List<SNP> tmp, bool reference)
        {
            double cm_len, cm_th;
            int snp_th;
            bool overTh;

            int diffPos = (end_pos - start_pos);

            if (reference) {
                snp_th = RefData.Admixture_Threshold_SNPs;
                cm_th = RefData.Admixture_Threshold_cM;
                overTh = diffPos > 5000;
            } else {
                if (chromosome == (byte)Chromosome.CHR_X) {
                    cm_th = RefData.Compare_X_Threshold_cM;
                    snp_th = RefData.Compare_X_Threshold_SNPs;
                } else {
                    cm_th = RefData.Compare_Autosomal_Threshold_cM;
                    snp_th = RefData.Compare_Autosomal_Threshold_SNPs;
                }
                overTh = diffPos / 1000000.0 > RefData.MB_THRESHOLD;
            }

            if (overTh) {
                cm_len = GetLength_in_cM(chromosome, start_pos, end_pos);

                if (cm_len >= cm_th && tmp.Count >= snp_th) {
                    if (prev_chr != chromosome)
                        segments_idx.Add(new ROHSegment(prev_chr, start_pos, end_pos, cm_len, tmp));
                    else
                        segments_idx.Add(new ROHSegment(chromosome, start_pos, end_pos, cm_len, tmp));
                }
            }

            tmp = new List<SNP>();
        }

        /*
         * Type: 
                U - Unknown or not computed.
                B - Both Paternal and Maternal
                M - Maternal
                P - Paternal
                C - Compound
         * 
         * Phased Details:
         * 
                 R = A/G
                 Y = C/T
                 S = G/C
                 W = A/T
                 K = G/T
                 M = A/C             
         */
        public static T GetPhasedSegmentImage<T>(IList<PhaseSegment> dt, byte chromosome) where T : AbstractImage, new()
        {
            int paternal_error_position = 0;
            int maternal_error_position = 0;
            int snp_th;
            int errorRadius;
            if (chromosome == (byte)Chromosome.CHR_X) {
                snp_th = RefData.Compare_X_Threshold_SNPs;
                errorRadius = snp_th / 2;
            } else {
                snp_th = RefData.Compare_Autosomal_Threshold_SNPs;
                errorRadius = snp_th / 2;
            }
            int no_call_limit = RefData.Compare_NoCalls_Limit;

            int paternal_no_call_count = 0;
            int maternal_no_call_count = 0;
            int x = 0;

            int width = 600;
            int height = 150;

            var img = new T();
            img.SetSize(width, height);

            int begin_maternal_pos = 0;
            int begin_paternal_pos = 0;

            int curr_pos = 0;
            int val;
            for (int ix = 0; ix < dt.Count; ix++) {
                var row = dt[ix];
                curr_pos = ix;
                x = curr_pos * width / dt.Count;

                // paternal not matched
                if (!IsPhasedMatch(row.Genotype, row.PaternalGenotype)) {
                    if ((row.Genotype.IndexOf('-') != -1 || row.PaternalGenotype.IndexOf('-') != -1 || row.Genotype.IndexOf('?') != -1 || row.PaternalGenotype.IndexOf('?') != -1) && paternal_no_call_count <= no_call_limit) {
                        //allow no call but count it.
                        paternal_no_call_count++;
                    } else if (curr_pos - paternal_error_position >= errorRadius && paternal_no_call_count <= no_call_limit) {
                        // allow but reset no call counter.
                        paternal_error_position = curr_pos;
                        paternal_no_call_count = 0;
                    } else {
                        //if (curr_pos - begin_paternal_pos > snp_th)
                        if (curr_pos - begin_paternal_pos > 5) {
                            val = curr_pos - begin_paternal_pos;
                            if (val > snp_th)
                                val = snp_th;
                            int tmp = (snp_th - val) * 255 / snp_th;
                            img.SetPen(255, tmp, tmp, 255, 1);
                            for (int i = begin_paternal_pos * width / dt.Count; i < x; i++)
                                img.DrawLine(i, 0, i, height / 2);
                        }

                        // don't allow but reset no call counter.
                        paternal_no_call_count = 0;
                        paternal_error_position = curr_pos;
                        begin_paternal_pos = curr_pos;
                    }
                }

                // maternal not matched
                if (!IsPhasedMatch(row.Genotype, row.MaternalGenotype)) {
                    if ((row.Genotype.IndexOf('-') != -1 || row.MaternalGenotype.IndexOf('-') != -1 || row.Genotype.IndexOf('?') != -1 || row.MaternalGenotype.IndexOf('?') != -1) && maternal_no_call_count <= no_call_limit) {
                        //allow no call but count it.
                        maternal_no_call_count++;
                    } else if (curr_pos - maternal_error_position >= errorRadius && maternal_no_call_count <= no_call_limit) {
                        // allow but reset no call counter.
                        maternal_error_position = curr_pos;
                        maternal_no_call_count = 0;
                    } else {
                        if (curr_pos - begin_maternal_pos > 5) {
                            val = curr_pos - begin_maternal_pos;
                            if (val > snp_th)
                                val = snp_th;
                            int tmp = (snp_th - val) * 255 / snp_th;
                            img.SetPen(255, 255, tmp, tmp, 1);
                            for (int i = begin_maternal_pos * width / dt.Count; i < x; i++)
                                img.DrawLine(i, height / 2, i, height);
                        }
                        // don't allow but reset no call counter.
                        maternal_no_call_count = 0;
                        maternal_error_position = curr_pos;
                        begin_maternal_pos = curr_pos;
                    }
                }
            }

            if (curr_pos - begin_paternal_pos > 5) {
                val = curr_pos - begin_paternal_pos;
                if (val > snp_th)
                    val = snp_th;
                int tmp = (snp_th - val) * 255 / snp_th;
                img.SetPen(255, tmp, tmp, 255, 1);
                for (int i = begin_paternal_pos * width / dt.Count; i < x; i++)
                    img.DrawLine(i, 0, i, height / 2);
            }

            if (curr_pos - begin_maternal_pos > 5) {
                val = curr_pos - begin_maternal_pos;
                if (val > snp_th)
                    val = snp_th;
                int tmp = (snp_th - val) * 255 / snp_th;
                img.SetPen(255, 255, tmp, tmp, 1);
                for (int i = begin_maternal_pos * width / dt.Count; i < x; i++)
                    img.DrawLine(i, height / 2, i, height);
            }
            return img;
        }

        public static bool IsPhasedMatch(string gt1, string gt2)
        {
            string[] nc = new string[]{
                "R", "AG",
                "Y", "CT",
                "S", "GC",
                "W", "AT",
                "K", "GT",
                "M", "AC"
            };

            for (int i = 0; i < nc.Length; i += 2) {
                gt1 = gt1.Replace(nc[i], nc[i + 1]);
                gt2 = gt2.Replace(nc[i], nc[i + 1]);
            }

            foreach (char c1 in gt1.ToCharArray())
                foreach (char c2 in gt2.ToCharArray()) {
                    if (c1 == c2)
                        return true;
                }

            return false;
        }

        public static char GetNucleotideCode(char bp1, char bp2)
        {
            /*
                 R = A/G
                 Y = C/T
                 S = G/C
                 W = A/T
                 K = G/T
                 M = A/C
             */

            if ((bp1 == 'A' && bp2 == 'G') || (bp1 == 'G' && bp2 == 'A'))
                return 'R';
            else if ((bp1 == 'C' && bp2 == 'T') || (bp1 == 'T' && bp2 == 'C'))
                return 'Y';
            else if ((bp1 == 'C' && bp2 == 'G') || (bp1 == 'G' && bp2 == 'C'))
                return 'S';
            else if ((bp1 == 'A' && bp2 == 'T') || (bp1 == 'T' && bp2 == 'A'))
                return 'W';
            else if ((bp1 == 'T' && bp2 == 'G') || (bp1 == 'G' && bp2 == 'T'))
                return 'K';
            else if ((bp1 == 'A' && bp2 == 'C') || (bp1 == 'C' && bp2 == 'A'))
                return 'M';
            else
                return 'N';
        }

        public static IList<CmpSegment> CompareOneToOne(string kit1, string kit2, BackgroundWorker bwCompare, bool reference, bool justUpdate)
        {
            // just_update - if parameter true, will update if record not found but will not return the existing record if found.
            IList<CmpSegment> segments_idx = new List<CmpSegment>();

            bool exists = GKSqlFuncs.CheckAlreadyCompared(kit1, kit2);

            if (exists && !justUpdate) {
                segments_idx = GKSqlFuncs.GetAutosomalCmp(kit1, kit2);

                foreach (var row in segments_idx) {
                    if (bwCompare != null && bwCompare.CancellationPending)
                        break;

                    if (!reference) {
                        row.Rows = GKSqlFuncs.GetCmpSeg(row.SegmentId);
                    }
                }
            } else {
                var otoRows = GKSqlFuncs.GetOTORows(kit1, kit2);

                var tmp = new List<SNPMatch>();

                byte prev_chr = 0;
                int start_pos = 0;
                int end_pos = 0;
                int prev_snp_count = 0;
                int no_call_counter = 0;
                int no_call_limit = RefData.Compare_NoCalls_Limit;

                foreach (var rd in otoRows) {
                    if (bwCompare != null && bwCompare.CancellationPending)
                        break;

                    string rsid = rd.rsID;
                    byte chromosome = rd.Chromosome;
                    int position = rd.Position;
                    var gt1 = rd.Genotype1;
                    var gt2 = rd.Genotype2;
                    string count = rd.Match;

                    if (prev_chr == 0) prev_chr = chromosome;
                    //gt1.CheckCompleteness();
                    //gt2.CheckCompleteness();

                    if (prev_chr == chromosome) {
                        float errorRadius = (chromosome == (byte)Chromosome.CHR_X) ? RefData.Compare_X_Threshold_SNPs / 2 : RefData.Compare_Autosomal_Threshold_SNPs / 2;

                        if (count == "1") {
                            // match both alleles
                            tmp.Add(new SNPMatch(rsid, chromosome, position, gt1, gt2, gt1.ToString()));
                            if (start_pos == 0) start_pos = position;
                        } else if (count == "2") {
                            // match 1 allele
                            if (gt1.Equals(gt2)) {
                                tmp.Add(new SNPMatch(rsid, chromosome, position, gt1, gt2, gt1.ToString()));
                                if (start_pos == 0) start_pos = position;
                            } else if (gt1.A1 == gt2.A1) {
                                tmp.Add(new SNPMatch(rsid, chromosome, position, gt1, gt2, gt2.A1.ToString()));
                                if (start_pos == 0) start_pos = position;
                            } else if (gt1.A1 == gt2.A2) {
                                tmp.Add(new SNPMatch(rsid, chromosome, position, gt1, gt2, gt2.A2.ToString()));
                                if (start_pos == 0) start_pos = position;
                            } else if (gt1.A2 == gt2.A1) {
                                tmp.Add(new SNPMatch(rsid, chromosome, position, gt1, gt2, gt2.A1.ToString()));
                                if (start_pos == 0) start_pos = position;
                            } else if (gt1.A2 == gt2.A2) {
                                tmp.Add(new SNPMatch(rsid, chromosome, position, gt1, gt2, gt2.A2.ToString()));
                                if (start_pos == 0) start_pos = position;
                            } else {
                                no_call_counter++;
                                if (no_call_counter > no_call_limit) {
                                    // no call exceeded..
                                    prev_snp_count = 0;
                                    no_call_counter = 0;
                                    DontMatchProc(start_pos, end_pos, prev_chr, chromosome, segments_idx, ref tmp, reference);
                                    start_pos = position;
                                } else if (gt1.IsEmptyOrUnknown() || gt2.IsEmptyOrUnknown()) {
                                    tmp.Add(new SNPMatch(rsid, chromosome, position, gt1, gt2, "-"));
                                    if (start_pos == 0) start_pos = position;
                                } else if (tmp.Count - prev_snp_count >= errorRadius && no_call_counter <= no_call_limit) {
                                    prev_snp_count = tmp.Count;
                                    no_call_counter = 0;
                                    tmp.Add(new SNPMatch(rsid, chromosome, position, gt1, gt2, ""));
                                    if (start_pos == 0)
                                        start_pos = position;
                                } else {
                                    // doesn't match on same chromosome
                                    prev_snp_count = 0;
                                    no_call_counter = 0;
                                    DontMatchProc(start_pos, end_pos, prev_chr, chromosome, segments_idx, ref tmp, reference);
                                    start_pos = position;
                                }
                            }
                        }
                    } else {
                        // next chromosome
                        prev_snp_count = 0;
                        no_call_counter = 0;
                        DontMatchProc(start_pos, end_pos, prev_chr, chromosome, segments_idx, ref tmp, reference);
                        start_pos = position;
                    }
                    end_pos = position;
                    prev_chr = chromosome;
                }

                otoRows.Clear();

                GKSqlFuncs.SaveAutosomalCmp(kit1, kit2, segments_idx, reference);
            }

            return segments_idx;
        }

        public static IList<ROHSegment> ROH(string kit, bool justUpdate)
        {
            IList<ROHSegment> segments_idx = new List<ROHSegment>();

            bool exists = GKSqlFuncs.ExistsROH(kit);

            if (exists && !justUpdate) {
                segments_idx = GKSqlFuncs.GetROHCmp(kit);

                foreach (var row in segments_idx) {
                    var rohSeg = GKSqlFuncs.GetROHSeg(kit, row.Chromosome, row.StartPosition, row.EndPosition);
                    row.Rows = rohSeg;
                }
            } else {
                var rows = GKSqlFuncs.GetAutosomal(kit);

                var tmp = new List<SNP>();

                byte prev_chr = 0;
                int start_pos = 0;
                int end_pos = 0;
                int prev_snp_count = 0;
                int no_call_counter = 0;
                int no_call_limit = RefData.Compare_NoCalls_Limit;
                foreach (var snp in rows) {
                    byte chromosome = snp.Chromosome;
                    int position = snp.Position;

                    if (prev_chr == 0)
                        prev_chr = chromosome;

                    if (prev_chr == chromosome) {
                        float errorRadius = (chromosome == (byte)Chromosome.CHR_X) ? RefData.Compare_X_Threshold_SNPs / 2 : RefData.Compare_Autosomal_Threshold_SNPs / 2;

                        var genotype = snp.Genotype;
                        char gt0 = genotype.A1;
                        char gt1 = genotype.A2;

                        bool gt0_unk = Genotype.IsEmptyOrUnknown(gt0);
                        bool gt1_unk = Genotype.IsEmptyOrUnknown(gt1);
                        if (gt1_unk) gt1 = gt0;

                        if (gt0 == gt1 && !gt0_unk) {
                            // match 
                            tmp.Add(snp);
                            if (start_pos == 0) start_pos = position;
                        } else if ((!gt0_unk && gt1_unk) || (!gt1_unk && gt0_unk)) {
                            no_call_counter++;
                            if (no_call_counter <= no_call_limit) {
                                tmp.Add(snp);
                                if (start_pos == 0) start_pos = position;
                            } else {
                                no_call_counter = 0;
                                // exceeded no call count.
                                prev_snp_count = 0;
                                DontMatchProcRoH(start_pos, end_pos, prev_chr, chromosome, segments_idx, ref tmp, false);
                                start_pos = position;
                            }
                        } else if (tmp.Count - prev_snp_count >= errorRadius) {
                            prev_snp_count = tmp.Count;
                            tmp.Add(snp);
                            if (start_pos == 0) start_pos = position;
                        } else {
                            // doesn't match on same chromosome
                            prev_snp_count = 0;
                            no_call_counter = 0;
                            DontMatchProcRoH(start_pos, end_pos, prev_chr, chromosome, segments_idx, ref tmp, false);
                            start_pos = position;
                        }
                    } else {
                        // next chromosome
                        prev_snp_count = 0;
                        no_call_counter = 0;
                        DontMatchProcRoH(start_pos, end_pos, prev_chr, chromosome, segments_idx, ref tmp, false);
                        start_pos = position;
                    }
                    end_pos = position;
                    prev_chr = chromosome;
                }

                rows.Clear();

                GKSqlFuncs.SaveROHCmp(kit, segments_idx);
            }
            return segments_idx;
        }

        public static void DoPhasing(IKitHost host, string fatherKit, string motherKit, string childKit, ref IList<PhaseRow> dt, bool chMale)
        {
            const char ZeroChar = Genotype.UnknownAllele;

            if (host != null) host.SetStatus("Phasing: data loading...");

            dt = GKSqlFuncs.GetPhaseRows(fatherKit, motherKit, childKit);

            if (host != null) host.SetStatus("Phasing: data processing...");

            for (int i = 0; i < dt.Count; i++) {
                var row = dt[i];
                var child = row.ChildGenotype;
                var father = row.PaternalGenotype;
                var mother = row.MaternalGenotype;

                child.CheckCompleteness();

                // check
                if ((!father.ContainsAny(child) || !mother.ContainsAny(child)) && row.Chromosome != (byte)Chromosome.CHR_X && !father.IsFullEmpty() && !mother.IsFullEmpty() && !child.IsFullEmpty()) {
                    row.Mutated = true;
                }

                bool amb = false;
                if (child.A1 != child.A2) {
                    if (father == child && mother.IsFullEmpty())
                        amb = true;
                    else if (mother == child && father.IsFullEmpty())
                        amb = true;
                    else if (father == child && mother == child)
                        amb = true;
                }

                if (amb) {
                    row.Ambiguous = true;
                    char nc = GetNucleotideCode(child.A1, child.A2);
                    row.PhasedPaternal = nc;
                    row.PhasedMaternal = nc;
                    continue;
                }

                if (child.IsFullEmpty() && father.A1 == father.A2 && father == mother && row.Chromosome != (byte)Chromosome.CHR_X) {
                    row.PhasedPaternal = father.A1;
                    row.PhasedMaternal = father.A1;
                    continue;
                }

                if (chMale && row.Chromosome == (byte)Chromosome.CHR_X) {
                    if (child.A1 == '-' && !mother.IsFullEmpty()) {
                        row.PhasedPaternal = ZeroChar;
                        row.PhasedMaternal = mother.A1;
                        continue;
                    }
                } else {
                    if (child.A1 == child.A2 && !Genotype.IsEmptyOrUnknown(child.A1)) {
                        row.PhasedPaternal = child.A1;
                        row.PhasedMaternal = child.A1;
                        continue;
                    }
                }

                // Women have two X chromosomes; men have one X and one Y chromosome. One X chromosome is inherited
                // from the mother, and the other (in women only) from the father. Although women have two X chromosomes,
                // in somatic cells one of them is inactivated and forms a Barr body.

                if (row.Chromosome == (byte)Chromosome.CHR_X) {
                    if (chMale) {
                        //row.ChildGenotype = child.A1.ToString(); // ?
                        //row.PaternalGenotype.Clear(); // ?
                        row.PhasedPaternal = ZeroChar;
                        row.PhasedMaternal = child.A1;
                    } else {
                        AutosomalSingleSNPPhase(child, father, mother, row);
                    }
                } else {
                    AutosomalSingleSNPPhase(child, father, mother, row);
                }

                if (row.PhasedPaternal == ZeroChar && row.PhasedMaternal != ZeroChar) {
                    row.PhasedPaternal = child.GetOther(row.PhasedMaternal);
                }

                if (row.PhasedMaternal == ZeroChar && row.PhasedPaternal != ZeroChar) {
                    row.PhasedMaternal = child.GetOther(row.PhasedPaternal);
                }
            }

            if (host != null) host.SetStatus("Phasing: data saving...");

            GKSqlFuncs.SavePhasedKit(fatherKit, motherKit, childKit, dt);
        }

        public static void AutosomalSingleSNPPhase(Genotype child, Genotype father, Genotype mother, PhaseRow row)
        {
            if (father.Contains(child.A1)) {
                row.PhasedPaternal = child.A1;
            }

            if (mother.Contains(child.A1)) {
                row.PhasedMaternal = child.A1;
            }

            if (father.Contains(child.A2)) {
                row.PhasedPaternal = child.A2;
            }

            if (mother.Contains(child.A2)) {
                row.PhasedMaternal = child.A2;
            }
        }

        public static void DoPhaseVisualizer(bool redoVisual, BackgroundWorker bw)
        {
            var phasedKits = GKSqlFuncs.GetPhasedKits();
            for (int i = 0; i < phasedKits.Count; i++) {
                string phased_kit = phasedKits[i];

                int percent = i * 100 / phasedKits.Count;
                if (bw != null) {
                    if (bw.CancellationPending) break;
                    bw.ReportProgress(percent, $"Phased Segments for kit #{phased_kit} ({GKSqlFuncs.GetKitName(phased_kit)}) - Processing ...");
                }

                var unphasedSegments = GKSqlFuncs.GetUnphasedSegments(phased_kit);

                foreach (var unphSeg in unphasedSegments) {
                    if (bw != null && bw.CancellationPending)
                        break;

                    string unphased_kit = unphSeg.UnphasedKit;
                    var chromosome = unphSeg.Chromosome;
                    int start_position = unphSeg.StartPosition;
                    int end_position = unphSeg.EndPosition;

                    var exists = GKSqlFuncs.HasUnphasedSegment(phased_kit, unphased_kit, chromosome, start_position, end_position);
                    if (exists) {
                        if (!redoVisual) {
                            if (bw != null)
                                bw.ReportProgress(percent, $"Segment [{GKSqlFuncs.GetKitName(phased_kit)}:{GKSqlFuncs.GetKitName(unphased_kit)}] Chr {chromosome}: {start_position}-{end_position}, Already Processed. Skipping ...");
                            continue;
                        } else {
                            GKSqlFuncs.DeletePhasedKit(phased_kit);
                        }
                    }

                    if (bw != null)
                        bw.ReportProgress(percent, $"Segment [{GKSqlFuncs.GetKitName(phased_kit)}:{GKSqlFuncs.GetKitName(unphased_kit)}] Chr {chromosome}: {start_position}-{end_position}, Processing ...");

                    /*var dt = GKSqlFuncs.GetPhaseSegments(phased_kit, unphased_kit, chromosome, start_position, end_position);
                    if (dt.Count > 0) {
                        if (bwPhaseVisualizer.CancellationPending)
                            break;

                        Image img = GGKGenFuncs.GetPhasedSegmentImage(dt, chromosome);
                    }*/
                }
            }
        }

        public static IList<string> FilterSNPsOnYTree(string kitSNPs)
        {
            var snpOnTree = RefData.SnpOnTree;

            string[] entered_snps = kitSNPs.Replace(" ", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var valid_snps = new List<string>();
            foreach (string s in entered_snps) {
                // extract string without ending sign (-)
                string es = s.Substring(0, s.Length - 1);
                if (snpOnTree.Contains(es)) {
                    valid_snps.Add(s);
                }
            }
            return valid_snps;
        }

        public static void ClearPhylogenyTree<T>(T pnNode, IList<T> nodesMap) where T : PhylogenyNode<T>
        {
            pnNode.Status = 0;
            foreach (var subnode in pnNode.Children) {
                ClearPhylogenyTree(subnode, nodesMap);
            }
            nodesMap.Add(pnNode);
        }

        public const int HGS_DG = 1;
        public const int HGS_LG = 2;
        public const int HGS_R = 3;

        #region Determine Y-DNA Haplogroup

        public static ISOGGYTreeNode FindYHaplogroup(ISOGGYTreeNode isoggYTree, IList<string> snpArray)
        {
            var nodesMap = new List<ISOGGYTreeNode>();
            ClearPhylogenyTree(isoggYTree, nodesMap);

            ISOGGYTreeNode hg_maxpath = null;
            foreach (var key in nodesMap) {
                string hg_snps = key.Markers;
                if (hg_snps.Equals("-")) continue;

                foreach (string hg_snp in hg_snps.Split(new char[] { ',', '/' })) {
                    var hg_snp_t = hg_snp.Trim();

                    foreach (string snp in snpArray) {
                        string snp_ss = snp.Substring(0, snp.Length - 1).Trim();
                        if (hg_snp_t != snp_ss) continue;

                        if (snp.EndsWith("-")) {
                            if (key.Status == HGS_DG) {
                                key.Status = HGS_LG;
                            } else if (key.Status != HGS_LG) {
                                key.Status = HGS_R;
                            }
                        } else if (snp.EndsWith("+")) {
                            if (key.Status == HGS_R) {
                                key.Status = HGS_LG;
                            } else if (key.Status != HGS_LG) {
                                key.Status = HGS_DG;
                            }

                            if (hg_maxpath == null) {
                                hg_maxpath = key;
                            } else if (key.Depth > hg_maxpath.Depth && key.Parent.Status != HGS_R) {
                                hg_maxpath = key;
                            }
                        }
                    }
                }
            }

            return hg_maxpath;
        }

        #endregion

        #region Determine Mt-DNA Haplogroup

        public static MtDNAPhylogenyNode FindMtHaplogroup(MtDNAPhylogenyNode mtTree, string kitMarkers, out string firstBest, out string secondBest)
        {
            var nodesMap = new List<MtDNAPhylogenyNode>();
            GKGenFuncs.ClearPhylogenyTree(mtTree, nodesMap);

            firstBest = string.Empty;
            secondBest = string.Empty;
            MtDNAPhylogenyNode name_maxpath = null;

            // dirty but ok for now
            kitMarkers = kitMarkers.Replace(",", " ").Replace("\t", " ").Replace("\r", " ").Replace("\n", " ");

            Regex regex = new Regex(@"[ ]{2,}", RegexOptions.None);
            kitMarkers = regex.Replace(kitMarkers, @" ");
            kitMarkers = kitMarkers.Replace(" ", ",");

            string[] marker_array = kitMarkers.Split(",".ToCharArray());

            var list = new List<MtDNAPhylogenyNode>();
            foreach (var key in nodesMap) {
                string marker_names = key.Markers;
                string[] marker_names_on_hg = marker_names.Split(",".ToCharArray());
                foreach (string marker_name in marker_names_on_hg) {
                    string m_name = marker_name.Replace("(", "").Replace(")", "").Replace("!", "").Trim();

                    foreach (string marker in marker_array) {
                        if (marker.Trim().IndexOf(m_name) != -1 || m_name == marker.Trim()) {
                            if (!list.Contains(key))
                                list.Add(key);

                            key.Status = HGS_DG;
                        }
                    }
                }
            }

            // go through all terminals with matches and count the number of matching parents. now, that's the score.
            var best_score = new SortedList<int, List<MtDNAPhylogenyNode>>();
            foreach (var key in list) {
                int pm = GetParentMatches(key);
                var alist = best_score.ContainsKey(pm) ? best_score[pm] : new List<MtDNAPhylogenyNode>();
                alist.Add(key);
                best_score.Remove(pm);
                best_score.Add(pm, alist);
            }
            var desc = best_score.Reverse();

            bool found_first = false;
            bool found_second = false;

            var sorted_hg_readjustment = new SortedDictionary<int, List<MtDNAPhylogenyNode>>();
            foreach (KeyValuePair<int, List<MtDNAPhylogenyNode>> kvp in desc) {
                var mlist = kvp.Value;
                string str = "";
                if (!found_first) {
                    foreach (var tn in mlist) {
                        if (IsMatchingAll(tn, marker_array, sorted_hg_readjustment)) {
                            name_maxpath = tn;
                            str = str + " " + tn.Name;
                            found_first = true;
                        }
                    }
                    firstBest = str.Trim().Replace(" ", ", ");
                    if (found_first)
                        continue;
                } else if (!found_second) {
                    foreach (var tn in mlist) {
                        if (IsMatchingAll(tn, marker_array, sorted_hg_readjustment)) {
                            //name_maxpath = tn;
                            str = str + " " + tn.Name;
                            found_second = true;
                        }
                    }
                    secondBest = str.Trim().Replace(" ", ", ");
                    if (found_second)
                        break;
                }
            }

            // --  final readjustment .. it's dirty
            found_first = false;
            found_second = false;
            string mstr = "";
            foreach (KeyValuePair<int, List<MtDNAPhylogenyNode>> hg in sorted_hg_readjustment) {
                var m_list = hg.Value;
                foreach (var mhg in m_list)
                    mstr = mstr + " " + mhg.Name;

                var mstr_tr = mstr.Trim().Replace(" ", ", ");

                if (!found_first) {
                    name_maxpath = m_list[0];
                    firstBest = mstr_tr;
                    found_first = true;
                    mstr = "";
                } else if (!found_second) {
                    secondBest = mstr_tr;
                    break;
                }
            }

            return name_maxpath;
        }

        private static bool IsMatchingAll(MtDNAPhylogenyNode key, string[] kitMarkers, SortedDictionary<int, List<MtDNAPhylogenyNode>> sorted_hg_readjustment)
        {
            var hgs = GetAllMatchingInParent(key);

            var hgs_found = new List<string>();
            var mismatches = new List<string>();
            var matches = new List<string>();

            foreach (string u_marker in kitMarkers) {
                bool ignore_found = false;
                foreach (string i_marker in RefData.MtIgnoreList) {
                    if (u_marker == i_marker || u_marker.StartsWith(i_marker) || u_marker.Substring(1).StartsWith(i_marker)) {
                        ignore_found = true;
                        break;
                    }
                }
                if (ignore_found) continue;

                //check u_marker in haplogroups
                bool found = false;
                string hg_found = "";
                foreach (string markers in hgs) {
                    string parent_markers = markers.Substring(markers.IndexOf(":") + 1);
                    string parent_hg = markers.Substring(0, markers.IndexOf(":"));
                    if (parent_markers.IndexOf(u_marker) != -1) {
                        found = true;
                        hg_found = markers;
                        break;
                    }
                }

                if (found) {
                    if (!hgs_found.Contains(hg_found))
                        hgs_found.Add(hg_found);

                    matches.Add(u_marker);
                } else
                    mismatches.Add(u_marker);
            }

            if (hgs.Count == hgs_found.Count) {
                var tnList = new List<MtDNAPhylogenyNode>();
                if (sorted_hg_readjustment.ContainsKey(mismatches.Count))
                    tnList = sorted_hg_readjustment[mismatches.Count];
                tnList.Add(key);
                sorted_hg_readjustment.Remove(mismatches.Count);
                sorted_hg_readjustment.Add(mismatches.Count, tnList);

                return true;
            } else
                return false;
        }

        private static List<string> GetAllMatchingInParent(MtDNAPhylogenyNode key)
        {
            var list = new List<string>();
            list.Add(key.Name + ":" + key.Markers);

            MtDNAPhylogenyNode parent = key.Parent;
            while (true) {
                if (parent.Name == "Eve")
                    break;
                if (parent.Status == HGS_DG) {
                    list.Add(parent.Name + ":" + parent.Markers);
                }
                parent = parent.Parent;
            }
            return list;
        }

        private static int GetParentMatches(MtDNAPhylogenyNode key)
        {
            int match = key.Status == HGS_DG ? 1 : 0;
            if (key.Parent != null)
                return match + GetParentMatches(key.Parent);
            else
                return match;
        }

        #endregion

        public static void GetMtDNA(string kit, out string mutations, out string fasta,
            SortedDictionary<int, List<string>> kitMutations,
            SortedDictionary<int, List<string>> kitInsertions)
        {
            GKSqlFuncs.GetMtDNA(kit, out mutations, out fasta);

            foreach (string mutation in mutations.Split(new char[] { ',' })) {
                string mut = mutation.Trim();
                string allele;
                List<string> alleles;

                if (mut.IndexOf(".") != -1) {
                    // insert
                    allele = mut[mut.Length - 1].ToString();
                    int pos = int.Parse(mut.Substring(0, mut.IndexOf(".")));

                    if (!kitInsertions.ContainsKey(pos))
                        alleles = new List<string>();
                    else
                        alleles = kitInsertions[pos];

                    alleles.Add(allele);
                    kitInsertions.Remove(pos);
                    kitInsertions.Add(pos, alleles);
                } else {
                    allele = mut[mut.Length - 1].ToString();
                    int pos = int.Parse(mut.Substring(1, mut.Length - 2));

                    if (!kitMutations.ContainsKey(pos))
                        alleles = new List<string>();
                    else
                        alleles = kitMutations[pos];

                    alleles.Add(allele);
                    kitMutations.Remove(pos);
                    kitMutations.Add(pos, alleles);
                }
            }
        }

        public static List<MtDNANucleotide> PopulateMtDnaNucleotides(int start, int end,
            SortedDictionary<int, List<string>> kitMutations,
            SortedDictionary<int, List<string>> kitInsertions)
        {
            int end_tmp = end;
            if (end < start)
                end_tmp = 16569; // Full mtDNA length

            var nucRes = new List<MtDNANucleotide>(end_tmp - start + 1);
            {
                var RSRS = RefData.RSRS;
                var nucleotides = new SortedList<int, MtDNANucleotide>(RSRS.Length);
                for (int i = 0; i < RSRS.Length; i++) {
                    var akey = i + 1;
                    nucleotides.Add(akey, new MtDNANucleotide(akey, RSRS[i].ToString(), RSRS[i].ToString()));
                }

                // Handles both normal sequences and circular wrap-around cases
                for (int i = start; i <= end_tmp; i++)
                    nucRes.Add(nucleotides[i]);

                if (end < start) {
                    for (int i = 1; i <= end; i++)
                        nucRes.Add(nucleotides[i]);
                }
            }

            // Create a lookup dictionary for faster access
            /*var mutationLookup = new Dictionary<int, string>(kitMutations.Count);
            foreach (var pair in kitMutations)
                mutationLookup[pair.Key] = pair.Value[0];

            Parallel.For(0, nucRes.Count, i =>
            {
                var nuc = nucRes[i];
                if (mutationLookup.TryGetValue(nuc.Pos, out string mutation)) {
                    nuc.Kit = mutation;
                    nuc.Mut = true;
                }
            });*/

            foreach (KeyValuePair<int, List<string>> a in kitMutations) {
                if ((a.Key >= start && a.Key <= end_tmp) || (end < start && a.Key <= end)) {
                    var nuc = nucRes.Find((x) => x.Pos == a.Key);
                    if (nuc != null) {
                        nuc.Kit = a.Value[0];
                        nuc.Mut = true;
                    }
                }
            }

            foreach (KeyValuePair<int, List<string>> a in kitInsertions) {
                if ((a.Key >= start && a.Key <= end_tmp) || (end < start && a.Key <= end)) {
                    var akey1 = (a.Key + 1);
                    var nucIdx = nucRes.FindIndex((x) => x.Pos == akey1);
                    if (nucIdx >= 0) {
                        foreach (string v in a.Value) {
                            nucRes.Insert(nucIdx, new MtDNANucleotide(a.Key, "", v, false, true));
                        }
                        break;
                    }
                }
            }

            return nucRes;
        }
    }
}

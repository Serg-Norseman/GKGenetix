/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using GenetixKit.Core.Model;

namespace GenetixKit.Core
{
    internal static class GKGenFuncs
    {
        public const int AUTOSOMAL_FTDNA = 0;
        public const int AUTOSOMAL_23ANDME = 1;
        public const int AUTOSOMAL_ANCESTRY = 2;
        public const int AUTOSOMAL_DECODEME = 3;
        public const int AUTOSOMAL_GENO2 = 4;

        private static readonly List<string> markers_new = new List<string>();


        public static double GetLength_in_cM(string chr, int start_pos, int end_pos)
        {
            return GetPosition_in_cM(chr, end_pos) - GetPosition_in_cM(chr, start_pos);
        }

        private static double GetPosition_in_cM(string chr, int pos)
        {
            double cm = 0.0;
            int chr_int = (chr == "X") ? 23 : int.Parse(chr);
            if (chr_int - 1 >= GKData.cM_Map.Length) return cm;

            SortedList<int, double> tmap = GKData.cM_Map[chr_int - 1];
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

        public static DNARec GetAutosomalDNAList(string file, BackgroundWorker bgw)
        {
            var rows = new List<SingleSNP>();

            char[] rsrs = GKData.RSRS;
            string[] lines;
            if (file.EndsWith(".gz")) {
                string tmp = Encoding.UTF8.GetString(GKUtils.GUnzip2Bytes(File.ReadAllBytes(file)));

                // ugly but required 
                tmp = tmp.Replace("\r\n", "\r");
                tmp = tmp.Replace("\n", "\r");
                tmp = tmp.Replace("\r\r", "\r");
                lines = tmp.Split("\r".ToCharArray());
            } else
                lines = File.ReadAllLines(file);

            int type = DetectDNAFileType(lines);

            if (type == -1) {
                GKUIFuncs.ShowMessage("Unable to identify file format for " + file);
                return new DNARec(new List<SingleSNP>(), new List<string>(), new List<string>());
            }

            string rsid = null;
            string chr = null;
            string pos = null;
            string genotype = null;
            List<string> ysnp = new List<string>();
            List<string> mtdna = new List<string>();

            foreach (string xline in lines) {
                string line = xline.Trim();
                if (line == "") continue;

                string[] data;

                if (type == AUTOSOMAL_FTDNA) {
                    if (line.StartsWith("RSID")) continue;

                    string tLine = line.Replace("\"", "");
                    data = tLine.Split(",".ToCharArray());
                    rsid = data[0];
                    chr = data[1];
                    pos = data[2];
                    genotype = data[3];
                }

                if (type == AUTOSOMAL_23ANDME) {
                    if (line.StartsWith("#")) continue;

                    data = line.Split("\t".ToCharArray());
                    rsid = data[0];
                    chr = data[1];
                    pos = data[2];
                    genotype = data[3];
                }

                if (type == AUTOSOMAL_ANCESTRY) {
                    if (line.StartsWith("#")) continue;
                    if (line.StartsWith("rsid\t")) continue;

                    data = line.Split("\t".ToCharArray());

                    rsid = data[0];
                    chr = data[1];
                    if (chr == "23")
                        chr = "X";
                    pos = data[2];
                    genotype = data[3] + data[4];
                }

                if (type == AUTOSOMAL_GENO2) {
                    if (line.StartsWith("SNP,")) continue;

                    data = line.Split(",".ToCharArray());
                    rsid = data[0];
                    chr = data[1];
                    pos = GetPosition(rsid);
                    genotype = data[2] + data[3];
                }

                if (type == AUTOSOMAL_DECODEME) {
                    if (line.StartsWith("Name,")) continue;

                    data = line.Split(",".ToCharArray());
                    rsid = data[0];
                    chr = data[2];
                    pos = data[3];
                    genotype = data[5];
                }

                if (chr != "Y" && chr != "MT") {
                    if (chr != "0")
                        rows.Add(new SingleSNP(rsid, chr, int.Parse(pos), genotype));
                } else {
                    if (chr == "Y") {
                        if (GKData.YMap.ContainsKey(pos)) {
                            string[] snp = GetYSNP(pos, genotype);
                            if (snp[0].IndexOf(";") == -1)
                                ysnp.Add(snp[0] + snp[1]);
                            else
                                ysnp.Add(snp[0].Substring(0, snp[0].IndexOf(";")) + snp[1]);
                        }
                    } else if (chr == "MT") {
                        if (rsrs[int.Parse(pos) - 1] != genotype[0] && genotype[0] != '-')
                            mtdna.Add(rsrs[int.Parse(pos) - 1].ToString() + pos + genotype);
                    }
                }
            }

            return new DNARec(rows, ysnp, mtdna);
        }

        public static string[] GetYSNP(string pos, string gt)
        {
            string[] data = GKData.YMap[pos];

            if (data[1].EndsWith("->" + gt))
                data[1] = "+";
            else
                data[1] = "-";

            return data;
        }

        private static int DetectDNAFileType(string[] lines)
        {
            int count = 0;
            foreach (string line in lines) {
                if (line == "RSID,CHROMOSOME,POSITION,RESULT")
                    return AUTOSOMAL_FTDNA;
                if (line == "# rsid\tchromosome\tposition\tgenotype")
                    return AUTOSOMAL_23ANDME;
                if (line == "rsid\tchromosome\tposition\tallele1\tallele2")
                    return AUTOSOMAL_ANCESTRY;
                if (line == "Name,Variation,Chromosome,Position,Strand,YourCode")
                    return AUTOSOMAL_DECODEME;
                if (line == "SNP,Chr,Allele1,Allele2")
                    return AUTOSOMAL_GENO2;
                /* if above doesn't work */
                if (line.Split("\t".ToCharArray()).Length == 4)
                    return AUTOSOMAL_23ANDME;
                if (line.Split("\t".ToCharArray()).Length == 5)
                    return AUTOSOMAL_ANCESTRY;
                if (line.Split(",".ToCharArray()).Length == 4)
                    return AUTOSOMAL_FTDNA;
                if (line.Split(",".ToCharArray()).Length == 6)
                    return AUTOSOMAL_DECODEME;
                if (count > 100) {
                    // detection useless... 
                    break;
                }
                count++;
            }
            return -1;
        }

        private static string GetPosition(string rsid)
        {
            return "0";
        }

        public static string GetMarkers(string file, string diff_work_dir)
        {
            string rsrs = new String(GKData.RSRS).ToUpper();
            string user = FastaSeq(file).ToUpper();
            rsrs = Regex.Replace(rsrs, "(.)", "$1\r\n");
            user = Regex.Replace(user, "(.)", "$1\r\n");
            File.WriteAllText(diff_work_dir + "rsrs.txt", rsrs);
            File.WriteAllText(diff_work_dir + "user.txt", user);

            File.WriteAllBytes(diff_work_dir + "diff.exe", Properties.Resources.diff);
            Process p = GKUtils.Execute(diff_work_dir + "rsrs.txt", diff_work_dir + "user.txt", diff_work_dir);
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
            return sb.ToString().Trim().Replace(" ", ", ");
        }

        private static string FastaSeq(string file)
        {
            StreamReader sr = new StreamReader(file);
            StringBuilder sb = new StringBuilder();
            string line = sr.ReadLine(); // skip first line
            while ((line = sr.ReadLine()) != null) {
                sb.Append(line);
            }
            sr.Close();
            return sb.ToString();
        }

        private static string ConvertInsDelToMod(List<string> markers, string file)
        {
            string rsrs = new String(GKData.RSRS).ToUpper();
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

            char f, u;
            var new_mut = new List<string>();
            foreach (var kvp in ht) {
                int start = kvp.Key;
                int end = kvp.Value;

                char[] fasta_char = rsrs.ToCharArray();
                char[] user_char = user.ToCharArray();
                for (int i = start; i <= end; i++) {
                    int offset;
                    if (i > 3107)
                        offset = 1;
                    else
                        offset = 0;
                    f = fasta_char[i - offset];
                    u = user_char[i];
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
            string diff_work_dir = Path.GetTempPath() + "Fasta2Rsrs\\";
            Directory.CreateDirectory(diff_work_dir);

            string txt = GetMarkers(fasta_file, diff_work_dir);

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
            Directory.Delete(diff_work_dir, true);
            return markers_str;
        }

        public static void DontMatchProc(int start_pos, int end_pos, string prev_chr, string chromosome, IList<CmpSegment> segments_idx, ref List<CmpSegmentRow> tmp, bool reference)
        {
            double cm_len, cm_th;
            int snp_th;
            bool overTh;

            int diffPos = (end_pos - start_pos);

            if (reference) {
                snp_th = GKSettings.Admixture_Threshold_SNPs;
                cm_th = GKSettings.Admixture_Threshold_cM;
                overTh = diffPos > 5000;
            } else {
                if (chromosome == "X") {
                    cm_th = GKSettings.Compare_X_Threshold_cM;
                    snp_th = GKSettings.Compare_X_Threshold_SNPs;
                } else {
                    cm_th = GKSettings.Compare_Autosomal_Threshold_cM;
                    snp_th = GKSettings.Compare_Autosomal_Threshold_SNPs;
                }
                overTh = diffPos / 1000000.0 > GKSettings.MB_THRESHOLD;
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

            tmp = new List<CmpSegmentRow>();
        }

        public static void DontMatchProcRoH(int start_pos, int end_pos, string prev_chr, string chromosome, IList<ROHSegment> segments_idx, ref List<SingleSNP> tmp, bool reference)
        {
            double cm_len, cm_th;
            int snp_th;
            bool overTh;

            int diffPos = (end_pos - start_pos);

            if (reference) {
                snp_th = GKSettings.Admixture_Threshold_SNPs;
                cm_th = GKSettings.Admixture_Threshold_cM;
                overTh = diffPos > 5000;
            } else {
                if (chromosome == "X") {
                    cm_th = GKSettings.Compare_X_Threshold_cM;
                    snp_th = GKSettings.Compare_X_Threshold_SNPs;
                } else {
                    cm_th = GKSettings.Compare_Autosomal_Threshold_cM;
                    snp_th = GKSettings.Compare_Autosomal_Threshold_SNPs;
                }
                overTh = diffPos / 1000000.0 > GKSettings.MB_THRESHOLD;
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

            tmp = new List<SingleSNP>();
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
        public static Image GetPhasedSegmentImage(IList<PhaseSegment> dt, string chromosome)
        {
            int paternal_error_position = 0;
            int maternal_error_position = 0;
            int snp_th;
            int errorRadius;
            if (chromosome == "X") {
                snp_th = GKSettings.Compare_X_Threshold_SNPs;
                errorRadius = snp_th / 2;
            } else {
                snp_th = GKSettings.Compare_Autosomal_Threshold_SNPs;
                errorRadius = snp_th / 2;
            }
            int no_call_limit = GKSettings.Compare_NoCalls_Limit;

            int paternal_no_call_count = 0;
            int maternal_no_call_count = 0;
            int x = 0;

            int width = 600;
            int height = 150;
            Image img = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(img);

            int begin_maternal_pos = 0;
            int begin_paternal_pos = 0;

            int curr_pos = 0;
            int val;
            for (int ix = 0; ix < dt.Count; ix++) {
                var row = dt[ix];
                curr_pos = ix;
                x = curr_pos * width / dt.Count;

                if (!IsPhasedMatch(row.Genotype, row.PaternalGenotype)) // paternal not matched
                {
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
                            Pen p1 = new Pen(Color.FromArgb(255, tmp, tmp, 255), 1);
                            for (int i = begin_paternal_pos * width / dt.Count; i < x; i++)
                                g.DrawLine(p1, i, 0, i, height / 2);
                        }

                        // don't allow but reset no call counter.
                        paternal_no_call_count = 0;
                        paternal_error_position = curr_pos;
                        begin_paternal_pos = curr_pos;
                    }
                }

                if (!IsPhasedMatch(row.Genotype, row.MaternalGenotype)) // maternal not matched
                {
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
                            Pen p1 = new Pen(Color.FromArgb(255, 255, tmp, tmp), 1);
                            for (int i = begin_maternal_pos * width / dt.Count; i < x; i++)
                                g.DrawLine(p1, i, height / 2, i, height);
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
                Pen p1 = new Pen(Color.FromArgb(255, tmp, tmp, 255), 1);
                for (int i = begin_paternal_pos * width / dt.Count; i < x; i++)
                    g.DrawLine(p1, i, 0, i, height / 2);
            }

            if (curr_pos - begin_maternal_pos > 5) {
                val = curr_pos - begin_maternal_pos;
                if (val > snp_th)
                    val = snp_th;
                int tmp = (snp_th - val) * 255 / snp_th;
                Pen p1 = new Pen(Color.FromArgb(255, 255, tmp, tmp), 1);
                for (int i = begin_maternal_pos * width / dt.Count; i < x; i++)
                    g.DrawLine(p1, i, height / 2, i, height);
            }

            g.Save();
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

                var tmp = new List<CmpSegmentRow>();

                string prev_chr = "";
                int start_pos = 0;
                int end_pos = 0;
                int prev_snp_count = 0;
                int no_call_counter = 0;
                int no_call_limit = GKSettings.Compare_NoCalls_Limit;

                foreach (var rd in otoRows) {
                    if (bwCompare != null && bwCompare.CancellationPending)
                        break;

                    string rsid = rd.RSID;
                    string chromosome = rd.Chromosome;
                    int position = rd.Position;
                    string gt1 = rd.Genotype1;
                    string gt2 = rd.Genotype2;
                    int cnt = rd.Count;

                    if (prev_chr == "") prev_chr = chromosome;
                    if (gt1.Length == 1) gt1 += gt1;
                    if (gt2.Length == 1) gt2 += gt2;

                    if (prev_chr == chromosome) {
                        float errorRadius = (chromosome == "X") ? GKSettings.Compare_X_Threshold_SNPs / 2 : GKSettings.Compare_Autosomal_Threshold_SNPs / 2;

                        if (cnt == 1) {
                            // match both alleles
                            tmp.Add(new CmpSegmentRow(rsid, chromosome, position, gt1, gt2, gt1));
                            if (start_pos == 0)
                                start_pos = position;
                        } else if (cnt == 2) {
                            // match 1 allele
                            if (gt1 == GKUtils.Reverse(gt2)) {
                                tmp.Add(new CmpSegmentRow(rsid, chromosome, position, gt1, gt2, gt1));
                                if (start_pos == 0)
                                    start_pos = position;
                            } else if (gt1[0] == gt2[0]) {
                                tmp.Add(new CmpSegmentRow(rsid, chromosome, position, gt1, gt2, gt2[0].ToString()));
                                if (start_pos == 0)
                                    start_pos = position;
                            } else if (gt1[0] == gt2[1]) {
                                tmp.Add(new CmpSegmentRow(rsid, chromosome, position, gt1, gt2, gt2[1].ToString()));
                                if (start_pos == 0)
                                    start_pos = position;
                            } else if (gt1[1] == gt2[0]) {
                                tmp.Add(new CmpSegmentRow(rsid, chromosome, position, gt1, gt2, gt2[0].ToString()));
                                if (start_pos == 0)
                                    start_pos = position;
                            } else if (gt1[1] == gt2[1]) {
                                tmp.Add(new CmpSegmentRow(rsid, chromosome, position, gt1, gt2, gt2[1].ToString()));
                                if (start_pos == 0)
                                    start_pos = position;
                            } else {
                                no_call_counter++;
                                if (no_call_counter > no_call_limit) {
                                    // no call exceeded..
                                    prev_snp_count = 0;
                                    no_call_counter = 0;
                                    DontMatchProc(start_pos, end_pos, prev_chr, chromosome, segments_idx, ref tmp, reference);
                                    start_pos = position;
                                } else if (gt1 == "--" || gt1 == "??" || gt1 == "00") {
                                    tmp.Add(new CmpSegmentRow(rsid, chromosome, position, gt1, gt2, "-"));
                                    if (start_pos == 0)
                                        start_pos = position;
                                } else if (gt2 == "--" || gt2 == "??" || gt2 == "00") {
                                    tmp.Add(new CmpSegmentRow(rsid, chromosome, position, gt1, gt2, "-"));
                                    if (start_pos == 0)
                                        start_pos = position;
                                } else if (gt1[0] == '-' || gt1[0] == '?' || gt1[0] == '0') {
                                    tmp.Add(new CmpSegmentRow(rsid, chromosome, position, gt1, gt2, "-"));
                                    if (start_pos == 0)
                                        start_pos = position;
                                } else if (gt1[1] == '-' || gt1[1] == '?' || gt1[1] == '0') {
                                    tmp.Add(new CmpSegmentRow(rsid, chromosome, position, gt1, gt2, "-"));
                                    if (start_pos == 0)
                                        start_pos = position;
                                } else if (gt2[0] == '-' || gt2[0] == '?' || gt2[0] == '0') {
                                    tmp.Add(new CmpSegmentRow(rsid, chromosome, position, gt1, gt2, "-"));
                                    if (start_pos == 0)
                                        start_pos = position;
                                } else if (gt2[1] == '-' || gt2[1] == '?' || gt2[1] == '0') {
                                    tmp.Add(new CmpSegmentRow(rsid, chromosome, position, gt1, gt2, "-"));
                                    if (start_pos == 0)
                                        start_pos = position;
                                } else if (tmp.Count - prev_snp_count >= errorRadius && no_call_counter <= no_call_limit) {
                                    prev_snp_count = tmp.Count;
                                    no_call_counter = 0;
                                    tmp.Add(new CmpSegmentRow(rsid, chromosome, position, gt1, gt2, ""));
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
                otoRows = null;

                //save
                GKSqlFuncs.SaveAutosomalCmp(kit1, kit2, segments_idx, reference);
            }

            return segments_idx;
        }

        public static IList<ROHSegment> ROH(string kit)
        {
            IList<ROHSegment> segments_idx = new List<ROHSegment>();

            bool exists = GKSqlFuncs.CheckROHExists(kit);

            if (exists) {
                segments_idx = GKSqlFuncs.GetROHCmp(kit);

                foreach (var row in segments_idx) {
                    var rohSeg = GKSqlFuncs.GetROHSeg(kit, row.Chromosome, row.StartPosition, row.EndPosition);
                    row.Rows = rohSeg;
                }
            } else {
                var rows = GKSqlFuncs.GetROHRows(kit);

                var tmp = new List<SingleSNP>();

                string prev_chr = "";
                int start_pos = 0;
                int end_pos = 0;
                int prev_snp_count = 0;
                int no_call_counter = 0;
                int no_call_limit = GKSettings.Compare_NoCalls_Limit;
                foreach (var snp in rows) {
                    string chromosome = snp.Chromosome;
                    int position = snp.Position;
                    string genotype = snp.Genotype;

                    if (genotype.Length == 1)
                        genotype += genotype;

                    if (prev_chr == "")
                        prev_chr = chromosome;

                    if (prev_chr == chromosome) {
                        float errorRadius;
                        if (chromosome == "X")
                            errorRadius = GKSettings.Compare_X_Threshold_SNPs / 2;
                        else
                            errorRadius = GKSettings.Compare_Autosomal_Threshold_SNPs / 2;

                        char gt0 = genotype[0];
                        char gt1 = genotype[1];

                        if (gt0 == gt1 && gt0 != '-' && gt0 != '?') {
                            // match 
                            tmp.Add(snp);
                            if (start_pos == 0)
                                start_pos = position;
                        } else if ((gt0 != '-' && gt0 != '?' && (gt1 == '-' || gt1 == '?')) || (gt1 != '-' && gt1 != '?' && (gt0 == '-' || gt0 == '?'))) {
                            no_call_counter++;
                            if (no_call_counter <= no_call_limit) {
                                tmp.Add(snp);
                                if (start_pos == 0)
                                    start_pos = position;
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
                            if (start_pos == 0)
                                start_pos = position;
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
                rows = null;

                GKSqlFuncs.SaveROHCmp(kit, segments_idx);
            }
            return segments_idx;
        }

        public static void DoPhasing(string fatherKit, string motherKit, string childKit, ref IList<PhaseRow> dt, bool male)
        {
            const char ZeroChar = '\0';

            dt = GKSqlFuncs.GetPhaseRows(fatherKit, motherKit, childKit);
            for (int ix = 0; ix < dt.Count; ix++) {
                var o = dt[ix];

                char phased_paternal = ZeroChar;
                char phased_maternal = ZeroChar;

                string child = o.ChildGenotype;
                string father = o.PaternalGenotype;
                string mother = o.MaternalGenotype;

                if (child.Length == 1)
                    child += child;

                // check
                if ((father.Replace(child[0].ToString(), "").Replace(child[1].ToString(), "") == father || mother.Replace(child[0].ToString(), "").Replace(child[1].ToString(), "") == mother)
                    && o.Chromosome != "X" && father != "--" && mother != "--" && child != "--") {
                    o.Mutated = true;
                }

                bool amb = false;
                if (father == child && child[0] != child[1] && mother == "--")
                    amb = true;
                else if (mother == child && child[0] != child[1] && father == "--")
                    amb = true;
                else if (father == child && child[0] != child[1] && mother == child)
                    amb = true;

                if (amb) {
                    o.Ambiguous = true;
                    char nc = GetNucleotideCode(child[0], child[1]);
                    o.PhasedPaternal = nc;
                    o.PhasedMaternal = nc;
                    continue;
                }

                if ((child == "--" || child == "??") && father[0] == father[1] && father == mother && o.Chromosome != "X") {
                    o.PhasedPaternal = father[0];
                    o.PhasedMaternal = father[0];
                    continue;
                }

                if (male && o.Chromosome == "X") {
                    child = child[0].ToString();
                    if (child == "-" && mother != "--") {
                        o.PhasedPaternal = ZeroChar;
                        o.PhasedMaternal = mother[0];
                        continue;
                    }
                } else {
                    if (child[0] == child[1] && child[0] != '-' && child[0] != '?') {
                        o.PhasedPaternal = child[0];
                        o.PhasedMaternal = child[0];
                        continue;
                    }
                }

                if (o.Chromosome != "X") {
                    AutosomalSingleSNPPhase(child, father, mother, o, ref phased_paternal, ref phased_maternal);
                } else if (o.Chromosome == "X") {
                    if (male) {
                        o.ChildGenotype = child[0].ToString();
                        o.PaternalGenotype = "";
                        o.PhasedPaternal = ZeroChar;
                        o.PhasedMaternal = child[0];
                        phased_paternal = ZeroChar;
                        phased_maternal = child[0];
                    } else {
                        AutosomalSingleSNPPhase(child, father, mother, o, ref phased_paternal, ref phased_maternal);
                    }
                }

                if (phased_paternal == ZeroChar && phased_maternal != ZeroChar) {
                    string ph_paternal = child.Replace("" + phased_maternal, "");
                    if (ph_paternal.Length > 0)
                        phased_paternal = ph_paternal[0];
                    o.PhasedPaternal = phased_paternal;
                }

                if (phased_maternal == ZeroChar && phased_paternal != ZeroChar) {
                    string ph_maternal = child.Replace("" + phased_paternal, "");
                    if (ph_maternal.Length > 0)
                        phased_maternal = ph_maternal[0];
                    o.PhasedMaternal = phased_maternal;
                }
            }

            Program.KitInstance.SetStatus("Saving Phased Kit " + childKit + " ...");
            GKSqlFuncs.SavePhasedKit(fatherKit, motherKit, childKit, dt);
        }

        public static void AutosomalSingleSNPPhase(string child, string father, string mother, PhaseRow row, ref char phased_paternal, ref char phased_maternal)
        {
            if (father.Contains(child[0])) {
                phased_paternal = child[0];
                row.PhasedPaternal = phased_paternal;
            }

            if (mother.Contains(child[0])) {
                phased_maternal = child[0];
                row.PhasedMaternal = phased_maternal;
            }

            if (father.Contains(child[1])) {
                phased_paternal = child[1];
                row.PhasedPaternal = phased_paternal;
            }

            if (mother.Contains(child[1])) {
                phased_maternal = child[1];
                row.PhasedMaternal = phased_maternal;
            }
        }
    }
}

/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace GenetixKit.Core
{
    internal static class GKGenFuncs
    {
        public const int AUTOSOMAL_FTDNA = 0;
        public const int AUTOSOMAL_23ANDME = 1;
        public const int AUTOSOMAL_ANCESTRY = 2;
        public const int AUTOSOMAL_DECODEME = 3;
        public const int AUTOSOMAL_GENO2 = 4;

        public const int EXPORT_ALL_GGK = 1;
        public const int EXPORT_AUTOSOMAL_FTDNA = 2;
        public const int EXPORT_AUTOSOMAL_23ANDME = 3;


        public const double MB_THRESHOLD = 0.5; // mandatory


        private static Dictionary<int, double>[] map = null;
        private static Dictionary<string, string[]> ymap = null;
        private static char[] RSRS = null;

        private static List<string> markers_new = new List<string>();

        public static double getLength_in_cM(string chr, int start_pos, int end_pos)
        {
            return getPosition_in_cM(chr, end_pos) - getPosition_in_cM(chr, start_pos);
        }

        public static double getPosition_in_cM(string chr, int pos)
        {
            double cm = 0.0;
            if (map == null)
                loadMap();
            int chr_int = -1;
            if (chr == "X")
                chr_int = 23;
            else
                chr_int = int.Parse(chr);

            Dictionary<int, double> tmap = map[chr_int - 1];

            if (!tmap.ContainsKey(pos)) {
                var prev_pos = from p in tmap.Keys where p <= pos select p;
                var next_pos = from p in tmap.Keys where p >= pos select p;

                int prev_pos_key = 0;
                int next_pos_key = 0;

                if (prev_pos.Count() == 0)
                    prev_pos_key = tmap.Keys.Min();
                else
                    prev_pos_key = prev_pos.Max();

                if (next_pos.Count() == 0)
                    next_pos_key = tmap.Keys.Max();
                else
                    next_pos_key = next_pos.Min();
                if (next_pos_key == prev_pos_key) {
                    if (next_pos_key < pos) {
                        prev_pos = from p in tmap.Keys where p < next_pos_key select p;
                        prev_pos_key = prev_pos.Max();

                        int diff = next_pos_key - prev_pos_key;
                        double cm_diff = tmap[next_pos_key] - tmap[prev_pos_key];
                        cm = tmap[next_pos_key] + cm_diff * (pos - next_pos_key) / diff;
                    } else {
                        next_pos = from p in tmap.Keys where p > prev_pos_key select p;
                        next_pos_key = next_pos.Max();

                        int diff = next_pos_key - prev_pos_key;
                        double cm_diff = tmap[next_pos_key] - tmap[prev_pos_key];
                        cm = tmap[prev_pos_key] - cm_diff * (prev_pos_key - pos) / diff;
                    }
                } else {
                    int diff = next_pos_key - prev_pos_key;
                    double cm_diff = tmap[next_pos_key] - tmap[prev_pos_key];
                    cm = tmap[prev_pos_key] + cm_diff * (pos - prev_pos_key) / diff;
                }
            } else
                cm = tmap[pos];
            return cm;
        }

        public static char[] getRSRS()
        {
            if (RSRS == null)
                RSRS = Encoding.ASCII.GetString(GKUtils.GUnzip(GenetixKit.Properties.Resources.RSRS)).ToCharArray();
            return RSRS;
        }

        public static Dictionary<string, string[]> getYMap()
        {
            if (ymap == null) {
                ymap = new Dictionary<string, string[]>();
                string csv = Encoding.UTF8.GetString(GKUtils.GUnzip(GenetixKit.Properties.Resources.ysnp_hg19));
                StringReader reader = new StringReader(csv);
                string l = null;
                string[] d = null;
                l = reader.ReadLine(); // header
                //snp;snp,pos,mutation
                while ((l = reader.ReadLine()) != null) {
                    d = l.Split(new char[] { ',' });
                    if (!ymap.ContainsKey(d[1]))
                        ymap.Add(d[1], new string[] { d[0], d[2] });
                }
            }
            return ymap;
        }

        public static Object[] getAutosomalDNAList(string file)
        {
            Object[] dnaout = new Object[3];
            ArrayList rows = new ArrayList();
            string[] lines = null;
            string tmp = null;
            char[] rsrs = getRSRS();
            if (file.EndsWith(".gz")) {
                tmp = Encoding.UTF8.GetString(GKUtils.GUnzip(File.ReadAllBytes(file)));
                // ugly but required 
                tmp = tmp.Replace("\r\n", "\r");
                tmp = tmp.Replace("\n", "\r");
                tmp = tmp.Replace("\r\r", "\r");
                lines = tmp.Split("\r".ToCharArray());
            } else
                lines = File.ReadAllLines(file);

            int type = detectDNAFileType(lines);

            if (type == -1) {
                MessageBox.Show("Unable to identify file format for " + file);
                dnaout[0] = new ArrayList();
                dnaout[1] = new List<string>();
                dnaout[2] = new ArrayList();
                return dnaout;
            }
            string[] data = null;
            string tLine = null;
            string rsid = null;
            string chr = null;
            string pos = null;
            string genotype = null;
            string[] snp = null;
            List<string> ysnp = new List<string>();
            ArrayList mtdna = new ArrayList();
            foreach (string line in lines) {
                //
                if (type == AUTOSOMAL_FTDNA) {
                    if (line.StartsWith("RSID"))
                        continue;
                    if (line.Trim() == "")
                        continue;
                    //
                    tLine = line.Replace("\"", "");
                    data = tLine.Split(",".ToCharArray());
                    rsid = data[0];
                    chr = data[1];
                    pos = data[2];
                    genotype = data[3];
                }
                if (type == AUTOSOMAL_23ANDME) {
                    if (line.StartsWith("#"))
                        continue;
                    if (line.Trim() == "")
                        continue;
                    //       
                    data = line.Split("\t".ToCharArray());
                    rsid = data[0];
                    chr = data[1];
                    pos = data[2];
                    genotype = data[3];
                }
                if (type == AUTOSOMAL_ANCESTRY) {
                    if (line.StartsWith("#"))
                        continue;
                    if (line.StartsWith("rsid\t"))
                        continue;
                    if (line.Trim() == "")
                        continue;
                    //            
                    data = line.Split("\t".ToCharArray());

                    rsid = data[0];
                    chr = data[1];
                    if (chr == "23")
                        chr = "X";
                    pos = data[2];
                    genotype = data[3] + data[4];
                }
                if (type == AUTOSOMAL_GENO2) {
                    if (line.StartsWith("SNP,"))
                        continue;
                    if (line.Trim() == "")
                        continue;
                    //            
                    data = line.Split(",".ToCharArray());

                    rsid = data[0];
                    chr = data[1];
                    pos = getPosition(rsid);
                    genotype = data[2] + data[3];
                }
                if (type == AUTOSOMAL_DECODEME) {
                    if (line.StartsWith("Name,"))
                        continue;
                    if (line.Trim() == "")
                        continue;
                    //            
                    data = line.Split(",".ToCharArray());

                    rsid = data[0];
                    chr = data[2];
                    pos = data[3];
                    genotype = data[5];
                }
                if (chr != "Y" && chr != "MT") {
                    if (chr != "0")
                        rows.Add(new string[] { rsid, chr, pos, genotype });
                } else {
                    //
                    if (chr == "Y") {
                        if (ymap.ContainsKey(pos)) {
                            snp = GKGenFuncs.getYSNP(pos, genotype);
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

            dnaout[0] = rows; // atdna
            dnaout[1] = ysnp; // ydna
            dnaout[2] = mtdna; // mtdna
            return dnaout;
        }

        public static string[] getYSNP(string pos, string gt)
        {
            string[] data = ymap[pos];

            if (data[1].EndsWith("->" + gt))
                data[1] = "+";
            else
                data[1] = "-";

            return data;
        }

        private static int detectDNAFileType(string[] lines)
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

        private static string getPosition(string rsid)
        {
            return "0";
        }

        public static string getMarkers(string file, string diff_work_dir)
        {
            string rsrs = new String(getRSRS()).ToUpper();
            string user = FastaSeq(file).ToUpper();
            rsrs = Regex.Replace(rsrs, "(.)", "$1\r\n");
            user = Regex.Replace(user, "(.)", "$1\r\n");
            File.WriteAllText(diff_work_dir + "rsrs.txt", rsrs);
            File.WriteAllText(diff_work_dir + "user.txt", user);

            File.WriteAllBytes(diff_work_dir + "diff.exe", GenetixKit.Properties.Resources.diff);
            Process p = GKUIFuncs.execute(diff_work_dir + "rsrs.txt", diff_work_dir + "user.txt", diff_work_dir);
            StringBuilder sb = new StringBuilder();
            string line = null;
            //int op = -1;
            string[] rsrs_pos = null;
            string[] user_pos = null;
            int count = 0;
            string[] rsrs_a = null;
            string[] user_a = null;
            while (!p.StandardOutput.EndOfStream) {
                line = p.StandardOutput.ReadLine();
                if (line.IndexOf("c") != -1) {
                    // change..
                    //op = OP_CHANGE;
                    rsrs_pos = line.Split(new char[] { 'c' })[0].Split(new char[] { ',' });
                    user_pos = line.Split(new char[] { 'c' })[1].Split(new char[] { ',' });
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
                    user_pos = line.Split(new char[] { 'a' })[1].Split(new char[] { ',' });
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
                    user_pos = line.Split(new char[] { 'd' })[1].Split(new char[] { ',' });
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
            string line = null;
            StringBuilder sb = new StringBuilder();
            line = sr.ReadLine();// ignore header
            while ((line = sr.ReadLine()) != null) {
                sb.Append(line);
            }
            sr.Close();
            return sb.ToString();
        }

        private static string convertInsDelToMod(List<string> markers, string file)
        {
            string rsrs = new String(getRSRS()).ToUpper();
            string user = FastaSeq(file).ToUpper();
            int start = 0;
            int end = 0;
            int start_paired = 0;
            Stack<int> stack = new Stack<int>();
            Hashtable ht = new Hashtable();
            foreach (string str in markers) {
                if (str.EndsWith("D")) {
                    start = int.Parse(str.Substring(1).Replace("D", ""));
                    stack.Push(start);
                } else if (str.Contains(".")) {
                    end = int.Parse(str.Substring(0, str.IndexOf(".")));
                    if (stack.Count != 0) {
                        start_paired = stack.Pop();
                        if (stack.Count == 0) {
                            ht.Add(start_paired, end);
                        }
                    }
                }
            }
            char f;
            char u;
            int offset = 0;
            List<string> new_mut = new List<string>();
            foreach (DictionaryEntry kvp in ht) {
                start = int.Parse(kvp.Key.ToString());
                end = int.Parse(kvp.Value.ToString());
                char[] fasta_char = rsrs.ToCharArray();
                char[] user_char = user.ToCharArray();
                for (int i = start; i <= end; i++) {
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
            //

            foreach (string str in markers) {
                foreach (DictionaryEntry kvp in ht) {
                    start = int.Parse(kvp.Key.ToString());
                    end = int.Parse(kvp.Value.ToString());
                    for (int i = start; i <= end; i++) {
                        if ((str.EndsWith(i + "D") && str.Length == (i + "D").Length + 1) || (str.IndexOf(i + ".") != -1 && str.Length == (i.ToString() + ".").Length + 2)) {
                            markers_new.Remove(str);
                        }
                    }
                }
            }

            //
            StringBuilder sb = new StringBuilder();
            foreach (string str in markers_new)
                sb.Append(str + " ");
            return sb.ToString().Trim().Replace(" ", ",");
        }

        public static String getMtDNAMarkers(string fasta_file)
        {
            string diff_work_dir = Path.GetTempPath() + "Fasta2Rsrs\\";
            Directory.CreateDirectory(diff_work_dir);


            string txt = getMarkers(fasta_file, diff_work_dir);

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

            string markers_str = convertInsDelToMod(markers, fasta_file);
            Directory.Delete(diff_work_dir, true);
            return markers_str;
        }

        public static void loadMap()
        {
            // required for cM calculation
            if (map == null)
                map = new Dictionary<int, double>[23];
            using (MemoryStream ms = new MemoryStream(GKUtils.GUnzip(GenetixKit.Properties.Resources.map_csv))) {
                StreamReader reader = new StreamReader(ms);
                string line = reader.ReadLine(); //header
                string[] data = null;
                int chr = -1;
                int pos = -1;
                double cm = 0.0;
                while ((line = reader.ReadLine()) != null) {
                    data = line.Split(new char[] { ',' });
                    chr = int.Parse(data[1]);
                    pos = int.Parse(data[2]);
                    cm = double.Parse(data[3]);
                    if (map[chr - 1] == null)
                        map[chr - 1] = new Dictionary<int, double>();
                    map[chr - 1].Add(pos, cm);
                }
                reader.Close();
            }
        }

        public static bool isPhasedMatch(string gt1, string gt2)
        {

            /*
                 R = A/G
                 Y = C/T
                 S = G/C
                 W = A/T
                 K = G/T
                 M = A/C  
             */
            string[] nc = new string[]{
                "R","AG",
                "Y","CT",
                "S","GC",
                "W","AT",
                "K","GT",
                "M","AC"};
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
    }
}

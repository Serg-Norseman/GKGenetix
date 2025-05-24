/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GenetixKit.Core
{
    internal static class GKData
    {
        public static readonly string[] ydna12 = new string[] { "DYS393", "DYS390", "DYS19", "DYS391", "DYS385", "DYS426", "DYS388", "DYS439", "DYS389I", "DYS392", "DYS389II" };
        public static readonly string[] ydna25 = new string[] { "DYS458", "DYS459", "DYS455", "DYS454", "DYS447", "DYS437", "DYS448", "DYS449", "DYS464" };
        public static readonly string[] ydna37 = new string[] { "DYS460", "Y-GATA-H4", "YCAII", "DYS456", "DYS607", "DYS576", "DYS570", "CDY", "DYS442", "DYS438" };
        public static readonly string[] ydna67 = new string[] { "DYS531", "DYS578", "DYF395S1", "DYS590", "DYS537", "DYS641", "DYS472", "DYF406S1", "DYS511", "DYS425", "DYS413", "DYS557", "DYS594", "DYS436", "DYS490", "DYS534", "DYS450", "DYS444", "DYS481", "DYS520", "DYS446", "DYS617", "DYS568", "DYS487", "DYS572", "DYS640", "DYS492", "DYS565" };
        public static readonly string[] ydna111 = new string[] { "DYS710", "DYS485", "DYS632", "DYS495", "DYS540", "DYS714", "DYS716", "DYS717", "DYS505", "DYS556", "DYS549", "DYS589", "DYS522", "DYS494", "DYS533", "DYS636", "DYS575", "DYS638", "DYS462", "DYS452", "DYS445", "Y-GATA-A10", "DYS463", "DYS441", "Y-GGAAT-1B07", "DYS525", "DYS712", "DYS593", "DYS650", "DYS532", "DYS715", "DYS504", "DYS513", "DYS561", "DYS552", "DYS726", "DYS635", "DYS587", "DYS643", "DYS497", "DYS510", "DYS434", "DYS461", "DYS435" };

        private static SortedList<int, double>[] cM_map = null;
        private static Dictionary<string, string[]> ymap = null;
        private static char[] rsrs = null;

        // required for cM calculation
        public static SortedList<int, double>[] cM_Map
        {
            get {
                if (cM_map == null) {
                    cM_map = new SortedList<int, double>[23];

                    using (var ms = GKUtils.GUnzip2Stream(Properties.Resources.map_csv)) {
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
                            cm = GKUtils.ParseFloat(data[3]);
                            if (cM_map[chr - 1] == null)
                                cM_map[chr - 1] = new SortedList<int, double>();
                            cM_map[chr - 1].Add(pos, cm);
                        }
                        reader.Close();
                    }
                }
                return cM_map;
            }
        }

        public static char[] RSRS
        {
            get {
                if (rsrs == null)
                    rsrs = Encoding.ASCII.GetString(GKUtils.GUnzip2Bytes(Properties.Resources.RSRS)).ToCharArray();

                return rsrs;
            }
        }

        public static Dictionary<string, string[]> YMap
        {
            get {
                if (ymap == null) {
                    ymap = new Dictionary<string, string[]>();

                    var stm = GKUtils.GUnzip2Stream(Properties.Resources.ysnp_hg19);
                    using (var reader = new StreamReader(stm, Encoding.UTF8)) {
                        string l = reader.ReadLine();
                        //snp;snp,pos,mutation
                        while ((l = reader.ReadLine()) != null) {
                            string[] d = l.Split(new char[] { ',' });
                            if (!ymap.ContainsKey(d[1]))
                                ymap.Add(d[1], new string[] { d[0], d[2] });
                        }
                    }
                }

                return ymap;
            }
        }
    }
}

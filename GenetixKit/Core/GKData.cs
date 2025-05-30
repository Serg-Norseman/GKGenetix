/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using GKGenetix.Core;
using GKGenetix.Core.Model;

namespace GenetixKit.Core
{
    internal static class GKData
    {
        #region Settings

        // This parameter is the cM threshold used when comparing autosomal data for matching purposes. Any matching segment below this threshold will be ignored.
        public static float Compare_Autosomal_Threshold_cM = 5.0f;

        // This parameter is the SNPs threshold used when comparing autosomal data for matching purposes. Any matching segment below this threshold will be ignored.
        public static int Compare_Autosomal_Threshold_SNPs = 500;

        // This parameter is the cM threshold used when comparing X-DNA data for matching purposes. Any matching segment below this threshold will be ignored.
        public static float Compare_X_Threshold_cM = 3.0f;

        // This parameter is the SNPs threshold used when comparing X-DNA data for matching purposes. Any matching segment below this threshold will be ignored.
        public static int Compare_X_Threshold_SNPs = 300;

        // This parameter is the cM threshold used for admixure calculations using compound segments. Any matching segment below this threshold will be ignored.
        public static float Admixture_Threshold_cM = 0.5f;

        // This parameter is the SNPs threshold used for admixure calculations using compound segments. Any matching segment below this threshold will be ignored.
        public static int Admixture_Threshold_SNPs = 100;

        // This parameter defines how many no-calls must be allowed in a matching segment. If the no-calls exceeds this limit in a segment, then the segment will not be matched.
        public static int Compare_NoCalls_Limit = 5;

        public const double MB_THRESHOLD = 0.5;

        #endregion


        public static readonly string[] ydna12 = new string[] { "DYS393", "DYS390", "DYS19", "DYS391", "DYS385", "DYS426", "DYS388", "DYS439", "DYS389I", "DYS392", "DYS389II" };
        public static readonly string[] ydna25 = new string[] { "DYS458", "DYS459", "DYS455", "DYS454", "DYS447", "DYS437", "DYS448", "DYS449", "DYS464" };
        public static readonly string[] ydna37 = new string[] { "DYS460", "Y-GATA-H4", "YCAII", "DYS456", "DYS607", "DYS576", "DYS570", "CDY", "DYS442", "DYS438" };
        public static readonly string[] ydna67 = new string[] { "DYS531", "DYS578", "DYF395S1", "DYS590", "DYS537", "DYS641", "DYS472", "DYF406S1", "DYS511", "DYS425", "DYS413", "DYS557", "DYS594", "DYS436", "DYS490", "DYS534", "DYS450", "DYS444", "DYS481", "DYS520", "DYS446", "DYS617", "DYS568", "DYS487", "DYS572", "DYS640", "DYS492", "DYS565" };
        public static readonly string[] ydna111 = new string[] { "DYS710", "DYS485", "DYS632", "DYS495", "DYS540", "DYS714", "DYS716", "DYS717", "DYS505", "DYS556", "DYS549", "DYS589", "DYS522", "DYS494", "DYS533", "DYS636", "DYS575", "DYS638", "DYS462", "DYS452", "DYS445", "Y-GATA-A10", "DYS463", "DYS441", "Y-GGAAT-1B07", "DYS525", "DYS712", "DYS593", "DYS650", "DYS532", "DYS715", "DYS504", "DYS513", "DYS561", "DYS552", "DYS726", "DYS635", "DYS587", "DYS643", "DYS497", "DYS510", "DYS434", "DYS461", "DYS435" };

        public static readonly string[] MtIgnoreList = new string[] { "309.1C", "315.1C", "515", "516", "517", "518", "519", "520", "521", "522", "16182C", "16183C", "16193.1C", "16519" };


        private static SortedList<int, double>[] cM_map = null;

        // required for cM calculation
        public static SortedList<int, double>[] cM_Map
        {
            get {
                if (cM_map == null) {
                    cM_map = new SortedList<int, double>[23];

                    using (var ms = Utilities.GUnzip2Stream(Properties.Resources.map_csv)) {
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
                            cm = Utilities.ParseFloat(data[3]);
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

        private static char[] rsrs = null;

        public static char[] RSRS
        {
            get {
                if (rsrs == null)
                    rsrs = Encoding.ASCII.GetString(Utilities.GUnzip2Bytes(Properties.Resources.RSRS)).ToCharArray();

                return rsrs;
            }
        }

        private static Dictionary<string, string[]> ymap = null;

        public static Dictionary<string, string[]> YMap
        {
            get {
                if (ymap == null) {
                    ymap = new Dictionary<string, string[]>();

                    var stm = Utilities.GUnzip2Stream(Properties.Resources.ysnp_hg19);
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

        private static List<MDMapRow> mtdna_map = null;

        public static List<MDMapRow> MtDnaMap
        {
            get {
                if (mtdna_map == null) {
                    mtdna_map = new List<MDMapRow>();

                    // Map Locus (0), Starting (1), Ending (2), bp Length (3), Shorthand (4), Description (5)
                    string csv = Properties.Resources.mtdna_map;
                    using (StreamReader reader = new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(csv)))) {
                        string line;
                        while ((line = reader.ReadLine()) != null) {
                            string[] data = line.Split(new char[] { ',' });
                            mtdna_map.Add(new MDMapRow(data[0], data[1], data[2], data[3], data[4], data[5]));
                        }
                    }
                }

                return mtdna_map;
            }
        }


        private static ISOGGYTreeNode isoggYTree = null;

        public static ISOGGYTreeNode ISOGGYTree
        {
            get {
                if (isoggYTree == null) {
                    XDocument doc = XDocument.Parse(Properties.Resources.ytree);

                    isoggYTree = new ISOGGYTreeNode(null, "Adam");
                    foreach (XElement el in doc.Root.Elements()) {
                        BuildYTree(isoggYTree, el);
                    }
                }

                return isoggYTree;
            }
        }

        private static void BuildYTree(ISOGGYTreeNode parent, XElement elmt)
        {
            ISOGGYTreeNode tn = null;

            XAttribute attrName = elmt.Attribute("name");
            XAttribute attrMarkers = elmt.Attribute("markers");

            if (attrName != null) {
                string pnName = attrName.Value.Trim();
                if (pnName.IndexOf(',') != -1)
                    pnName = Utilities.RemoveDuplicates(pnName);

                string pnMarkers = attrMarkers.Value.Trim();

                if (pnName != "") {
                    tn = new ISOGGYTreeNode(parent, pnName, pnMarkers);
                    parent.Children.Add(tn);
                }

                foreach (XElement el in elmt.Elements()) {
                    if (tn != null)
                        BuildYTree(tn, el);
                    else
                        BuildYTree(parent, el);
                }
            }
        }


        private static MtDNAPhylogenyNode mtTree = null;

        public static MtDNAPhylogenyNode MtTree
        {
            get {
                if (mtTree == null) {
                    XDocument doc = XDocument.Parse(Properties.Resources.mtDNAPhylogeny);

                    mtTree = new MtDNAPhylogenyNode(null, "Eve");
                    foreach (XElement el in doc.Root.Elements()) {
                        BuildMtTree(mtTree, el);
                    }
                }

                return mtTree;
            }
        }

        private static void BuildMtTree(MtDNAPhylogenyNode parent, XElement elmt)
        {
            MtDNAPhylogenyNode tn = null;

            XAttribute attrName = elmt.Attribute("Id");
            XAttribute attrMarkers = elmt.Attribute("HG");

            string pnName = attrName.Value.Trim();
            string pnMarkers = attrMarkers.Value.Trim();

            if (pnName != "") {
                tn = new MtDNAPhylogenyNode(parent, pnName, pnMarkers);
                parent.Children.Add(tn);
            }

            foreach (XElement el in elmt.Elements()) {
                if (tn != null)
                    BuildMtTree(tn, el);
                else
                    BuildMtTree(parent, el);
            }
        }

        private static List<string> snpOnTree = null;

        public static List<string> SnpOnTree
        {
            get {
                if (snpOnTree == null) {
                    snpOnTree = new List<string>();
                    snpOnTree.AddRange(Properties.Resources.snps_on_tree.Split(new char[] { ',' }));
                }
                return snpOnTree;
            }
        }
    }
}

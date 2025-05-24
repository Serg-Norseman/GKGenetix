/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

namespace GenetixKit.Core
{
    internal static class GKSettings
    {
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

        // The URL from which the latest mtDNA Phylogeny will be fetched.
        public static string Phylogeny_mtDNA_URL = "http://www.mtdnacommunity.org/downloads/mtDNAPhylogeny.xml";

        public const double MB_THRESHOLD = 0.5;
    }
}

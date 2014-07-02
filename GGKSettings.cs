using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace Genetic_Genealogy_Kit
{
    class GGKSettings
    {
     
        private static SortedDictionary<string, string[]> settings = null;

        public static SortedDictionary<string, string[]> getDefaultResetSettings()
        {
            SortedDictionary<string, string[]>  _s = new SortedDictionary<string, string[]>();
            _s.Add("GGK.Version", new string[] { "1.0", "This parameter shows the current version of the software. It is internally used to determine any database, software or data structure upgrades. This parameter cannot be changed and is readonly.", "1" });
            
            _s.Add("Compare.Autosomal.Threshold.cM", new string[] { "5.0", "This parameter is the cM threshold used when comparing autosomal data for matching purposes. Any matching segment below this threshold will be ignored.", "0" });
            _s.Add("Compare.Autosomal.Threshold.SNPs", new string[] { "500", "This parameter is the SNPs threshold used when comparing autosomal data for matching purposes. Any matching segment below this threshold will be ignored.", "0" });
            _s.Add("Compare.X.Threshold.cM", new string[] { "3.0", "This parameter is the cM threshold used when comparing X-DNA data for matching purposes. Any matching segment below this threshold will be ignored.", "0" });
            _s.Add("Compare.X.Threshold.SNPs", new string[] { "300", "This parameter is the SNPs threshold used when comparing X-DNA data for matching purposes. Any matching segment below this threshold will be ignored.", "0" });

            _s.Add("Admixture.Threshold.cM", new string[] { "0.5", "This parameter is the cM threshold used for admixure calculations using compound segments. Any matching segment below this threshold will be ignored.", "1" });
            _s.Add("Admixture.Threshold.SNPs", new string[] { "100", "This parameter is the SNPs threshold used for admixure calculations using compound segments. Any matching segment below this threshold will be ignored.", "1" });

            _s.Add("Admixture.ReferencePopulations.Hide", new string[] { "1", "This parameter hides the reference population autosomal data from being opened. If you wish to open and export, you can change this parameter to 0.", "0" });

            _s.Add("Compare.NoCalls.Limit", new string[] { "5", "This parameter defines how many no-calls must be allowed in a matching segment. If the no-calls exceeds this limit in a segment, then the segment will not be matched.", "0" });

            _s.Add("Phylogeny.mtDNA.URL", new string[] { "http://www.mtdnacommunity.org/downloads/mtDNAPhylogeny.xml", "The URL from which the latest mtDNA Phylogeny will be fetched.", "1" });
            
            return _s;
        }

        public static SortedDictionary<string, string[]> getSettings()
        {
            if (settings == null)
                loadSettings();
            return settings;
        }

        public static void loadSettings(bool force_reload)
        {
            if (force_reload)
                settings = null;
            loadSettings();
        }

        public static void loadSettings()
        {
            if (settings == null)
            {
                settings = new SortedDictionary<string, string[]>();
                DataTable dt_settings = GGKUtilLib.queryDatabase("ggk_settings", new string[] {"key","value","description","readonly","last_modified" });
                object[] data = null;
                foreach(DataRow row in dt_settings.Rows)
                {
                    data = row.ItemArray;
                    settings.Add(data[0].ToString(), new string[]{data[1].ToString(),data[2].ToString(),data[3].ToString(),data[4].ToString()});
                }
            }
        }

        public static void saveParameterValue(string key,string value)
        {
            SQLiteConnection cnn = GGKUtilLib.getDBConnection();
            SQLiteCommand upCmd = new SQLiteCommand(@"UPDATE ggk_settings set value=@value WHERE key=@key", cnn);
            upCmd.Parameters.AddWithValue("@value", value);
            upCmd.Parameters.AddWithValue("@key", key);
            upCmd.ExecuteNonQuery();
            upCmd.Dispose();
            
            //
            settings[key][0] = value;
        }

        public static string getParameterValue(string key)
        {
            if (settings == null)
                loadSettings();
            return settings[key][0];
        }

        public static void _addParameter(string key, string value,string desc,string read_only)
        {
            SQLiteConnection cnn = GGKUtilLib.getDBConnection();
            SQLiteCommand upCmd = new SQLiteCommand(@"INSERT into ggk_settings(key,value,description,readonly) VALUES (@key,@value,@desc,@readonly)", cnn);
            upCmd.Parameters.AddWithValue("@key", key);
            upCmd.Parameters.AddWithValue("@value", value);
            upCmd.Parameters.AddWithValue("@desc", desc);
            upCmd.Parameters.AddWithValue("@readonly", read_only);            
            upCmd.ExecuteNonQuery();
            upCmd.Dispose();
            
        }
    }
}

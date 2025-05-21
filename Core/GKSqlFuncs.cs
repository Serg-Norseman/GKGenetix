/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace GenetixKit.Core
{
    class GKSqlFuncs
    {
        public static string SQLITE_DB = @"ggk.db";

        private static Dictionary<string, string> kit_name = null;

        private const string TABLE_kit_master_CREATE_SQL = "CREATE TABLE [kit_master] ([kit_no] TEXT NOT NULL, [name] TEXT NOT NULL, [sex] CHAR NOT NULL DEFAULT U, [disabled] INTEGER NOT NULL DEFAULT 0, [reference] INTEGER NOT NULL DEFAULT 0, [x] INTEGER DEFAULT 0, [y] INTEGER DEFAULT 0,  [roh_status] INTEGER NOT NULL DEFAULT 0,  [last_modified] DATETIME DEFAULT CURRENT_TIMESTAMP, CONSTRAINT [sqlite_autoindex_kit_master_1] PRIMARY KEY ([kit_no]))";
        private const string TABLE_kit_autosomal_CREATE_SQL = "CREATE TABLE [kit_autosomal] ([kit_no] TEXT REFERENCES [kit_master]([kit_no]) ON DELETE CASCADE ON UPDATE CASCADE,[rsid] TEXT NOT NULL,[chromosome] TEXT NOT NULL,[position] INTEGER NOT NULL,[genotype] TEXT NOT NULL,CONSTRAINT [sqlite_autoindex_kit_autosomal_1] PRIMARY KEY ([kit_no], [rsid]))";
        private const string TABLE_kit_mtdna_CREATE_SQL = "CREATE TABLE [kit_mtdna] ([kit_no] TEXT NOT NULL REFERENCES [kit_master]([kit_no]) ON DELETE CASCADE ON UPDATE CASCADE, [mutations] TEXT NOT NULL, [fasta] TEXT, CONSTRAINT [sqlite_autoindex_kit_mtdna_1] PRIMARY KEY ([kit_no]))";
        private const string TABLE_kit_ysnps_CREATE_SQL = "CREATE TABLE [kit_ysnps] ([kit_no] TEXT NOT NULL REFERENCES [kit_master]([kit_no]) ON DELETE CASCADE ON UPDATE CASCADE, [ysnps] TEXT NOT NULL, CONSTRAINT [] PRIMARY KEY ([kit_no]))";
        private const string TABLE_kit_ystr_CREATE_SQL = "CREATE TABLE [kit_ystr] ([kit_no] TEXT NOT NULL REFERENCES [kit_master]([kit_no]) ON DELETE CASCADE ON UPDATE CASCADE,[marker] TEXT NOT NULL,[value] TEXT NOT NULL,CONSTRAINT [sqlite_autoindex_kit_ystr_1] PRIMARY KEY ([kit_no], [marker]))";
        private const string TABLE_ggk_settings_CREATE_SQL = "CREATE TABLE [ggk_settings] ([key] TEXT NOT NULL, [value] TEXT NOT NULL, [description] TEXT NOT NULL, [readonly] INT NOT NULL DEFAULT 0, [last_modified] DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP, CONSTRAINT [sqlite_autoindex_ggk_settings_1] PRIMARY KEY ([key]))";
        private const string TABLE_cmp_autosomal_CREATE_SQL = "CREATE TABLE [cmp_autosomal] ([segment_id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,[cmp_id] INTEGER NOT NULL REFERENCES [cmp_status]([cmp_id]) ON DELETE CASCADE ON UPDATE CASCADE, [kit1] TEXT NOT NULL REFERENCES [kit_master]([kit_no]) ON DELETE CASCADE ON UPDATE CASCADE, [kit2] TEXT NOT NULL REFERENCES [kit_master]([kit_no]) ON DELETE CASCADE ON UPDATE CASCADE, [chromosome] TEXT NOT NULL, [start_position] INT NOT NULL, [end_position] INT NOT NULL,[segment_length_cm] DOUBLE NOT NULL,[snp_count] INT NOT NULL,[segment_type] CHAR NOT NULL DEFAULT U, [last_modified] DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP)";
        private const string TABLE_cmp_mrca_CREATE_SQL = "CREATE TABLE [cmp_mrca] ([rsid] TEXT NOT NULL, [chromosome] TEXT NOT NULL, [position] INTEGER NOT NULL, [kit1_genotype] TEXT, [kit2_genotype] TEXT, [match] TEXT, [segment_id] INTEGER NOT NULL REFERENCES [cmp_autosomal]([segment_id]) ON DELETE CASCADE ON UPDATE CASCADE)";
        private const string TABLE_cmp_status_CREATE_SQL = "CREATE TABLE [cmp_status] ([cmp_id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, [kit1] TEXT NOT NULL REFERENCES [kit_master]([kit_no]) ON DELETE CASCADE ON UPDATE CASCADE, [kit2] TEXT NOT NULL REFERENCES [kit_master]([kit_no]) ON DELETE CASCADE ON UPDATE CASCADE, [status_autosomal] INTEGER NOT NULL DEFAULT 0, [status_ysnp] INTEGER NOT NULL DEFAULT 0, [status_ystr] INTEGER NOT NULL DEFAULT 0, [status_mtdna] INTEGER NOT NULL DEFAULT 0, [at_longest] DOUBLE NOT NULL DEFAULT (0.0), [at_total] DOUBLE NOT NULL DEFAULT (0.0), [x_longest] DOUBLE NOT NULL DEFAULT (0.0), [x_total] DOUBLE NOT NULL DEFAULT (0.0), [mrca] INTEGER NOT NULL DEFAULT 0, [mt_haplogroup] TEXT, [y_haplogroup] TEXT, [last_processed] DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP)";
        private const string TABLE_kit_roh_CREATE_SQL = "CREATE TABLE [kit_roh] ([kit_no] TEXT NOT NULL REFERENCES [kit_master]([kit_no]) ON DELETE CASCADE ON UPDATE CASCADE, [chromosome] TEXT NOT NULL, [start_position] INTEGER NOT NULL, [end_position] INTEGER NOT NULL, [segment_length_cm] DOUBLE NOT NULL DEFAULT (0.0), [snp_count] INTEGER NOT NULL)";
        private const string TABLE_kit_phased_CREATE_SQL = "CREATE TABLE [kit_phased] ([kit_no] TEXT REFERENCES [kit_master]([kit_no]) ON DELETE CASCADE ON UPDATE CASCADE, [rsid] TEXT NOT NULL, [chromosome] TEXT NOT NULL, [position] INTEGER NOT NULL, [paternal_genotype] TEXT NOT NULL, [maternal_genotype] TEXT NOT NULL, [paternal_kit_no] TEXT, [maternal_kit_no] TEXT, CONSTRAINT [sqlite_autoindex_kit_phased_1] PRIMARY KEY ([kit_no], [rsid]))";
        private const string TABLE_cmp_phased_CREATE_SQL = "CREATE TABLE [cmp_phased] ([phased_kit] TEXT NOT NULL REFERENCES [kit_master]([kit_no]) ON DELETE CASCADE ON UPDATE CASCADE, [match_kit] TEXT NOT NULL, [chromosome] TEXT NOT NULL, [start_position] INTEGER NOT NULL, [end_position] INTEGER NOT NULL, [segment_image] BLOB, [segment_xml] TEXT)";

        private static string[] CreateTableSQL = new string[]{
            "kit_master",TABLE_kit_master_CREATE_SQL,
            "kit_autosomal",TABLE_kit_autosomal_CREATE_SQL,
            "kit_mtdna",TABLE_kit_mtdna_CREATE_SQL,
            "kit_ysnps",TABLE_kit_ysnps_CREATE_SQL,
            "kit_ystr",TABLE_kit_ystr_CREATE_SQL,
            "ggk_settings",TABLE_ggk_settings_CREATE_SQL,
            "cmp_autosomal",TABLE_cmp_autosomal_CREATE_SQL,
            "cmp_mrca",TABLE_cmp_mrca_CREATE_SQL,
            "cmp_status",TABLE_cmp_status_CREATE_SQL,
            "kit_roh",TABLE_kit_roh_CREATE_SQL,
            "kit_phased",TABLE_kit_phased_CREATE_SQL,
            "cmp_phased",TABLE_cmp_phased_CREATE_SQL
        };

        public static void FactoryReset()
        {
            if (File.Exists(SQLITE_DB))
                File.Move(SQLITE_DB, SQLITE_DB + "-" + DateTime.Now.Ticks.ToString("X"));
            SQLiteConnection connection = new SQLiteConnection(@"Data Source=" + GKSqlFuncs.SQLITE_DB + @";Version=3; Compress=True; New=True; PRAGMA foreign_keys = ON; PRAGMA auto_vacuum = FULL;");
            connection.Open();
            Dictionary<string, string> pragma = new Dictionary<string, string>();

            pragma.Add("foreign_keys", "ON");
            pragma.Add("auto_vacuum", "FULL");
            using (SQLiteTransaction trans = connection.BeginTransaction()) {
                SQLiteCommand ss2 = null;
                foreach (string key in pragma.Keys) {
                    ss2 = new SQLiteCommand("PRAGMA " + key + " = " + pragma[key] + ";", connection);
                    ss2.ExecuteNonQuery();
                }
                // ---
                for (int idx = 0; idx < CreateTableSQL.Length; idx += 2) {
                    ss2 = new SQLiteCommand(CreateTableSQL[idx + 1], connection);
                    ss2.ExecuteNonQuery();
                }
                insertDefaultSettings(connection);
                trans.Commit();
            }
            connection.Close();
        }

        public static SQLiteConnection getDBConnection()
        {
            if (File.Exists(GKSqlFuncs.SQLITE_DB)) {
                SQLiteConnection connection = new SQLiteConnection(@"Data Source=" + GKSqlFuncs.SQLITE_DB + @";Version=3; Compress=True; PRAGMA foreign_keys = ON; PRAGMA auto_vacuum = FULL;");
                connection.Open();
                Dictionary<string, string> pragma = new Dictionary<string, string>();

                pragma.Add("foreign_keys", "ON");
                pragma.Add("auto_vacuum", "FULL");
                SQLiteCommand ss2 = null;
                foreach (string key in pragma.Keys) {
                    ss2 = new SQLiteCommand("PRAGMA " + key + " = " + pragma[key] + ";", connection);
                    ss2.ExecuteNonQuery();
                }

                return connection;
            } else {
                //FactoryReset();
                //return getDBConnection();
                if (MessageBox.Show("Data file ggk.db doesn't exist. If this is the first time you are opening the software, you can ignore this error. Do you wish to create one? ", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes) {
                    FactoryReset();
                    return getDBConnection();
                }
                Application.Exit();
            }
            return null;
        }

        public static void integrityCheckAndFix()
        {
            SQLiteConnection conn = getDBConnection();
            SQLiteCommand ss = new SQLiteCommand("select tbl_name from sqlite_master where type='table'", conn);
            SQLiteDataReader reader = ss.ExecuteReader();
            ArrayList list = new ArrayList();
            while (reader.Read())
                list.Add(reader["tbl_name"]);
            reader.Close();
            ss.Dispose();
            for (int idx = 0; idx < CreateTableSQL.Length; idx += 2) {
                if (!list.Contains(CreateTableSQL[idx])) {
                    ss = new SQLiteCommand(CreateTableSQL[idx + 1], conn);
                    ss.ExecuteNonQuery();
                    ss.Dispose();
                    if (CreateTableSQL[idx] == "ggk_settings") {
                        insertDefaultSettings(conn);
                    }
                }
            }
            conn.Close();
            if (list.Contains("ggk_settings")) {
                GKSettings.loadSettings();
                SortedDictionary<string, string[]> db_settings = GKSettings.getSettings();
                SortedDictionary<string, string[]> upgrade_settings = GKSettings.getDefaultResetSettings();
                foreach (string key in upgrade_settings.Keys) {
                    if (!db_settings.ContainsKey(key)) {
                        // insert into db...
                        GKSettings._addParameter(key, upgrade_settings[key][0], upgrade_settings[key][1], upgrade_settings[key][2]);
                    }
                }
            }

            GKSettings.loadSettings(true);
        }


        public static DataTable queryDatabase(string table, string[] fields)
        {
            return queryDatabase(table, fields, "");
        }

        public static DataTable queryDatabase(string table, string[] fields, string conditions)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string field in fields)
                sb.Append(field + " ");
            string fields_list = sb.ToString().Trim().Replace(' ', ',');
            SQLiteConnection conn = getDBConnection();
            SQLiteCommand ss = new SQLiteCommand("select " + fields_list + " from " + table + " " + conditions, conn);
            SQLiteDataReader reader = ss.ExecuteReader();
            DataTable dt = new DataTable(table);
            dt.Load(reader);
            reader.Close();
            conn.Close();
            return dt;
        }

        public static string queryValue(string table, string[] fields, string conditions)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string field in fields)
                sb.Append(field + " ");
            string fields_list = sb.ToString().Trim().Replace(' ', ',');
            SQLiteConnection conn = getDBConnection();
            SQLiteCommand ss = new SQLiteCommand("select " + fields_list + " from " + table + " " + conditions, conn);
            SQLiteDataReader reader = ss.ExecuteReader();
            DataTable dt = new DataTable(table);
            dt.Load(reader);
            reader.Close();
            conn.Close();
            if (dt.Rows.Count > 0) {
                if (dt.Rows[0].ItemArray.Length > 0)
                    return dt.Rows[0].ItemArray[0].ToString();
            }
            return "";
        }

        public static DataTable QueryDB(string sql)
        {
            SQLiteConnection conn = getDBConnection();
            SQLiteCommand ss = new SQLiteCommand(sql, conn);
            SQLiteDataReader reader = ss.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            reader.Close();
            conn.Close();
            return dt;
        }

        public static int UpdateDB(string sql)
        {
            SQLiteConnection conn = getDBConnection();
            SQLiteCommand ss = new SQLiteCommand(sql, conn);
            int output = ss.ExecuteNonQuery();
            conn.Close();
            return output;
        }

        public static DataTable queryDatabase(SQLiteConnection conn, string table, string[] fields, string conditions)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string field in fields)
                sb.Append(field + " ");
            string fields_list = sb.ToString().Trim().Replace(' ', ',');
            SQLiteCommand ss = new SQLiteCommand("select " + fields_list + " from " + table + " " + conditions, conn);
            SQLiteDataReader reader = ss.ExecuteReader();
            DataTable dt = new DataTable(table);
            dt.Load(reader);
            reader.Close();
            return dt;
        }

        private static void insertDefaultSettings(SQLiteConnection cnn)
        {
            SortedDictionary<string, string[]> settings = GKSettings.getDefaultResetSettings();
            SQLiteCommand upCmd = null;
            using (var transaction = cnn.BeginTransaction()) {
                foreach (string key in settings.Keys) {
                    upCmd = new SQLiteCommand(@"INSERT OR REPLACE INTO ggk_settings(key, value,description,readonly)values(@key,@value,@desc,@readonly)", cnn);
                    upCmd.Parameters.AddWithValue("@key", key);
                    upCmd.Parameters.AddWithValue("@value", settings[key][0]);
                    upCmd.Parameters.AddWithValue("@desc", settings[key][1]);
                    upCmd.Parameters.AddWithValue("@readonly", settings[key][2]);
                    upCmd.ExecuteNonQuery();
                }
                transaction.Commit();
            }
        }

        public static void clearAllComparisions(bool exclue_references)
        {
            SQLiteConnection cnn = getDBConnection();
            SQLiteCommand upCmd = null;
            if (exclue_references)
                upCmd = new SQLiteCommand(@"DELETE from cmp_status where kit1 in (select kit_no from kit_master where reference=0) and  kit2 in (select kit_no from kit_master where reference=0) ", cnn);
            else
                upCmd = new SQLiteCommand(@"DELETE FROM cmp_status", cnn);
            upCmd.ExecuteNonQuery();
            cnn.Close();
        }

        private static void addAutosomalCmp(string kit1, string kit2, DataTable segment_idx, List<DataTable> segments, bool reference)
        {
            SQLiteConnection cnn = getDBConnection();
            //
            SQLiteCommand upCmd = new SQLiteCommand(@"DELETE from cmp_status WHERE (kit1=@kit1 AND kit2=@kit2) OR (kit1=@kit3 AND kit2=@kit4)", cnn);
            upCmd.Parameters.AddWithValue("@kit1", kit1);
            upCmd.Parameters.AddWithValue("@kit2", kit2);
            upCmd.Parameters.AddWithValue("@kit3", kit2);
            upCmd.Parameters.AddWithValue("@kit4", kit1);
            upCmd.ExecuteNonQuery();
            //
            double total = 0;
            double longest = 0;
            double x_total = 0;
            double x_longest = 0;
            int mrca = 0;
            //
            object[] obj = null;
            double seg_len = 0;
            foreach (DataRow row in segment_idx.Rows) {
                obj = row.ItemArray;
                seg_len = double.Parse(obj[3].ToString());
                if (obj[0].ToString() == "X") {
                    x_total += seg_len;
                    if (x_longest < seg_len)
                        x_longest = seg_len;
                } else {
                    total += seg_len;
                    if (longest < seg_len)
                        longest = seg_len;
                }
            }
            double shared = 0;
            double range_begin = 0;
            double range_end = 0;
            for (int gen = 0; gen < 10; gen++) {
                shared = 3600 / Math.Pow(2, gen);
                range_begin = shared - shared / 4;
                range_end = shared + shared / 4;
                if (total < range_end && total > range_begin)
                    mrca = gen + 1;
            }
            //
            upCmd = new SQLiteCommand(@"INSERT OR REPLACE INTO cmp_status (kit1,kit2,status_autosomal,at_longest,at_total,x_longest,x_total,mrca) VALUES (@kit1,@kit2,@status_autosomal,@at_longest,@at_total,@x_longest,@x_total,@mrca)", cnn);
            upCmd.Parameters.AddWithValue("@kit1", kit1);
            upCmd.Parameters.AddWithValue("@kit2", kit2);
            upCmd.Parameters.AddWithValue("@status_autosomal", "1");
            upCmd.Parameters.AddWithValue("@at_longest", longest);
            upCmd.Parameters.AddWithValue("@at_total", total);
            upCmd.Parameters.AddWithValue("@x_longest", x_longest);
            upCmd.Parameters.AddWithValue("@x_total", x_total);
            upCmd.Parameters.AddWithValue("@mrca", mrca);
            upCmd.ExecuteNonQuery();
            //

            upCmd = new SQLiteCommand(@"SELECT cmp_id FROM cmp_status WHERE kit1=@kit1 AND kit2=@kit2", cnn);
            upCmd.Parameters.AddWithValue("@kit1", kit1);
            upCmd.Parameters.AddWithValue("@kit2", kit2);
            SQLiteDataReader reader = upCmd.ExecuteReader();
            string cmp_id = "-1";
            if (reader.Read())
                cmp_id = reader["cmp_id"].ToString();
            reader.Close();
            //
            obj = null;
            object[] obj2 = null;
            string chromosome = null;
            string start_position = null;
            string end_position = null;
            string segment_length_cm = null;
            string snp_count = null;
            string segment_id = "-1";

            for (int i = 0; i < segment_idx.Rows.Count; i++) {
                obj = segment_idx.Rows[i].ItemArray;
                chromosome = obj[0].ToString();
                start_position = obj[1].ToString();
                end_position = obj[2].ToString();
                segment_length_cm = obj[3].ToString();
                snp_count = obj[4].ToString();
                upCmd = new SQLiteCommand(@"INSERT OR REPLACE INTO cmp_autosomal(cmp_id,kit1,kit2,chromosome,start_position,end_position,segment_length_cm,snp_count) values (@cmp_id,@kit1,@kit2,@chromosome,@start_position,@end_position,@segment_length_cm,@snp_count)", cnn);
                upCmd.Parameters.AddWithValue("@cmp_id", cmp_id);
                upCmd.Parameters.AddWithValue("@kit1", kit1);
                upCmd.Parameters.AddWithValue("@kit2", kit2);
                upCmd.Parameters.AddWithValue("@chromosome", chromosome);
                upCmd.Parameters.AddWithValue("@start_position", start_position);
                upCmd.Parameters.AddWithValue("@end_position", end_position);
                upCmd.Parameters.AddWithValue("@segment_length_cm", segment_length_cm);
                upCmd.Parameters.AddWithValue("@snp_count", snp_count);
                upCmd.ExecuteNonQuery();
                //
                if (!reference) {
                    upCmd = new SQLiteCommand(@"SELECT segment_id FROM cmp_autosomal WHERE kit1=@kit1 AND kit2=@kit2 AND chromosome=@chromosome AND start_position=@start_position AND end_position=@end_position", cnn);
                    upCmd.Parameters.AddWithValue("@kit1", kit1);
                    upCmd.Parameters.AddWithValue("@kit2", kit2);
                    upCmd.Parameters.AddWithValue("@chromosome", chromosome);
                    upCmd.Parameters.AddWithValue("@start_position", start_position);
                    upCmd.Parameters.AddWithValue("@end_position", end_position);
                    reader = upCmd.ExecuteReader();
                    segment_id = "-1";
                    if (reader.Read()) {
                        segment_id = reader.GetInt16(0).ToString();
                    }
                    reader.Close();
                    ///
                    using (var transaction = cnn.BeginTransaction()) {
                        foreach (DataRow row in segments[i].Rows) {
                            obj2 = row.ItemArray;
                            upCmd = new SQLiteCommand(@"INSERT OR REPLACE INTO cmp_mrca (rsid,chromosome,position,kit1_genotype,kit2_genotype,match,segment_id) values (@rsid,@chromosome,@position,@kit1_genotype,@kit2_genotype,@match,@segment_id)", cnn);
                            upCmd.Parameters.AddWithValue("@rsid", obj2[0].ToString());
                            upCmd.Parameters.AddWithValue("@chromosome", obj2[1].ToString());
                            upCmd.Parameters.AddWithValue("@position", obj2[2].ToString());
                            upCmd.Parameters.AddWithValue("@kit1_genotype", obj2[3].ToString());
                            upCmd.Parameters.AddWithValue("@kit2_genotype", obj2[4].ToString());
                            upCmd.Parameters.AddWithValue("@match", obj2[5].ToString());
                            upCmd.Parameters.AddWithValue("@segment_id", segment_id);
                            upCmd.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                }
            }
        }


        public static void exportKit(string kit, string filename, int option)
        {
            string name = null;

            name = getKitName(kit);
            SQLiteConnection cnn = GKSqlFuncs.getDBConnection();
            // kit autosomal - RSID,Chromosome,Position,Genotypoe
            SQLiteCommand query = new SQLiteCommand(@"SELECT kit_no,rsid,chromosome,position,genotype from kit_autosomal where kit_no=@kit_no order by chromosome,position", cnn);
            query.Parameters.AddWithValue("@kit_no", kit);
            SQLiteDataReader reader = query.ExecuteReader();
            DataTable dt_autosomal = new DataTable();
            dt_autosomal.Load(reader);
            reader.Close();
            query.Dispose();

            switch (option) {
                case GKGenFuncs.EXPORT_ALL_GGK:

                    string autosomal_schema = null;
                    string ysnps = null;
                    string ystr = null;
                    string mtdna = null;
                    string fasta = null;
                    string autosomal_file = Path.GetTempFileName();
                    FileStream fs = new FileStream(autosomal_file, FileMode.Create);
                    dt_autosomal.WriteXml(fs);
                    fs.Close();
                    MemoryStream ms = new MemoryStream();
                    dt_autosomal.WriteXmlSchema(ms);
                    autosomal_schema = Encoding.UTF8.GetString(ms.ToArray());

                    // kit - ysnp
                    query = new SQLiteCommand(@"SELECT ysnps from kit_ysnps where kit_no=@kit_no", cnn);
                    query.Parameters.AddWithValue("@kit_no", kit);
                    reader = query.ExecuteReader();
                    if (reader.Read()) {
                        ysnps = reader.GetString(0);
                    }
                    if (ysnps == null)
                        ysnps = "";
                    reader.Close();
                    query.Dispose();


                    // kit - ystr
                    query = new SQLiteCommand(@"SELECT marker,value from kit_ystr where kit_no=@kit_no", cnn);
                    query.Parameters.AddWithValue("@kit_no", kit);
                    reader = query.ExecuteReader();
                    string marker = null;
                    string value = null;
                    string ystr_array = "";
                    while (reader.Read()) {
                        marker = reader.GetString(0);
                        value = reader.GetString(1);
                        ystr_array += marker.Trim() + "=" + value.Trim() + " ";
                    }
                    ystr = ystr_array.Trim().Replace(" ", ",");
                    reader.Close();
                    query.Dispose();

                    // kit - mtdna
                    query = new SQLiteCommand(@"SELECT mutations,fasta from kit_mtdna where kit_no=@kit_no", cnn);
                    query.Parameters.AddWithValue("@kit_no", kit);
                    reader = query.ExecuteReader();

                    if (reader.Read()) {
                        mtdna = reader.GetString(0);
                        fasta = reader.GetString(1);
                    }
                    if (mtdna == null)
                        mtdna = "";
                    if (fasta == null)
                        fasta = "";

                    reader.Close();
                    query.Dispose();
                    cnn.Close();
                    //out of memory... on several occations. (!?) - need some quality checks
                    string temp_file = Path.GetTempFileName();
                    File.AppendAllText(temp_file, "@KIT@\r\n" + kit + "\r\n@NAME@\r\n" + name + "\r\n@AUTOSOMAL@\r\n");

                    //temp_file+autosomal_file=temp_file
                    StreamReader at_reader = File.OpenText(autosomal_file);
                    string line = null;
                    int count = 0;
                    StringBuilder sb = new StringBuilder();
                    while ((line = at_reader.ReadLine()) != null) {
                        sb.Append(line);
                        sb.Append("\r\n");
                        if (count > 65535) {
                            File.AppendAllText(temp_file, sb.ToString());
                            sb.Length = 0;
                            count = 0;
                        }
                    }
                    at_reader.Close();
                    File.AppendAllText(temp_file, sb.ToString());

                    File.AppendAllText(temp_file, "\r\n@AUTOSOMAL-SCHEMA@\r\n" + autosomal_schema.Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Trim());
                    File.AppendAllText(temp_file, "\r\n@YSNPS@\r\n" + ysnps + "\r\n@YSTR@\r\n" + ystr + "\r\n@MTDNA@\r\n" + mtdna + "\r\n@FASTA@\r\n" + Convert.ToBase64String(Encoding.UTF8.GetBytes(fasta)) + "\r\n");
                    GKUtils.GZipFile(temp_file, filename);
                    File.Delete(autosomal_file);
                    File.Delete(temp_file);
                    break;

                case GKGenFuncs.EXPORT_AUTOSOMAL_FTDNA:
                    StringBuilder sb_ftdna = new StringBuilder();
                    sb_ftdna.Append("RSID,CHROMOSOME,POSITION,RESULT\r\n");
                    foreach (DataRow row in dt_autosomal.Rows) {
                        sb_ftdna.Append("\"" + row.ItemArray[1] + "\",\"" + row.ItemArray[2] + "\",\"" + row.ItemArray[3] + "\",\"" + row.ItemArray[4] + "\"\r\n");
                    }
                    File.WriteAllText(filename, sb_ftdna.ToString());
                    break;

                case GKGenFuncs.EXPORT_AUTOSOMAL_23ANDME:
                    StringBuilder sb_23andme = new StringBuilder();
                    sb_23andme.Append("# rsid\tchromosome\tposition\tgenotype\r\n");
                    foreach (DataRow row in dt_autosomal.Rows) {
                        sb_23andme.Append(row.ItemArray[1] + "\t" + row.ItemArray[2] + "\t" + row.ItemArray[3] + "\t" + row.ItemArray[4] + "\r\n");
                    }
                    File.WriteAllText(filename, sb_23andme.ToString());
                    break;

                default:
                    break;
            }

        }

        public static void importKit(string filename)
        {
            string temp_file = Path.GetTempFileName();
            string temp_autosomal = Path.GetTempFileName();
            //string temp_file = Path.GetTempFileName();           

            string kit_no = null;
            string name = null;
            string autosomal_schema = null;
            string ysnps = null;
            string ystr = null;
            string mtdna = null;
            string fasta = null;
            StringBuilder sb = new StringBuilder();
            int count = 0;
            GKUtils.GUnzipFile(filename, temp_file);
            StreamReader reader = File.OpenText(temp_file);
            string line = null;
            while ((line = reader.ReadLine()) != null) {
                if (line.Trim() == "@KIT@") {
                    kit_no = reader.ReadLine();
                } else if (line.Trim() == "@NAME@") {
                    name = reader.ReadLine();
                } else if (line.Trim() == "@AUTOSOMAL@") {
                    while ((line = reader.ReadLine()) != "@AUTOSOMAL-SCHEMA@") {
                        sb.Append(line);
                        sb.Append("\r\n");
                        if (count > 65535) {
                            File.AppendAllText(temp_autosomal, sb.ToString());
                            sb.Length = 0;
                            count = 0;
                        }
                    }
                    File.AppendAllText(temp_autosomal, sb.ToString());
                    autosomal_schema = reader.ReadLine();
                } else if (line.Trim() == "@YSNPS@") {
                    ysnps = reader.ReadLine();
                } else if (line.Trim() == "@YSTR@") {
                    ystr = reader.ReadLine();
                } else if (line.Trim() == "@MTDNA@") {
                    mtdna = reader.ReadLine();
                } else if (line.Trim() == "@FASTA@") {
                    fasta = Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadLine()));
                }
            }
            reader.Close();
            // now insert into db...

            Program.GGKitFrmMainInst.Invoke(new MethodInvoker(delegate {
                GKUIFuncs.setStatus("20% complete.");
                GKUIFuncs.setProgress(20);
            }));

            SQLiteConnection cnn = GKSqlFuncs.getDBConnection();

            try {
                //remove existing - on delete cascade...
                SQLiteCommand upCmd = new SQLiteCommand(@"DELETE from kit_master where kit_no=@kit_no", cnn);
                upCmd.Parameters.AddWithValue("@kit_no", kit_no);
                upCmd.ExecuteNonQuery();

                //kit master
                upCmd = new SQLiteCommand(@"INSERT OR REPLACE INTO kit_master(kit_no, name)values(@kit_no,@name)", cnn);
                upCmd.Parameters.AddWithValue("@kit_no", kit_no);
                upCmd.Parameters.AddWithValue("@name", name);
                upCmd.ExecuteNonQuery();
                upCmd.Dispose();


                //kit autosomal
                //upCmd = new SQLiteCommand(@"INSERT OR REPLACE INTO kit_autosomal(kit_no, rsid,chromosome,position,genotype)values(@kit_no,@rsid,@chromosome,@position,@genotype)", cnn);

                DataTable dt = new DataTable();
                dt.ReadXmlSchema(new MemoryStream(Encoding.UTF8.GetBytes(autosomal_schema)));
                dt.ReadXml(temp_autosomal);
                using (var transaction = cnn.BeginTransaction()) {
                    SQLiteDataAdapter sqliteAdapter = new SQLiteDataAdapter("SELECT kit_no,rsid,chromosome,position,genotype FROM kit_autosomal", cnn);
                    var cmdBuilder = new SQLiteCommandBuilder(sqliteAdapter);
                    sqliteAdapter.Update(dt);
                    transaction.Commit();
                }

                //kit ysnps
                if (ysnps.Trim() != "") {
                    upCmd = new SQLiteCommand(@"INSERT OR REPLACE INTO kit_ysnps(kit_no, ysnps)values(@kit_no,@ysnps)", cnn);
                    upCmd.Parameters.AddWithValue("@kit_no", kit_no);
                    upCmd.Parameters.AddWithValue("@ysnps", ysnps);
                    upCmd.ExecuteNonQuery();
                    upCmd.Dispose();
                }
                Program.GGKitFrmMainInst.Invoke(new MethodInvoker(delegate {
                    GKUIFuncs.setStatus("80% complete.");
                    GKUIFuncs.setProgress(80);
                }));

                //kit ystr
                if (ystr.Trim() != "") {
                    upCmd = new SQLiteCommand(@"INSERT OR REPLACE INTO kit_ystr(kit_no, marker, value)values(@kit_no,@marker,@value)", cnn);
                    using (var transaction = cnn.BeginTransaction()) {
                        string ystrtrim = null;
                        foreach (string yt in ystr.Split(new char[] { ',' })) {
                            ystrtrim = yt.Trim();
                            if (ystrtrim == "")
                                continue;
                            upCmd.Parameters.AddWithValue("@kit_no", kit_no);
                            upCmd.Parameters.AddWithValue("@marker", ystrtrim.Split(new char[] { '=' })[0]);
                            upCmd.Parameters.AddWithValue("@value", ystrtrim.Split(new char[] { '=' })[1]);
                            upCmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    upCmd.Dispose();
                }
                //kit mtdna
                if (mtdna.Trim() != "") {
                    upCmd = new SQLiteCommand(@"INSERT OR REPLACE INTO kit_mtdna(kit_no, mutations,fasta)values(@kit_no,@mutations,@fasta)", cnn);
                    upCmd.Parameters.AddWithValue("@kit_no", kit_no);
                    upCmd.Parameters.AddWithValue("@mutations", mtdna);
                    upCmd.Parameters.AddWithValue("@fasta", fasta);
                    upCmd.ExecuteNonQuery();
                    upCmd.Dispose();
                }
            } catch (Exception err) {
                MessageBox.Show("Not Saved. Techical Details: " + err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            cnn.Dispose();
            File.Delete(temp_file);
            File.Delete(temp_autosomal);
        }

        public static object[] compareOneToOne(string kit1, string kit2)
        {
            string ref1 = queryValue("kit_master", new string[] { "reference" }, "where kit_no='" + kit1 + "'");
            string ref2 = queryValue("kit_master", new string[] { "reference" }, "where kit_no='" + kit2 + "'");
            bool reference = ref1 == "1" || ref2 == "1";
            return compareOneToOne(kit1, kit2, null, reference, false);
        }

        public static object[] compareOneToOne(string kit1, string kit2, BackgroundWorker bwCompare, bool reference, bool just_update)
        {
            // just_update - if parameter true, will update if record not found but will not return the existing record if found.
            string name1 = getKitName(kit1);
            string name2 = getKitName(kit2);
            List<DataTable> segments = new List<DataTable>();
            DataTable segments_idx = new DataTable();
            SQLiteConnection cnn = getDBConnection();
            //
            Boolean exists = checkAlreadyCompared(cnn, kit1, kit2);

            if (exists) {
                if (!just_update) {
                    DataTable cmp = getAutosomalCmp(cnn, kit1, kit2);
                    cnn = getDBConnection();
                    segments_idx.Columns.Add("Chromosome");
                    segments_idx.Columns.Add("Begin Position");
                    segments_idx.Columns.Add("End Position");
                    segments_idx.Columns.Add("Segment Length (cM)");
                    segments_idx.Columns.Add("SNP Count");

                    object[] o1 = null;
                    foreach (DataRow row in cmp.Rows) {
                        if (bwCompare != null)
                            if (bwCompare.CancellationPending)
                                break;

                        o1 = row.ItemArray;
                        segments_idx.Rows.Add(new object[] { o1[1], o1[2], o1[3], o1[4], o1[5] });
                        //
                        if (!reference) {
                            DataTable dt = queryDatabase("cmp_mrca", new string[] { "rsid'RSID'", "chromosome'Chromosome'", "position'Position'", "kit1_genotype'" + name1.Replace(" ", "_") + "'", "kit2_genotype'" + name2.Replace(" ", "_") + "'", "match'Match'" }, "WHERE segment_id='" + o1[0] + "'");
                            segments.Add(dt);
                        }
                    }
                }
            } else {

                cnn = getDBConnection();
                SQLiteCommand cmpQuery = new SQLiteCommand(@"select rsid,chr,pos,gt1,gt2, count(*) FROM (SELECT kit1.rsid 'rsid',kit1.chromosome 'chr',kit1.position 'pos',kit1.genotype 'gt1',kit2.genotype 'gt2' FROM kit_autosomal kit1 LEFT JOIN kit_autosomal kit2 on kit1.rsid=kit2.rsid WHERE kit1.kit_no = '" + kit1 + "' AND kit2.kit_no = '" + kit2 + "' UNION SELECT kit1.rsid 'rsid',kit1.chromosome 'chr',kit1.position 'pos',kit1.genotype 'gt1', kit2.genotype 'gt2' FROM kit_autosomal kit1 LEFT JOIN kit_autosomal kit2 on kit1.rsid=kit2.rsid WHERE kit1.kit_no = '" + kit2 + "' AND kit2.kit_no = '" + kit1 + "') GROUP BY rsid ORDER BY CAST(chr as INTEGER),pos", cnn);
                SQLiteDataReader reader = cmpQuery.ExecuteReader();


                segments_idx.Columns.Add("Chromosome");
                segments_idx.Columns.Add("Begin Position");
                segments_idx.Columns.Add("End Position");
                segments_idx.Columns.Add("Segment Length (cM)");
                segments_idx.Columns.Add("SNP Count");

                DataTable tmp = new DataTable();
                tmp.Columns.Add("RSID");
                tmp.Columns.Add("Chromosome");
                tmp.Columns.Add("Position");
                tmp.Columns.Add(name1);
                tmp.Columns.Add(name2);
                tmp.Columns.Add("Match");

                string chromosome = "";
                string prev_chr = "";
                int start_pos = 0;
                int end_pos = 0;
                int position = 0;
                string rsid = null;
                string gt1 = null;
                string gt2 = null;

                int prev_snp_count = 0;
                int errorRadius = 250;
                int no_call_counter = 0;
                int no_call_limit = int.Parse(GKSettings.getParameterValue("Compare.NoCalls.Limit"));
                while (reader.Read()) {
                    if (bwCompare != null)
                        if (bwCompare.CancellationPending)
                            break;

                    rsid = reader.GetString(0);
                    gt1 = reader.GetString(3);
                    gt2 = reader.GetString(4);
                    if (gt1.Length == 1)
                        gt1 = gt1 + gt1;
                    if (gt2.Length == 1)
                        gt2 = gt2 + gt2;

                    chromosome = reader.GetString(1);
                    if (prev_chr == "")
                        prev_chr = chromosome;
                    position = reader.GetInt32(2);
                    if (prev_chr == chromosome) {
                        if (chromosome == "X")
                            errorRadius = int.Parse(GKSettings.getParameterValue("Compare.X.Threshold.SNPs")) / 2;
                        else
                            errorRadius = int.Parse(GKSettings.getParameterValue("Compare.Autosomal.Threshold.SNPs")) / 2;

                        if (reader.GetInt32(5) == 1) {
                            // match both alleles
                            tmp.Rows.Add(new object[] { rsid, chromosome, position.ToString(), gt1, gt2, gt1 });
                            if (start_pos == 0)
                                start_pos = position;
                        } else if (reader.GetInt32(5) == 2) {
                            // match 1 allele
                            if (gt1 == GKUtils.Reverse(gt2)) {
                                tmp.Rows.Add(new object[] { rsid, chromosome, position.ToString(), gt1, gt2, gt1 });
                                if (start_pos == 0)
                                    start_pos = position;
                            } else if (gt1[0] == gt2[0]) {
                                tmp.Rows.Add(new object[] { rsid, chromosome, position.ToString(), gt1, gt2, gt2[0].ToString() });
                                if (start_pos == 0)
                                    start_pos = position;
                            } else if (gt1[0] == gt2[1]) {
                                tmp.Rows.Add(new object[] { rsid, chromosome, position.ToString(), gt1, gt2, gt2[1].ToString() });
                                if (start_pos == 0)
                                    start_pos = position;
                            } else if (gt1[1] == gt2[0]) {
                                tmp.Rows.Add(new object[] { rsid, chromosome, position.ToString(), gt1, gt2, gt2[0].ToString() });
                                if (start_pos == 0)
                                    start_pos = position;
                            } else if (gt1[1] == gt2[1]) {
                                tmp.Rows.Add(new object[] { rsid, chromosome, position.ToString(), gt1, gt2, gt2[1].ToString() });
                                if (start_pos == 0)
                                    start_pos = position;
                            } else {
                                no_call_counter++;
                                if (no_call_counter > no_call_limit) {
                                    // no call exceeded..
                                    prev_snp_count = 0;
                                    no_call_counter = 0;
                                    dontMatchProc(start_pos, end_pos, prev_chr, chromosome, ref segments_idx, ref segments, ref tmp, reference);
                                    start_pos = position;
                                } else if (gt1 == "--" || gt1 == "??" || gt1 == "00") {
                                    tmp.Rows.Add(new object[] { rsid, chromosome, position.ToString(), gt1, gt2, "-" });
                                    if (start_pos == 0)
                                        start_pos = position;
                                } else if (gt2 == "--" || gt2 == "??" || gt2 == "00") {
                                    tmp.Rows.Add(new object[] { rsid, chromosome, position.ToString(), gt1, gt2, "-" });
                                    if (start_pos == 0)
                                        start_pos = position;
                                } else if (gt1[0] == '-' || gt1[0] == '?' || gt1[0] == '0') {
                                    tmp.Rows.Add(new object[] { rsid, chromosome, position.ToString(), gt1, gt2, "-" });
                                    if (start_pos == 0)
                                        start_pos = position;
                                } else if (gt1[1] == '-' || gt1[1] == '?' || gt1[1] == '0') {
                                    tmp.Rows.Add(new object[] { rsid, chromosome, position.ToString(), gt1, gt2, "-" });
                                    if (start_pos == 0)
                                        start_pos = position;
                                } else if (gt2[0] == '-' || gt2[0] == '?' || gt2[0] == '0') {
                                    tmp.Rows.Add(new object[] { rsid, chromosome, position.ToString(), gt1, gt2, "-" });
                                    if (start_pos == 0)
                                        start_pos = position;
                                } else if (gt2[1] == '-' || gt2[1] == '?' || gt2[1] == '0') {
                                    tmp.Rows.Add(new object[] { rsid, chromosome, position.ToString(), gt1, gt2, "-" });
                                    if (start_pos == 0)
                                        start_pos = position;
                                } else if (tmp.Rows.Count - prev_snp_count >= errorRadius && no_call_counter <= no_call_limit) {
                                    prev_snp_count = tmp.Rows.Count;
                                    no_call_counter = 0;
                                    tmp.Rows.Add(new object[] { rsid, chromosome, position.ToString(), gt1, gt2, "" });
                                    if (start_pos == 0)
                                        start_pos = position;
                                } else {
                                    // doesn't match on same chromosome
                                    prev_snp_count = 0;
                                    no_call_counter = 0;
                                    dontMatchProc(start_pos, end_pos, prev_chr, chromosome, ref segments_idx, ref segments, ref tmp, reference);
                                    start_pos = position;
                                }
                            }
                        }
                    } else {
                        // next chromosome
                        prev_snp_count = 0;
                        no_call_counter = 0;
                        dontMatchProc(start_pos, end_pos, prev_chr, chromosome, ref segments_idx, ref segments, ref tmp, reference);
                        start_pos = position;
                    }
                    end_pos = position;
                    prev_chr = chromosome;
                }
                reader.Close();
                cnn.Close();
                //
                //save
                GKSqlFuncs.addAutosomalCmp(kit1, kit2, segments_idx, segments, reference);

            }
            return new object[] { segments_idx, segments };
        }

        private static bool checkAlreadyCompared(SQLiteConnection cnn, string kit1, string kit2)
        {
            DataTable dt = queryDatabase(cnn, "cmp_status", new string[] { "status_autosomal" }, "WHERE (kit1='" + kit1 + "' AND kit2='" + kit2 + "') OR (kit1='" + kit2 + "' AND kit2='" + kit1 + "')");
            if (dt.Rows.Count == 0)
                return false;
            else
                return true;
        }

        private static DataTable getAutosomalCmp(SQLiteConnection cnn, string kit1, string kit2)
        {
            DataTable dt = queryDatabase("cmp_autosomal", new string[] { "segment_id", "chromosome", "start_position", "end_position", "segment_length_cm", "snp_count" }, "WHERE (kit1='" + kit1 + "' AND kit2='" + kit2 + "') OR (kit1='" + kit2 + "' AND kit2='" + kit1 + "')");
            return dt;
        }

        private static void dontMatchProc(int start_pos, int end_pos, string prev_chr, string chromosome, ref DataTable segments_idx, ref List<DataTable> segments, ref DataTable tmp, bool reference)
        {
            double cm_len = 0.0;
            double cm_th = 0.0;
            int snp_th = 0;
            double mb_th = GKGenFuncs.MB_THRESHOLD;

            if (reference) {
                snp_th = int.Parse(GKSettings.getParameterValue("Admixture.Threshold.SNPs"));
                cm_th = double.Parse(GKSettings.getParameterValue("Admixture.Threshold.cM"));

                if ((end_pos - start_pos) > 5000) {
                    cm_len = GKGenFuncs.getLength_in_cM(chromosome, start_pos, end_pos);

                    if (cm_len >= cm_th && tmp.Rows.Count >= snp_th) {
                        segments.Add(tmp.Copy());
                        if (prev_chr != chromosome)
                            segments_idx.Rows.Add(new object[] { prev_chr, start_pos, end_pos, cm_len.ToString("#0.00"), tmp.Rows.Count.ToString() });
                        else
                            segments_idx.Rows.Add(new object[] { chromosome, start_pos, end_pos, cm_len.ToString("#0.00"), tmp.Rows.Count.ToString() });
                    }
                }
            } else {
                if (chromosome == "X") {
                    cm_th = double.Parse(GKSettings.getParameterValue("Compare.X.Threshold.cM"));
                    snp_th = int.Parse(GKSettings.getParameterValue("Compare.X.Threshold.SNPs"));
                } else {
                    cm_th = double.Parse(GKSettings.getParameterValue("Compare.Autosomal.Threshold.cM"));
                    snp_th = int.Parse(GKSettings.getParameterValue("Compare.Autosomal.Threshold.SNPs"));
                }

                if ((end_pos - start_pos) / 1000000.0 > mb_th) {
                    cm_len = GKGenFuncs.getLength_in_cM(chromosome, start_pos, end_pos);

                    if (cm_len >= cm_th && tmp.Rows.Count >= snp_th) {
                        segments.Add(tmp.Copy());
                        if (prev_chr != chromosome)
                            segments_idx.Rows.Add(new object[] { prev_chr, start_pos, end_pos, cm_len.ToString("#0.00"), tmp.Rows.Count.ToString() });
                        else
                            segments_idx.Rows.Add(new object[] { chromosome, start_pos, end_pos, cm_len.ToString("#0.00"), tmp.Rows.Count.ToString() });
                    }
                }
            }

            tmp.Clear();
        }

        public static string getKitName(string kit)
        {
            if (kit_name == null)
                kit_name = new Dictionary<string, string>();
            if (kit_name.ContainsKey(kit))
                return kit_name[kit];
            else {
                DataTable dt = GKSqlFuncs.queryDatabase("kit_master", new string[] { "name" }, "where kit_no='" + kit + "'");
                if (dt.Rows.Count > 0) {
                    kit_name.Add(kit, dt.Rows[0].ItemArray[0].ToString());
                    return dt.Rows[0].ItemArray[0].ToString();
                }
                return "Unknown";
            }
        }

        public static object[] ROH(string kit)
        {
            List<DataTable> segments = new List<DataTable>();
            DataTable segments_idx = new DataTable();
            SQLiteConnection cnn = getDBConnection();
            //
            Boolean exists = checkROHExists(cnn, kit); ;

            if (exists) {
                segments_idx = geRoHCmp(cnn, kit);
                cnn = getDBConnection();


                object[] o1 = null;
                foreach (DataRow row in segments_idx.Rows) {
                    o1 = row.ItemArray;
                    //
                    DataTable dt = QueryDB("select rsid'RSID',chromosome'Chromosome',position'Position',genotype'Genotype' from  kit_autosomal where kit_no='" + kit + "' and chromosome='" + o1[0] + "' and position>=" + o1[1] + " and position<=" + o1[2] + " order by position");
                    segments.Add(dt);
                }
            } else {

                cnn = getDBConnection();
                SQLiteCommand cmpQuery = new SQLiteCommand(@"select rsid,chromosome,position,genotype from  kit_autosomal where kit_no='" + kit + "' order by cast(chromosome as integer), position", cnn);
                SQLiteDataReader reader = cmpQuery.ExecuteReader();


                segments_idx.Columns.Add("Chromosome");
                segments_idx.Columns.Add("Begin Position");
                segments_idx.Columns.Add("End Position");
                segments_idx.Columns.Add("Segment Length (cM)");
                segments_idx.Columns.Add("SNP Count");

                DataTable tmp = new DataTable();
                tmp.Columns.Add("RSID");
                tmp.Columns.Add("Chromosome");
                tmp.Columns.Add("Position");
                tmp.Columns.Add("Genotype");

                string chromosome = "";
                string prev_chr = "";
                int start_pos = 0;
                int end_pos = 0;
                int position = 0;
                string rsid = null;
                string genotype = null;

                int prev_snp_count = 0;
                int errorRadius = 250;
                int no_call_counter = 0;
                int no_call_limit = int.Parse(GKSettings.getParameterValue("Compare.NoCalls.Limit"));
                while (reader.Read()) {
                    rsid = reader.GetString(0);
                    chromosome = reader.GetString(1);
                    position = reader.GetInt32(2);
                    genotype = reader.GetString(3);

                    if (genotype.Length == 1)
                        genotype = genotype + genotype;

                    if (prev_chr == "")
                        prev_chr = chromosome;

                    if (prev_chr == chromosome) {
                        if (chromosome == "X")
                            errorRadius = int.Parse(GKSettings.getParameterValue("Compare.X.Threshold.SNPs")) / 2;
                        else
                            errorRadius = int.Parse(GKSettings.getParameterValue("Compare.Autosomal.Threshold.SNPs")) / 2;

                        if (genotype[0] == genotype[1] && genotype[0] != '-' && genotype[0] != '?') {
                            // match 
                            tmp.Rows.Add(new object[] { rsid, chromosome, position.ToString(), genotype });
                            if (start_pos == 0)
                                start_pos = position;
                        } else if ((genotype[0] != '-' && genotype[0] != '?' && (genotype[1] == '-' || genotype[1] == '?')) ||
                              (genotype[1] != '-' && genotype[1] != '?' && (genotype[0] == '-' || genotype[0] == '?'))) {
                            no_call_counter++;
                            if (no_call_counter <= no_call_limit) {
                                tmp.Rows.Add(new object[] { rsid, chromosome, position.ToString(), genotype });
                                if (start_pos == 0)
                                    start_pos = position;
                            } else {
                                no_call_counter = 0;
                                // exceeded no call count.
                                prev_snp_count = 0;
                                dontMatchProcRoH(start_pos, end_pos, prev_chr, chromosome, ref segments_idx, ref segments, ref tmp, false);
                                start_pos = position;
                            }
                        } else if (tmp.Rows.Count - prev_snp_count >= errorRadius) {
                            prev_snp_count = tmp.Rows.Count;
                            tmp.Rows.Add(new object[] { rsid, chromosome, position.ToString(), genotype });
                            if (start_pos == 0)
                                start_pos = position;
                        } else {
                            // doesn't match on same chromosome
                            prev_snp_count = 0;
                            no_call_counter = 0;
                            dontMatchProcRoH(start_pos, end_pos, prev_chr, chromosome, ref segments_idx, ref segments, ref tmp, false);
                            start_pos = position;
                        }
                    } else {
                        // next chromosome
                        prev_snp_count = 0;
                        no_call_counter = 0;
                        dontMatchProcRoH(start_pos, end_pos, prev_chr, chromosome, ref segments_idx, ref segments, ref tmp, false);
                        start_pos = position;
                    }
                    end_pos = position;
                    prev_chr = chromosome;
                }
                reader.Close();
                cnn.Close();
                //
                addROHCmp(kit, segments_idx);
            }
            return new object[] { segments_idx, segments };
        }

        private static bool checkROHExists(SQLiteConnection cnn, string kit)
        {
            string roh = queryValue("kit_master", new string[] { "roh_status" }, "where kit_no='" + kit + "'");
            if (roh == "0")
                return false;
            else
                return true;
        }

        private static void dontMatchProcRoH(int start_pos, int end_pos, string prev_chr, string chromosome, ref DataTable segments_idx, ref List<DataTable> segments, ref DataTable tmp, bool reference)
        {
            double cm_len = 0.0;
            double cm_th = 0.0;
            int snp_th = 0;
            double mb_th = GKGenFuncs.MB_THRESHOLD;

            if (reference) {
                snp_th = int.Parse(GKSettings.getParameterValue("Admixture.Threshold.SNPs"));
                cm_th = double.Parse(GKSettings.getParameterValue("Admixture.Threshold.cM"));

                if ((end_pos - start_pos) > 5000) {
                    cm_len = GKGenFuncs.getLength_in_cM(chromosome, start_pos, end_pos);

                    if (cm_len >= cm_th && tmp.Rows.Count >= snp_th) {
                        segments.Add(tmp.Copy());
                        if (prev_chr != chromosome)
                            segments_idx.Rows.Add(new object[] { prev_chr, start_pos, end_pos, cm_len.ToString("#0.00"), tmp.Rows.Count.ToString() });
                        else
                            segments_idx.Rows.Add(new object[] { chromosome, start_pos, end_pos, cm_len.ToString("#0.00"), tmp.Rows.Count.ToString() });
                    }
                }
            } else {

                if (chromosome == "X") {
                    cm_th = double.Parse(GKSettings.getParameterValue("Compare.X.Threshold.cM"));
                    snp_th = int.Parse(GKSettings.getParameterValue("Compare.X.Threshold.SNPs"));
                } else {
                    cm_th = double.Parse(GKSettings.getParameterValue("Compare.Autosomal.Threshold.cM"));
                    snp_th = int.Parse(GKSettings.getParameterValue("Compare.Autosomal.Threshold.SNPs"));
                }

                if ((end_pos - start_pos) / 1000000.0 > mb_th) {
                    cm_len = GKGenFuncs.getLength_in_cM(chromosome, start_pos, end_pos);

                    if (cm_len >= cm_th && tmp.Rows.Count >= snp_th) {
                        segments.Add(tmp.Copy());
                        if (prev_chr != chromosome)
                            segments_idx.Rows.Add(new object[] { prev_chr, start_pos, end_pos, cm_len.ToString("#0.00"), tmp.Rows.Count.ToString() });
                        else
                            segments_idx.Rows.Add(new object[] { chromosome, start_pos, end_pos, cm_len.ToString("#0.00"), tmp.Rows.Count.ToString() });
                    }
                }
            }

            tmp.Clear();
        }

        public static void addROHCmp(string kit, DataTable segment_idx)
        {
            SQLiteConnection cnn = getDBConnection();
            //
            SQLiteCommand upCmd = new SQLiteCommand(@"DELETE from kit_roh WHERE kit_no=@kit", cnn);
            upCmd.Parameters.AddWithValue("@kit", kit);
            upCmd.ExecuteNonQuery();
            //
            object[] obj = null;
            string chromosome = null;
            string start_position = null;
            string end_position = null;
            string segment_length_cm = null;
            string snp_count = null;
            using (SQLiteTransaction trans = cnn.BeginTransaction()) {
                for (int i = 0; i < segment_idx.Rows.Count; i++) {
                    obj = segment_idx.Rows[i].ItemArray;
                    chromosome = obj[0].ToString();
                    start_position = obj[1].ToString();
                    end_position = obj[2].ToString();
                    segment_length_cm = obj[3].ToString();
                    snp_count = obj[4].ToString();
                    upCmd = new SQLiteCommand(@"INSERT OR REPLACE INTO kit_roh(kit_no,chromosome,start_position,end_position,segment_length_cm,snp_count) values (@kit_no,@chromosome,@start_position,@end_position,@segment_length_cm,@snp_count)", cnn);
                    upCmd.Parameters.AddWithValue("@kit_no", kit);
                    upCmd.Parameters.AddWithValue("@chromosome", chromosome);
                    upCmd.Parameters.AddWithValue("@start_position", start_position);
                    upCmd.Parameters.AddWithValue("@end_position", end_position);
                    upCmd.Parameters.AddWithValue("@segment_length_cm", segment_length_cm);
                    upCmd.Parameters.AddWithValue("@snp_count", snp_count);
                    upCmd.ExecuteNonQuery();
                }
                trans.Commit();
            }

            upCmd = new SQLiteCommand(@"UPDATE kit_master SET roh_status=1 WHERE kit_no=@kit_no", cnn);
            upCmd.Parameters.AddWithValue("@kit_no", kit);
            upCmd.ExecuteNonQuery();

            cnn.Close();
        }

        private static DataTable geRoHCmp(SQLiteConnection cnn, string kit)
        {
            DataTable dt = queryDatabase("kit_roh", new string[] { "chromosome'Chromosome'", "start_position'Begin Position'", "end_position'End Position'", "segment_length_cm'Segment Length (cM)'", "snp_count'SNP Count'" }, "WHERE kit_no='" + kit + "'");
            return dt;
        }

        public static bool isPhased(string kit)
        {
            DataTable dt = QueryDB("select kit_no from kit_phased where kit_no='" + kit + "'");
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
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
        public static Image getPhasedSegmentImage(DataTable dt, string chromosome)
        {
            int width = 600;
            int height = 150;
            Image img = new Bitmap(width, height);
            int total_count = dt.Rows.Count;
            int paternal_error_position = 0;
            int maternal_error_position = 0;

            int errorRadius = 250;
            int snp_th = -1;
            if (chromosome == "X") {
                snp_th = int.Parse(GKSettings.getParameterValue("Compare.X.Threshold.SNPs"));
                errorRadius = snp_th / 2;
            } else {
                snp_th = int.Parse(GKSettings.getParameterValue("Compare.Autosomal.Threshold.SNPs"));
                errorRadius = snp_th / 2;
            }
            int no_call_limit = int.Parse(GKSettings.getParameterValue("Compare.NoCalls.Limit"));

            int paternal_no_call_count = 0;
            int maternal_no_call_count = 0;
            object[] o = null;

            int x = 0;

            Graphics g = Graphics.FromImage(img);

            int begin_maternal_pos = 0;
            int begin_paternal_pos = 0;

            int curr_pos = 0;
            int tmp = 0;
            int val = 0;
            foreach (DataRow row in dt.Rows) {
                curr_pos = dt.Rows.IndexOf(row);
                x = curr_pos * width / dt.Rows.Count;
                o = row.ItemArray;
                if (!GKGenFuncs.isPhasedMatch(o[1].ToString(), o[2].ToString())) // paternal not matched
                {
                    if ((o[1].ToString().IndexOf('-') != -1 || o[2].ToString().IndexOf('-') != -1 || o[1].ToString().IndexOf('?') != -1 || o[2].ToString().IndexOf('?') != -1) && paternal_no_call_count <= no_call_limit) {
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
                            tmp = (snp_th - val) * 255 / snp_th;
                            Pen p1 = new Pen(Color.FromArgb(255, tmp, tmp, 255), 1);
                            for (int i = begin_paternal_pos * width / dt.Rows.Count; i < x; i++)
                                g.DrawLine(p1, i, 0, i, height / 2);
                        }

                        // don't allow but reset no call counter.
                        paternal_no_call_count = 0;
                        paternal_error_position = curr_pos;
                        begin_paternal_pos = curr_pos;
                    }
                }

                if (!GKGenFuncs.isPhasedMatch(o[1].ToString(), o[3].ToString())) // maternal not matched
                {
                    if ((o[1].ToString().IndexOf('-') != -1 || o[3].ToString().IndexOf('-') != -1 || o[1].ToString().IndexOf('?') != -1 || o[3].ToString().IndexOf('?') != -1) && maternal_no_call_count <= no_call_limit) {
                        //allow no call but count it.
                        maternal_no_call_count++;
                    } else if (curr_pos - maternal_error_position >= errorRadius && maternal_no_call_count <= no_call_limit) {
                        // allow but reset no call counter.
                        maternal_error_position = dt.Rows.IndexOf(row);
                        maternal_no_call_count = 0;
                    } else {
                        if (curr_pos - begin_maternal_pos > 5) {
                            val = curr_pos - begin_maternal_pos;
                            if (val > snp_th)
                                val = snp_th;
                            tmp = (snp_th - val) * 255 / snp_th;
                            Pen p1 = new Pen(Color.FromArgb(255, 255, tmp, tmp), 1);
                            for (int i = begin_maternal_pos * width / dt.Rows.Count; i < x; i++)
                                g.DrawLine(p1, i, height / 2, i, height);
                        }
                        // don't allow but reset no call counter.
                        maternal_no_call_count = 0;
                        maternal_error_position = curr_pos;
                        begin_maternal_pos = curr_pos;
                    }
                }
            }
            ////
            if (curr_pos - begin_paternal_pos > 5) {
                val = curr_pos - begin_paternal_pos;
                if (val > snp_th)
                    val = snp_th;
                tmp = (snp_th - val) * 255 / snp_th;
                Pen p1 = new Pen(Color.FromArgb(255, tmp, tmp, 255), 1);
                for (int i = begin_paternal_pos * width / dt.Rows.Count; i < x; i++)
                    g.DrawLine(p1, i, 0, i, height / 2);
            }

            if (curr_pos - begin_maternal_pos > 5) {
                val = curr_pos - begin_maternal_pos;
                if (val > snp_th)
                    val = snp_th;
                tmp = (snp_th - val) * 255 / snp_th;
                Pen p1 = new Pen(Color.FromArgb(255, 255, tmp, tmp), 1);
                for (int i = begin_maternal_pos * width / dt.Rows.Count; i < x; i++)
                    g.DrawLine(p1, i, height / 2, i, height);
            }



            g.Save();
            return img;
        }
    }
}

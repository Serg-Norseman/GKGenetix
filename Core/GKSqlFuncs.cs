/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;
using GenetixKit.Core.Model;

namespace GenetixKit.Core
{
    internal static class GKSqlFuncs
    {
        private static Dictionary<string, string> kit_name = null;

        private static readonly string SQLITE_DB = @"ggk.db";

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

        private static readonly string[] CreateTableSQL = new string[]{
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

        private static void ResetFactory()
        {
            if (File.Exists(SQLITE_DB))
                File.Move(SQLITE_DB, SQLITE_DB + "-" + DateTime.Now.Ticks.ToString("X"));
            SQLiteConnection connection = new SQLiteConnection(@"Data Source=" + SQLITE_DB + @";Version=3; Compress=True; New=True; PRAGMA foreign_keys = ON; PRAGMA auto_vacuum = FULL;");
            connection.Open();

            Dictionary<string, string> pragma = new Dictionary<string, string> {
                { "foreign_keys", "ON" },
                { "auto_vacuum", "FULL" }
            };

            using (SQLiteTransaction trans = connection.BeginTransaction()) {
                SQLiteCommand ss2 = null;
                foreach (string key in pragma.Keys) {
                    ss2 = new SQLiteCommand("PRAGMA " + key + " = " + pragma[key] + ";", connection);
                    ss2.ExecuteNonQuery();
                }

                for (int idx = 0; idx < CreateTableSQL.Length; idx += 2) {
                    ss2 = new SQLiteCommand(CreateTableSQL[idx + 1], connection);
                    ss2.ExecuteNonQuery();
                }
                trans.Commit();
            }
            connection.Close();
        }

        public static SQLiteConnection GetDBConnection()
        {
            if (File.Exists(SQLITE_DB)) {
                string connStr = @"Data Source=" + SQLITE_DB + @";Version=3; Compress=True; PRAGMA foreign_keys = ON; PRAGMA auto_vacuum = FULL;";

                SQLiteConnection connection = new SQLiteConnection(connStr);
                connection.Open();

                Dictionary<string, string> pragma = new Dictionary<string, string> {
                    { "foreign_keys", "ON" },
                    { "auto_vacuum", "FULL" }
                };

                foreach (string key in pragma.Keys) {
                    SQLiteCommand ss2 = new SQLiteCommand("PRAGMA " + key + " = " + pragma[key] + ";", connection);
                    ss2.ExecuteNonQuery();
                }

                return connection;
            } else {
                if (MessageBox.Show("Data file ggk.db doesn't exist. If this is the first time you are opening the software, you can ignore this error. Do you wish to create one? ", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes) {
                    ResetFactory();
                    return GetDBConnection();
                }
                Application.Exit();
            }
            return null;
        }

        public static void DeleteKit(string kit)
        {
            SQLiteConnection conn = GetDBConnection();
            using (SQLiteTransaction trans = conn.BeginTransaction()) {
                SQLiteCommand upCmd = new SQLiteCommand("DELETE FROM kit_master WHERE kit_no=@kit_no", conn);
                upCmd.Parameters.AddWithValue("@kit_no", kit);
                upCmd.ExecuteNonQuery();
                trans.Commit();
            }
        }

        public static void DeleteAutosomal(string kit_no)
        {
            UpdateDB(@"DELETE from kit_autosomal where kit_no = {0}", kit_no);
        }

        public static void CheckIntegrity()
        {
            SQLiteConnection conn = GetDBConnection();
            SQLiteCommand ss = new SQLiteCommand("select tbl_name from sqlite_master where type='table'", conn);
            SQLiteDataReader reader = ss.ExecuteReader();
            var list = new List<string>();
            while (reader.Read())
                list.Add(reader["tbl_name"].ToString());
            reader.Close();
            ss.Dispose();
            for (int idx = 0; idx < CreateTableSQL.Length; idx += 2) {
                if (!list.Contains(CreateTableSQL[idx])) {
                    ss = new SQLiteCommand(CreateTableSQL[idx + 1], conn);
                    ss.ExecuteNonQuery();
                    ss.Dispose();
                }
            }
            conn.Close();
        }

        private static SQLiteDataReader QueryReader(string table, string[] fields, string conditions)
        {
            string fields_list = string.Join(", ", fields);

            SQLiteConnection conn = GetDBConnection();
            SQLiteCommand ss = new SQLiteCommand("select " + fields_list + " from " + table + " " + conditions, conn);
            return ss.ExecuteReader();
        }

        private static SQLiteDataReader QueryReader(string sql)
        {
            SQLiteConnection conn = GetDBConnection();
            SQLiteCommand ss = new SQLiteCommand(sql, conn);
            return ss.ExecuteReader();
        }

        public static DataTable QueryTable(string table, string[] fields, string conditions)
        {
            string fields_list = string.Join(", ", fields);
            return QueryTable("select " + fields_list + " from " + table + " " + conditions);
        }

        public static DataTable QueryTable(string sql)
        {
            using (SQLiteConnection conn = GetDBConnection())
            using (SQLiteCommand ss = new SQLiteCommand(sql, conn))
            using (SQLiteDataReader reader = ss.ExecuteReader()) {
                DataTable dt = new DataTable();
                dt.Load(reader);
                reader.Close();
                conn.Close();
                return dt;
            }
        }

        public static string QueryValue(string table, string[] fields, string conditions)
        {
            string fields_list = string.Join(", ", fields);

            using (SQLiteConnection conn = GetDBConnection())
            using (SQLiteCommand ss = new SQLiteCommand("select " + fields_list + " from " + table + " " + conditions, conn))
            using (SQLiteDataReader reader = ss.ExecuteReader())
            using (DataTable dt = new DataTable(table)) {
                dt.Load(reader);
                reader.Close();
                conn.Close();
                if (dt.Rows.Count > 0 && dt.Rows[0].ItemArray.Length > 0) {
                    return dt.Rows[0].ItemArray[0].ToString();
                }
                return "";
            }
        }

        private static int UpdateSQL(string sql)
        {
            using (SQLiteConnection conn = GetDBConnection())
            using (SQLiteCommand ss = new SQLiteCommand(sql, conn)) {
                int output = ss.ExecuteNonQuery();
                conn.Close();
                return output;
            }
        }

        public static void UpdateDB(string sql, params object[] values)
        {
            sql = string.Format(sql, values);
            UpdateSQL(sql);
        }

        public static void ClearAllComparisons(bool excludeReferences)
        {
            string upCmd;
            if (excludeReferences)
                upCmd = @"DELETE from cmp_status where kit1 in (select kit_no from kit_master) and kit2 in (select kit_no from kit_master) ";
            else
                upCmd = @"DELETE FROM cmp_status";
            GKSqlFuncs.UpdateSQL(upCmd);
        }

        public static void SaveAutosomalCmp(string kit1, string kit2, List<CmpSegment> segment_idx, bool reference)
        {
            SQLiteConnection cnn = GetDBConnection();

            SQLiteCommand upCmd = new SQLiteCommand(@"DELETE from cmp_status WHERE (kit1=@kit1 AND kit2=@kit2) OR (kit1=@kit3 AND kit2=@kit4)", cnn);
            upCmd.Parameters.AddWithValue("@kit1", kit1);
            upCmd.Parameters.AddWithValue("@kit2", kit2);
            upCmd.Parameters.AddWithValue("@kit3", kit2);
            upCmd.Parameters.AddWithValue("@kit4", kit1);
            upCmd.ExecuteNonQuery();

            var segmentStats = SegmentStats.CalculateSegmentStats(segment_idx);

            upCmd = new SQLiteCommand(@"INSERT OR REPLACE INTO cmp_status (kit1,kit2,status_autosomal,at_longest,at_total,x_longest,x_total,mrca) VALUES (@kit1,@kit2,@status_autosomal,@at_longest,@at_total,@x_longest,@x_total,@mrca)", cnn);
            upCmd.Parameters.AddWithValue("@kit1", kit1);
            upCmd.Parameters.AddWithValue("@kit2", kit2);
            upCmd.Parameters.AddWithValue("@status_autosomal", "1");
            upCmd.Parameters.AddWithValue("@at_longest", segmentStats.Longest);
            upCmd.Parameters.AddWithValue("@at_total", segmentStats.Total);
            upCmd.Parameters.AddWithValue("@x_longest", segmentStats.XLongest);
            upCmd.Parameters.AddWithValue("@x_total", segmentStats.XTotal);
            upCmd.Parameters.AddWithValue("@mrca", segmentStats.Mrca);
            upCmd.ExecuteNonQuery();

            upCmd = new SQLiteCommand(@"SELECT cmp_id FROM cmp_status WHERE kit1=@kit1 AND kit2=@kit2", cnn);
            upCmd.Parameters.AddWithValue("@kit1", kit1);
            upCmd.Parameters.AddWithValue("@kit2", kit2);
            SQLiteDataReader reader = upCmd.ExecuteReader();
            string cmp_id = "-1";
            if (reader.Read())
                cmp_id = reader["cmp_id"].ToString();
            reader.Close();

            for (int i = 0; i < segment_idx.Count; i++) {
                var obj = segment_idx[i];

                string chromosome = obj.Chromosome;
                string start_position = obj.StartPosition.ToString();
                string end_position = obj.EndPosition.ToString();
                string segment_length_cm = obj.SegmentLength_cm.ToString();
                string snp_count = obj.SNPCount.ToString();

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

                if (!reference) {
                    upCmd = new SQLiteCommand(@"SELECT segment_id FROM cmp_autosomal WHERE kit1=@kit1 AND kit2=@kit2 AND chromosome=@chromosome AND start_position=@start_position AND end_position=@end_position", cnn);
                    upCmd.Parameters.AddWithValue("@kit1", kit1);
                    upCmd.Parameters.AddWithValue("@kit2", kit2);
                    upCmd.Parameters.AddWithValue("@chromosome", chromosome);
                    upCmd.Parameters.AddWithValue("@start_position", start_position);
                    upCmd.Parameters.AddWithValue("@end_position", end_position);
                    reader = upCmd.ExecuteReader();
                    string segment_id = "-1";
                    if (reader.Read()) {
                        segment_id = reader.GetInt16(0).ToString();
                    }
                    reader.Close();

                    using (var transaction = cnn.BeginTransaction()) {
                        foreach (var row in obj.Rows) {
                            upCmd = new SQLiteCommand(@"INSERT OR REPLACE INTO cmp_mrca (rsid,chromosome,position,kit1_genotype,kit2_genotype,match,segment_id) values (@rsid,@chromosome,@position,@kit1_genotype,@kit2_genotype,@match,@segment_id)", cnn);
                            upCmd.Parameters.AddWithValue("@rsid", row.RSID);
                            upCmd.Parameters.AddWithValue("@chromosome", row.Chromosome);
                            upCmd.Parameters.AddWithValue("@position", row.Position);
                            upCmd.Parameters.AddWithValue("@kit1_genotype", row.Kit1Genotype);
                            upCmd.Parameters.AddWithValue("@kit2_genotype", row.Kit2Genotype);
                            upCmd.Parameters.AddWithValue("@match", row.Match);
                            upCmd.Parameters.AddWithValue("@segment_id", segment_id);
                            upCmd.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                }
            }
        }

        public static string GetKitName(string kit)
        {
            if (kit_name == null)
                kit_name = new Dictionary<string, string>();
            if (kit_name.ContainsKey(kit))
                return kit_name[kit];
            else {
                DataTable dt = QueryTable("kit_master", new string[] { "name" }, "where kit_no='" + kit + "'");
                if (dt.Rows.Count > 0) {
                    kit_name.Add(kit, dt.Rows[0].ItemArray[0].ToString());
                    return dt.Rows[0].ItemArray[0].ToString();
                }
                return "Unknown";
            }
        }

        public static bool CheckROHExists(string kit)
        {
            string roh = QueryValue("kit_master", new string[] { "roh_status" }, "where kit_no='" + kit + "'");
            if (roh == "0")
                return false;
            else
                return true;
        }

        public static void SaveROHCmp(string kit, IList<ROHSegment> segment_idx)
        {
            using (SQLiteConnection cnn = GetDBConnection()) {
                using (var upCmd = new SQLiteCommand(@"DELETE from kit_roh WHERE kit_no=@kit", cnn)) {
                    upCmd.Parameters.AddWithValue("@kit", kit);
                    upCmd.ExecuteNonQuery();
                    using (SQLiteTransaction trans = cnn.BeginTransaction()) {
                        for (int i = 0; i < segment_idx.Count; i++) {
                            var obj = segment_idx[i];

                            using (var xCmd = new SQLiteCommand(@"INSERT OR REPLACE INTO kit_roh(kit_no,chromosome,start_position,end_position,segment_length_cm,snp_count) values (@kit_no,@chromosome,@start_position,@end_position,@segment_length_cm,@snp_count)", cnn)) {
                                xCmd.Parameters.AddWithValue("@kit_no", kit);
                                xCmd.Parameters.AddWithValue("@chromosome", obj.Chromosome);
                                xCmd.Parameters.AddWithValue("@start_position", obj.StartPosition);
                                xCmd.Parameters.AddWithValue("@end_position", obj.EndPosition);
                                xCmd.Parameters.AddWithValue("@segment_length_cm", obj.SegmentLength_cm);
                                xCmd.Parameters.AddWithValue("@snp_count", obj.SNPCount);
                                xCmd.ExecuteNonQuery();
                            }
                        }
                        trans.Commit();
                    }
                }

                using (var upCmd = new SQLiteCommand(@"UPDATE kit_master SET roh_status=1 WHERE kit_no=@kit_no", cnn)) {
                    upCmd.Parameters.AddWithValue("@kit_no", kit);
                    upCmd.ExecuteNonQuery();
                }

                cnn.Close();
            }
        }

        public static IList<ROHSegment> GetROHCmp(string kit)
        {
            var reader = QueryReader("kit_roh",
                new string[] { "chromosome", "start_position", "end_position", "segment_length_cm", "snp_count" },
                "WHERE kit_no='" + kit + "'");

            using (reader) {
                var result = new List<ROHSegment>();
                while (reader.Read()) {
                    result.Add(new ROHSegment(reader));
                }
                return result;
            }
        }

        public static List<CmpSegment> GetAutosomalCmp(string kit1, string kit2)
        {
            var reader = QueryReader(
                "cmp_autosomal",
                new string[] { "segment_id", "chromosome", "start_position", "end_position", "segment_length_cm", "snp_count" },
                "WHERE (kit1='" + kit1 + "' AND kit2='" + kit2 + "') OR (kit1='" + kit2 + "' AND kit2='" + kit1 + "')");

            using (reader) {
                var result = new List<CmpSegment>();
                while (reader.Read()) {
                    result.Add(new CmpSegment(reader));
                }
                return result;
            }
        }

        public static bool IsPhased(string kit)
        {
            DataTable dt = QueryTable("select kit_no from kit_phased where kit_no='" + kit + "'");
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        public static bool CheckAlreadyCompared(string kit1, string kit2)
        {
            using (var tbl = QueryTable("cmp_status", new string[] { "status_autosomal" }, "WHERE (kit1='" + kit1 + "' AND kit2='" + kit2 + "') OR (kit1='" + kit2 + "' AND kit2='" + kit1 + "')")) {
                return tbl.Rows.Count != 0;
            }
        }

        public static IList<CmpSegmentRow> GetCmpSeg(int segmentId)
        {
            var reader = QueryReader(
                "cmp_mrca",
                new string[] { "rsid", "chromosome", "position", "kit1_genotype", "kit2_genotype", "match" },
                "WHERE segment_id='" + segmentId + "'");

            using (reader) {
                var result = new List<CmpSegmentRow>();
                while (reader.Read()) {
                    result.Add(new CmpSegmentRow(reader));
                }
                return result;
            }
        }

        public static IList<SingleSNP> GetROHSeg(string kit, string chromosome, int startPos, int endPos)
        {
            var reader = QueryReader(
                        "select rsid, chromosome, position, genotype from kit_autosomal " +
                        "where kit_no='" + kit + "' and chromosome='" + chromosome + "' and position>=" + startPos + " and position<=" + endPos + " order by position");

            using (reader) {
                var result = new List<SingleSNP>();
                while (reader.Read()) {
                    result.Add(new SingleSNP(reader));
                }
                return result;
            }
        }

        public static IList<SingleSNP> GetAutosomal(string kit)
        {
            var reader = QueryReader(
                        @"SELECT rsid, chromosome, position, genotype from kit_autosomal where kit_no=" + kit + " order by chromosome,position");

            using (reader) {
                var result = new List<SingleSNP>();
                while (reader.Read()) {
                    result.Add(new SingleSNP(reader));
                }
                return result;
            }
        }

        public static IList<PhaseSegment> GetPhaseSegments(string unphased_kit, string start_position, string end_position, string chromosome, string phased_kit)
        {
            string query =
                "select a.position,a.genotype,p.paternal_genotype,p.maternal_genotype from kit_autosomal a,kit_phased p where a.kit_no='" + unphased_kit +
                "' and a.position>" + start_position + " and a.position<" + end_position + " and a.chromosome='" + chromosome +
                "' and p.rsid=a.rsid and p.kit_no='" + phased_kit + "' order by a.position";

            var reader = QueryReader(query);

            using (reader) {
                var result = new List<PhaseSegment>();
                while (reader.Read()) {
                    result.Add(new PhaseSegment(reader));
                }
                return result;
            }
        }

        public static IList<PhaseRow> GetPhaseRows(string father_kit, string mother_kit, string child_kit)
        {
            string query = "";

            if (father_kit != "Unknown" && mother_kit != "Unknown")
                query = ("SELECT c.[rsid]'RSID',c.[chromosome]'Chromosome',c.[position]'Position',c.[genotype]\"Child\",COALESCE(f.[genotype],'--')\"Father\",COALESCE(m.[genotype],'--')\"Mother\",''\"Phased Paternal\",''\"Phased Maternal\"  FROM kit_autosomal c left outer join kit_autosomal f,kit_autosomal m on f.rsid=c.rsid AND m.rsid=c.rsid WHERE c.kit_no='" + child_kit + "' AND f.kit_no='" + father_kit + "' AND m.[kit_no]='" + mother_kit + "' ORDER BY cast(c.chromosome as integer),c.position");
            else if (father_kit != "Unknown" && mother_kit == "Unknown")
                query = ("SELECT c.[rsid]'RSID',c.[chromosome]'Chromosome',c.[position]'Position',c.[genotype]\"Child\",COALESCE(f.[genotype],'--')\"Father\",'--'\"Mother\",''\"Phased Paternal\",''\"Phased Maternal\"  FROM kit_autosomal c left outer join kit_autosomal f on f.rsid=c.rsid  WHERE c.kit_no='" + child_kit + "' AND f.kit_no='" + father_kit + "' ORDER BY cast(c.chromosome as integer),c.position");
            else if (father_kit == "Unknown" && mother_kit != "Unknown")
                query = ("SELECT c.[rsid]'RSID',c.[chromosome]'Chromosome',c.[position]'Position',c.[genotype]\"Child\",'--'\"Father\",COALESCE(m.[genotype],'--')\"Mother\" ,''\"Phased Paternal\",''\"Phased Maternal\"  FROM kit_autosomal c left outer join kit_autosomal m on m.rsid=c.rsid WHERE c.kit_no='" + child_kit + "' AND m.kit_no='" + mother_kit + "' ORDER BY cast(c.chromosome as integer),c.position");

            var reader = QueryReader(query);

            using (reader) {
                var result = new List<PhaseRow>();
                while (reader.Read()) {
                    result.Add(new PhaseRow(reader));
                }
                return result;
            }
        }

        public static void SavePhasedKit(string father_kit, string mother_kit, string child_kit, IList<PhaseRow> dt)
        {
            UpdateDB("DELETE FROM kit_phased where kit_no = {0}", child_kit);

            var conn = GetDBConnection();
            using (var trans = conn.BeginTransaction()) {
                foreach (var row in dt) {
                    string phasedPaternal = ("" + row.PhasedPaternal).Trim();
                    if (string.IsNullOrEmpty(phasedPaternal)) phasedPaternal = string.Empty;

                    string phasedMaternal = ("" + row.PhasedMaternal).Trim();
                    if (string.IsNullOrEmpty(phasedMaternal)) phasedMaternal = string.Empty;

                    var cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT OR REPLACE INTO kit_phased(kit_no,rsid,chromosome,position,paternal_genotype,maternal_genotype,paternal_kit_no,maternal_kit_no) VALUES (@kit_no,@rsid,@chromosome,@position,@paternal_genotype,@maternal_genotype,@paternal_kit_no,@maternal_kit_no)";
                    cmd.Parameters.AddWithValue("@kit_no", child_kit);
                    cmd.Parameters.AddWithValue("@rsid", row.RSID);
                    cmd.Parameters.AddWithValue("@chromosome", row.Chromosome);
                    cmd.Parameters.AddWithValue("@position", row.Position);
                    cmd.Parameters.AddWithValue("@paternal_genotype", phasedPaternal);
                    cmd.Parameters.AddWithValue("@maternal_genotype", phasedMaternal);
                    if (father_kit == "Unknown")
                        cmd.Parameters.AddWithValue("@paternal_kit_no", "");
                    else
                        cmd.Parameters.AddWithValue("@paternal_kit_no", father_kit);
                    if (mother_kit == "Unknown")
                        cmd.Parameters.AddWithValue("@maternal_kit_no", "");
                    else
                        cmd.Parameters.AddWithValue("@maternal_kit_no", mother_kit);
                    cmd.ExecuteNonQuery();
                }
                trans.Commit();
            }
        }

        public static IList<OTORow> GetOTORows(string kit1, string kit2)
        {
            string query =
                @"select rsid, chr, pos, gt1, gt2, count(*) FROM (SELECT kit1.rsid 'rsid',kit1.chromosome 'chr',kit1.position 'pos',kit1.genotype 'gt1',kit2.genotype 'gt2' FROM kit_autosomal kit1 LEFT JOIN kit_autosomal kit2 on kit1.rsid=kit2.rsid WHERE kit1.kit_no = '" + kit1 + "' AND kit2.kit_no = '" + kit2 + "' UNION SELECT kit1.rsid 'rsid',kit1.chromosome 'chr',kit1.position 'pos',kit1.genotype 'gt1', kit2.genotype 'gt2' FROM kit_autosomal kit1 LEFT JOIN kit_autosomal kit2 on kit1.rsid=kit2.rsid WHERE kit1.kit_no = '" + kit2 + "' AND kit2.kit_no = '" + kit1 + "') GROUP BY rsid ORDER BY CAST(chr as INTEGER),pos";

            var reader = QueryReader(query);

            using (reader) {
                var result = new List<OTORow>();
                while (reader.Read()) {
                    result.Add(new OTORow(reader));
                }
                return result;
            }
        }

        public static IList<SingleSNP> GetROHRows(string kit)
        {
            string query =
                @"select rsid, chromosome, position, genotype from kit_autosomal where kit_no='" + kit + "' order by cast(chromosome as integer), position";

            var reader = QueryReader(query);

            using (reader) {
                var result = new List<SingleSNP>();
                while (reader.Read()) {
                    result.Add(new SingleSNP(reader));
                }
                return result;
            }
        }

        public static IList<YSTR> GetYSTR(string kit)
        {
            string query =
                @"SELECT marker, value from kit_ystr where kit_no = " + kit;

            var reader = QueryReader(query);

            using (reader) {
                var result = new List<YSTR>();
                while (reader.Read()) {
                    result.Add(new YSTR(reader));
                }
                return result;
            }
        }

        public static IList<AdmixtureRec> GetAdmixture(string kit, string limit = "3")
        {
            string query =
                "select name, at_total, at_longest, x, y FROM (select b.name,a.at_total,a.at_longest,b.x,b.y from cmp_status a,kit_master b where a.kit1='" + kit + "' and a.kit2=b.kit_no and a.status_autosomal=1 and a.at_longest<" + limit + " and a.at_total!=0 UNION select b.name,a.at_total,a.at_longest,b.x,b.y from cmp_status a,kit_master b where a.kit2='" + kit + "' and a.kit1=b.kit_no and a.status_autosomal=1 and a.at_longest<" + limit + " and a.at_total!=0) ORDER BY at_total DESC";
            //  "select name, at_total, at_longest, x, y FROM (select b.name,a.at_total,a.at_longest,b.x,b.y from cmp_status a,kit_master b where a.kit1='" + kit + "' and a.kit2=b.kit_no and a.kit2 like 'HGDP%' and a.status_autosomal=1 and a.at_longest<3 and a.at_total!=0 UNION select b.name,a.at_total,a.at_longest,b.x,b.y from cmp_status a,kit_master b where a.kit2='" + kit + "' and a.kit1=b.kit_no and a.kit1 like 'HGDP%' and a.status_autosomal=1 and a.at_longest<3 and a.at_total!=0) ORDER BY at_total DESC";

            var reader = QueryReader(query);

            using (reader) {
                var result = new List<AdmixtureRec>();
                while (reader.Read()) {
                    result.Add(new AdmixtureRec(reader));
                }
                return result;
            }
        }

        public static void SaveKitMtDNA(string kit_no, string mutations, string fasta)
        {
            using (var conn = GetDBConnection()) {
                SQLiteCommand upCmd = new SQLiteCommand(@"INSERT OR REPLACE INTO kit_mtdna(kit_no, mutations,fasta)values(@kit_no,@mutations,@fasta)", conn);
                upCmd.Parameters.AddWithValue("@kit_no", kit_no);
                upCmd.Parameters.AddWithValue("@mutations", mutations);
                upCmd.Parameters.AddWithValue("@fasta", fasta);
                upCmd.ExecuteNonQuery();
            }
        }

        public static void SaveKitYSNPs(string kit_no, string ysnps_list)
        {
            using (var conn = GetDBConnection()) {
                SQLiteCommand upCmd = new SQLiteCommand(@"INSERT OR REPLACE INTO kit_ysnps(kit_no, ysnps) values (@kit_no,@ysnps)", conn);
                upCmd.Parameters.AddWithValue("@kit_no", kit_no);
                upCmd.Parameters.AddWithValue("@ysnps", ysnps_list);
                upCmd.ExecuteNonQuery();
            }
        }

        public static void UpdateKit(string kit_no, string name, string sex)
        {
            using (var conn = GetDBConnection()) {
                SQLiteCommand upCmd = new SQLiteCommand(@"UPDATE kit_master SET name=@name, sex=@sex WHERE kit_no=@kit_no", conn);
                upCmd.Parameters.AddWithValue("@name", name);
                upCmd.Parameters.AddWithValue("@sex", sex[0].ToString());
                upCmd.Parameters.AddWithValue("@kit_no", kit_no);
                upCmd.ExecuteNonQuery();
            }
        }

        public static void InsertKit(string kit_no, string name, string sex)
        {
            using (var conn = GetDBConnection()) {
                SQLiteCommand upCmd = new SQLiteCommand(@"INSERT OR REPLACE INTO kit_master(kit_no, name, sex)values(@kit_no,@name,@sex)", conn);
                upCmd.Parameters.AddWithValue("@kit_no", kit_no);
                upCmd.Parameters.AddWithValue("@name", name);
                upCmd.Parameters.AddWithValue("@sex", sex[0].ToString());
                upCmd.ExecuteNonQuery();
            }
        }

        public static void SaveKit(string kit_no, string name, string sex, bool disabled, string x, string y)
        {
            using (var conn = GetDBConnection()) {
                SQLiteCommand upCmd = new SQLiteCommand("UPDATE kit_master set name=@name, sex=@sex, disabled=@disabled, x=@x, y=@y WHERE kit_no=@kit_no", conn);
                upCmd.Parameters.AddWithValue("@name", name);

                if (sex == "Unknown")
                    upCmd.Parameters.AddWithValue("@sex", "U");
                else if (sex == "Male")
                    upCmd.Parameters.AddWithValue("@sex", "M");
                else if (sex == "Female")
                    upCmd.Parameters.AddWithValue("@sex", "F");

                if (disabled)
                    upCmd.Parameters.AddWithValue("@disabled", "1");
                else
                    upCmd.Parameters.AddWithValue("@disabled", "0");

                upCmd.Parameters.AddWithValue("@x", x);
                upCmd.Parameters.AddWithValue("@y", y);

                upCmd.Parameters.AddWithValue("@kit_no", kit_no);
                upCmd.ExecuteNonQuery();
            }
        }

        public static IList<KitDTO> QueryKits()
        {
            var result = new List<KitDTO>();

            using (SQLiteConnection cnn = GetDBConnection())
            using (SQLiteCommand query = new SQLiteCommand(@"SELECT kit_no, name, sex, disabled, coalesce(x, 0), coalesce(y, 0), last_modified FROM kit_master order by last_modified DESC", cnn))
            using (SQLiteDataReader reader = query.ExecuteReader())
                while (reader.Read()) {
                    result.Add(new KitDTO(reader, true, true));
                }

            return result;
        }

        public static string GetYSNPs(string kit)
        {
            using (var conn = GetDBConnection()) {
                var query = new SQLiteCommand(@"SELECT ysnps from kit_ysnps where kit_no=@kit_no", conn);
                query.Parameters.AddWithValue("@kit_no", kit);
                var reader = query.ExecuteReader();
                if (reader.Read()) {
                    return reader.GetString(0);
                }
                reader.Close();
                query.Dispose();
            }
            return string.Empty;
        }

        public static void GetMtDNA(string kit, out string mutations, out string fasta)
        {
            using (var conn = GetDBConnection()) {
                var query = new SQLiteCommand(@"SELECT mutations,fasta from kit_mtdna where kit_no=@kit_no", conn);
                query.Parameters.AddWithValue("@kit_no", kit);
                var reader = query.ExecuteReader();
                if (reader.Read()) {
                    mutations = reader.GetString(0);
                    fasta = reader.GetString(1);
                }
                reader.Close();
                query.Dispose();
            }

            mutations = string.Empty;
            fasta = string.Empty;
        }

        public static void GetKit(string kitNo, out string name, out string sex)
        {
            using (var conn = GetDBConnection()) {
                SQLiteCommand query = new SQLiteCommand(@"SELECT name, sex from kit_master where kit_no=@kit_no", conn);
                query.Parameters.AddWithValue("@kit_no", kitNo);
                SQLiteDataReader reader = query.ExecuteReader();
                if (reader.Read()) {
                    name = reader.GetString(0);
                    sex = reader.GetString(1);
                }
                reader.Close();
                query.Dispose();
            }

            name = string.Empty;
            sex = string.Empty;
        }

        public static void SaveAutosomal(string kit_no, DataGridViewRowCollection rows)
        {
            DeleteAutosomal(kit_no);

            using (var conn = GetDBConnection()) {
                SQLiteCommand upCmd = new SQLiteCommand(@"INSERT OR REPLACE INTO kit_autosomal(kit_no, rsid, chromosome, position, genotype) values (@kit_no, @rsid, @chromosome, @position, @genotype)", conn);
                using (var transaction = conn.BeginTransaction()) {
                    bool incomplete = false;
                    foreach (DataGridViewRow row in rows) {
                        if (row.IsNewRow)
                            continue;

                        incomplete = false;
                        for (int c = 0; c < row.Cells.Count; c++)
                            if (row.Cells[c].Value == null) {
                                incomplete = true;
                                break;
                            } else if (row.Cells[c].Value.ToString().Trim() == "") {
                                incomplete = true;
                                break;
                            }

                        if (incomplete)
                            continue;

                        upCmd.Parameters.AddWithValue("@kit_no", kit_no);
                        upCmd.Parameters.AddWithValue("@rsid", row.Cells[0].Value.ToString());
                        upCmd.Parameters.AddWithValue("@chromosome", row.Cells[1].Value.ToString());
                        upCmd.Parameters.AddWithValue("@position", row.Cells[2].Value.ToString());
                        upCmd.Parameters.AddWithValue("@genotype", row.Cells[3].Value.ToString());
                        upCmd.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
        }

        public static void SaveYSTR(string kit_no, DataGridViewRow[] yRows)
        {
            using (var conn = GetDBConnection()) {
                SQLiteCommand upCmd = new SQLiteCommand(@"delete from kit_ystr where kit_no=@kit_no", conn);
                upCmd.Parameters.AddWithValue("@kit_no", kit_no);
                upCmd.ExecuteNonQuery();

                upCmd = new SQLiteCommand(@"insert or replace into kit_ystr(kit_no, marker, value) values (@kit_no, @marker, @value)", conn);
                using (var transaction = conn.BeginTransaction()) {
                    foreach (DataGridViewRow row in yRows) {
                        if (row.IsNewRow || row.Cells[1].Value.ToString().Trim() == "") continue;

                        upCmd.Parameters.AddWithValue("@kit_no", kit_no);
                        upCmd.Parameters.AddWithValue("@marker", row.Cells[0].Value.ToString());
                        upCmd.Parameters.AddWithValue("@value", row.Cells[1].Value.ToString());
                        upCmd.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                upCmd.Dispose();
            }
        }
    }
}

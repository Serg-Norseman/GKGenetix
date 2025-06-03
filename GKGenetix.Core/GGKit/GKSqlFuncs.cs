/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using GKGenetix.Core.Model;

namespace GGKit.Core
{
    public static class GKSqlFuncs
    {
        #region Tables

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
            "kit_master", TABLE_kit_master_CREATE_SQL,
            "kit_autosomal", TABLE_kit_autosomal_CREATE_SQL,
            "kit_mtdna", TABLE_kit_mtdna_CREATE_SQL,
            "kit_ysnps", TABLE_kit_ysnps_CREATE_SQL,
            "kit_ystr", TABLE_kit_ystr_CREATE_SQL,
            "ggk_settings", TABLE_ggk_settings_CREATE_SQL,
            "cmp_autosomal", TABLE_cmp_autosomal_CREATE_SQL,
            "cmp_mrca", TABLE_cmp_mrca_CREATE_SQL,
            "cmp_status", TABLE_cmp_status_CREATE_SQL,
            "kit_roh", TABLE_kit_roh_CREATE_SQL,
            "kit_phased", TABLE_kit_phased_CREATE_SQL,
            "cmp_phased", TABLE_cmp_phased_CREATE_SQL
        };

        #endregion

        private static SQLiteConnection _connection;
        private static IKitHost _host;

        public static void SetHost(IKitHost host)
        {
            _host = host;
        }

        #region Common database

        private static void ResetTables()
        {
            if (File.Exists(SQLITE_DB))
                File.Move(SQLITE_DB, SQLITE_DB + "-" + DateTime.Now.Ticks.ToString("X"));

            using (var connection = new SQLiteConnection(@"Data Source=" + SQLITE_DB + @";Version=3; Compress=True; New=True; PRAGMA foreign_keys = ON; PRAGMA auto_vacuum = FULL;")) {
                connection.Open();

                Dictionary<string, string> pragma = new Dictionary<string, string> {
                    { "foreign_keys", "ON" },
                    { "auto_vacuum", "FULL" },
                    { "journal_mode", "WAL" }
                };

                using (SQLiteTransaction trans = connection.BeginTransaction()) {
                    foreach (string key in pragma.Keys) {
                        ExecCmd("PRAGMA " + key + " = " + pragma[key] + ";", connection, trans);
                    }

                    for (int idx = 0; idx < CreateTableSQL.Length; idx += 2) {
                        ExecCmd(CreateTableSQL[idx + 1], connection, trans);
                    }

                    trans.Commit();
                }
            }
        }

        private static void CheckConnection()
        {
            if (_connection != null) return;

            if (File.Exists(SQLITE_DB)) {
                string connStr = @"Data Source=" + SQLITE_DB + @";Version=3; Compress=True; PRAGMA foreign_keys = ON; PRAGMA auto_vacuum = FULL;";

                _connection = new SQLiteConnection(connStr);
                _connection.Open();

                Dictionary<string, string> pragma = new Dictionary<string, string> {
                    { "foreign_keys", "ON" },
                    { "auto_vacuum", "FULL" },
                    { "journal_mode", "WAL" }
                };

                foreach (string key in pragma.Keys) {
                    ExecCmd("PRAGMA " + key + " = " + pragma[key] + ";", _connection);
                }
            } else {
                if (_host.ShowQuestion("Data file ggk.db doesn't exist. If this is the first time you are opening the software, you can ignore this error. Do you wish to create one?")) {
                    ResetTables();
                    CheckConnection();
                } else _host.Exit();
            }
        }

        public static void CheckIntegrity()
        {
            CheckConnection();

            var list = new List<string>();

            using (SQLiteCommand ss = new SQLiteCommand("select tbl_name from sqlite_master where type='table'", _connection))
            using (SQLiteDataReader reader = ss.ExecuteReader()) {
                while (reader.Read())
                    list.Add(reader["tbl_name"].ToString());
            }

            for (int idx = 0; idx < CreateTableSQL.Length; idx += 2) {
                if (!list.Contains(CreateTableSQL[idx])) {
                    ExecCmd(CreateTableSQL[idx + 1], _connection);
                }
            }

        }

        private static string QueryValue(string sql)
        {
            CheckConnection();

            using (SQLiteCommand ss = new SQLiteCommand(sql, _connection))
            using (SQLiteDataReader reader = ss.ExecuteReader()) {
                while (reader.Read()) {
                    if (reader.FieldCount > 0) {
                        return Convert.ToString(reader[0]);
                    }
                }
            }
            return null;
        }

        private static void ExecCmd(string command, SQLiteConnection conn, SQLiteTransaction trans = null)
        {
            using (var cmd = new SQLiteCommand(command, conn, trans))
                cmd.ExecuteNonQuery();
        }

        private static IList<T> GetRows<T>(string query) where T : ITableRow, new()
        {
            CheckConnection();

            using (SQLiteCommand ss = new SQLiteCommand(query, _connection))
            using (SQLiteDataReader reader = ss.ExecuteReader()) {
                var result = new List<T>();
                while (reader.Read()) {
                    var row = new T();
                    row.Load(reader);
                    result.Add(row);
                }
                return result;
            }
        }

        #endregion

        #region Kits

        public static string GetKitName(string kit)
        {
            string kitName = QueryValue($"select name from kit_master where kit_no='{kit}'");
            return !string.IsNullOrEmpty(kitName) ? kitName : "Unknown";
        }

        public static void DeleteKit(string kit)
        {
            CheckConnection();

            using (SQLiteTransaction trans = _connection.BeginTransaction()) {
                ExecCmd($"delete from kit_master where kit_no = '{kit}'", _connection, trans);
                trans.Commit();
            }
        }

        public static IList<KitDTO> QueryKits(bool excludeDisabled = false, bool excludeRefs = true, char requestedSex = 'U')
        {
            string where = "";
            if (excludeDisabled)
                where = "where disabled = 0";

            if (excludeRefs) {
                if (string.IsNullOrEmpty(where))
                    where = " where reference = 0";
                else where += " and reference = 0";
            }

            if (requestedSex != 'U') {
                if (string.IsNullOrEmpty(where))
                    where = $" where sex = '{requestedSex}'";
                else where += $" and sex = '{requestedSex}'";
            }

            string sql = $"select kit_no, name, sex, disabled, coalesce(x, 0), coalesce(y, 0), last_modified, reference, roh_status from kit_master {where} order by last_modified desc";
            return GetRows<KitDTO>(sql);
        }

        public static void GetKit(string kitNo, out string name, out string sex)
        {
            CheckConnection();

            using (SQLiteCommand query = new SQLiteCommand($"select name, sex from kit_master where kit_no = '{kitNo}'", _connection))
            using (SQLiteDataReader reader = query.ExecuteReader()) {
                if (reader.Read()) {
                    name = reader.GetString(0);
                    sex = reader.GetString(1);
                    return;
                }
            }

            name = string.Empty;
            sex = string.Empty;
        }

        public static void UpdateKit(string kit_no, string name, string sex)
        {
            CheckConnection();

            ExecCmd($"update kit_master set name = '{name}', sex = '{sex[0]}' where kit_no = '{kit_no}'", _connection);
        }

        public static void InsertKit(string kit_no, string name, string sex)
        {
            CheckConnection();

            ExecCmd($"insert or replace into kit_master (kit_no, name, sex) values ('{kit_no}', '{name}', '{sex[0]}')", _connection);
        }

        public static void SaveKit(string kit_no, string name, string sex, bool disabled, string x, string y)
        {
            CheckConnection();

            sex = sex[0].ToString();
            string dis = (disabled) ? "1" : "0";
            ExecCmd($"update kit_master set name = '{name}', sex = '{sex}', disabled = {dis}, x = {x}, y = {y} where kit_no = '{kit_no}'", _connection);
        }

        #endregion

        #region Common comparisons

        public static void ClearAllComparisons(bool excludeReferences)
        {
            string upCmd;
            if (excludeReferences)
                upCmd = @"delete from cmp_status where kit1 in (select kit_no from kit_master) and kit2 in (select kit_no from kit_master) ";
            else
                upCmd = @"delete from cmp_status";

            CheckConnection();

            ExecCmd(upCmd, _connection);
        }

        public static bool CheckAlreadyCompared(string kit1, string kit2)
        {
            var val = QueryValue($"select status_autosomal from cmp_status where (kit1 = '{kit1}' and kit2 = '{kit2}') or (kit1 = '{kit2}' and kit2 = '{kit1}')");
            return !string.IsNullOrEmpty(val);
        }

        public static IList<CmpSegmentRow> GetCmpSeg(int segmentId)
        {
            return GetRows<CmpSegmentRow>(
                $"select rsid, chromosome, position, kit1_genotype, kit2_genotype, match from cmp_mrca where segment_id = '{segmentId}'");
        }

        public static IList<AdmixtureRec> GetAdmixture(string kit, string limit = "< 3")
        {
            //  "select name, at_total, at_longest, x, y FROM (select b.name,a.at_total,a.at_longest,b.x,b.y from cmp_status a,kit_master b where a.kit1='" + kit + "' and a.kit2=b.kit_no and a.kit2 like 'HGDP%' and a.status_autosomal=1 and a.at_longest<3 and a.at_total!=0 UNION select b.name,a.at_total,a.at_longest,b.x,b.y from cmp_status a,kit_master b where a.kit2='" + kit + "' and a.kit1=b.kit_no and a.kit1 like 'HGDP%' and a.status_autosomal=1 and a.at_longest<3 and a.at_total!=0) ORDER BY at_total DESC";

            return GetRows<AdmixtureRec>(
                "select name, at_total, at_longest, x, y from (" +
                $"select b.name,a.at_total,a.at_longest,b.x,b.y from cmp_status a,kit_master b where a.kit1='{kit}' and a.kit2=b.kit_no and a.status_autosomal=1 and a.at_longest {limit} and a.at_total != 0 " +
                $"union select b.name,a.at_total,a.at_longest,b.x,b.y from cmp_status a,kit_master b where a.kit2='{kit}' and a.kit1=b.kit_no and a.status_autosomal=1 and a.at_longest {limit} and a.at_total != 0" +
                ") order by at_total desc");
        }

        #endregion

        #region Autosomal

        public static void DeleteAutosomal(string kit_no)
        {
            CheckConnection();

            ExecCmd($"delete from kit_autosomal where kit_no = '{kit_no}'", _connection);
        }

        public static void SaveAutosomalCmp(string kit1, string kit2, IList<CmpSegment> segment_idx, bool reference)
        {
            CheckConnection();

            ExecCmd($"delete from cmp_status where (kit1 = '{kit1}' and kit2 = '{kit2}') or (kit1 = '{kit2}' and kit2 = '{kit1}')", _connection);

            var stats = SegmentStats.CalculateSegmentStats(segment_idx);

            ExecCmd(
                @"insert or replace into cmp_status (kit1, kit2, status_autosomal, at_longest, at_total, x_longest, x_total, mrca) " +
                $"values ('{kit1}', '{kit2}', 1, {stats.Longest}, {stats.Total}, {stats.XLongest}, {stats.XTotal}, {stats.Mrca})", _connection);

            string cmp_id = QueryValue($"select cmp_id from cmp_status where kit1 = '{kit1}' and kit2 = '{kit2}'");

            for (int i = 0; i < segment_idx.Count; i++) {
                var obj = segment_idx[i];

                var chromosome = obj.Chromosome;
                string start_position = obj.StartPosition.ToString();
                string end_position = obj.EndPosition.ToString();
                string segment_length_cm = obj.SegmentLength_cm.ToString();
                string snp_count = obj.SNPCount.ToString();

                ExecCmd(
                    @"insert or replace into cmp_autosomal (cmp_id, kit1, kit2, chromosome, start_position, end_position, segment_length_cm, snp_count) " +
                    $"values ({cmp_id}, '{kit1}', '{kit2}', '{chromosome}', {start_position}, {end_position}, {segment_length_cm}, {snp_count})", _connection);

                if (!reference) {
                    var segment_id = QueryValue(
                        @"select segment_id from cmp_autosomal " +
                        $"where kit1 = '{kit1}' and kit2 = '{kit2}' and chromosome = '{chromosome}' and start_position = '{start_position}' and end_position = '{end_position}'");

                    using (var transaction = _connection.BeginTransaction()) {
                        foreach (var row in obj.Rows) {
                            ExecCmd(
                                "insert or replace into cmp_mrca (rsid, chromosome, position, kit1_genotype, kit2_genotype, match, segment_id) " +
                                $"values ('{row.rsID}', '{row.Chromosome}', {row.Position}, '{row.Genotype1}', '{row.Genotype2}', '{row.Match}', '{segment_id}')", _connection, transaction);
                        }
                        transaction.Commit();
                    }
                }
            }
        }

        public static IList<CmpSegment> GetAutosomalCmp(string kit1, string kit2)
        {
            return GetRows<CmpSegment>(
                $"select segment_id, chromosome, start_position, end_position, segment_length_cm, snp_count from cmp_autosomal " +
                $"where (kit1 = '{kit1}' and kit2 = '{kit2}') or (kit1 = '{kit2}' and kit2 = '{kit1}')");
        }

        public static IList<CmpSegment> GetAutosomalCmp(int cmp_id)
        {
            return GetRows<CmpSegment>(
                $"select segment_id, chromosome, start_position, end_position, segment_length_cm, snp_count from cmp_autosomal where cmp_id = '{cmp_id}'");
        }

        public static IList<SNP> GetAutosomal(string kit)
        {
            return GetRows<SNP>(
                $"select rsid, chromosome, position, genotype from kit_autosomal where kit_no = '{kit}' order by chromosome, position");
        }

        public static IList<OTORow> GetOTORows(string kit1, string kit2)
        {
            return GetRows<OTORow>(
                @"select rsid, chr, pos, gt1, gt2, count(*) from (" +
                "select kit1.rsid 'rsid', kit1.chromosome 'chr', kit1.position 'pos', kit1.genotype 'gt1', kit2.genotype 'gt2' from kit_autosomal kit1 " +
                $"left join kit_autosomal kit2 on kit1.rsid=kit2.rsid where kit1.kit_no = '{kit1}' and kit2.kit_no = '{kit2}' " +
                "union select kit1.rsid 'rsid', kit1.chromosome 'chr', kit1.position 'pos', kit1.genotype 'gt1', kit2.genotype 'gt2' from kit_autosomal kit1 " +
                $"left join kit_autosomal kit2 on kit1.rsid=kit2.rsid where kit1.kit_no = '{kit2}' and kit2.kit_no = '{kit1}'" +
                ") group by rsid order by cast(chr as integer), pos");
        }

        public static void SaveAutosomal(string kit_no, List<SNP> rows)
        {
            DeleteAutosomal(kit_no);

            CheckConnection();

            using (var transaction = _connection.BeginTransaction()) {
                foreach (var row in rows) {
                    var rsid = row.rsID;
                    var chr = row.Chromosome.ToString();
                    var pos = row.Position.ToString();
                    var gt = row.Genotype.ToString();

                    if (string.IsNullOrEmpty(rsid) || string.IsNullOrEmpty(chr) || string.IsNullOrEmpty(pos) || string.IsNullOrEmpty(gt))
                        continue;

                    ExecCmd($"insert or replace into kit_autosomal(kit_no, rsid, chromosome, position, genotype) values ('{kit_no}', '{rsid}', '{chr}', {pos}, '{gt}')", _connection, transaction);
                }

                transaction.Commit();
            }
        }

        #endregion

        #region RoH

        public static bool CheckROHExists(string kit)
        {
            string roh = QueryValue($"select roh_status from kit_master where kit_no = '{kit}'");
            return (roh != "0");
        }

        public static void SaveROHCmp(string kit, IList<ROHSegment> segment_idx)
        {
            CheckConnection();

            ExecCmd($"delete from kit_roh where kit_no = '{kit}'", _connection);

            using (SQLiteTransaction trans = _connection.BeginTransaction()) {
                for (int i = 0; i < segment_idx.Count; i++) {
                    var obj = segment_idx[i];

                    ExecCmd(
                        "insert or replace into kit_roh (kit_no, chromosome, start_position, end_position, segment_length_cm, snp_count) " +
                        $"values ('{kit}', '{obj.Chromosome}', {obj.StartPosition}, {obj.EndPosition}, {obj.SegmentLength_cm}, {obj.SNPCount})",
                        _connection, trans);
                }

                trans.Commit();
            }

            ExecCmd($"update kit_master set roh_status = 1 where kit_no = '{kit}'", _connection);
        }

        public static IList<ROHSegment> GetROHCmp(string kit)
        {
            return GetRows<ROHSegment>($"select chromosome, start_position, end_position, segment_length_cm, snp_count from kit_roh where kit_no = '{kit}'");
        }

        public static IList<SNP> GetROHSeg(string kit, byte chromosome, int startPos, int endPos)
        {
            return GetRows<SNP>(
                "select rsid, chromosome, position, genotype from kit_autosomal " +
                $"where kit_no = '{kit}' and chromosome = '{chromosome}' and position >= {startPos} and position <= {endPos} order by position");
        }

        public static IList<SNP> GetROHRows(string kit)
        {
            return GetRows<SNP>($"select rsid, chromosome, position, genotype from kit_autosomal where kit_no = '{kit}' order by cast(chromosome as integer), position");
        }

        #endregion

        #region Phasing

        public static bool IsPhased(string kit)
        {
            var val = QueryValue($"select kit_no from kit_phased where kit_no = '{kit}'");
            return !string.IsNullOrEmpty(val);
        }

        public static IList<UnphasedSegment> GetUnphasedSegments(string phasedKit)
        {
            return GetRows<UnphasedSegment>(
                "select unphased_kit, chromosome, start_position, end_position from (" +
                $"select kit1'unphased_kit', chromosome, start_position, end_position from cmp_autosomal where kit2='{phasedKit}' " +
                $"union select kit2'unphased_kit', chromosome, start_position, end_position from cmp_autosomal where kit1='{phasedKit}'" +
                ") order by cast(chromosome as integer), start_position");
        }

        public static bool HasUnphasedSegment(string phasedKit, string unphasedKit, byte chromosome, int startPosition, int endPosition)
        {
            var val = QueryValue(
                $"select phased_kit from cmp_phased where phased_kit='{phasedKit}' and match_kit='{unphasedKit}' and chromosome='{chromosome}' and start_position={startPosition} and end_position={endPosition}");
            return !string.IsNullOrEmpty(val);
        }

        public static IList<PhaseSegment> GetPhaseSegments(string phasedKit, string unphasedKit, byte chromosome, int startPosition, int endPosition)
        {
            return GetRows<PhaseSegment>(
                $"select a.position, a.genotype, p.paternal_genotype, p.maternal_genotype from kit_autosomal a, kit_phased p where a.kit_no = '{unphasedKit}' " +
                $"and a.position > {startPosition} and a.position < {endPosition} and a.chromosome = '{chromosome}' " +
                $"and p.rsid = a.rsid and p.kit_no = '{phasedKit}' order by a.position");
        }

        public static IList<PhaseRow> GetPhaseRows(string fatherKit, string motherKit, string childKit)
        {
            string query = "";

            // [rsid], [chromosome], [position], "Child", "Father", "Mother", "Phased Paternal", "Phased Maternal"
            if (fatherKit != "Unknown" && motherKit != "Unknown")
                query = ("select c.[rsid], c.[chromosome], c.[position], c.[genotype]\"Child\", coalesce(f.[genotype],'--')\"Father\", coalesce(m.[genotype],'--')\"Mother\", ''\"Phased Paternal\",''\"Phased Maternal\"  from kit_autosomal c left outer join kit_autosomal f,kit_autosomal m on f.rsid=c.rsid and m.rsid=c.rsid where c.kit_no='" + childKit + "' and f.kit_no='" + fatherKit + "' and m.[kit_no]='" + motherKit + "' order by cast(c.chromosome as integer),c.position");
            else if (fatherKit != "Unknown" && motherKit == "Unknown")
                query = ("select c.[rsid], c.[chromosome], c.[position], c.[genotype]\"Child\", coalesce(f.[genotype],'--')\"Father\", '--'\"Mother\", ''\"Phased Paternal\", ''\"Phased Maternal\"  from kit_autosomal c left outer join kit_autosomal f on f.rsid=c.rsid  where c.kit_no='" + childKit + "' and f.kit_no='" + fatherKit + "' order by cast(c.chromosome as integer), c.position");
            else if (fatherKit == "Unknown" && motherKit != "Unknown")
                query = ("select c.[rsid], c.[chromosome], c.[position], c.[genotype]\"Child\", '--'\"Father\", coalesce(m.[genotype],'--')\"Mother\", ''\"Phased Paternal\", ''\"Phased Maternal\"  from kit_autosomal c left outer join kit_autosomal m on m.rsid=c.rsid where c.kit_no='" + childKit + "' and m.kit_no='" + motherKit + "' order by cast(c.chromosome as integer), c.position");

            return GetRows<PhaseRow>(query);
        }

        public static void SavePhasedKit(string fatherKit, string motherKit, string childKit, IList<PhaseRow> dt)
        {
            CheckConnection();

            ExecCmd($"delete from kit_phased where kit_no = '{childKit}'", _connection);

            if (fatherKit == "Unknown")
                fatherKit = "";

            if (motherKit == "Unknown")
                motherKit = "";

            using (var trans = _connection.BeginTransaction()) {
                foreach (var row in dt) {
                    string phasedPaternal = ("" + row.PhasedPaternal).Replace("\0", "").Trim();
                    if (string.IsNullOrEmpty(phasedPaternal)) phasedPaternal = string.Empty;

                    string phasedMaternal = ("" + row.PhasedMaternal).Replace("\0", "").Trim();
                    if (string.IsNullOrEmpty(phasedMaternal)) phasedMaternal = string.Empty;

                    ExecCmd(
                        "insert or replace into kit_phased (kit_no, rsid, chromosome, position, paternal_genotype, maternal_genotype, paternal_kit_no, maternal_kit_no) " +
                        $"values ('{childKit}', '{row.rsID}', '{row.Chromosome}', {row.Position}, '{phasedPaternal}', '{phasedMaternal}', '{fatherKit}', '{motherKit}')", _connection, trans);
                }
                trans.Commit();
            }
        }

        public static void DeletePhasedKit(string kit)
        {
            CheckConnection();

            ExecCmd($"delete from cmp_phased where phased_kit = '{kit}'", _connection);
        }

        public static IList<string> GetPhasedKits()
        {
            CheckConnection();

            using (SQLiteCommand ss = new SQLiteCommand("select distinct kit_no from kit_phased", _connection))
            using (var reader = ss.ExecuteReader()) {
                var result = new List<string>();
                while (reader.Read()) {
                    var row = reader.GetString(0);
                    result.Add(row);
                }
                return result;
            }
        }

        #endregion

        #region MtDna

        public static bool ExistsMtDna(string kit)
        {
            string val = QueryValue($"select kit_no from kit_mtdna where kit_no = '{kit}'");
            return (val == kit);
        }

        public static void SaveKitMtDNA(string kit_no, string mutations, string fasta)
        {
            CheckConnection();

            ExecCmd($"insert or replace into kit_mtdna (kit_no, mutations, fasta) values ('{kit_no}', '{mutations}', '{fasta}')", _connection);
        }

        public static void GetMtDNA(string kit, out string mutations, out string fasta)
        {
            CheckConnection();

            using (var query = new SQLiteCommand($"select mutations, fasta from kit_mtdna where kit_no = '{kit}'", _connection))
            using (var reader = query.ExecuteReader()) {
                if (reader.Read()) {
                    mutations = reader.GetString(0);
                    fasta = reader.GetString(1);
                    return;
                }
            }

            mutations = string.Empty;
            fasta = string.Empty;
        }

        #endregion

        #region YDna

        public static bool ExistsYDna(string kit)
        {
            string val = QueryValue($"select kit_no from kit_ysnps where kit_no = '{kit}'");
            return (val == kit);
        }

        public static IList<YSTR> GetYSTR(string kit)
        {
            return GetRows<YSTR>($"select marker, value from kit_ystr where kit_no = '{kit}'");
        }

        public static void SaveKitYSNPs(string kit_no, string ysnps_list)
        {
            CheckConnection();

            ExecCmd($"insert or replace into kit_ysnps (kit_no, ysnps) values ('{kit_no}', '{ysnps_list}')", _connection);
        }

        public static string GetYSNPs(string kit)
        {
            return QueryValue($"select ysnps from kit_ysnps where kit_no = '{kit}'");
        }

        public static void SaveYSTR(string kit_no, List<YSTR> yRows)
        {
            CheckConnection();

            using (var trans = _connection.BeginTransaction()) {
                ExecCmd($"delete from kit_ystr where kit_no = '{kit_no}'", _connection, trans);

                foreach (var row in yRows) {
                    if (row.Repeats.Trim() == "") continue;

                    ExecCmd($"insert or replace into kit_ystr (kit_no, marker, value) values ('{kit_no}', '{row.Marker}', '{row.Repeats}')", _connection, trans);
                }

                trans.Commit();
            }
        }

        #endregion

        #region One2Many (MatchingKits)

        public static IList<MatchingKit> GetMatchingKits(string kit)
        {
            return GetRows<MatchingKit>(
                "select cmp_id, kit, name, at_longest, at_total, x_longest, x_total, mrca from (" +
                "select a.cmp_id,a.kit1'kit',b.name,a.at_longest,a.at_total,a.x_longest,a.x_total,a.mrca " +
                $"from cmp_status a,kit_master b where a.at_total!=0 and a.kit1!='{kit}' and a.kit2='{kit}' and a.status_autosomal=1 and b.kit_no=a.kit1 and b.disabled=0 " +
                "union select a.cmp_id,a.kit2'kit',b.name,a.at_longest,a.at_total,a.x_longest,a.x_total,a.mrca " +
                $"from cmp_status a,kit_master b where a.at_total!=0 and a.kit2!='{kit}' and a.kit1='{kit}' and a.status_autosomal=1 and b.kit_no=a.kit2 and b.disabled=0 " +
                ") order by at_longest desc, at_total desc");
        }

        #endregion
    }
}

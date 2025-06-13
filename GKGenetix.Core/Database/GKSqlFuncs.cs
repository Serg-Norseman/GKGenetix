/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System.Collections.Generic;
using System.IO;
using System.Linq;
using GKGenetix.Core.Model;
using SQLite;

namespace GKGenetix.Core.Database
{
    public static class GKSqlFuncs
    {
        private sealed class DBPatch
        {
            public int ReqVer;
            public string Sql;

            public DBPatch(int reqVer, string sql)
            {
                ReqVer = reqVer;
                Sql = sql;
            }
        }

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
        };

        private static readonly DBPatch[] Patches = new DBPatch[] {
            //new DBPatch(102, ""),
            new DBPatch(102, "update [kit_autosomal] set [chromosome] = '23' where [chromosome] = 'X'"),
            new DBPatch(102, "update [cmp_autosomal] set [chromosome] = '23' where [chromosome] = 'X'"),
            new DBPatch(102, "update [cmp_mrca] set [chromosome] = '23' where [chromosome] = 'X'"),
            new DBPatch(102, "update [kit_roh] set [chromosome] = '23' where [chromosome] = 'X'"),
            new DBPatch(102, "update [kit_phased] set [chromosome] = '23' where [chromosome] = 'X'"),
            new DBPatch(103, "drop table if exists cmp_phased"),
        };

        private const int DB_VER = 104;

        #endregion

        static GKSqlFuncs()
        {
#if !NETSTANDARD && !NET461 && !MONO
            SQLiteLoader.Load();
#endif
        }

        private static SQLiteConnection _connection;
        private static IKitHost _host;
        private static string _appDataPath = string.Empty;

        public static void SetHost(IKitHost host)
        {
            _host = host;
        }

        public static void SetAppDataPath(string path)
        {
            _appDataPath = path;
        }

        #region Common database

        private static void ResetTables(string dbPath)
        {
            using (var conn = new SQLiteConnection(dbPath)) {
                conn.BeginTransaction();
                for (int idx = 0; idx < CreateTableSQL.Length; idx += 2) {
                    conn.Execute(CreateTableSQL[idx + 1]);
                }
                conn.Commit();
            }
        }

        private static void CheckConnection()
        {
            if (_connection != null) return;

            string dbPath = Path.Combine(_appDataPath, SQLITE_DB);

            if (File.Exists(dbPath)) {
                //string connStr = @"Data Source=" + SQLITE_DB + @";Version=3; Compress=True; PRAGMA foreign_keys = ON; PRAGMA auto_vacuum = FULL;";

                _connection = new SQLiteConnection(dbPath);

                _connection.ExecuteScalar<string>("PRAGMA journal_mode = WAL;");
                _connection.Execute("PRAGMA foreign_keys = ON;");
                _connection.Execute("PRAGMA auto_vacuum = FULL;");

                var dbVerStr = GetSettingValue("DB:Version");
                int dbVer = string.IsNullOrEmpty(dbVerStr) ? 102 : int.Parse(dbVerStr);

                if (dbVer < DB_VER) {
                    foreach (var patch in Patches) {
                        if (patch.ReqVer <= dbVer)
                            _connection.Execute(patch.Sql);
                    }

                    SetSettingValue("DB:Version", DB_VER.ToString());
                }
            } else {
                if (_host.ShowQuestion("Data file ggk.db doesn't exist. If this is the first time you are opening the software, you can ignore this error. Do you wish to create one?")) {
                    ResetTables(dbPath);
                    CheckConnection();
                } else _host.Exit();
            }
        }

        public static void CheckIntegrity()
        {
            CheckConnection();

            var rows = _connection.Query<QString>("select tbl_name'Value' from sqlite_master where type='table'");
            var list = rows.Select(x => x.Value).ToList();

            for (int idx = 0; idx < CreateTableSQL.Length; idx += 2) {
                if (!list.Contains(CreateTableSQL[idx])) {
                    _connection.Execute(CreateTableSQL[idx + 1]);
                }
            }
        }

        public static string GetSettingValue(string key)
        {
            CheckConnection();

            var rows = _connection.Query<QString>($"select value'Value' from ggk_settings where key = '{key}'");
            return (rows != null && rows.Count > 0) ? rows[0].Value : null;
        }

        public static void SetSettingValue(string key, string value)
        {
            CheckConnection();

            _connection.Execute($"insert or replace into ggk_settings (key, value, description) values ('{key}', '{value}', '')");
        }

        private class QString
        {
            public string Value { get; set; }
        }

        private static string QueryValue(string sql)
        {
            CheckConnection();

            var rows = _connection.Query<QString>(sql);
            return (rows != null && rows.Count > 0) ? rows[0].Value : null;
        }

        private static IList<T> GetRows<T>(string query) where T : IDataRecord, new()
        {
            CheckConnection();

            return _connection.Query<T>(query);
        }

        #endregion

        #region Kits

        public static string GetKitName(string kit)
        {
            string kitName = QueryValue($"select name'Value' from kit_master where kit_no='{kit}'");
            return !string.IsNullOrEmpty(kitName) ? kitName : "Unknown";
        }

        public static void DeleteKit(string kit)
        {
            CheckConnection();

            _connection.BeginTransaction();
            _connection.Execute($"delete from kit_master where kit_no = '{kit}'");
            _connection.Commit();
        }

        public static IList<TestRecord> QueryKits(bool excludeDisabled = false, bool excludeRefs = true, char requestedSex = 'U')
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

            string sql = $"select kit_no'KitNo', name'Name', sex'Sex', disabled'Disabled', [x]'Lng', [y]'Lat', last_modified'LastModified', reference'Reference', roh_status'RoH_Status' from kit_master {where} order by last_modified desc";
            return GetRows<TestRecord>(sql);
        }

        public static void GetKit(string kitNo, out string name, out string sex)
        {
            CheckConnection();

            var rows = _connection.Query<TestRecord>($"select name'Name', sex'Sex' from kit_master where kit_no = '{kitNo}'");
            if (rows != null && rows.Count > 0) {
                name = rows[0].Name;
                sex = rows[0].Sex;
                return;
            }

            name = string.Empty;
            sex = string.Empty;
        }

        public static void UpdateKit(string kit_no, string name, string sex)
        {
            CheckConnection();

            _connection.Execute($"update kit_master set name = '{name}', sex = '{sex[0]}' where kit_no = '{kit_no}'");
        }

        public static void InsertKit(string kit_no, string name, string sex)
        {
            CheckConnection();

            _connection.Execute($"insert or replace into kit_master (kit_no, name, sex) values ('{kit_no}', '{name}', '{sex[0]}')");
        }

        public static void SaveKit(TestRecord testRec)
        {
            CheckConnection();

            var sex = testRec.Sex[0].ToString();
            string dis = (testRec.Disabled) ? "1" : "0";
            _connection.Execute($"update kit_master set name = '{testRec.Name}', sex = '{sex}', disabled = {dis}, x = {(int)testRec.Lng}, y = {(int)testRec.Lat} where kit_no = '{testRec.KitNo}'");
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

            _connection.Execute(upCmd);
        }

        public static bool CheckAlreadyCompared(string kit1, string kit2)
        {
            var val = QueryValue($"select status_autosomal'Value' from cmp_status where (kit1 = '{kit1}' and kit2 = '{kit2}') or (kit1 = '{kit2}' and kit2 = '{kit1}')");
            return !string.IsNullOrEmpty(val);
        }

        public static IList<SNPMatch> GetCmpSeg(int segmentId)
        {
            return GetRows<SNPMatch>(
                $"select rsid'rsID', chromosome'ChrStr', position'Position', kit1_genotype'Gt1Str', kit2_genotype'Gt2Str', match'Match' " +
                $"from cmp_mrca where segment_id = '{segmentId}' order by position");
        }

        #endregion

        #region Autosomal

        public static void DeleteAutosomal(string kit_no)
        {
            CheckConnection();

            _connection.Execute($"delete from kit_autosomal where kit_no = '{kit_no}'");
        }

        public static void SaveAutosomalCmp(string kit1, string kit2, IList<CmpSegment> segment_idx, bool reference)
        {
            CheckConnection();

            _connection.Execute($"delete from cmp_status where (kit1 = '{kit1}' and kit2 = '{kit2}') or (kit1 = '{kit2}' and kit2 = '{kit1}')");

            var stats = SegmentStats.CalculateSegmentStats(segment_idx);

            _connection.Execute(
                @"insert or replace into cmp_status (kit1, kit2, status_autosomal, at_longest, at_total, x_longest, x_total, mrca) " +
                $"values ('{kit1}', '{kit2}', 1, {stats.Longest}, {stats.Total}, {stats.XLongest}, {stats.XTotal}, {stats.Mrca})");

            string cmp_id = QueryValue($"select cmp_id'Value' from cmp_status where kit1 = '{kit1}' and kit2 = '{kit2}'");

            for (int i = 0; i < segment_idx.Count; i++) {
                var obj = segment_idx[i];

                var chromosome = obj.Chromosome;
                string start_position = obj.StartPosition.ToString();
                string end_position = obj.EndPosition.ToString();
                string segment_length_cm = obj.SegmentLength_cm.ToString();
                string snp_count = obj.SNPCount.ToString();

                _connection.Execute(
                    @"insert or replace into cmp_autosomal (cmp_id, kit1, kit2, chromosome, start_position, end_position, segment_length_cm, snp_count) " +
                    $"values ({cmp_id}, '{kit1}', '{kit2}', '{chromosome}', {start_position}, {end_position}, {segment_length_cm}, {snp_count})");

                if (!reference) {
                    var segment_id = QueryValue(
                        @"select segment_id'Value' from cmp_autosomal " +
                        $"where kit1 = '{kit1}' and kit2 = '{kit2}' and chromosome = '{chromosome}' and start_position = '{start_position}' and end_position = '{end_position}'");

                    _connection.BeginTransaction();
                    foreach (var row in obj.Rows) {
                        _connection.Execute(
                            "insert or replace into cmp_mrca (rsid, chromosome, position, kit1_genotype, kit2_genotype, match, segment_id) " +
                            $"values ('{row.rsID}', '{row.Chromosome}', {row.Position}, '{row.Genotype1}', '{row.Genotype2}', '{row.Match}', '{segment_id}')");
                    }
                    _connection.Commit();
                }
            }
        }

        public static IList<CmpSegment> GetAutosomalCmp(string kit1, string kit2)
        {
            return GetRows<CmpSegment>(
                $"select segment_id'SegmentId', chromosome'ChrStr', start_position'StartPosition', end_position'EndPosition', segment_length_cm'SegmentLength_cm', snp_count'SNPCount' from cmp_autosomal " +
                $"where (kit1 = '{kit1}' and kit2 = '{kit2}') or (kit1 = '{kit2}' and kit2 = '{kit1}') order by cast(chromosome as integer), start_position");
        }

        public static IList<CmpSegment> GetAutosomalCmp(int cmp_id)
        {
            return GetRows<CmpSegment>(
                $"select segment_id'SegmentId', chromosome'ChrStr', start_position'StartPosition', end_position'EndPosition', segment_length_cm'SegmentLength_cm', snp_count'SNPCount' from cmp_autosomal " +
                $"where cmp_id = '{cmp_id}' order by cast(chromosome as integer), start_position");
        }

        public static IList<SNP> GetAutosomal(string kit)
        {
            return GetRows<SNP>($"select rsid'rsID', chromosome'ChrStr', position'Position', genotype'GtStr' from kit_autosomal where kit_no = '{kit}' order by cast(chromosome as integer), position");
        }

        public static IList<SNP> GetROHSeg(string kit, byte chromosome, int startPos, int endPos)
        {
            return GetRows<SNP>(
                "select rsid'rsID', chromosome'ChrStr', position'Position', genotype'GtStr' from kit_autosomal " +
                $"where kit_no = '{kit}' and chromosome = '{chromosome}' and position >= {startPos} and position <= {endPos} order by position");
        }

        public static void SaveAutosomal(string kit_no, List<SNP> rows)
        {
            DeleteAutosomal(kit_no);

            CheckConnection();

            _connection.BeginTransaction();
            foreach (var row in rows) {
                var rsid = row.rsID;
                var chr = row.Chromosome.ToString();
                var pos = row.Position.ToString();
                var gt = row.Genotype.ToString();

                if (string.IsNullOrEmpty(rsid) || string.IsNullOrEmpty(chr) || string.IsNullOrEmpty(pos) || string.IsNullOrEmpty(gt))
                    continue;

                _connection.Execute($"insert or replace into kit_autosomal(kit_no, rsid, chromosome, position, genotype) values ('{kit_no}', '{rsid}', '{chr}', {pos}, '{gt}')");
            }
            _connection.Commit();
        }

        #endregion

        #region SNP


        #endregion

        #region RoH

        public static bool ExistsROH(string kit)
        {
            string roh = QueryValue($"select roh_status'Value' from kit_master where kit_no = '{kit}'");
            return (roh != "0");
        }

        public static void SaveROHCmp(string kit, IList<ROHSegment> segment_idx)
        {
            CheckConnection();

            _connection.Execute($"delete from kit_roh where kit_no = '{kit}'");

            _connection.BeginTransaction();
            for (int i = 0; i < segment_idx.Count; i++) {
                var obj = segment_idx[i];

                _connection.Execute(
                    "insert or replace into kit_roh (kit_no, chromosome, start_position, end_position, segment_length_cm, snp_count) " +
                    $"values ('{kit}', '{obj.Chromosome}', {obj.StartPosition}, {obj.EndPosition}, {obj.SegmentLength_cm}, {obj.SNPCount})");
            }
            _connection.Commit();

            _connection.Execute($"update kit_master set roh_status = 1 where kit_no = '{kit}'");
        }

        public static IList<ROHSegment> GetROHCmp(string kit)
        {
            return GetRows<ROHSegment>($"select chromosome'ChrStr', start_position'StartPosition', end_position'EndPosition', segment_length_cm'SegmentLength_cm', snp_count'SNPCount' from kit_roh where kit_no = '{kit}'");
        }

        #endregion

        #region Phasing

        public static bool IsPhased(string kit)
        {
            var val = QueryValue($"select kit_no'Value' from kit_phased where kit_no = '{kit}'");
            return !string.IsNullOrEmpty(val);
        }

        /*public static IList<UnphasedSegment> GetUnphasedSegments(string phasedKit)
        {
            return GetRows<UnphasedSegment>(
                "select unphased_kit'UnphasedKit', chromosome'ChrStr', start_position'StartPosition', end_position'EndPosition' from (" +
                $"select kit1'unphased_kit', chromosome, start_position, end_position from cmp_autosomal where kit2='{phasedKit}' " +
                $"union select kit2'unphased_kit', chromosome, start_position, end_position from cmp_autosomal where kit1='{phasedKit}'" +
                ") order by cast(chromosome as integer), start_position");
        }*/

        public static IList<PhaseSegment> GetPhaseSegments(string phasedKit, string unphasedKit, byte chromosome, int startPosition, int endPosition)
        {
            return GetRows<PhaseSegment>(
                $"select a.position'Position', a.genotype'Genotype', p.paternal_genotype'PaternalGenotype', p.maternal_genotype'MaternalGenotype' " +
                $"from kit_autosomal a, kit_phased p where a.kit_no = '{unphasedKit}' " +
                $"and a.position > {startPosition} and a.position < {endPosition} and a.chromosome = '{chromosome}' " +
                $"and p.rsid = a.rsid and p.kit_no = '{phasedKit}' order by a.position");
        }

        public static IList<PhaseRow> GetPhaseRows(string fatherKit, string motherKit, string childKit)
        {
            string query = "";

            // PhaseRow: rsID, ChrStr, Position, Child, Father, Mother
            if (fatherKit != "Unknown" && motherKit != "Unknown")
                query = ("select c.[rsid]'rsID', c.[chromosome]'ChrStr', c.[position]'Position', c.[genotype]'Child', coalesce(f.[genotype],'--')'Father', coalesce(m.[genotype],'--')'Mother' from kit_autosomal c left outer join kit_autosomal f,kit_autosomal m on f.rsid=c.rsid and m.rsid=c.rsid where c.kit_no='" + childKit + "' and f.kit_no='" + fatherKit + "' and m.[kit_no]='" + motherKit + "' order by cast(c.chromosome as integer),c.position");
            else if (fatherKit != "Unknown" && motherKit == "Unknown")
                query = ("select c.[rsid]'rsID', c.[chromosome]'ChrStr', c.[position]'Position', c.[genotype]'Child', coalesce(f.[genotype],'--')'Father', '--''Mother' from kit_autosomal c left outer join kit_autosomal f on f.rsid=c.rsid  where c.kit_no='" + childKit + "' and f.kit_no='" + fatherKit + "' order by cast(c.chromosome as integer), c.position");
            else if (fatherKit == "Unknown" && motherKit != "Unknown")
                query = ("select c.[rsid]'rsID', c.[chromosome]'ChrStr', c.[position]'Position', c.[genotype]'Child', '--''Father', coalesce(m.[genotype],'--')'Mother' from kit_autosomal c left outer join kit_autosomal m on m.rsid=c.rsid where c.kit_no='" + childKit + "' and m.kit_no='" + motherKit + "' order by cast(c.chromosome as integer), c.position");

            return GetRows<PhaseRow>(query);
        }

        public static void SavePhasedKit(string fatherKit, string motherKit, string childKit, IList<PhaseRow> dt)
        {
            CheckConnection();

            _connection.Execute($"delete from kit_phased where kit_no = '{childKit}'");

            if (fatherKit == "Unknown") fatherKit = "";
            if (motherKit == "Unknown") motherKit = "";

            _connection.BeginTransaction();
            foreach (var row in dt) {
                string phasedPaternal = ("" + row.PhasedPaternal).Replace("\0", "").Trim();
                if (string.IsNullOrEmpty(phasedPaternal)) phasedPaternal = string.Empty;

                string phasedMaternal = ("" + row.PhasedMaternal).Replace("\0", "").Trim();
                if (string.IsNullOrEmpty(phasedMaternal)) phasedMaternal = string.Empty;

                _connection.Execute(
                    "insert or replace into kit_phased (kit_no, rsid, chromosome, position, paternal_genotype, maternal_genotype, paternal_kit_no, maternal_kit_no) " +
                    $"values ('{childKit}', '{row.rsID}', '{row.Chromosome}', {row.Position}, '{phasedPaternal}', '{phasedMaternal}', '{fatherKit}', '{motherKit}')");
            }
            _connection.Commit();
        }

        /*public static IList<string> GetPhasedKits()
        {
            CheckConnection();

            var rows = _connection.Query<QString>("select distinct kit_no'Value' from kit_phased");
            return rows.Select(x => x.Value).ToList();
        }*/

        #endregion

        #region Mt-DNA

        public static bool ExistsMtDNA(string kit)
        {
            string val = QueryValue($"select kit_no'Value' from kit_mtdna where kit_no = '{kit}'");
            return (val == kit);
        }

        public static void GetMtDNA(string kit, out string mutations, out string fasta)
        {
            CheckConnection();

            var rows = _connection.Query<MtDNARecord>($"select mutations'Mutations', fasta'Fasta' from kit_mtdna where kit_no = '{kit}'");
            if (rows != null && rows.Count > 0) {
                mutations = rows[0].Mutations;
                fasta = rows[0].Fasta;
                return;
            }

            mutations = string.Empty;
            fasta = string.Empty;
        }

        public static void SaveMtDNA(string kit_no, string mutations, string fasta)
        {
            CheckConnection();

            _connection.Execute($"insert or replace into kit_mtdna (kit_no, mutations, fasta) values ('{kit_no}', '{mutations}', '{fasta}')");
        }

        #endregion

        #region Y-DNA

        public static bool ExistsYSNPs(string kit)
        {
            string val = QueryValue($"select kit_no'Value' from kit_ysnps where kit_no = '{kit}'");
            return (val == kit);
        }

        public static string GetYSNPs(string kit)
        {
            return QueryValue($"select ysnps'Value' from kit_ysnps where kit_no = '{kit}'");
        }

        public static void SaveYSNPs(string kit_no, string ysnps_list)
        {
            CheckConnection();

            _connection.Execute($"insert or replace into kit_ysnps (kit_no, ysnps) values ('{kit_no}', '{ysnps_list}')");
        }

        public static IList<YSTR> GetYSTR(string kit)
        {
            return GetRows<YSTR>($"select marker'Marker', value'Repeats' from kit_ystr where kit_no = '{kit}'");
        }

        public static void SaveYSTR(string kit_no, List<YSTR> yRows)
        {
            CheckConnection();

            _connection.BeginTransaction();

            _connection.Execute($"delete from kit_ystr where kit_no = '{kit_no}'");

            foreach (var row in yRows) {
                if (row.Repeats.Trim() == "") continue;

                _connection.Execute($"insert or replace into kit_ystr (kit_no, marker, value) values ('{kit_no}', '{row.Marker}', '{row.Repeats}')");
            }

            _connection.Commit();
        }

        #endregion

        #region Synthetic queries

        public static IList<AdmixtureRecord> GetAdmixture(string kit, string limit = "< 3")
        {
            //  "select name, at_total, at_longest, x, y FROM (select b.name,a.at_total,a.at_longest,b.x,b.y from cmp_status a,kit_master b where a.kit1='" + kit + "' and a.kit2=b.kit_no and a.kit2 like 'HGDP%' and a.status_autosomal=1 and a.at_longest<3 and a.at_total!=0 UNION select b.name,a.at_total,a.at_longest,b.x,b.y from cmp_status a,kit_master b where a.kit2='" + kit + "' and a.kit1=b.kit_no and a.kit1 like 'HGDP%' and a.status_autosomal=1 and a.at_longest<3 and a.at_total!=0) ORDER BY at_total DESC";

            return GetRows<AdmixtureRecord>(
                "select [name]'Name', [at_total]'AtTotal', [at_longest]'AtLongest', [x]'Lng', [y]'Lat' from (" +
                $"select b.name,a.at_total,a.at_longest,b.x,b.y from cmp_status a,kit_master b where a.kit1='{kit}' and a.kit2=b.kit_no and a.status_autosomal=1 and a.at_longest {limit} and a.at_total != 0 " +
                $"union select b.name,a.at_total,a.at_longest,b.x,b.y from cmp_status a,kit_master b where a.kit2='{kit}' and a.kit1=b.kit_no and a.status_autosomal=1 and a.at_longest {limit} and a.at_total != 0" +
                ") order by at_total desc");
        }

        /// <summary>
        /// Target: One2One (One2One and ProcessKits).
        /// </summary>
        public static IList<SNPMatch> GetOTORows(string kit1, string kit2)
        {
            return GetRows<SNPMatch>(
                @"select rsid'rsID', chr'ChrStr', pos'Position', gt1'Gt1Str', gt2'Gt2Str', count(*)'Match' from (" +
                "select kit1.rsid 'rsid', kit1.chromosome 'chr', kit1.position 'pos', kit1.genotype 'gt1', kit2.genotype 'gt2' from kit_autosomal kit1 " +
                $"left join kit_autosomal kit2 on kit1.rsid=kit2.rsid where kit1.kit_no = '{kit1}' and kit2.kit_no = '{kit2}' " +
                "union select kit1.rsid 'rsid', kit1.chromosome 'chr', kit1.position 'pos', kit1.genotype 'gt1', kit2.genotype 'gt2' from kit_autosomal kit1 " +
                $"left join kit_autosomal kit2 on kit1.rsid=kit2.rsid where kit1.kit_no = '{kit2}' and kit2.kit_no = '{kit1}'" +
                ") group by rsid order by cast(chr as integer), pos");
        }

        /// <summary>
        /// Target: One2Many (MatchingKits).
        /// </summary>
        public static IList<MatchingKit> GetMatchingKits(string kit)
        {
            return GetRows<MatchingKit>(
                "select cmp_id'CmpId', kit'Kit', name'Name', at_longest'Longest', at_total'Total', x_longest'XLongest', x_total'XTotal', mrca'Mrca' from (" +
                "select a.cmp_id,a.kit1'kit',b.name,a.at_longest,a.at_total,a.x_longest,a.x_total,a.mrca " +
                $"from cmp_status a,kit_master b where a.at_total!=0 and a.kit1!='{kit}' and a.kit2='{kit}' and a.status_autosomal=1 and b.kit_no=a.kit1 and b.disabled=0 " +
                "union select a.cmp_id,a.kit2'kit',b.name,a.at_longest,a.at_total,a.x_longest,a.x_total,a.mrca " +
                $"from cmp_status a,kit_master b where a.at_total!=0 and a.kit2!='{kit}' and a.kit1='{kit}' and a.status_autosomal=1 and b.kit_no=a.kit2 and b.disabled=0 " +
                ") order by at_longest desc, at_total desc");
        }

        #endregion
    }
}

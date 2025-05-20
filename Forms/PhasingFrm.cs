using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Genetic_Genealogy_Kit
{
    public partial class PhasingFrm : Form
    {
        string father_kit = "Unknown";
        string mother_kit = "Unknown";
        string child_kit = "Unknown";
        DataTable dt = null;
        bool male = true;
        List<int> mutatedRow = new List<int>();
        
        List<int> ambiguousRow = new List<int>();

        public PhasingFrm()
        {
            InitializeComponent();
        }

        private void bwPhasing_DoWork(object sender, DoWorkEventArgs e)
        {
            if (father_kit != "Unknown" && mother_kit != "Unknown")
                dt = GGKUtilLib.QueryDB("SELECT c.[rsid]'RSID',c.[chromosome]'Chromosome',c.[position]'Position',c.[genotype]\"Child\",COALESCE(f.[genotype],'--')\"Father\",COALESCE(m.[genotype],'--')\"Mother\",''\"Phased Paternal\",''\"Phased Maternal\"  FROM kit_autosomal c left outer join kit_autosomal f,kit_autosomal m on f.rsid=c.rsid AND m.rsid=c.rsid WHERE c.kit_no='" + child_kit + "' AND f.kit_no='" + father_kit + "' AND m.[kit_no]='" + mother_kit + "' ORDER BY cast(c.chromosome as integer),c.position");
            else if (father_kit != "Unknown" && mother_kit == "Unknown")
                dt = GGKUtilLib.QueryDB("SELECT c.[rsid]'RSID',c.[chromosome]'Chromosome',c.[position]'Position',c.[genotype]\"Child\",COALESCE(f.[genotype],'--')\"Father\",'--'\"Mother\",''\"Phased Paternal\",''\"Phased Maternal\"  FROM kit_autosomal c left outer join kit_autosomal f on f.rsid=c.rsid  WHERE c.kit_no='" + child_kit + "' AND f.kit_no='" + father_kit + "' ORDER BY cast(c.chromosome as integer),c.position");
            else if (father_kit == "Unknown" && mother_kit != "Unknown")
                dt = GGKUtilLib.QueryDB("SELECT c.[rsid]'RSID',c.[chromosome]'Chromosome',c.[position]'Position',c.[genotype]\"Child\",'--'\"Father\",COALESCE(m.[genotype],'--')\"Mother\" ,''\"Phased Paternal\",''\"Phased Maternal\"  FROM kit_autosomal c left outer join kit_autosomal m on m.rsid=c.rsid WHERE c.kit_no='" + child_kit + "' AND m.kit_no='" + mother_kit + "' ORDER BY cast(c.chromosome as integer),c.position");

            // after phasing...
            string child = null;
            string father = null;
            string mother = null;
            string phased_paternal = null;
            string phased_maternal = null;
            object[] o=null;
            string nc = "N";
            bool amb = false;
            foreach(DataRow row in dt.Rows)
            {
                
                o=row.ItemArray;
                phased_paternal = "";
                phased_maternal = "";
                child = o[3].ToString();
                father = o[4].ToString();
                mother = o[5].ToString();

                if(child.Length==1)
                    child=child+child;

                //check
                if ((father.Replace(child[0].ToString(), "").Replace(child[1].ToString(), "") == father || mother.Replace(child[0].ToString(), "").Replace(child[1].ToString(), "") == mother) && o[1].ToString() != "X" && father!="--" && mother!="--" && child!="--")
                {
                    mutatedRow.Add(dt.Rows.IndexOf(row));
                }

                amb = false;
                if (father == child && child[0] != child[1] && mother=="--")
                    amb = true;                                    
                else if (mother == child && child[0] != child[1] && father == "--")
                    amb = true;
                else if (father == child && child[0] != child[1] && mother == child)
                    amb = true;

                if(amb)
                {
                    ambiguousRow.Add(dt.Rows.IndexOf(row));
                    nc = getNucleotideCode(child[0].ToString(), child[1].ToString());
                    row.SetField(6, nc);
                    row.SetField(7, nc);
                    phased_paternal = nc;
                    phased_maternal = nc;
                    continue;
                }


                if (child == "--" || child == "??")
                {
                    if (father[0] == father[1] && father == mother && o[1].ToString() != "X")
                    {
                        row.SetField(6, father[0].ToString());
                        row.SetField(7, father[0].ToString());
                        phased_paternal = father[0].ToString();
                        phased_maternal = father[0].ToString();
                        continue;
                    }
                }

                if (male && o[1].ToString() == "X")
                {
                    child = child[0].ToString();
                    if(child=="-" && mother!="--")
                    {
                        row.SetField(6, "");
                        row.SetField(7, mother[0].ToString());
                        phased_paternal = "";
                        phased_maternal = mother[0].ToString();
                        continue;
                    }
                }
                else
                {
                    if (child[0] == child[1] && child[0] != '-' && child[0] != '?')
                    {
                        row.SetField(6, child[0].ToString());
                        row.SetField(7, child[0].ToString());
                        phased_paternal = child[0].ToString();
                        phased_maternal = child[0].ToString();
                        continue;
                    }
                }

                if (o[1].ToString() != "X" )
                {
                    autosomalSingleSNPPhase(child, father, mother, row, ref phased_paternal, ref phased_maternal);
                }
                else if (o[1].ToString() == "X")
                {
                    if(male)
                    {
                        row.SetField(3, child[0].ToString());
                        row.SetField(4, "");
                        row.SetField(6, "");
                        row.SetField(7, child[0].ToString());
                        phased_paternal = "";
                        phased_maternal = child[0].ToString();
                    }
                    else
                    {
                        autosomalSingleSNPPhase(child, father, mother, row, ref phased_paternal, ref phased_maternal);
                    }
                }

                if (phased_paternal == "" && phased_maternal!="")
                {
                    phased_paternal = child.Replace(phased_maternal, "");
                    if (phased_paternal.Length > 0)
                        phased_paternal = phased_paternal[0].ToString();
                    row.SetField(6, phased_paternal);
                    
                }
                if (phased_maternal == "" && phased_paternal != "")
                {
                    phased_maternal = child.Replace(phased_paternal, "");
                    if (phased_maternal.Length > 0)
                        phased_maternal = phased_maternal[0].ToString();
                    row.SetField(7, phased_maternal);
                }
            }

            // save to kit_phased 
            string rsid = null;
            string chromosome = null;
            string position = null;
            bwPhasing.ReportProgress(-1,"Saving Phased Kit "+child_kit+" ...");
            SQLiteConnection conn = GGKUtilLib.getDBConnection();

            SQLiteCommand cmd = new SQLiteCommand("DELETE FROM kit_phased where kit_no=@kit_no", conn);
            cmd.Parameters.AddWithValue("@kit_no", child_kit);
            cmd.ExecuteNonQuery();

            using (SQLiteTransaction trans = conn.BeginTransaction())
            {                
                foreach (DataRow row in dt.Rows)
                {
                    o = row.ItemArray;
                    rsid = o[0].ToString();
                    chromosome = o[1].ToString();
                    position = o[2].ToString();

                    phased_paternal = o[6].ToString();
                    phased_maternal = o[7].ToString();

                    cmd = new SQLiteCommand("INSERT OR REPLACE INTO kit_phased(kit_no,rsid,chromosome,position,paternal_genotype,maternal_genotype,paternal_kit_no,maternal_kit_no) VALUES(@kit_no,@rsid,@chromosome,@position,@paternal_genotype,@maternal_genotype,@paternal_kit_no,@maternal_kit_no)", conn);
                    cmd.Parameters.AddWithValue("@kit_no", child_kit);
                    cmd.Parameters.AddWithValue("@rsid", rsid);
                    cmd.Parameters.AddWithValue("@chromosome", chromosome);
                    cmd.Parameters.AddWithValue("@position", position);
                    cmd.Parameters.AddWithValue("@paternal_genotype", phased_paternal);
                    cmd.Parameters.AddWithValue("@maternal_genotype", phased_maternal);
                    if (father_kit == "Unknown")
                        cmd.Parameters.AddWithValue("@paternal_kit_no", ""); 
                    else
                        cmd.Parameters.AddWithValue("@paternal_kit_no", father_kit);
                    if (mother_kit=="Unknown")
                        cmd.Parameters.AddWithValue("@maternal_kit_no", "");
                    else
                        cmd.Parameters.AddWithValue("@maternal_kit_no", mother_kit);
                    cmd.ExecuteNonQuery();

                }
                trans.Commit();
            }
            
        }

        private string getNucleotideCode(string bp1, string bp2)
        {
            /*
                 R = A/G
                 Y = C/T
                 S = G/C
                 W = A/T
                 K = G/T
                 M = A/C
             */

            if ((bp1 == "A" && bp2 == "G") || (bp1 == "G" && bp2 == "A"))
                return "R";
            else if ((bp1 == "C" && bp2 == "T") || (bp1 == "T" && bp2 == "C"))
                return "Y";
            else if ((bp1 == "C" && bp2 == "G") || (bp1 == "G" && bp2 == "C"))
                return "S";
            else if ((bp1 == "A" && bp2 == "T") || (bp1 == "T" && bp2 == "A"))
                return "W";
            else if ((bp1 == "T" && bp2 == "G") || (bp1 == "G" && bp2 == "T"))
                return "K";
            else if ((bp1 == "A" && bp2 == "C") || (bp1 == "C" && bp2 == "A"))
                return "M";
            else
                return "N";
        }

        public void autosomalSingleSNPPhase(string child, string father, string mother, DataRow row, ref string phased_paternal, ref string phased_maternal)
        {
            if (father.Contains(child[0].ToString()))
            {
                phased_paternal = child[0].ToString();
                row.SetField(6, child[0].ToString());
            }
            if (mother.Contains(child[0].ToString()))
            {
                phased_maternal = child[0].ToString();
                row.SetField(7, child[0].ToString());
            }
            if (father.Contains(child[1].ToString()))
            {
                phased_paternal = child[1].ToString();
                row.SetField(6, child[1].ToString());
            }
            if (mother.Contains(child[1].ToString()))
            {
                phased_maternal = child[1].ToString();
                row.SetField(7, child[1].ToString());
            }
        }

        private void bwPhasing_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
            GGKUtilLib.setStatus("Done.");
            dgvPhasing.Columns.Clear();
            dgvPhasing.DataSource = dt;
            dgvPhasing.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvPhasing.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvPhasing.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvPhasing.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvPhasing.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvPhasing.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvPhasing.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvPhasing.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            DataGridViewCellStyle style=new DataGridViewCellStyle();
            style.BackColor = Color.Red;
            style.ForeColor=Color.White;
            foreach (int idx in mutatedRow)            
                dgvPhasing.Rows[idx].DefaultCellStyle = style;

            
                DataGridViewCellStyle style2=new DataGridViewCellStyle();
            style2.BackColor = Color.LightGray;

            foreach (int idx in ambiguousRow)            
                dgvPhasing.Rows[idx].DefaultCellStyle = style2;


            btnPhasing.Enabled = true;
            btnChild.Enabled = true;
            btnFather.Enabled = true;
            btnMother.Enabled = true;
            rbMale.Enabled = true;
            rbFemale.Enabled = true;
        }

        private void btnFather_Click(object sender, EventArgs e)
        {
            SelectKitFrm open = new SelectKitFrm(SelectKitFrm.SELECT_KIT);
            open.ShowDialog(this);
            father_kit = open.getSelectedKit();
            btnFather.Text = GGKUtilLib.getKitName(father_kit);
            if ((father_kit != "Unknown" || mother_kit != "Unknown") && child_kit != "Unknown")
                btnPhasing.Enabled = true;
        }

        private void btnMother_Click(object sender, EventArgs e)
        {
            SelectKitFrm open = new SelectKitFrm(SelectKitFrm.SELECT_KIT);
            open.ShowDialog(this);
            mother_kit = open.getSelectedKit();
            btnMother.Text = GGKUtilLib.getKitName(mother_kit);
            if ((father_kit != "Unknown" || mother_kit != "Unknown") && child_kit != "Unknown")
                btnPhasing.Enabled = true;
        }

        private void btnChild_Click(object sender, EventArgs e)
        {
            SelectKitFrm open = new SelectKitFrm(SelectKitFrm.SELECT_KIT);
            open.ShowDialog(this);
            child_kit = open.getSelectedKit();
            btnChild.Text = GGKUtilLib.getKitName(child_kit);
            if ((father_kit != "Unknown" || mother_kit != "Unknown") && child_kit != "Unknown")
                btnPhasing.Enabled = true;
        }

        private void btnPhasing_Click(object sender, EventArgs e)
        {
            btnPhasing.Enabled = false;
            btnChild.Enabled = false;
            btnFather.Enabled = false;
            btnMother.Enabled = false;
            rbMale.Enabled = false;
            rbFemale.Enabled = false;
            GGKUtilLib.setStatus("Phasing...");
            male = rbMale.Checked;
            bwPhasing.RunWorkerAsync();
        }

        private void bwPhasing_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            GGKUtilLib.setStatus(e.UserState.ToString());
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if(rbMale.Checked)            
                pbChild.Image = Genetic_Genealogy_Kit.Properties.Resources.boy;            
            else
                pbChild.Image = Genetic_Genealogy_Kit.Properties.Resources.girl;       
        }
    }
}

namespace GenetixKit.Forms
{
    partial class GKMainFrm
    {
        private System.Windows.Forms.MenuStrip menuStripGGK;
        private System.Windows.Forms.ToolStripMenuItem miFile;
        private System.Windows.Forms.ToolStripMenuItem miExit;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLbl;
        private System.Windows.Forms.ToolStripMenuItem miNew;
        private System.Windows.Forms.ToolStripMenuItem miOpen;
        private System.Windows.Forms.ToolStripMenuItem miSave;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.ToolStripMenuItem miEdit;
        private System.Windows.Forms.ToolStripMenuItem miDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem miDNA;
        private System.Windows.Forms.ToolStripMenuItem miOneToOne;
        private System.Windows.Forms.ToolStripMenuItem miOneToMany;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem miProcessKits;
        private System.Windows.Forms.ToolStripMenuItem miQuickEdit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem miAdmixture;
        private System.Windows.Forms.ToolStripMenuItem miRunsOfHomozygosity;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem miMtDnaPhylogeny;
        private System.Windows.Forms.ToolStripMenuItem miMitoMap;
        private System.Windows.Forms.ToolStripMenuItem miISOGGYTree;
        private System.Windows.Forms.ToolStripMenuItem miPhasing;

        private void InitializeComponent()
        {
            this.menuStripGGK = new System.Windows.Forms.MenuStrip();
            this.miFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miNew = new System.Windows.Forms.ToolStripMenuItem();
            this.miOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.miSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.miExit = new System.Windows.Forms.ToolStripMenuItem();
            this.miEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.miQuickEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.miDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.miDNA = new System.Windows.Forms.ToolStripMenuItem();
            this.miOneToOne = new System.Windows.Forms.ToolStripMenuItem();
            this.miOneToMany = new System.Windows.Forms.ToolStripMenuItem();
            this.miAdmixture = new System.Windows.Forms.ToolStripMenuItem();
            this.miRunsOfHomozygosity = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.miProcessKits = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.miMtDnaPhylogeny = new System.Windows.Forms.ToolStripMenuItem();
            this.miMitoMap = new System.Windows.Forms.ToolStripMenuItem();
            this.miISOGGYTree = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.statusLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.miPhasing = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripGGK.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripGGK
            // 
            this.menuStripGGK.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStripGGK.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFile,
            this.miEdit,
            this.miDNA});
            this.menuStripGGK.Location = new System.Drawing.Point(0, 0);
            this.menuStripGGK.Name = "menuStripGGK";
            this.menuStripGGK.Size = new System.Drawing.Size(1045, 28);
            this.menuStripGGK.TabIndex = 0;
            this.menuStripGGK.Text = "menuStrip1";
            // 
            // miFile
            // 
            this.miFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miNew,
            this.miOpen,
            this.miSave,
            this.toolStripSeparator1,
            this.miExit});
            this.miFile.Name = "miFile";
            this.miFile.Size = new System.Drawing.Size(46, 24);
            this.miFile.Text = "&File";
            // 
            // miNew
            // 
            this.miNew.Name = "miNew";
            this.miNew.Size = new System.Drawing.Size(224, 26);
            this.miNew.Text = "&New";
            this.miNew.Click += new System.EventHandler(this.miNew_Click);
            // 
            // miOpen
            // 
            this.miOpen.Name = "miOpen";
            this.miOpen.Size = new System.Drawing.Size(224, 26);
            this.miOpen.Text = "&Open";
            this.miOpen.Click += new System.EventHandler(this.miOpen_Click);
            // 
            // miSave
            // 
            this.miSave.Enabled = false;
            this.miSave.Name = "miSave";
            this.miSave.Size = new System.Drawing.Size(224, 26);
            this.miSave.Text = "&Save";
            this.miSave.Click += new System.EventHandler(this.miSave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(221, 6);
            // 
            // miExit
            // 
            this.miExit.Name = "miExit";
            this.miExit.Size = new System.Drawing.Size(224, 26);
            this.miExit.Text = "E&xit";
            this.miExit.Click += new System.EventHandler(this.miExit_Click);
            // 
            // miEdit
            // 
            this.miEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miQuickEdit,
            this.toolStripSeparator5,
            this.miDelete});
            this.miEdit.Name = "miEdit";
            this.miEdit.Size = new System.Drawing.Size(49, 24);
            this.miEdit.Text = "&Edit";
            // 
            // miQuickEdit
            // 
            this.miQuickEdit.Name = "miQuickEdit";
            this.miQuickEdit.Size = new System.Drawing.Size(224, 26);
            this.miQuickEdit.Text = "Quick &Edit";
            this.miQuickEdit.Click += new System.EventHandler(this.miQuickEdit_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(221, 6);
            // 
            // miDelete
            // 
            this.miDelete.Enabled = false;
            this.miDelete.Name = "miDelete";
            this.miDelete.Size = new System.Drawing.Size(224, 26);
            this.miDelete.Text = "Delete";
            this.miDelete.Click += new System.EventHandler(this.miDelete_Click);
            // 
            // miDNA
            // 
            this.miDNA.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miOneToOne,
            this.miOneToMany,
            this.miAdmixture,
            this.miRunsOfHomozygosity,
            this.miPhasing,
            this.toolStripSeparator3,
            this.miProcessKits,
            this.toolStripMenuItem1,
            this.miMtDnaPhylogeny,
            this.miMitoMap,
            this.miISOGGYTree});
            this.miDNA.Name = "miDNA";
            this.miDNA.Size = new System.Drawing.Size(95, 24);
            this.miDNA.Text = "&DNA";
            // 
            // miOneToOne
            // 
            this.miOneToOne.Name = "miOneToOne";
            this.miOneToOne.Size = new System.Drawing.Size(241, 26);
            this.miOneToOne.Text = "&One-to-One";
            this.miOneToOne.Click += new System.EventHandler(this.miOneToOne_Click);
            // 
            // miOneToMany
            // 
            this.miOneToMany.Name = "miOneToMany";
            this.miOneToMany.Size = new System.Drawing.Size(241, 26);
            this.miOneToMany.Text = "One-to-&Many";
            this.miOneToMany.Click += new System.EventHandler(this.miOneToMany_Click);
            // 
            // miAdmixture
            // 
            this.miAdmixture.Name = "miAdmixture";
            this.miAdmixture.Size = new System.Drawing.Size(194, 22);
            this.miAdmixture.Text = "&Admixture";
            this.miAdmixture.Click += new System.EventHandler(this.miAdmixture_Click);
            // 
            // miRunsOfHomozygosity
            // 
            this.miRunsOfHomozygosity.Name = "miRunsOfHomozygosity";
            this.miRunsOfHomozygosity.Size = new System.Drawing.Size(241, 26);
            this.miRunsOfHomozygosity.Text = "&Runs of Homozygosity";
            this.miRunsOfHomozygosity.Click += new System.EventHandler(this.miRunsOfHomozygosity_Click);
            // 
            // miPhasing
            // 
            this.miPhasing.Name = "miPhasing";
            this.miPhasing.Size = new System.Drawing.Size(194, 22);
            this.miPhasing.Text = "Phasing &Utility";
            this.miPhasing.Click += new System.EventHandler(this.miPhasing_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(238, 6);
            // 
            // miProcessKits
            // 
            this.miProcessKits.Name = "miProcessKits";
            this.miProcessKits.Size = new System.Drawing.Size(241, 26);
            this.miProcessKits.Text = "&Process Kits";
            this.miProcessKits.Click += new System.EventHandler(this.miProcessKits_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(238, 6);
            // 
            // miMtDnaPhylogeny
            // 
            this.miMtDnaPhylogeny.Name = "miMtDnaPhylogeny";
            this.miMtDnaPhylogeny.Size = new System.Drawing.Size(241, 26);
            this.miMtDnaPhylogeny.Text = "Mt-Dna &Phylogeny";
            this.miMtDnaPhylogeny.Click += new System.EventHandler(this.miMtDnaPhylogeny_Click);
            // 
            // miMitoMap
            // 
            this.miMitoMap.Name = "miMitoMap";
            this.miMitoMap.Size = new System.Drawing.Size(241, 26);
            this.miMitoMap.Text = "Mito &Map";
            this.miMitoMap.Click += new System.EventHandler(this.miMitoMap_Click);
            // 
            // miISOGGYTree
            // 
            this.miISOGGYTree.Name = "miISOGGYTree";
            this.miISOGGYTree.Size = new System.Drawing.Size(241, 26);
            this.miISOGGYTree.Text = "&ISOGG Y-Tree";
            this.miISOGGYTree.Click += new System.EventHandler(this.miISOGGYTree_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressBar,
            this.statusLbl});
            this.statusStrip1.Location = new System.Drawing.Point(0, 666);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 20, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1045, 26);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // progressBar
            // 
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(133, 18);
            this.progressBar.Visible = false;
            // 
            // statusLbl
            // 
            this.statusLbl.Name = "statusLbl";
            this.statusLbl.Size = new System.Drawing.Size(48, 20);
            this.statusLbl.Text = "Done.";
            // 
            // GGKitFrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1480, 692);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStripGGK);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStripGGK;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(1061, 728);
            this.Name = "GGKitFrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Genetic Genealogy Kit (GGK)";
            this.Load += new System.EventHandler(this.GGKitFrmMain_Load);
            this.menuStripGGK.ResumeLayout(false);
            this.menuStripGGK.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

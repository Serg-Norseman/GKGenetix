namespace GGKit.Forms
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
        private System.Windows.Forms.ToolStripMenuItem miTest;
        private System.Windows.Forms.ToolStripMenuItem miDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem miDNA;
        private System.Windows.Forms.ToolStripMenuItem miOneToOne;
        private System.Windows.Forms.ToolStripMenuItem miOneToMany;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem miProcessKits;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem miAdmixture;
        private System.Windows.Forms.ToolStripMenuItem miRunsOfHomozygosity;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem miMtDnaPhylogeny;
        private System.Windows.Forms.ToolStripMenuItem miMitoMap;
        private System.Windows.Forms.ToolStripMenuItem miISOGGYTree;
        private System.Windows.Forms.ToolStripMenuItem miPhasing;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private GGKit.Forms.KitsExplorer kitsExplorer;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnWidgetClose;
        private System.Windows.Forms.Label lblWidgetTitle;
        private System.Windows.Forms.Panel panWidget;
        private System.Windows.Forms.ToolStripMenuItem miImport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem miDevConsole;

        private void InitializeComponent()
        {
            this.menuStripGGK = new System.Windows.Forms.MenuStrip();
            this.miFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miNew = new System.Windows.Forms.ToolStripMenuItem();
            this.miOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.miSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.miExit = new System.Windows.Forms.ToolStripMenuItem();
            this.miTest = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.miDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.miDNA = new System.Windows.Forms.ToolStripMenuItem();
            this.miOneToOne = new System.Windows.Forms.ToolStripMenuItem();
            this.miOneToMany = new System.Windows.Forms.ToolStripMenuItem();
            this.miAdmixture = new System.Windows.Forms.ToolStripMenuItem();
            this.miRunsOfHomozygosity = new System.Windows.Forms.ToolStripMenuItem();
            this.miPhasing = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.miProcessKits = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.miMtDnaPhylogeny = new System.Windows.Forms.ToolStripMenuItem();
            this.miMitoMap = new System.Windows.Forms.ToolStripMenuItem();
            this.miISOGGYTree = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.statusLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.kitsExplorer = new GGKit.Forms.KitsExplorer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panWidget = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnWidgetClose = new System.Windows.Forms.Button();
            this.lblWidgetTitle = new System.Windows.Forms.Label();
            this.miImport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.miDevConsole = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripGGK.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripGGK
            // 
            this.menuStripGGK.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStripGGK.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFile,
            this.miTest,
            this.miDNA});
            this.menuStripGGK.Location = new System.Drawing.Point(0, 0);
            this.menuStripGGK.Name = "menuStripGGK";
            this.menuStripGGK.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStripGGK.Size = new System.Drawing.Size(1110, 24);
            this.menuStripGGK.TabIndex = 0;
            this.menuStripGGK.Text = "menuStrip1";
            // 
            // miFile
            // 
            this.miFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miDevConsole,
            this.toolStripSeparator1,
            this.miExit});
            this.miFile.Name = "miFile";
            this.miFile.Size = new System.Drawing.Size(37, 20);
            this.miFile.Text = "&File";
            // 
            // miNew
            // 
            this.miNew.Name = "miNew";
            this.miNew.Size = new System.Drawing.Size(103, 22);
            this.miNew.Text = "&New";
            this.miNew.Click += new System.EventHandler(this.miNew_Click);
            // 
            // miOpen
            // 
            this.miOpen.Name = "miOpen";
            this.miOpen.Size = new System.Drawing.Size(103, 22);
            this.miOpen.Text = "&Open";
            this.miOpen.Click += new System.EventHandler(this.miOpen_Click);
            // 
            // miImport
            // 
            this.miImport.Name = "miImport";
            this.miImport.Size = new System.Drawing.Size(103, 22);
            this.miImport.Text = "&Import";
            this.miImport.Click += new System.EventHandler(this.miImport_Click);
            // 
            // miSave
            // 
            this.miSave.Enabled = false;
            this.miSave.Name = "miSave";
            this.miSave.Size = new System.Drawing.Size(103, 22);
            this.miSave.Text = "&Save";
            this.miSave.Click += new System.EventHandler(this.miSave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(100, 6);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(100, 6);
            // 
            // miExit
            // 
            this.miExit.Name = "miExit";
            this.miExit.Size = new System.Drawing.Size(103, 22);
            this.miExit.Text = "E&xit";
            this.miExit.Click += new System.EventHandler(this.miExit_Click);
            // 
            // miTest
            // 
            this.miTest.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miNew,
            this.miOpen,
            this.miSave,
            this.toolStripSeparator2,
            this.miImport,
            this.toolStripSeparator5,
            this.miDelete});
            this.miTest.Name = "miTest";
            this.miTest.Size = new System.Drawing.Size(39, 20);
            this.miTest.Text = "&Test";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(104, 6);
            // 
            // miDelete
            // 
            this.miDelete.Enabled = false;
            this.miDelete.Name = "miDelete";
            this.miDelete.Size = new System.Drawing.Size(107, 22);
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
            this.miDNA.Size = new System.Drawing.Size(44, 20);
            this.miDNA.Text = "&DNA";
            // 
            // miOneToOne
            // 
            this.miOneToOne.Name = "miOneToOne";
            this.miOneToOne.Size = new System.Drawing.Size(194, 22);
            this.miOneToOne.Text = "&One-to-One";
            this.miOneToOne.Click += new System.EventHandler(this.miOneToOne_Click);
            // 
            // miOneToMany
            // 
            this.miOneToMany.Name = "miOneToMany";
            this.miOneToMany.Size = new System.Drawing.Size(194, 22);
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
            this.miRunsOfHomozygosity.Size = new System.Drawing.Size(194, 22);
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
            this.toolStripSeparator3.Size = new System.Drawing.Size(191, 6);
            // 
            // miProcessKits
            // 
            this.miProcessKits.Name = "miProcessKits";
            this.miProcessKits.Size = new System.Drawing.Size(194, 22);
            this.miProcessKits.Text = "&Process Kits";
            this.miProcessKits.Click += new System.EventHandler(this.miProcessKits_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(191, 6);
            // 
            // miMtDnaPhylogeny
            // 
            this.miMtDnaPhylogeny.Name = "miMtDnaPhylogeny";
            this.miMtDnaPhylogeny.Size = new System.Drawing.Size(194, 22);
            this.miMtDnaPhylogeny.Text = "Mt-Dna &Phylogeny";
            this.miMtDnaPhylogeny.Click += new System.EventHandler(this.miMtDnaPhylogeny_Click);
            // 
            // miMitoMap
            // 
            this.miMitoMap.Name = "miMitoMap";
            this.miMitoMap.Size = new System.Drawing.Size(194, 22);
            this.miMitoMap.Text = "Mito &Map";
            this.miMitoMap.Click += new System.EventHandler(this.miMitoMap_Click);
            // 
            // miISOGGYTree
            // 
            this.miISOGGYTree.Name = "miISOGGYTree";
            this.miISOGGYTree.Size = new System.Drawing.Size(194, 22);
            this.miISOGGYTree.Text = "&ISOGG Y-Tree";
            this.miISOGGYTree.Click += new System.EventHandler(this.miISOGGYTree_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressBar,
            this.statusLbl});
            this.statusStrip1.Location = new System.Drawing.Point(0, 540);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 15, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1110, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // progressBar
            // 
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 16);
            this.progressBar.Visible = false;
            // 
            // statusLbl
            // 
            this.statusLbl.Name = "statusLbl";
            this.statusLbl.Size = new System.Drawing.Size(38, 17);
            this.statusLbl.Text = "Done.";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.kitsExplorer);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(1110, 516);
            this.splitContainer1.SplitterDistance = 481;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 0;
            // 
            // kitsExplorer
            // 
            this.kitsExplorer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kitsExplorer.Location = new System.Drawing.Point(0, 0);
            this.kitsExplorer.Margin = new System.Windows.Forms.Padding(2);
            this.kitsExplorer.Name = "kitsExplorer";
            this.kitsExplorer.Size = new System.Drawing.Size(481, 516);
            this.kitsExplorer.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panWidget);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(627, 516);
            this.panel1.TabIndex = 3;
            // 
            // panWidget
            // 
            this.panWidget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panWidget.Location = new System.Drawing.Point(0, 28);
            this.panWidget.Name = "panWidget";
            this.panWidget.Size = new System.Drawing.Size(627, 488);
            this.panWidget.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnWidgetClose);
            this.panel2.Controls.Add(this.lblWidgetTitle);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(4);
            this.panel2.Size = new System.Drawing.Size(627, 28);
            this.panel2.TabIndex = 0;
            // 
            // btnWidgetClose
            // 
            this.btnWidgetClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnWidgetClose.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnWidgetClose.Location = new System.Drawing.Point(601, 4);
            this.btnWidgetClose.Margin = new System.Windows.Forms.Padding(0);
            this.btnWidgetClose.Name = "btnWidgetClose";
            this.btnWidgetClose.Size = new System.Drawing.Size(20, 18);
            this.btnWidgetClose.TabIndex = 1;
            this.btnWidgetClose.Text = "X";
            this.btnWidgetClose.UseVisualStyleBackColor = true;
            this.btnWidgetClose.Click += new System.EventHandler(this.btnWidgetClose_Click);
            // 
            // lblWidgetTitle
            // 
            this.lblWidgetTitle.AutoSize = true;
            this.lblWidgetTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblWidgetTitle.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblWidgetTitle.Location = new System.Drawing.Point(4, 4);
            this.lblWidgetTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblWidgetTitle.Name = "lblWidgetTitle";
            this.lblWidgetTitle.Size = new System.Drawing.Size(98, 18);
            this.lblWidgetTitle.TabIndex = 0;
            this.lblWidgetTitle.Text = "...";
            // 
            // miDevConsole
            // 
            this.miDevConsole.Name = "miDevConsole";
            this.miDevConsole.Size = new System.Drawing.Size(194, 22);
            this.miDevConsole.Text = "&Dev Console";
            this.miDevConsole.Click += new System.EventHandler(this.miDevConsole_Click);
            // 
            // GKMainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1210, 562);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStripGGK);
            this.MainMenuStrip = this.menuStripGGK;
            this.MinimumSize = new System.Drawing.Size(800, 599);
            this.Name = "GKMainFrm";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GenetixKit";
            this.Load += new System.EventHandler(this.GKMainFrm_Load);
            this.menuStripGGK.ResumeLayout(false);
            this.menuStripGGK.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

namespace Genetic_Genealogy_Kit
{
    partial class GGKitFrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GGKitFrmMain));
            this.menuStripGGK = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quickEditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.enableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autosomalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.onetoOneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.onetoManyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.admixtureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runsOfHomozygosityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.phasingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.processKitsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.yDnaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iSOGGYTreeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mitochondrialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mtDnaPhylogenyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mitoMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.advancedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.factoryResetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.licenseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.statusLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripGGK = new System.Windows.Forms.ToolStrip();
            this.newToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripEnableIcon = new System.Windows.Forms.ToolStripButton();
            this.toolStripDisableBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripDeleteBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.openFileDialogGGK = new System.Windows.Forms.OpenFileDialog();
            this.bwImport = new System.ComponentModel.BackgroundWorker();
            this.bwIChkAndFix = new System.ComponentModel.BackgroundWorker();
            this.menuStripGGK.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStripGGK.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripGGK
            // 
            this.menuStripGGK.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.autosomalToolStripMenuItem,
            this.yDnaToolStripMenuItem,
            this.mitochondrialToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStripGGK.Location = new System.Drawing.Point(0, 0);
            this.menuStripGGK.Name = "menuStripGGK";
            this.menuStripGGK.Size = new System.Drawing.Size(784, 24);
            this.menuStripGGK.TabIndex = 0;
            this.menuStripGGK.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripSeparator1,
            this.importToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(107, 6);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.importToolStripMenuItem.Text = "&Import";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.exportToolStripMenuItem.Text = "&Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(107, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quickEditToolStripMenuItem,
            this.toolStripSeparator5,
            this.enableToolStripMenuItem,
            this.disableToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // quickEditToolStripMenuItem
            // 
            this.quickEditToolStripMenuItem.Name = "quickEditToolStripMenuItem";
            this.quickEditToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.quickEditToolStripMenuItem.Text = "Quick &Edit";
            this.quickEditToolStripMenuItem.Click += new System.EventHandler(this.quickEditToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(125, 6);
            // 
            // enableToolStripMenuItem
            // 
            this.enableToolStripMenuItem.Enabled = false;
            this.enableToolStripMenuItem.Name = "enableToolStripMenuItem";
            this.enableToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.enableToolStripMenuItem.Text = "&Enable";
            this.enableToolStripMenuItem.Click += new System.EventHandler(this.enableToolStripMenuItem_Click);
            // 
            // disableToolStripMenuItem
            // 
            this.disableToolStripMenuItem.Enabled = false;
            this.disableToolStripMenuItem.Name = "disableToolStripMenuItem";
            this.disableToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.disableToolStripMenuItem.Text = "&Disable";
            this.disableToolStripMenuItem.Click += new System.EventHandler(this.disableToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Enabled = false;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // autosomalToolStripMenuItem
            // 
            this.autosomalToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.onetoOneToolStripMenuItem,
            this.onetoManyToolStripMenuItem,
            this.admixtureToolStripMenuItem,
            this.runsOfHomozygosityToolStripMenuItem,
            this.phasingToolStripMenuItem,
            this.toolStripSeparator3,
            this.processKitsToolStripMenuItem});
            this.autosomalToolStripMenuItem.Name = "autosomalToolStripMenuItem";
            this.autosomalToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.autosomalToolStripMenuItem.Text = "&Autosomal";
            // 
            // onetoOneToolStripMenuItem
            // 
            this.onetoOneToolStripMenuItem.Name = "onetoOneToolStripMenuItem";
            this.onetoOneToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.onetoOneToolStripMenuItem.Text = "&One-to-One";
            this.onetoOneToolStripMenuItem.Click += new System.EventHandler(this.onetoOneToolStripMenuItem_Click);
            // 
            // onetoManyToolStripMenuItem
            // 
            this.onetoManyToolStripMenuItem.Name = "onetoManyToolStripMenuItem";
            this.onetoManyToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.onetoManyToolStripMenuItem.Text = "One-to-&Many";
            this.onetoManyToolStripMenuItem.Click += new System.EventHandler(this.onetoManyToolStripMenuItem_Click);
            // 
            // admixtureToolStripMenuItem
            // 
            this.admixtureToolStripMenuItem.Name = "admixtureToolStripMenuItem";
            this.admixtureToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.admixtureToolStripMenuItem.Text = "&Admixture";
            this.admixtureToolStripMenuItem.Click += new System.EventHandler(this.admixtureToolStripMenuItem_Click);
            // 
            // runsOfHomozygosityToolStripMenuItem
            // 
            this.runsOfHomozygosityToolStripMenuItem.Name = "runsOfHomozygosityToolStripMenuItem";
            this.runsOfHomozygosityToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.runsOfHomozygosityToolStripMenuItem.Text = "&Runs of Homozygosity";
            this.runsOfHomozygosityToolStripMenuItem.Click += new System.EventHandler(this.runsOfHomozygosityToolStripMenuItem_Click);
            // 
            // phasingToolStripMenuItem
            // 
            this.phasingToolStripMenuItem.Name = "phasingToolStripMenuItem";
            this.phasingToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.phasingToolStripMenuItem.Text = "Phasing &Utility";
            this.phasingToolStripMenuItem.Click += new System.EventHandler(this.phasingToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(191, 6);
            // 
            // processKitsToolStripMenuItem
            // 
            this.processKitsToolStripMenuItem.Name = "processKitsToolStripMenuItem";
            this.processKitsToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.processKitsToolStripMenuItem.Text = "&Process Kits";
            this.processKitsToolStripMenuItem.Click += new System.EventHandler(this.processKitsToolStripMenuItem_Click);
            // 
            // yDnaToolStripMenuItem
            // 
            this.yDnaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iSOGGYTreeToolStripMenuItem});
            this.yDnaToolStripMenuItem.Name = "yDnaToolStripMenuItem";
            this.yDnaToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.yDnaToolStripMenuItem.Text = "&Y-Dna";
            // 
            // iSOGGYTreeToolStripMenuItem
            // 
            this.iSOGGYTreeToolStripMenuItem.Name = "iSOGGYTreeToolStripMenuItem";
            this.iSOGGYTreeToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.iSOGGYTreeToolStripMenuItem.Text = "&ISOGG Y-Tree";
            this.iSOGGYTreeToolStripMenuItem.Click += new System.EventHandler(this.iSOGGYTreeToolStripMenuItem_Click);
            // 
            // mitochondrialToolStripMenuItem
            // 
            this.mitochondrialToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mtDnaPhylogenyToolStripMenuItem,
            this.mitoMapToolStripMenuItem});
            this.mitochondrialToolStripMenuItem.Name = "mitochondrialToolStripMenuItem";
            this.mitochondrialToolStripMenuItem.Size = new System.Drawing.Size(94, 20);
            this.mitochondrialToolStripMenuItem.Text = "&Mitochondrial";
            // 
            // mtDnaPhylogenyToolStripMenuItem
            // 
            this.mtDnaPhylogenyToolStripMenuItem.Name = "mtDnaPhylogenyToolStripMenuItem";
            this.mtDnaPhylogenyToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.mtDnaPhylogenyToolStripMenuItem.Text = "Mt-Dna &Phylogeny";
            this.mtDnaPhylogenyToolStripMenuItem.Click += new System.EventHandler(this.mtDnaPhylogenyToolStripMenuItem_Click);
            // 
            // mitoMapToolStripMenuItem
            // 
            this.mitoMapToolStripMenuItem.Name = "mitoMapToolStripMenuItem";
            this.mitoMapToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.mitoMapToolStripMenuItem.Text = "Mito &Map";
            this.mitoMapToolStripMenuItem.Click += new System.EventHandler(this.mitoMapToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.advancedToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.settingsToolStripMenuItem.Text = "&Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // advancedToolStripMenuItem
            // 
            this.advancedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.factoryResetToolStripMenuItem});
            this.advancedToolStripMenuItem.Name = "advancedToolStripMenuItem";
            this.advancedToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.advancedToolStripMenuItem.Text = "&Advanced";
            this.advancedToolStripMenuItem.Visible = false;
            // 
            // factoryResetToolStripMenuItem
            // 
            this.factoryResetToolStripMenuItem.Name = "factoryResetToolStripMenuItem";
            this.factoryResetToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.factoryResetToolStripMenuItem.Text = "&Factory Reset";
            this.factoryResetToolStripMenuItem.Click += new System.EventHandler(this.factoryResetToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.licenseToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // licenseToolStripMenuItem
            // 
            this.licenseToolStripMenuItem.Name = "licenseToolStripMenuItem";
            this.licenseToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.licenseToolStripMenuItem.Text = "&License";
            this.licenseToolStripMenuItem.Click += new System.EventHandler(this.licenseToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressBar,
            this.statusLbl});
            this.statusStrip1.Location = new System.Drawing.Point(0, 540);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 15, 0);
            this.statusStrip1.Size = new System.Drawing.Size(784, 22);
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
            // toolStripGGK
            // 
            this.toolStripGGK.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton,
            this.openToolStripButton,
            this.saveToolStripButton,
            this.printToolStripButton,
            this.toolStripEnableIcon,
            this.toolStripDisableBtn,
            this.toolStripDeleteBtn,
            this.toolStripSeparator,
            this.helpToolStripButton});
            this.toolStripGGK.Location = new System.Drawing.Point(0, 24);
            this.toolStripGGK.Name = "toolStripGGK";
            this.toolStripGGK.Size = new System.Drawing.Size(784, 25);
            this.toolStripGGK.TabIndex = 5;
            this.toolStripGGK.Text = "toolStrip1";
            // 
            // newToolStripButton
            // 
            this.newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripButton.Image")));
            this.newToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripButton.Name = "newToolStripButton";
            this.newToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.newToolStripButton.Text = "&New Kit";
            this.newToolStripButton.Click += new System.EventHandler(this.newToolStripButton_Click);
            // 
            // openToolStripButton
            // 
            this.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripButton.Image")));
            this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripButton.Name = "openToolStripButton";
            this.openToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.openToolStripButton.Text = "&Open";
            this.openToolStripButton.Click += new System.EventHandler(this.openToolStripButton_Click);
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveToolStripButton.Enabled = false;
            this.saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripButton.Image")));
            this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.saveToolStripButton.Text = "&Save";
            this.saveToolStripButton.Click += new System.EventHandler(this.saveToolStripButton_Click);
            // 
            // printToolStripButton
            // 
            this.printToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.printToolStripButton.Enabled = false;
            this.printToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("printToolStripButton.Image")));
            this.printToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printToolStripButton.Name = "printToolStripButton";
            this.printToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.printToolStripButton.Text = "&Print";
            // 
            // toolStripEnableIcon
            // 
            this.toolStripEnableIcon.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripEnableIcon.Enabled = false;
            this.toolStripEnableIcon.Image = global::Genetic_Genealogy_Kit.Properties.Resources.enable;
            this.toolStripEnableIcon.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripEnableIcon.Name = "toolStripEnableIcon";
            this.toolStripEnableIcon.Size = new System.Drawing.Size(23, 22);
            this.toolStripEnableIcon.Text = "Enable";
            this.toolStripEnableIcon.Click += new System.EventHandler(this.toolStripEnableIcon_Click);
            // 
            // toolStripDisableBtn
            // 
            this.toolStripDisableBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDisableBtn.Enabled = false;
            this.toolStripDisableBtn.Image = global::Genetic_Genealogy_Kit.Properties.Resources.disable;
            this.toolStripDisableBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDisableBtn.Name = "toolStripDisableBtn";
            this.toolStripDisableBtn.Size = new System.Drawing.Size(23, 22);
            this.toolStripDisableBtn.Text = "Disable";
            this.toolStripDisableBtn.Click += new System.EventHandler(this.toolStripDisableBtn_Click);
            // 
            // toolStripDeleteBtn
            // 
            this.toolStripDeleteBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDeleteBtn.Enabled = false;
            this.toolStripDeleteBtn.Image = global::Genetic_Genealogy_Kit.Properties.Resources.delete;
            this.toolStripDeleteBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDeleteBtn.Name = "toolStripDeleteBtn";
            this.toolStripDeleteBtn.Size = new System.Drawing.Size(23, 22);
            this.toolStripDeleteBtn.Text = "Delete";
            this.toolStripDeleteBtn.Click += new System.EventHandler(this.toolStripDeleteBtn_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // helpToolStripButton
            // 
            this.helpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.helpToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("helpToolStripButton.Image")));
            this.helpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.helpToolStripButton.Name = "helpToolStripButton";
            this.helpToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.helpToolStripButton.Text = "He&lp";
            // 
            // openFileDialogGGK
            // 
            this.openFileDialogGGK.Filter = "Genetic Genealogy Kit|*.ggk";
            // 
            // bwImport
            // 
            this.bwImport.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwImport_DoWork);
            this.bwImport.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwImport_RunWorkerCompleted);
            // 
            // bwIChkAndFix
            // 
            this.bwIChkAndFix.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwIChkAndFix_DoWork);
            this.bwIChkAndFix.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwIChkAndFix_RunWorkerCompleted);
            // 
            // GGKitFrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.toolStripGGK);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStripGGK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStripGGK;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "GGKitFrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Genetic Genealogy Kit (GGK)";
            this.Load += new System.EventHandler(this.GGKitFrmMain_Load);
            this.menuStripGGK.ResumeLayout(false);
            this.menuStripGGK.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStripGGK.ResumeLayout(false);
            this.toolStripGGK.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripGGK;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLbl;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem licenseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStripGGK;
        private System.Windows.Forms.ToolStripButton newToolStripButton;
        private System.Windows.Forms.ToolStripButton openToolStripButton;
        private System.Windows.Forms.ToolStripButton saveToolStripButton;
        private System.Windows.Forms.ToolStripButton printToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripButton helpToolStripButton;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.ToolStripButton toolStripDeleteBtn;
        private System.Windows.Forms.ToolStripButton toolStripDisableBtn;
        private System.Windows.Forms.ToolStripButton toolStripEnableIcon;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialogGGK;
        private System.ComponentModel.BackgroundWorker bwImport;
        private System.Windows.Forms.ToolStripMenuItem autosomalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem onetoOneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem onetoManyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem advancedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem factoryResetToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker bwIChkAndFix;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem processKitsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem admixtureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quickEditToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem runsOfHomozygosityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem phasingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem yDnaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mitochondrialToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mtDnaPhylogenyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mitoMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iSOGGYTreeToolStripMenuItem;
    }
}


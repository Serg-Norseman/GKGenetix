namespace GKGenetix.UI.Forms
{
    partial class NewEditKitFrm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblKitNo;
        private System.Windows.Forms.TabControl tabsNewKit;
        private System.Windows.Forms.TabPage tabAutosomal;
        private System.Windows.Forms.TabPage tabYDNA;
        private System.Windows.Forms.TabPage tabMtDNA;
        private System.Windows.Forms.TabControl tabsY;
        private System.Windows.Forms.TabPage tabYSNPs;
        private System.Windows.Forms.TabPage tabYSTR;
        private System.Windows.Forms.DataGridView dgvAutosomal;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtKit;
        private System.ComponentModel.BackgroundWorker bwNewKitAutosomalJob;
        private System.Windows.Forms.TextBox txtYDNA;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblY37;
        private System.Windows.Forms.Label lblY25;
        private System.Windows.Forms.Label lblY67;
        private System.Windows.Forms.Label lblY111;
        private System.Windows.Forms.Label lblYMisc;
        private System.Windows.Forms.DataGridView dgvYMisc;
        private System.Windows.Forms.DataGridView dgvY111;
        private System.Windows.Forms.DataGridView dgvY67;
        private System.Windows.Forms.DataGridView dgvY37;
        private System.Windows.Forms.DataGridView dgvY25;
        private System.Windows.Forms.DataGridView dgvY12;
        private System.Windows.Forms.TextBox txtMtDNA;
        private System.Windows.Forms.Label tipLbl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label lblY12;
        private System.Windows.Forms.Button btnPasteY;
        private System.Windows.Forms.Button btnClearY;
        private System.ComponentModel.BackgroundWorker bwSave;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.ComponentModel.BackgroundWorker bwPopulate;
        private System.Windows.Forms.ContextMenuStrip mnuYDNAMisc;
        private System.Windows.Forms.ToolStripMenuItem miDeleteRowYDNAMisc;
        private System.Windows.Forms.ContextMenuStrip mnuAutosomal;
        private System.Windows.Forms.ToolStripMenuItem miDeleteRowAutosomal;
        private System.Windows.Forms.ToolStripMenuItem miClearAllYMisc;
        private System.Windows.Forms.ToolStripMenuItem miClearAllAutosomal;
        private System.Windows.Forms.Label lblSex;
        private System.Windows.Forms.ComboBox cbSex;
        private System.Windows.Forms.TabControl tabsMt;
        private System.Windows.Forms.TabPage tabMutations;
        private System.Windows.Forms.TabPage tabFasta;
        private System.Windows.Forms.TextBox txtFASTA;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewEditKitFrm));
            this.lblName = new System.Windows.Forms.Label();
            this.lblKitNo = new System.Windows.Forms.Label();
            this.tabsNewKit = new System.Windows.Forms.TabControl();
            this.tabAutosomal = new System.Windows.Forms.TabPage();
            this.dgvAutosomal = new System.Windows.Forms.DataGridView();
            this.mnuAutosomal = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miDeleteRowAutosomal = new System.Windows.Forms.ToolStripMenuItem();
            this.miClearAllAutosomal = new System.Windows.Forms.ToolStripMenuItem();
            this.tabYDNA = new System.Windows.Forms.TabPage();
            this.tabsY = new System.Windows.Forms.TabControl();
            this.tabYSNPs = new System.Windows.Forms.TabPage();
            this.txtYDNA = new System.Windows.Forms.TextBox();
            this.tabYSTR = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dgvYMisc = new System.Windows.Forms.DataGridView();
            this.mnuYDNAMisc = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miDeleteRowYDNAMisc = new System.Windows.Forms.ToolStripMenuItem();
            this.miClearAllYMisc = new System.Windows.Forms.ToolStripMenuItem();
            this.dgvY111 = new System.Windows.Forms.DataGridView();
            this.dgvY67 = new System.Windows.Forms.DataGridView();
            this.dgvY37 = new System.Windows.Forms.DataGridView();
            this.dgvY25 = new System.Windows.Forms.DataGridView();
            this.lblY37 = new System.Windows.Forms.Label();
            this.lblY25 = new System.Windows.Forms.Label();
            this.lblY67 = new System.Windows.Forms.Label();
            this.lblY111 = new System.Windows.Forms.Label();
            this.lblYMisc = new System.Windows.Forms.Label();
            this.dgvY12 = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnPasteY = new System.Windows.Forms.Button();
            this.btnClearY = new System.Windows.Forms.Button();
            this.lblY12 = new System.Windows.Forms.Label();
            this.tabMtDNA = new System.Windows.Forms.TabPage();
            this.tabsMt = new System.Windows.Forms.TabControl();
            this.tabMutations = new System.Windows.Forms.TabPage();
            this.txtMtDNA = new System.Windows.Forms.TextBox();
            this.tabFasta = new System.Windows.Forms.TabPage();
            this.txtFASTA = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtKit = new System.Windows.Forms.TextBox();
            this.bwNewKitAutosomalJob = new System.ComponentModel.BackgroundWorker();
            this.tipLbl = new System.Windows.Forms.Label();
            this.bwSave = new System.ComponentModel.BackgroundWorker();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.bwPopulate = new System.ComponentModel.BackgroundWorker();
            this.lblSex = new System.Windows.Forms.Label();
            this.cbSex = new System.Windows.Forms.ComboBox();
            this.tabsNewKit.SuspendLayout();
            this.tabAutosomal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAutosomal)).BeginInit();
            this.mnuAutosomal.SuspendLayout();
            this.tabYDNA.SuspendLayout();
            this.tabsY.SuspendLayout();
            this.tabYSNPs.SuspendLayout();
            this.tabYSTR.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvYMisc)).BeginInit();
            this.mnuYDNAMisc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvY111)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvY67)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvY37)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvY25)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvY12)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabMtDNA.SuspendLayout();
            this.tabsMt.SuspendLayout();
            this.tabMutations.SuspendLayout();
            this.tabFasta.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(13, 13);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name";
            // 
            // lblKitNo
            // 
            this.lblKitNo.AutoSize = true;
            this.lblKitNo.Location = new System.Drawing.Point(13, 45);
            this.lblKitNo.Name = "lblKitNo";
            this.lblKitNo.Size = new System.Drawing.Size(29, 13);
            this.lblKitNo.TabIndex = 1;
            this.lblKitNo.Text = "Kit #";
            // 
            // tabsNewKit
            // 
            this.tabsNewKit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabsNewKit.Controls.Add(this.tabAutosomal);
            this.tabsNewKit.Controls.Add(this.tabYDNA);
            this.tabsNewKit.Controls.Add(this.tabMtDNA);
            this.tabsNewKit.Location = new System.Drawing.Point(16, 81);
            this.tabsNewKit.Name = "tabsNewKit";
            this.tabsNewKit.SelectedIndex = 0;
            this.tabsNewKit.Size = new System.Drawing.Size(756, 469);
            this.tabsNewKit.TabIndex = 2;
            this.tabsNewKit.SelectedIndexChanged += new System.EventHandler(this.tabsNewKit_SelectedIndexChanged);
            // 
            // tabAutosomal
            // 
            this.tabAutosomal.Controls.Add(this.dgvAutosomal);
            this.tabAutosomal.Location = new System.Drawing.Point(4, 22);
            this.tabAutosomal.Name = "tabAutosomal";
            this.tabAutosomal.Padding = new System.Windows.Forms.Padding(3);
            this.tabAutosomal.Size = new System.Drawing.Size(748, 443);
            this.tabAutosomal.TabIndex = 1;
            this.tabAutosomal.Text = "Autosomal";
            this.tabAutosomal.UseVisualStyleBackColor = true;
            // 
            // dataGridViewAutosomal
            // 
            this.dgvAutosomal.AllowDrop = true;
            this.dgvAutosomal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAutosomal.ContextMenuStrip = this.mnuAutosomal;
            this.dgvAutosomal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAutosomal.Location = new System.Drawing.Point(3, 3);
            this.dgvAutosomal.Name = "dataGridViewAutosomal";
            this.dgvAutosomal.Size = new System.Drawing.Size(742, 437);
            this.dgvAutosomal.TabIndex = 0;
            this.dgvAutosomal.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvAutosomal_DataError);
            this.dgvAutosomal.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgvAutosomal_DragDrop);
            this.dgvAutosomal.DragEnter += new System.Windows.Forms.DragEventHandler(this.nekf_DragEnter);
            this.dgvAutosomal.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvAutosomal_MouseDown);
            // 
            // mnuAutosomal
            // 
            this.mnuAutosomal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miDeleteRowAutosomal,
            this.miClearAllAutosomal});
            this.mnuAutosomal.Name = "mnuAutosomal";
            this.mnuAutosomal.Size = new System.Drawing.Size(134, 48);
            // 
            // miDeleteRowAutosomal
            // 
            this.miDeleteRowAutosomal.Name = "miDeleteRowAutosomal";
            this.miDeleteRowAutosomal.Size = new System.Drawing.Size(133, 22);
            this.miDeleteRowAutosomal.Text = "Delete Row";
            this.miDeleteRowAutosomal.Click += new System.EventHandler(this.miDeleteRowAutosomal_Click);
            // 
            // miClearAllAutosomal
            // 
            this.miClearAllAutosomal.Name = "miClearAllAutosomal";
            this.miClearAllAutosomal.Size = new System.Drawing.Size(133, 22);
            this.miClearAllAutosomal.Text = "Clear All";
            this.miClearAllAutosomal.Click += new System.EventHandler(this.miClearAllAutosomal_Click);
            // 
            // tabYDNA
            // 
            this.tabYDNA.Controls.Add(this.tabsY);
            this.tabYDNA.Location = new System.Drawing.Point(4, 22);
            this.tabYDNA.Name = "tabYDNA";
            this.tabYDNA.Padding = new System.Windows.Forms.Padding(3);
            this.tabYDNA.Size = new System.Drawing.Size(748, 443);
            this.tabYDNA.TabIndex = 2;
            this.tabYDNA.Text = "Y DNA";
            this.tabYDNA.UseVisualStyleBackColor = true;
            // 
            // tabsY
            // 
            this.tabsY.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tabsY.Controls.Add(this.tabYSNPs);
            this.tabsY.Controls.Add(this.tabYSTR);
            this.tabsY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabsY.Location = new System.Drawing.Point(3, 3);
            this.tabsY.Multiline = true;
            this.tabsY.Name = "tabsY";
            this.tabsY.SelectedIndex = 0;
            this.tabsY.Size = new System.Drawing.Size(742, 437);
            this.tabsY.TabIndex = 0;
            this.tabsY.SelectedIndexChanged += new System.EventHandler(this.tabsY_SelectedIndexChanged);
            // 
            // tabYSNPs
            // 
            this.tabYSNPs.Controls.Add(this.txtYDNA);
            this.tabYSNPs.Location = new System.Drawing.Point(23, 4);
            this.tabYSNPs.Name = "tabYSNPs";
            this.tabYSNPs.Padding = new System.Windows.Forms.Padding(3);
            this.tabYSNPs.Size = new System.Drawing.Size(715, 429);
            this.tabYSNPs.TabIndex = 1;
            this.tabYSNPs.Text = "Y-SNPs";
            this.tabYSNPs.UseVisualStyleBackColor = true;
            // 
            // txtYDNA
            // 
            this.txtYDNA.AllowDrop = true;
            this.txtYDNA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtYDNA.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtYDNA.Location = new System.Drawing.Point(3, 3);
            this.txtYDNA.Multiline = true;
            this.txtYDNA.Name = "txtYDNA";
            this.txtYDNA.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtYDNA.Size = new System.Drawing.Size(709, 423);
            this.txtYDNA.TabIndex = 0;
            this.txtYDNA.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtYDNA_DragDrop);
            this.txtYDNA.DragEnter += new System.Windows.Forms.DragEventHandler(this.nekf_DragEnter);
            // 
            // tabYSTR
            // 
            this.tabYSTR.Controls.Add(this.tableLayoutPanel1);
            this.tabYSTR.Location = new System.Drawing.Point(23, 4);
            this.tabYSTR.Name = "tabYSTR";
            this.tabYSTR.Padding = new System.Windows.Forms.Padding(3);
            this.tabYSTR.Size = new System.Drawing.Size(715, 429);
            this.tabYSTR.TabIndex = 0;
            this.tabYSTR.Text = "Y-STR";
            this.tabYSTR.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tableLayoutPanel1.Controls.Add(this.dgvYMisc, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.dgvY111, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.dgvY67, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.dgvY37, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.dgvY25, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblY37, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblY25, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblY67, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblY111, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblYMisc, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.dgvY12, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(709, 423);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // dgvYMisc
            // 
            this.dgvYMisc.ContextMenuStrip = this.mnuYDNAMisc;
            this.dgvYMisc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvYMisc.Location = new System.Drawing.Point(469, 244);
            this.dgvYMisc.Name = "dgvYMisc";
            this.dgvYMisc.Size = new System.Drawing.Size(237, 176);
            this.dgvYMisc.TabIndex = 11;
            this.dgvYMisc.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvY_DataError);
            this.dgvYMisc.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvYMisc_MouseDown);
            // 
            // mnuYDNAMisc
            // 
            this.mnuYDNAMisc.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miDeleteRowYDNAMisc,
            this.miClearAllYMisc});
            this.mnuYDNAMisc.Name = "mnuYDNAMisc";
            this.mnuYDNAMisc.Size = new System.Drawing.Size(134, 48);
            // 
            // miDeleteRowYDNAMisc
            // 
            this.miDeleteRowYDNAMisc.Name = "miDeleteRowYDNAMisc";
            this.miDeleteRowYDNAMisc.Size = new System.Drawing.Size(133, 22);
            this.miDeleteRowYDNAMisc.Text = "Delete Row";
            this.miDeleteRowYDNAMisc.Click += new System.EventHandler(this.miDeleteRowYDNAMisc_Click);
            // 
            // miClearAllYMisc
            // 
            this.miClearAllYMisc.Name = "miClearAllYMisc";
            this.miClearAllYMisc.Size = new System.Drawing.Size(133, 22);
            this.miClearAllYMisc.Text = "Clear All";
            this.miClearAllYMisc.Click += new System.EventHandler(this.miClearAllYMisc_Click);
            // 
            // dgvY111
            // 
            this.dgvY111.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvY111.Location = new System.Drawing.Point(236, 244);
            this.dgvY111.Name = "dgvY111";
            this.dgvY111.Size = new System.Drawing.Size(227, 176);
            this.dgvY111.TabIndex = 10;
            this.dgvY111.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvY_DataError);
            // 
            // dgvY67
            // 
            this.dgvY67.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvY67.Location = new System.Drawing.Point(3, 244);
            this.dgvY67.Name = "dgvY67";
            this.dgvY67.Size = new System.Drawing.Size(227, 176);
            this.dgvY67.TabIndex = 9;
            this.dgvY67.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvY_DataError);
            // 
            // dgvY37
            // 
            this.dgvY37.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvY37.Location = new System.Drawing.Point(469, 33);
            this.dgvY37.Name = "dgvY37";
            this.dgvY37.Size = new System.Drawing.Size(237, 175);
            this.dgvY37.TabIndex = 8;
            this.dgvY37.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvY_DataError);
            // 
            // dgvY25
            // 
            this.dgvY25.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvY25.Location = new System.Drawing.Point(236, 33);
            this.dgvY25.Name = "dgvY25";
            this.dgvY25.Size = new System.Drawing.Size(227, 175);
            this.dgvY25.TabIndex = 7;
            this.dgvY25.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvY_DataError);
            // 
            // lblY37
            // 
            this.lblY37.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblY37.AutoSize = true;
            this.lblY37.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblY37.Location = new System.Drawing.Point(556, 8);
            this.lblY37.Name = "lblY37";
            this.lblY37.Size = new System.Drawing.Size(63, 13);
            this.lblY37.TabIndex = 2;
            this.lblY37.Text = "Y-DNA 37";
            // 
            // lblY25
            // 
            this.lblY25.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblY25.AutoSize = true;
            this.lblY25.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblY25.Location = new System.Drawing.Point(318, 8);
            this.lblY25.Name = "lblY25";
            this.lblY25.Size = new System.Drawing.Size(63, 13);
            this.lblY25.TabIndex = 1;
            this.lblY25.Text = "Y-DNA 25";
            // 
            // lblY67
            // 
            this.lblY67.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblY67.AutoSize = true;
            this.lblY67.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblY67.Location = new System.Drawing.Point(85, 219);
            this.lblY67.Name = "lblY67";
            this.lblY67.Size = new System.Drawing.Size(63, 13);
            this.lblY67.TabIndex = 3;
            this.lblY67.Text = "Y-DNA 67";
            // 
            // lblY111
            // 
            this.lblY111.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblY111.AutoSize = true;
            this.lblY111.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblY111.Location = new System.Drawing.Point(314, 219);
            this.lblY111.Name = "lblY111";
            this.lblY111.Size = new System.Drawing.Size(70, 13);
            this.lblY111.TabIndex = 4;
            this.lblY111.Text = "Y-DNA 111";
            // 
            // lblYMisc
            // 
            this.lblYMisc.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblYMisc.AutoSize = true;
            this.lblYMisc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYMisc.Location = new System.Drawing.Point(550, 219);
            this.lblYMisc.Name = "lblYMisc";
            this.lblYMisc.Size = new System.Drawing.Size(75, 13);
            this.lblYMisc.TabIndex = 5;
            this.lblYMisc.Text = "Y-DNA Misc";
            // 
            // dgvY12
            // 
            this.dgvY12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvY12.Location = new System.Drawing.Point(3, 33);
            this.dgvY12.Name = "dgvY12";
            this.dgvY12.Size = new System.Drawing.Size(227, 175);
            this.dgvY12.TabIndex = 6;
            this.dgvY12.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvY_DataError);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.Controls.Add(this.btnPasteY, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnClearY, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblY12, 2, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(233, 30);
            this.tableLayoutPanel2.TabIndex = 12;
            // 
            // btnPasteY
            // 
            this.btnPasteY.FlatAppearance.BorderSize = 0;
            this.btnPasteY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPasteY.Image = global::GKGenetix.UI.Properties.Resources.paste;
            this.btnPasteY.Location = new System.Drawing.Point(0, 0);
            this.btnPasteY.Margin = new System.Windows.Forms.Padding(0);
            this.btnPasteY.Name = "btnPasteY";
            this.btnPasteY.Size = new System.Drawing.Size(30, 30);
            this.btnPasteY.TabIndex = 2;
            this.btnPasteY.UseVisualStyleBackColor = true;
            this.btnPasteY.Click += new System.EventHandler(this.btnPasteY_Click);
            // 
            // btnClearY
            // 
            this.btnClearY.FlatAppearance.BorderSize = 0;
            this.btnClearY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearY.Image = global::GKGenetix.UI.Properties.Resources.eraser;
            this.btnClearY.Location = new System.Drawing.Point(30, 0);
            this.btnClearY.Margin = new System.Windows.Forms.Padding(0);
            this.btnClearY.Name = "btnClearY";
            this.btnClearY.Size = new System.Drawing.Size(30, 30);
            this.btnClearY.TabIndex = 3;
            this.btnClearY.UseVisualStyleBackColor = true;
            this.btnClearY.Click += new System.EventHandler(this.btnClearY_Click);
            // 
            // lblY12
            // 
            this.lblY12.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblY12.AutoSize = true;
            this.lblY12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblY12.Location = new System.Drawing.Point(85, 8);
            this.lblY12.Name = "lblY12";
            this.lblY12.Size = new System.Drawing.Size(63, 13);
            this.lblY12.TabIndex = 1;
            this.lblY12.Text = "Y-DNA 12";
            // 
            // tabMtDNA
            // 
            this.tabMtDNA.Controls.Add(this.tabsMt);
            this.tabMtDNA.Location = new System.Drawing.Point(4, 22);
            this.tabMtDNA.Name = "tabMtDNA";
            this.tabMtDNA.Size = new System.Drawing.Size(748, 443);
            this.tabMtDNA.TabIndex = 3;
            this.tabMtDNA.Text = "MT DNA";
            this.tabMtDNA.UseVisualStyleBackColor = true;
            // 
            // tabsMt
            // 
            this.tabsMt.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tabsMt.Controls.Add(this.tabMutations);
            this.tabsMt.Controls.Add(this.tabFasta);
            this.tabsMt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabsMt.Location = new System.Drawing.Point(0, 0);
            this.tabsMt.Multiline = true;
            this.tabsMt.Name = "tabsMt";
            this.tabsMt.SelectedIndex = 0;
            this.tabsMt.Size = new System.Drawing.Size(748, 443);
            this.tabsMt.TabIndex = 2;
            // 
            // tabMutations
            // 
            this.tabMutations.Controls.Add(this.txtMtDNA);
            this.tabMutations.Location = new System.Drawing.Point(23, 4);
            this.tabMutations.Name = "tabMutations";
            this.tabMutations.Padding = new System.Windows.Forms.Padding(3);
            this.tabMutations.Size = new System.Drawing.Size(721, 435);
            this.tabMutations.TabIndex = 0;
            this.tabMutations.Text = "Mutations";
            this.tabMutations.UseVisualStyleBackColor = true;
            // 
            // textBoxMtDNA
            // 
            this.txtMtDNA.AllowDrop = true;
            this.txtMtDNA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMtDNA.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMtDNA.Location = new System.Drawing.Point(3, 3);
            this.txtMtDNA.Multiline = true;
            this.txtMtDNA.Name = "textBoxMtDNA";
            this.txtMtDNA.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMtDNA.Size = new System.Drawing.Size(715, 429);
            this.txtMtDNA.TabIndex = 1;
            this.txtMtDNA.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtMtDNA_DragDrop);
            this.txtMtDNA.DragEnter += new System.Windows.Forms.DragEventHandler(this.nekf_DragEnter);
            // 
            // tabFasta
            // 
            this.tabFasta.Controls.Add(this.txtFASTA);
            this.tabFasta.Location = new System.Drawing.Point(23, 4);
            this.tabFasta.Name = "tabFasta";
            this.tabFasta.Padding = new System.Windows.Forms.Padding(3);
            this.tabFasta.Size = new System.Drawing.Size(721, 435);
            this.tabFasta.TabIndex = 1;
            this.tabFasta.Text = "FASTA";
            this.tabFasta.UseVisualStyleBackColor = true;
            // 
            // tbFASTA
            // 
            this.txtFASTA.AllowDrop = true;
            this.txtFASTA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFASTA.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFASTA.Location = new System.Drawing.Point(3, 3);
            this.txtFASTA.Multiline = true;
            this.txtFASTA.Name = "tbFASTA";
            this.txtFASTA.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtFASTA.Size = new System.Drawing.Size(715, 429);
            this.txtFASTA.TabIndex = 2;
            this.txtFASTA.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtFASTA_DragDrop);
            this.txtFASTA.DragEnter += new System.Windows.Forms.DragEventHandler(this.nekf_DragEnter);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(54, 10);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(258, 20);
            this.txtName.TabIndex = 3;
            // 
            // txtKit
            // 
            this.txtKit.Location = new System.Drawing.Point(54, 42);
            this.txtKit.Name = "txtKit";
            this.txtKit.Size = new System.Drawing.Size(135, 20);
            this.txtKit.TabIndex = 4;
            // 
            // bwNewKitAutosomalJob
            // 
            this.bwNewKitAutosomalJob.WorkerReportsProgress = true;
            this.bwNewKitAutosomalJob.WorkerSupportsCancellation = true;
            this.bwNewKitAutosomalJob.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwNewKitAutosomalJob_DoWork);
            this.bwNewKitAutosomalJob.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwNewKitAutosomalJob_ProgressChanged);
            this.bwNewKitAutosomalJob.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwNewKitAutosomalJob_RunWorkerCompleted);
            // 
            // tipLbl
            // 
            this.tipLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tipLbl.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tipLbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tipLbl.Location = new System.Drawing.Point(528, 9);
            this.tipLbl.Name = "tipLbl";
            this.tipLbl.Size = new System.Drawing.Size(244, 49);
            this.tipLbl.TabIndex = 5;
            this.tipLbl.Text = "Tip: Drag and drop any autosomal raw file into the grid below. You can select mul" +
    "tiple files. e.g, Autosomal and X.";
            // 
            // bwSave
            // 
            this.bwSave.WorkerReportsProgress = true;
            this.bwSave.WorkerSupportsCancellation = true;
            this.bwSave.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwSave_DoWork);
            this.bwSave.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwSave_ProgressChanged);
            this.bwSave.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwSave_RunWorkerCompleted);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // bwPopulate
            // 
            this.bwPopulate.WorkerSupportsCancellation = true;
            this.bwPopulate.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwPopulate_DoWork);
            this.bwPopulate.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwPopulate_RunWorkerCompleted);
            // 
            // lblSex
            // 
            this.lblSex.AutoSize = true;
            this.lblSex.Location = new System.Drawing.Point(213, 45);
            this.lblSex.Name = "lblSex";
            this.lblSex.Size = new System.Drawing.Size(25, 13);
            this.lblSex.TabIndex = 6;
            this.lblSex.Text = "Sex";
            // 
            // cbSex
            // 
            this.cbSex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSex.FormattingEnabled = true;
            this.cbSex.Items.AddRange(new object[] {
            "Unknown",
            "Male",
            "Female"});
            this.cbSex.Location = new System.Drawing.Point(244, 41);
            this.cbSex.Name = "cbSex";
            this.cbSex.Size = new System.Drawing.Size(121, 21);
            this.cbSex.TabIndex = 7;
            // 
            // NewEditKitFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.cbSex);
            this.Controls.Add(this.lblSex);
            this.Controls.Add(this.tipLbl);
            this.Controls.Add(this.txtKit);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.tabsNewKit);
            this.Controls.Add(this.lblKitNo);
            this.Controls.Add(this.lblName);
            this.Name = "NewEditKitFrm";
            this.Text = "New/Edit Kit";
            this.Closing += new System.EventHandler(this.NewKitFrm_FormClosing);
            this.Load += new System.EventHandler(this.NewKitFrm_Load);
            this.tabsNewKit.ResumeLayout(false);
            this.tabAutosomal.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAutosomal)).EndInit();
            this.mnuAutosomal.ResumeLayout(false);
            this.tabYDNA.ResumeLayout(false);
            this.tabsY.ResumeLayout(false);
            this.tabYSNPs.ResumeLayout(false);
            this.tabYSNPs.PerformLayout();
            this.tabYSTR.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvYMisc)).EndInit();
            this.mnuYDNAMisc.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvY111)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvY67)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvY37)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvY25)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvY12)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tabMtDNA.ResumeLayout(false);
            this.tabsMt.ResumeLayout(false);
            this.tabMutations.ResumeLayout(false);
            this.tabMutations.PerformLayout();
            this.tabFasta.ResumeLayout(false);
            this.tabFasta.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

namespace Genetic_Genealogy_Kit
{
    partial class MatchingKitsFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MatchingKitsFrm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblKit = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvMatches = new System.Windows.Forms.DataGridView();
            this.cmp_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.kit_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.at_longest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.at_total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.x_longest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.x_total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mrca = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvSegments = new System.Windows.Forms.DataGridView();
            this.chr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.start_pos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.end_pos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seg_len = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.snp_count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvAlleles = new System.Windows.Forms.DataGridView();
            this.rsid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.position = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.kit1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.kit2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.match = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.lblSegLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMatches)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSegments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlleles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dgvMatches, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(784, 281);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 80);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(3);
            this.label3.Size = new System.Drawing.Size(778, 30);
            this.label3.TabIndex = 3;
            this.label3.Text = "List of matching kits:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblKit);
            this.panel1.Controls.Add(this.lblName);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(778, 74);
            this.panel1.TabIndex = 0;
            // 
            // lblKit
            // 
            this.lblKit.AutoSize = true;
            this.lblKit.Location = new System.Drawing.Point(55, 47);
            this.lblKit.Name = "lblKit";
            this.lblKit.Size = new System.Drawing.Size(75, 26);
            this.lblKit.TabIndex = 4;
            this.lblKit.Text = "..";
            this.lblKit.UseVisualStyleBackColor = true;
            this.lblKit.Click += new System.EventHandler(this.lblKit_Click);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(54, 17);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(13, 13);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "..";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Kit No:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // dgvMatches
            // 
            this.dgvMatches.AllowUserToAddRows = false;
            this.dgvMatches.AllowUserToDeleteRows = false;
            this.dgvMatches.AllowUserToResizeRows = false;
            this.dgvMatches.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvMatches.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMatches.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cmp_id,
            this.kit_no,
            this.name,
            this.at_longest,
            this.at_total,
            this.x_longest,
            this.x_total,
            this.mrca});
            this.dgvMatches.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMatches.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvMatches.Location = new System.Drawing.Point(3, 113);
            this.dgvMatches.MultiSelect = false;
            this.dgvMatches.Name = "dgvMatches";
            this.dgvMatches.ReadOnly = true;
            this.dgvMatches.RowHeadersVisible = false;
            this.dgvMatches.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvMatches.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMatches.ShowEditingIcon = false;
            this.dgvMatches.Size = new System.Drawing.Size(778, 165);
            this.dgvMatches.TabIndex = 1;
            this.dgvMatches.SelectionChanged += new System.EventHandler(this.dgvMatches_SelectionChanged);
            // 
            // cmp_id
            // 
            this.cmp_id.HeaderText = "ID";
            this.cmp_id.Name = "cmp_id";
            this.cmp_id.ReadOnly = true;
            this.cmp_id.Visible = false;
            // 
            // kit_no
            // 
            this.kit_no.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.kit_no.HeaderText = "Kit No";
            this.kit_no.Name = "kit_no";
            this.kit_no.ReadOnly = true;
            // 
            // name
            // 
            this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.name.HeaderText = "Name";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // at_longest
            // 
            this.at_longest.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.at_longest.HeaderText = "Autosomal Longest";
            this.at_longest.Name = "at_longest";
            this.at_longest.ReadOnly = true;
            // 
            // at_total
            // 
            this.at_total.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.at_total.HeaderText = "Autosomal Total";
            this.at_total.Name = "at_total";
            this.at_total.ReadOnly = true;
            // 
            // x_longest
            // 
            this.x_longest.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.x_longest.HeaderText = "X Longest";
            this.x_longest.Name = "x_longest";
            this.x_longest.ReadOnly = true;
            // 
            // x_total
            // 
            this.x_total.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.x_total.HeaderText = "X Total";
            this.x_total.Name = "x_total";
            this.x_total.ReadOnly = true;
            // 
            // mrca
            // 
            this.mrca.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.mrca.HeaderText = "MRCA";
            this.mrca.Name = "mrca";
            this.mrca.ReadOnly = true;
            // 
            // dgvSegments
            // 
            this.dgvSegments.AllowUserToAddRows = false;
            this.dgvSegments.AllowUserToDeleteRows = false;
            this.dgvSegments.AllowUserToResizeColumns = false;
            this.dgvSegments.AllowUserToResizeRows = false;
            this.dgvSegments.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvSegments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSegments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chr,
            this.start_pos,
            this.end_pos,
            this.seg_len,
            this.snp_count});
            this.dgvSegments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSegments.Location = new System.Drawing.Point(3, 33);
            this.dgvSegments.MultiSelect = false;
            this.dgvSegments.Name = "dgvSegments";
            this.dgvSegments.ReadOnly = true;
            this.dgvSegments.RowHeadersVisible = false;
            this.dgvSegments.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvSegments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSegments.Size = new System.Drawing.Size(386, 241);
            this.dgvSegments.TabIndex = 6;
            this.dgvSegments.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSegments_CellDoubleClick);
            this.dgvSegments.SelectionChanged += new System.EventHandler(this.dgvSegments_SelectionChanged);
            // 
            // chr
            // 
            this.chr.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.chr.HeaderText = "Chromosome";
            this.chr.Name = "chr";
            this.chr.ReadOnly = true;
            // 
            // start_pos
            // 
            this.start_pos.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.start_pos.HeaderText = "Start Position";
            this.start_pos.Name = "start_pos";
            this.start_pos.ReadOnly = true;
            // 
            // end_pos
            // 
            this.end_pos.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.end_pos.HeaderText = "End Position";
            this.end_pos.Name = "end_pos";
            this.end_pos.ReadOnly = true;
            // 
            // seg_len
            // 
            this.seg_len.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.seg_len.HeaderText = "Segment Length (cM)";
            this.seg_len.Name = "seg_len";
            this.seg_len.ReadOnly = true;
            // 
            // snp_count
            // 
            this.snp_count.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.snp_count.HeaderText = "SNP Count";
            this.snp_count.Name = "snp_count";
            this.snp_count.ReadOnly = true;
            // 
            // dgvAlleles
            // 
            this.dgvAlleles.AllowUserToAddRows = false;
            this.dgvAlleles.AllowUserToDeleteRows = false;
            this.dgvAlleles.AllowUserToResizeColumns = false;
            this.dgvAlleles.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvAlleles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAlleles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.rsid,
            this.position,
            this.kit1,
            this.kit2,
            this.match});
            this.dgvAlleles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAlleles.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvAlleles.Location = new System.Drawing.Point(395, 33);
            this.dgvAlleles.MultiSelect = false;
            this.dgvAlleles.Name = "dgvAlleles";
            this.dgvAlleles.ReadOnly = true;
            this.dgvAlleles.RowHeadersVisible = false;
            this.dgvAlleles.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvAlleles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAlleles.ShowEditingIcon = false;
            this.dgvAlleles.Size = new System.Drawing.Size(386, 241);
            this.dgvAlleles.TabIndex = 7;
            // 
            // rsid
            // 
            this.rsid.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.rsid.HeaderText = "RSID";
            this.rsid.Name = "rsid";
            this.rsid.ReadOnly = true;
            // 
            // position
            // 
            this.position.HeaderText = "Position";
            this.position.Name = "position";
            this.position.ReadOnly = true;
            // 
            // kit1
            // 
            this.kit1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.kit1.HeaderText = "Kit1";
            this.kit1.Name = "kit1";
            this.kit1.ReadOnly = true;
            // 
            // kit2
            // 
            this.kit2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.kit2.HeaderText = "Kit2";
            this.kit2.Name = "kit2";
            this.kit2.ReadOnly = true;
            // 
            // match
            // 
            this.match.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.match.HeaderText = "Match";
            this.match.Name = "match";
            this.match.ReadOnly = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel3);
            this.splitContainer1.Size = new System.Drawing.Size(784, 562);
            this.splitContainer1.SplitterDistance = 281;
            this.splitContainer1.TabIndex = 1;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.label4, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblSegLabel, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.dgvAlleles, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.dgvSegments, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(784, 277);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(395, 0);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(3);
            this.label4.Size = new System.Drawing.Size(386, 30);
            this.label4.TabIndex = 9;
            this.label4.Text = "Segment Matching Details:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSegLabel
            // 
            this.lblSegLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSegLabel.Location = new System.Drawing.Point(3, 0);
            this.lblSegLabel.Name = "lblSegLabel";
            this.lblSegLabel.Padding = new System.Windows.Forms.Padding(3);
            this.lblSegLabel.Size = new System.Drawing.Size(386, 30);
            this.lblSegLabel.TabIndex = 8;
            this.lblSegLabel.Text = "List of segments matching with ...";
            this.lblSegLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MatchingKitsFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MatchingKitsFrm";
            this.Text = "Matching Kits";
            this.Load += new System.EventHandler(this.MatchingKitsFrm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMatches)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSegments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlleles)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.DataGridView dgvMatches;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmp_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn kit_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn at_longest;
        private System.Windows.Forms.DataGridViewTextBoxColumn at_total;
        private System.Windows.Forms.DataGridViewTextBoxColumn x_longest;
        private System.Windows.Forms.DataGridViewTextBoxColumn x_total;
        private System.Windows.Forms.DataGridViewTextBoxColumn mrca;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgvSegments;
        private System.Windows.Forms.DataGridViewTextBoxColumn chr;
        private System.Windows.Forms.DataGridViewTextBoxColumn start_pos;
        private System.Windows.Forms.DataGridViewTextBoxColumn end_pos;
        private System.Windows.Forms.DataGridViewTextBoxColumn seg_len;
        private System.Windows.Forms.DataGridViewTextBoxColumn snp_count;
        private System.Windows.Forms.DataGridView dgvAlleles;
        private System.Windows.Forms.DataGridViewTextBoxColumn rsid;
        private System.Windows.Forms.DataGridViewTextBoxColumn position;
        private System.Windows.Forms.DataGridViewTextBoxColumn kit1;
        private System.Windows.Forms.DataGridViewTextBoxColumn kit2;
        private System.Windows.Forms.DataGridViewTextBoxColumn match;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label lblSegLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button lblKit;
    }
}
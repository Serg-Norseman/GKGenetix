namespace Genetic_Genealogy_Kit
{
    partial class OneToOneCmpFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OneToOneCmpFrm));
            this.bwCompare = new System.ComponentModel.BackgroundWorker();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvSegmentIdx = new System.Windows.Forms.DataGridView();
            this.chromosome = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.start = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.end_pos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seg_len = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.snp_count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvMatching = new System.Windows.Forms.DataGridView();
            this.rsid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.kit_1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.kit_2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.match = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblLongestXSegment = new System.Windows.Forms.Label();
            this.lblTotalXSegments = new System.Windows.Forms.Label();
            this.lblMRCA = new System.Windows.Forms.Label();
            this.lblLongestSegment = new System.Windows.Forms.Label();
            this.lblTotalSegments = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.saveFileDlg = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSegmentIdx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMatching)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bwCompare
            // 
            this.bwCompare.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwCompare_DoWork);
            this.bwCompare.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwCompare_RunWorkerCompleted);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 103);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvSegmentIdx);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvMatching);
            this.splitContainer1.Size = new System.Drawing.Size(778, 456);
            this.splitContainer1.SplitterDistance = 387;
            this.splitContainer1.TabIndex = 0;
            // 
            // dgvSegmentIdx
            // 
            this.dgvSegmentIdx.AllowUserToAddRows = false;
            this.dgvSegmentIdx.AllowUserToDeleteRows = false;
            this.dgvSegmentIdx.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvSegmentIdx.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSegmentIdx.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chromosome,
            this.start,
            this.end_pos,
            this.seg_len,
            this.snp_count});
            this.dgvSegmentIdx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSegmentIdx.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvSegmentIdx.Location = new System.Drawing.Point(0, 0);
            this.dgvSegmentIdx.MultiSelect = false;
            this.dgvSegmentIdx.Name = "dgvSegmentIdx";
            this.dgvSegmentIdx.ReadOnly = true;
            this.dgvSegmentIdx.RowHeadersVisible = false;
            this.dgvSegmentIdx.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSegmentIdx.ShowEditingIcon = false;
            this.dgvSegmentIdx.Size = new System.Drawing.Size(387, 456);
            this.dgvSegmentIdx.TabIndex = 0;
            this.dgvSegmentIdx.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSegmentIdx_CellDoubleClick);
            this.dgvSegmentIdx.SelectionChanged += new System.EventHandler(this.dgvSegmentIdx_SelectionChanged);
            // 
            // chromosome
            // 
            this.chromosome.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.chromosome.HeaderText = "Chromosome";
            this.chromosome.Name = "chromosome";
            this.chromosome.ReadOnly = true;
            // 
            // start
            // 
            this.start.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.start.HeaderText = "Start Position";
            this.start.Name = "start";
            this.start.ReadOnly = true;
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
            // dgvMatching
            // 
            this.dgvMatching.AllowUserToAddRows = false;
            this.dgvMatching.AllowUserToDeleteRows = false;
            this.dgvMatching.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvMatching.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMatching.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.rsid,
            this.chr,
            this.pos,
            this.kit_1,
            this.kit_2,
            this.match});
            this.dgvMatching.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMatching.Location = new System.Drawing.Point(0, 0);
            this.dgvMatching.Name = "dgvMatching";
            this.dgvMatching.ReadOnly = true;
            this.dgvMatching.RowHeadersVisible = false;
            this.dgvMatching.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMatching.ShowEditingIcon = false;
            this.dgvMatching.Size = new System.Drawing.Size(387, 456);
            this.dgvMatching.TabIndex = 0;
            this.dgvMatching.Sorted += new System.EventHandler(this.dgvMatching_Sorted);
            // 
            // rsid
            // 
            this.rsid.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.rsid.HeaderText = "RSID";
            this.rsid.Name = "rsid";
            this.rsid.ReadOnly = true;
            // 
            // chr
            // 
            this.chr.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.chr.HeaderText = "Chromosome";
            this.chr.Name = "chr";
            this.chr.ReadOnly = true;
            // 
            // pos
            // 
            this.pos.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.pos.HeaderText = "Position";
            this.pos.Name = "pos";
            this.pos.ReadOnly = true;
            // 
            // kit_1
            // 
            this.kit_1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.kit_1.HeaderText = "Kit #1";
            this.kit_1.Name = "kit_1";
            this.kit_1.ReadOnly = true;
            // 
            // kit_2
            // 
            this.kit_2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.kit_2.HeaderText = "Kit #2";
            this.kit_2.Name = "kit_2";
            this.kit_2.ReadOnly = true;
            // 
            // match
            // 
            this.match.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.match.HeaderText = "Match";
            this.match.Name = "match";
            this.match.ReadOnly = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.splitContainer1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(784, 562);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblLongestXSegment);
            this.panel1.Controls.Add(this.lblTotalXSegments);
            this.panel1.Controls.Add(this.lblMRCA);
            this.panel1.Controls.Add(this.lblLongestSegment);
            this.panel1.Controls.Add(this.lblTotalSegments);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(778, 94);
            this.panel1.TabIndex = 1;
            // 
            // lblLongestXSegment
            // 
            this.lblLongestXSegment.AutoSize = true;
            this.lblLongestXSegment.Location = new System.Drawing.Point(455, 41);
            this.lblLongestXSegment.Name = "lblLongestXSegment";
            this.lblLongestXSegment.Size = new System.Drawing.Size(13, 13);
            this.lblLongestXSegment.TabIndex = 9;
            this.lblLongestXSegment.Text = "..";
            // 
            // lblTotalXSegments
            // 
            this.lblTotalXSegments.AutoSize = true;
            this.lblTotalXSegments.Location = new System.Drawing.Point(455, 16);
            this.lblTotalXSegments.Name = "lblTotalXSegments";
            this.lblTotalXSegments.Size = new System.Drawing.Size(13, 13);
            this.lblTotalXSegments.TabIndex = 8;
            this.lblTotalXSegments.Text = "..";
            // 
            // lblMRCA
            // 
            this.lblMRCA.AutoSize = true;
            this.lblMRCA.Location = new System.Drawing.Point(160, 68);
            this.lblMRCA.Name = "lblMRCA";
            this.lblMRCA.Size = new System.Drawing.Size(13, 13);
            this.lblMRCA.TabIndex = 7;
            this.lblMRCA.Text = "..";
            // 
            // lblLongestSegment
            // 
            this.lblLongestSegment.AutoSize = true;
            this.lblLongestSegment.Location = new System.Drawing.Point(160, 41);
            this.lblLongestSegment.Name = "lblLongestSegment";
            this.lblLongestSegment.Size = new System.Drawing.Size(13, 13);
            this.lblLongestSegment.TabIndex = 6;
            this.lblLongestSegment.Text = "..";
            // 
            // lblTotalSegments
            // 
            this.lblTotalSegments.AutoSize = true;
            this.lblTotalSegments.Location = new System.Drawing.Point(160, 16);
            this.lblTotalSegments.Name = "lblTotalSegments";
            this.lblTotalSegments.Size = new System.Drawing.Size(13, 13);
            this.lblTotalSegments.TabIndex = 5;
            this.lblTotalSegments.Text = "..";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(343, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Longest X Segment:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(343, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Total X Segments:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Estimated MRCA:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(145, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Longest Autosomal Segment:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Total Autosomal Segments:";
            // 
            // saveFileDlg
            // 
            this.saveFileDlg.Filter = "CSV Files|*.csv";
            this.saveFileDlg.Title = "Save Common Ancestor Profile";
            // 
            // OneToOneCmpFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OneToOneCmpFrm";
            this.ShowInTaskbar = false;
            this.Text = "One-To-One Comparision";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OneToOneCmpFrm_FormClosing);
            this.Load += new System.EventHandler(this.OneToOneCmpFrm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSegmentIdx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMatching)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker bwCompare;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgvSegmentIdx;
        private System.Windows.Forms.DataGridView dgvMatching;
        private System.Windows.Forms.DataGridViewTextBoxColumn chromosome;
        private System.Windows.Forms.DataGridViewTextBoxColumn start;
        private System.Windows.Forms.DataGridViewTextBoxColumn end_pos;
        private System.Windows.Forms.DataGridViewTextBoxColumn seg_len;
        private System.Windows.Forms.DataGridViewTextBoxColumn snp_count;
        private System.Windows.Forms.DataGridViewTextBoxColumn rsid;
        private System.Windows.Forms.DataGridViewTextBoxColumn chr;
        private System.Windows.Forms.DataGridViewTextBoxColumn pos;
        private System.Windows.Forms.DataGridViewTextBoxColumn kit_1;
        private System.Windows.Forms.DataGridViewTextBoxColumn kit_2;
        private System.Windows.Forms.DataGridViewTextBoxColumn match;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblLongestXSegment;
        private System.Windows.Forms.Label lblTotalXSegments;
        private System.Windows.Forms.Label lblMRCA;
        private System.Windows.Forms.Label lblLongestSegment;
        private System.Windows.Forms.Label lblTotalSegments;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.SaveFileDialog saveFileDlg;
    }
}
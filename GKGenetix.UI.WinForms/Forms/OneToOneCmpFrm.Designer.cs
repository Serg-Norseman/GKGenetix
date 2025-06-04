namespace GKGenetix.UI.Forms
{
    partial class OneToOneCmpFrm
    {
        private System.Windows.Forms.DataGridView dgvSegmentIdx;
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

        private void InitializeComponent()
        {
            this.dgvSegmentIdx = new System.Windows.Forms.DataGridView();
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
            ((System.ComponentModel.ISupportInitialize)(this.dgvSegmentIdx)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvSegmentIdx
            // 
            this.dgvSegmentIdx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSegmentIdx.Location = new System.Drawing.Point(0, 0);
            this.dgvSegmentIdx.Name = "dgvSegmentIdx";
            this.dgvSegmentIdx.Size = new System.Drawing.Size(387, 456);
            this.dgvSegmentIdx.TabIndex = 0;
            this.dgvSegmentIdx.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSegmentIdx_CellDoubleClick);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dgvSegmentIdx, 0, 1);
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
            // OneToOneCmpFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "OneToOneCmpFrm";
            this.Text = "One-To-One Comparison";
            ((System.ComponentModel.ISupportInitialize)(this.dgvSegmentIdx)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}

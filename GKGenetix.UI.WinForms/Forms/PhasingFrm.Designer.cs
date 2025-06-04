namespace GKGenetix.UI.Forms
{
    partial class PhasingFrm
    {
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnFather;
        private System.Windows.Forms.Button btnMother;
        private System.Windows.Forms.Button btnChild;
        private System.Windows.Forms.Button btnPhasing;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.DataGridView dgvPhasing;

        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnFather = new System.Windows.Forms.Button();
            this.btnMother = new System.Windows.Forms.Button();
            this.btnChild = new System.Windows.Forms.Button();
            this.btnPhasing = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.dgvPhasing = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhasing)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.Controls.Add(this.btnFather, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnMother, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnChild, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnPhasing, 2, 7);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 9;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(229, 556);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnFather
            // 
            this.btnFather.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnFather.Location = new System.Drawing.Point(14, 156);
            this.btnFather.Name = "btnFather";
            this.btnFather.Size = new System.Drawing.Size(62, 63);
            this.btnFather.TabIndex = 17;
            this.btnFather.Text = "Select Father";
            this.btnFather.UseVisualStyleBackColor = true;
            this.btnFather.Click += new System.EventHandler(this.btnFather_Click);
            // 
            // btnMother
            // 
            this.btnMother.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnMother.Location = new System.Drawing.Point(150, 156);
            this.btnMother.Name = "btnMother";
            this.btnMother.Size = new System.Drawing.Size(62, 63);
            this.btnMother.TabIndex = 18;
            this.btnMother.Text = "Select Mother";
            this.btnMother.UseVisualStyleBackColor = true;
            this.btnMother.Click += new System.EventHandler(this.btnMother_Click);
            // 
            // btnChild
            // 
            this.btnChild.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnChild.Location = new System.Drawing.Point(82, 318);
            this.btnChild.Name = "btnChild";
            this.btnChild.Size = new System.Drawing.Size(62, 63);
            this.btnChild.TabIndex = 19;
            this.btnChild.Text = "Select Child";
            this.btnChild.UseVisualStyleBackColor = true;
            this.btnChild.Click += new System.EventHandler(this.btnChild_Click);
            // 
            // btnPhasing
            // 
            this.btnPhasing.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPhasing.Enabled = false;
            this.btnPhasing.Location = new System.Drawing.Point(150, 456);
            this.btnPhasing.Name = "btnPhasing";
            this.btnPhasing.Size = new System.Drawing.Size(62, 63);
            this.btnPhasing.TabIndex = 20;
            this.btnPhasing.Text = "Begin Phasing";
            this.btnPhasing.UseVisualStyleBackColor = true;
            this.btnPhasing.Click += new System.EventHandler(this.btnPhasing_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.dgvPhasing, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(784, 562);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // dgvPhasing
            // 
            this.dgvPhasing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPhasing.Location = new System.Drawing.Point(238, 3);
            this.dgvPhasing.Name = "dgvPhasing";
            this.dgvPhasing.Size = new System.Drawing.Size(543, 556);
            this.dgvPhasing.TabIndex = 1;
            this.dgvPhasing.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(dgvPhasing_CellFormatting);
            // 
            // PhasingFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Name = "PhasingFrm";
            this.Text = "Phasing Utility";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhasing)).EndInit();
            this.ResumeLayout(false);
        }
    }
}

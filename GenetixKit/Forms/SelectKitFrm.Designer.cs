namespace GenetixKit.Forms
{
    partial class SelectKitFrm
    {
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.DataGridView dgvKits;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label kitLbl;

        private void InitializeComponent()
        {
            this.btnOpen = new System.Windows.Forms.Button();
            this.dgvKits = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.kitLbl = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKits)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(358, 285);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(94, 29);
            this.btnOpen.TabIndex = 1;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // dgvKits
            // 
            this.dgvKits.Location = new System.Drawing.Point(13, 13);
            this.dgvKits.Name = "dgvKits";
            this.dgvKits.Size = new System.Drawing.Size(439, 259);
            this.dgvKits.TabIndex = 2;
            this.dgvKits.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvKits_CellContentDoubleClick);
            this.dgvKits.SelectionChanged += new System.EventHandler(this.dgvKits_SelectionChanged);
            this.dgvKits.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(dgvKits_CellFormatting);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 293);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Selected Kit:";
            // 
            // kitLbl
            // 
            this.kitLbl.AutoSize = true;
            this.kitLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.kitLbl.Location = new System.Drawing.Point(83, 293);
            this.kitLbl.Name = "kitLbl";
            this.kitLbl.Size = new System.Drawing.Size(11, 13);
            this.kitLbl.TabIndex = 4;
            this.kitLbl.Text = ".";
            // 
            // SelectKitFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 326);
            this.Controls.Add(this.kitLbl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvKits);
            this.Controls.Add(this.btnOpen);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectKitFrm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Kit";
            this.Load += new System.EventHandler(this.OpenKitFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKits)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

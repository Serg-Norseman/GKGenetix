namespace GKGenetix.UI.Forms
{
    partial class ImportTestFrm
    {
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.DataGridView dgvTests;

        private void InitializeComponent()
        {
            this.btnOpen = new System.Windows.Forms.Button();
            this.dgvTests = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTests)).BeginInit();
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
            // dgvTests
            // 
            this.dgvTests.Location = new System.Drawing.Point(13, 13);
            this.dgvTests.Name = "dgvTests";
            this.dgvTests.Size = new System.Drawing.Size(439, 259);
            this.dgvTests.TabIndex = 2;
            this.dgvTests.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTests_CellContentDoubleClick);
            // 
            // ImportTestFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 326);
            this.Controls.Add(this.dgvTests);
            this.Controls.Add(this.btnOpen);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportTestFrm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import Test";
            this.Load += new System.EventHandler(this.OpenKitFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTests)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

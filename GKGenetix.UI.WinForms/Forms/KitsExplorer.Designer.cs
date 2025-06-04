namespace GKGenetix.UI.Forms
{
    partial class KitsExplorer
    {
        private System.Windows.Forms.DataGridView dgvEditKit;

        private void InitializeComponent()
        {
            this.dgvEditKit = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEditKit)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvEditKit
            // 
            this.dgvEditKit.AllowUserToAddRows = false;
            this.dgvEditKit.AllowUserToDeleteRows = false;
            this.dgvEditKit.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvEditKit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEditKit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvEditKit.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvEditKit.Location = new System.Drawing.Point(0, 0);
            this.dgvEditKit.Name = "dgvEditKit";
            this.dgvEditKit.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEditKit.Size = new System.Drawing.Size(784, 562);
            this.dgvEditKit.TabIndex = 1;
            this.dgvEditKit.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEditKit_CellContentClick);
            this.dgvEditKit.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(dgvEditKit_CellFormatting);
            this.dgvEditKit.DoubleClick += new System.EventHandler(this.dgvEditKit_DoubleClick);
            // 
            // QuickEditKit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.dgvEditKit);
            this.Name = "QuickEditKit";
            this.Text = "Quick Edit Kit";
            this.Closing += new System.EventHandler(this.QuickEditKit_FormClosing);
            this.Load += new System.EventHandler(this.QuickEditKit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEditKit)).EndInit();
            this.ResumeLayout(false);
        }
    }
}

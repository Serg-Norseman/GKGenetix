namespace GenetixKit.Forms
{
    partial class QuickEditKit
    {
        private System.Windows.Forms.DataGridView dgvEditKit;
        private System.ComponentModel.BackgroundWorker bwDelete;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuickEditKit));
            this.dgvEditKit = new System.Windows.Forms.DataGridView();
            this.bwDelete = new System.ComponentModel.BackgroundWorker();
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
            // 
            // bwDelete
            // 
            this.bwDelete.WorkerReportsProgress = true;
            this.bwDelete.WorkerSupportsCancellation = true;
            this.bwDelete.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwDelete_DoWork);
            this.bwDelete.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwDelete_RunWorkerCompleted);
            // 
            // QuickEditKit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.dgvEditKit);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QuickEditKit";
            this.ShowInTaskbar = false;
            this.Text = "Quick Edit Kit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.QuickEditKit_FormClosing);
            this.Load += new System.EventHandler(this.QuickEditKit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEditKit)).EndInit();
            this.ResumeLayout(false);
        }
    }
}

namespace Genetic_Genealogy_Kit
{
    partial class QuickEditKit
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuickEditKit));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.dgvEditKit = new System.Windows.Forms.DataGridView();
            this.kit_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sex = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.disabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.location = new System.Windows.Forms.DataGridViewButtonColumn();
            this.bwDelete = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEditKit)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // dgvEditKit
            // 
            this.dgvEditKit.AllowUserToAddRows = false;
            this.dgvEditKit.AllowUserToDeleteRows = false;
            this.dgvEditKit.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvEditKit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEditKit.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.kit_no,
            this.name,
            this.sex,
            this.disabled,
            this.location});
            this.dgvEditKit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvEditKit.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvEditKit.Location = new System.Drawing.Point(0, 0);
            this.dgvEditKit.Name = "dgvEditKit";
            this.dgvEditKit.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEditKit.Size = new System.Drawing.Size(784, 562);
            this.dgvEditKit.TabIndex = 1;
            this.dgvEditKit.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEditKit_CellContentClick);
            // 
            // kit_no
            // 
            this.kit_no.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.kit_no.HeaderText = "Kit#";
            this.kit_no.Name = "kit_no";
            this.kit_no.ReadOnly = true;
            // 
            // name
            // 
            this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.name.HeaderText = "Name";
            this.name.Name = "name";
            // 
            // sex
            // 
            this.sex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.sex.HeaderText = "Sex";
            this.sex.Items.AddRange(new object[] {
            "Unknown",
            "Male",
            "Female"});
            this.sex.Name = "sex";
            // 
            // disabled
            // 
            this.disabled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.disabled.HeaderText = "Disabled";
            this.disabled.Name = "disabled";
            // 
            // location
            // 
            this.location.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.location.HeaderText = "Location";
            this.location.Name = "location";
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
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataGridView dgvEditKit;
        private System.Windows.Forms.DataGridViewTextBoxColumn kit_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewComboBoxColumn sex;
        private System.Windows.Forms.DataGridViewCheckBoxColumn disabled;
        private System.Windows.Forms.DataGridViewButtonColumn location;
        private System.ComponentModel.BackgroundWorker bwDelete;
    }
}
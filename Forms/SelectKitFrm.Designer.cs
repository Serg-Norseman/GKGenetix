namespace Genetic_Genealogy_Kit
{
    partial class SelectKitFrm
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
            this.btnOpen = new System.Windows.Forms.Button();
            this.dataGridViewOpenKit = new System.Windows.Forms.DataGridView();
            this.kit_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.last_modified = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.kitLbl = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.bwExport = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOpenKit)).BeginInit();
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
            // dataGridViewOpenKit
            // 
            this.dataGridViewOpenKit.AllowUserToAddRows = false;
            this.dataGridViewOpenKit.AllowUserToDeleteRows = false;
            this.dataGridViewOpenKit.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridViewOpenKit.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.kit_no,
            this.name,
            this.last_modified});
            this.dataGridViewOpenKit.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewOpenKit.Location = new System.Drawing.Point(13, 13);
            this.dataGridViewOpenKit.MultiSelect = false;
            this.dataGridViewOpenKit.Name = "dataGridViewOpenKit";
            this.dataGridViewOpenKit.ReadOnly = true;
            this.dataGridViewOpenKit.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewOpenKit.ShowCellErrors = false;
            this.dataGridViewOpenKit.ShowCellToolTips = false;
            this.dataGridViewOpenKit.ShowEditingIcon = false;
            this.dataGridViewOpenKit.ShowRowErrors = false;
            this.dataGridViewOpenKit.Size = new System.Drawing.Size(439, 259);
            this.dataGridViewOpenKit.TabIndex = 2;
            this.dataGridViewOpenKit.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewOpenKit_CellContentDoubleClick);
            this.dataGridViewOpenKit.SelectionChanged += new System.EventHandler(this.dataGridViewOpenKit_SelectionChanged);
            // 
            // kit_no
            // 
            this.kit_no.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.kit_no.HeaderText = "Kit #";
            this.kit_no.Name = "kit_no";
            this.kit_no.ReadOnly = true;
            this.kit_no.Width = 54;
            // 
            // name
            // 
            this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.name.HeaderText = "Name";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // last_modified
            // 
            this.last_modified.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.last_modified.HeaderText = "Last Modified";
            this.last_modified.Name = "last_modified";
            this.last_modified.ReadOnly = true;
            this.last_modified.Width = 95;
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
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Genetic Genealogy Kit|*.ggk|FTDNA Format (Autosomal and X Only)|*.csv|23andMe For" +
    "mat (Autosomal and X Only)|*.txt";
            this.saveFileDialog.Title = "Export As";
            // 
            // bwExport
            // 
            this.bwExport.WorkerSupportsCancellation = true;
            this.bwExport.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwExport_DoWork);
            this.bwExport.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwExport_RunWorkerCompleted);
            // 
            // SelectKitFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 326);
            this.Controls.Add(this.kitLbl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridViewOpenKit);
            this.Controls.Add(this.btnOpen);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectKitFrm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Kit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SelectKitFrm_FormClosing);
            this.Load += new System.EventHandler(this.OpenKitFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOpenKit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.DataGridView dataGridViewOpenKit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label kitLbl;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataGridViewTextBoxColumn kit_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn last_modified;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.ComponentModel.BackgroundWorker bwExport;

    }
}
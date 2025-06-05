namespace GKGenetix.UI.Forms
{
    partial class ProcessKitsFrm
    {
        private System.Windows.Forms.Button btnStart;
        private System.ComponentModel.BackgroundWorker bwCompare;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.CheckBox chkDontSkip;
        private System.Windows.Forms.Label label1;
        private System.ComponentModel.BackgroundWorker bwROH;
        private System.ComponentModel.BackgroundWorker bwPhaseVisualizer;
        private System.Windows.Forms.CheckBox chkRedoRoH;

        private void InitializeComponent()
        {
            this.btnStart = new System.Windows.Forms.Button();
            this.bwCompare = new System.ComponentModel.BackgroundWorker();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.chkDontSkip = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bwROH = new System.ComponentModel.BackgroundWorker();
            this.bwPhaseVisualizer = new System.ComponentModel.BackgroundWorker();
            this.chkRedoRoH = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 35);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(131, 40);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // bwCompare
            // 
            this.bwCompare.WorkerReportsProgress = true;
            this.bwCompare.WorkerSupportsCancellation = true;
            this.bwCompare.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwCompare_DoWork);
            this.bwCompare.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwCompare_RunWorkerCompleted);
            // 
            // txtStatus
            // 
            this.txtStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStatus.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStatus.Location = new System.Drawing.Point(12, 110);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtStatus.Size = new System.Drawing.Size(760, 440);
            this.txtStatus.TabIndex = 3;
            // 
            // chkDontSkip
            // 
            this.chkDontSkip.AutoSize = true;
            this.chkDontSkip.Location = new System.Drawing.Point(12, 12);
            this.chkDontSkip.Name = "chkDontSkip";
            this.chkDontSkip.Size = new System.Drawing.Size(164, 17);
            this.chkDontSkip.TabIndex = 4;
            this.chkDontSkip.Text = "Delete Existing Comparisons.";
            this.chkDontSkip.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Status:";
            // 
            // bwROH
            // 
            this.bwROH.WorkerReportsProgress = true;
            this.bwROH.WorkerSupportsCancellation = true;
            this.bwROH.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwROH_DoWork);
            this.bwROH.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwROH_ProgressChanged);
            this.bwROH.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwROH_RunWorkerCompleted);
            // 
            // bwPhaseVisualizer
            // 
            this.bwPhaseVisualizer.WorkerReportsProgress = true;
            this.bwPhaseVisualizer.WorkerSupportsCancellation = true;
            this.bwPhaseVisualizer.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwPhaseVisualizer_DoWork);
            this.bwPhaseVisualizer.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwPhaseVisualizer_ProgressChanged);
            this.bwPhaseVisualizer.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwPhaseVisualizer_RunWorkerCompleted);
            // 
            // chkRedoRoH
            // 
            this.chkRedoRoH.AutoSize = true;
            this.chkRedoRoH.Location = new System.Drawing.Point(332, 12);
            this.chkRedoRoH.Name = "chkRedoRoH";
            this.chkRedoRoH.Size = new System.Drawing.Size(207, 17);
            this.chkRedoRoH.TabIndex = 7;
            this.chkRedoRoH.Text = "Delete Existing Runs of Homozygosity";
            this.chkRedoRoH.UseVisualStyleBackColor = true;
            // 
            // ProcessKitsFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.chkRedoRoH);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkDontSkip);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.btnStart);
            this.Name = "ProcessKitsFrm";
            this.Text = "Process Kits";
            this.Closing += new System.EventHandler(this.ProcessKitsFrm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

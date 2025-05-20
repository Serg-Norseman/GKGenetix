namespace Genetic_Genealogy_Kit
{
    partial class ProcessKitsFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProcessKitsFrm));
            this.btnStart = new System.Windows.Forms.Button();
            this.bwCompare = new System.ComponentModel.BackgroundWorker();
            this.lblComparing = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.tbStatus = new System.Windows.Forms.TextBox();
            this.cbDontSkip = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.cbNoAdmixture = new System.Windows.Forms.CheckBox();
            this.bwROH = new System.ComponentModel.BackgroundWorker();
            this.bwPhaseVisualizer = new System.ComponentModel.BackgroundWorker();
            this.cbRedoVisual = new System.Windows.Forms.CheckBox();
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
            this.bwCompare.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwCompare_ProgressChanged);
            this.bwCompare.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwCompare_RunWorkerCompleted);
            // 
            // lblComparing
            // 
            this.lblComparing.AutoSize = true;
            this.lblComparing.Location = new System.Drawing.Point(12, 94);
            this.lblComparing.Name = "lblComparing";
            this.lblComparing.Size = new System.Drawing.Size(471, 13);
            this.lblComparing.TabIndex = 1;
            this.lblComparing.Text = "Click \'Start\' to begin. You should be able to use One-to-Many once this processin" +
    "g gets completed.";
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(12, 110);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(760, 31);
            this.progressBar.TabIndex = 2;
            // 
            // tbStatus
            // 
            this.tbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbStatus.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbStatus.Location = new System.Drawing.Point(12, 175);
            this.tbStatus.Multiline = true;
            this.tbStatus.Name = "tbStatus";
            this.tbStatus.ReadOnly = true;
            this.tbStatus.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbStatus.Size = new System.Drawing.Size(760, 375);
            this.tbStatus.TabIndex = 3;
            // 
            // cbDontSkip
            // 
            this.cbDontSkip.AutoSize = true;
            this.cbDontSkip.Location = new System.Drawing.Point(12, 12);
            this.cbDontSkip.Name = "cbDontSkip";
            this.cbDontSkip.Size = new System.Drawing.Size(164, 17);
            this.cbDontSkip.TabIndex = 4;
            this.cbDontSkip.Text = "Delete Existing Comparisions.";
            this.cbDontSkip.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 157);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Status:";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // cbNoAdmixture
            // 
            this.cbNoAdmixture.AutoSize = true;
            this.cbNoAdmixture.Location = new System.Drawing.Point(182, 12);
            this.cbNoAdmixture.Name = "cbNoAdmixture";
            this.cbNoAdmixture.Size = new System.Drawing.Size(144, 17);
            this.cbNoAdmixture.TabIndex = 6;
            this.cbNoAdmixture.Text = "No Admixture Calculation";
            this.cbNoAdmixture.UseVisualStyleBackColor = true;
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
            // cbRedoVisual
            // 
            this.cbRedoVisual.AutoSize = true;
            this.cbRedoVisual.Location = new System.Drawing.Point(332, 12);
            this.cbRedoVisual.Name = "cbRedoVisual";
            this.cbRedoVisual.Size = new System.Drawing.Size(207, 17);
            this.cbRedoVisual.TabIndex = 7;
            this.cbRedoVisual.Text = "Delete Existing Segment Visualizations";
            this.cbRedoVisual.UseVisualStyleBackColor = true;
            // 
            // ProcessKitsFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.cbRedoVisual);
            this.Controls.Add(this.cbNoAdmixture);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbDontSkip);
            this.Controls.Add(this.tbStatus);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblComparing);
            this.Controls.Add(this.btnStart);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProcessKitsFrm";
            this.ShowInTaskbar = false;
            this.Text = "Process Kits";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProcessKitsFrm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.ComponentModel.BackgroundWorker bwCompare;
        private System.Windows.Forms.Label lblComparing;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TextBox tbStatus;
        private System.Windows.Forms.CheckBox cbDontSkip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox cbNoAdmixture;
        private System.ComponentModel.BackgroundWorker bwROH;
        private System.ComponentModel.BackgroundWorker bwPhaseVisualizer;
        private System.Windows.Forms.CheckBox cbRedoVisual;
    }
}
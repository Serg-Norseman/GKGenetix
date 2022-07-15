namespace GKGenetix
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnDNAAnalysis;
        private System.Windows.Forms.Button btnDNAInheritanceTest;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnDNAAnalysis = new System.Windows.Forms.Button();
            this.btnDNAInheritanceTest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDNAAnalysis
            // 
            this.btnDNAAnalysis.Location = new System.Drawing.Point(12, 12);
            this.btnDNAAnalysis.Name = "btnDNAAnalysis";
            this.btnDNAAnalysis.Size = new System.Drawing.Size(200, 56);
            this.btnDNAAnalysis.TabIndex = 0;
            this.btnDNAAnalysis.Text = "DNA Analysis";
            this.btnDNAAnalysis.UseVisualStyleBackColor = true;
            this.btnDNAAnalysis.Click += new System.EventHandler(this.btnDNAAnalysis_Click);
            // 
            // btnDNAInheritanceTest
            // 
            this.btnDNAInheritanceTest.Location = new System.Drawing.Point(12, 74);
            this.btnDNAInheritanceTest.Name = "btnDNAInheritanceTest";
            this.btnDNAInheritanceTest.Size = new System.Drawing.Size(200, 56);
            this.btnDNAInheritanceTest.TabIndex = 0;
            this.btnDNAInheritanceTest.Text = "DNA Inheritance Test";
            this.btnDNAInheritanceTest.UseVisualStyleBackColor = true;
            this.btnDNAInheritanceTest.Click += new System.EventHandler(this.btnDNAInheritanceTest_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(227, 145);
            this.Controls.Add(this.btnDNAInheritanceTest);
            this.Controls.Add(this.btnDNAAnalysis);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GKGenetix";
            this.ResumeLayout(false);
        }
    }
}

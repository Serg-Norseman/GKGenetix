namespace GKGenetix.UI
{
    partial class DevConsole
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnSimpleAnalysis;
        private System.Windows.Forms.ToolStripButton btnInheritanceTest;
        private System.Windows.Forms.ToolStripButton btnGenImage;
        private System.Windows.Forms.TextBox txtOutput;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnSimpleAnalysis = new System.Windows.Forms.ToolStripButton();
            this.btnInheritanceTest = new System.Windows.Forms.ToolStripButton();
            this.btnGenImage = new System.Windows.Forms.ToolStripButton();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSimpleAnalysis, this.btnInheritanceTest, this.btnGenImage});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1955, 39);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnSimpleAnalysis
            // 
            this.btnSimpleAnalysis.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSimpleAnalysis.Name = "btnSimpleAnalysis";
            this.btnSimpleAnalysis.Size = new System.Drawing.Size(73, 36);
            this.btnSimpleAnalysis.Text = "Simple analysis...";
            this.btnSimpleAnalysis.Click += new System.EventHandler(this.btnSimpleAnalysis_Click);
            // 
            // btnInheritanceTest
            // 
            this.btnInheritanceTest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnInheritanceTest.Name = "btnInheritanceTest";
            this.btnInheritanceTest.Size = new System.Drawing.Size(79, 24);
            this.btnInheritanceTest.Text = "Inheritance test...";
            this.btnInheritanceTest.Click += new System.EventHandler(this.btnInheritanceTest_Click);
            // 
            // btnGenImage
            // 
            this.btnGenImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGenImage.Name = "btnLoadFile";
            this.btnGenImage.Size = new System.Drawing.Size(73, 36);
            this.btnGenImage.Text = "Gen Image";
            this.btnGenImage.Click += new System.EventHandler(this.btnGenImage_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOutput.Location = new System.Drawing.Point(0, 0);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOutput.Size = new System.Drawing.Size(1300, 863);
            this.txtOutput.TabIndex = 3;
            // 
            // DevConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1564, 729);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "DevConsole";
            this.Text = "DNA Analysis Development Console";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

﻿namespace GKGenetix.UI.Forms
{
    partial class PhasedSegmentFrm
    {
        private System.Windows.Forms.PictureBox pbSegment;
        private System.Windows.Forms.DataGridView dgvSegment;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLbl;

        private void InitializeComponent()
        {
            this.dgvSegment = new System.Windows.Forms.DataGridView();
            this.pbSegment = new System.Windows.Forms.PictureBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLbl = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSegment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSegment)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvSegment
            // 
            this.dgvSegment.Location = new System.Drawing.Point(12, 168);
            this.dgvSegment.Name = "dgvSegment";
            this.dgvSegment.Size = new System.Drawing.Size(600, 232);
            this.dgvSegment.TabIndex = 1;
            this.dgvSegment.SelectionChanged += new System.EventHandler(this.dgvSegment_SelectionChanged);
            // 
            // pbSegment
            // 
            this.pbSegment.BackColor = System.Drawing.Color.White;
            this.pbSegment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbSegment.Location = new System.Drawing.Point(12, 12);
            this.pbSegment.Name = "pbSegment";
            this.pbSegment.Size = new System.Drawing.Size(600, 150);
            this.pbSegment.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbSegment.TabIndex = 0;
            this.pbSegment.TabStop = false;
            this.pbSegment.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbSegment_MouseClick);
            this.pbSegment.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbSegment_MouseMove);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLbl});
            this.statusStrip1.Location = new System.Drawing.Point(0, 420);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(624, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLbl
            // 
            this.statusLbl.Name = "statusLbl";
            this.statusLbl.Size = new System.Drawing.Size(48, 17);
            this.statusLbl.Text = "Phased.";
            // 
            // PhasedSegmentVisualizerFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 442);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.dgvSegment);
            this.Controls.Add(this.pbSegment);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PhasedSegmentVisualizerFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Phased Segment Visualizer";
            this.Load += new System.EventHandler(this.PhasedSegmentFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSegment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSegment)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

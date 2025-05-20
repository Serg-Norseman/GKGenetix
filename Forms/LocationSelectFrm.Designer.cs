namespace Genetic_Genealogy_Kit
{
    partial class LocationSelectFrm
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
            this.pbWorldMap = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbWorldMap)).BeginInit();
            this.SuspendLayout();
            // 
            // pbWorldMap
            // 
            this.pbWorldMap.BackColor = System.Drawing.Color.White;
            this.pbWorldMap.Cursor = System.Windows.Forms.Cursors.Cross;
            this.pbWorldMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbWorldMap.Image = global::Genetic_Genealogy_Kit.Properties.Resources.world_map;
            this.pbWorldMap.Location = new System.Drawing.Point(0, 0);
            this.pbWorldMap.Name = "pbWorldMap";
            this.pbWorldMap.Size = new System.Drawing.Size(678, 314);
            this.pbWorldMap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbWorldMap.TabIndex = 0;
            this.pbWorldMap.TabStop = false;
            this.pbWorldMap.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbWorldMap_MouseClick);
            this.pbWorldMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbWorldMap_MouseMove);
            // 
            // LocationSelectFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(678, 314);
            this.Controls.Add(this.pbWorldMap);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "LocationSelectFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "World Map";
            this.Load += new System.EventHandler(this.LocationSelectFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbWorldMap)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbWorldMap;
    }
}
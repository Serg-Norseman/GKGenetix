namespace GKGenetix.UI.Forms
{
    partial class LocationSelectFrm
    {
        private GKGenetix.UI.GKMapBrowser pbWorldMap;

        private void InitializeComponent()
        {
            this.pbWorldMap = new GKGenetix.UI.GKMapBrowser();
            this.SuspendLayout();
            // 
            // pbWorldMap
            // 
            this.pbWorldMap.BackColor = System.Drawing.Color.White;
            this.pbWorldMap.Cursor = System.Windows.Forms.Cursors.Cross;
            this.pbWorldMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbWorldMap.Location = new System.Drawing.Point(0, 0);
            this.pbWorldMap.Name = "pbWorldMap";
            this.pbWorldMap.Size = new System.Drawing.Size(678, 314);
            this.pbWorldMap.TabIndex = 0;
            this.pbWorldMap.TabStop = false;
            // 
            // LocationSelectFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(678, 314);
            this.Controls.Add(this.pbWorldMap);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "LocationSelectFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "World Map";
            this.ResumeLayout(false);
        }
    }
}

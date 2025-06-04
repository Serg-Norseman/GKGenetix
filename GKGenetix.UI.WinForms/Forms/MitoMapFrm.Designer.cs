namespace GKGenetix.UI.Forms
{
    partial class MitoMapFrm
    {
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataVisualization.Charting.Chart mtdna_chart;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage mtMapTab;
        private System.Windows.Forms.TabPage details;
        private System.Windows.Forms.DataGridView dgvMtDna;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage rsrs;
        private System.Windows.Forms.DataGridView dgvNucleotides;
        private System.Windows.Forms.TabPage tabFASTA;
        private System.Windows.Forms.RichTextBox rtbFASTA;

        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.mtMapTab = new System.Windows.Forms.TabPage();
            this.mtdna_chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.details = new System.Windows.Forms.TabPage();
            this.dgvMtDna = new System.Windows.Forms.DataGridView();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.rsrs = new System.Windows.Forms.TabPage();
            this.dgvNucleotides = new System.Windows.Forms.DataGridView();
            this.tabFASTA = new System.Windows.Forms.TabPage();
            this.rtbFASTA = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.mtMapTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mtdna_chart)).BeginInit();
            this.details.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMtDna)).BeginInit();
            this.tabControl2.SuspendLayout();
            this.rsrs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNucleotides)).BeginInit();
            this.tabFASTA.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tabControl2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(784, 562);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.mtMapTab);
            this.tabControl1.Controls.Add(this.details);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 33);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(542, 526);
            this.tabControl1.TabIndex = 1;
            // 
            // mtMapTab
            // 
            this.mtMapTab.Controls.Add(this.mtdna_chart);
            this.mtMapTab.Location = new System.Drawing.Point(4, 22);
            this.mtMapTab.Name = "mtMapTab";
            this.mtMapTab.Padding = new System.Windows.Forms.Padding(3);
            this.mtMapTab.Size = new System.Drawing.Size(534, 500);
            this.mtMapTab.TabIndex = 0;
            this.mtMapTab.Text = "Mitocondria Map";
            this.mtMapTab.UseVisualStyleBackColor = true;
            // 
            // mtdna_chart
            // 
            this.mtdna_chart.BackColor = System.Drawing.Color.Transparent;
            chartArea1.Area3DStyle.Enable3D = true;
            chartArea1.Area3DStyle.LightStyle = System.Windows.Forms.DataVisualization.Charting.LightStyle.Realistic;
            chartArea1.BackColor = System.Drawing.Color.Transparent;
            chartArea1.Name = "ChartArea1";
            this.mtdna_chart.ChartAreas.Add(chartArea1);
            this.mtdna_chart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtdna_chart.Location = new System.Drawing.Point(3, 3);
            this.mtdna_chart.Name = "mtdna_chart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
            series1.CustomProperties = "PieLineColor=Black, CollectedSliceExploded=True, DoughnutRadius=5, PieDrawingStyl" +
    "e=SoftEdge, PieLabelStyle=Outside, PieStartAngle=300";
            series1.EmptyPointStyle.CustomProperties = "Exploded=True, PieLabelStyle=Outside";
            series1.EmptyPointStyle.IsVisibleInLegend = false;
            series1.Name = "Series1";
            series1.SmartLabelStyle.CalloutStyle = System.Windows.Forms.DataVisualization.Charting.LabelCalloutStyle.Box;
            series1.SmartLabelStyle.Enabled = false;
            this.mtdna_chart.Series.Add(series1);
            this.mtdna_chart.Size = new System.Drawing.Size(528, 494);
            this.mtdna_chart.TabIndex = 0;
            this.mtdna_chart.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mtdna_chart_MouseClick);
            this.mtdna_chart.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mtdna_chart_MouseDown);
            this.mtdna_chart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mtdna_chart_MouseMove);
            this.mtdna_chart.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mtdna_chart_MouseUp);
            // 
            // details
            // 
            this.details.Controls.Add(this.dgvMtDna);
            this.details.Location = new System.Drawing.Point(4, 22);
            this.details.Name = "details";
            this.details.Padding = new System.Windows.Forms.Padding(3);
            this.details.Size = new System.Drawing.Size(534, 500);
            this.details.TabIndex = 1;
            this.details.Text = "Details";
            this.details.UseVisualStyleBackColor = true;
            // 
            // dgvMtDna
            // 
            this.dgvMtDna.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMtDna.Location = new System.Drawing.Point(3, 3);
            this.dgvMtDna.Name = "dgvMtDna";
            this.dgvMtDna.Size = new System.Drawing.Size(528, 494);
            this.dgvMtDna.TabIndex = 0;
            this.dgvMtDna.SelectionChanged += new System.EventHandler(this.dgvMtDna_SelectionChanged);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.rsrs);
            this.tabControl2.Controls.Add(this.tabFASTA);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(551, 33);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(230, 526);
            this.tabControl2.TabIndex = 0;
            // 
            // rsrs
            // 
            this.rsrs.Controls.Add(this.dgvNucleotides);
            this.rsrs.Location = new System.Drawing.Point(4, 22);
            this.rsrs.Name = "rsrs";
            this.rsrs.Padding = new System.Windows.Forms.Padding(3);
            this.rsrs.Size = new System.Drawing.Size(222, 500);
            this.rsrs.TabIndex = 1;
            this.rsrs.Text = "Nucleotides";
            this.rsrs.UseVisualStyleBackColor = true;
            // 
            // dgvNucleotides
            // 
            this.dgvNucleotides.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvNucleotides.Location = new System.Drawing.Point(3, 3);
            this.dgvNucleotides.Name = "dgvNucleotides";
            this.dgvNucleotides.Size = new System.Drawing.Size(216, 494);
            this.dgvNucleotides.TabIndex = 0;
            this.dgvNucleotides.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(dgvNucleotides_CellFormatting);
            // 
            // tabFASTA
            // 
            this.tabFASTA.Controls.Add(this.rtbFASTA);
            this.tabFASTA.Location = new System.Drawing.Point(4, 22);
            this.tabFASTA.Name = "tabFASTA";
            this.tabFASTA.Padding = new System.Windows.Forms.Padding(3);
            this.tabFASTA.Size = new System.Drawing.Size(222, 500);
            this.tabFASTA.TabIndex = 2;
            this.tabFASTA.Text = "FASTA";
            this.tabFASTA.UseVisualStyleBackColor = true;
            // 
            // rtbFASTA
            // 
            this.rtbFASTA.BackColor = System.Drawing.Color.White;
            this.rtbFASTA.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbFASTA.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.rtbFASTA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbFASTA.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbFASTA.ForeColor = System.Drawing.Color.LightGray;
            this.rtbFASTA.Location = new System.Drawing.Point(3, 3);
            this.rtbFASTA.Name = "rtbFASTA";
            this.rtbFASTA.Size = new System.Drawing.Size(216, 494);
            this.rtbFASTA.TabIndex = 0;
            this.rtbFASTA.Text = "";
            this.rtbFASTA.WordWrap = false;
            // 
            // MitoMapFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MitoMapFrm";
            this.Text = "Mito Map";
            this.Load += new System.EventHandler(this.MitoMapFrm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.mtMapTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mtdna_chart)).EndInit();
            this.details.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMtDna)).EndInit();
            this.tabControl2.ResumeLayout(false);
            this.rsrs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNucleotides)).EndInit();
            this.tabFASTA.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}

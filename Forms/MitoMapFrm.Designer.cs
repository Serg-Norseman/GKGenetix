namespace Genetic_Genealogy_Kit
{
    partial class MitoMapFrm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MitoMapFrm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.mtMapTab = new System.Windows.Forms.TabPage();
            this.mtdna_chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.details = new System.Windows.Forms.TabPage();
            this.dgvmtdna = new System.Windows.Forms.DataGridView();
            this.map_locus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.start = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.end = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shorthand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.rsrs = new System.Windows.Forms.TabPage();
            this.dgvNucleotides = new System.Windows.Forms.DataGridView();
            this.pos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rsrsname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ckit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabFASTA = new System.Windows.Forms.TabPage();
            this.rtbFASTA = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.mtMapTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mtdna_chart)).BeginInit();
            this.details.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvmtdna)).BeginInit();
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
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tabControl2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
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
            this.details.Controls.Add(this.dgvmtdna);
            this.details.Location = new System.Drawing.Point(4, 22);
            this.details.Name = "details";
            this.details.Padding = new System.Windows.Forms.Padding(3);
            this.details.Size = new System.Drawing.Size(534, 500);
            this.details.TabIndex = 1;
            this.details.Text = "Details";
            this.details.UseVisualStyleBackColor = true;
            // 
            // dgvmtdna
            // 
            this.dgvmtdna.AllowUserToAddRows = false;
            this.dgvmtdna.AllowUserToDeleteRows = false;
            this.dgvmtdna.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvmtdna.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvmtdna.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.map_locus,
            this.start,
            this.end,
            this.total,
            this.shorthand,
            this.description});
            this.dgvmtdna.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvmtdna.Location = new System.Drawing.Point(3, 3);
            this.dgvmtdna.MultiSelect = false;
            this.dgvmtdna.Name = "dgvmtdna";
            this.dgvmtdna.ReadOnly = true;
            this.dgvmtdna.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvmtdna.Size = new System.Drawing.Size(528, 494);
            this.dgvmtdna.TabIndex = 0;
            this.dgvmtdna.SelectionChanged += new System.EventHandler(this.dgvmtdna_SelectionChanged);
            // 
            // map_locus
            // 
            this.map_locus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.map_locus.HeaderText = "Map Locus";
            this.map_locus.Name = "map_locus";
            this.map_locus.ReadOnly = true;
            // 
            // start
            // 
            this.start.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.start.HeaderText = "Start Position";
            this.start.Name = "start";
            this.start.ReadOnly = true;
            // 
            // end
            // 
            this.end.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.end.HeaderText = "End Position";
            this.end.Name = "end";
            this.end.ReadOnly = true;
            // 
            // total
            // 
            this.total.HeaderText = "Total Nucleotides";
            this.total.Name = "total";
            this.total.ReadOnly = true;
            // 
            // shorthand
            // 
            this.shorthand.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.shorthand.HeaderText = "Shorthand";
            this.shorthand.Name = "shorthand";
            this.shorthand.ReadOnly = true;
            // 
            // description
            // 
            this.description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.description.HeaderText = "Description";
            this.description.Name = "description";
            this.description.ReadOnly = true;
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
            this.dgvNucleotides.AllowUserToAddRows = false;
            this.dgvNucleotides.AllowUserToDeleteRows = false;
            this.dgvNucleotides.AllowUserToResizeColumns = false;
            this.dgvNucleotides.AllowUserToResizeRows = false;
            this.dgvNucleotides.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvNucleotides.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNucleotides.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.pos,
            this.rsrsname,
            this.ckit});
            this.dgvNucleotides.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvNucleotides.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvNucleotides.Location = new System.Drawing.Point(3, 3);
            this.dgvNucleotides.MultiSelect = false;
            this.dgvNucleotides.Name = "dgvNucleotides";
            this.dgvNucleotides.ReadOnly = true;
            this.dgvNucleotides.RowHeadersVisible = false;
            this.dgvNucleotides.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvNucleotides.Size = new System.Drawing.Size(216, 494);
            this.dgvNucleotides.TabIndex = 0;
            // 
            // pos
            // 
            this.pos.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.pos.HeaderText = "Position";
            this.pos.Name = "pos";
            this.pos.ReadOnly = true;
            this.pos.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // rsrsname
            // 
            this.rsrsname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.rsrsname.HeaderText = "RSRS";
            this.rsrsname.Name = "rsrsname";
            this.rsrsname.ReadOnly = true;
            this.rsrsname.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ckit
            // 
            this.ckit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ckit.HeaderText = "Kit";
            this.ckit.Name = "ckit";
            this.ckit.ReadOnly = true;
            this.ckit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
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
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "..";
            // 
            // MitoMapFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MitoMapFrm";
            this.Text = "Mito Map";
            this.Load += new System.EventHandler(this.MitoMapFrm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.mtMapTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mtdna_chart)).EndInit();
            this.details.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvmtdna)).EndInit();
            this.tabControl2.ResumeLayout(false);
            this.rsrs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNucleotides)).EndInit();
            this.tabFASTA.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataVisualization.Charting.Chart mtdna_chart;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage mtMapTab;
        private System.Windows.Forms.TabPage details;
        private System.Windows.Forms.DataGridView dgvmtdna;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage rsrs;
        private System.Windows.Forms.DataGridView dgvNucleotides;
        private System.Windows.Forms.DataGridViewTextBoxColumn map_locus;
        private System.Windows.Forms.DataGridViewTextBoxColumn start;
        private System.Windows.Forms.DataGridViewTextBoxColumn end;
        private System.Windows.Forms.DataGridViewTextBoxColumn total;
        private System.Windows.Forms.DataGridViewTextBoxColumn shorthand;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
        private System.Windows.Forms.DataGridViewTextBoxColumn pos;
        private System.Windows.Forms.DataGridViewTextBoxColumn rsrsname;
        private System.Windows.Forms.DataGridViewTextBoxColumn ckit;
        private System.Windows.Forms.TabPage tabFASTA;
        private System.Windows.Forms.RichTextBox rtbFASTA;
        private System.Windows.Forms.Label label1;
    }
}
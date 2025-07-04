﻿namespace GKGenetix.UI.Forms
{
    partial class AdmixtureFrm
    {
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabOrigins;
        private System.Windows.Forms.TabPage tabAdmixTable;
        private System.Windows.Forms.DataGridView dgvAdmixture;
        private System.Windows.Forms.TabPage tabChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private GKGenetix.UI.GKMapBrowser pbWorldMap;

        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabOrigins = new System.Windows.Forms.TabPage();
            this.pbWorldMap = new GKGenetix.UI.GKMapBrowser();
            this.tabAdmixTable = new System.Windows.Forms.TabPage();
            this.dgvAdmixture = new System.Windows.Forms.DataGridView();
            this.tabChart = new System.Windows.Forms.TabPage();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabOrigins.SuspendLayout();
            this.tabAdmixTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdmixture)).BeginInit();
            this.tabChart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 0);
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
            this.tabControl1.Controls.Add(this.tabOrigins);
            this.tabControl1.Controls.Add(this.tabChart);
            this.tabControl1.Controls.Add(this.tabAdmixTable);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 63);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(778, 496);
            this.tabControl1.TabIndex = 2;
            // 
            // tabOrigins
            // 
            this.tabOrigins.Controls.Add(this.pbWorldMap);
            this.tabOrigins.Location = new System.Drawing.Point(4, 22);
            this.tabOrigins.Name = "tabOrigins";
            this.tabOrigins.Padding = new System.Windows.Forms.Padding(3);
            this.tabOrigins.Size = new System.Drawing.Size(770, 470);
            this.tabOrigins.TabIndex = 2;
            this.tabOrigins.Text = "Ethnic Origins";
            this.tabOrigins.UseVisualStyleBackColor = true;
            // 
            // pbWorldMap
            // 
            this.pbWorldMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbWorldMap.Location = new System.Drawing.Point(3, 3);
            this.pbWorldMap.Name = "pbWorldMap";
            this.pbWorldMap.Size = new System.Drawing.Size(764, 464);
            this.pbWorldMap.TabIndex = 0;
            this.pbWorldMap.TabStop = false;
            // 
            // tabAdmixTable
            // 
            this.tabAdmixTable.Controls.Add(this.dgvAdmixture);
            this.tabAdmixTable.Location = new System.Drawing.Point(4, 22);
            this.tabAdmixTable.Name = "tabAdmixTable";
            this.tabAdmixTable.Padding = new System.Windows.Forms.Padding(3);
            this.tabAdmixTable.Size = new System.Drawing.Size(770, 470);
            this.tabAdmixTable.TabIndex = 0;
            this.tabAdmixTable.Text = "Admixture Table";
            this.tabAdmixTable.UseVisualStyleBackColor = true;
            // 
            // dgvAdmixture
            // 
            this.dgvAdmixture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAdmixture.Location = new System.Drawing.Point(3, 3);
            this.dgvAdmixture.Name = "dgvAdmixture";
            this.dgvAdmixture.Size = new System.Drawing.Size(764, 464);
            this.dgvAdmixture.TabIndex = 0;
            // 
            // tabChart
            // 
            this.tabChart.Controls.Add(this.chart1);
            this.tabChart.Location = new System.Drawing.Point(4, 22);
            this.tabChart.Name = "tabChart";
            this.tabChart.Padding = new System.Windows.Forms.Padding(3);
            this.tabChart.Size = new System.Drawing.Size(770, 470);
            this.tabChart.TabIndex = 1;
            this.tabChart.Text = "Chart View";
            this.tabChart.UseVisualStyleBackColor = true;
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(3, 3);
            this.chart1.Name = "chart1";
            series1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.LeftRight;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series1.SmartLabelStyle.CalloutBackColor = System.Drawing.Color.White;
            series1.YValueMembers = " ";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(764, 464);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // AdmixtureFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "AdmixtureFrm";
            this.Text = "Admixture";
            this.Load += new System.EventHandler(this.AdmixtureFrm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabOrigins.ResumeLayout(false);
            this.tabAdmixTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdmixture)).EndInit();
            this.tabChart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
        }
    }
}

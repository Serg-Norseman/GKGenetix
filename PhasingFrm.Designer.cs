namespace Genetic_Genealogy_Kit
{
    partial class PhasingFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PhasingFrm));
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnFather = new System.Windows.Forms.Button();
            this.btnMother = new System.Windows.Forms.Button();
            this.btnChild = new System.Windows.Forms.Button();
            this.btnPhasing = new System.Windows.Forms.Button();
            this.bwPhasing = new System.ComponentModel.BackgroundWorker();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbMale = new System.Windows.Forms.RadioButton();
            this.rbFemale = new System.Windows.Forms.RadioButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pbChild = new System.Windows.Forms.PictureBox();
            this.dgvPhasing = new System.Windows.Forms.DataGridView();
            this.rsid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chromosome = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.position = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.child = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.father = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mother = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.phased_paternal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.phased_maternal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbChild)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhasing)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(82, 153);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 69);
            this.label3.TabIndex = 5;
            this.label3.Text = "Child";
            this.label3.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(150, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 30);
            this.label2.TabIndex = 3;
            this.label2.Text = "Mother";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(14, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 30);
            this.label1.TabIndex = 2;
            this.label1.Text = "Father";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox2, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.pbChild, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnFather, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnMother, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnChild, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnPhasing, 3, 7);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 3, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 9;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(229, 556);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnFather
            // 
            this.btnFather.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnFather.Location = new System.Drawing.Point(14, 156);
            this.btnFather.Name = "btnFather";
            this.btnFather.Size = new System.Drawing.Size(62, 63);
            this.btnFather.TabIndex = 17;
            this.btnFather.Text = "Select Father";
            this.btnFather.UseVisualStyleBackColor = true;
            this.btnFather.Click += new System.EventHandler(this.btnFather_Click);
            // 
            // btnMother
            // 
            this.btnMother.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnMother.Location = new System.Drawing.Point(150, 156);
            this.btnMother.Name = "btnMother";
            this.btnMother.Size = new System.Drawing.Size(62, 63);
            this.btnMother.TabIndex = 18;
            this.btnMother.Text = "Select Mother";
            this.btnMother.UseVisualStyleBackColor = true;
            this.btnMother.Click += new System.EventHandler(this.btnMother_Click);
            // 
            // btnChild
            // 
            this.btnChild.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnChild.Location = new System.Drawing.Point(82, 318);
            this.btnChild.Name = "btnChild";
            this.btnChild.Size = new System.Drawing.Size(62, 63);
            this.btnChild.TabIndex = 19;
            this.btnChild.Text = "Select Child";
            this.btnChild.UseVisualStyleBackColor = true;
            this.btnChild.Click += new System.EventHandler(this.btnChild_Click);
            // 
            // btnPhasing
            // 
            this.btnPhasing.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPhasing.Enabled = false;
            this.btnPhasing.Location = new System.Drawing.Point(150, 456);
            this.btnPhasing.Name = "btnPhasing";
            this.btnPhasing.Size = new System.Drawing.Size(62, 63);
            this.btnPhasing.TabIndex = 20;
            this.btnPhasing.Text = "Begin Phasing";
            this.btnPhasing.UseVisualStyleBackColor = true;
            this.btnPhasing.Click += new System.EventHandler(this.btnPhasing_Click);
            // 
            // bwPhasing
            // 
            this.bwPhasing.WorkerReportsProgress = true;
            this.bwPhasing.WorkerSupportsCancellation = true;
            this.bwPhasing.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwPhasing_DoWork);
            this.bwPhasing.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwPhasing_ProgressChanged);
            this.bwPhasing.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwPhasing_RunWorkerCompleted);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.dgvPhasing, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(784, 562);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbFemale);
            this.panel1.Controls.Add(this.rbMale);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(150, 318);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(62, 63);
            this.panel1.TabIndex = 21;
            // 
            // rbMale
            // 
            this.rbMale.AutoSize = true;
            this.rbMale.Checked = true;
            this.rbMale.Location = new System.Drawing.Point(3, 4);
            this.rbMale.Name = "rbMale";
            this.rbMale.Size = new System.Drawing.Size(48, 17);
            this.rbMale.TabIndex = 0;
            this.rbMale.Text = "Male";
            this.rbMale.UseVisualStyleBackColor = true;
            this.rbMale.CheckedChanged += new System.EventHandler(this.rbMale_CheckedChanged);
            // 
            // rbFemale
            // 
            this.rbFemale.AutoSize = true;
            this.rbFemale.Location = new System.Drawing.Point(3, 23);
            this.rbFemale.Name = "rbFemale";
            this.rbFemale.Size = new System.Drawing.Size(59, 17);
            this.rbFemale.TabIndex = 1;
            this.rbFemale.Text = "Female";
            this.rbFemale.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::Genetic_Genealogy_Kit.Properties.Resources.father;
            this.pictureBox1.Location = new System.Drawing.Point(14, 63);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(62, 87);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Image = global::Genetic_Genealogy_Kit.Properties.Resources.mother;
            this.pictureBox2.Location = new System.Drawing.Point(150, 63);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(62, 87);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // pbChild
            // 
            this.pbChild.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbChild.Image = global::Genetic_Genealogy_Kit.Properties.Resources.boy;
            this.pbChild.Location = new System.Drawing.Point(82, 225);
            this.pbChild.Name = "pbChild";
            this.pbChild.Size = new System.Drawing.Size(62, 87);
            this.pbChild.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbChild.TabIndex = 6;
            this.pbChild.TabStop = false;
            // 
            // dgvPhasing
            // 
            this.dgvPhasing.AllowUserToAddRows = false;
            this.dgvPhasing.AllowUserToDeleteRows = false;
            this.dgvPhasing.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPhasing.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvPhasing.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPhasing.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.rsid,
            this.chromosome,
            this.position,
            this.child,
            this.father,
            this.mother,
            this.phased_paternal,
            this.phased_maternal});
            this.dgvPhasing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPhasing.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvPhasing.Location = new System.Drawing.Point(238, 3);
            this.dgvPhasing.Name = "dgvPhasing";
            this.dgvPhasing.ReadOnly = true;
            this.dgvPhasing.RowHeadersVisible = false;
            this.dgvPhasing.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPhasing.Size = new System.Drawing.Size(543, 556);
            this.dgvPhasing.TabIndex = 1;
            // 
            // rsid
            // 
            this.rsid.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.rsid.HeaderText = "RSID";
            this.rsid.Name = "rsid";
            this.rsid.ReadOnly = true;
            // 
            // chromosome
            // 
            this.chromosome.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.chromosome.HeaderText = "Chromosome";
            this.chromosome.Name = "chromosome";
            this.chromosome.ReadOnly = true;
            // 
            // position
            // 
            this.position.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.position.HeaderText = "Position";
            this.position.Name = "position";
            this.position.ReadOnly = true;
            // 
            // child
            // 
            this.child.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.child.HeaderText = "Child";
            this.child.Name = "child";
            this.child.ReadOnly = true;
            // 
            // father
            // 
            this.father.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.father.HeaderText = "Father";
            this.father.Name = "father";
            this.father.ReadOnly = true;
            // 
            // mother
            // 
            this.mother.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.mother.HeaderText = "Mother";
            this.mother.Name = "mother";
            this.mother.ReadOnly = true;
            // 
            // phased_paternal
            // 
            this.phased_paternal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.phased_paternal.HeaderText = "Phased Paternal";
            this.phased_paternal.Name = "phased_paternal";
            this.phased_paternal.ReadOnly = true;
            // 
            // phased_maternal
            // 
            this.phased_maternal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.phased_maternal.HeaderText = "Phased Maternal";
            this.phased_maternal.Name = "phased_maternal";
            this.phased_maternal.ReadOnly = true;
            // 
            // PhasingFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PhasingFrm";
            this.ShowInTaskbar = false;
            this.Text = "Phasing Utility";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbChild)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhasing)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbChild;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.ComponentModel.BackgroundWorker bwPhasing;
        private System.Windows.Forms.Button btnFather;
        private System.Windows.Forms.Button btnMother;
        private System.Windows.Forms.Button btnChild;
        private System.Windows.Forms.Button btnPhasing;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbFemale;
        private System.Windows.Forms.RadioButton rbMale;
        private System.Windows.Forms.DataGridView dgvPhasing;
        private System.Windows.Forms.DataGridViewTextBoxColumn rsid;
        private System.Windows.Forms.DataGridViewTextBoxColumn chromosome;
        private System.Windows.Forms.DataGridViewTextBoxColumn position;
        private System.Windows.Forms.DataGridViewTextBoxColumn child;
        private System.Windows.Forms.DataGridViewTextBoxColumn father;
        private System.Windows.Forms.DataGridViewTextBoxColumn mother;
        private System.Windows.Forms.DataGridViewTextBoxColumn phased_paternal;
        private System.Windows.Forms.DataGridViewTextBoxColumn phased_maternal;

    }
}
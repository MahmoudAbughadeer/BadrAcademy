namespace BadrAcademy.Forms.SubjectAssignments.Controls
{
    partial class ctrExamSchedulePage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.cbPeriod = new System.Windows.Forms.ComboBox();
            this.Period = new System.Windows.Forms.Label();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.cbSettings = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dgvExamSchedule = new System.Windows.Forms.DataGridView();
            this.label6 = new System.Windows.Forms.Label();
            this.cbLevels = new System.Windows.Forms.ComboBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.cbDepartments = new System.Windows.Forms.ComboBox();
            this.ctrWait1 = new BadrAcademy.Forms.Misc.Controls.ctrWait();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExamSchedule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.Controls.Add(this.btnEdit);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 750);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1260, 70);
            this.panel1.TabIndex = 1;
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnEdit.BackColor = System.Drawing.Color.White;
            this.btnEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEdit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdit.ForeColor = System.Drawing.Color.Black;
            this.btnEdit.Image = global::BadrAcademy.Properties.Resources.edit32;
            this.btnEdit.Location = new System.Drawing.Point(629, 10);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(139, 50);
            this.btnEdit.TabIndex = 4;
            this.btnEdit.Text = "تعديل   ";
            this.btnEdit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDelete.BackColor = System.Drawing.Color.White;
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.Image = global::BadrAcademy.Properties.Resources.delete32;
            this.btnDelete.Location = new System.Drawing.Point(493, 10);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(111, 50);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "حذف  ";
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1260, 321);
            this.panel2.TabIndex = 2;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label3);
            this.panel5.Controls.Add(this.pictureBox4);
            this.panel5.Controls.Add(this.cbDepartments);
            this.panel5.Controls.Add(this.pictureBox2);
            this.panel5.Controls.Add(this.label6);
            this.panel5.Controls.Add(this.cbLevels);
            this.panel5.Controls.Add(this.cbPeriod);
            this.panel5.Controls.Add(this.Period);
            this.panel5.Controls.Add(this.pictureBox6);
            this.panel5.Controls.Add(this.label4);
            this.panel5.Controls.Add(this.pictureBox3);
            this.panel5.Controls.Add(this.cbSettings);
            this.panel5.Controls.Add(this.label2);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel5.Location = new System.Drawing.Point(0, 213);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1260, 108);
            this.panel5.TabIndex = 3;
            // 
            // cbPeriod
            // 
            this.cbPeriod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPeriod.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbPeriod.FormattingEnabled = true;
            this.cbPeriod.Items.AddRange(new object[] {
            "لاشئ",
            "الأولى",
            "الثانية"});
            this.cbPeriod.Location = new System.Drawing.Point(78, 53);
            this.cbPeriod.Margin = new System.Windows.Forms.Padding(4);
            this.cbPeriod.Name = "cbPeriod";
            this.cbPeriod.Size = new System.Drawing.Size(155, 44);
            this.cbPeriod.TabIndex = 85;
            this.cbPeriod.SelectedIndexChanged += new System.EventHandler(this.cb_SelectedIndexChanged);
            // 
            // Period
            // 
            this.Period.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Period.AutoSize = true;
            this.Period.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Period.Location = new System.Drawing.Point(145, 11);
            this.Period.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Period.Name = "Period";
            this.Period.Size = new System.Drawing.Size(73, 36);
            this.Period.TabIndex = 86;
            this.Period.Text = "الفترة:";
            // 
            // pictureBox6
            // 
            this.pictureBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox6.Image = global::BadrAcademy.Properties.Resources.date32;
            this.pictureBox6.Location = new System.Drawing.Point(105, 13);
            this.pictureBox6.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(32, 32);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox6.TabIndex = 87;
            this.pictureBox6.TabStop = false;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(824, 13);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(149, 36);
            this.label4.TabIndex = 83;
            this.label4.Text = "العام الدراسى:";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.pictureBox3.Image = global::BadrAcademy.Properties.Resources.date32;
            this.pictureBox3.Location = new System.Drawing.Point(784, 15);
            this.pictureBox3.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(32, 32);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox3.TabIndex = 84;
            this.pictureBox3.TabStop = false;
            // 
            // cbSettings
            // 
            this.cbSettings.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cbSettings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbSettings.FormattingEnabled = true;
            this.cbSettings.Location = new System.Drawing.Point(674, 53);
            this.cbSettings.Margin = new System.Windows.Forms.Padding(4);
            this.cbSettings.Name = "cbSettings";
            this.cbSettings.Size = new System.Drawing.Size(395, 44);
            this.cbSettings.TabIndex = 73;
            this.cbSettings.SelectedIndexChanged += new System.EventHandler(this.cb_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Black", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(1076, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(181, 42);
            this.label2.TabIndex = 0;
            this.label2.Text = "بحث بواسطة:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel4
            // 
            this.panel4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.pictureBox1);
            this.panel4.Location = new System.Drawing.Point(450, 13);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(342, 176);
            this.panel4.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 130);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(327, 46);
            this.label1.TabIndex = 0;
            this.label1.Text = "ادارة جداول الامتحانات";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox1.Image = global::BadrAcademy.Properties.Resources.exam128;
            this.pictureBox1.Location = new System.Drawing.Point(110, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(128, 128);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.ctrWait1);
            this.panel3.Controls.Add(this.dgvExamSchedule);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 321);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.panel3.Size = new System.Drawing.Size(1260, 429);
            this.panel3.TabIndex = 3;
            // 
            // dgvExamSchedule
            // 
            this.dgvExamSchedule.AllowUserToAddRows = false;
            this.dgvExamSchedule.AllowUserToDeleteRows = false;
            this.dgvExamSchedule.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvExamSchedule.BackgroundColor = System.Drawing.Color.White;
            this.dgvExamSchedule.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvExamSchedule.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvExamSchedule.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvExamSchedule.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvExamSchedule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvExamSchedule.Location = new System.Drawing.Point(8, 0);
            this.dgvExamSchedule.MultiSelect = false;
            this.dgvExamSchedule.Name = "dgvExamSchedule";
            this.dgvExamSchedule.ReadOnly = true;
            this.dgvExamSchedule.RowHeadersWidth = 51;
            this.dgvExamSchedule.RowTemplate.Height = 24;
            this.dgvExamSchedule.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvExamSchedule.Size = new System.Drawing.Size(1247, 427);
            this.dgvExamSchedule.TabIndex = 8;
            this.dgvExamSchedule.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvExamSchedule_RowPostPaint);
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(555, 9);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(102, 36);
            this.label6.TabIndex = 89;
            this.label6.Text = "المستوي:";
            // 
            // cbLevels
            // 
            this.cbLevels.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cbLevels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLevels.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbLevels.FormattingEnabled = true;
            this.cbLevels.Location = new System.Drawing.Point(515, 53);
            this.cbLevels.Margin = new System.Windows.Forms.Padding(4);
            this.cbLevels.Name = "cbLevels";
            this.cbLevels.Size = new System.Drawing.Size(136, 44);
            this.cbLevels.TabIndex = 88;
            this.cbLevels.SelectedIndexChanged += new System.EventHandler(this.cb_SelectedIndexChanged);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.pictureBox2.Image = global::BadrAcademy.Properties.Resources.Level32;
            this.pictureBox2.Location = new System.Drawing.Point(515, 13);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(32, 32);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 90;
            this.pictureBox2.TabStop = false;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(355, 9);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 36);
            this.label3.TabIndex = 92;
            this.label3.Text = "الشعب:";
            // 
            // pictureBox4
            // 
            this.pictureBox4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.pictureBox4.Image = global::BadrAcademy.Properties.Resources.department32;
            this.pictureBox4.Location = new System.Drawing.Point(315, 11);
            this.pictureBox4.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(32, 32);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox4.TabIndex = 93;
            this.pictureBox4.TabStop = false;
            // 
            // cbDepartments
            // 
            this.cbDepartments.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cbDepartments.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDepartments.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbDepartments.FormattingEnabled = true;
            this.cbDepartments.Location = new System.Drawing.Point(252, 53);
            this.cbDepartments.Margin = new System.Windows.Forms.Padding(4);
            this.cbDepartments.Name = "cbDepartments";
            this.cbDepartments.Size = new System.Drawing.Size(246, 44);
            this.cbDepartments.TabIndex = 91;
            this.cbDepartments.SelectedIndexChanged += new System.EventHandler(this.cb_SelectedIndexChanged);
            // 
            // ctrWait1
            // 
            this.ctrWait1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ctrWait1.BackColor = System.Drawing.Color.White;
            this.ctrWait1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctrWait1.Location = new System.Drawing.Point(449, 169);
            this.ctrWait1.Margin = new System.Windows.Forms.Padding(6);
            this.ctrWait1.Name = "ctrWait1";
            this.ctrWait1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ctrWait1.Size = new System.Drawing.Size(358, 88);
            this.ctrWait1.SpinnerColor = BadrAcademy.Forms.Misc.Controls.ctrWait.enSpinnerColor.Black;
            this.ctrWait1.TabIndex = 5;
            this.ctrWait1.Visible = false;
            // 
            // ctrExamSchedulePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "ctrExamSchedulePage";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(1260, 820);
            this.Load += new System.EventHandler(this.dgvExamSchedule_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvExamSchedule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel4;
        private Forms.Misc.Controls.ctrWait ctrWait1;
        private System.Windows.Forms.DataGridView dgvExamSchedule;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbSettings;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.ComboBox cbPeriod;
        private System.Windows.Forms.Label Period;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbLevels;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.ComboBox cbDepartments;
    }
}

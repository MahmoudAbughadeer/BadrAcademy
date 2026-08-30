namespace BadrAcademy.Forms.Misc
{
    partial class frmHome
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHome));
            this.msHome = new System.Windows.Forms.MenuStrip();
            this.tsmiHome = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAcademicYear = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiLevels = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDepartments = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHalls = new System.Windows.Forms.ToolStripMenuItem();
            this.االبياناتالاكاديميةToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSubjects = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSubjectsAssignment = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDoctors = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmisTests = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExamScchedule = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDistributions = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiUsers = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAccount = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCurrentUserInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiChagnePassword = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.lblCurrentUserFullName = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblCurrentUsername = new System.Windows.Forms.Label();
            this.pPagePlace = new System.Windows.Forms.Panel();
            this.msHome.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // msHome
            // 
            this.msHome.BackColor = System.Drawing.Color.White;
            this.msHome.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.msHome.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.msHome.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiHome,
            this.tsmiSettings,
            this.االبياناتالاكاديميةToolStripMenuItem,
            this.tsmisTests,
            this.tsmiUsers,
            this.tsmiAccount});
            this.msHome.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.msHome.Location = new System.Drawing.Point(0, 0);
            this.msHome.Name = "msHome";
            this.msHome.Padding = new System.Windows.Forms.Padding(9, 2, 0, 2);
            this.msHome.Size = new System.Drawing.Size(1360, 72);
            this.msHome.TabIndex = 0;
            this.msHome.Text = "menuStrip1";
            // 
            // tsmiHome
            // 
            this.tsmiHome.ForeColor = System.Drawing.Color.Black;
            this.tsmiHome.Image = ((System.Drawing.Image)(resources.GetObject("tsmiHome.Image")));
            this.tsmiHome.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmiHome.Name = "tsmiHome";
            this.tsmiHome.Size = new System.Drawing.Size(170, 68);
            this.tsmiHome.Text = "الرئيسية";
            this.tsmiHome.Click += new System.EventHandler(this.tsmiHome_Click);
            // 
            // tsmiSettings
            // 
            this.tsmiSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAcademicYear,
            this.tsmiLevels,
            this.tsmiDepartments,
            this.tsmiHalls});
            this.tsmiSettings.ForeColor = System.Drawing.Color.Black;
            this.tsmiSettings.Image = ((System.Drawing.Image)(resources.GetObject("tsmiSettings.Image")));
            this.tsmiSettings.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmiSettings.Name = "tsmiSettings";
            this.tsmiSettings.Size = new System.Drawing.Size(184, 68);
            this.tsmiSettings.Text = "الإعدادات";
            // 
            // tsmiAcademicYear
            // 
            this.tsmiAcademicYear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.tsmiAcademicYear.ForeColor = System.Drawing.Color.White;
            this.tsmiAcademicYear.Image = global::BadrAcademy.Properties.Resources.settingsWhite32;
            this.tsmiAcademicYear.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmiAcademicYear.Name = "tsmiAcademicYear";
            this.tsmiAcademicYear.Size = new System.Drawing.Size(243, 38);
            this.tsmiAcademicYear.Text = "العام الدراسي";
            this.tsmiAcademicYear.Click += new System.EventHandler(this.tsmiAcademicYear_Click);
            // 
            // tsmiLevels
            // 
            this.tsmiLevels.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.tsmiLevels.ForeColor = System.Drawing.Color.White;
            this.tsmiLevels.Image = global::BadrAcademy.Properties.Resources.LevelWhite32;
            this.tsmiLevels.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmiLevels.Name = "tsmiLevels";
            this.tsmiLevels.Size = new System.Drawing.Size(243, 38);
            this.tsmiLevels.Text = "المستويات";
            this.tsmiLevels.Click += new System.EventHandler(this.tsmiLevels_Click);
            // 
            // tsmiDepartments
            // 
            this.tsmiDepartments.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.tsmiDepartments.ForeColor = System.Drawing.Color.White;
            this.tsmiDepartments.Image = global::BadrAcademy.Properties.Resources.departmentWhite32;
            this.tsmiDepartments.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmiDepartments.Name = "tsmiDepartments";
            this.tsmiDepartments.Size = new System.Drawing.Size(243, 38);
            this.tsmiDepartments.Text = "الشعب";
            this.tsmiDepartments.Click += new System.EventHandler(this.tsmiDepartments_Click);
            // 
            // tsmiHalls
            // 
            this.tsmiHalls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.tsmiHalls.ForeColor = System.Drawing.Color.White;
            this.tsmiHalls.Image = global::BadrAcademy.Properties.Resources.HallsWhite32;
            this.tsmiHalls.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmiHalls.Name = "tsmiHalls";
            this.tsmiHalls.Size = new System.Drawing.Size(243, 38);
            this.tsmiHalls.Text = "القاعات";
            this.tsmiHalls.Click += new System.EventHandler(this.tsmiHalls_Click);
            // 
            // االبياناتالاكاديميةToolStripMenuItem
            // 
            this.االبياناتالاكاديميةToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDoctors,
            this.tsmiSubjects,
            this.tsmiSubjectsAssignment});
            this.االبياناتالاكاديميةToolStripMenuItem.Image = global::BadrAcademy.Properties.Resources.acadimcData64;
            this.االبياناتالاكاديميةToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.االبياناتالاكاديميةToolStripMenuItem.Name = "االبياناتالاكاديميةToolStripMenuItem";
            this.االبياناتالاكاديميةToolStripMenuItem.Size = new System.Drawing.Size(263, 68);
            this.االبياناتالاكاديميةToolStripMenuItem.Text = "البيانات الاكاديمية";
            // 
            // tsmiSubjects
            // 
            this.tsmiSubjects.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.tsmiSubjects.ForeColor = System.Drawing.Color.White;
            this.tsmiSubjects.Image = global::BadrAcademy.Properties.Resources.SubjectWhite32;
            this.tsmiSubjects.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmiSubjects.Name = "tsmiSubjects";
            this.tsmiSubjects.Size = new System.Drawing.Size(257, 38);
            this.tsmiSubjects.Text = "المقررات";
            this.tsmiSubjects.Click += new System.EventHandler(this.tsmiSubjects_Click);
            // 
            // tsmiSubjectsAssignment
            // 
            this.tsmiSubjectsAssignment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.tsmiSubjectsAssignment.ForeColor = System.Drawing.Color.White;
            this.tsmiSubjectsAssignment.Image = global::BadrAcademy.Properties.Resources.SubjectWhite32;
            this.tsmiSubjectsAssignment.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmiSubjectsAssignment.Name = "tsmiSubjectsAssignment";
            this.tsmiSubjectsAssignment.Size = new System.Drawing.Size(257, 38);
            this.tsmiSubjectsAssignment.Text = "إسناد المقررات";
            this.tsmiSubjectsAssignment.Click += new System.EventHandler(this.tsmiSubjectsAssignment_Click);
            // 
            // tsmiDoctors
            // 
            this.tsmiDoctors.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.tsmiDoctors.ForeColor = System.Drawing.Color.White;
            this.tsmiDoctors.Image = global::BadrAcademy.Properties.Resources.doctorWhile32;
            this.tsmiDoctors.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmiDoctors.Name = "tsmiDoctors";
            this.tsmiDoctors.Size = new System.Drawing.Size(257, 38);
            this.tsmiDoctors.Text = "المدرسون";
            this.tsmiDoctors.Click += new System.EventHandler(this.tsmiDoctors_Click);
            // 
            // tsmisTests
            // 
            this.tsmisTests.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiExamScchedule,
            this.tsmiDistributions});
            this.tsmisTests.Image = global::BadrAcademy.Properties.Resources.tests64;
            this.tsmisTests.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmisTests.Name = "tsmisTests";
            this.tsmisTests.Size = new System.Drawing.Size(190, 68);
            this.tsmisTests.Text = "الامتحانات";
            // 
            // tsmiExamScchedule
            // 
            this.tsmiExamScchedule.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.tsmiExamScchedule.ForeColor = System.Drawing.Color.White;
            this.tsmiExamScchedule.Image = global::BadrAcademy.Properties.Resources.ScheduleWhile32;
            this.tsmiExamScchedule.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmiExamScchedule.Name = "tsmiExamScchedule";
            this.tsmiExamScchedule.Size = new System.Drawing.Size(273, 38);
            this.tsmiExamScchedule.Text = "جدول الإمتحانات";
            this.tsmiExamScchedule.Click += new System.EventHandler(this.tsmiExamScchedule_Click);
            // 
            // tsmiDistributions
            // 
            this.tsmiDistributions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.tsmiDistributions.ForeColor = System.Drawing.Color.White;
            this.tsmiDistributions.Image = global::BadrAcademy.Properties.Resources.DistributionsWhite32;
            this.tsmiDistributions.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmiDistributions.Name = "tsmiDistributions";
            this.tsmiDistributions.Size = new System.Drawing.Size(273, 38);
            this.tsmiDistributions.Text = "التوزيعات";
            this.tsmiDistributions.Click += new System.EventHandler(this.tsmiDistributions_Click);
            // 
            // tsmiUsers
            // 
            this.tsmiUsers.Image = global::BadrAcademy.Properties.Resources.users64;
            this.tsmiUsers.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmiUsers.Name = "tsmiUsers";
            this.tsmiUsers.Size = new System.Drawing.Size(216, 68);
            this.tsmiUsers.Text = "المستخدمون";
            this.tsmiUsers.Click += new System.EventHandler(this.tsmiUsers_Click);
            // 
            // tsmiAccount
            // 
            this.tsmiAccount.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCurrentUserInfo,
            this.tsmiChagnePassword,
            this.tsmiLogout});
            this.tsmiAccount.Image = ((System.Drawing.Image)(resources.GetObject("tsmiAccount.Image")));
            this.tsmiAccount.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmiAccount.Name = "tsmiAccount";
            this.tsmiAccount.Size = new System.Drawing.Size(168, 68);
            this.tsmiAccount.Text = "الحساب";
            // 
            // tsmiCurrentUserInfo
            // 
            this.tsmiCurrentUserInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.tsmiCurrentUserInfo.ForeColor = System.Drawing.Color.White;
            this.tsmiCurrentUserInfo.Image = global::BadrAcademy.Properties.Resources.userWhite64;
            this.tsmiCurrentUserInfo.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmiCurrentUserInfo.Name = "tsmiCurrentUserInfo";
            this.tsmiCurrentUserInfo.Size = new System.Drawing.Size(344, 38);
            this.tsmiCurrentUserInfo.Text = "بيانات المستخدم الحالى";
            this.tsmiCurrentUserInfo.Click += new System.EventHandler(this.tsmiCurrentUserInfo_Click);
            // 
            // tsmiChagnePassword
            // 
            this.tsmiChagnePassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.tsmiChagnePassword.ForeColor = System.Drawing.Color.White;
            this.tsmiChagnePassword.Image = global::BadrAcademy.Properties.Resources.ChangePasswordWhite32;
            this.tsmiChagnePassword.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmiChagnePassword.Name = "tsmiChagnePassword";
            this.tsmiChagnePassword.Size = new System.Drawing.Size(344, 38);
            this.tsmiChagnePassword.Text = "تغيير الرقم السرى";
            this.tsmiChagnePassword.Click += new System.EventHandler(this.tsmiChagnePassword_Click);
            // 
            // tsmiLogout
            // 
            this.tsmiLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.tsmiLogout.ForeColor = System.Drawing.Color.White;
            this.tsmiLogout.Image = global::BadrAcademy.Properties.Resources.logoutWhite32;
            this.tsmiLogout.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmiLogout.Name = "tsmiLogout";
            this.tsmiLogout.Size = new System.Drawing.Size(344, 38);
            this.tsmiLogout.Text = "تسجيل الخروج";
            this.tsmiLogout.Click += new System.EventHandler(this.tsmiLogout_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 72);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1360, 58);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel2.Controls.Add(this.pictureBox2);
            this.panel2.Controls.Add(this.lblCurrentUserFullName);
            this.panel2.Location = new System.Drawing.Point(365, 5);
            this.panel2.Name = "panel2";
            this.panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panel2.Size = new System.Drawing.Size(408, 47);
            this.panel2.TabIndex = 5;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.Image = global::BadrAcademy.Properties.Resources.ActiveUser32;
            this.pictureBox2.Location = new System.Drawing.Point(348, 9);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(43, 31);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // lblCurrentUserFullName
            // 
            this.lblCurrentUserFullName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCurrentUserFullName.Location = new System.Drawing.Point(14, 9);
            this.lblCurrentUserFullName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCurrentUserFullName.Name = "lblCurrentUserFullName";
            this.lblCurrentUserFullName.Size = new System.Drawing.Size(330, 31);
            this.lblCurrentUserFullName.TabIndex = 3;
            this.lblCurrentUserFullName.Text = "Full Name";
            this.lblCurrentUserFullName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.pictureBox1);
            this.panel3.Controls.Add(this.lblCurrentUsername);
            this.panel3.Location = new System.Drawing.Point(1118, 5);
            this.panel3.Name = "panel3";
            this.panel3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panel3.Size = new System.Drawing.Size(237, 47);
            this.panel3.TabIndex = 4;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = global::BadrAcademy.Properties.Resources.ActiveUser32;
            this.pictureBox1.Location = new System.Drawing.Point(182, 9);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(43, 31);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // lblCurrentUsername
            // 
            this.lblCurrentUsername.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCurrentUsername.Location = new System.Drawing.Point(13, 9);
            this.lblCurrentUsername.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCurrentUsername.Name = "lblCurrentUsername";
            this.lblCurrentUsername.Size = new System.Drawing.Size(165, 31);
            this.lblCurrentUsername.TabIndex = 3;
            this.lblCurrentUsername.Text = "Username";
            this.lblCurrentUsername.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pPagePlace
            // 
            this.pPagePlace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pPagePlace.Location = new System.Drawing.Point(0, 130);
            this.pPagePlace.Name = "pPagePlace";
            this.pPagePlace.Size = new System.Drawing.Size(1360, 572);
            this.pPagePlace.TabIndex = 2;
            // 
            // frmHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(244)))), ((int)(((byte)(248)))));
            this.ClientSize = new System.Drawing.Size(1360, 702);
            this.Controls.Add(this.pPagePlace);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.msHome);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.msHome;
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "frmHome";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmHome2_Load);
            this.msHome.ResumeLayout(false);
            this.msHome.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip msHome;
        private System.Windows.Forms.ToolStripMenuItem tsmiHome;
        private System.Windows.Forms.ToolStripMenuItem tsmiSettings;
        private System.Windows.Forms.ToolStripMenuItem tsmiAcademicYear;
        private System.Windows.Forms.ToolStripMenuItem tsmiLevels;
        private System.Windows.Forms.ToolStripMenuItem tsmiDepartments;
        private System.Windows.Forms.ToolStripMenuItem tsmiHalls;
        private System.Windows.Forms.ToolStripMenuItem االبياناتالاكاديميةToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiSubjects;
        private System.Windows.Forms.ToolStripMenuItem tsmiSubjectsAssignment;
        private System.Windows.Forms.ToolStripMenuItem tsmisTests;
        private System.Windows.Forms.ToolStripMenuItem tsmiExamScchedule;
        private System.Windows.Forms.ToolStripMenuItem tsmiDistributions;
        private System.Windows.Forms.ToolStripMenuItem tsmiUsers;
        private System.Windows.Forms.ToolStripMenuItem tsmiAccount;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblCurrentUserFullName;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblCurrentUsername;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel pPagePlace;
        private System.Windows.Forms.ToolStripMenuItem tsmiCurrentUserInfo;
        private System.Windows.Forms.ToolStripMenuItem tsmiChagnePassword;
        private System.Windows.Forms.ToolStripMenuItem tsmiLogout;
        private System.Windows.Forms.ToolStripMenuItem tsmiDoctors;
    }
}
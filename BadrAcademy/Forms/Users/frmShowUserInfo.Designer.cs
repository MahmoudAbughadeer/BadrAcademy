namespace BadrAcademy.Forms.Users
{
    partial class frmShowUserInfo
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
            this.lblOperationTitle = new System.Windows.Forms.Label();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.ctrUserCard1 = new BadrAcademy.Forms.Users.Controls.ctrUserCard();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            this.SuspendLayout();
            // 
            // lblOperationTitle
            // 
            this.lblOperationTitle.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOperationTitle.Location = new System.Drawing.Point(135, 145);
            this.lblOperationTitle.Name = "lblOperationTitle";
            this.lblOperationTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblOperationTitle.Size = new System.Drawing.Size(559, 46);
            this.lblOperationTitle.TabIndex = 54;
            this.lblOperationTitle.Text = "بيانات المستخدم";
            this.lblOperationTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox8
            // 
            this.pictureBox8.Image = global::BadrAcademy.Properties.Resources.UserInfo128;
            this.pictureBox8.Location = new System.Drawing.Point(350, 13);
            this.pictureBox8.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(128, 128);
            this.pictureBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox8.TabIndex = 53;
            this.pictureBox8.TabStop = false;
            // 
            // ctrUserCard1
            // 
            this.ctrUserCard1.BackColor = System.Drawing.Color.White;
            this.ctrUserCard1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctrUserCard1.Location = new System.Drawing.Point(16, 197);
            this.ctrUserCard1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.ctrUserCard1.Name = "ctrUserCard1";
            this.ctrUserCard1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ctrUserCard1.Size = new System.Drawing.Size(797, 174);
            this.ctrUserCard1.TabIndex = 55;
            // 
            // frmShowUserInfo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(828, 391);
            this.Controls.Add(this.ctrUserCard1);
            this.Controls.Add(this.lblOperationTitle);
            this.Controls.Add(this.pictureBox8);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmShowUserInfo";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.frmShowUserInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblOperationTitle;
        private System.Windows.Forms.PictureBox pictureBox8;
        private Controls.ctrUserCard ctrUserCard1;
    }
}
using BadrAcademy.Global;
using BadrAcademy.Helpers;
using BLL;
using BLL.Validation;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BadrAcademy.Forms.Misc
{
    public partial class frmLogin : Form
    {
        //Private Fields
        private bool _dragging = false;
        Point _dragCursorPoint;
        Point _dragFormPoint;
        clsTextBoxValidator _validator;

        //ctor
        public frmLogin()
        {
            InitializeComponent();
            InitializeFormDragging();
            _validator = new clsTextBoxValidator(epLogin);
        }


        //Form Events

        private void frmLogin_Load(object sender, EventArgs e)
        {
            _validator.Add(txtUsername, new clsRequiredValidator());
            _validator.Add(txtPassword, new clsRequiredValidator());

            LoadStoredCredintials();
        }
        
        private void frmLogin_MouseDown(object sender, MouseEventArgs e)
        {
            _dragging = true;
            _dragCursorPoint = Cursor.Position;//mouse position
            _dragFormPoint = this.Location;
        }

        private void frmLogin_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragging)
            {
                //Size chage the point to size for example if poin is 1000, 200 it turning into size of width 1000, hight 200
                //so this is diff between old mouse position and the new one
                Point diff = Point.Subtract(Cursor.Position, new Size(_dragCursorPoint));


                //Add thi diff size to this form location
                this.Location = Point.Add(_dragFormPoint, new Size(diff));
            }
        }

        private void frmLogin_MouseUp(object sender, MouseEventArgs e)
        {
            _dragging = false;
        }




        //Private Method
        private void InitializeFormDragging()
        {
            splitContainer1.Panel1.MouseDown += frmLogin_MouseDown;
            splitContainer1.Panel1.MouseMove += frmLogin_MouseMove;
            splitContainer1.Panel1.MouseUp += frmLogin_MouseUp;

            splitContainer1.Panel2.MouseDown += frmLogin_MouseDown;
            splitContainer1.Panel2.MouseMove += frmLogin_MouseMove;
            splitContainer1.Panel2.MouseUp += frmLogin_MouseUp;

            pbLoginIcon.MouseDown += frmLogin_MouseDown;
            pbLoginIcon.MouseMove += frmLogin_MouseMove;
            pbLoginIcon.MouseUp += frmLogin_MouseUp;

            pbLoginGIF.MouseDown += frmLogin_MouseDown;
            pbLoginGIF.MouseMove += frmLogin_MouseMove;
            pbLoginGIF.MouseUp += frmLogin_MouseUp;

            lblLoginTitle.MouseDown += frmLogin_MouseDown;
            lblLoginTitle.MouseMove += frmLogin_MouseMove;
            lblLoginTitle.MouseUp += frmLogin_MouseUp;

            lblLoginToYourAccount.MouseDown += frmLogin_MouseDown;
            lblLoginToYourAccount.MouseMove += frmLogin_MouseMove;
            lblLoginToYourAccount.MouseUp += frmLogin_MouseUp;
        }
        private void HandelRememberCredentials()
        {
            if (CheckBoxSaveCredentials.Checked)
                clsCredentials.RememberUsernameAndPassword(txtUsername.Text.Trim(), txtPassword.Text.Trim());
            else
                clsCredentials.RememberUsernameAndPassword(null, null);
        }
        private void LoadStoredCredintials()
        {
            clsCredentials.GetStoredCredentials(out string username, out string password);
            txtUsername.Text = username;
            txtPassword.Text = password;
        }


        //Buttons Events
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (!_validator.AreAllFieldsValid())
            {
                clsMessageBoxHelper.ShowWarning("بعض الحقول غير صحيحة، ضع مؤشر الماوس فوق الأيقونات الحمراء لمعرفة الخطأ");
                return;
            }

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            clsGlobal.CurrentUser = await clsAsyncMethodExecutor.RunWithWaitAsync<clsUser>(
                ctrWait1, () => clsUser.FindByCredentialsAsync(username, password));

            if (clsGlobal.CurrentUser == null)
            {
                MessageBox.Show("!!!اسم المستخدم أو كلمة المرور غير صحيحة", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!clsGlobal.CurrentUser.IsActive)
            {
                MessageBox.Show("هذا الحساب غير مفعل برجاء التواصل مع المشرف", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            HandelRememberCredentials();
            this.Hide(); //Hide login form

            frmHome home = new frmHome();
            DialogResult result = home.ShowDialog();
            if (result == DialogResult.OK)//Flag sign out button(so if dialog result is ok that means user press sign out button (not press x to close the form))
            {
                home.Dispose();
                LoadStoredCredintials();
                this.Show();//reshowing login forms
            }
            else
                this.Close();
        }
    }
}

using BLL;
using BLL.Core;
using BLL.Validation;
using BadrAcademy.Helpers;
using BadrAcademy.Global;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using BLL.Utilities;

namespace BadrAcademy.Forms.Users
{
    public partial class frmAddEditUser : Form
    {
        
        enMode _mode;
        int _userID;
        clsUser _user;
        clsTextBoxValidator _formValidator;


        public frmAddEditUser()
        {
            InitializeComponent();
            _mode = enMode.Add;
            _user = null;
        }

        public frmAddEditUser(int userID)
        {
            InitializeComponent();
            _mode = enMode.Update;
            _userID = userID;
        }



        //Events

        public delegate Task SaveCompletedAsync();

        public event SaveCompletedAsync SaveCompleted;

        private async void frmAddEditUser_Load(object sender, EventArgs e)
        {
            _formValidator = new clsTextBoxValidator(epUser);
            SetupValidation();

            if(_mode == enMode.Update)
            {
                _user = await clsAsyncMethodExecutor.RunWithWaitAsync<clsUser>(ctrWait1, () => clsUser.FindAsync(_userID));
                ChangeFormTitleToUpdate();
                DiplayUserData();
                DisableCredentials();

            }       
        }



        //Private Methods


        private void DisableCredentials()
        {
            txtUsername.Enabled = false;
            txtPassword.Text = "";
            txtPassword.Enabled = false;
        }

        private void SetupValidation()
        {
            _formValidator.Clear();

            if (_mode == enMode.Add)
            {
                _formValidator.Add(txtUsername, new clsUsernameValidator(_user));
                _formValidator.Add(txtPassword, new clsRequiredValidator());
            }

        }

        private void LoadUserInfoFromFormToUserObject()
        {
            string name = txtFullName.Text.Trim();
            string userName = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            bool isActive = rbActive.Checked;

            if (_mode == enMode.Add)
            {
                _user = new clsUser(name, userName, password, isActive);
                return;
            }

            _user.FullName = name;
            _user.Username = userName;
            _user.IsActive = isActive;
        }

        private void DiplayUserData()
        {
            txtFullName.Text = _user.FullName.Trim();
            lblUserID.Text = _user.UserID.ToString().Trim();
            txtUsername.Text = _user.Username.Trim();

            if (_user.IsActive)
                rbActive.Checked = true;
            else
                rbNotActive.Checked = true;
        }

        private void ChangeFormTitleToUpdate()
        {
            lblOperationTitle.Text = "تعديل بيانات المستخدم";
        }

        private bool IsUserDataChanged()
        {
            return
                _user.FullName.Trim() != txtFullName.Text.Trim() ||
                _user.IsActive != rbActive.Checked;

        }



        //Buttons Events
        private async void btnSave_Click(object sender, EventArgs e)
        {

            if (!_formValidator.AreAllFieldsValid())
            {
                clsMessageBoxHelper.ShowWarning("بعض الحقول غير صحيحة، ضع مؤشر الماوس فوق الأيقونات الحمراء لمعرفة الخطأ");
                return;
            }

            if (_mode == enMode.Update && !IsUserDataChanged())
            {
                clsMessageBoxHelper.ShowWarning("لم يتم إجراء اى تعديلات");
                return;
            }



            LoadUserInfoFromFormToUserObject();
            if (!await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, () => _user.SaveAsync()))
            {

                clsMessageBoxHelper.ShowWarning("حدث خطأ أثناء محاولة حفظ بيانات المستخدم");
                return;
            }

            lblUserID.Text = _user.UserID.ToString();

            await SaveCompleted?.Invoke();

            if (_mode == enMode.Add)
            {
                _mode = enMode.Update;
                ChangeFormTitleToUpdate();
                DisableCredentials();
                SetupValidation();
            }

            clsMessageBoxHelper.ShowInfo("تم حفظ البيانات بنجاح");

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}








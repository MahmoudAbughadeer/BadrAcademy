using BLL;
using BLL.Validation;
using BadrAcademy.Helpers;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BadrAcademy.Forms.Users
{
    public partial class frmChangeUserPassword : Form
    {
        private int _userId;
        private clsTextBoxValidator _validator;
        public frmChangeUserPassword(int userId)
        {
            InitializeComponent();
            _validator = new clsTextBoxValidator(epChangePassword);
            _userId = userId;
        }


        //Form events
        private async void frmChangeUserPassword_Load(object sender, EventArgs e)
        {
            await ctrUserCard1.LoadInfo(_userId);
            _validator.Add(txtCurrentPassword, new clsPasswordValidator(ctrUserCard1.User));
            _validator.Add(txtNewPassword, new clsRequiredValidator());
        }


        //Buttons Events
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_validator.AreAllFieldsValid())
                {
                    clsMessageBoxHelper.ShowWarning("بعض الحقول غير صحيحة، ضع مؤشر الماوس فوق الأيقونات الحمراء لمعرفة الخطأ");
                    return;
                }

                if (MessageBox.Show("هل انت متأكد من تغيير الرقم السري؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1) == DialogResult.No)
                    return;

                if (!await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, () => clsUser.ChangePasswordAsync(_userId, txtNewPassword.Text.Trim())))
                {
                    clsMessageBoxHelper.ShowWarning("حدث خطأ أثناء محاولة حفظ بيانات المستخدم");
                    return;
                }

                clsMessageBoxHelper.ShowInfo("تم نغيير الرقم السري بنجاح");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        //TextBoxes Validating events
        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtConfirmPassword.Text.Trim() != txtNewPassword.Text.Trim())
                epChangePassword.SetError(txtConfirmPassword, "رقم سري غير متطابق");
            else
                epChangePassword.SetError(txtConfirmPassword, "");
        }

       
    }
}

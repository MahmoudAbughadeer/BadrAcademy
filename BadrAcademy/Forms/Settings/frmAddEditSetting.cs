using BadrAcademy.Helpers;
using BLL;
using BLL.Core;
using BLL.Validation;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BadrAcademy.Forms.Settings
{
    public partial class frmAddEditSetting : Form
    {
        
        enMode _mode;
        int _settingID;
        clsSetting _setting;
        clsTextBoxValidator _formValidator;


        public frmAddEditSetting()
        {
            InitializeComponent();
            _mode = enMode.Add;
            _setting = null;
        }

        public frmAddEditSetting(int settingID)
        {
            InitializeComponent();
            _mode = enMode.Update;
            _settingID = settingID;
        }



        //Events

        public delegate Task SaveCompletedAsync();

        public event SaveCompletedAsync SaveCompleted;

        private async void frmAddEditUser_Load(object sender, EventArgs e)
        {

            //Set combo boxeds
            cbSemester.SelectedIndex = 0;
            cbSemesterType.SelectedIndex = 0;

            //set validation
            _formValidator = new clsTextBoxValidator(epSetting);
            SetupValidation();


            //Handel update mode
            if(_mode == enMode.Update)
            {
                _setting = await clsAsyncMethodExecutor.RunWithWaitAsync<clsSetting>(ctrWait1, () => clsSetting.FindAsync(_settingID));
                ChangeFormTitleToUpdate();
                DiplaySettingData();
            }       
        }



        //Private Methods

        private void SetupValidation()
        {
            _formValidator.Clear();

            if (_mode == enMode.Add)
            {
                _formValidator.Add(txtAcademicYear, new clsRequiredValidator());
                _formValidator.Add(txtDefAnswerForm, new clsRequiredValidator());
            }

        }

        private void LoadSettingInfoFromFormToSettingObject()
        {
            string academicYear = txtAcademicYear.Text.Trim();
            string semester = cbSemester.Text.Trim();
            string semesterType = cbSemesterType.Text.Trim();
            string defAnswerForm = txtDefAnswerForm.Text.Trim();

            if (_mode == enMode.Add)
            {
                _setting = new clsSetting(academicYear, semester, semesterType, defAnswerForm);
                return;
            }

            _setting.AcademicYear = academicYear;
            _setting.Semester = semester;
            _setting.SemesterType = semesterType;
            _setting.DefaultAnswerForms = defAnswerForm;
        }

        private void DiplaySettingData()
        {
            lblSettingID.Text = _setting.SettingID.ToString();
            txtAcademicYear.Text = _setting.AcademicYear.Trim();
            cbSemester.Text = _setting.Semester.Trim();
            cbSemesterType.Text = _setting.SemesterType.Trim();
            txtDefAnswerForm.Text = _setting.DefaultAnswerForms.Trim();
        }

        private void ChangeFormTitleToUpdate()
        {
            lblOperationTitle.Text = "تعديل اعدادات العام الدراسي";
        }

        private bool IsSettingChanged()
        {
            return
                _setting.AcademicYear.Trim() != txtAcademicYear.Text.Trim() ||
                _setting.Semester != cbSemester.Text.Trim() ||
                _setting.SemesterType != cbSemesterType.Text.Trim() ||
                _setting.DefaultAnswerForms != txtDefAnswerForm.Text.Trim();
        }



        //Buttons Events
        private async void btnSave_Click(object sender, EventArgs e)
        {

            if (!_formValidator.AreAllFieldsValid())
            {
                clsMessageBoxHelper.ShowWarning("بعض الحقول غير صحيحة، ضع مؤشر الماوس فوق الأيقونات الحمراء لمعرفة الخطأ");
                return;
            }

            if (_mode == enMode.Update && !IsSettingChanged())
            {
                clsMessageBoxHelper.ShowWarning("لم يتم إجراء اى تعديلات");
                return;
            }



            LoadSettingInfoFromFormToSettingObject();
            if (!await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, () => _setting.SaveAsync()))
            {

                clsMessageBoxHelper.ShowWarning("حدث خطأ أثناء محاولة حفظ الاعدادات");
                return;
            }

            lblSettingID.Text = _setting.SettingID.ToString();

            await SaveCompleted?.Invoke();

            if (_mode == enMode.Add)
            {
                _mode = enMode.Update;
                ChangeFormTitleToUpdate();
            }

            clsMessageBoxHelper.ShowInfo("تم حفظ البيانات بنجاح");

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        //combo box events
        private void cbSemesterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSemesterType.SelectedIndex == 0)
                txtDefAnswerForm.Text = "أ";
            else
                txtDefAnswerForm.Text = "أ / ب";


        }
    }
}








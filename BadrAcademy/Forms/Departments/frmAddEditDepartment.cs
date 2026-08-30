using BadrAcademy.Helpers;
using BLL;
using BLL.Core;
using BLL.Validation;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BadrAcademy.Forms.Departments
{
    public partial class frmAddEditDepartment : Form
    {
        
        enMode _mode;
        int DepartmentID;
        clsDepartment _Department;
        clsTextBoxValidator _formValidator;
        public frmAddEditDepartment()
        {
            InitializeComponent();
            _mode = enMode.Add;
            _Department = null;
        }

        public frmAddEditDepartment(int departmentID)
        {
            InitializeComponent();
            _mode = enMode.Update;
            DepartmentID = departmentID;
        }



        //Events

        public delegate Task SaveCompletedAsync();

        public event SaveCompletedAsync SaveCompleted;

        private async void frmAddEditDepartment_Load(object sender, EventArgs e)
        {
            _formValidator = new clsTextBoxValidator(epDepartment);
            SetupValidation();

            //Handel update mode
            if(_mode == enMode.Update)
            {
                _Department = await clsAsyncMethodExecutor.RunWithWaitAsync<clsDepartment>(ctrWait1, () => clsDepartment.FindAsync(DepartmentID));
                ChangeFormTitleToUpdate();
                DiplayDepartmentData();
            }       
        }



        //Private Methods

        private void SetupValidation()
        {
            _formValidator.Clear();
            _formValidator.Add(txtDepartmentName, new clsRequiredValidator());
        }
        private void LoadSettingInfoFromFormToSettingObject()
        {

            string departmentName = txtDepartmentName.Text.Trim();
            string description = rtxtDescription.Text.Trim();

            if (_mode == enMode.Add)
            {
                _Department = new clsDepartment(departmentName, description);
                return;
            }

            _Department.DepartmentName = departmentName;
            _Department.Description = description;
        }

        private void DiplayDepartmentData()
        {
            lblDepartmentID.Text = _Department.DepartmentID.ToString();
            txtDepartmentName.Text = _Department.DepartmentName.Trim();
            rtxtDescription.Text = _Department.Description;
        }

        private void ChangeFormTitleToUpdate()
        {
            lblOperationTitle.Text = "تعديل اعدادات القسم";
        }

        private bool IsLevelDataChanged()
        {
            return _Department.DepartmentName.Trim() != txtDepartmentName.Text.Trim() ||
                _Department.Description.Trim() != rtxtDescription.Text.Trim();
        }



        //Buttons Events
        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!_formValidator.AreAllFieldsValid())
            {
                clsMessageBoxHelper.ShowWarning("بعض الحقول غير صحيحة، ضع مؤشر الماوس فوق الأيقونات الحمراء لمعرفة الخطأ");
                return;
            }

            if (_mode == enMode.Update && !IsLevelDataChanged())
            {
                clsMessageBoxHelper.ShowWarning("لم يتم إجراء اى تعديلات");
                return;
            }



            LoadSettingInfoFromFormToSettingObject();
            if (!await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, () => _Department.SaveAsync()))
            {

                clsMessageBoxHelper.ShowWarning("حدث خطأ أثناء محاولة حفظ بيانات القسم");
                return;
            }

            lblDepartmentID.Text = _Department.DepartmentID.ToString();

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

    }
}








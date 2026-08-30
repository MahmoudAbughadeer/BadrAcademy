using BadrAcademy.Helpers;
using BLL;
using BLL.Core;
using BLL.Validation;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BadrAcademy.Forms.Doctors
{
    public partial class frmAddEditDoctor : Form
    {
        enMode _mode;
        int _doctorID;
        clsDoctor _Doctor;
        clsTextBoxValidator _formValidator;
        public frmAddEditDoctor()
        {
            InitializeComponent();
            _mode = enMode.Add;
            _Doctor = null;
        }

        public frmAddEditDoctor(int doctorID)
        {
            InitializeComponent();
            _mode = enMode.Update;
            _doctorID = doctorID;
        }



        //Events

        public delegate Task SaveCompletedAsync();

        public event SaveCompletedAsync SaveCompleted;

        private async void frmAddEditDoctor_Load(object sender, EventArgs e)
        {
            _formValidator = new clsTextBoxValidator(epDoctor);
            SetupValidation();
            await FillComoboxWithDepartments();

            //Handel update mode
            if (_mode == enMode.Update)
            {
                _Doctor = await clsAsyncMethodExecutor.RunWithWaitAsync<clsDoctor>(ctrWait1, () => clsDoctor.FindAsync(_doctorID));
                ChangeFormTitleToUpdate();
                DiplayDoctorData();
            }       
        }



        //Private Methods


        private async Task FillComoboxWithDepartments()
        {
            cbDepartment.DataSource = await clsDepartment.GetAllAsync();
            cbDepartment.ValueMember = "DepartmentID";
            cbDepartment.DisplayMember = "DepartmentName";
        }

        private void SetupValidation()
        {
            _formValidator.Clear();
            _formValidator.Add(txtDoctorName, new clsRequiredValidator());
        }
        private void LoadSettingInfoFromFormToDoctorObject()
        {

            string doctorName = txtDoctorName.Text.Trim();
            int departmentID = Convert.ToInt32(cbDepartment.SelectedValue);

            if (_mode == enMode.Add)
            {
                _Doctor = new clsDoctor(doctorName, departmentID);
                return;
            }

            _Doctor.DoctorName = doctorName;
            _Doctor.DepartmentID = departmentID;
        }

        private void DiplayDoctorData()
        {
            lblDoctorID.Text = _Doctor.DepartmentID.ToString();
            txtDoctorName.Text = _Doctor.DoctorName.Trim();
            cbDepartment.SelectedValue = _Doctor.DepartmentID;
        }

        private void ChangeFormTitleToUpdate()
        {
            lblOperationTitle.Text = "تعديل معلومات المدرس";
        }

        private bool IsDoctorDataChanged()
        {
            return _Doctor.DoctorName.Trim() != txtDoctorName.Text.Trim() ||
                _Doctor.DepartmentID != Convert.ToInt32(cbDepartment.SelectedValue);
        }



        //Buttons Events
        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!_formValidator.AreAllFieldsValid())
            {
                clsMessageBoxHelper.ShowWarning("بعض الحقول غير صحيحة، ضع مؤشر الماوس فوق الأيقونات الحمراء لمعرفة الخطأ");
                return;
            }

            if (_mode == enMode.Update && !IsDoctorDataChanged())
            {
                clsMessageBoxHelper.ShowWarning("لم يتم إجراء اى تعديلات");
                return;
            }



            LoadSettingInfoFromFormToDoctorObject();
            if (!await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, () => _Doctor.SaveAsync()))
            {

                clsMessageBoxHelper.ShowWarning("حدث خطأ أثناء محاولة حفظ بيانات المدرس");
                return;
            }

            lblDoctorID.Text = _Doctor.DoctorID.ToString();

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








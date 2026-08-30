using BadrAcademy.Helpers;
using BLL;
using BLL.Core;
using BLL.Validation;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BadrAcademy.Forms.Halls
{
    public partial class frmAddEditHall : Form
    {
        
        enMode _mode;
        int _hallID;
        clsHall _hall;
        clsTextBoxValidator _formValidator;


        public frmAddEditHall()
        {
            InitializeComponent();
            _mode = enMode.Add;
            _hall = null;
        }

        public frmAddEditHall(int hallID)
        {
            InitializeComponent();
            _mode = enMode.Update;
            _hallID = hallID;
        }



        //Events

        public delegate Task SaveCompletedAsync();

        public event SaveCompletedAsync SaveCompleted;

        private async void frmAddEditUser_Load(object sender, EventArgs e)
        {

            //Set combo boxeds
            cbHallType.SelectedIndex = 0;

            //set validation
            _formValidator = new clsTextBoxValidator(epHall);
            SetupValidation();


            //Handel update mode
            if(_mode == enMode.Update)
            {
                _hall = await clsAsyncMethodExecutor.RunWithWaitAsync<clsHall>(ctrWait1, () => clsHall.FindAsync(_hallID));
                ChangeFormTitleToUpdate();
                DiplayHallData();
            }       
        }



        //Private Methods

        private void SetupValidation()
        {
            _formValidator.Clear();
            _formValidator.Add(txtHallName, new clsRequiredValidator());
        }

        private void LoadSettingInfoFromFormToSHallObject()
        {

            string hallName = txtHallName.Text.Trim();
            string hallType = cbHallType.Text.Trim();
            int defaultCapacity = Convert.ToInt32(nDefaultCapacity.Value);
            bool isActive = rbActive.Checked;

            if (_mode == enMode.Add)
            {
                _hall = new clsHall(hallName, hallName, defaultCapacity, isActive);
                return;
            }

            _hall.HallName = hallName;
            _hall.HallType = hallType;
            _hall.DefaultCapacity = defaultCapacity;
            _hall.IsActive = isActive;
        }

        private void DiplayHallData()
        {
            lblHallID.Text = _hall.HallID.ToString();
            txtHallName.Text = _hall.HallName.Trim();
            cbHallType.Text = _hall.HallType.Trim();
            nDefaultCapacity.Value = _hall.DefaultCapacity;

            if (_hall.IsActive)
                rbActive.Checked = true;
            else
                rbNotActive.Checked = true;
        }

        private void ChangeFormTitleToUpdate()
        {
            lblOperationTitle.Text = "تعديل اعدادات القاعة";
        }

        private bool IsHallDataChanged()
        {
            return
                _hall.HallName.Trim() != txtHallName.Text.Trim() ||
                _hall.HallType != cbHallType.Text.Trim() ||
                _hall.DefaultCapacity != nDefaultCapacity.Value ||
                _hall.IsActive != rbActive.Checked;
        }



        //Buttons Events
        private async void btnSave_Click(object sender, EventArgs e)
        {

            if (!_formValidator.AreAllFieldsValid())
            {
                clsMessageBoxHelper.ShowWarning("بعض الحقول غير صحيحة، ضع مؤشر الماوس فوق الأيقونات الحمراء لمعرفة الخطأ");
                return;
            }

            if (_mode == enMode.Update && !IsHallDataChanged())
            {
                clsMessageBoxHelper.ShowWarning("لم يتم إجراء اى تعديلات");
                return;
            }



            LoadSettingInfoFromFormToSHallObject();
            if (!await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, () => _hall.SaveAsync()))
            {

                clsMessageBoxHelper.ShowWarning("حدث خطأ أثناء محاولة حفظ بيانات القاعة");
                return;
            }

            lblHallID.Text = _hall.HallID.ToString();

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








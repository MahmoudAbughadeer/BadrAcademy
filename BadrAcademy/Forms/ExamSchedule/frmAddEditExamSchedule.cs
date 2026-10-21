using BadrAcademy.Helpers;
using BLL;
using BLL.Core;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;


namespace BadrAcademy.Forms.ExamSchedule
{
    public partial class frmAddEditExamSchedule : Form
    {
       

        enMode _mode;
        int _offeringID;
        clsExamSchedule _examSchedule;

        public frmAddEditExamSchedule(int offeringID, bool inUpdateMode)
        {
            InitializeComponent();
            _mode = inUpdateMode? enMode.Update : enMode.Add;
            _offeringID = offeringID;
        }



        //Events

        public delegate Task SaveCompletedAsync();

        public event SaveCompletedAsync SaveCompleted;

        private async void frmAddEditExamSchedule_Load(object sender, EventArgs e)
        {

            await ctrSubjectOfferingCard1.LoadInfo(_offeringID);

            //Set combo boxeds
            cbPeriod.SelectedIndex = 0;


            cbSettings.DataSource =  await clsSetting.GetAllForComboBoxAsync();
            cbSettings.ValueMember = "SettingID";
            cbSettings.DisplayMember = "SettingDetails";
            cbSettings.SelectedIndex = 0;


            lblExamDayName.Text = GetDayNameByDate(dtpExamDate.Value);

            //Handel update mode
            if (_mode == enMode.Update)
            {
                _examSchedule = await clsAsyncMethodExecutor.RunWithWaitAsync<clsExamSchedule>(ctrWait1, () => clsExamSchedule.FindByOfferingIdAsync(_offeringID));
                ChangeFormTitleToUpdate();
                DisplayExamSchedule();
            }
        }



        //Private Methods
        private void LoadExamScheduleInfoFromFormToExamScheduleObject()
        {
            
            string examDayName = lblExamDayName.Text.Trim();
            DateTime examDate = dtpExamDate.Value;
            DateTime timeFrom = dtpTimeFrom.Value;
            DateTime timeTo = dtpTimeTo.Value;
            string period = cbPeriod.Text;
            int settingID = Convert.ToInt32(cbSettings.SelectedValue);

            if (_mode == enMode.Add)
            {
                _examSchedule = new clsExamSchedule(_offeringID, examDayName, examDate, timeFrom, timeTo, period, settingID);
                return;
            }

            _examSchedule.ExamDayName = examDayName;
            _examSchedule.ExamDate = examDate;
            _examSchedule.TimeFrom = timeFrom;
            _examSchedule.TimeTo = timeTo;
            _examSchedule.Period = period;
            _examSchedule.SettingID = settingID;
            
        }

        private void DisplayExamSchedule()
        {
            lblExamDayName.Text = _examSchedule.ExamDayName;
            dtpExamDate.Value = _examSchedule.ExamDate;
            dtpTimeFrom.Value = _examSchedule.TimeFrom;
            dtpTimeTo.Value = _examSchedule.TimeTo;
            cbPeriod.Text = _examSchedule.Period;
            cbSettings.SelectedValue = _examSchedule.SettingID;
            
        }

        private void ChangeFormTitleToUpdate()
        {
            lblOperationTitle.Text = "تعديل جدول امتحان";
        }

        private bool IsExamScheduleChanged()
        {
            return
                _examSchedule.ExamDayName.Trim() != lblExamDayName.Text.Trim() ||
                _examSchedule.ExamDate != dtpExamDate.Value ||
                _examSchedule.TimeFrom != dtpTimeFrom.Value ||
                _examSchedule.TimeTo != dtpTimeTo.Value ||
                _examSchedule.Period != cbPeriod.Text ||
                _examSchedule.SettingID != Convert.ToInt32(cbSettings.SelectedValue);
        }

        private string GetDayNameByDate(DateTime date)
        {
            return date.ToString("dddd", new CultureInfo("ar-EG")).Trim();
        }

        //Buttons Events
        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (_mode == enMode.Update && !IsExamScheduleChanged())
            {
                clsMessageBoxHelper.ShowWarning("لم يتم إجراء اى تعديلات");
                return;
            }

            LoadExamScheduleInfoFromFormToExamScheduleObject();

            if (!await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, () => _examSchedule.SaveAsync()))
            {

                clsMessageBoxHelper.ShowWarning("حدث خطأ أثناء محاولة حفظ بيانات جدول الامتحان");
                return;
            }

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


        //DateTimePicker Events
        private void dtpExamDate_ValueChanged(object sender, EventArgs e)
        {
            lblExamDayName.Text = GetDayNameByDate(dtpExamDate.Value);
        }
    
    
    }
}








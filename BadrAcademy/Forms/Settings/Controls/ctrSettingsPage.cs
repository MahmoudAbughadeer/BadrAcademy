using BLL;
using BadrAcademy.Helpers;
using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BadrAcademy.Forms.Settings.Controls
{
    public partial class ctrSettingsPage : UserControl
    {
        //Private fields

        private BindingSource _bindingSource = new BindingSource();


        //Constructors
        public ctrSettingsPage()
        {
            InitializeComponent();
        }


        //DataGridView Events
      
       
        private void dgvSettings_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            dgvSettigns.Rows[e.RowIndex].Cells["RowNumber"].Value = e.RowIndex + 1;
        }

        //Events
        private async void ctrSettingsPage_Load(object sender, EventArgs e)
        {
            InitializeDataGrideViewColumns();//Here I add the columns manually
            dgvSettigns.AutoGenerateColumns = false;//this is adding columnss/property name that come from database

            //change the font size and adjust row height
            dgvSettigns.Font = new Font("Arial", 14);
            dgvSettigns.RowTemplate.Height = 35;
            dgvSettigns.DefaultCellStyle.Padding = new Padding(5);

            await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadDataAsync);
        }


        //Private Methods
        private async Task LoadDataAsync()
        {
            _bindingSource.DataSource = null;

            DataTable settings = await clsSetting.GetAllAsync();

            _bindingSource.DataSource = settings;
            dgvSettigns.DataSource = _bindingSource;
            clsButtonHelper.SetEditDelteButtonsState(dgvSettigns, btnEdit, btnDelete);
        }

        private void InitializeDataGrideViewColumns()
        {
            dgvSettigns.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "RowNumber",
                DataPropertyName = "RowNumber",
                HeaderText = "م",
                FillWeight = 10,
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            dgvSettigns.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SettingID",
                DataPropertyName = "SettingID",
                HeaderText = "المعرف",
                FillWeight = 15
            });

            dgvSettigns.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "AcademicYear",
                DataPropertyName = "AcademicYear",
                HeaderText = "العام الدراسي",
                FillWeight = 20
            });

            dgvSettigns.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Semester",
                DataPropertyName = "Semester",
                HeaderText = "الفصل",
                FillWeight = 20
            });


            dgvSettigns.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SemesterType",
                DataPropertyName = "SemesterType",
                HeaderText = "نوع الفصل",
                FillWeight = 20
            });

            dgvSettigns.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DefaultAnswerForms",
                DataPropertyName = "DefaultAnswerForms",
                HeaderText = "نموذج الاجابة",
                FillWeight = 15
            });
        }
        
      
        private async Task AddEditSetting_SaveCompletedAsync()
        {
            await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadDataAsync);
        }



        //Buttons Events
        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddEditSetting frm = new frmAddEditSetting();
            frm.SaveCompleted += AddEditSetting_SaveCompletedAsync; //will not be block you event is Task and awaitable
            frm.ShowDialog();

            frm.SaveCompleted -= AddEditSetting_SaveCompletedAsync;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int currentUserID = clsDataGridViewHelper.GetCellValueAsInt(this.dgvSettigns, "SettingID");
            frmAddEditSetting frm = new frmAddEditSetting(currentUserID);

            frm.SaveCompleted += AddEditSetting_SaveCompletedAsync;//will not be block you event is Task and awaitable
            frm.ShowDialog();
            frm.SaveCompleted -= AddEditSetting_SaveCompletedAsync;
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            int currentSettingsID = clsDataGridViewHelper.GetCellValueAsInt(this.dgvSettigns, "SettingID");

            if (clsMessageBoxHelper.ShowConfirmation("هل أنت متأكد من حذف هذا الاعداد؟") == DialogResult.Cancel)
                return;

            if (await clsAsyncMethodExecutor.RunWithWaitAsync<bool>(ctrWait1, () => clsSetting.DeleteAsync(currentSettingsID)))
            {
                await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadDataAsync);
                clsMessageBoxHelper.ShowInfo("تم حذف بيانات الاعداد");         
            }
            else
                clsMessageBoxHelper.ShowError("حدث خطأ عند محاولة حذف بيانات الاعداد");
        }    
    }
}

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
        private int _currentPage = 1;

        private const int _pageSize = 20;

        private bool _isLoading = false;

        private bool _hasMoreData = true;

        private BindingSource _bindingSource = new BindingSource();


        //Constructors
        public ctrSettingsPage()
        {
            InitializeComponent();
        }


        //DataGridView Events
        private async void dgvSettings_Scroll(object sender, ScrollEventArgs e)
        {
            if (_isLoading || !_hasMoreData)
                return;

            if (IsNearBottom())
                await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadNextPageAsync);
               
        }
       
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

            await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadFirstPageAsync);
        }


        //Private Methods
        private async Task LoadFirstPageAsync()
        {
            _currentPage = 1;
            _hasMoreData = true;
            _bindingSource.DataSource = null;


            DataTable settings = await clsSetting.GetAllAsync(_currentPage, _pageSize);

            _bindingSource.DataSource = settings;
            dgvSettigns.DataSource = _bindingSource;
            _hasMoreData = settings.Rows.Count == _pageSize;     

            clsButtonHelper.SetEditDelteButtonsState(dgvSettigns, btnEdit, btnDelete);


            //To load more pages if there is more space in grid view
            while (clsDataGridViewHelper.HasEmptyVisibleArea(dgvSettigns) && _hasMoreData)
                await LoadNextPageAsync();

        }

        private bool IsNearBottom()
        {
            //returns how many rows are currently visible in the DataGridView viewport.
            int displayRows = dgvSettigns.DisplayedRowCount(false);

            //gives the index of the first row currently visible at the top of the viewport.
            //Example: if the user scrolled down so row 21 is at the top, firstRow = 20 (0-based index)
            int firstDisplayRowIndex = dgvSettigns.FirstDisplayedScrollingRowIndex;

            int totalRows = dgvSettigns.RowCount;

            return displayRows + firstDisplayRowIndex >= totalRows - 2;
        }

        private async Task LoadNextPageAsync()
        {
            _isLoading = true;
            _currentPage++;

            DataTable newSettings = await clsSetting.GetAllAsync(_currentPage, _pageSize);

            if (_bindingSource.DataSource is DataTable existingTable)
                foreach (DataRow row in newSettings.Rows)
                    existingTable.ImportRow(row);

            _hasMoreData = newSettings.Rows.Count == _pageSize;
            _isLoading = false;

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
            await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadFirstPageAsync);
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
                await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadFirstPageAsync);
                clsMessageBoxHelper.ShowInfo("تم حذف بيانات الاعداد");         
            }
            else
                clsMessageBoxHelper.ShowError("حدث خطأ عند محاولة حذف بيانات الاعداد");
        }    
    }
}

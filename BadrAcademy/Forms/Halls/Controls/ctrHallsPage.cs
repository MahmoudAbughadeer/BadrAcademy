using BLL;
using BadrAcademy.Helpers;
using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BadrAcademy.Forms.Halls.Controls
{
    public partial class ctrHallsPage : UserControl
    {
        //Private fields
        private int _currentPage = 1;

        private const int _pageSize = 20;

        private bool _isLoading = false;

        private bool _hasMoreData = true;

        private BindingSource _bindingSource = new BindingSource();


        //Constructors
        public ctrHallsPage()
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


            DataTable halls = await clsHall.GetAllAsync(_currentPage, _pageSize);

            _bindingSource.DataSource = halls;
            dgvSettigns.DataSource = _bindingSource;
            _hasMoreData = halls.Rows.Count == _pageSize;     

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

            DataTable newHalls = await clsHall.GetAllAsync(_currentPage, _pageSize);

            if (_bindingSource.DataSource is DataTable existingTable)
                foreach (DataRow row in newHalls.Rows)
                    existingTable.ImportRow(row);

            _hasMoreData = newHalls.Rows.Count == _pageSize;
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
                Name = "HallID",
                DataPropertyName = "HallID",
                HeaderText = "المعرف",
                FillWeight = 15
            });

            dgvSettigns.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "HallName",
                DataPropertyName = "HallName",
                HeaderText = "الأسم",
                FillWeight = 25
            });

            dgvSettigns.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "HallType",
                DataPropertyName = "HallType",
                HeaderText = "النوع",
                FillWeight = 25
            });


            dgvSettigns.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DefaultCapacity",
                DataPropertyName = "DefaultCapacity",
                HeaderText = "العدد",
                FillWeight = 15
            });

            dgvSettigns.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "IsActive ",
                DataPropertyName = "IsActive",
                HeaderText = "الحالة",
                FillWeight = 10
            });
        }
        
      
        private async Task AddEditSetting_SaveCompletedAsync()
        {
            await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadFirstPageAsync);
        }



        //Buttons Events
        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddEditHall frm = new frmAddEditHall();
            frm.SaveCompleted += AddEditSetting_SaveCompletedAsync; //will not be block you event is Task and awaitable
            frm.ShowDialog();

            frm.SaveCompleted -= AddEditSetting_SaveCompletedAsync;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int currentHallID = clsDataGridViewHelper.GetCellValueAsInt(this.dgvSettigns, "HallID");
            frmAddEditHall frm = new frmAddEditHall(currentHallID);

            frm.SaveCompleted += AddEditSetting_SaveCompletedAsync;//will not be block you event is Task and awaitable
            frm.ShowDialog();
            frm.SaveCompleted -= AddEditSetting_SaveCompletedAsync;
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            int currentHallID = clsDataGridViewHelper.GetCellValueAsInt(this.dgvSettigns, "HallID");

            if (clsMessageBoxHelper.ShowConfirmation("هل أنت متأكد من حذف هذه القاعة؟") == DialogResult.Cancel)
                return;

            if (await clsAsyncMethodExecutor.RunWithWaitAsync<bool>(ctrWait1, () => clsHall.DeleteAsync(currentHallID)))
            {
                await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadFirstPageAsync);
                clsMessageBoxHelper.ShowInfo("تم حذف بيانات القاعة");         
            }
            else
                clsMessageBoxHelper.ShowError("حدث خطأ عند محاولة حذف بيانات القاعة");
        }    
    }
}

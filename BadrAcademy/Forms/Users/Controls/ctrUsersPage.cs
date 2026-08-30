using BLL;
using BadrAcademy.Helpers;
using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BadrAcademy.Forms.Users.Controls
{
    public partial class ctrUsersPage : UserControl
    {
        //Private fields
        private int _currentPage = 1;

        private const int _pageSize = 20;

        private bool _isLoading = false;

        private bool _hasMoreData = true;

        private BindingSource _bindingSource = new BindingSource();


        //Constructors
        public ctrUsersPage()
        {
            InitializeComponent();
        }


        //DataGridView Events
        private async void dgvUsers_Scroll(object sender, ScrollEventArgs e)
        {
            if (_isLoading || !_hasMoreData)
                return;

            if (IsNearBottom())
                await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadNextPageAsync);
               
        }
       
        private void dgvUsers_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            dgvUsers.Rows[e.RowIndex].Cells["RowNumber"].Value = e.RowIndex + 1;
        }

        //Events
        private async void ctrUsersPage_Load(object sender, EventArgs e)
        {
            cbFilter.SelectedIndex = 0;

            InitializeDataGrideViewColumns();//Here I add the columns manually
            dgvUsers.AutoGenerateColumns = false;//this is adding columnss/property name that come from database

            //change the font size and adjust row height
            dgvUsers.Font = new Font("Arial", 14);
            dgvUsers.RowTemplate.Height = 35;
            dgvUsers.DefaultCellStyle.Padding = new Padding(5);



            await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadFirstPageAsync);
        }


        //Private Methods
        private async Task LoadFirstPageAsync()
        {
            _currentPage = 1;
            _hasMoreData = true;
            _bindingSource.DataSource = null;


            DataTable users = await clsUser.GetAllAsync(_currentPage, _pageSize);

            _bindingSource.DataSource = users;
            dgvUsers.DataSource = _bindingSource;
            _hasMoreData = users.Rows.Count == _pageSize;     

            clsButtonHelper.SetEditDelteButtonsState(dgvUsers, btnEdit, btnDelete, btnShowUser);


            //To load more pages if there is more space in grid view
            while (clsDataGridViewHelper.HasEmptyVisibleArea(dgvUsers) && _hasMoreData)
                await LoadNextPageAsync();

        }

        private bool IsNearBottom()
        {
            //returns how many rows are currently visible in the DataGridView viewport.
            int displayRows = dgvUsers.DisplayedRowCount(false);

            //gives the index of the first row currently visible at the top of the viewport.
            //Example: if the user scrolled down so row 21 is at the top, firstRow = 20 (0-based index)
            int firstDisplayRowIndex = dgvUsers.FirstDisplayedScrollingRowIndex;

            int totalRows = dgvUsers.RowCount;

            return displayRows + firstDisplayRowIndex >= totalRows - 2;
        }

        private async Task LoadNextPageAsync()
        {
            _isLoading = true;
            _currentPage++;

            DataTable newUsers = await clsUser.GetAllAsync(_currentPage, _pageSize);

            if (_bindingSource.DataSource is DataTable existingTable)
                foreach (DataRow row in newUsers.Rows)
                    existingTable.ImportRow(row);

            _hasMoreData = newUsers.Rows.Count == _pageSize;
            _isLoading = false;

        }
       
        private void InitializeDataGrideViewColumns()
        {
            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "RowNumber",
                DataPropertyName = "RowNumber",
                HeaderText = "م",
                FillWeight = 10,
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UserID",
                DataPropertyName = "UserID",
                HeaderText = "المعرف",
                FillWeight = 15
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UserName",
                DataPropertyName = "UserName",
                HeaderText = "إسم المستخدم",
                FillWeight = 25
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "FullName",
                DataPropertyName = "FullName",
                HeaderText = "الأسم",
                FillWeight = 40
            });


            dgvUsers.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "IsActive",
                DataPropertyName = "IsActive",
                HeaderText = "نشط",
                FillWeight = 10
            });
        }
        
        private string GetFilterColumn()
        {
            string filterColumn = "";

            switch (cbFilter.Text)
            {
                case "المعرف":
                    filterColumn = "UserID";
                    break;
                case "اسم المستخدم":
                    filterColumn = "UserName";
                    break;
                case "الأسم":
                    filterColumn = "Name";
                    break;
                case "الحالة":
                    filterColumn = "IsActive";
                    break;
                default:
                    filterColumn = null;
                    break;
            }

            return filterColumn;
        }

        private async Task ApplyFilter()
        {

            string filterColumn = GetFilterColumn();
            
            if (filterColumn == null || (txtFilter.Visible && txtFilter.Text.Trim() == ""))
            {
                _bindingSource.RemoveFilter();
                return;
            }


            switch (filterColumn)
            {
                case "UserID":
                    _bindingSource.Filter = $"{filterColumn} = {txtFilter.Text.Trim()}";
                    break;
                case "UserName":
                case "Name":
                    _bindingSource.Filter = $"{filterColumn} LIKE '%{txtFilter.Text.Trim()}%'";
                    break;
                case "IsActive":
                    _bindingSource.Filter = $"{filterColumn} = {(cbIsActive.Text.Trim() == "نشط"? 1 : 0)}";
                    break;
            }


            //to load more data after filter if there is more space in grid view
            while (clsDataGridViewHelper.HasEmptyVisibleArea(dgvUsers) && _hasMoreData)
                await LoadNextPageAsync();
        }

        private async Task AddEditUser_SaveCompletedAsync()
        {
            await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadFirstPageAsync);
        }



        //Buttons Events
        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddEditUser frm = new frmAddEditUser();
            frm.SaveCompleted += AddEditUser_SaveCompletedAsync; //will not be block you event is Task and awaitable
            frm.ShowDialog();

            frm.SaveCompleted -= AddEditUser_SaveCompletedAsync;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int currentUserID = clsDataGridViewHelper.GetCellValueAsInt(this.dgvUsers, "UserID");
            frmAddEditUser frm = new frmAddEditUser(currentUserID);

            frm.SaveCompleted += AddEditUser_SaveCompletedAsync;//will not be block you event is Task and awaitable
            frm.ShowDialog();
            frm.SaveCompleted -= AddEditUser_SaveCompletedAsync;
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            int currentUserID = clsDataGridViewHelper.GetCellValueAsInt(this.dgvUsers, "UserID");

            if (clsMessageBoxHelper.ShowConfirmation("هل أنت متأكد من حذف هذا المستخدم؟") == DialogResult.Cancel)
                return;

            if (await clsAsyncMethodExecutor.RunWithWaitAsync<bool>(ctrWait1, () => clsUser.DeleteAsync(currentUserID)))
            {
                await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadFirstPageAsync);
                clsMessageBoxHelper.ShowInfo("تم حذف بيانات المستخدم");         
            }
            else
                clsMessageBoxHelper.ShowError("حدث خطأ عند محاولة حذف بيانات المستخدم");

        }

        private void btnShowUser_Click(object sender, EventArgs e)
        {
            int currentUserID = clsDataGridViewHelper.GetCellValueAsInt(this.dgvUsers, "UserID");
            clsOpenFormHelper.ShowDialogCenteredInContainer(new frmShowUserInfo(currentUserID), this);
        }



        //ComboBox Events
        private async void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilter.SelectedIndex == 0)
                await ApplyFilter(); //Just to clear the filter

            txtFilter.Visible = cbFilter.SelectedIndex == 1 || cbFilter.SelectedIndex == 2 || cbFilter.SelectedIndex == 3;

            if(txtFilter.Visible)
            {
                txtFilter.Focus();
            }
               
            cbIsActive.Visible = cbFilter.SelectedIndex == 4;
            if (cbIsActive.Visible)
            {
                cbIsActive.SelectedIndex = 0;
            }

            
        }

        private async void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            await ApplyFilter();
        }


        //TextBox Events

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.Text == "المعرف")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private async void txtFilter_TextChanged(object sender, EventArgs e)
        {
            await ApplyFilter();
        }

        
    }
}

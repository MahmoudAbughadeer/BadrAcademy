using BLL;
using BadrAcademy.Helpers;
using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BadrAcademy.Forms.Subjects.Controls
{
    public partial class ctrSubjectsPage : UserControl
    {
        //Private fields
        private int _currentPage = 1;

        private const int _pageSize = 20;

        private bool _isLoading = false;

        private bool _hasMoreData = true;

        private BindingSource _bindingSource = new BindingSource();


        //Constructors
        public ctrSubjectsPage()
        {
            InitializeComponent();
        }


        //DataGridView Events
        private async void dgvSubjects_Scroll(object sender, ScrollEventArgs e)
        {
            if (_isLoading || !_hasMoreData)
                return;

            if (IsNearBottom())
                await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadNextPageAsync);
               
        }

        private void dgvSubjects_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            /*
             * It is called every time a row is repainted. A row is repainted whenever, for example:
                - The grid is first displayed.
                - You scroll.
                - You resize the form or the grid.
                - A cell value changes.
                - You select a row.
                - You enter or leave a cell.
                - The grid is refreshed or invalidated.
             */
            if (dgvSubjects.Rows[e.RowIndex].IsNewRow)
                return;

            dgvSubjects.Rows[e.RowIndex].Cells["RowNumber"].Value = e.RowIndex + 1;
        }

        private void dgvSubjects_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && dgvSubjects.CurrentRow != null && !dgvSubjects.CurrentRow.IsNewRow)
                btnDelete_Click(null, null);
            else if (e.KeyCode == Keys.Enter)
            {
                btnSave.PerformClick();
                e.Handled = true;
            }
        }



        //Events
        private async void ctrSubjectsPage_Load(object sender, EventArgs e)
        {
            cbFilter.SelectedIndex = 0;

            InitializeDataGridViewColumns();
            dgvSubjects.AutoGenerateColumns = false;


            //change the font size and adjust row height
            dgvSubjects.Font = new Font("Arial", 14);
            dgvSubjects.RowTemplate.Height = 35;
            dgvSubjects.DefaultCellStyle.Padding = new Padding(5);



            await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadFirstPageAsync);
        }


        //Private Methods
        private async Task LoadFirstPageAsync()
        {
            _currentPage = 1;
            _hasMoreData = true;
            _bindingSource.DataSource = null;


            DataTable subjects = await clsSubject.GetAllAsync(_currentPage, _pageSize);

            _bindingSource.DataSource = subjects;
            dgvSubjects.DataSource = _bindingSource;
            _hasMoreData = subjects.Rows.Count == _pageSize;     

            clsButtonHelper.SetEditDelteButtonsState(dgvSubjects, btnEdit, btnDelete);


            //To load more pages if there is more space in grid view
            while (clsDataGridViewHelper.HasEmptyVisibleArea(dgvSubjects) && _hasMoreData)
                await LoadNextPageAsync();

        }

        private bool IsNearBottom()
        {
            //returns how many rows are currently visible in the DataGridView viewport.
            int displayRows = dgvSubjects.DisplayedRowCount(false);

            //gives the index of the first row currently visible at the top of the viewport.
            //Example: if the user scrolled down so row 21 is at the top, firstRow = 20 (0-based index)
            int firstDisplayRowIndex = dgvSubjects.FirstDisplayedScrollingRowIndex;

            int totalRows = dgvSubjects.RowCount;

            return displayRows + firstDisplayRowIndex >= totalRows - 2;
        }

        private async Task LoadNextPageAsync()
        {
            _isLoading = true;
            _currentPage++;

            DataTable newSubjects = await clsSubject.GetAllAsync(_currentPage, _pageSize);
            
            if (_bindingSource.DataSource is DataTable existingTable)
                foreach (DataRow row in newSubjects.Rows)
                    existingTable.ImportRow(row);

            _hasMoreData = newSubjects.Rows.Count == _pageSize;
            _isLoading = false;

        }

        private void InitializeDataGridViewColumns()
        {
            dgvSubjects.Columns.Clear();

            dgvSubjects.Columns.Insert(0, new DataGridViewTextBoxColumn
            {
                Name = "RowNumber",
                HeaderText = "م",
                ReadOnly = true,
                FillWeight = 20,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            dgvSubjects.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SubjectID",
                DataPropertyName = "SubjectID",
                ReadOnly = true,
                Visible = false,
            });

            dgvSubjects.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SubjectName",
                DataPropertyName = "SubjectName",
                HeaderText = "اسم المادة",
                FillWeight = 80
            });
        }

        private string GetFilterColumn()
        {
            string filterColumn = "";

            switch (cbFilter.Text)
            {
                case "اسم المقرر":
                    filterColumn = "SubjectName";
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
                case "SubjectName":
                    _bindingSource.Filter = $"{filterColumn} LIKE '%{txtFilter.Text.Trim()}%'";
                    break;
            }


            //to load more data after filter if there is more space in grid view
            while (clsDataGridViewHelper.HasEmptyVisibleArea(dgvSubjects) && _hasMoreData)
                await LoadNextPageAsync();
        }

        private async Task AddEditSubject_SaveCompletedAsync()
        {
            await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadFirstPageAsync);
        }

        private async Task<bool> SaveNewRowAsync(DataRow row)
        {
            string subjectName = row["SubjectName"]?.ToString();

            clsSubject subject = new clsSubject(subjectName);
            return await subject.SaveAsync();
        }

        private async Task<bool> UpdateRowAsync(DataRow row)
        {
            int subjectId = Convert.ToInt32(row["SubjectID"]);
            string subjectName = row["SubjectName"]?.ToString();

            // No extra DB call needed
            clsSubject subject = clsSubject.CreateForUpdate(subjectId, subjectName);
            return await subject.SaveAsync();
        }


        //Buttons Events
        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddEditSubject frm = new frmAddEditSubject();
            frm.SaveCompleted += AddEditSubject_SaveCompletedAsync; //will not be block you event is Task and awaitable
            frm.ShowDialog();

            frm.SaveCompleted -= AddEditSubject_SaveCompletedAsync;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int currentSubjectID = clsDataGridViewHelper.GetCellValueAsInt(this.dgvSubjects, "SubjectID");
            frmAddEditSubject frm = new frmAddEditSubject(currentSubjectID);

            frm.SaveCompleted += AddEditSubject_SaveCompletedAsync;//will not be block you event is Task and awaitable
            frm.ShowDialog();
            frm.SaveCompleted -= AddEditSubject_SaveCompletedAsync;
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            int currentSubjectID = clsDataGridViewHelper.GetCellValueAsInt(this.dgvSubjects, "SubjectID");

            if (clsMessageBoxHelper.ShowConfirmation("هل أنت متأكد من حذف هذا المقرر؟") == DialogResult.Cancel)
                return;

            if (await clsAsyncMethodExecutor.RunWithWaitAsync<bool>(ctrWait1, () => clsSubject.DeleteAsync(currentSubjectID)))
            {
                await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadFirstPageAsync);
                clsMessageBoxHelper.ShowInfo("تم حذف بيانات المقرر");         
            }
            else
                clsMessageBoxHelper.ShowError("حدث خطأ عند محاولة حذف بيانات المقرر");

        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            // 1. Commit any active edit first
            dgvSubjects.EndEdit();



            // 2. Get the underlying DataTable
            DataTable dt = _bindingSource.DataSource as DataTable;
            if (dt == null)
                return;

            DataTable changes = dt.GetChanges();

            if (changes == null || changes.Rows.Count == 0)
            {
                clsMessageBoxHelper.ShowWarning("لم يتم إجراء اى تعديلات");
                return;
            }

            bool allSuccess = true;
            bool success = false;

            foreach (DataRow row in changes.Rows)
            {

                switch (row.RowState)
                {
                    case DataRowState.Added:
                        success = await clsAsyncMethodExecutor.RunWithWaitAsync<bool>(ctrWait1, () => SaveNewRowAsync(row));
                        break;

                    case DataRowState.Modified:
                        success = await clsAsyncMethodExecutor.RunWithWaitAsync<bool>(ctrWait1, () => UpdateRowAsync(row));
                        break;
                }

                if (!success)
                    allSuccess = false;
            }

            if (allSuccess)
            {
                await LoadFirstPageAsync();
                clsMessageBoxHelper.ShowInfo("تم الحفظ بنجاح");
            }           
            else
                clsMessageBoxHelper.ShowError("حدث خطأ أثناء الحفظ");

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
        }


        //TextBox Events
        private async void txtFilter_TextChanged(object sender, EventArgs e)
        {
            await ApplyFilter();
        }

        
    }
}

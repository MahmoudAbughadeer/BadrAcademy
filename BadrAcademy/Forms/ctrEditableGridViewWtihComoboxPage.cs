using BLL;
using BadrAcademy.Helpers;
using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BadrAcademy.Forms
{
    public partial class ctrEditableGridViewWtihComoboxPage : UserControl
    {
        //Private fields
        private int _currentPage = 1;

        private const int _pageSize = 20;

        private bool _isLoading = false;

        private bool _hasMoreData = true;

        private BindingSource _bindingSource = new BindingSource();

        private int _defaultDoctorID;

        //Constructors
        public ctrEditableGridViewWtihComoboxPage()
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

        private void dgvSubjects_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["DoctorID"].Value = _defaultDoctorID; // Required
            e.Row.Cells["DoctorID2"].Value = DBNull.Value;    // Optional
        }

        private void dgvSubjects_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception != null && (e.ColumnIndex == dgvSubjects.Columns["DoctorID"].Index ||
                e.ColumnIndex == dgvSubjects.Columns["DoctorID2"].Index))
            {
                // Suppress the error popup; optionally log it
                e.ThrowException = false;
            }
        }

        private void dgvSubjects_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && (e.ColumnIndex == dgvSubjects.Columns["DoctorID"].Index ||
                e.ColumnIndex == dgvSubjects.Columns["DoctorID2"].Index))
            {
                dgvSubjects.BeginEdit(true);
                if (dgvSubjects.EditingControl is ComboBox cb)
                    cb.DroppedDown = true;
            }
        }

        private void dgvSubjects_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvSubjects.IsCurrentCellDirty && dgvSubjects.CurrentCell is DataGridViewComboBoxCell)
            {
                dgvSubjects.CommitEdit(DataGridViewDataErrorContexts.Commit);
                /*Immediately commits the selected value to the DataGridView and its data source,
                instead of waiting until the user leaves the cell.*/
            }
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

            InitializeDataGridViewColumns(await clsDoctor.GetAllAsync());
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

            subjects.Columns["DoctorID"].AllowDBNull = true;

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

        private void InitializeDataGridViewColumns(DataTable doctors)
        {
            dgvSubjects.Columns.Clear();


            // Save the first doctor ID as the default
            _defaultDoctorID = Convert.ToInt32(doctors.Rows[0]["DoctorID"]);


            // Prepare nullable copy for Doctor 2
            DataTable doctorsNullable = doctors.Copy();
            DataRow blankRow = doctorsNullable.NewRow();
            blankRow["DoctorID"] = DBNull.Value; // ← must be DBNull not null
            blankRow["DoctorName"] = "";
            blankRow["DepartmentName"] = "";
            doctorsNullable.Rows.InsertAt(blankRow, 0);


            dgvSubjects.Columns.Insert(0, new DataGridViewTextBoxColumn
            {
                Name = "RowNumber",
                HeaderText = "م",
                ReadOnly = true,
                FillWeight = 10,
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
                FillWeight = 40
            });

            dgvSubjects.Columns.Add(new DataGridViewComboBoxColumn
            {
                Name = "DoctorID",
                DataPropertyName = "DoctorID",
                HeaderText = "المدرس الأول",
                FillWeight = 25,
                DataSource = doctors,
                DisplayMember = "DoctorName",
                ValueMember = "DoctorID",
                FlatStyle = FlatStyle.Flat
            });

            dgvSubjects.Columns.Add(new DataGridViewComboBoxColumn
            {
                Name = "DoctorID2",
                DataPropertyName = "DoctorID2",
                HeaderText = "المدرس الثاني",
                FillWeight = 25,
                DataSource = doctorsNullable,
                DisplayMember = "DoctorName",
                ValueMember = "DoctorID",
                FlatStyle = FlatStyle.Flat,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    NullValue = DBNull.Value   // ← DBNull not null
                }
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
                    _bindingSource.Filter = $"{filterColumn} = {(cbIsActive.Text.Trim() == "نشط" ? 1 : 0)}";
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
            int doctorId1 = Convert.ToInt32(row["DoctorID"]);
            int? doctorId2 = row["DoctorID2"] == DBNull.Value
                                    ? (int?)null
                                    : Convert.ToInt32(row["DoctorID2"]);

            //clsSubject subject = new clsSubject(subjectName, doctorId1, doctorId2);
            //return await subject.SaveAsync();

            return false; //delete it when using this control
        }

        private async Task<bool> UpdateRowAsync(DataRow row)
        {
            int subjectId = Convert.ToInt32(row["SubjectID"]);
            string subjectName = row["SubjectName"]?.ToString();
            int doctorId1 = Convert.ToInt32(row["DoctorID"]);
            int? doctorId2 = row["DoctorID2"] == DBNull.Value
                                    ? (int?)null
                                    : Convert.ToInt32(row["DoctorID2"]);

            // No extra DB call needed
            //clsSubject subject = clsSubject.CreateForUpdate(subjectId, subjectName,
            //                                                 doctorId1, doctorId2);
            //return await subject.SaveAsync();

            return false;//delete it when using this control
        }

        private async Task<bool> DeleteRowAsync(DataRow row)
        {
            // Must use DataRowVersion.Original — deleted rows lose current values
            int subjectId = Convert.ToInt32(row["SubjectID", DataRowVersion.Original]);

            return await clsSubject.DeleteAsync(subjectId);
        }


        //Buttons Events
        private void btnAdd_Click(object sender, EventArgs e)
        {
            //frmAddEditSubject frm = new frmAddEditSubject();
            //frm.SaveCompleted += AddEditSubject_SaveCompletedAsync; //will not be block you event is Task and awaitable
            //frm.ShowDialog();

            //frm.SaveCompleted -= AddEditSubject_SaveCompletedAsync;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int currentSubjectID = clsDataGridViewHelper.GetCellValueAsInt(this.dgvSubjects, "SubjectID");
            //frmAddEditSubject frm = new frmAddEditSubject(currentSubjectID);

            //frm.SaveCompleted += AddEditSubject_SaveCompletedAsync;//will not be block you event is Task and awaitable
            //frm.ShowDialog();
            //frm.SaveCompleted -= AddEditSubject_SaveCompletedAsync;
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

            if (txtFilter.Visible)
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

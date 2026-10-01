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
        private BindingSource _bindingSource = new BindingSource();


        //Constructors
        public ctrSubjectsPage()
        {
            InitializeComponent();
        }


        //DataGridView Events
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



            await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadSubjectsDataAsync);
        }


        //Private Methods
        private async Task LoadSubjectsDataAsync()
        {
            _bindingSource.DataSource = null;


            DataTable subjects = await clsSubject.GetAllAsync();

            _bindingSource.DataSource = subjects;
            dgvSubjects.DataSource = _bindingSource;  

            clsButtonHelper.SetEditDelteButtonsState(dgvSubjects, btnEdit, btnDelete);
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
            await LoadSubjectsDataAsync();
        }

        private async Task AddEditSubject_SaveCompletedAsync()
        {
            await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadSubjectsDataAsync);
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
                await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadSubjectsDataAsync);
                clsMessageBoxHelper.ShowInfo("تم حذف بيانات المقرر");         
            }
            else
                clsMessageBoxHelper.ShowError("حدث خطأ عند محاولة حذف بيانات المقرر");

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

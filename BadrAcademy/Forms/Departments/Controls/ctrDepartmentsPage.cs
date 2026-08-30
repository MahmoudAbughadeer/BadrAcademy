using BLL;
using BadrAcademy.Helpers;
using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BadrAcademy.Forms.Departments.Controls
{
    public partial class ctrDepartmentsPage : UserControl
    {
        //Private fields
        private BindingSource _bindingSource = new BindingSource();


        //Constructors
        public ctrDepartmentsPage()
        {
            InitializeComponent();
        }


        //DataGridView Events
        private void dgvLevels_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            dgvLevels.Rows[e.RowIndex].Cells["RowNumber"].Value = e.RowIndex + 1;
        }

        //Events
        private async void ctrSettingsPage_Load(object sender, EventArgs e)
        {
            InitializeDataGrideViewColumns();//Here I add the columns manually
            dgvLevels.AutoGenerateColumns = false;//this is adding columnss/property name that come from database

            //change the font size and adjust row height
            dgvLevels.Font = new Font("Arial", 14);
            dgvLevels.RowTemplate.Height = 35;
            dgvLevels.DefaultCellStyle.Padding = new Padding(5);

            await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadDepartments);
        }


        //Private Methods
        private async Task LoadDepartments()
        {
            _bindingSource.DataSource = null;

            DataTable departments = await clsDepartment.GetAllAsync();
            _bindingSource.DataSource = departments;
            dgvLevels.DataSource = _bindingSource;

            clsButtonHelper.SetEditDelteButtonsState(dgvLevels, btnEdit, btnDelete);
        }
 
        private void InitializeDataGrideViewColumns()
        {
            dgvLevels.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "RowNumber",
                DataPropertyName = "RowNumber",
                HeaderText = "م",
                FillWeight = 10,
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            dgvLevels.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DepartmentID",
                DataPropertyName = "DepartmentID",
                HeaderText = "المعرف",
                FillWeight = 20
            });

            dgvLevels.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DepartmentName",
                DataPropertyName = "DepartmentName",
                HeaderText = "الشعبة",
                FillWeight = 30
            });

            dgvLevels.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Description",
                DataPropertyName = "Description",
                HeaderText = "الوصف",
                FillWeight = 40
            });
        }
        
      
        private async Task AddEditDepartment_SaveCompletedAsync()
        {
            await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadDepartments);
        }



        //Buttons Events
        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddEditDepartment frm = new frmAddEditDepartment();
            frm.SaveCompleted += AddEditDepartment_SaveCompletedAsync; //will not be block you event is Task and awaitable
            frm.ShowDialog();

            frm.SaveCompleted -= AddEditDepartment_SaveCompletedAsync;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int currentDepartmentID = clsDataGridViewHelper.GetCellValueAsInt(this.dgvLevels, "DepartmentID");
            frmAddEditDepartment frm = new frmAddEditDepartment(currentDepartmentID);

            frm.SaveCompleted += AddEditDepartment_SaveCompletedAsync;//will not be block you event is Task and awaitable
            frm.ShowDialog();
            frm.SaveCompleted -= AddEditDepartment_SaveCompletedAsync;
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            int currentDepartmentID = clsDataGridViewHelper.GetCellValueAsInt(this.dgvLevels, "DepartmentID");

            if (clsMessageBoxHelper.ShowConfirmation("هل أنت متأكد من حذف هذه القسم؟") == DialogResult.Cancel)
                return;

            if (await clsAsyncMethodExecutor.RunWithWaitAsync<bool>(ctrWait1, () => clsDepartment.DeleteAsync(currentDepartmentID)))
            {
                await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadDepartments);
                clsMessageBoxHelper.ShowInfo("تم حذف بيانات القسم");         
            }
            else
                clsMessageBoxHelper.ShowError("حدث خطأ عند محاولة حذف بيانات القسم");
        }    
    }
}

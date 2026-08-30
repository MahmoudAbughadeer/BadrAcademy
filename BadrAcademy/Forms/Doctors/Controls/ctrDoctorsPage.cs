using BLL;
using BadrAcademy.Helpers;
using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BadrAcademy.Forms.Doctors.Controls
{
    public partial class ctrDoctorsPage : UserControl
    {
        //Private fields
        private BindingSource _bindingSource = new BindingSource();


        //Constructors
        public ctrDoctorsPage()
        {
            InitializeComponent();
        }


        //DataGridView Events
        private void dgvDoctors_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            dgvDoctors.Rows[e.RowIndex].Cells["RowNumber"].Value = e.RowIndex + 1;
        }

        //Events
        private async void ctrDoctorsPage_Load(object sender, EventArgs e)
        {

            InitializeDataGrideViewColumns();//Here I add the columns manually
            dgvDoctors.AutoGenerateColumns = false;//this is adding columnss/property name that come from database

            cbFilter.SelectedIndex = 0;


            await FillComoboxWithDepartments();

            //change the font size and adjust row height
            dgvDoctors.Font = new Font("Arial", 14);
            dgvDoctors.RowTemplate.Height = 35;
            dgvDoctors.DefaultCellStyle.Padding = new Padding(5);

            await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadDoctors);
        }


        //Private Methods
        private async Task LoadDoctors()
        {
            _bindingSource.DataSource = null;

            DataTable doctors = await clsDoctor.GetAllAsync();
            _bindingSource.DataSource = doctors;
            dgvDoctors.DataSource = _bindingSource;

            clsButtonHelper.SetEditDelteButtonsState(dgvDoctors, btnEdit, btnDelete);
        }
 
        private void InitializeDataGrideViewColumns()
        {
            dgvDoctors.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "RowNumber",
                DataPropertyName = "RowNumber",
                HeaderText = "م",
                FillWeight = 10,
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            dgvDoctors.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DoctorID",
                DataPropertyName = "DoctorID",
                HeaderText = "المعرف",
                FillWeight = 20
            });

            dgvDoctors.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DoctorName",
                DataPropertyName = "DoctorName",
                HeaderText = "الأسم",
                FillWeight = 40
            });

            dgvDoctors.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DepartmentName",
                DataPropertyName = "DepartmentName",
                HeaderText = "القسم",
                FillWeight = 30
            });
        }

        private string GetFilterColumn()
        {
            string filterColumn = "";

            switch (cbFilter.Text)
            {
                case "المعرف":
                    filterColumn = "DoctorID";
                    break;
                case "الأسم":
                    filterColumn = "DoctorName";
                    break;
                case "القسم":
                    filterColumn = "DepartmentName";
                    break;
                default:
                    filterColumn = null;
                    break;
            }

            return filterColumn;
        }

        private void ApplyFilter()
        {

            string filterColumn = GetFilterColumn();

            if (filterColumn == null || (txtFilter.Visible && txtFilter.Text.Trim() == ""))
            {
                _bindingSource.RemoveFilter();
                return;
            }


            switch (filterColumn)
            {
                case "DoctorID":
                    _bindingSource.Filter = $"{filterColumn} = {txtFilter.Text.Trim()}";
                    break;
                case "DoctorName":
                    _bindingSource.Filter = $"{filterColumn} LIKE '%{txtFilter.Text.Trim()}%'";
                    break;
                case "DepartmentName":
                    _bindingSource.Filter = $"{filterColumn} = '{cbDepartments.Text.Trim()}'";
                    break;
            }
        }
        private async Task FillComoboxWithDepartments()
        {
            cbDepartments.DataSource = await clsDepartment.GetAllAsync();
            cbDepartments.ValueMember = "DepartmentID";
            cbDepartments.DisplayMember = "DepartmentName";
        }



        private async Task AddEditDoctor_SaveCompletedAsync()
        {
            await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadDoctors);
        }



        //Buttons Events
        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddEditDoctor frm = new frmAddEditDoctor();
            frm.SaveCompleted += AddEditDoctor_SaveCompletedAsync; //will not be block you event is Task and awaitable
            frm.ShowDialog();

            frm.SaveCompleted -= AddEditDoctor_SaveCompletedAsync;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int currentDoctorID = clsDataGridViewHelper.GetCellValueAsInt(this.dgvDoctors, "DoctorID");
            frmAddEditDoctor frm = new frmAddEditDoctor(currentDoctorID);

            frm.SaveCompleted += AddEditDoctor_SaveCompletedAsync;//will not be block you event is Task and awaitable
            frm.ShowDialog();
            frm.SaveCompleted -= AddEditDoctor_SaveCompletedAsync;
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            int currentDoctorID = clsDataGridViewHelper.GetCellValueAsInt(this.dgvDoctors, "DoctorID");

            if (clsMessageBoxHelper.ShowConfirmation("هل أنت متأكد من حذف هذه المدرس؟") == DialogResult.Cancel)
                return;

            if (await clsAsyncMethodExecutor.RunWithWaitAsync<bool>(ctrWait1, () => clsDoctor.DeleteAsync(currentDoctorID)))
            {
                await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadDoctors);
                clsMessageBoxHelper.ShowInfo("تم حذف بيانات المدرس");         
            }
            else
                clsMessageBoxHelper.ShowError("حدث خطأ عند محاولة حذف بيانات المدرس");
        }



        //ComboBox events
        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilter.SelectedIndex == 0)
                ApplyFilter(); //Just to clear the filter

            txtFilter.Visible = cbFilter.SelectedIndex == 1 || cbFilter.SelectedIndex == 2;

            if (txtFilter.Visible)
            {
                txtFilter.Focus();
            }

            cbDepartments.Visible = cbFilter.SelectedIndex == 3;
            if (cbDepartments.Visible)
            {
                cbDepartments.SelectedIndex = 0;
                ApplyFilter();
            }
        }

        private void cbDepartments_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }


        //TextBox Events

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.Text == "المعرف")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

    }
}

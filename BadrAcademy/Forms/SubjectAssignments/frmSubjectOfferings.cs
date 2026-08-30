using BadrAcademy.Helpers;
using BLL;
using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace BadrAcademy.Forms.SubjectAssignments
{
    public partial class frmSubjectOfferings : Form
    {
        //Private fields
        BindingSource _bindingSource = new BindingSource();
        clsSubjectOffering _subjectOffering;

        public frmSubjectOfferings()
        {
            InitializeComponent();
            _subjectOffering = null;
        }


        //Events
        public delegate Task SaveCompletedAsync();

        public event SaveCompletedAsync SaveCompleted;
        private async void frmSubjectOfferings_Load(object sender, EventArgs e)
        {

            cbLevels.DataSource = clsLevel.GetAllAsync();
            cbLevels.ValueMember = "LevelID";
            cbLevels.DisplayMember = "LevelName";


            cbDepartments.DataSource = clsDepartment.GetAllAsync();
            cbDepartments.ValueMember = "DepartmentID";
            cbDepartments.DisplayMember = "DepartmentName";



            InitializeDataGridViewColumns();//Here I add the columns manually
            dgvSubjectAssignments.AutoGenerateColumns = false;//this is adding columnss/property name that come from database

            //change the font size and adjust row height
            dgvSubjectAssignments.Font = new Font("Arial", 14);
            dgvSubjectAssignments.RowTemplate.Height = 35;
            dgvSubjectAssignments.DefaultCellStyle.Padding = new Padding(5);

            await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadSubjectOfferings);
        }



        //DataGridView Events
        private void dgvSubjectAssignments_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            dgvSubjectAssignments.Rows[e.RowIndex].Cells["RowNumber"].Value = e.RowIndex + 1;
        }



        //Private Methods
        private async Task LoadSubjectOfferings()
        {
            _bindingSource.DataSource = null;
            int subjectID = Convert.ToInt32(cbSubjects.SelectedValue);
            int levelID = Convert.ToInt32(cbLevels.SelectedValue);
            int departmentID = Convert.ToInt32(cbDepartments.SelectedValue);
            string semester = cbSemesters.Text.Trim();

            DataTable subjectOfferings = await clsSubjectOffering.GetByLevelDepartmentSemesterAsync(levelID, departmentID, semester);
            _bindingSource.DataSource = subjectOfferings;
            dgvSubjectAssignments.DataSource = _bindingSource;
        }

        private void LoadUserInfoFromFormToUserObject()
        {
            int subjectID = Convert.ToInt32(cbSubjects.SelectedValue);
            int levelID = Convert.ToInt32(cbLevels.SelectedValue);
            int departmentID = Convert.ToInt32(cbDepartments.SelectedValue);
            string semester = cbSemesters.Text.Trim();

            _subjectOffering = new clsSubjectOffering(subjectID, levelID, departmentID, semester);
        }

        private void InitializeDataGridViewColumns()
        {
            dgvSubjectAssignments.Columns.Clear();


            dgvSubjectAssignments.Columns.Insert(0, new DataGridViewTextBoxColumn
            {
                Name = "RowNumber",
                HeaderText = "م",
                ReadOnly = true,
                FillWeight = 10,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            dgvSubjectAssignments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "OfferingID",
                DataPropertyName = "OfferingID",
                ReadOnly = true,
                Visible = false,
            });

            dgvSubjectAssignments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SubjectName",
                DataPropertyName = "SubjectName",
                HeaderText = "اسم المادة",
                FillWeight = 30
            });

            dgvSubjectAssignments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "LevelName",
                DataPropertyName = "LevelName",
                HeaderText = "المستوى",
                FillWeight = 15
            });

            dgvSubjectAssignments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DepartmentName",
                DataPropertyName = "DepartmentName",
                HeaderText = "الشعب",
                FillWeight = 15
            });

            dgvSubjectAssignments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Semester",
                DataPropertyName = "Semester",
                HeaderText = "الفصل",
                FillWeight = 15
            });

        }


        //Buttons Events
        private async void btnSave_Click(object sender, EventArgs e)
        {
            LoadUserInfoFromFormToUserObject();

            if (!await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, () => _subjectOffering.SaveAsync()))
            {

                clsMessageBoxHelper.ShowWarning("حدث خطأ أثناء محاولة حفظ بيانات تعيين المواد");
                return;
            }

            await SaveCompleted?.Invoke();
            await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadSubjectOfferings);         
        }

        
    }
}








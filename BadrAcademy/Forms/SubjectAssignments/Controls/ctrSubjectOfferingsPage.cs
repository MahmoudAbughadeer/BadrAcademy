using BLL;
using BadrAcademy.Helpers;
using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BadrAcademy.Forms.SubjectAssignments.Controls
{
    public partial class ctrSubjectOfferingsPage : UserControl
    {
        //Private fields
        private BindingSource _bindingSource = new BindingSource();


        //Constructors
        public ctrSubjectOfferingsPage()
        {
            InitializeComponent();
        }


        //DataGridView Events
         
        private void dgvSubjectOfferings_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            dgvSubjectOfferings.Rows[e.RowIndex].Cells["RowNumber"].Value = e.RowIndex + 1;
        }

        //Events
        private async void dgvSubjectOfferings_Load(object sender, EventArgs e)
        {

            cbLevels.DataSource = await clsLevel.GetAllAsync();
            cbLevels.ValueMember = "LevelID";
            cbLevels.DisplayMember = "LevelName";

            //================================================================
            DataTable dtDepartments = await clsDepartment.GetAllAsync();

            DataRow defaultDepartmentRow = dtDepartments.NewRow();
            defaultDepartmentRow["DepartmentID"] = 0;
            defaultDepartmentRow["DepartmentName"] = "لاشئ";

            dtDepartments.Rows.InsertAt(defaultDepartmentRow, 0);

            cbDepartments.DataSource = dtDepartments;
            cbDepartments.ValueMember = "DepartmentID";
            cbDepartments.DisplayMember = "DepartmentName";


            //================================================================
            DataTable dtSubjects = await clsSubject.GetAllAsync();

            DataRow defaultSubjectsRow = dtSubjects.NewRow();
            defaultSubjectsRow["SubjectID"] = 0;
            defaultSubjectsRow["SubjectName"] = "لاشئ";

            dtSubjects.Rows.InsertAt(defaultSubjectsRow, 0);

            cbSubjects.DataSource = dtSubjects;
            cbSubjects.ValueMember = "SubjectID";
            cbSubjects.DisplayMember = "SubjectName";


            cbLevels.SelectedIndex = 2;
            cbSemesters.SelectedIndex = 0;

            InitializeDataGrideViewColumns();//Here I add the columns manually
            dgvSubjectOfferings.AutoGenerateColumns = false;//this is adding columnss/property name that come from database

            //change the font size and adjust row height
            dgvSubjectOfferings.Font = new Font("Arial", 14);
            dgvSubjectOfferings.RowTemplate.Height = 35;
            dgvSubjectOfferings.DefaultCellStyle.Padding = new Padding(5);



            await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadSubjectOfferingsDataAsync);
        }


        //Private Methods
        private async Task LoadSubjectOfferingsDataAsync()
        {
            _bindingSource.DataSource = null;
            DataTable subjectOfferings = await clsSubjectOffering.GetAllWithNamesAsync();
            _bindingSource.DataSource = subjectOfferings;
            dgvSubjectOfferings.DataSource = _bindingSource;
        }

 
        private void InitializeDataGrideViewColumns()
        {
            dgvSubjectOfferings.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "RowNumber",
                DataPropertyName = "RowNumber",
                HeaderText = "م",
                FillWeight = 10,
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            dgvSubjectOfferings.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "OfferingID",
                DataPropertyName = "OfferingID",
                HeaderText = "المعرف",
                Visible = false,
            });

            dgvSubjectOfferings.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SubjectName",
                DataPropertyName = "SubjectName",
                HeaderText = "المقرر",
                FillWeight = 20
            });

            dgvSubjectOfferings.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "LevelName",
                DataPropertyName = "LevelName",
                HeaderText = "المستوى",
                FillWeight = 20
            });


            dgvSubjectOfferings.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DepartmentName",
                DataPropertyName = "DepartmentName",
                HeaderText = "القسم",
                FillWeight = 20
            });

            dgvSubjectOfferings.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DoctorName",
                DataPropertyName = "DoctorName",
                HeaderText = "الدكتور",
                FillWeight = 20
            });

            dgvSubjectOfferings.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Semester",
                DataPropertyName = "Semester",
                HeaderText = "التيرم",
                FillWeight = 10
            });
        }
        
        private void ApplyFilter()
        {
            string levelName = cbLevels.Text.Trim();
            string semester = cbSemesters.Text.Trim() == "لاشئ"? string.Empty : cbSemesters.Text.Trim();
            string departmentName = cbDepartments.Text.Trim() == "لاشئ" ? string.Empty : cbDepartments.Text.Trim();
            string subjectName = cbSubjects.Text.Trim() == "لاشئ" ? string.Empty : cbSubjects.Text.Trim();
          
            _bindingSource.Filter = $@"LevelName LIKE '%{levelName}%' 
                                       AND (Semester = '' OR Semester LIKE '%{semester}%')
                                       AND (DepartmentName = '' OR DepartmentName LIKE '%{departmentName}%')
                                       AND (SubjectName = '' OR SubjectName LIKE '%{subjectName}%')";
        }

        private async Task frmSubjectOfferings_SaveCompletedAsync()
        {
            await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadSubjectOfferingsDataAsync);
        }


        //Buttons Events
        private void btnEditSubjectOfferings_Click(object sender, EventArgs e)
        {
            frmSubjectOfferings frm = new frmSubjectOfferings();
            frm.SaveCompleted += frmSubjectOfferings_SaveCompletedAsync;
            frm.ShowDialog();
        }



        //ComboBox events
        private void cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }
    }
}

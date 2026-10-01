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

            cbLevels.DataSource = await clsLevel.GetAllAsync();
            cbLevels.ValueMember = "LevelID";
            cbLevels.DisplayMember = "LevelName";

            //================================================================
            cbDepartments.DataSource = await clsDepartment.GetAllAsync(); ;
            cbDepartments.ValueMember = "DepartmentID";
            cbDepartments.DisplayMember = "DepartmentName";

            //================================================================
            cbSubjects.DataSource = await clsSubject.GetAllAsync(); ;
            cbSubjects.ValueMember = "SubjectID";
            cbSubjects.DisplayMember = "SubjectName";

            //================================================================
            cbDoctors.DataSource = await clsDoctor.GetAllAsync();
            cbDoctors.ValueMember = "DoctorID";
            cbDoctors.DisplayMember = "DoctorName";


            cbSemesters.SelectedIndex = 0;
            cbLevels.SelectedIndex = 2;

            InitializeDataGridViewColumns();//Here I add the columns manually
            dgvSubjectAssignments.AutoGenerateColumns = false;//this is adding columnss/property name that come from database

            //change the font size and adjust row height
            dgvSubjectAssignments.Font = new Font("Arial", 14);
            dgvSubjectAssignments.RowTemplate.Height = 35;
            dgvSubjectAssignments.DefaultCellStyle.Padding = new Padding(5);



            await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadSubjectOfferingsDataAsync);
        }



        //DataGridView Events
        private void dgvSubjectAssignments_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            dgvSubjectAssignments.Rows[e.RowIndex].Cells["RowNumber"].Value = e.RowIndex + 1;
        }
        
        private async void dgvSubjectAssignments_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (dgvSubjectAssignments.Columns[e.ColumnIndex].Name == "Delete")
            {
                int offeringId = clsDataGridViewHelper.GetCellValueAsInt(dgvSubjectAssignments, "OfferingID");

                DialogResult result = MessageBox.Show("هل تريد حذف هذا السجل؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;

                bool deleted = await clsSubjectOffering.DeleteAsync(offeringId);

                if (deleted)
                {
                    await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadSubjectOfferingsDataAsync);
                    await SaveCompleted?.Invoke();
                }
                    
                else
                    MessageBox.Show("فشل حذف السجل", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        //Private Methods
        private async Task LoadSubjectOfferingsDataAsync()
        {
            _bindingSource.DataSource = null;
            DataTable subjectOfferings = await clsSubjectOffering.GetAllWithNamesAsync();
            _bindingSource.DataSource = subjectOfferings;
            dgvSubjectAssignments.DataSource = _bindingSource;
        }

        private void LoadSubjectOfferingInfoFromFormToSubjectOfferingObject()
        {
            int subjectID = Convert.ToInt32(cbSubjects.SelectedValue);
            int levelID = Convert.ToInt32(cbLevels.SelectedValue);
            int departmentID = Convert.ToInt32(cbDepartments.SelectedValue);
            int doctorID = Convert.ToInt32(cbDoctors.SelectedValue);
            string semester = cbSemesters.Text.Trim();

            _subjectOffering = new clsSubjectOffering(subjectID, levelID, departmentID, doctorID, semester);
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
                Name = "LevelName",
                DataPropertyName = "LevelName",
                HeaderText = "المستوى",
                FillWeight = 10
            });

            dgvSubjectAssignments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Semester",
                DataPropertyName = "Semester",
                HeaderText = "الفصل",
                FillWeight = 10
            });

            dgvSubjectAssignments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DepartmentName",
                DataPropertyName = "DepartmentName",
                HeaderText = "الشعب",
                FillWeight = 20
            });

            dgvSubjectAssignments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SubjectName",
                DataPropertyName = "SubjectName",
                HeaderText = "المقرر",
                FillWeight = 25
            });

            dgvSubjectAssignments.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DoctorName",
                DataPropertyName = "DoctorName",
                HeaderText = "الدكتور",
                FillWeight = 15
            });

            dgvSubjectAssignments.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "Delete",
                HeaderText = "حذف",
                Text = "X",
                UseColumnTextForButtonValue = true,
                FlatStyle = FlatStyle.Flat,
                FillWeight = 10
            });
        }
        
        private void ApplyFilter()
        {
            string levelName = cbLevels.Text.Trim();
            string semester = cbSemesters.Text.Trim() == "لاشئ" ? string.Empty : cbSemesters.Text.Trim();
            string departmentName = cbDepartments.Text.Trim() == "لاشئ" ? string.Empty : cbDepartments.Text.Trim();

            _bindingSource.Filter = $@"LevelName LIKE '%{levelName}%' 
                                       AND (Semester = '' OR Semester LIKE '%{semester}%')
                                       AND (DepartmentName = '' OR DepartmentName LIKE '%{departmentName}%')";
        }

        
        //Buttons Events
        private async void btnSave_Click(object sender, EventArgs e)
        {
            LoadSubjectOfferingInfoFromFormToSubjectOfferingObject();

            if (!await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, () => _subjectOffering.SaveAsync()))
            {

                clsMessageBoxHelper.ShowWarning("حدث خطأ أثناء محاولة حفظ بيانات تعيين المواد");
                return;
            }

            await SaveCompleted?.Invoke();
            await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadSubjectOfferingsDataAsync);         
        }


        //ComboBox events
        private void cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        
    }
}
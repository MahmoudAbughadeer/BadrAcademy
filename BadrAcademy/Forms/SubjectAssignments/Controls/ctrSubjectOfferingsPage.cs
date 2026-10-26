using BLL;
using BadrAcademy.Helpers;
using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using BadrAcademy.Forms.ExamSchedule;

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
            var row = dgvSubjectOfferings.Rows[e.RowIndex];

            row.Cells["RowNumber"].Value = e.RowIndex + 1;

            if (Convert.ToBoolean(row.Cells["HasExam"].Value))
                row.DefaultCellStyle.BackColor = Color.LightGreen;
            else
                row.DefaultCellStyle.BackColor = Color.Empty;
        }

        private async void dgvSubjectOfferings_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (dgvSubjectOfferings.Columns[e.ColumnIndex].Name == "AddEditExamSchedule")
            {
                int offeringId = clsDataGridViewHelper.GetCellValueAsInt(dgvSubjectOfferings, "OfferingID");
                object value = dgvSubjectOfferings.Rows[e.RowIndex].Cells["HasExam"].Value;
                bool hasExam = Convert.ToBoolean(value);

                frmAddEditExamSchedule frm = new frmAddEditExamSchedule(offeringId, hasExam);
                frm.SaveCompleted += frmSubjectOfferings_SaveCompletedAsync;
                frm.ShowDialog();
            }

            if (dgvSubjectOfferings.Columns[e.ColumnIndex].Name == "DeleteExamSchedule")
            {
                int offeringId = clsDataGridViewHelper.GetCellValueAsInt(dgvSubjectOfferings, "OfferingID");
                DialogResult dialogResult = MessageBox.Show
                    (
                        "هل تريد حذف جدول امتحان هذه المادة؟",
                        "",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1
                    );

                if (dialogResult == DialogResult.No)
                    return;
                
                bool deletetionSuccess = await clsExamSchedule.DeleteByOfferingIDAsync(offeringId);
                await LoadSubjectOfferingsDataAsync();
            }

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
                Name = "OfferingID",
                DataPropertyName = "OfferingID",
                HeaderText = "المعرف",
                Visible = false,
            });

            dgvSubjectOfferings.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "HasExam",
                DataPropertyName = "HasExam",
                HeaderText = "الامتحان",
                Visible = false
            });

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
                FillWeight = 13
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

            dgvSubjectOfferings.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "AddEditExamSchedule",
                HeaderText = "",
                Text = "📝",
                UseColumnTextForButtonValue = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.Green
                },
                FlatStyle = FlatStyle.Flat,
                FillWeight = 4
            });

            dgvSubjectOfferings.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "DeleteExamSchedule",
                HeaderText = "",
                Text = "X",
                UseColumnTextForButtonValue = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.Red
                },
                FlatStyle = FlatStyle.Flat,
                FillWeight = 3
            });
        }
        
            private void ApplyFilter()
        {
            string levelName = cbLevels.Text.Trim();
            string semester = cbSemesters.Text.Trim() == "لاشئ" ? string.Empty : cbSemesters.Text.Trim();
            string departmentName = cbDepartments.Text.Trim() == "لاشئ" ? string.Empty : cbDepartments.Text.Trim();
            string subjectName = cbSubjects.Text.Trim() == "لاشئ" ? string.Empty : cbSubjects.Text.Trim();

            string filter = $"LevelName LIKE '%{levelName}%'";

            if (!string.IsNullOrEmpty(semester))
                filter += $" AND Semester LIKE '%{semester}%'";

            if (!string.IsNullOrEmpty(departmentName))
                filter += $" AND DepartmentName LIKE '%{departmentName}%'";

            if (!string.IsNullOrEmpty(subjectName))
                filter += $" AND SubjectName LIKE '%{subjectName}%'";

            _bindingSource.Filter = filter;
        
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

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
    public partial class ctrExamSchedulePage : UserControl
    {
        //Private fields
        private BindingSource _bindingSource = new BindingSource();


        //Constructors
        public ctrExamSchedulePage()
        {
            InitializeComponent();
        }


        //DataGridView Events
         
        private void dgvExamSchedule_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var row = dgvExamSchedule.Rows[e.RowIndex];
            row.Cells["RowNumber"].Value = e.RowIndex + 1;
        }


        //Events
        private async void dgvExamSchedule_Load(object sender, EventArgs e)
        {
            DataTable dtSettings = await clsSetting.GetAllForComboBoxAsync();
            cbSettings.ValueMember = "SettingID";
            cbSettings.DisplayMember = "SettingDetails";
            cbSettings.DataSource = dtSettings;

            if (dtSettings.Rows.Count > 0)
            {
                cbSettings.SelectedIndex = 0;
            }


            cbPeriod.SelectedIndex = 0;


            InitializeDataGrideViewColumns();//Here I add the columns manually
            dgvExamSchedule.AutoGenerateColumns = false;//this is adding columnss/property name that come from database

            //change the font size and adjust row height
            dgvExamSchedule.Font = new Font("Arial", 14);
            dgvExamSchedule.RowTemplate.Height = 35;
            dgvExamSchedule.DefaultCellStyle.Padding = new Padding(5);



            await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadPageDate);
        }


        //Private Methods
        private async Task LoadPageDate()
        {
            _bindingSource.DataSource = null;
            DataTable subjectOfferings = await clsExamSchedule.GetExamScheduleDetailsAsync();
            _bindingSource.DataSource = subjectOfferings;
            dgvExamSchedule.DataSource = _bindingSource;
        }

        private void InitializeDataGrideViewColumns()
        {
            dgvExamSchedule.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ExamID",
                DataPropertyName = "ExamID",
                Visible = false,
            });

            dgvExamSchedule.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SettingID",
                DataPropertyName = "OfferingID",
                Visible = false,
            });

            dgvExamSchedule.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "OfferingID",
                DataPropertyName = "OfferingID",
                Visible = false,
            });


            dgvExamSchedule.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "RowNumber",
                DataPropertyName = "RowNumber",
                HeaderText = "م",
                FillWeight = 5,
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            dgvExamSchedule.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "LevelName",
                DataPropertyName = "LevelName",
                HeaderText = "المستوى",
                FillWeight = 15
            });


            dgvExamSchedule.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DepartmentName",
                DataPropertyName = "DepartmentName",
                HeaderText = "القسم",
                FillWeight = 15
            });

            dgvExamSchedule.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SubjectName",
                DataPropertyName = "SubjectName",
                HeaderText = "المقرر",
                FillWeight = 15
            });

            dgvExamSchedule.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DoctorName",
                DataPropertyName = "DoctorName",
                HeaderText = "الدكتور",
                FillWeight = 15
            });

            dgvExamSchedule.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ExamDate",
                DataPropertyName = "ExamDate",
                HeaderText = "التاريخ",
                FillWeight = 10
            });

            dgvExamSchedule.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TimeFrom",
                DataPropertyName = "TimeFrom",
                HeaderText = "من",
                FillWeight = 4
            });


            dgvExamSchedule.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TimeTo",
                DataPropertyName = "TimeTo",
                HeaderText = "الى",
                FillWeight = 4
            });

            dgvExamSchedule.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Period",
                DataPropertyName = "Period",
                HeaderText = "الفترة",
                FillWeight = 7
            });
        }
        
        private void ApplyFilter()
        {
            object selectedVAlue = cbSettings.SelectedValue;
            int settingID = Convert.ToInt32(selectedVAlue);
            string period = cbPeriod.Text.Trim();

            if(period == "لاشئ")
            {
                _bindingSource.Filter = $@"SettingID = {settingID}";
                return;
            }      
            
            _bindingSource.Filter = $@"SettingID = {settingID}
                                       AND Period LIKE '%{period}%'";
        }

        private async Task frmExamSchedule_SaveCompletedAsync()
        {
            await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadPageDate);
        }


        //Buttons Events
        private void btnEdit_Click(object sender, EventArgs e)
        {
            int offeringID = clsDataGridViewHelper.GetCellValueAsInt(dgvExamSchedule, "OfferingID");
            frmAddEditExamSchedule frm = new frmAddEditExamSchedule(offeringID, true);
            frm.SaveCompleted += frmExamSchedule_SaveCompletedAsync;
            frm.ShowDialog();
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            int examID = clsDataGridViewHelper.GetCellValueAsInt(dgvExamSchedule, "ExamID");
            DialogResult result = MessageBox.Show(
                    "Are you sure you want to delete this Exam?",
                    "",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1);

            if (result == DialogResult.Yes)
            {
                await clsExamSchedule.DeleteAsync(examID);
                await frmExamSchedule_SaveCompletedAsync();
            }

        }


        //ComboBox events
        private void cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();    
        }

        
    }
}

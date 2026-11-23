using BadrAcademy.Helpers;
using BLL;
using Shared.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BadrAcademy.Forms.Exporters.Controls
{
    public partial class ctrExportersPage : UserControl
    {
        private int _settingId;
        private int? _levelId;
        private int? _departmentId;
        private int? _subjectId;

        public ctrExportersPage()
        {
            InitializeComponent();
        }


        private static string SelectWordFile()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select Word File";
                openFileDialog.Filter = "Word Files (*.doc;*.docx)|*.doc;*.docx";
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return openFileDialog.FileName;
                }

                return string.Empty;
            }
        }

        private static string SelectExcelFile()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select Excel File";
                openFileDialog.Filter = "Excel Files (*.xls;*.xlsx)|*.xls;*.xlsx";
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return openFileDialog.FileName;
                }

                return string.Empty;
            }
        }

        private static string SelectWordOutputPath()
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Title = "Save Word File";
                dialog.Filter = "Word Document (*.docx)|*.docx";
                dialog.DefaultExt = "docx";
                dialog.AddExtension = true;
                dialog.FileName = "Output.docx";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return dialog.FileName;
                }

                return string.Empty;
            }
        }

        private static string SelectExcelOutputPath()
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Title = "Save Excel File";
                dialog.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
                dialog.DefaultExt = "xlsx";
                dialog.AddExtension = true;
                dialog.FileName = "Output.xlsx";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return dialog.FileName;
                }

                return string.Empty;
            }
        }

        private async Task WordExporter(DataTable dt)
        {
            string wordPath = SelectWordFile();
            if (string.IsNullOrEmpty(wordPath))
                return;

            string wordOutputPath = SelectWordOutputPath();
            if (string.IsNullOrEmpty(wordOutputPath))
                return;

            await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, async () => await clsWordExporter.GenerateEnvelopesAsync(wordPath, wordOutputPath, dt));

            MessageBox.Show("تم توليد الاظرف بنجاح");
        }
       
        private async Task ExcelEporter(DataTable dt)
        {
            string excelOutputPath = SelectExcelOutputPath();
            if (string.IsNullOrEmpty(excelOutputPath))
                return;

            await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, async () => await clsExcelExporter.ExportAsync(excelOutputPath, dt));

            MessageBox.Show("تم توليد الشيت بنجاح");
        }

        
        //===========================================
        private void UpdateLevelFilter()
        {
            if(cbLevels.SelectedValue == null)
            {
                _levelId = null;
                return;
            }


            if(int.TryParse(cbLevels.SelectedValue.ToString(), out int levelId))
            {
                if (levelId == 0)
                    _levelId = null;
                else
                    _levelId = levelId;
            }
            else
            {
                _levelId = null;
            }    
        }
        
        private void UpdateDepartmentFilter()
        {
            if (cbDepartments.SelectedValue == null)
            {
                _departmentId = null;
                return;
            }

            if (int.TryParse(cbDepartments.SelectedValue.ToString(), out int departmentId))
            {
                if (departmentId == 0)
                    _departmentId = null;
                else
                    _departmentId = departmentId;
            }
            else
            {
                _departmentId = null;
            }
        }

        private void UpdateSubjectFilter()
        {
            if (cbSubjects.SelectedValue == null)
            {
                _subjectId = null;
                return;
            }

            if (int.TryParse(cbSubjects.SelectedValue.ToString(), out int subjectId))
            {
                if (subjectId == 0)
                    _subjectId = null;
                else
                    _subjectId = subjectId;
            }
            else
            {
                _subjectId = null;
            }
        }
        
        private async Task LoadSettingData()
        {
            DataTable dtSettings = await clsSetting.GetAllForComboBoxAsync();
            cbSettings.ValueMember = "SettingID";
            cbSettings.DisplayMember = "SettingDetails";
            cbSettings.DataSource = dtSettings;
        }

        private async Task LoadLevelData()
        {
            DataTable dtLevels = await clsLevel.GetAllAsync();

            DataRow defaultLevelRow = dtLevels.NewRow();
            defaultLevelRow["LevelID"] = 0;
            defaultLevelRow["LevelName"] = "لاشئ";
            dtLevels.Rows.InsertAt(defaultLevelRow, 0);

            cbLevels.DataSource = dtLevels;
            cbLevels.ValueMember = "LevelID";
            cbLevels.DisplayMember = "LevelName";
        }
        private async Task LoadSubjectsData()
        {
            DataTable dtSubjects = await clsSubject.GetAllByDepartmentIdAsync(_departmentId);

            DataRow defaultSubjectRow = dtSubjects.NewRow();
            defaultSubjectRow["SubjectID"] = 0;
            defaultSubjectRow["SubjectName"] = "لاشئ";

            dtSubjects.Rows.InsertAt(defaultSubjectRow, 0);

            cbSubjects.DataSource = dtSubjects;
            cbSubjects.ValueMember = "SubjectID";
            cbSubjects.DisplayMember = "SubjectName";
        }
        private async Task LoadDepartmentsData()
        {
            DataTable dtDepartments = await clsDepartment.GetAllAsync();

            DataRow defaultDepartmentRow = dtDepartments.NewRow();
            defaultDepartmentRow["DepartmentID"] = 0;
            defaultDepartmentRow["DepartmentName"] = "لاشئ";

            dtDepartments.Rows.InsertAt(defaultDepartmentRow, 0);

            cbDepartments.DataSource = dtDepartments;
            cbDepartments.ValueMember = "DepartmentID";
            cbDepartments.DisplayMember = "DepartmentName";
        }
        
        
        //Events
        private async void ctrExportersPage_Load(object sender, EventArgs e)
        {
            await LoadSettingData();
            await LoadLevelData();
            await LoadDepartmentsData();
            await LoadSubjectsData();

            cbSettings.SelectedIndex = 0;
        }



        //Buttons Events (مرايات)
        private async void btnQuestionEnvelopes_Click(object sender, EventArgs e)
        {
            DataTable dt = await clsQuestionEnvelopes.GetAllAsync(_settingId, _levelId, _departmentId, _subjectId);
            await WordExporter(dt);
        }

        private async void btnAnswerEnvelopeFinal_Click(object sender, EventArgs e)
        {
            DataTable dt = await clsAnswerEnvelopes.GetAllAsyncForFinal(_settingId, _levelId, _departmentId, _subjectId);
            await WordExporter(dt);
        }

        private async void btnAnswerEvnelopeMid_Click(object sender, EventArgs e)
        {
            DataTable dt = await clsAnswerEnvelopes.GetAllAsyncForMidterm(_settingId, _levelId, _departmentId, _subjectId);
            await WordExporter(dt);
        }

        private async void btnHangingMirrors_Click(object sender, EventArgs e)
        {
            DataTable dt = await clsDayGeneral.GetAllAsync(_settingId, _levelId, _departmentId, _subjectId);
            await WordExporter(dt);
        }

        private void btnSmallEnvelopes_Click(object sender, EventArgs e)
        {
            btnHangingMirrors.PerformClick();
        }


        //Buttons Events (فتح مظاريف)
        private async void btnEnvelopesCovers_Click(object sender, EventArgs e)
        {
            DataTable dt = await clsDayGeneral.GetAllAsync(_settingId, _levelId, _departmentId, _subjectId);

            string wordPath = SelectWordFile();
            if (string.IsNullOrEmpty(wordPath))
                return;

            string wordOutputPath = SelectWordOutputPath();
            if (string.IsNullOrEmpty(wordOutputPath))
                return;

            await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, async () => await clsWordExporter.GenerateExamsPerDayReportsAsync(wordPath, wordOutputPath, dt));

            MessageBox.Show("تم توليد اغلف الاظرف بنجاح");
        }
        
        private void btnMinutesOpenEnvelopes_Click(object sender, EventArgs e)
        {
            btnHangingMirrors.PerformClick();
        }

        private async void btnDistributionStaff_Click(object sender, EventArgs e)
        {
            if(_levelId == null)
            {
                MessageBox.Show("You must select level first", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataTable dt = await clsExamSchedule.GetExamDayDataAsync(_levelId.Value);
            await WordExporter(dt);
        }
        
        private void btnDistributionObserver_Click(object sender, EventArgs e)
        {
            btnDistributionStaff.PerformClick();
        }

        private void btnStudentAttendance_Click(object sender, EventArgs e)
        {
            btnDistributionStaff.PerformClick();
        }
        
        private async void btnDepartmentControlReview_Click(object sender, EventArgs e)
        {
            if (_levelId == null || _departmentId == null)
            {
                MessageBox.Show("You must select level and department first", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataTable dt = await clsDayGeneral.GetAllAsync(_settingId, _levelId, _departmentId, null);
            await WordExporter(dt);
        }


        //Buttons Events (غلاف رزم)
        private void btnPinNumber_Click(object sender, EventArgs e)
        {
            btnHangingMirrors.PerformClick();
        }
        private async void btnBundleWrapper_Click(object sender, EventArgs e)
        {
            if (_departmentId == null)
            {
                MessageBox.Show(
                    "You must select department first",
                    "Warning",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            Dictionary<int, int> dic = new Dictionary<int, int>();
            dic.Add(_departmentId.Value, Convert.ToInt32(nudBundleCount.Value));

            DataTable dt = await clsDayGeneral.GetAllRepeatedAsync(
                dic,
                _settingId,
                _levelId,
                _departmentId,
                _subjectId);

            await WordExporter(dt);
        }


        //Buttons Events (Excel Exporters)
        private async void btnExamsSchedule_Click(object sender, EventArgs e)
        {
            DataTable dt = await clsDayGeneral.GetAllAsync(_settingId, _levelId, _departmentId, _subjectId);
            await ExcelEporter(dt);
        }

        private async void btnQuestionEnvelopesSheet_Click(object sender, EventArgs e)
        {
            DataTable dt = await clsQuestionEnvelopes.GetAllAsync(_settingId, _levelId, _departmentId, _subjectId);
            await ExcelEporter(dt);
        }

        private async void btnAnswerEnvelopesSheet_Click(object sender, EventArgs e)
        {
            DataTable dt = await clsAnswerEnvelopes.GetAllAsyncForFinal(_settingId, _levelId, _departmentId, _subjectId);
            
            await ExcelEporter(dt);
        }


        //CB Events
        private void cbSettings_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSettings.SelectedValue != null &&
            int.TryParse(cbSettings.SelectedValue.ToString(), out int settingId))
            {
                _settingId = settingId;
            }
        }

        private void cbLevels_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateLevelFilter();
        }

        private async void cbDepartments_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDepartmentFilter();
            await LoadSubjectsData();
        }

        private void cbSubjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSubjectFilter();
        }

        
    }
}

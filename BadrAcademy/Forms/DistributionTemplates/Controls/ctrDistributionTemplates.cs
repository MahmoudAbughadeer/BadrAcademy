using BadrAcademy.Forms.Misc.Controls;
using BadrAcademy.Helpers;
using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BadrAcademy.Forms.DistributionTemplates.Controls
{
    public partial class ctrDistributionTemplates : UserControl
    {
        //Private fields
        BindingSource _bindingSource = new BindingSource();
        clsDistributionTemplate _distributionTemplate;

        public ctrDistributionTemplates()
        {
            InitializeComponent();
            _distributionTemplate = null;
        }

        private async void ctrDistributionTemplates_Load(object sender, EventArgs e)
        {
            DataTable dtSettings = await clsSetting.GetAllForComboBoxAsync();
            cbSettings.ValueMember = "SettingID";
            cbSettings.DisplayMember = "SettingDetails";
            cbSettings.DataSource = dtSettings;

            //================================================
            cbLevels.DataSource = await clsLevel.GetAllAsync();
            cbLevels.ValueMember = "LevelID";
            cbLevels.DisplayMember = "LevelName";

            //================================================================
            cbDepartments.DataSource = await clsDepartment.GetAllAsync(); ;
            cbDepartments.ValueMember = "DepartmentID";
            cbDepartments.DisplayMember = "DepartmentName";

            //================================================================
            cbHalls.DataSource = await clsHall.GetAllActiveAsync(); ;
            cbHalls.ValueMember = "HallID";
            cbHalls.DisplayMember = "HallName";


            cbSettings.SelectedIndex = 0;
            cbLevels.SelectedIndex = 2;
            cbDepartments.SelectedIndex = 0;
            cbHalls.SelectedIndex = 0;

            InitializeDataGridViewColumns();//Here I add the columns manually
            dgvDistributionTemplates.AutoGenerateColumns = false;//this is adding columnss/property name that come from database

            //change the font size and adjust row height
            dgvDistributionTemplates.Font = new Font("Arial", 14);
            dgvDistributionTemplates.RowTemplate.Height = 35;
            dgvDistributionTemplates.DefaultCellStyle.Padding = new Padding(5);



            await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadDistributionTemplatesDataAsync);
        }



        //DataGridView Events
        private void dgvDistributionTemplates_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            dgvDistributionTemplates.Rows[e.RowIndex].Cells["RowNumber"].Value = e.RowIndex + 1;
        }

        private async void dgvDistributionTemplates_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (dgvDistributionTemplates.Columns[e.ColumnIndex].Name == "Delete")
            {
                int templateID = clsDataGridViewHelper.GetCellValueAsInt(dgvDistributionTemplates, "TemplateID");

                DialogResult result = MessageBox.Show("هل تريد حذف هذا السجل؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;

                bool deleted = await clsDistributionTemplate.DeleteAsync(templateID);

                if (deleted)
                {
                    await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadDistributionTemplatesDataAsync);
                }

                else
                    MessageBox.Show("فشل حذف السجل", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        //Private Methods
        private async Task LoadDistributionTemplatesDataAsync()
        {
            _bindingSource.DataSource = null;
            DataTable distributionTemplates = await clsDistributionTemplate.GetDistributionTemplateDetailsAsync();
            _bindingSource.DataSource = distributionTemplates;
            dgvDistributionTemplates.DataSource = _bindingSource;
        }

        private void LoadDistributionTemplatesInfoFromFormToObject()
        {
            int levelID = Convert.ToInt32(cbLevels.SelectedValue);
            int departmentID = Convert.ToInt32(cbDepartments.SelectedValue);
            int committeNo = Convert.ToInt32(nudCommitteeNo.Value);
            int studentsCount = Convert.ToInt32(nudStudentsCount.Value);
            int settingID = Convert.ToInt32(cbSettings.SelectedValue);
            int hallID = Convert.ToInt32(cbHalls.SelectedValue);

            _distributionTemplate = new clsDistributionTemplate(levelID, departmentID, committeNo, studentsCount, settingID, hallID);
        }

        private void InitializeDataGridViewColumns()
        {
            dgvDistributionTemplates.Columns.Clear();

            dgvDistributionTemplates.Columns.Insert(0, new DataGridViewTextBoxColumn
            {
                Name = "RowNumber",
                HeaderText = "م",
                ReadOnly = true,
                FillWeight = 10,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            dgvDistributionTemplates.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TemplateID",
                DataPropertyName = "TemplateID",
                ReadOnly = true,
                Visible = false,
            });
            dgvDistributionTemplates.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SettingID",
                DataPropertyName = "SettingID",
                ReadOnly = true,
                Visible = false,
            });

            dgvDistributionTemplates.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "LevelName",
                DataPropertyName = "LevelName",
                HeaderText = "المستوى",
                FillWeight = 15
            });

            dgvDistributionTemplates.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DepartmentName",
                DataPropertyName = "DepartmentName",
                HeaderText = "الشعب",
                FillWeight = 30
            });

            dgvDistributionTemplates.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "HallName",
                DataPropertyName = "HallName",
                HeaderText = "المكان",
                FillWeight = 20
            });

            dgvDistributionTemplates.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CommitteeNo",
                DataPropertyName = "CommitteeNo",
                HeaderText = "اللجنة",
                FillWeight = 10
            });

            dgvDistributionTemplates.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StudentsCount",
                DataPropertyName = "StudentsCount",
                HeaderText = "العدد",
                FillWeight = 10
            });

            dgvDistributionTemplates.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "Delete",
                HeaderText = "",
                Text = "X",
                UseColumnTextForButtonValue = true,

                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.Red
                },

                FlatStyle = FlatStyle.Flat,
                FillWeight = 5
            });
        }

        private void ApplyFilter()
        {
            if (cbSettings.SelectedValue == null || cbSettings.SelectedValue == DBNull.Value)
                return;

            int settingID = Convert.ToInt32(cbSettings.SelectedValue);
            string levelName = cbLevels.Text.Trim();
            string departmentName = cbDepartments.Text.Trim() == "لاشئ" ? string.Empty : cbDepartments.Text.Trim();

            string filter = $"SettingID = {settingID} AND LevelName LIKE '%{levelName}%'";

            if (!string.IsNullOrEmpty(departmentName))
                filter += $" AND DepartmentName LIKE '%{departmentName}%'";

            _bindingSource.Filter = filter;
        }


        //Buttons Events
        private async void btnSave_Click(object sender, EventArgs e)
        {
            LoadDistributionTemplatesInfoFromFormToObject();

            if (!await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, () => _distributionTemplate.SaveAsync()))
            {

                clsMessageBoxHelper.ShowWarning("حدث خطأ أثناء محاولة حفظ بيانات توزيع اللجان");
                return;
            }

            await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadDistributionTemplatesDataAsync);
        }


        //ComboBox events
        private void cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }
    }
}

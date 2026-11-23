using BadrAcademy.Forms.Misc.Controls;
using BadrAcademy.Helpers;
using BLL;
using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BadrAcademy.Forms.SubjectAssignments
{
    public partial class frmEditSubjectOfferings : Form
    {
        int _offeringID;
        clsSubjectOffering _subjectOffering;
        public frmEditSubjectOfferings(int offeringID)
        {
            InitializeComponent();
            _offeringID = offeringID;
        }

        //Events
        public delegate Task SaveCompletedAsync();

        public event SaveCompletedAsync SaveCompleted;

        private async void frmEditSubjectOfferings_Load(object sender, EventArgs e)
        {
            _subjectOffering = await clsSubjectOffering.FindAsync(_offeringID);

            //==================================================
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

            cbDoctors2.DataSource = await clsDoctor.GetAllAsync();
            cbDoctors2.ValueMember = "DoctorID";
            cbDoctors2.DisplayMember = "DoctorName";

            DisplayOfferingData();
        }

        private void DisplayOfferingData()
        {
            cbLevels.SelectedValue = _subjectOffering.LevelID;
            cbSemesters.Text = _subjectOffering.Semester;
            cbDepartments.SelectedValue = _subjectOffering.DepartmentID;
            cbSubjects.SelectedValue = _subjectOffering.SubjectID;
            cbDoctors.SelectedValue = _subjectOffering.DoctorID;
            cbDoctors2.SelectedValue = _subjectOffering.Doctor2ID;
        }
        private void LoadSubjectOfferingInfoFromFormToSubjectOfferingObject()
        {
            _subjectOffering.SubjectID = Convert.ToInt32(cbSubjects.SelectedValue);
            _subjectOffering.LevelID = Convert.ToInt32(cbLevels.SelectedValue);
            _subjectOffering.DepartmentID = Convert.ToInt32(cbDepartments.SelectedValue);
            _subjectOffering.DoctorID = Convert.ToInt32(cbDoctors.SelectedValue);
            _subjectOffering.Doctor2ID = Convert.ToInt32(cbDoctors2.SelectedValue);
            _subjectOffering.Semester = cbSemesters.Text.Trim();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            LoadSubjectOfferingInfoFromFormToSubjectOfferingObject();

            if (!await _subjectOffering.SaveAsync())
            {
                clsMessageBoxHelper.ShowWarning("حدث خطأ أثناء محاولة حفظ بيانات تعيين المواد");
                return;
            }

            await SaveCompleted?.Invoke();
            clsMessageBoxHelper.ShowWarning("تم الحفظ بنجاج");
        }

    }
}

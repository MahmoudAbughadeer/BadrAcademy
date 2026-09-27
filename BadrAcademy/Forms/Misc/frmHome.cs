using BadrAcademy.Forms.Departments.Controls;
using BadrAcademy.Forms.Doctors.Controls;
using BadrAcademy.Forms.Halls.Controls;
using BadrAcademy.Forms.Levels.Controls;
using BadrAcademy.Forms.Misc.Controls;
using BadrAcademy.Forms.Settings.Controls;
using BadrAcademy.Forms.SubjectAssignments.Controls;
using BadrAcademy.Forms.Subjects.Controls;
using BadrAcademy.Forms.Users;
using BadrAcademy.Forms.Users.Controls;
using BadrAcademy.Global;
using BadrAcademy.Helpers;
using System;
using System.Windows.Forms;

namespace BadrAcademy.Forms.Misc
{
    public partial class frmHome : Form
    {
        public frmHome()
        {
            InitializeComponent();
        }

        //Private Methods
        public void OpenPage(UserControl page)
        {
            pPagePlace.Controls.Clear();
            page.Dock = DockStyle.Fill;
            pPagePlace.Controls.Add(page);
        }

        private void frmHome2_Load(object sender, EventArgs e)
        {
            lblCurrentUsername.Text = clsGlobal.CurrentUser.Username;
            lblCurrentUserFullName.Text = clsGlobal.CurrentUser.FullName;
        }


        //Tool strip menu items event
        private void tsmiHome_Click(object sender, EventArgs e)
        {

        }

        private void tsmiAcademicYear_Click(object sender, EventArgs e)
        {
            OpenPage(new ctrSettingsPage());
        }

        private void tsmiLevels_Click(object sender, EventArgs e)
        {
            OpenPage(new ctrLevelsPage());
        }

        private void tsmiDepartments_Click(object sender, EventArgs e)
        {
            OpenPage(new ctrDepartmentsPage());
        }

        private void tsmiHalls_Click(object sender, EventArgs e)
        {
            OpenPage(new ctrHallsPage());
        }

        private void tsmiDoctors_Click(object sender, EventArgs e)
        {
            OpenPage(new ctrDoctorsPage());
        }
        private void tsmiSubjects_Click(object sender, EventArgs e)
        {
            OpenPage(new ctrSubjectsPage());
        }

        private void tsmiSubjectsAssignment_Click(object sender, EventArgs e)
        {
            OpenPage(new ctrSubjectOfferingsPage());
        }

        private void tsmiExamScchedule_Click(object sender, EventArgs e)
        {

        }

        private void tsmiDistributions_Click(object sender, EventArgs e)
        {

        }

        private void tsmiUsers_Click(object sender, EventArgs e)
        {
            OpenPage(new ctrUsersPage());
        }

        private void tsmiCurrentUserInfo_Click(object sender, EventArgs e)
        {
            clsOpenFormHelper.ShowDialogCenteredInContainer(new frmShowUserInfo(clsGlobal.CurrentUser.UserID.Value), pPagePlace);
        }

        private void tsmiChagnePassword_Click(object sender, EventArgs e)
        {
            clsOpenFormHelper.ShowDialogCenteredInContainer(new frmChangeUserPassword(clsGlobal.CurrentUser.UserID.Value), pPagePlace);
        }

        private void tsmiLogout_Click(object sender, EventArgs e)
        {
            clsGlobal.CurrentUser = null;
            this.DialogResult = DialogResult.OK;//To flag the signout proccesse
            this.Close();
        }


    }
}

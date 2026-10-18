using BLL;
using BadrAcademy.Helpers;
using Shared.Models;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BadrAcademy.Forms.SubjectAssignments.Controls
{
    public partial class ctrSubjectOfferingCard : UserControl
    {
        public clsOfferingDetailsDto OfferingDetail { get; set; } = null;
        public ctrSubjectOfferingCard()
        {
            InitializeComponent();
        }

        public async Task LoadInfo(int offeringID)
        {
            OfferingDetail = await clsAsyncMethodExecutor.RunAsync(() => clsSubjectOffering.FindWithNamesAsync(offeringID));

            if (OfferingDetail == null)
                return;

            lblOfferingID.Text = OfferingDetail.OfferingID.ToString();
            lblSubjectName.Text = OfferingDetail.SubjectName.Trim();
            lblLevelName.Text = OfferingDetail.LevelName.Trim();
            lblDepartmentName.Text = OfferingDetail.DepartmentName.Trim();
            lblSemesterName.Text = OfferingDetail.Semester.Trim();
            lblDoctorName.Text = OfferingDetail.DoctorName.Trim();
        }
    }
}

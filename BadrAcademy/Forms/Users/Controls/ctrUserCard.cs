using BLL;
using BadrAcademy.Helpers;

using System.Threading.Tasks;
using System.Windows.Forms;

namespace BadrAcademy.Forms.Users.Controls
{
    public partial class ctrUserCard : UserControl
    {
        public clsUser User;
        public ctrUserCard()
        {
            InitializeComponent();
        }

        public async Task LoadInfo(int userID)
        {
            User = await clsAsyncMethodExecutor.RunAsync(() => clsUser.FindAsync(userID));

            if (User == null)
                return;

            lblUserID.Text = User.UserID.ToString();
            lblName.Text = User.FullName;
            lblUserName.Text = User.Username;
            rbActive.Checked = User.IsActive;
            rbNotActive.Checked = !User.IsActive;
        }
    }
}

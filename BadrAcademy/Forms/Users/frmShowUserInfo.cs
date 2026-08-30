using BadrAcademy.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BadrAcademy.Forms.Users
{
    public partial class frmShowUserInfo : Form
    {
        int _userID;
        public frmShowUserInfo(int userID)
        {
            InitializeComponent();
            _userID = userID;
        }

        private async void frmShowUserInfo_Load(object sender, EventArgs e)
        {
            await clsAsyncMethodExecutor.RunAsync(() => ctrUserCard1.LoadInfo(_userID));
        }
    }
}

using BadrAcademy.Forms.Misc;
using BLL;
using BLL.Utilities;
using System;
using System.Windows.Forms;

namespace BadrAcademy
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Initialize Database
            clsSystemManager.InitializeSystem();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmLogin());
        }
    }
}

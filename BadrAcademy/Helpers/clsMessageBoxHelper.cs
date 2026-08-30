using System.Windows.Forms;

namespace BadrAcademy.Helpers
{
    public static class clsMessageBoxHelper
    {
        private static string AppName => "MedCenter";

        public static void ShowInfo(string message, string title = null)
            => MessageBox.Show(message, title?? AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

        public static void ShowWarning(string message, string title = null)
            => MessageBox.Show(message, title ?? AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);

        public static void ShowError(string message, string title = null)
            => MessageBox.Show(message, title ?? AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);

        public static DialogResult ShowQuestion(string message, string title = null)
            => MessageBox.Show(message, title ?? AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

        public static DialogResult ShowConfirmation(string message, string title = null)
            => MessageBox.Show(message, title ?? AppName, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2); 
    }
}

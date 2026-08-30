
using System.Windows.Forms;

namespace BadrAcademy.Helpers
{
    static internal class clsButtonHelper
    {
        static public void SetEditDelteButtonsState(DataGridView dgv, Button edit, Button delete, Button show = null)
        {
            if (show == null)
            {
                edit.Enabled = delete.Enabled = clsDataGridViewHelper.IsThereAnyEntries(dgv);
                return;
            }
            edit.Enabled = delete.Enabled = show.Enabled = clsDataGridViewHelper.IsThereAnyEntries(dgv);
        }
    }
}

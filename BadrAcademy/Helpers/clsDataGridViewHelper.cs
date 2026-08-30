using System;
using System.Windows.Forms;

namespace BadrAcademy.Helpers
{
    internal static class clsDataGridViewHelper
    {
        static public int GetCellValueAsInt(DataGridView dgv, string columnName)
        {
            return Convert.ToInt32(dgv.CurrentRow.Cells[columnName].Value);
        }

        static public bool IsThereAnyEntries(DataGridView dgv)
        {
            return dgv.Rows.Count > 0;
        }

        static public bool HasEmptyVisibleArea(DataGridView dgv)
        {

            int totalDisplayRowsHeight = 0;

            foreach(DataGridViewRow row in dgv.Rows)
            {
                if (row.Displayed)
                    totalDisplayRowsHeight += row.Height;
                else
                    break;
            }


            return totalDisplayRowsHeight + dgv.ColumnHeadersHeight < dgv.ClientSize.Height;
        }
    
    
    
    }
}

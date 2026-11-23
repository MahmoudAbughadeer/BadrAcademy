using ClosedXML.Excel;
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace Shared.Utilities
{
    public static class clsExcelExporter
    {
        public static Task ExportAsync(string outputPath, DataTable rows)
        {
            return Task.Run(() => Export(outputPath, rows));
        }

        // Writes any DataTable to a new .xlsx file: headers come from the column names.
        public static void Export(string outputPath, DataTable rows)
        {
            if (rows == null)
                throw new ArgumentNullException(nameof(rows));

            string dir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);

            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Sheet1");
                ws.RightToLeft = true;                          // Arabic layout

                int colCount = rows.Columns.Count;

                if (rows.Rows.Count > 0)
                {
                    // headers + data, as a formatted Excel table with filter buttons
                    ws.Cell(1, 1).InsertTable(rows, "Data", true);
                }
                else
                {
                    // nothing to export: write the headers only
                    for (int c = 0; c < colCount; c++)
                        ws.Cell(1, c + 1).Value = rows.Columns[c].ColumnName;
                }

                var all = ws.Range(1, 1, Math.Max(rows.Rows.Count + 1, 1), colCount);
                all.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                all.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                all.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                all.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                ws.Row(1).Style.Font.Bold = true;
                ws.SheetView.FreezeRows(1);
                ws.Columns().AdjustToContents();

                // print setup
                ws.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                ws.PageSetup.FitToPages(1, 0);                  // fit the width to one page
                ws.PageSetup.SetRowsToRepeatAtTop(1, 1);

                wb.SaveAs(outputPath);
            }
        }
    }
}
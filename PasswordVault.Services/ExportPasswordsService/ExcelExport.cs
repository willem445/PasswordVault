using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OfficeOpenXml;

namespace PasswordVault.Services
{
    class ExcelExport : IExport
    {
        public ExportResult Export(string exportPath)
        {
            ExportResult result = ExportResult.Fail;

            using (var p = new ExcelPackage())
            {
                var ws = p.Workbook.Worksheets.Add("Passwords");
                ws.Cells["A1"].Value = "This is cell A1";
                p.SaveAs(new FileInfo(@"C:\Users\WillH\Desktop\PasswordBackup.xlsx"));
            }

            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using OfficeOpenXml;
using PasswordVault.Models;

namespace PasswordVault.Services
{
    class ExcelExport : IExport
    {
        public ExportResult Export(string exportPath, List<Password> passwords)
        {
            ExportResult result = ExportResult.Fail;

            using (var p = new ExcelPackage())
            {
                var ws = p.Workbook.Worksheets.Add("Passwords");

                PropertyInfo[] properties = typeof(Password).GetProperties();

                ws.Cells[0, 0].Value = "This is cell A1";
                p.SaveAs(new FileInfo(exportPath));
            }

            return result;
        }
    }
}

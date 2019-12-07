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
        public ExportResult Export(string exportPath, List<Password> passwords, string encryptionPassword)
        {
            ExportResult result = ExportResult.Fail;

            using (var p = new ExcelPackage())
            {
                // TODO - Add workbook encryption
                var wb = p.Workbook;
                wb.Protection.SetPassword("Password");
                p.Encryption.IsEncrypted = true;
                p.Encryption.Algorithm = EncryptionAlgorithm.AES256;
                var ws = p.Workbook.Worksheets.Add("Passwords");
                ws.Protection.SetPassword("Password");

                int headerColCount = 1;
                int rowCount = 1;
                List<string> propertyNames = new List<string>();
                PropertyInfo[] properties = typeof(Password).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    propertyNames.Add(property.Name);

                    ws.Cells[rowCount, headerColCount].Value = property.Name;

                    headerColCount++;
                }

                rowCount = 2;
                foreach (var password in passwords)
                {
                    int tempColCount = 1;

                    foreach (var propertyName in propertyNames)
                    {
                        ws.Cells[rowCount, tempColCount].Value = password.GetType().GetProperty(propertyName).GetValue(password);

                        tempColCount++;
                    }

                    rowCount++;
                }
                  
                p.SaveAs(new FileInfo(exportPath), "Password");
            }

            return result;
        }
    }
}

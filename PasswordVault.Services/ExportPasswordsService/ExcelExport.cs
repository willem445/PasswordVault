﻿using System;
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
        public ExportResult Export(string exportPath, List<Password> passwords, string encryptionPassword, bool passwordEnabled)
        {
            ExportResult result = ExportResult.Success;

            try
            {
                using (var p = new ExcelPackage())
                {
                    var wb = p.Workbook;
                    var ws = p.Workbook.Worksheets.Add("Passwords");

                    if (passwordEnabled)
                    {
                        wb.Protection.SetPassword(encryptionPassword);
                        p.Encryption.IsEncrypted = true;
                        p.Encryption.Algorithm = OfficeOpenXml.EncryptionAlgorithm.AES256;
                    }
                    
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

                    if (passwordEnabled)
                    {
                        p.SaveAs(new FileInfo(exportPath), encryptionPassword);
                    }
                    else
                    {
                        p.SaveAs(new FileInfo(exportPath));
                    }
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch
#pragma warning restore CA1031 // Do not catch general exception types
            {
                result = ExportResult.Fail;
            }
            
            return result;
        }
    }
}

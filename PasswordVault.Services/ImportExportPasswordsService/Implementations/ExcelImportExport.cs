using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using OfficeOpenXml;
using PasswordVault.Models;

namespace PasswordVault.Services
{
    class ExcelImportExport : IImportExport
    {
        public ImportExportResult Export(string exportPath, List<Password> passwords, string encryptionPassword=null, bool passwordEnabled=false)
        {
            ImportExportResult result = ImportExportResult.Success;

            if (string.IsNullOrEmpty(exportPath))
            {
                return ImportExportResult.InvalidPath;
            }

            if (passwordEnabled && string.IsNullOrEmpty(encryptionPassword))
            {
                return ImportExportResult.PasswordInvalid;
            }

            if (passwords == null || passwords.Count == 0)
            {
                return ImportExportResult.Fail;
            }

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
                result = ImportExportResult.Fail;
            }
            
            return result;
        }

        public ImportResult Import(string path, string passphrase=null, bool passwordEnabled=false)
        {
            ImportExportResult result = ImportExportResult.Success;
            List<Password> passwords = new List<Password>();
            Dictionary<string, int> headerIdx = new Dictionary<string, int>();

            if (!File.Exists(path))
            {
                return new ImportResult(ImportExportResult.InvalidPath, null);
            }

            if (passwordEnabled && string.IsNullOrEmpty(passphrase))
            {
                return new ImportResult(ImportExportResult.PasswordInvalid, null);
            }

            FileInfo file = new FileInfo(path);
            ExcelPackage excel = null;
            if (passwordEnabled)
            {
                try
                {
                    excel = new ExcelPackage(file, passphrase);
                }                 
                catch(System.Security.SecurityException e)
                {
                    if (e.Message == "Invalid password")
                    {
                        result = ImportExportResult.PasswordInvalid;
                    }
                }
                catch(System.IO.InvalidDataException e)
                {
                    if (e.Message.Contains("is not an encrypted package"))
                    {
                        excel = new ExcelPackage(file);
                    }
                }
            }
            else
            {
                try
                {
                    excel = new ExcelPackage(file);
                }
                catch (System.Exception e)
                {
                    if (e.Message == "Can not open the package. Package is an OLE compound document. If this is an encrypted package, please supply the password")
                    {
                        result = ImportExportResult.PasswordProtected;
                    }
                }            
            }
         
            if (result == ImportExportResult.Success)
            {
                try
                {
                    var wb = excel.Workbook;
                    var ws = excel.Workbook.Worksheets["Passwords"];

                    bool exit = false;
                    int col = 1;
                    while (exit == false)
                    {
                        var header = Convert.ToString(ws.Cells[1, col].Value);

                        if (string.IsNullOrEmpty(header))
                        {
                            exit = true;
                        }
                        else
                        {
                            headerIdx[header] = col;
                            col++;
                        }
                    }

                    exit = false;
                    int row = 2;

                    while (exit == false)
                    {
                        var uniqueID = ws.Cells[row, headerIdx["UniqueID"]].Value;

                        if (uniqueID == null)
                        {
                            exit = true;
                        }
                        else
                        {
                            string cell = (string)ws.Cells[row, headerIdx["Application"]].Value;
                            var application = cell != null ? cell : "";
                            cell = (string)ws.Cells[row, headerIdx["Username"]].Value;
                            var username = cell != null ? cell : "";
                            cell = (string)ws.Cells[row, headerIdx["Email"]].Value;
                            var email = cell != null ? cell : "";
                            cell = (string)ws.Cells[row, headerIdx["Description"]].Value;
                            var description = cell != null ? cell : "";
                            cell = (string)ws.Cells[row, headerIdx["Website"]].Value;
                            var website = cell != null ? cell : "";
                            cell = (string)ws.Cells[row, headerIdx["Passphrase"]].Value;
                            var password = cell != null ? cell : "";

                            var passwordObj = new Password(application, username, email, description, website, password);
                            passwords.Add(passwordObj);

                            row++;
                        }
                    }
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    result = ImportExportResult.Fail;
                }
            }

            if (excel != null)
            {
                excel.Dispose();
            }

            return new ImportResult(result, passwords);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PasswordVault.Models;
using PasswordVault.Services;

namespace PasswordVault.ServicesTests
{
    [TestClass]
    public class ImportExportTest : PasswordServiceTestBase
    {
        private string ExportTestPath = Path.Combine(Environment.CurrentDirectory, @"..\..\..\PasswordVault.Data\TestDb") + "\\ExportTest.xlsx";

        [TestMethod]
        public void ExcelImportExportEncryptedTest()
        {
            string pass = passwordService.GeneratePassword();
            int user0PasswordsCount;
            int user1PasswordsCount;
            List<Password> user0Passwords;
            List<Password> user1Passwords;


            Services.ImportExportResult exportResult = passwordService.ExportPasswords(Services.ImportExportFileType.Excel, ExportTestPath, pass, true);
            Assert.AreEqual(Services.ImportExportResult.Success, exportResult);

            user0PasswordsCount = passwordService.GetPasswordCount();
            Assert.AreEqual(20, user0PasswordsCount);

            Services.ImportExportResult importResult = passwordService.ImportPasswords(Services.ImportExportFileType.Excel, ExportTestPath, pass, true);
            Assert.AreEqual(Services.ImportExportResult.Success, importResult); // Succeeds because no passwords are actually added to the db since they already exist

            user0Passwords = new List<Password>(passwordService.GetPasswords().ToArray()); // since the list is a reference type, we need to deep copy
            user0PasswordsCount = passwordService.GetPasswordCount();
            Assert.AreEqual(20, user0PasswordsCount);

            Logout();
            CreateAccount(TestUsers[1]);
            Login(TestUsers[1].Username, TestUsers[1].PlainTextPassword);
            user1PasswordsCount = passwordService.GetPasswordCount();
            Assert.AreEqual(0, user1PasswordsCount);
            importResult = passwordService.ImportPasswords(Services.ImportExportFileType.Excel, ExportTestPath, pass, true);
            user1PasswordsCount = passwordService.GetPasswordCount();
            user1Passwords = passwordService.GetPasswords();
            Assert.AreEqual(Services.ImportExportResult.Success, exportResult);
            Assert.AreEqual(20, user1PasswordsCount);
            Assert.AreEqual(user0PasswordsCount, user1PasswordsCount);

            for (int i = 0; i < 20; i++)
            {
                if (!user0Passwords[i].Equals(user1Passwords[i]))
                {
                    Assert.Fail();
                }
            }

            if (File.Exists(ExportTestPath))
            {
                File.Delete(ExportTestPath);
            }
        }

        [TestMethod]
        public void ExportImportPasswordProtectionTest()
        {
            ImportExportResult importResult;
            string pass = passwordService.GeneratePassword();
            int user0PasswordsCount;

            ImportExportResult exportResult = passwordService.ExportPasswords(Services.ImportExportFileType.Excel, ExportTestPath, pass, true);
            Assert.AreEqual(Services.ImportExportResult.Success, exportResult);

            user0PasswordsCount = passwordService.GetPasswordCount();
            Assert.AreEqual(20, user0PasswordsCount);
            DeleteUserPasswords();

            importResult = passwordService.ImportPasswords(Services.ImportExportFileType.Excel, ExportTestPath);
            Assert.AreEqual(Services.ImportExportResult.PasswordProtected, importResult);

            importResult = passwordService.ImportPasswords(Services.ImportExportFileType.Excel, ExportTestPath, pass+"1", true);
            Assert.AreEqual(Services.ImportExportResult.PasswordInvalid, importResult);

            importResult = passwordService.ImportPasswords(Services.ImportExportFileType.Excel, ExportTestPath, null, true);
            Assert.AreEqual(Services.ImportExportResult.PasswordInvalid, importResult);

            importResult = passwordService.ImportPasswords(Services.ImportExportFileType.Excel, ExportTestPath, "", true);
            Assert.AreEqual(Services.ImportExportResult.PasswordInvalid, importResult);

            if (File.Exists(ExportTestPath))
            {
                File.Delete(ExportTestPath);
            }
        }

        [TestMethod]
        public void ExportImportPasswordArgumentTest()
        {
            ImportExportResult result;

            // Test path
            result = passwordService.ExportPasswords(Services.ImportExportFileType.Excel, null, "", false);
            Assert.AreEqual(Services.ImportExportResult.InvalidPath, result);

            result = passwordService.ExportPasswords(Services.ImportExportFileType.Excel, "", "", false);
            Assert.AreEqual(Services.ImportExportResult.InvalidPath, result);

            // Test password
            result = passwordService.ExportPasswords(Services.ImportExportFileType.Excel, ExportTestPath, "", true);
            Assert.AreEqual(Services.ImportExportResult.PasswordInvalid, result);

            result = passwordService.ExportPasswords(Services.ImportExportFileType.Excel, ExportTestPath, null, true);
            Assert.AreEqual(Services.ImportExportResult.PasswordInvalid, result);

            // Test invalid path, the excel file should not exist yet
            result = passwordService.ImportPasswords(ImportExportFileType.Excel, ExportTestPath);
            Assert.AreEqual(Services.ImportExportResult.InvalidPath, result);

            // Test invalid passwords, need to perform successful export first
            passwordService.ExportPasswords(Services.ImportExportFileType.Excel, ExportTestPath, "1", true);
            result = passwordService.ImportPasswords(ImportExportFileType.Excel, ExportTestPath, null, true);
            Assert.AreEqual(Services.ImportExportResult.PasswordInvalid, result);

            result = passwordService.ImportPasswords(ImportExportFileType.Excel, ExportTestPath, "", true);
            Assert.AreEqual(Services.ImportExportResult.PasswordInvalid, result);

            if (File.Exists(ExportTestPath))
            {
                File.Delete(ExportTestPath);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PasswordVault.Models;
using PasswordVault.Services;

namespace PasswordVault.ServicesTests
{
    [TestClass]
    public class ImportExportTest : PasswordServiceTestBase
    {
        [TestMethod]
        public void ExcelImportExportEncryptedTest()
        {
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\..\PasswordVault.Data\TestDb") + "\\ExportTest.xlsx";
            string pass = passwordService.GeneratePasswordKey();
            Services.ImportExportResult exportResult = passwordService.ExportPasswords(Services.ImportExportFileType.Excel, path, pass, true);
            Assert.AreEqual(Services.ImportExportResult.Success, exportResult);

            var user0PasswordsCount = passwordService.GetPasswordCount();
            Assert.AreEqual(20, user0PasswordsCount);

            Services.ImportExportResult importResult = passwordService.ImportPasswords(Services.ImportExportFileType.Excel, path, pass, true);
            Assert.AreEqual(Services.ImportExportResult.Success, importResult); // Succeeds because no passwords are actually added to the db since they already exist

            List<Password> user0Passwords = new List<Password>(passwordService.GetPasswords().ToArray()); // since the list is a reference type, we need to deep copy
            user0PasswordsCount = passwordService.GetPasswordCount();
            Assert.AreEqual(20, user0PasswordsCount);

            Logout();
            CreateAccount(TestUsers[1]);
            Login(TestUsers[1].Username, TestUsers[1].PlainTextPassword);
            var user1PasswordsCount = passwordService.GetPasswordCount();
            Assert.AreEqual(0, user1PasswordsCount);
            importResult = passwordService.ImportPasswords(Services.ImportExportFileType.Excel, path, pass, true);
            user1PasswordsCount = passwordService.GetPasswordCount();
            var user1Passwords = passwordService.GetPasswords();
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
        }
    }
}

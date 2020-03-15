using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PasswordVault.Data;
using PasswordVault.Services;
using PasswordVault.Models;
using System.Diagnostics;
using PasswordVault.Desktop.Winforms;

namespace PasswordVault.ServicesTests
{
    /// <summary>
    /// Summary description for AddPasswordTest
    /// </summary>
    [TestClass]
    public class PasswordCountTests : PasswordServiceTestBase
    {
        /// <summary>
        /// Tests that the get password count feature works. 
        /// 
        /// Pass Criteria:
        /// - Password count matches number of passwords being stored in DB.
        /// </summary>
        [TestMethod]
        public void PasswordCountTest()
        {
            DeleteUserPasswords();

            for (int i = 0; i < ValidTestPasswords.Count; i++)
            {
                Password password = GetPassword(i);
                addModifyPasswordResult = passwordService.AddPassword(password);
                Assert.AreEqual(ValidatePassword.Success, addModifyPasswordResult);
                Assert.AreEqual(i+1, ((InMemoryDatabase)db).LocalPasswordDbAccess.Count);
                Assert.AreEqual(i+1, passwordService.GetPasswordCount());
            }
        }
    }
}

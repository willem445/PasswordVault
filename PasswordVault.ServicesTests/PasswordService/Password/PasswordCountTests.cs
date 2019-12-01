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
        /// 
        /// 
        /// Pass Criteria:
        /// - 
        /// </summary>
        [TestMethod]
        public void PasswordCountTest()
        {
            for (int i = 0; i < ValidTestPasswords.Count; i++)
            {
                Password password = GetTestPassword(i);
                addPasswordResult = passwordService.AddPassword(password);
                Assert.AreEqual(AddModifyPasswordResult.Success, addPasswordResult);
                Assert.AreEqual(i+1, ((InMemoryDatabase)db).LocalPasswordDbAccess.Count);
                Assert.AreEqual(i+1, passwordService.GetPasswordCount());
            }
        }
    }
}

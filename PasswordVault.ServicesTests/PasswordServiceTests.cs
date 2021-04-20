using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PasswordVault.Data;
using PasswordVault.Services;
using PasswordVault.Models;
using PasswordVault.Desktop.Winforms;

namespace PasswordVault.ServicesTests
{
    /// <summary>
    /// Summary description for PasswordServiceTests
    /// </summary>
    [TestClass]
    public class PasswordServiceTests
    {
        IDatabase db;
        IDesktopServiceWrapper passwordService;
        AddUserResult createUserResult;
        AuthenticateResult loginResult;
        LogOutResult logoutResult;
        User user;

        public PasswordServiceTests()
        {

        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        [TestInitialize()]
        public void MyTestInitialize()
        {
            db = DatabaseFactory.GetDatabase(Database.InMemory);
            passwordService = DesktopPasswordServiceBuilder.BuildDesktopServiceWrapper(db);

            user = new User("testAccount", "testPassword1@aaaaaaaaa", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.Successful, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);
        }
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void GetCurrentUserTest()
        {
            loginResult = passwordService.Login("testAccount", "testPassword1@aaaaaaaaa");
            Assert.AreEqual(AuthenticateResult.Successful, loginResult);

            User user = passwordService.GetCurrentUser();
            Assert.AreEqual("testAccount", user.Username);
            Assert.AreEqual("testFirstName", user.FirstName);
            Assert.AreEqual("testLastName", user.LastName);
            Assert.AreEqual("222-111-1111", user.PhoneNumber);
            Assert.AreEqual("test@test.com", user.Email);
            Assert.AreEqual(true, user.ValidUser);

            logoutResult = passwordService.Logout();
            Assert.AreEqual(LogOutResult.Success, logoutResult);

            user = passwordService.GetCurrentUser();
            Assert.AreEqual(false, user.ValidUser);
        }

        [TestMethod]
        public void GetCurrentUserNameTest()
        {
            loginResult = passwordService.Login("testAccount", "testPassword1@aaaaaaaaa");
            Assert.AreEqual(AuthenticateResult.Successful, loginResult);

            string username = passwordService.GetCurrentUsername();
            Assert.AreEqual("testAccount", username);

            logoutResult = passwordService.Logout();
            Assert.AreEqual(LogOutResult.Success, logoutResult);

            username = passwordService.GetCurrentUsername();
            Assert.AreEqual("", username);
        }

        [TestMethod]
        public void VerifyCurrentUserPasswordTest()
        {
            loginResult = passwordService.Login("testAccount", "testPassword1@aaaaaaaaa");
            Assert.AreEqual(AuthenticateResult.Successful, loginResult);

            bool verifyResult;

            verifyResult = passwordService.VerifyCurrentUserPassword("testPassword1@aaaaaaaaa");
            Assert.AreEqual(true, verifyResult);

            verifyResult = passwordService.VerifyCurrentUserPassword("testPassword1");
            Assert.AreEqual(false, verifyResult);

            verifyResult = passwordService.VerifyCurrentUserPassword("");
            Assert.AreEqual(false, verifyResult);

            verifyResult = passwordService.VerifyCurrentUserPassword(null);
            Assert.AreEqual(false, verifyResult);

            logoutResult = passwordService.Logout();
            Assert.AreEqual(LogOutResult.Success, logoutResult);

            verifyResult = passwordService.VerifyCurrentUserPassword("testPassword1");
            Assert.AreEqual(false, verifyResult);
        }

        [TestMethod]
        public void GeneratePasswordKeyTest()
        {
            string passphrase = passwordService.GeneratePassword();
            Assert.AreEqual(20, passphrase.Length);
        }

        [TestMethod]
        public void GetMinimumPasswordTest()
        {
            var num = passwordService.GetMinimumPasswordLength();
            Assert.AreEqual(15, num);
        }
    }
}

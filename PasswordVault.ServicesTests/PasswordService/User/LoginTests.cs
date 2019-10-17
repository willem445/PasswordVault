using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PasswordVault.Data;
using PasswordVault.Services;
using PasswordVault.Models;

namespace PasswordVault.ServicesTests
{
    /// <summary>
    /// Summary description for LoginTests
    /// </summary>
    [TestClass]
    public class LoginTests
    {
        IDatabase db;
        IPasswordService passwordService;
        CreateUserResult createUserResult;
        LoginResult loginResult;
        LogOutResult logoutResult;
        AddPasswordResult addPasswordResult;
        User user;

        public LoginTests()
        {
            //
            // TODO: Add constructor logic here
            //
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
        //
        [TestInitialize()]
        public void MyTestInitialize()
        {
            db = DatabaseFactory.GetDatabase(Database.InMemory);
            passwordService = new PasswordService(db, new MasterPassword(), new RijndaelManagedEncryption());

            user = new User("testAccount", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.Successful, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);
        }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void LoginTest()
        {
            loginResult = passwordService.Login("testAccount", "testPassword1@");
            Assert.AreEqual(LoginResult.Successful, loginResult);

            // Test logging into already logged in account
            loginResult = passwordService.Login("testAccount", "testPassword1@");
            Assert.AreEqual(LoginResult.Failed, loginResult);

            logoutResult = passwordService.Logout();
            Assert.AreEqual(LogOutResult.Success, logoutResult);

            // Test null and empty args
            loginResult = passwordService.Login("", "testPassword1@");
            Assert.AreEqual(LoginResult.Failed, loginResult);

            loginResult = passwordService.Login("testAccount", "");
            Assert.AreEqual(LoginResult.Failed, loginResult);

            loginResult = passwordService.Login("testAccount", "");
            Assert.AreEqual(LoginResult.Failed, loginResult);

            loginResult = passwordService.Login(null, "testPassword1@");
            Assert.AreEqual(LoginResult.Failed, loginResult);

            loginResult = passwordService.Login("testAccount", null);
            Assert.AreEqual(LoginResult.Failed, loginResult);

            loginResult = passwordService.Login(null, null);
            Assert.AreEqual(LoginResult.Failed, loginResult);

            // Test non existent account
            loginResult = passwordService.Login("test", "testPassword1@");
            Assert.AreEqual(LoginResult.UsernameDoesNotExist, loginResult);
        }
    }
}

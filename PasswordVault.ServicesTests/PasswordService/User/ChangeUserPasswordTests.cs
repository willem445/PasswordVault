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
    /// Summary description for ChangePasswordTests
    /// </summary>
    [TestClass]
    public class ChangePasswordTests
    {
        IDatabase db;
        IPasswordService passwordService;

        public ChangePasswordTests()
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

        [TestInitialize()]
        public void MyTestInitialize()
        {
            db = DatabaseFactory.GetDatabase(Database.InMemory);
            passwordService = new PasswordService(db, new MasterPassword(), new RijndaelManagedEncryption());
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            ((InMemoryDatabase)db).LocalPasswordDbAccess.Clear();
            ((InMemoryDatabase)db).LocalUserDbAccess.Clear();
            passwordService.Logout();
        }
        #endregion

        [TestMethod]
        public void ChangeUserPasswordTest()
        {
            CreateUserResult createUserResult;
            ChangeUserPasswordResult changeUserPasswordResult;
            LoginResult loginResult;
            LogOutResult logoutResult;
            User user;

            user = new User("testAccount", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.Successful, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            // Not logged in
            changeUserPasswordResult = passwordService.ChangeUserPassword("testPassword1@", "testPassword1@2", "testPassword1@2");
            Assert.AreEqual(ChangeUserPasswordResult.Failed, changeUserPasswordResult);

            loginResult = passwordService.Login("testAccount", "testPassword1@");
            Assert.AreEqual(LoginResult.Successful, loginResult);

            changeUserPasswordResult = passwordService.ChangeUserPassword("testPassword1@", "testPassword1@2", "testPassword1@2");
            Assert.AreEqual(ChangeUserPasswordResult.Success, changeUserPasswordResult);

            logoutResult = passwordService.Logout();
            Assert.AreEqual(LogOutResult.Success, logoutResult);

            loginResult = passwordService.Login("testAccount", "testPassword1@");
            Assert.AreEqual(LoginResult.PasswordIncorrect, loginResult);

            loginResult = passwordService.Login("testAccount", "testPassword1@2");
            Assert.AreEqual(LoginResult.Successful, loginResult);

            // Test null and empty
            changeUserPasswordResult = passwordService.ChangeUserPassword("", "1@3aA", "1@3aA");
            Assert.AreEqual(ChangeUserPasswordResult.Failed, changeUserPasswordResult);

            changeUserPasswordResult = passwordService.ChangeUserPassword(null, "1@3aA", "1@3aA");
            Assert.AreEqual(ChangeUserPasswordResult.Failed, changeUserPasswordResult);

            changeUserPasswordResult = passwordService.ChangeUserPassword("testPassword1@2", "", "1@3aA");
            Assert.AreEqual(ChangeUserPasswordResult.Failed, changeUserPasswordResult);

            changeUserPasswordResult = passwordService.ChangeUserPassword("testPassword1@2", null, "1@3aA");
            Assert.AreEqual(ChangeUserPasswordResult.Failed, changeUserPasswordResult);

            changeUserPasswordResult = passwordService.ChangeUserPassword("testPassword1@2", "1@3aA", "");
            Assert.AreEqual(ChangeUserPasswordResult.Failed, changeUserPasswordResult);

            changeUserPasswordResult = passwordService.ChangeUserPassword("testPassword1@2", "1@3aA", null);
            Assert.AreEqual(ChangeUserPasswordResult.Failed, changeUserPasswordResult);

            changeUserPasswordResult = passwordService.ChangeUserPassword(null, null, null);
            Assert.AreEqual(ChangeUserPasswordResult.Failed, changeUserPasswordResult);

            changeUserPasswordResult = passwordService.ChangeUserPassword("", "", "");
            Assert.AreEqual(ChangeUserPasswordResult.Failed, changeUserPasswordResult);

            // Test invalid password
            changeUserPasswordResult = passwordService.ChangeUserPassword("testPassword1@", "1@3aA", "1@3aA");
            Assert.AreEqual(ChangeUserPasswordResult.InvalidPassword, changeUserPasswordResult);

            // Test length requirement
            changeUserPasswordResult = passwordService.ChangeUserPassword("testPassword1@2", "1@3aA", "1@3aA");
            Assert.AreEqual(ChangeUserPasswordResult.LengthRequirementNotMet, changeUserPasswordResult);

            changeUserPasswordResult = passwordService.ChangeUserPassword("testPassword1@2", "1@3AAAAAAAAAAAAA", "1@3AAAAAAAAAAAAA");
            Assert.AreEqual(ChangeUserPasswordResult.NoLowerCaseCharacter, changeUserPasswordResult);

            changeUserPasswordResult = passwordService.ChangeUserPassword("testPassword1@2", "a@GAAAAAAAAAAAAA", "a@GAAAAAAAAAAAAA");
            Assert.AreEqual(ChangeUserPasswordResult.NoNumber, changeUserPasswordResult);

            changeUserPasswordResult = passwordService.ChangeUserPassword("testPassword1@2", "1A3aaaaaaaaaaaaa", "1A3aaaaaaaaaaaaa");
            Assert.AreEqual(ChangeUserPasswordResult.NoSpecialCharacter, changeUserPasswordResult);

            changeUserPasswordResult = passwordService.ChangeUserPassword("testPassword1@2", "1@3aaaaaaaaaaaaa", "1@3aaaaaaaaaaaaa");
            Assert.AreEqual(ChangeUserPasswordResult.NoUpperCaseCharacter, changeUserPasswordResult);

            changeUserPasswordResult = passwordService.ChangeUserPassword("testPassword1@2", "1@3aaaaaaaaaaaaa", "1@3aaaaaaaaaaaaaZ");
            Assert.AreEqual(ChangeUserPasswordResult.PasswordsDoNotMatch, changeUserPasswordResult);

            changeUserPasswordResult = passwordService.ChangeUserPassword("testPassword1@2", "", "");
            Assert.AreEqual(ChangeUserPasswordResult.Failed, changeUserPasswordResult);

            changeUserPasswordResult = passwordService.ChangeUserPassword("testPassword1@2", null, null);
            Assert.AreEqual(ChangeUserPasswordResult.Failed, changeUserPasswordResult);

            changeUserPasswordResult = passwordService.ChangeUserPassword("testPassword1@2", "testPassword1@", "testPassword1@");
            Assert.AreEqual(ChangeUserPasswordResult.Success, changeUserPasswordResult);

            changeUserPasswordResult = passwordService.ChangeUserPassword("testPassword1@", "testPassword1@2~`!@#$%^&*()_+{}[]|@\'';:.>,</?\"", "testPassword1@2~`!@#$%^&*()_+{}[]|@\'';:.>,</?\"");
            Assert.AreEqual(ChangeUserPasswordResult.Success, changeUserPasswordResult);

            logoutResult = passwordService.Logout();
            Assert.AreEqual(LogOutResult.Success, logoutResult);

            loginResult = passwordService.Login("testAccount", "testPassword1@2~`!@#$%^&*()_+{}[]|@\'';:.>,</?\"");
            Assert.AreEqual(LoginResult.Successful, loginResult);
        }

    }
}

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
    /// Summary description for DeletePasswordTests
    /// </summary>
    [TestClass]
    public class DeletePasswordTests
    {
        IDatabase db;
        IPasswordService passwordService;
        CreateUserResult createUserResult;
        LoginResult loginResult;
        LogOutResult logoutResult;
        AddPasswordResult addPasswordResult;
        DeletePasswordResult deletePasswordResult;
        User user;

        public DeletePasswordTests()
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

        // Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            db = DatabaseFactory.GetDatabase(Database.InMemory);
            passwordService = new PasswordService(db, new MasterPassword(), new RijndaelManagedEncryption());

            user = new User("testAccount", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.Successful, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            loginResult = passwordService.Login("testAccount", "testPassword1@");
            Assert.AreEqual(LoginResult.Successful, loginResult);
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            ((InMemoryDatabase)db).LocalPasswordDbAccess.Clear();
            ((InMemoryDatabase)db).LocalUserDbAccess.Clear();

            logoutResult = passwordService.Logout();
            Assert.AreEqual(LogOutResult.Success, logoutResult);
        }
        #endregion

        /// <summary>
        /// This test verifies that passwords can be deleted from a users account and that all
        /// combinations of delete password result are tested.
        /// 
        /// Pass Criteria:
        /// - Password can be deleted
        /// - Delete password results verified
        /// </summary>
        [TestMethod]
        public void DeletePasswordTest()
        {
            Password password = new Password("App1", "username", "email@email.com", "descriptions", "https://www.website.com", "passphrase");
            addPasswordResult = passwordService.AddPassword(password);
            Assert.AreEqual(AddPasswordResult.Success, addPasswordResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalPasswordDbAccess.Count);

            deletePasswordResult = passwordService.DeletePassword(null);
            Assert.AreEqual(DeletePasswordResult.Failed, deletePasswordResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalPasswordDbAccess.Count);

            deletePasswordResult = passwordService.DeletePassword(password);
            Assert.AreEqual(DeletePasswordResult.Success, deletePasswordResult);
            Assert.AreEqual(0, ((InMemoryDatabase)db).LocalPasswordDbAccess.Count);

            deletePasswordResult = passwordService.DeletePassword(password);
            Assert.AreEqual(DeletePasswordResult.PasswordDoesNotExist, deletePasswordResult);
            Assert.AreEqual(0, ((InMemoryDatabase)db).LocalPasswordDbAccess.Count);

            addPasswordResult = passwordService.AddPassword(password);
            Assert.AreEqual(AddPasswordResult.Success, addPasswordResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalPasswordDbAccess.Count);

            logoutResult = passwordService.Logout();
            Assert.AreEqual(LogOutResult.Success, logoutResult);        

            deletePasswordResult = passwordService.DeletePassword(password);
            Assert.AreEqual(DeletePasswordResult.Failed, deletePasswordResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalPasswordDbAccess.Count);

            loginResult = passwordService.Login("testAccount", "testPassword1@");
            Assert.AreEqual(LoginResult.Successful, loginResult);
        }

        /// <summary>
        /// This test verifies deleting passwords with multiple accounts, that the
        /// password is only deleted for the logged in user.
        /// 
        /// Pass Criteria:
        /// - Password only deleted for logged in user
        /// </summary>
        [TestMethod]
        public void MultipleUsersDeletePasswordTest()
        {
            logoutResult = passwordService.Logout();
            Assert.AreEqual(LogOutResult.Success, logoutResult);

            user = new User("testAccount2", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.Successful, createUserResult);
            Assert.AreEqual(2, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            loginResult = passwordService.Login("testAccount", "testPassword1@");
            Assert.AreEqual(LoginResult.Successful, loginResult);

            Password password = new Password("App1", "username", "email@email.com", "descriptions", "https://www.website.com", "passphrase");
            Password password2 = new Password("App2", "username", "email@email.com", "descriptions", "https://www.website.com", "passphrase");
            addPasswordResult = passwordService.AddPassword(password);
            addPasswordResult = passwordService.AddPassword(password2);
            Assert.AreEqual(AddPasswordResult.Success, addPasswordResult);
            Assert.AreEqual(2, ((InMemoryDatabase)db).LocalPasswordDbAccess.Count);

            logoutResult = passwordService.Logout();
            Assert.AreEqual(LogOutResult.Success, logoutResult);

            loginResult = passwordService.Login("testAccount2", "testPassword1@");
            Assert.AreEqual(LoginResult.Successful, loginResult);

            addPasswordResult = passwordService.AddPassword(password);
            addPasswordResult = passwordService.AddPassword(password2);
            Assert.AreEqual(AddPasswordResult.Success, addPasswordResult);
            Assert.AreEqual(4, ((InMemoryDatabase)db).LocalPasswordDbAccess.Count);

            deletePasswordResult = passwordService.DeletePassword(password);
            Assert.AreEqual(3, ((InMemoryDatabase)db).LocalPasswordDbAccess.Count);

            List<Password> passwords = passwordService.GetPasswords();
            Assert.AreEqual(1, passwords.Count);
            Assert.AreEqual("App2", passwordService.GetPasswords()[0].Application);
            Assert.AreEqual("username", passwordService.GetPasswords()[0].Username);
            Assert.AreEqual("email@email.com", passwordService.GetPasswords()[0].Email);
            Assert.AreEqual("descriptions", passwordService.GetPasswords()[0].Description);
            Assert.AreEqual("https://www.website.com", passwordService.GetPasswords()[0].Website);
            Assert.AreNotEqual("passphrase", passwordService.GetPasswords()[0].Passphrase); // verify that password is encrypted
            Password decryptPassword = passwordService.DecryptPassword(passwords[0]);
            Assert.AreEqual("App2", decryptPassword.Application);
            Assert.AreEqual("username", decryptPassword.Username);
            Assert.AreEqual("email@email.com", decryptPassword.Email);
            Assert.AreEqual("descriptions", decryptPassword.Description);
            Assert.AreEqual("https://www.website.com", decryptPassword.Website);
            Assert.AreEqual("passphrase", decryptPassword.Passphrase); // verify that password is decrypted

            logoutResult = passwordService.Logout();
            Assert.AreEqual(LogOutResult.Success, logoutResult);

            loginResult = passwordService.Login("testAccount", "testPassword1@");
            Assert.AreEqual(LoginResult.Successful, loginResult);
            passwords = passwordService.GetPasswords();
            Assert.AreEqual(2, passwords.Count);
            Assert.AreEqual("App1", passwordService.GetPasswords()[0].Application);
            Assert.AreEqual("username", passwordService.GetPasswords()[0].Username);
            Assert.AreEqual("email@email.com", passwordService.GetPasswords()[0].Email);
            Assert.AreEqual("descriptions", passwordService.GetPasswords()[0].Description);
            Assert.AreEqual("https://www.website.com", passwordService.GetPasswords()[0].Website);
            Assert.AreNotEqual("passphrase", passwordService.GetPasswords()[0].Passphrase); // verify that password is encrypted
            decryptPassword = passwordService.DecryptPassword(passwords[0]);
            Assert.AreEqual("App1", decryptPassword.Application);
            Assert.AreEqual("username", decryptPassword.Username);
            Assert.AreEqual("email@email.com", decryptPassword.Email);
            Assert.AreEqual("descriptions", decryptPassword.Description);
            Assert.AreEqual("https://www.website.com", decryptPassword.Website);
            Assert.AreEqual("passphrase", decryptPassword.Passphrase); // verify that password is decrypted

            Assert.AreEqual("App2", passwordService.GetPasswords()[1].Application);
            Assert.AreEqual("username", passwordService.GetPasswords()[1].Username);
            Assert.AreEqual("email@email.com", passwordService.GetPasswords()[1].Email);
            Assert.AreEqual("descriptions", passwordService.GetPasswords()[1].Description);
            Assert.AreEqual("https://www.website.com", passwordService.GetPasswords()[1].Website);
            Assert.AreNotEqual("passphrase", passwordService.GetPasswords()[1].Passphrase); // verify that password is encrypted
            decryptPassword = passwordService.DecryptPassword(passwords[1]);
            Assert.AreEqual("App2", decryptPassword.Application);
            Assert.AreEqual("username", decryptPassword.Username);
            Assert.AreEqual("email@email.com", decryptPassword.Email);
            Assert.AreEqual("descriptions", decryptPassword.Description);
            Assert.AreEqual("https://www.website.com", decryptPassword.Website);
            Assert.AreEqual("passphrase", decryptPassword.Passphrase); // verify that password is decrypted
        }
    }
}

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
    /// Summary description for ModifyPasswordTests
    /// </summary>
    [TestClass]
    public class ModifyPasswordTests
    {
        IDatabase db;
        IDesktopServiceWrapper passwordService;
        AddUserResult createUserResult;
        AuthenticateResult loginResult;
        LogOutResult logoutResult;
        AddModifyPasswordResult addPasswordResult;
        User user;

        public ModifyPasswordTests()
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
            passwordService = DesktopPasswordServiceBuilder.BuildDesktopServiceWrapper(db);

            user = new User("testAccount", "testPassword1@aaaaaaaaa", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.Successful, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            loginResult = passwordService.Login("testAccount", "testPassword1@aaaaaaaaa");
            Assert.AreEqual(AuthenticateResult.Successful, loginResult);
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
        /// 
        /// </summary>
        public void ModifyNonExistentPassword()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void MultipleUserModifyTest()
        {
            List<Password> passwords = new List<Password>();
            Password decryptPassword;

            logoutResult = passwordService.Logout();
            Assert.AreEqual(LogOutResult.Success, logoutResult);

            user = new User("testAccount2", "testPassword1@aaaaaaaaa", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.Successful, createUserResult);
            Assert.AreEqual(2, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            loginResult = passwordService.Login("testAccount", "testPassword1@aaaaaaaaa");
            Assert.AreEqual(AuthenticateResult.Successful, loginResult);

            Password password = new Password("App1", "username", "email@email.com", "descriptions", "https://www.website.com", "passphrase");
            Password password2 = new Password("App2", "username", "email@email.com", "descriptions", "https://www.website.com", "passphrase");
            addPasswordResult = passwordService.AddPassword(password);
            addPasswordResult = passwordService.AddPassword(password2);
            Assert.AreEqual(AddModifyPasswordResult.Success, addPasswordResult);
            Assert.AreEqual(2, ((InMemoryDatabase)db).LocalPasswordDbAccess.Count);

            logoutResult = passwordService.Logout();
            Assert.AreEqual(LogOutResult.Success, logoutResult);

            loginResult = passwordService.Login("testAccount2", "testPassword1@aaaaaaaaa");
            Assert.AreEqual(AuthenticateResult.Successful, loginResult);

            addPasswordResult = passwordService.AddPassword(password);
            addPasswordResult = passwordService.AddPassword(password2);
            Assert.AreEqual(AddModifyPasswordResult.Success, addPasswordResult);
            Assert.AreEqual(4, ((InMemoryDatabase)db).LocalPasswordDbAccess.Count);

            Password newPassword = new Password("App1-modified", "username-modified", "email-modified@email.com", "descriptions-modified", "https://www.website-modified.com", "passphrase-modified");
            addPasswordResult = passwordService.ModifyPassword(password, newPassword);
            Assert.AreEqual(AddModifyPasswordResult.Success, addPasswordResult);
            Assert.AreEqual(4, ((InMemoryDatabase)db).LocalPasswordDbAccess.Count);

            passwords = passwordService.GetPasswords();
            Assert.AreEqual("App1-modified", passwordService.GetPasswords()[0].Application);
            Assert.AreEqual("username-modified", passwordService.GetPasswords()[0].Username);
            Assert.AreEqual("email-modified@email.com", passwordService.GetPasswords()[0].Email);
            Assert.AreEqual("descriptions-modified", passwordService.GetPasswords()[0].Description);
            Assert.AreEqual("https://www.website-modified.com", passwordService.GetPasswords()[0].Website);
            Assert.AreEqual("passphrase-modified", passwordService.GetPasswords()[0].Passphrase); // verify that password is encrypted

            Assert.AreEqual("App2", passwordService.GetPasswords()[1].Application);
            Assert.AreEqual("username", passwordService.GetPasswords()[1].Username);
            Assert.AreEqual("email@email.com", passwordService.GetPasswords()[1].Email);
            Assert.AreEqual("descriptions", passwordService.GetPasswords()[1].Description);
            Assert.AreEqual("https://www.website.com", passwordService.GetPasswords()[1].Website);
            Assert.AreEqual("passphrase", passwordService.GetPasswords()[1].Passphrase); // verify that password is encrypted

            logoutResult = passwordService.Logout();
            Assert.AreEqual(LogOutResult.Success, logoutResult);

            loginResult = passwordService.Login("testAccount", "testPassword1@aaaaaaaaa");
            Assert.AreEqual(AuthenticateResult.Successful, loginResult);

            passwords = passwordService.GetPasswords();
            Assert.AreEqual("App1", passwordService.GetPasswords()[0].Application);
            Assert.AreEqual("username", passwordService.GetPasswords()[0].Username);
            Assert.AreEqual("email@email.com", passwordService.GetPasswords()[0].Email);
            Assert.AreEqual("descriptions", passwordService.GetPasswords()[0].Description);
            Assert.AreEqual("https://www.website.com", passwordService.GetPasswords()[0].Website);
            Assert.AreEqual("passphrase", passwordService.GetPasswords()[0].Passphrase); // verify that password is encrypted

            Assert.AreEqual("App2", passwordService.GetPasswords()[1].Application);
            Assert.AreEqual("username", passwordService.GetPasswords()[1].Username);
            Assert.AreEqual("email@email.com", passwordService.GetPasswords()[1].Email);
            Assert.AreEqual("descriptions", passwordService.GetPasswords()[1].Description);
            Assert.AreEqual("https://www.website.com", passwordService.GetPasswords()[1].Website);
            Assert.AreEqual("passphrase", passwordService.GetPasswords()[1].Passphrase); // verify that password is encrypted
        }

        /// <summary>
        /// Test verifies that passwords can be modified correctly. All fields of a password object can be modified.
        /// 
        /// Pass Criteria:
        /// - Modify password and verify that add fields are populated correctly
        /// - Verify that a duplicate password is detected
        /// - Verify that application field does not accept null or empty
        /// - Verify that username field does not accept null or empty
        /// - Verify that email field does not accept null and has valid email if populated
        /// - Verify that description field does not accept null
        /// - Verify that website field does not accept null and is valid website if populated
        /// - Verify that passphrase is not null or empty
        /// - Verify that max length is enforced
        /// </summary>
        [TestMethod]
        public void ModifyPasswordTest()
        {
            List<Password> passwords;
            Password decryptedPass;

            Password password = new Password("App1", "username", "email@email.com", "descriptions", "https://www.website.com", "passphrase");
            Password password2 = new Password("App2", "username", "email@email.com", "descriptions", "https://www.website.com", "passphrase");
            addPasswordResult = passwordService.AddPassword(password);
            addPasswordResult = passwordService.AddPassword(password2);
            Assert.AreEqual(AddModifyPasswordResult.Success, addPasswordResult);
            Assert.AreEqual(2, ((InMemoryDatabase)db).LocalPasswordDbAccess.Count);
            passwords = passwordService.GetPasswords();
            Assert.AreEqual("App1", passwordService.GetPasswords()[0].Application);
            Assert.AreEqual("username", passwordService.GetPasswords()[0].Username);
            Assert.AreEqual("email@email.com", passwordService.GetPasswords()[0].Email);
            Assert.AreEqual("descriptions", passwordService.GetPasswords()[0].Description);
            Assert.AreEqual("https://www.website.com", passwordService.GetPasswords()[0].Website);
            Assert.AreEqual("passphrase", passwordService.GetPasswords()[0].Passphrase); // verify that password is encrypted

            Password newPassword = new Password("App1-modified", "username-modified", "email-modified@email.com", "descriptions-modified", "https://www.website-modified.com", "passphrase-modified");
            addPasswordResult = passwordService.ModifyPassword(password, newPassword);
            Assert.AreEqual(AddModifyPasswordResult.Success, addPasswordResult);
            Assert.AreEqual(2, ((InMemoryDatabase)db).LocalPasswordDbAccess.Count);
            passwords = passwordService.GetPasswords();
            Assert.AreEqual("App1-modified", passwordService.GetPasswords()[0].Application);
            Assert.AreEqual("username-modified", passwordService.GetPasswords()[0].Username);
            Assert.AreEqual("email-modified@email.com", passwordService.GetPasswords()[0].Email);
            Assert.AreEqual("descriptions-modified", passwordService.GetPasswords()[0].Description);
            Assert.AreEqual("https://www.website-modified.com", passwordService.GetPasswords()[0].Website);
            Assert.AreEqual("passphrase-modified", passwordService.GetPasswords()[0].Passphrase); // verify that password is encrypted

            // Test null values
            addPasswordResult = passwordService.ModifyPassword(null, password);
            Assert.AreEqual(AddModifyPasswordResult.Failed, addPasswordResult);

            addPasswordResult = passwordService.ModifyPassword(newPassword, null);
            Assert.AreEqual(AddModifyPasswordResult.Failed, addPasswordResult);

            addPasswordResult = passwordService.ModifyPassword(null, null);
            Assert.AreEqual(AddModifyPasswordResult.Failed, addPasswordResult);

            // Test value that does not exist
            Password password3 = new Password("x", "x", "x@email.com", "x", "https://www.x.com", "x");
            Password password4 = new Password("x2", "x", "x@email.com", "x", "https://www.x.com", "x");
            addPasswordResult = passwordService.ModifyPassword(password3, password4);
            Assert.AreEqual(AddModifyPasswordResult.Failed, addPasswordResult);

            // Test identical passwords
            password = new Password("x", "x", "x@email.com", "x", "https://www.x.com", "x");
            addPasswordResult = passwordService.ModifyPassword(password, password);
            Assert.AreEqual(AddModifyPasswordResult.Failed, addPasswordResult);

            // Test duplicate password
            password = new Password("App2", "username", "email@email.com", "descriptions", "https://www.website.com", "passphrase");
            addPasswordResult = passwordService.ModifyPassword(newPassword, password);
            Assert.AreEqual(AddModifyPasswordResult.DuplicatePassword, addPasswordResult);

            // Test application field
            password = new Password("", "username", "email@email.com", "descriptions", "https://www.website.com", "passphrase");
            addPasswordResult = passwordService.ModifyPassword(newPassword, password);
            Assert.AreEqual(AddModifyPasswordResult.ApplicationError, addPasswordResult);

            password = new Password(null, "username", "email@email.com", "descriptions", "https://www.website.com", "passphrase");
            addPasswordResult = passwordService.ModifyPassword(newPassword, password);
            Assert.AreEqual(AddModifyPasswordResult.ApplicationError, addPasswordResult);

            // Test user name field
            password = new Password("App3", "", "email@email.com", "descriptions", "https://www.website.com", "passphrase");
            addPasswordResult = passwordService.ModifyPassword(newPassword, password);
            Assert.AreEqual(AddModifyPasswordResult.UsernameError, addPasswordResult);

            password = new Password("App3", null, "email@email.com", "descriptions", "https://www.website.com", "passphrase");
            addPasswordResult = passwordService.ModifyPassword(newPassword, password);
            Assert.AreEqual(AddModifyPasswordResult.UsernameError, addPasswordResult);

            // Test email field
            password = new Password("App3", "username", "emailemail.com", "descriptions", "https://www.website.com", "passphrase");
            addPasswordResult = passwordService.ModifyPassword(newPassword, password);
            Assert.AreEqual(AddModifyPasswordResult.EmailError, addPasswordResult);

            password = new Password("App3", "username", "email@emailcom", "descriptions", "https://www.website.com", "passphrase");
            addPasswordResult = passwordService.ModifyPassword(newPassword, password);
            Assert.AreEqual(AddModifyPasswordResult.EmailError, addPasswordResult);

            password = new Password("App3", "username", null, "descriptions", "https://www.website.com", "passphrase");
            addPasswordResult = passwordService.ModifyPassword(newPassword, password);
            Assert.AreEqual(AddModifyPasswordResult.EmailError, addPasswordResult);

            // Test description field
            password = new Password("App3", "username", "email@email.com", null, "https://www.website.com", "passphrase");
            addPasswordResult = passwordService.ModifyPassword(newPassword, password);
            Assert.AreEqual(AddModifyPasswordResult.DescriptionError, addPasswordResult);

            // Test website field
            password = new Password("App3", "username", "email@email.com", "descriptions", null, "passphrase");
            addPasswordResult = passwordService.ModifyPassword(newPassword, password);
            Assert.AreEqual(AddModifyPasswordResult.WebsiteError, addPasswordResult);

            password = new Password("App3", "username", "email@email.com", "descriptions", "w", "passphrase");
            addPasswordResult = passwordService.ModifyPassword(newPassword, password);
            Assert.AreEqual(AddModifyPasswordResult.WebsiteError, addPasswordResult);

            // Test passphrase field
            password = new Password("App3", "username", "email@email.com", "descriptions", "https://www.website.com", "");
            addPasswordResult = passwordService.ModifyPassword(newPassword, password);
            Assert.AreEqual(AddModifyPasswordResult.PassphraseError, addPasswordResult);

            // Test logged out
            logoutResult = passwordService.Logout();
            Assert.AreEqual(LogOutResult.Success, logoutResult);

            addPasswordResult = passwordService.ModifyPassword(newPassword, password);
            Assert.AreEqual(AddModifyPasswordResult.Failed, addPasswordResult);

            loginResult = passwordService.Login("testAccount", "testPassword1@aaaaaaaaa");
            Assert.AreEqual(AuthenticateResult.Successful, loginResult);

        }
    }
}

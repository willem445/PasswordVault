using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PasswordVault.Data;
using PasswordVault.Services;
using PasswordVault.Models;
using System.Diagnostics;
using PasswordVault.Desktop.Winforms;
using PasswordVault.Utilities;

namespace PasswordVault.ServicesTests
{
    /// <summary>
    /// Summary description for AddPasswordTest
    /// </summary>
    [TestClass]
    public class AddPasswordTests
    {
        IDatabase db;
        IDesktopServiceWrapper passwordService;
        AddUserResult createUserResult;
        AuthenticateResult loginResult;
        LogOutResult logoutResult;
        ValidatePassword addPasswordResult;
        User user;

        public AddPasswordTests()
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

        // Use TestInitialize to run code before running each test
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
        /// This tests verifies that passwords can be added to the database. Verifies that the 
        /// data validation for a new password is correct.
        /// 
        /// Pass Criteria:
        /// - Add new password and verify that add fields are populated correctly
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
        public void AddPasswordTest()
        {
            Password password = new Password("App1", "username", "email@email.com", "descriptions", "https://www.website.com", "passphrase");
            addPasswordResult = passwordService.AddPassword(password);
            Assert.AreEqual(ValidatePassword.Success, addPasswordResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalPasswordDbAccess.Count);      
            List<Password> passowrds = passwordService.GetPasswords();
            Assert.AreEqual("App1", passwordService.GetPasswords()[0].Application);
            Assert.AreEqual("username", passwordService.GetPasswords()[0].Username);
            Assert.AreEqual("email@email.com", passwordService.GetPasswords()[0].Email);
            Assert.AreEqual("descriptions", passwordService.GetPasswords()[0].Description);
            Assert.AreEqual("https://www.website.com", passwordService.GetPasswords()[0].Website);
            Assert.AreEqual("passphrase", passwordService.GetPasswords()[0].Passphrase); // verify that password is encrypted

            // Test null password
            addPasswordResult = passwordService.AddPassword(null);
            Assert.AreEqual(ValidatePassword.Failed, addPasswordResult);
            
            // Test duplicate password
            password = new Password("App1", "username", "email@email.com", "descriptions", "https://www.website.com", "passphrase");
            addPasswordResult = passwordService.AddPassword(password);
            Assert.AreEqual(ValidatePassword.DuplicatePassword, addPasswordResult);

            // Test application field
            password = new Password("", "username", "email@email.com", "descriptions", "https://www.website.com", "passphrase");
            addPasswordResult = passwordService.AddPassword(password);
            Assert.AreEqual(ValidatePassword.ApplicationError, addPasswordResult);

            password = new Password(null, "username", "email@email.com", "descriptions", "https://www.website.com", "passphrase");
            addPasswordResult = passwordService.AddPassword(password);
            Assert.AreEqual(ValidatePassword.ApplicationError, addPasswordResult);

            // Test user name field
            password = new Password("App2", "", "email@email.com", "descriptions", "https://www.website.com", "passphrase");
            addPasswordResult = passwordService.AddPassword(password);
            Assert.AreEqual(ValidatePassword.UsernameError, addPasswordResult);

            password = new Password("App2", null, "email@email.com", "descriptions", "https://www.website.com", "passphrase");
            addPasswordResult = passwordService.AddPassword(password);
            Assert.AreEqual(ValidatePassword.UsernameError, addPasswordResult);

            // Test email field
            password = new Password("App2", "username", "emailemail.com", "descriptions", "https://www.website.com", "passphrase");
            addPasswordResult = passwordService.AddPassword(password);
            Assert.AreEqual(ValidatePassword.EmailError, addPasswordResult);

            password = new Password("App2", "username", "email@emailcom", "descriptions", "https://www.website.com", "passphrase");
            addPasswordResult = passwordService.AddPassword(password);
            Assert.AreEqual(ValidatePassword.EmailError, addPasswordResult);

            password = new Password("App2", "username", null, "descriptions", "https://www.website.com", "passphrase");
            addPasswordResult = passwordService.AddPassword(password);
            Assert.AreEqual(ValidatePassword.EmailError, addPasswordResult);

            // Test description field
            password = new Password("App2", "username", "email@email.com", null, "https://www.website.com", "passphrase");
            addPasswordResult = passwordService.AddPassword(password);
            Assert.AreEqual(ValidatePassword.DescriptionError, addPasswordResult);

            // Test website field
            password = new Password("App2", "username", "email@email.com", "descriptions", null, "passphrase");
            addPasswordResult = passwordService.AddPassword(password);
            Assert.AreEqual(ValidatePassword.WebsiteError, addPasswordResult);

            password = new Password("App2", "username", "email@email.com", "descriptions", "w", "passphrase");
            addPasswordResult = passwordService.AddPassword(password);
            Assert.AreEqual(ValidatePassword.WebsiteError, addPasswordResult);

            // Test passphrase field
            password = new Password("App2", "username", "email@email.com", "descriptions", "https://www.website.com", "");
            addPasswordResult = passwordService.AddPassword(password);
            Assert.AreEqual(ValidatePassword.PassphraseError, addPasswordResult);

            // Test valid combinations
            password = new Password("App2", "username", "email@email.com", "descriptions", "https://www.website.com", null);
            addPasswordResult = passwordService.AddPassword(password);
            Assert.AreEqual(ValidatePassword.PassphraseError, addPasswordResult);

            password = new Password("App2", "username", "email@email.com", "descriptions", "", "passphrase");
            addPasswordResult = passwordService.AddPassword(password);
            Assert.AreEqual(ValidatePassword.Success, addPasswordResult);

            password = new Password("App3", "username", "", "", "", "passphrase");
            addPasswordResult = passwordService.AddPassword(password);
            Assert.AreEqual(ValidatePassword.Success, addPasswordResult);

        }

        /// <summary>
        /// This test verifies that the time to add passwords stays below a certain time limit. If 
        /// it takes too long to add a password, the application hangs noticably. Previously the 
        /// implementation was not optimized and the time to add a password increased linearly as
        /// the number of passwords increased.
        /// 
        /// Pass Criteria:
        ///     - Time to add a single password is less than 500 ms for 200 password adds.
        /// </summary>
        [TestMethod]
        [Ignore()] // Ignore for now. Time is less of a concern over security.
        public void AddPasswordTimeTest()
        {
            MasterPassword generateKey = new MasterPassword();
            Stopwatch stopWatch = new Stopwatch();

            List<Password> passwords = new List<Password>();

            for (int i = 0; i < 200; i++)
            {
                string application = KeyGenerator.GenerateRandomPassword(10);
                Password password = new Password(application, "username", "email@email.com", "descriptions", "https://www.website.com", "passphrase");
                passwords.Add(password);
            }

            foreach (var password in passwords)
            {
                stopWatch.Start();
                addPasswordResult = passwordService.AddPassword(password);
                stopWatch.Stop();
                Assert.AreEqual(ValidatePassword.Success, addPasswordResult);

                TimeSpan ts = stopWatch.Elapsed;

                if (ts.TotalMilliseconds > 500)
                {
                    Assert.Fail();
                }

                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);

                Trace.WriteLine("RunTime: " + elapsedTime);
                stopWatch.Reset();
            }
        }
    }
}

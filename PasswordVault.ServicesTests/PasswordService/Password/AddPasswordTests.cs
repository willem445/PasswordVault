using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PasswordVault.Data;
using PasswordVault.Services;
using PasswordVault.Models;
using System.Diagnostics;

namespace PasswordVault.ServicesTests
{
    /// <summary>
    /// Summary description for AddPasswordTest
    /// </summary>
    [TestClass]
    public class AddPasswordTests
    {
        IDatabase db;
        IPasswordService passwordService;
        CreateUserResult createUserResult;
        LoginResult loginResult;
        LogOutResult logoutResult;
        AddPasswordResult addPasswordResult;
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
        /// 
        /// </summary>
        [TestMethod]
        public void AddPasswordTest()
        {
            Password password = new Password("App1", "username", "email@email.com", "descriptions", "website", "passphrase");
            addPasswordResult = passwordService.AddPassword(password);
            Assert.AreEqual(AddPasswordResult.Success, addPasswordResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalPasswordDbAccess.Count);      
            List<Password> passowrds = passwordService.GetPasswords();
            Assert.AreEqual("App1", passwordService.GetPasswords()[0].Application);
            Assert.AreEqual("username", passwordService.GetPasswords()[0].Username);
            Assert.AreEqual("email@email.com", passwordService.GetPasswords()[0].Email);
            Assert.AreEqual("descriptions", passwordService.GetPasswords()[0].Description);
            Assert.AreEqual("website", passwordService.GetPasswords()[0].Website);
            Assert.AreNotEqual("passphrase", passwordService.GetPasswords()[0].Passphrase); // verify that password is encrypted
            Password decryptedPass = passwordService.DecryptPassword(passowrds[0]);
            Assert.AreEqual("App1", decryptedPass.Application);
            Assert.AreEqual("username", decryptedPass.Username);
            Assert.AreEqual("email@email.com", decryptedPass.Email);
            Assert.AreEqual("descriptions", decryptedPass.Description);
            Assert.AreEqual("website", decryptedPass.Website);
            Assert.AreEqual("passphrase", decryptedPass.Passphrase); // verify that password is decrypted

            // Test duplicate password
            password = new Password("App1", "username", "email@email.com", "descriptions", "website", "passphrase");
            addPasswordResult = passwordService.AddPassword(password);
            Assert.AreEqual(AddPasswordResult.DuplicatePassword, addPasswordResult);

            // Test application field
            password = new Password("", "username", "email@email.com", "descriptions", "website", "passphrase");
            addPasswordResult = passwordService.AddPassword(password);
            Assert.AreEqual(AddPasswordResult.ApplicationError, addPasswordResult);

            password = new Password(null, "username", "email@email.com", "descriptions", "website", "passphrase");
            addPasswordResult = passwordService.AddPassword(password);
            Assert.AreEqual(AddPasswordResult.ApplicationError, addPasswordResult);

            // Test email field
            password = new Password("App2", "username", "emailemail.com", "descriptions", "website", "passphrase");
            addPasswordResult = passwordService.AddPassword(password);
            Assert.AreEqual(AddPasswordResult.EmailError, addPasswordResult);

            password = new Password("App2", "username", "email@emailcom", "descriptions", "website", "passphrase");
            addPasswordResult = passwordService.AddPassword(password);
            Assert.AreEqual(AddPasswordResult.EmailError, addPasswordResult);

            password = new Password("App2", "username", null, "descriptions", "website", "passphrase");
            addPasswordResult = passwordService.AddPassword(password);
            Assert.AreEqual(AddPasswordResult.EmailError, addPasswordResult);

            password = new Password("App2", "username", "", "descriptions", "website", "passphrase");
            addPasswordResult = passwordService.AddPassword(password);
            Assert.AreEqual(AddPasswordResult.EmailError, addPasswordResult);

            // Test description field
            password = new Password("App2", "username", "email@email.com", null, "website", "passphrase");
            addPasswordResult = passwordService.AddPassword(password);
            Assert.AreEqual(AddPasswordResult.DescriptionError, addPasswordResult);

            // Test website field
            password = new Password("App2", "username", "email@email.com", "descriptions", null, "passphrase");
            addPasswordResult = passwordService.AddPassword(password);
            Assert.AreEqual(AddPasswordResult.WebsiteError, addPasswordResult);

            // Test passphrase field
            password = new Password("App2", "username", "email@email.com", "descriptions", "website", "");
            addPasswordResult = passwordService.AddPassword(password);
            Assert.AreEqual(AddPasswordResult.PassphraseError, addPasswordResult);

            password = new Password("App2", "username", "email@email.com", "descriptions", "website", null);
            addPasswordResult = passwordService.AddPassword(password);
            Assert.AreEqual(AddPasswordResult.PassphraseError, addPasswordResult);

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
        public void AddPasswordTimeTest()
        {
            MasterPassword generateKey = new MasterPassword();
            Stopwatch stopWatch = new Stopwatch();

            List<Password> passwords = new List<Password>();

            for (int i = 0; i < 200; i++)
            {
                string application = generateKey.GenerateRandomKey();
                Password password = new Password(application, "username", "email@email.com", "descriptions", "website", "passphrase");
                passwords.Add(password);
            }

            foreach (var password in passwords)
            {
                stopWatch.Start();
                addPasswordResult = passwordService.AddPassword(password);
                stopWatch.Stop();
                Assert.AreEqual(AddPasswordResult.Success, addPasswordResult);

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

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
        public AddPasswordTests()
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
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

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
            IDatabase db = DatabaseFactory.GetDatabase(Database.InMemory);
            IPasswordService passwordService = new PasswordService(db, new MasterPassword(), new RijndaelManagedEncryption());
            MasterPassword generateKey = new MasterPassword();
            Stopwatch stopWatch = new Stopwatch();

            CreateUserResult createUserResult;
            LoginResult loginResult;
            LogOutResult logoutResult;
            AddPasswordResult addPasswordResult;
            User user;

            List<Password> passwords = new List<Password>();

            for (int i = 0; i < 200; i++)
            {
                string application = generateKey.GenerateRandomKey();
                Password password = new Password(application, "username", "email@email.com", "descriptions", "website", "passphrase");
                passwords.Add(password);
            }

            user = new User("testAccount", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.Successful, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            loginResult = passwordService.Login("testAccount", "testPassword1@");
            Assert.AreEqual(LoginResult.Successful, loginResult);

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

            logoutResult = passwordService.Logout();
            Assert.AreEqual(LogOutResult.Success, logoutResult);
        }
    }
}

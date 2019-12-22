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
    /// Summary description for PasswordServiceTestBase
    /// </summary>
    [TestClass]
    public class PasswordServiceTestBase
    {
        public IDatabase db;
        public IDesktopServiceWrapper passwordService;
        public AddUserResult createUserResult;
        public AuthenticateResult loginResult;
        public LogOutResult logoutResult;
        public AddModifyPasswordResult addPasswordResult;

        List<User> testUsers;
        List<Password> validTestPasswords;

        public PasswordServiceTestBase()
        {
            testUsers = new List<User>();
            testUsers.Add(new User("testAccount0", "testPassword1@aaaaaaaaa", "testFirstName", "testLastName", "222-111-1111", "test@test.com"));
            testUsers.Add(new User("testAccount1", "testPassword1@aaaaaaaaa", "testFirstName", "testLastName", "222-111-1111", "test@test.com"));
            testUsers.Add(new User("testAccount2", "testPassword1@aaaaaaaaa", "testFirstName", "testLastName", "222-111-1111", "test@test.com"));

            validTestPasswords = new List<Password>();
            validTestPasswords.Add(new Password("App0", "username", "email@email.com", "descriptions", "https://www.website.com", "passphrase"));
            validTestPasswords.Add(new Password("App1", "username", "email@email.com", "descriptions", "https://www.website.com", "passphrase"));
            validTestPasswords.Add(new Password("App2", "username", "email@email.com", "descriptions", "https://www.website.com", "passphrase"));
            validTestPasswords.Add(new Password("App3", "username", "email@email.com", "descriptions", "https://www.website.com", "passphrase"));
            validTestPasswords.Add(new Password("App4", "username", "email@email.com", "descriptions", "https://www.website.com", "passphrase"));
            validTestPasswords.Add(new Password("App5", "username", "email@email.com", "descriptions", "https://www.website.com", "passphrase"));
            validTestPasswords.Add(new Password("App6", "username", "email@email.com", "descriptions", "https://www.website.com", "passphrase"));
            validTestPasswords.Add(new Password("App7", "username", "email@email.com", "descriptions", "https://www.website.com", "passphrase"));
            validTestPasswords.Add(new Password("App8", "username", "email@email.com", "descriptions", "https://www.website.com", "passphrase"));
            validTestPasswords.Add(new Password("App9", "username", "email@email.com", "descriptions", "https://www.website.com", "passphrase"));
            validTestPasswords.Add(new Password("App10", "username", "email@email.com", "descriptions", "https://www.website.com", "passphrase"));
            validTestPasswords.Add(new Password("App11", "username", "email@email.com", "descriptions", "https://www.website.com", "passphrase"));
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

        public List<User> TestUsers
        {
            get
            {
                return TestUsers;
            }
        }

        public List<Password> ValidTestPasswords
        {
            get
            {
                return validTestPasswords;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext) 
        {
           
        }

        // Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup() 
        { 

        }

        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize() 
        {
            db = DatabaseFactory.GetDatabase(Database.InMemory);
            passwordService = DesktopPasswordServiceBuilder.BuildDesktopServiceWrapper(db);

            User testUserNumber = GetTestUser(0);
            CreateAccount(testUserNumber);
            TestLogin(testUserNumber.Username, testUserNumber.PlainTextPassword);
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup() 
        {
            TestLogout();
            ((InMemoryDatabase)db).LocalPasswordDbAccess.Clear();
            ((InMemoryDatabase)db).LocalUserDbAccess.Clear();       
        }

        #endregion

        public void CreateAccount(User user)
        {
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.Successful, createUserResult);
        }

        public void TestLogin(string username, string password)
        {
            loginResult = passwordService.Login(username, password);
            Assert.AreEqual(AuthenticateResult.Successful, loginResult);
        }

        public void TestLogout()
        {
            logoutResult = passwordService.Logout();
            Assert.AreEqual(LogOutResult.Success, logoutResult);
        }

        public User GetTestUser(int userNum)
        {
            if (userNum < testUsers.Count)
            {
                return testUsers[userNum];
            }

            Assert.Fail();
            return new User(false);
        }

        public Password GetTestPassword(int passwordNum)
        {
            if (passwordNum < validTestPasswords.Count)
            {
                return validTestPasswords[passwordNum];
            }

            Assert.Fail();
            return new Password();
        }

    }
}

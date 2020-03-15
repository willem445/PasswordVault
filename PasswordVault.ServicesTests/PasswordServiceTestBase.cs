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
        public AddPasswordResult addPasswordResult;
        public LogOutResult logoutResult;
        public ValidatePassword addModifyPasswordResult;

        private List<User> testUsers;
        private List<Password> validTestPasswords;
        private Dictionary<string, User> invalidTestUsers;
        private Dictionary<string, Password> invalidTestPasswords;
        private int userAccountCount = 0;
        private int passwordCount = 0;
        private TestContext testContextInstance;

        public PasswordServiceTestBase()
        {
            testUsers = new List<User>();
            validTestPasswords = new List<Password>();
        }

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
                return testUsers;
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
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext) 
        {
           
        }

        [ClassCleanup()]
        public static void MyClassCleanup() 
        { 

        }

        [TestInitialize()]
        public void MyTestInitialize() 
        {
            db = DatabaseFactory.GetDatabase(Database.InMemory);
            passwordService = DesktopPasswordServiceBuilder.BuildDesktopServiceWrapper(db);

            for (int i = 0; i < 20; i++)
            {
                testUsers.Add(GetRandomValidUser());
            }

            for (int i = 0; i < 20; i++)
            {
                validTestPasswords.Add(GetRandomValidPassword());
            }

            invalidTestUsers = new Dictionary<string, User>()
            {
                // Usernames
                { "UsernameAlreadyTaken", new User("testusername0", "testPassword1@aaaaaaaaa", "Name", "Last", "222-222-2222", "email@email.com")}, // this one assumes that it has already been added
                { "UsernameEmpty", new User("", "testPassword1@aaaaaaaaa", "Name", "Last", "222-222-2222", "email@email.com")},
                { "UsernameNull", new User(null, "testPassword1@aaaaaaaaa", "Name", "Last", "222-222-2222", "email@email.com")},

                // Emails
                { "InvalidEmailNotIncludeAt", new User("username", "testPassword1@aaaaaaaaa", "Name", "Last", "222-222-2222", "emailemail.com")},
                { "InvalidEmailNotIncludePeriod", new User("username", "testPassword1@aaaaaaaaa", "Name", "Last", "222-222-2222", "email@emailcom")},
                { "EmptyEmail", new User("username", "testPassword1@aaaaaaaaa", "Name", "Last", "222-222-2222", "")},
                { "NullEmail", new User("username", "testPassword1@aaaaaaaaa", "Name", "Last", "222-222-2222", null)},
                { "EmailMatchesGhostText", new User("username", "testPassword1@aaaaaaaaa", "Name", "Last", "222-222-2222", "example@provider.com")},
            };

            invalidTestPasswords = new Dictionary<string, Password>()
            {
                // Usernames
                { "InvalidUsernameNull", new Password("app", null, "", "", "", "testPassword1@aaaaaaaaa") }
            };

            User testUserNumber = GetUser(0);
            CreateAccount(testUserNumber);
            Login(testUserNumber.Username, testUserNumber.PlainTextPassword);

            foreach (var password in ValidTestPasswords)
            {
                AddPassword(password);
            }
        }

        [TestCleanup()]
        public void MyTestCleanup() 
        {
            Logout();
            ((InMemoryDatabase)db).LocalPasswordDbAccess.Clear();
            ((InMemoryDatabase)db).LocalUserDbAccess.Clear();       
        }

        #endregion

        public User GetRandomValidUser()
        {
            string usernamebase = "testusername";

            User user = new User(
                username: usernamebase + userAccountCount++,
                "testPassword1@aaaaaaaaa", 
                "testFirstName", 
                "testLastName", 
                "222-111-1111", 
                "test@test.com");

            return user;
        }

        public Password GetRandomValidPassword()
        {
            string appnamebase = "testappname";

            Password password = new Password(
                appnamebase + passwordCount++,
                "username",
                "email@email.com",
                "description",
                "https://fakesite.com",
                passwordService.GeneratePassword()
                );

            return password;
        }

        public void CreateAccount(User user)
        {
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.Successful, createUserResult);
        }

        public void DeleteUserPasswords()
        {
            List<Password> passwords = new List<Password>(passwordService.GetPasswords().ToArray());

            foreach (var password in ValidTestPasswords)
            {
                passwordService.DeletePassword(password);
            }
        }

        public void Login(string username, string password)
        {
            loginResult = passwordService.Login(username, password);
            Assert.AreEqual(AuthenticateResult.Successful, loginResult);
        }

        public void AddPassword(Password password)
        {
            addModifyPasswordResult = passwordService.AddPassword(password);
            Assert.AreEqual(ValidatePassword.Success, addModifyPasswordResult);
        }

        public void Logout()
        {
            logoutResult = passwordService.Logout();
            Assert.AreEqual(LogOutResult.Success, logoutResult);
        }

        public User GetUser(int userNum)
        {
            if (userNum < testUsers.Count)
            {
                return testUsers[userNum];
            }

            Assert.Fail();
            return new User(false);
        }

        public Password GetPassword(int passwordNum)
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

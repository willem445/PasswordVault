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
    /// Summary description for CreateUserTests
    /// </summary>
    [TestClass]
    public class CreateUserTests
    {
        IDatabase db;
        IDesktopServiceWrapper passwordService;

        public CreateUserTests()
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
        public void CreateUserTest()
        {
            AddUserResult createUserResult;

            User user;

            user = new User("testAccount", "testPassword1@aaaaaaaaa", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.Successful, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", "testPassword1@aaaaaaaaa", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.Successful, createUserResult);
            Assert.AreEqual(2, ((InMemoryDatabase)db).LocalUserDbAccess.Count);
        }

        [TestMethod]
        public void CreateUserValidationTest()
        {
            AddUserResult createUserResult;
            User user;

            // Test success
            user = new User("testAccount", "testPassword1@aaaaaaaaa", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.Successful, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            // Test null user
            createUserResult = passwordService.CreateNewUser(null);
            Assert.AreEqual(AddUserResult.Failed, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            // Test user names
            user = new User("testAccount", "testPassword1@aaaaaaaaa", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.UsernameTaken, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("", "testPassword1@aaaaaaaaa", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.UsernameNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User(null, "testPassword1@aaaaaaaaa", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.UsernameNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            // Test passwords
            user = new User("testAccount1", "testPassword1", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.NoSpecialCharacter, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", "aaaaaaaaaaaaaa!1", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.NoUpperCaseCharacter, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", "AAAAAAAAAAAAA!a", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.NoNumber, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", "AAAAAAAAAAAAA!1", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.NoLowerCaseCharacter, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", "1A@a", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.LengthRequirementNotMet, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", "", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.PasswordNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", null, "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.PasswordNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            // Test first names
            user = new User("testAccount1", "testPassword1@aaaaaaaaa", "", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.FirstNameNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", "testPassword1@aaaaaaaaa", null, "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.FirstNameNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            // Test last names
            user = new User("testAccount1", "testPassword1@aaaaaaaaa", "testFirstName", "", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.LastNameNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", "testPassword1@aaaaaaaaa", "testFirstName", null, "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.LastNameNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            // Test email
            user = new User("testAccount1", "testPassword1@aaaaaaaaa", "testFirstName", "testLastName", "222-111-1111", "test@testcom");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.EmailNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", "testPassword1@aaaaaaaaa", "testFirstName", "testLastName", "222-111-1111", "testtest.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.EmailNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", "testPassword1@aaaaaaaaa", "testFirstName", "testLastName", "222-111-1111", "testtestcom");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.EmailNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", "testPassword1@aaaaaaaaa", "testFirstName", "testLastName", "222-111-1111", "example@provider.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.EmailNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", "testPassword1@aaaaaaaaa", "testFirstName", "testLastName", "222-111-1111", "");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.EmailNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", "testPassword1@aaaaaaaaa", "testFirstName", "testLastName", "222-111-1111", null);
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.EmailNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            // Test phone number
            user = new User("testAccount1", "testPassword1@aaaaaaaaa", "testFirstName", "testLastName", "5555555555555555555", "test@testcom");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.PhoneNumberNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", "testPassword1@aaaaaaaaa", "testFirstName", "testLastName", "sgsdfgsdfgfdsg", "test@testcom");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.PhoneNumberNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", "testPassword1@aaaaaaaaa", "testFirstName", "testLastName", "#$%^#$^#$^%$#^@@#$", "test@testcom");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.PhoneNumberNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", "testPassword1@aaaaaaaaa", "testFirstName", "testLastName", "g-222-111-1111", "test@testcom");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.PhoneNumberNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", "testPassword1@aaaaaaaaa", "testFirstName", "testLastName", "xxx-xxx-xxxx", "test@testcom");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.PhoneNumberNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", "testPassword1@aaaaaaaaa", "testFirstName", "testLastName", "", "test@testcom");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.PhoneNumberNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", "testPassword1@aaaaaaaaa", "testFirstName", "testLastName", null, "test@testcom");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.PhoneNumberNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            // Test null user
            createUserResult = passwordService.CreateNewUser(null);
            Assert.AreEqual(AddUserResult.Failed, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

        }

    }
}

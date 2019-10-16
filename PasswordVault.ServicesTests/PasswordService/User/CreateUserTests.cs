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
    /// Summary description for CreateUserTests
    /// </summary>
    [TestClass]
    public class CreateUserTests
    {
        IDatabase db;
        IPasswordService passwordService;

        public CreateUserTests()
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
        public void CreateUserTest()
        {
            CreateUserResult createUserResult;

            User user;

            user = new User("testAccount", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.Successful, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.Successful, createUserResult);
            Assert.AreEqual(2, ((InMemoryDatabase)db).LocalUserDbAccess.Count);
        }

        [TestMethod]
        public void CreateUserValidationTest()
        {
            CreateUserResult createUserResult;
            User user;

            // Test success
            user = new User("testAccount", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.Successful, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            // Test null user
            createUserResult = passwordService.CreateNewUser(null);
            Assert.AreEqual(CreateUserResult.Failed, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            // Test user names
            user = new User("testAccount", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.UsernameTaken, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.UsernameNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User(null, "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.UsernameNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            // Test passwords
            user = new User("testAccount1", "testPassword1", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.NoSpecialCharacter, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", "aaaaaaaaaaaaaa!1", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.NoUpperCaseCharacter, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", "AAAAAAAAAAAAA!a", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.NoNumber, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", "AAAAAAAAAAAAA!1", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.NoLowerCaseCharacter, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", "1A@a", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.LengthRequirementNotMet, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", "", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.PasswordNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", null, "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.PasswordNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            // Test first names
            user = new User("testAccount1", "testPassword1@", "", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.FirstNameNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", "testPassword1@", null, "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.FirstNameNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            // Test last names
            user = new User("testAccount1", "testPassword1@", "testFirstName", "", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.LastNameNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", "testPassword1@", "testFirstName", null, "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.LastNameNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            // Test email
            user = new User("testAccount1", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "test@testcom");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.EmailNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "testtest.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.EmailNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "testtestcom");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.EmailNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            // Test phone number
            user = new User("testAccount1", "testPassword1@", "testFirstName", "testLastName", "5555555555555555555", "test@testcom");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.PhoneNumberNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", "testPassword1@", "testFirstName", "testLastName", "sgsdfgsdfgfdsg", "test@testcom");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.PhoneNumberNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", "testPassword1@", "testFirstName", "testLastName", "#$%^#$^#$^%$#^@@#$", "test@testcom");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.PhoneNumberNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            user = new User("testAccount1", "testPassword1@", "testFirstName", "testLastName", "g-222-111-1111", "test@testcom");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.PhoneNumberNotValid, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);
        }

    }
}

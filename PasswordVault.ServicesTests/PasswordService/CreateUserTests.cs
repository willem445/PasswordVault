using System;
using System.Text;
using System.Collections.Generic;
using Xunit;
using PasswordVault.Data;
using PasswordVault.Services;
using PasswordVault.Models;

namespace PasswordVault.ServicesTests
{
    /// <summary>
    /// Summary description for CreateUserTests
    /// </summary>
    public class CreateUserTests
    {
        public CreateUserTests()
        {
            //
            // TODO: Add constructor logic here
            //
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

        [Fact]
        public void CreateUserTest()
        {
            IDatabase db = DatabaseFactory.GetDatabase(Database.InMemory);
            IPasswordService passwordService = new PasswordService(db, new MasterPassword(), new RijndaelManagedEncryption());

            CreateUserResult createUserResult;

            User user;

            user = new User("testAccount", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.Equal(CreateUserResult.Successful, createUserResult);
            Assert.Single(((InMemoryDatabase)db).LocalUserDbAccess);

            user = new User("testAccount1", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.Equal(CreateUserResult.Successful, createUserResult);
            Assert.Equal(2, ((InMemoryDatabase)db).LocalUserDbAccess.Count);
        }

        [Fact]
        public void CreateUserValidationTest()
        {
            IDatabase db = DatabaseFactory.GetDatabase(Database.InMemory);
            IPasswordService passwordService = new PasswordService(db, new MasterPassword(), new RijndaelManagedEncryption());

            CreateUserResult createUserResult;
            User user;

            // Test success
            user = new User("testAccount", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.Equal(CreateUserResult.Successful, createUserResult);
            Assert.Single(((InMemoryDatabase)db).LocalUserDbAccess);
            
            // Test user names
            user = new User("testAccount", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.Equal(CreateUserResult.UsernameTaken, createUserResult);
            Assert.Single(((InMemoryDatabase)db).LocalUserDbAccess);

            user = new User("", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.Equal(CreateUserResult.UsernameNotValid, createUserResult);
            Assert.Single(((InMemoryDatabase)db).LocalUserDbAccess);

            user = new User(null, "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.Equal(CreateUserResult.UsernameNotValid, createUserResult);
            Assert.Single(((InMemoryDatabase)db).LocalUserDbAccess);

            // Test passwords
            user = new User("testAccount1", "testPassword1", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.Equal(CreateUserResult.NoSpecialCharacter, createUserResult);
            Assert.Single(((InMemoryDatabase)db).LocalUserDbAccess);

            user = new User("testAccount1", "aaaaaaaaaaaaaa!1", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.Equal(CreateUserResult.NoUpperCaseCharacter, createUserResult);
            Assert.Single(((InMemoryDatabase)db).LocalUserDbAccess);

            user = new User("testAccount1", "AAAAAAAAAAAAA!a", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.Equal(CreateUserResult.NoNumber, createUserResult);
            Assert.Single(((InMemoryDatabase)db).LocalUserDbAccess);

            user = new User("testAccount1", "AAAAAAAAAAAAA!1", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.Equal(CreateUserResult.NoLowerCaseCharacter, createUserResult);
            Assert.Single(((InMemoryDatabase)db).LocalUserDbAccess);

            user = new User("testAccount1", "1A@a", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.Equal(CreateUserResult.LengthRequirementNotMet, createUserResult);
            Assert.Single(((InMemoryDatabase)db).LocalUserDbAccess);

            user = new User("testAccount1", "", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.Equal(CreateUserResult.PasswordNotValid, createUserResult);
            Assert.Single(((InMemoryDatabase)db).LocalUserDbAccess);

            user = new User("testAccount1", null, "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.Equal(CreateUserResult.PasswordNotValid, createUserResult);
            Assert.Single(((InMemoryDatabase)db).LocalUserDbAccess);

            // Test first names
            user = new User("testAccount1", "testPassword1@", "", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.Equal(CreateUserResult.FirstNameNotValid, createUserResult);
            Assert.Single(((InMemoryDatabase)db).LocalUserDbAccess);

            user = new User("testAccount1", "testPassword1@", null, "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.Equal(CreateUserResult.FirstNameNotValid, createUserResult);
            Assert.Single(((InMemoryDatabase)db).LocalUserDbAccess);

            // Test last names
            user = new User("testAccount1", "testPassword1@", "testFirstName", "", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.Equal(CreateUserResult.LastNameNotValid, createUserResult);
            Assert.Single(((InMemoryDatabase)db).LocalUserDbAccess);

            user = new User("testAccount1", "testPassword1@", "testFirstName", null, "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.Equal(CreateUserResult.LastNameNotValid, createUserResult);
            Assert.Single(((InMemoryDatabase)db).LocalUserDbAccess);

            // Test email
            user = new User("testAccount1", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "test@testcom");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.Equal(CreateUserResult.EmailNotValid, createUserResult);
            Assert.Single(((InMemoryDatabase)db).LocalUserDbAccess);

            user = new User("testAccount1", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "testtest.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.Equal(CreateUserResult.EmailNotValid, createUserResult);
            Assert.Single(((InMemoryDatabase)db).LocalUserDbAccess);

            user = new User("testAccount1", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "testtestcom");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.Equal(CreateUserResult.EmailNotValid, createUserResult);
            Assert.Single(((InMemoryDatabase)db).LocalUserDbAccess);

            // Test phone number
            user = new User("testAccount1", "testPassword1@", "testFirstName", "testLastName", "5555555555555555555", "test@testcom");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.Equal(CreateUserResult.PhoneNumberNotValid, createUserResult);
            Assert.Single(((InMemoryDatabase)db).LocalUserDbAccess);

            user = new User("testAccount1", "testPassword1@", "testFirstName", "testLastName", "sgsdfgsdfgfdsg", "test@testcom");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.Equal(CreateUserResult.PhoneNumberNotValid, createUserResult);
            Assert.Single(((InMemoryDatabase)db).LocalUserDbAccess);

            user = new User("testAccount1", "testPassword1@", "testFirstName", "testLastName", "#$%^#$^#$^%$#^@@#$", "test@testcom");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.Equal(CreateUserResult.PhoneNumberNotValid, createUserResult);
            Assert.Single(((InMemoryDatabase)db).LocalUserDbAccess);

            user = new User("testAccount1", "testPassword1@", "testFirstName", "testLastName", "g-222-111-1111", "test@testcom");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.Equal(CreateUserResult.PhoneNumberNotValid, createUserResult);
            Assert.Single(((InMemoryDatabase)db).LocalUserDbAccess);
        }

    }
}

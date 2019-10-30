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
    /// Summary description for ModifyUserTests
    /// </summary>
    public class ModifyUserTests
    {
        public ModifyUserTests()
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
            LoginResult loginResult;
            UserInformationResult modifyUserResult;
            User user;
            User userResult;

            user = new User("testAccount", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.Equal(CreateUserResult.Successful, createUserResult);
            Assert.Single(((InMemoryDatabase)db).LocalUserDbAccess);

            modifyUserResult = passwordService.EditUser(new User(null, null, "testFirstName2", "testLastName2", "222-111-2222", "test2@test.com"));
            Assert.Equal(UserInformationResult.Failed, modifyUserResult);

            loginResult = passwordService.Login("testAccount", "testPassword1@");
            Assert.Equal(LoginResult.Successful, loginResult);
            userResult = passwordService.GetCurrentUser();
            Assert.Equal("testFirstName", userResult.FirstName);
            Assert.Equal("testLastName", userResult.LastName);
            Assert.Equal("222-111-1111", userResult.PhoneNumber);
            Assert.Equal("test@test.com", userResult.Email);

            modifyUserResult = passwordService.EditUser(new User(null, null, "testFirstName2", "testLastName2", "222-111-2222", "test2@test.com"));
            Assert.Equal(UserInformationResult.Success, modifyUserResult);

            userResult = passwordService.GetCurrentUser();
            Assert.Equal("testFirstName2", userResult.FirstName);
            Assert.Equal("testLastName2", userResult.LastName);
            Assert.Equal("222-111-2222", userResult.PhoneNumber);
            Assert.Equal("test2@test.com", userResult.Email);

            modifyUserResult = passwordService.EditUser(new User(null, null, "", "testLastName2", "222-111-2222", "test2@test.com"));
            Assert.Equal(UserInformationResult.InvalidFirstName, modifyUserResult);
            userResult = passwordService.GetCurrentUser();
            Assert.Equal("testFirstName2", userResult.FirstName);
            Assert.Equal("testLastName2", userResult.LastName);
            Assert.Equal("222-111-2222", userResult.PhoneNumber);
            Assert.Equal("test2@test.com", userResult.Email);

            modifyUserResult = passwordService.EditUser(new User(null, null, null, "testLastName2", "222-111-2222", "test2@test.com"));
            Assert.Equal(UserInformationResult.InvalidFirstName, modifyUserResult);
            userResult = passwordService.GetCurrentUser();
            Assert.Equal("testFirstName2", userResult.FirstName);
            Assert.Equal("testLastName2", userResult.LastName);
            Assert.Equal("222-111-2222", userResult.PhoneNumber);
            Assert.Equal("test2@test.com", userResult.Email);

            modifyUserResult = passwordService.EditUser(new User(null, null, "testFirstName2", "", "222-111-2222", "test2@test.com"));
            Assert.Equal(UserInformationResult.InvalidLastName, modifyUserResult);
            userResult = passwordService.GetCurrentUser();
            Assert.Equal("testFirstName2", userResult.FirstName);
            Assert.Equal("testLastName2", userResult.LastName);
            Assert.Equal("222-111-2222", userResult.PhoneNumber);
            Assert.Equal("test2@test.com", userResult.Email);

            modifyUserResult = passwordService.EditUser(new User(null, null, "testFirstName2", null, "222-111-2222", "test2@test.com"));
            Assert.Equal(UserInformationResult.InvalidLastName, modifyUserResult);
            userResult = passwordService.GetCurrentUser();
            Assert.Equal("testFirstName2", userResult.FirstName);
            Assert.Equal("testLastName2", userResult.LastName);
            Assert.Equal("222-111-2222", userResult.PhoneNumber);
            Assert.Equal("test2@test.com", userResult.Email);

            modifyUserResult = passwordService.EditUser(new User(null, null, "testFirstName2", "testLastName2", "222-111-222", "test2@test.com"));
            Assert.Equal(UserInformationResult.InvalidPhoneNumber, modifyUserResult);
            userResult = passwordService.GetCurrentUser();
            Assert.Equal("testFirstName2", userResult.FirstName);
            Assert.Equal("testLastName2", userResult.LastName);
            Assert.Equal("222-111-2222", userResult.PhoneNumber);
            Assert.Equal("test2@test.com", userResult.Email);

            modifyUserResult = passwordService.EditUser(new User(null, null, "testFirstName2", "testLastName2", "222-111-2222", "test2test.com"));
            Assert.Equal(UserInformationResult.InvalidEmail, modifyUserResult);
            userResult = passwordService.GetCurrentUser();
            Assert.Equal("testFirstName2", userResult.FirstName);
            Assert.Equal("testLastName2", userResult.LastName);
            Assert.Equal("222-111-2222", userResult.PhoneNumber);
            Assert.Equal("test2@test.com", userResult.Email);

        }
    }
}

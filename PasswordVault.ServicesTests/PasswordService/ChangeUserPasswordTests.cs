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
    /// Summary description for ChangePasswordTests
    /// </summary>
    public class ChangePasswordTests
    {
        public ChangePasswordTests()
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
        public void ChangeUserPasswordTest()
        {
            IDatabase db = DatabaseFactory.GetDatabase(Database.InMemory);
            IPasswordService passwordService = new PasswordService(db, new MasterPassword(), new RijndaelManagedEncryption());

            CreateUserResult createUserResult;
            ChangeUserPasswordResult changeUserPasswordResult;
            LoginResult loginResult;
            LogOutResult logoutResult;
            User user;

            user = new User("testAccount", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.Equal(CreateUserResult.Successful, createUserResult);
            Assert.Single(((InMemoryDatabase)db).LocalUserDbAccess);

            // Not logged in
            changeUserPasswordResult = passwordService.ChangeUserPassword("testPassword1@", "testPassword1@2", "testPassword1@2");
            Assert.Equal(ChangeUserPasswordResult.Failed, changeUserPasswordResult);

            loginResult = passwordService.Login("testAccount", "testPassword1@");
            Assert.Equal(LoginResult.Successful, loginResult);

            changeUserPasswordResult = passwordService.ChangeUserPassword("testPassword1@", "testPassword1@2", "testPassword1@2");
            Assert.Equal(ChangeUserPasswordResult.Success, changeUserPasswordResult);

            logoutResult = passwordService.Logout();
            Assert.Equal(LogOutResult.Success, logoutResult);

            loginResult = passwordService.Login("testAccount", "testPassword1@");
            Assert.Equal(LoginResult.PasswordIncorrect, loginResult);

            loginResult = passwordService.Login("testAccount", "testPassword1@2");
            Assert.Equal(LoginResult.Successful, loginResult);

            changeUserPasswordResult = passwordService.ChangeUserPassword("testPassword1@2", "1@3aA", "1@3aA");
            Assert.Equal(ChangeUserPasswordResult.LengthRequirementNotMet, changeUserPasswordResult);

            changeUserPasswordResult = passwordService.ChangeUserPassword("testPassword1@2", "1@3AAAAAAAAAAAAA", "1@3AAAAAAAAAAAAA");
            Assert.Equal(ChangeUserPasswordResult.NoLowerCaseCharacter, changeUserPasswordResult);

            changeUserPasswordResult = passwordService.ChangeUserPassword("testPassword1@2", "a@GAAAAAAAAAAAAA", "a@GAAAAAAAAAAAAA");
            Assert.Equal(ChangeUserPasswordResult.NoNumber, changeUserPasswordResult);

            changeUserPasswordResult = passwordService.ChangeUserPassword("testPassword1@2", "1A3aaaaaaaaaaaaa", "1A3aaaaaaaaaaaaa");
            Assert.Equal(ChangeUserPasswordResult.NoSpecialCharacter, changeUserPasswordResult);

            changeUserPasswordResult = passwordService.ChangeUserPassword("testPassword1@2", "1@3aaaaaaaaaaaaa", "1@3aaaaaaaaaaaaa");
            Assert.Equal(ChangeUserPasswordResult.NoUpperCaseCharacter, changeUserPasswordResult);

            changeUserPasswordResult = passwordService.ChangeUserPassword("testPassword1@2", "1@3aaaaaaaaaaaaa", "1@3aaaaaaaaaaaaaZ");
            Assert.Equal(ChangeUserPasswordResult.PasswordsDoNotMatch, changeUserPasswordResult);

            changeUserPasswordResult = passwordService.ChangeUserPassword("testPassword1@2", "", "");
            Assert.Equal(ChangeUserPasswordResult.Failed, changeUserPasswordResult);

            changeUserPasswordResult = passwordService.ChangeUserPassword("testPassword1@2", null, null);
            Assert.Equal(ChangeUserPasswordResult.Failed, changeUserPasswordResult);

            changeUserPasswordResult = passwordService.ChangeUserPassword("testPassword1@2", "testPassword1@", "testPassword1@");
            Assert.Equal(ChangeUserPasswordResult.Success, changeUserPasswordResult);

            changeUserPasswordResult = passwordService.ChangeUserPassword("testPassword1@", "testPassword1@2~`!@#$%^&*()_+{}[]|@\'';:.>,</?\"", "testPassword1@2~`!@#$%^&*()_+{}[]|@\'';:.>,</?\"");
            Assert.Equal(ChangeUserPasswordResult.Success, changeUserPasswordResult);

            logoutResult = passwordService.Logout();
            Assert.Equal(LogOutResult.Success, logoutResult);

            loginResult = passwordService.Login("testAccount", "testPassword1@2~`!@#$%^&*()_+{}[]|@\'';:.>,</?\"");
            Assert.Equal(LoginResult.Successful, loginResult);
        }
    }
}

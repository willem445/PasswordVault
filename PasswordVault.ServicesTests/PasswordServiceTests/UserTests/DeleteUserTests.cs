using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using PasswordVault.Data;
using PasswordVault.Services;
using PasswordVault.Models;
using PasswordVault.Desktop.Winforms;

namespace PasswordVault.ServicesTests
{
    [TestClass()]
    public class DeleteUserTests
    {
        IDatabase db;
        IDesktopServiceWrapper passwordService;

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

        /// <summary>
        /// 
        /// </summary>
        [TestMethod()]
        public void DeleteSingleUserTest()
        {
            AddUserResult createResult;
            DeleteUserResult deleteResult;
            User user;

            // Mock a user model that would be coming from the UI
            user = new User("testAccount", "testPassword1@aaaaaaaaa", "testFirstName", "testLastName", "222-111-1111", "test@test.com");

            deleteResult = passwordService.DeleteUser(user, 0);
            Assert.AreEqual(DeleteUserResult.Failed, deleteResult);

            createResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.Successful, createResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            deleteResult = passwordService.DeleteUser(null, 0);
            Assert.AreEqual(DeleteUserResult.Failed, deleteResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            deleteResult = passwordService.DeleteUser(user, 0);
            Assert.AreEqual(DeleteUserResult.Success, deleteResult);
            Assert.AreEqual(0, ((InMemoryDatabase)db).LocalUserDbAccess.Count);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod()]
        public void DeleteMultipleUsersTest()
        {
            AddUserResult createResult;
            DeleteUserResult deleteResult;

            // Mock a user model that would be coming from the UI
            User[] users = 
            {
                new User("testAccount0", "testPassword1@aaaaaaaaa", "testFirstName", "testLastName", "222-111-1111", "test@test.com"),
                new User("testAccount1", "testPassword1@aaaaaaaaa", "testFirstName", "testLastName", "222-111-1111", "test@test.com"),
                new User("testAccount2", "testPassword1@aaaaaaaaa", "testFirstName", "testLastName", "222-111-1111", "test@test.com"),
                new User("testAccount3", "testPassword1@aaaaaaaaa", "testFirstName", "testLastName", "222-111-1111", "test@test.com"),
                new User("testAccount4", "testPassword1@aaaaaaaaa", "testFirstName", "testLastName", "222-111-1111", "test@test.com"),
            };

            Int32 addedUsersCount = 1;
            foreach (var user in users)
            {
                createResult = passwordService.CreateNewUser(user);
                Assert.AreEqual(AddUserResult.Successful, createResult);
                Assert.AreEqual(addedUsersCount, ((InMemoryDatabase)db).LocalUserDbAccess.Count);
                addedUsersCount++;
            }

            Int32 deletedUsersCount = users.Count() - 1;
            foreach (var user in users)
            {
                deleteResult = passwordService.DeleteUser(user, 0);
                Assert.AreEqual(DeleteUserResult.Success, deleteResult);
                Assert.AreEqual(deletedUsersCount, ((InMemoryDatabase)db).LocalUserDbAccess.Count);
                deletedUsersCount--;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod()]
        public void DeleteUserWithPasswords()
        {
            User user;
            AddUserResult createResult;
            AuthenticateResult loginResult;
            DeleteUserResult deleteResult;

            // Mock a user model that would be coming from the UI
            user = new User("testAccount", "testPassword1@aaaaaaaaa", "testFirstName", "testLastName", "222-111-1111", "test@test.com");

            createResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(AddUserResult.Successful, createResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            loginResult = passwordService.Login("testAccount", "testPassword1@aaaaaaaaa");
            Assert.AreEqual(AuthenticateResult.Successful, loginResult);

            // Mock a password model that would be coming from the UI
            Password[] passwords =
            {
                new Password("FakeStore0", "NewUsername", "email@FakeStore.com", "This is my FakeStore account.", "https://www.website.com", "123456"),
                new Password("FakeStore1", "NewUsername", "email@FakeStore.com", "This is my FakeStore account.", "https://www.website.com", "123456"),
                new Password("FakeStore2", "NewUsername", "email@FakeStore.com", "This is my FakeStore account.", "https://www.website.com", "123456"),
                new Password("FakeStore3", "NewUsername", "email@FakeStore.com", "This is my FakeStore account.", "https://www.website.com", "123456"),
            };

            Int32 addedPasswordsCount = 1;
            foreach (var password in passwords)
            {
                AddModifyPasswordResult result = passwordService.AddPassword(password);
                Assert.AreEqual(AddModifyPasswordResult.Success, result);
                Assert.AreEqual(addedPasswordsCount, ((InMemoryDatabase)db).LocalPasswordDbAccess.Count);
                addedPasswordsCount++;
            }

            deleteResult = passwordService.DeleteUser(user, 0);
            Assert.AreEqual(DeleteUserResult.Success, deleteResult);
            Assert.AreEqual(0, ((InMemoryDatabase)db).LocalPasswordDbAccess.Count);
            Assert.AreEqual(0, ((InMemoryDatabase)db).LocalUserDbAccess.Count);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod()]
        
        public void DeleteUserWithMultiplePasswords()
        {
            AddUserResult createResult;
            AuthenticateResult loginResult;
            DeleteUserResult deleteResult;
            LogOutResult logOutResult;

            // Mock a user model that would be coming from the UI
            User[] users =
            {
                new User("testAccount0", "testPassword1@aaaaaaaaa1", "testFirstName", "testLastName", "222-111-1111", "test@test.com"),
                new User("testAccount1", "testPassword1@aaaaaaaaa2", "testFirstName", "testLastName", "222-111-1111", "test@test.com"),
                new User("testAccount2", "testPassword1@aaaaaaaaa3", "testFirstName", "testLastName", "222-111-1111", "test@test.com"),
                new User("testAccount3", "testPassword1@aaaaaaaaa4", "testFirstName", "testLastName", "222-111-1111", "test@test.com"),
                new User("testAccount4", "testPassword1@aaaaaaaaa5", "testFirstName", "testLastName", "222-111-1111", "test@test.com"),
            };

            // Mock a password model that would be coming from the UI
            Password[] passwords =
            {
                new Password("FakeStore0", "NewUsername", "email@FakeStore.com", "This is my FakeStore account.", "https://www.website.com", "123456"),
                new Password("FakeStore1", "NewUsername", "email@FakeStore.com", "This is my FakeStore account.", "https://www.website.com", "123456"),
                new Password("FakeStore2", "NewUsername", "email@FakeStore.com", "This is my FakeStore account.", "https://www.website.com", "123456"),
                new Password("FakeStore3", "NewUsername", "email@FakeStore.com", "This is my FakeStore account.", "https://www.website.com", "123456"),
            };

            // Add each user to the database, log in, add each password to each user, logout
            Int32 addedUsersCount = 1;
            Int32 addedPasswordsCount = 1;
            foreach (var user in users)
            {
                createResult = passwordService.CreateNewUser(user);
                Assert.AreEqual(AddUserResult.Successful, createResult);
                Assert.AreEqual(addedUsersCount, ((InMemoryDatabase)db).LocalUserDbAccess.Count);
                addedUsersCount++;

                loginResult = passwordService.Login(user.Username, user.PlainTextPassword);
                Assert.AreEqual(AuthenticateResult.Successful, loginResult);
           
                foreach (var password in passwords)
                {
                    AddModifyPasswordResult result = passwordService.AddPassword(password);
                    Assert.AreEqual(AddModifyPasswordResult.Success, result);
                    Assert.AreEqual(addedPasswordsCount, ((InMemoryDatabase)db).LocalPasswordDbAccess.Count);
                    addedPasswordsCount++;
                }

                logOutResult = passwordService.Logout();
                Assert.AreEqual(LogOutResult.Success, logOutResult);
            }

            // Login to the account to delete
            loginResult = passwordService.Login("testAccount0", "testPassword1@aaaaaaaaa1");
            Assert.AreEqual(AuthenticateResult.Successful, loginResult);

            // Delete the account
            string currentUserGUID = passwordService.GetCurrentUser().Uuid;
            deleteResult = passwordService.DeleteUser(passwordService.GetCurrentUser(), 0);
            Assert.AreEqual(DeleteUserResult.Success, deleteResult);
            Assert.AreEqual(((passwords.Length * users.Length) - passwords.Length), ((InMemoryDatabase)db).LocalPasswordDbAccess.Count);
            Assert.AreEqual((users.Length - 1), ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            // Verify that the deleted account's passwords are deleted from the password database table
            foreach (var password in ((InMemoryDatabase)db).LocalPasswordDbAccess)
            {
                if (password.UserUuid == currentUserGUID)
                {
                    Assert.Fail();
                }
            }
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using PasswordVault.Data;
using PasswordVault.Service;
using PasswordVault.Models;
using PasswordVault.Desktop.Winforms;

namespace PasswordVaultTests
{
    [TestClass()]
    public class DeleteUserTests
    {
        /// <summary>
        /// 
        /// </summary>
        [TestMethod()]
        public void CreateUserTest()
        {
            IDatabase db = DatabaseFactory.GetDatabase(Database.InMemory);
            IPasswordService passwordService = new PasswordService(db, new MasterPassword(), new RijndaelManagedEncryption());

            CreateUserResult result;
            User user;

            user = new User("testAccount", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            result = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.Successful, result);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            result = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.UsernameTaken, result);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod()]
        public void DeleteSingleUserTest()
        {
            IDatabase db = DatabaseFactory.GetDatabase(Database.InMemory);
            IPasswordService passwordService = new PasswordService(db, new MasterPassword(), new RijndaelManagedEncryption());

            CreateUserResult createResult;
            DeleteUserResult deleteResult;
            User user;

            // Mock a user model that would be coming from the UI
            user = new User("testAccount", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "test@test.com");

            deleteResult = passwordService.DeleteUser(user);
            Assert.AreEqual(DeleteUserResult.Failed, deleteResult);

            createResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.Successful, createResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            deleteResult = passwordService.DeleteUser(user);
            Assert.AreEqual(DeleteUserResult.Success, deleteResult);
            Assert.AreEqual(0, ((InMemoryDatabase)db).LocalUserDbAccess.Count);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod()]
        public void DeleteMultipleUsersTest()
        {
            IDatabase db = DatabaseFactory.GetDatabase(Database.InMemory);
            IPasswordService passwordService = new PasswordService(db, new MasterPassword(), new RijndaelManagedEncryption());

            CreateUserResult createResult;
            DeleteUserResult deleteResult;

            // Mock a user model that would be coming from the UI
            User[] users = 
            {
                new User("testAccount0", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "test@test.com"),
                new User("testAccount1", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "test@test.com"),
                new User("testAccount2", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "test@test.com"),
                new User("testAccount3", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "test@test.com"),
                new User("testAccount4", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "test@test.com"),
            };

            Int32 addedUsersCount = 1;
            foreach (var user in users)
            {
                createResult = passwordService.CreateNewUser(user);
                Assert.AreEqual(CreateUserResult.Successful, createResult);
                Assert.AreEqual(addedUsersCount, ((InMemoryDatabase)db).LocalUserDbAccess.Count);
                addedUsersCount++;
            }

            Int32 deletedUsersCount = users.Count() - 1;
            foreach (var user in users)
            {
                deleteResult = passwordService.DeleteUser(user);
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
            IDatabase db = DatabaseFactory.GetDatabase(Database.InMemory);
            IPasswordService passwordService = new PasswordService(db, new MasterPassword(), new RijndaelManagedEncryption());

            User user;
            CreateUserResult createResult;
            LoginResult loginResult;
            DeleteUserResult deleteResult;

            // Mock a user model that would be coming from the UI
            user = new User("testAccount", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "test@test.com");

            createResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.Successful, createResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            loginResult = passwordService.Login("testAccount", "testPassword1@");
            Assert.AreEqual(LoginResult.Successful, loginResult);

            // Mock a password model that would be coming from the UI
            Password[] passwords =
            {
                new Password("FakeStore0", "NewUsername", "email@FakeStore.com", "This is my FakeStore account.", "www.FakeStore.com", "123456"),
                new Password("FakeStore1", "NewUsername", "email@FakeStore.com", "This is my FakeStore account.", "www.FakeStore.com", "123456"),
                new Password("FakeStore2", "NewUsername", "email@FakeStore.com", "This is my FakeStore account.", "www.FakeStore.com", "123456"),
                new Password("FakeStore3", "NewUsername", "email@FakeStore.com", "This is my FakeStore account.", "www.FakeStore.com", "123456"),
            };

            Int32 addedPasswordsCount = 1;
            foreach (var password in passwords)
            {
                AddPasswordResult result = passwordService.AddPassword(password);
                Assert.AreEqual(AddPasswordResult.Success, result);
                Assert.AreEqual(addedPasswordsCount, ((InMemoryDatabase)db).LocalPasswordDbAccess.Count);
                addedPasswordsCount++;
            }

            deleteResult = passwordService.DeleteUser(user);
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
            IDatabase db = DatabaseFactory.GetDatabase(Database.InMemory);
            IPasswordService passwordService = new PasswordService(db, new MasterPassword(), new RijndaelManagedEncryption());

            CreateUserResult createResult;
            LoginResult loginResult;
            DeleteUserResult deleteResult;
            LogOutResult logOutResult;

            // Mock a user model that would be coming from the UI
            User[] users =
            {
                new User("testAccount0", "testPassword1@1", "testFirstName", "testLastName", "222-111-1111", "test@test.com"),
                new User("testAccount1", "testPassword1@2", "testFirstName", "testLastName", "222-111-1111", "test@test.com"),
                new User("testAccount2", "testPassword1@3", "testFirstName", "testLastName", "222-111-1111", "test@test.com"),
                new User("testAccount3", "testPassword1@4", "testFirstName", "testLastName", "222-111-1111", "test@test.com"),
                new User("testAccount4", "testPassword1@5", "testFirstName", "testLastName", "222-111-1111", "test@test.com"),
            };

            // Mock a password model that would be coming from the UI
            Password[] passwords =
            {
                new Password("FakeStore0", "NewUsername", "email@FakeStore.com", "This is my FakeStore account.", "www.FakeStore.com", "123456"),
                new Password("FakeStore1", "NewUsername", "email@FakeStore.com", "This is my FakeStore account.", "www.FakeStore.com", "123456"),
                new Password("FakeStore2", "NewUsername", "email@FakeStore.com", "This is my FakeStore account.", "www.FakeStore.com", "123456"),
                new Password("FakeStore3", "NewUsername", "email@FakeStore.com", "This is my FakeStore account.", "www.FakeStore.com", "123456"),
            };

            // Add each user to the database, log in, add each password to each user, logout
            Int32 addedUsersCount = 1;
            Int32 addedPasswordsCount = 1;
            foreach (var user in users)
            {
                createResult = passwordService.CreateNewUser(user);
                Assert.AreEqual(CreateUserResult.Successful, createResult);
                Assert.AreEqual(addedUsersCount, ((InMemoryDatabase)db).LocalUserDbAccess.Count);
                addedUsersCount++;

                loginResult = passwordService.Login(user.Username, user.PlainTextPassword);
                Assert.AreEqual(LoginResult.Successful, loginResult);
           
                foreach (var password in passwords)
                {
                    AddPasswordResult result = passwordService.AddPassword(password);
                    Assert.AreEqual(AddPasswordResult.Success, result);
                    Assert.AreEqual(addedPasswordsCount, ((InMemoryDatabase)db).LocalPasswordDbAccess.Count);
                    addedPasswordsCount++;
                }

                logOutResult = passwordService.Logout();
                Assert.AreEqual(LogOutResult.Success, logOutResult);
            }

            // Login to the account to delete
            loginResult = passwordService.Login("testAccount0", "testPassword1@1");
            Assert.AreEqual(LoginResult.Successful, loginResult);

            // Delete the account
            string currentUserGUID = passwordService.GetCurrentUser().GUID;
            deleteResult = passwordService.DeleteUser(passwordService.GetCurrentUser());
            Assert.AreEqual(DeleteUserResult.Success, deleteResult);
            Assert.AreEqual(((passwords.Length * users.Length) - passwords.Length), ((InMemoryDatabase)db).LocalPasswordDbAccess.Count);
            Assert.AreEqual((users.Length - 1), ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            // Verify that the deleted account's passwords are deleted from the password database table
            foreach (var password in ((InMemoryDatabase)db).LocalPasswordDbAccess)
            {
                if (password.UserGUID == currentUserGUID)
                {
                    Assert.Fail();
                }
            }
        }
    }
}

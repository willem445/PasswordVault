using Xunit;
using System;
using System.Linq;
using PasswordVault.Data;
using PasswordVault.Services;
using PasswordVault.Models;

namespace PasswordVault.ServicesTests
{
    public class DeleteUserTests
    {
        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void CreateUserTest()
        {
            IDatabase db = DatabaseFactory.GetDatabase(Database.InMemory);
            IPasswordService passwordService = new PasswordService(db, new MasterPassword(), new RijndaelManagedEncryption());

            CreateUserResult result;
            User user;

            user = new User("testAccount", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            result = passwordService.CreateNewUser(user);
            Assert.Equal(CreateUserResult.Successful, result);
            Assert.Single(((InMemoryDatabase)db).LocalUserDbAccess);

            result = passwordService.CreateNewUser(user);
            Assert.Equal(CreateUserResult.UsernameTaken, result);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
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
            Assert.Equal(DeleteUserResult.Failed, deleteResult);

            createResult = passwordService.CreateNewUser(user);
            Assert.Equal(CreateUserResult.Successful, createResult);
            Assert.Single(((InMemoryDatabase)db).LocalUserDbAccess);

            deleteResult = passwordService.DeleteUser(user);
            Assert.Equal(DeleteUserResult.Success, deleteResult);
            Assert.Empty(((InMemoryDatabase)db).LocalUserDbAccess);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
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
                Assert.Equal(CreateUserResult.Successful, createResult);
                Assert.Equal(addedUsersCount, ((InMemoryDatabase)db).LocalUserDbAccess.Count);
                addedUsersCount++;
            }

            Int32 deletedUsersCount = users.Count() - 1;
            foreach (var user in users)
            {
                deleteResult = passwordService.DeleteUser(user);
                Assert.Equal(DeleteUserResult.Success, deleteResult);
                Assert.Equal(deletedUsersCount, ((InMemoryDatabase)db).LocalUserDbAccess.Count);
                deletedUsersCount--;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
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
            Assert.Equal(CreateUserResult.Successful, createResult);
            Assert.Single(((InMemoryDatabase)db).LocalUserDbAccess);

            loginResult = passwordService.Login("testAccount", "testPassword1@");
            Assert.Equal(LoginResult.Successful, loginResult);

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
                Assert.Equal(AddPasswordResult.Success, result);
                Assert.Equal(addedPasswordsCount, ((InMemoryDatabase)db).LocalPasswordDbAccess.Count);
                addedPasswordsCount++;
            }

            deleteResult = passwordService.DeleteUser(user);
            Assert.Equal(DeleteUserResult.Success, deleteResult);
            Assert.Empty(((InMemoryDatabase)db).LocalPasswordDbAccess);
            Assert.Empty(((InMemoryDatabase)db).LocalUserDbAccess);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        
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
                Assert.Equal(CreateUserResult.Successful, createResult);
                Assert.Equal(addedUsersCount, ((InMemoryDatabase)db).LocalUserDbAccess.Count);
                addedUsersCount++;

                loginResult = passwordService.Login(user.Username, user.PlainTextPassword);
                Assert.Equal(LoginResult.Successful, loginResult);
           
                foreach (var password in passwords)
                {
                    AddPasswordResult result = passwordService.AddPassword(password);
                    Assert.Equal(AddPasswordResult.Success, result);
                    Assert.Equal(addedPasswordsCount, ((InMemoryDatabase)db).LocalPasswordDbAccess.Count);
                    addedPasswordsCount++;
                }

                logOutResult = passwordService.Logout();
                Assert.Equal(LogOutResult.Success, logOutResult);
            }

            // Login to the account to delete
            loginResult = passwordService.Login("testAccount0", "testPassword1@1");
            Assert.Equal(LoginResult.Successful, loginResult);

            // Delete the account
            string currentUserGUID = passwordService.GetCurrentUser().GUID;
            deleteResult = passwordService.DeleteUser(passwordService.GetCurrentUser());
            Assert.Equal(DeleteUserResult.Success, deleteResult);
            Assert.Equal(((passwords.Length * users.Length) - passwords.Length), ((InMemoryDatabase)db).LocalPasswordDbAccess.Count);
            Assert.Equal((users.Length - 1), ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            // Verify that the deleted account's passwords are deleted from the password database table
            foreach (var password in ((InMemoryDatabase)db).LocalPasswordDbAccess)
            {
                Assert.NotEqual(password.UserGUID, currentUserGUID);
            }
        }
    }
}

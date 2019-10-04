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
    /// Summary description for ModifyUserTests
    /// </summary>
    [TestClass]
    public class ModifyUserTests
    {
        public ModifyUserTests()
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

        [TestMethod]
        public void CreateUserTest()
        {
            IDatabase db = DatabaseFactory.GetDatabase(Database.InMemory);
            IPasswordService passwordService = new PasswordService(db, new MasterPassword(), new RijndaelManagedEncryption());

            CreateUserResult createUserResult;
            LoginResult loginResult;
            LogOutResult logoutResult;
            UserInformationResult modifyUserResult;
            User user;
            User userResult;

            user = new User("testAccount", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.Successful, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);

            modifyUserResult = passwordService.EditUser(new User(null, null, "testFirstName2", "testLastName2", "222-111-2222", "test2@test.com"));
            Assert.AreEqual(UserInformationResult.Failed, modifyUserResult);

            loginResult = passwordService.Login("testAccount", "testPassword1@");
            Assert.AreEqual(LoginResult.Successful, loginResult);
            userResult = passwordService.GetCurrentUser();
            Assert.AreEqual(userResult.FirstName, "testFirstName");
            Assert.AreEqual(userResult.LastName, "testLastName");
            Assert.AreEqual(userResult.PhoneNumber, "222-111-1111");
            Assert.AreEqual(userResult.Email, "test@test.com");

            modifyUserResult = passwordService.EditUser(new User(null, null, "testFirstName2", "testLastName2", "222-111-2222", "test2@test.com"));
            Assert.AreEqual(UserInformationResult.Success, modifyUserResult);

            userResult = passwordService.GetCurrentUser();
            Assert.AreEqual(userResult.FirstName, "testFirstName2");
            Assert.AreEqual(userResult.LastName, "testLastName2");
            Assert.AreEqual(userResult.PhoneNumber, "222-111-2222");
            Assert.AreEqual(userResult.Email, "test2@test.com");

            modifyUserResult = passwordService.EditUser(new User(null, null, "", "testLastName2", "222-111-2222", "test2@test.com"));
            Assert.AreEqual(UserInformationResult.InvalidFirstName, modifyUserResult);
            userResult = passwordService.GetCurrentUser();
            Assert.AreEqual(userResult.FirstName, "testFirstName2");
            Assert.AreEqual(userResult.LastName, "testLastName2");
            Assert.AreEqual(userResult.PhoneNumber, "222-111-2222");
            Assert.AreEqual(userResult.Email, "test2@test.com");

            modifyUserResult = passwordService.EditUser(new User(null, null, null, "testLastName2", "222-111-2222", "test2@test.com"));
            Assert.AreEqual(UserInformationResult.InvalidFirstName, modifyUserResult);
            userResult = passwordService.GetCurrentUser();
            Assert.AreEqual(userResult.FirstName, "testFirstName2");
            Assert.AreEqual(userResult.LastName, "testLastName2");
            Assert.AreEqual(userResult.PhoneNumber, "222-111-2222");
            Assert.AreEqual(userResult.Email, "test2@test.com");

            modifyUserResult = passwordService.EditUser(new User(null, null, "testFirstName2", "", "222-111-2222", "test2@test.com"));
            Assert.AreEqual(UserInformationResult.InvalidLastName, modifyUserResult);
            userResult = passwordService.GetCurrentUser();
            Assert.AreEqual(userResult.FirstName, "testFirstName2");
            Assert.AreEqual(userResult.LastName, "testLastName2");
            Assert.AreEqual(userResult.PhoneNumber, "222-111-2222");
            Assert.AreEqual(userResult.Email, "test2@test.com");

            modifyUserResult = passwordService.EditUser(new User(null, null, "testFirstName2", null, "222-111-2222", "test2@test.com"));
            Assert.AreEqual(UserInformationResult.InvalidLastName, modifyUserResult);
            userResult = passwordService.GetCurrentUser();
            Assert.AreEqual(userResult.FirstName, "testFirstName2");
            Assert.AreEqual(userResult.LastName, "testLastName2");
            Assert.AreEqual(userResult.PhoneNumber, "222-111-2222");
            Assert.AreEqual(userResult.Email, "test2@test.com");

            modifyUserResult = passwordService.EditUser(new User(null, null, "testFirstName2", "testLastName2", "222-111-222", "test2@test.com"));
            Assert.AreEqual(UserInformationResult.InvalidPhoneNumber, modifyUserResult);
            userResult = passwordService.GetCurrentUser();
            Assert.AreEqual(userResult.FirstName, "testFirstName2");
            Assert.AreEqual(userResult.LastName, "testLastName2");
            Assert.AreEqual(userResult.PhoneNumber, "222-111-2222");
            Assert.AreEqual(userResult.Email, "test2@test.com");

            modifyUserResult = passwordService.EditUser(new User(null, null, "testFirstName2", "testLastName2", "222-111-2222", "test2test.com"));
            Assert.AreEqual(UserInformationResult.InvalidEmail, modifyUserResult);
            userResult = passwordService.GetCurrentUser();
            Assert.AreEqual(userResult.FirstName, "testFirstName2");
            Assert.AreEqual(userResult.LastName, "testLastName2");
            Assert.AreEqual(userResult.PhoneNumber, "222-111-2222");
            Assert.AreEqual(userResult.Email, "test2@test.com");

        }
    }
}

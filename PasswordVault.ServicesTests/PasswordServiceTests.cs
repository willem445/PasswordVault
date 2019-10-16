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
    /// Summary description for PasswordServiceTests
    /// </summary>
    [TestClass]
    public class PasswordServiceTests
    {
        IDatabase db;
        IPasswordService passwordService;
        CreateUserResult createUserResult;
        LoginResult loginResult;
        LogOutResult logoutResult;
        AddPasswordResult addPasswordResult;
        User user;

        public PasswordServiceTests()
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

            user = new User("testAccount", "testPassword1@", "testFirstName", "testLastName", "222-111-1111", "test@test.com");
            createUserResult = passwordService.CreateNewUser(user);
            Assert.AreEqual(CreateUserResult.Successful, createUserResult);
            Assert.AreEqual(1, ((InMemoryDatabase)db).LocalUserDbAccess.Count);
        }
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),"Expected database null exception.")]
        public void PasswordServiceConstructorDatabaseArgTest()
        {
            db = DatabaseFactory.GetDatabase(Database.InMemory);
            PasswordService ps = new PasswordService(null, new MasterPassword(), new RijndaelManagedEncryption());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Expected master password null exception.")]
        public void PasswordServiceConstructorMasterPasswordArgTest()
        {
            db = DatabaseFactory.GetDatabase(Database.InMemory);
            PasswordService ps = new PasswordService(db, null, new RijndaelManagedEncryption());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Expected encryption null exception.")]
        public void PasswordServiceConstructorExcryptionArgTest()
        {
            db = DatabaseFactory.GetDatabase(Database.InMemory);
            PasswordService ps = new PasswordService(db, new MasterPassword(), null);
        }
    }
}

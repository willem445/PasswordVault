﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PasswordVault.Models;
using Dapper;
using System.Data.SQLite;

/*=================================================================================================
DESCRIPTION
*================================================================================================*/
/* 
 ------------------------------------------------------------------------------------------------*/

namespace PasswordVault.Data
{
    /*=================================================================================================
	ENUMERATIONS
	*================================================================================================*/

    /*=================================================================================================
	STRUCTS
	*================================================================================================*/

    /*=================================================================================================
	CLASSES
	*================================================================================================*/
    public class SQLiteDatabase : IDatabase
    {
        /*=================================================================================================
		CONSTANTS
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		FIELDS
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/
#if DEBUG
        private string DB_FILE = Path.Combine(Environment.CurrentDirectory, @"..\..\..\PasswordVault.Data\TestDb") + "\\PasswordDb.sqlite";
#else
        private string DB_FILE = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PasswordVault") + "\\PasswordDb.sqlite";
#endif

        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/
        private SQLiteConnection DbConnection
        {
            get { return new SQLiteConnection("Data Source=" + DB_FILE); }
        }

        private string DbFile
        {
            get { return DB_FILE; }
        }

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public SQLiteDatabase()
        {
            VerifyDbExists();
        }

        /*=================================================================================================
		PUBLIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        public bool AddUser(User user)
        {
            bool result = false;

            if (user != null)
            {
                using (var dbConn = DbConnection)
                {
                    dbConn.Open();

                    string query = @"INSERT INTO Users 
                                    (GUID, EncryptedKey, Username, Iterations, MemorySize, DegreeOfParallelism, Salt, Hash, FirstName, LastName, PhoneNumber, Email, PasswordEncryptionService, PasswordBlockSize, PasswordKeySize, PasswordIterations)
                                 VALUES(
                                    @GUID,
                                    @EncryptedKey,
                                    @Username,
                                    @Iterations,
                                    @MemorySize,
                                    @DegreeOfParallelism,
                                    @Salt,
                                    @Hash,
                                    @FirstName,
                                    @LastName,
                                    @PhoneNumber,
                                    @Email,
                                    @PasswordEncryptionService,
                                    @PasswordBlockSize,
                                    @PasswordKeySize,
                                    @PasswordIterations)";

                    int dbresult = dbConn.Execute(query, new
                    {
                        GUID = user.GUID,
                        EncryptedKey = user.EncryptedKey,
                        Username = user.Username,
                        Iterations = user.Iterations,
                        MemorySize = user.MemorySize,
                        DegreeOfParallelism = user.DegreeOfParallelism,
                        Salt = user.Salt,
                        Hash = user.Hash,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        Email = user.Email,
                        PasswordEncryptionService = user.PasswordEncryptionService,
                        PasswordBlockSize = user.PasswordBlockSize,
                        PasswordKeySize = user.PasswordKeySize,
                        PasswordIterations = user.PasswordIterations,
                    });

                    if (dbresult != 0)
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        /*************************************************************************************************/
        public bool DeleteUser(User user, int expectedNumPasswords)
        {
            bool result = false;
            bool userResult = false;
            bool userPasswordResult = false;

            if (user != null)
            {
                using (var dbConn = DbConnection)
                {
                    dbConn.Open();

                    string query = @"DELETE FROM Users
                                     WHERE GUID = @Guid";

                    var dbResult = dbConn.Execute(query, new { Guid = user.GUID });

                    if (dbResult == 1)
                    {
                        userResult = true;
                    }

                    query = @"DELETE FROM Passwords
                              WHERE UserGUID = @UserGUID";

                    dbResult = dbConn.Execute(query, new { UserGUID = user.GUID });

                    if (dbResult == expectedNumPasswords)
                    {
                        userPasswordResult = true;
                    }
                }
            }

            if (userResult && userPasswordResult)
            {
                result = true;
            }

            return result;
        }

        /*************************************************************************************************/
        public bool ModifyUser(User user, User modifiedUser)
        {
            bool result = false;

            if (user != null && modifiedUser != null)
            {
                using (var dbConn = DbConnection)
                {
                    dbConn.Open();

                    string query = @"UPDATE Users 
                                 SET EncryptedKey = @EncryptedKey, 
                                     Iterations = @Iterations, 
                                     MemorySize = @MemorySize,
                                     DegreeOfParallelism = @DegreeOfParallelism,
                                     Salt = @Salt,
                                     Hash = @Hash,
                                     FirstName = @FirstName,
                                     LastName = @LastName,
                                     PhoneNumber = @PhoneNumber,
                                     Email = @Email,
                                     PasswordEncryptionService = @PasswordEncryptionService,
                                     PasswordBlockSize = @PasswordBlockSize,
                                     PasswordKeySize = @PasswordKeySize,
                                     PasswordIterations = @PasswordIterations
                                 WHERE GUID = @GUID";

                    var dbResult = dbConn.Execute(query, new
                    {
                        EncryptedKey = modifiedUser.EncryptedKey,
                        Iterations = modifiedUser.Iterations,
                        MemorySize = modifiedUser.MemorySize,
                        DegreeOfParallelism = modifiedUser.DegreeOfParallelism,
                        Salt = modifiedUser.Salt,
                        Hash = modifiedUser.Hash,
                        FirstName = modifiedUser.FirstName,
                        LastName = modifiedUser.LastName,
                        PhoneNumber = modifiedUser.PhoneNumber,
                        Email = modifiedUser.Email,
                        PasswordEncryptionService = modifiedUser.PasswordEncryptionService,
                        PasswordBlockSize = modifiedUser.PasswordBlockSize,
                        PasswordKeySize = modifiedUser.PasswordKeySize,
                        PasswordIterations = modifiedUser.PasswordIterations,
                        GUID = user.GUID
                    });

                    if (dbResult > 0)
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        /*************************************************************************************************/
        public List<User> GetAllUsers()
        {
            List<User> result = new List<User>();

            using (var dbConn = DbConnection)
            {
                dbConn.Open();

                string query = @"SELECT * FROM Users";

                var user = dbConn.Query<User>(query);

                if (user.Any())
                {
                    result = user.ToList<User>();
                }
            }

            return result;
        }

        /*************************************************************************************************/
        public User GetUserByGUID(string guid)
        {
            User result = null;

            using (var dbConn = DbConnection)
            {
                dbConn.Open();

                string query = @"SELECT * FROM Users
                                 WHERE GUID = @GUID";

                var user = dbConn.Query<User>(query, new { Guid = guid }).FirstOrDefault();

                if (user != null)
                {
                    result = user;
                }
            }

            return result;
        }

        /*************************************************************************************************/
        public User GetUserByUsername(string username)
        {
            User result = null;

            using (var dbConn = DbConnection)
            {
                dbConn.Open();

                string query = @"SELECT * FROM Users
                                 WHERE Username = @Username";

                var user = dbConn.Query<User>(query, new { Username = username }).FirstOrDefault();

                if (user != null)
                {
                    result = user;
                }
            }

            return result;
        }

        /*************************************************************************************************/
        public bool UserExistsByGUID(string guid)
        {
            bool result = false;

            using (var dbConn = DbConnection)
            {
                dbConn.Open();

                string query = @"SELECT * FROM Users
                                 WHERE GUID = @GUID";

                var dbresult = dbConn.Query<User>(query, new { GUID = guid });

                if (dbresult.Any())
                {
                    result = true;
                }
            }

            return result;
        }

        /*************************************************************************************************/
        public bool UserExistsByUsername(string username)
        {
            bool result = false;

            using (var dbConn = DbConnection)
            {
                dbConn.Open();

                string query = @"SELECT * FROM Users
                                 WHERE Username = @Username";

                var dbresult = dbConn.Query<User>(query, new { Username = username });

                if (dbresult.Any())
                {
                    result = true;
                }
            }

            return result;
        }

        /*************************************************************************************************/
        public List<DatabasePassword> GetUserPasswordsByGUID(string guid)
        {
            List<DatabasePassword> result = new List<DatabasePassword>();

            using (var dbConn = DbConnection)
            {
                dbConn.Open();

                string query = @"SELECT * FROM Passwords 
                                 WHERE UserGUID = @Guid";

                var dbresult = dbConn.Query<DatabasePassword>(query, new { Guid = guid });

                if (dbresult.Any())
                {
                    result = dbresult.ToList<DatabasePassword>();
                }
            }

            return result;
        }

        /*************************************************************************************************/
        public Int64 AddPassword(DatabasePassword password)
        {
            Int64 uniqueID = -1;

            if (password != null)
            {
                using (var dbConn = DbConnection)
                {
                    dbConn.Open();

                    string query = @"INSERT INTO Passwords 
                                    (UserGUID, Application, Username, Email, Description, Website, Passphrase)
                                 VALUES(
                                    @UserGUID,
                                    @Application,
                                    @Username,
                                    @Email,
                                    @Description,
                                    @Website,
                                    @Passphrase);
                                 SELECT last_insert_rowid()";

                    int dbresult = dbConn.Query<int>(query, new
                    {
                        UserGUID = password.UserGUID,
                        Application = password.Application,
                        Username = password.Username,
                        Email = password.Email,
                        Description = password.Description,
                        Website = password.Website,
                        Passphrase = password.Passphrase,
                    }).Single();

                    if (dbresult >= 0)
                    {
                        uniqueID = dbresult;
                    }
                }
            }

            return uniqueID;
        }

        /*************************************************************************************************/
        public bool ModifyPassword(DatabasePassword modifiedPassword)
        {
            bool result = false;

            if (modifiedPassword != null)
            {
                using (var dbConn = DbConnection)
                {
                    dbConn.Open();

                    string query = @"UPDATE Passwords 
                                 SET Application = @Application, 
                                     Username = @Username, 
                                     Email = @Email,
                                     Description = @Description,
                                     Website = @Website,
                                     Passphrase = @Passphrase
                                 WHERE UniqueID = @UniqueID";

                    var dbResult = dbConn.Execute(query, new
                    {
                        Application = modifiedPassword.Application,
                        Username = modifiedPassword.Username,
                        Email = modifiedPassword.Email,
                        Description = modifiedPassword.Description,
                        Website = modifiedPassword.Website,
                        Passphrase = modifiedPassword.Passphrase,
                        UniqueID = modifiedPassword.UniqueID
                    });

                    if (dbResult > 0)
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        /*************************************************************************************************/
        public bool DeletePassword(Int64 passwordUniqueId)
        {
            bool result = false;

            using (var dbConn = DbConnection)
            {
                dbConn.Open();

                string query = @"DELETE FROM Passwords
                                WHERE UniqueID = @UniqueID";

                var dbResult = dbConn.Execute(query, new { UniqueID = passwordUniqueId });

                if (dbResult == 1)
                {
                    result = true;
                }
            }
            
            return result;
        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        private void VerifyDbExists()
        {
            if (!File.Exists(DbFile))
            {
                var dir = Path.GetDirectoryName(DbFile);
                Directory.CreateDirectory(dir);
                var file = File.Create(DbFile);
                file.Close();

                CreateDatabase();
            }
        }

        /*************************************************************************************************/
        private void CreateDatabase()
        {
            using (var dbConn = DbConnection)
            {
                dbConn.Open();

                dbConn.Execute(@"
                    CREATE TABLE IF NOT EXISTS [Users] (
                        [GUID] TEXT NOT NULL PRIMARY KEY,
                        [EncryptedKey] TEXT NOT NULL,
                        [Username] TEXT NOT NULL,
                        [Iterations] INTEGER NOT NULL,
                        [MemorySize] INTEGER NOT NULL,
                        [DegreeOfParallelism] INTEGER NOT NULL,
                        [Salt] TEXT NOT NULL,
                        [Hash] TEXT NOT NULL,
                        [FirstName] TEXT NOT NULL,
                        [LastName] TEXT NOT NULL,
                        [PhoneNumber] TEXT NOT NULL,
                        [Email] TEXT NOT NULL,
                        [PasswordEncryptionService] INTEGER,
                        [PasswordBlockSize] INTEGER,
                        [PasswordKeySize] INTEGER,
                        [PasswordIterations] INTEGER
                    )");

                dbConn.Execute(@"
                    CREATE TABLE IF NOT EXISTS [Passwords] (
                        [UniqueID] INTEGER PRIMARY KEY,
                        [UserGUID] TEXT NOT NULL,                
                        [Application] TEXT NOT NULL,
                        [Username] TEXT NOT NULL,
                        [Email] TEXT NOT NULL,
                        [Description] TEXT NOT NULL,
                        [Website] TEXT NOT NULL,
                        [Passphrase] TEXT NOT NULL
                    )");
            }
        }

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // SQLiteDatabase CLASS
} // PasswordVault.Data.Standard NAMESPACE

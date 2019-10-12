using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Dapper;
using System.IO;
using System.Linq;
using PasswordVault.Models;

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
        private string DB_FILE = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PasswordVault") + "\\PasswordDb.sqlite";
        private Int64 _lastUniqueId = 0;

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
                                    (GUID, EncryptedKey, Username, Iterations, Salt, Hash, FirstName, LastName, PhoneNumber, Email)
                                 VALUES(
                                    @GUID,
                                    @EncryptedKey,
                                    @Username,
                                    @Iterations,
                                    @Salt,
                                    @Hash,
                                    @FirstName,
                                    @LastName,
                                    @PhoneNumber,
                                    @Email)";

                    int dbresult = dbConn.Execute(query, new
                    {
                        GUID = user.GUID,
                        EncryptedKey = user.EncryptedKey,
                        Username = user.Username,
                        Iterations = user.Iterations,
                        Salt = user.Salt,
                        Hash = user.Hash,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        Email = user.Email
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
        public bool DeleteUser(User user)
        {
            bool result = false;

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
                        result = true;
                    }
                }
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
                                     Salt = @Salt,
                                     Hash = @Hash,
                                     FirstName = @FirstName,
                                     LastName = @LastName,
                                     PhoneNumber = @PhoneNumber,
                                     Email = @Email
                                 WHERE GUID = @GUID";

                    var dbResult = dbConn.Execute(query, new
                    {
                        EncryptedKey = modifiedUser.EncryptedKey,
                        Iterations = modifiedUser.Iterations,
                        Salt = modifiedUser.Salt,
                        Hash = modifiedUser.Hash,
                        FirstName = modifiedUser.FirstName,
                        LastName = modifiedUser.LastName,
                        PhoneNumber = modifiedUser.PhoneNumber,
                        Email = modifiedUser.Email,
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
        public bool AddPassword(DatabasePassword password)
        {
            bool result = false;

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
                        result = true;
                        _lastUniqueId = dbresult;
                    }
                }
            }

            return result;
        }

        /*************************************************************************************************/
        public bool ModifyPassword(DatabasePassword password, DatabasePassword modifiedPassword)
        {
            bool result = false;

            if (password != null && modifiedPassword != null)
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
                        UniqueID = password.UniqueID
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
        public bool DeletePassword(DatabasePassword password)
        {
            bool result = false;

            if (password != null)
            {
                using (var dbConn = DbConnection)
                {
                    dbConn.Open();

                    string query = @"DELETE FROM Passwords
                                 WHERE UniqueID = @UniqueID";

                    var dbResult = dbConn.Execute(query, new { UniqueID = password.UniqueID });

                    if (dbResult == 1)
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        /*************************************************************************************************/
        public Int64 GetLastUniqueId()
        {
            return _lastUniqueId;
        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        private void VerifyDbExists()
        {
            if (!File.Exists(DbFile))
            {
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PasswordVault"));
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
                        [Salt] TEXT NOT NULL,
                        [Hash] TEXT NOT NULL,
                        [FirstName] TEXT NOT NULL,
                        [LastName] TEXT NOT NULL,
                        [PhoneNumber] TEXT NOT NULL,
                        [Email] TEXT NOT NULL
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
} // PasswordVault.Data NAMESPACE

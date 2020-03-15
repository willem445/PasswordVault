using System;
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
                                    (Uuid, 
                                     EncryptedKey, 
                                     Username, 
                                     Hash, 
                                     FirstName, 
                                     LastName, 
                                     PhoneNumber, 
                                     Email,
                                     SwVersion)
                                 VALUES(
                                    @Uuid,
                                    @EncryptedKey,
                                    @Username,
                                    @Hash,
                                    @FirstName,
                                    @LastName,
                                    @PhoneNumber,
                                    @Email,
                                    @SwVersion)";

                    int dbresult = dbConn.Execute(query, new
                    {
                        Uuid = user.Uuid,
                        EncryptedKey = user.EncryptedKey,
                        Username = user.Username,
                        Hash = user.Hash,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        Email = user.Email,
                        SwVersion = user.SwVersion
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
                                     WHERE Uuid = @Uuid";

                    var dbResult = dbConn.Execute(query, new { Uuid = user.Uuid });

                    if (dbResult == 1)
                    {
                        userResult = true;
                    }

                    query = @"DELETE FROM Passwords
                              WHERE UserUuid = @UserUuid";

                    dbResult = dbConn.Execute(query, new { UserUuid = user.Uuid });

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
                                     Hash = @Hash,
                                     FirstName = @FirstName,
                                     LastName = @LastName,
                                     PhoneNumber = @PhoneNumber,
                                     Email = @Email,
                                     SwVersion = @SwVersion
                                 WHERE Uuid = @Uuid";

                    var dbResult = dbConn.Execute(query, new
                    {
                        EncryptedKey = modifiedUser.EncryptedKey,
                        Hash = modifiedUser.Hash,
                        FirstName = modifiedUser.FirstName,
                        LastName = modifiedUser.LastName,
                        PhoneNumber = modifiedUser.PhoneNumber,
                        Email = modifiedUser.Email,
                        SwVersion = modifiedUser.SwVersion,
                        Uuid = user.Uuid,
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
        public User GetUserByUuid(string uuid)
        {
            User result = null;

            using (var dbConn = DbConnection)
            {
                dbConn.Open();

                string query = @"SELECT * FROM Users
                                 WHERE Uuid = @Uuid";

                var user = dbConn.Query<User>(query, new { Uuid = uuid }).FirstOrDefault();

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
        public bool UserExistsByUuid(string uuid)
        {
            bool result = false;

            using (var dbConn = DbConnection)
            {
                dbConn.Open();

                string query = @"SELECT * FROM Users
                                 WHERE Uuid = @Uuid";

                var dbresult = dbConn.Query<User>(query, new { Uuid = uuid });

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
        public List<DatabasePassword> GetUserPasswordsByUuid(string uuid)
        {
            List<DatabasePassword> result = new List<DatabasePassword>();

            using (var dbConn = DbConnection)
            {
                dbConn.Open();

                string query = @"SELECT * FROM Passwords 
                                 WHERE UserUuid = @Uuid";

                var dbresult = dbConn.Query<DatabasePassword>(query, new { Uuid = uuid });

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
                                    (UserUuid, Application, Username, Email, Description, Website, Passphrase)
                                 VALUES(
                                    @UserUuid,
                                    @Application,
                                    @Username,
                                    @Email,
                                    @Description,
                                    @Website,
                                    @Passphrase);
                                 SELECT last_insert_rowid()";

                    int dbresult = dbConn.Query<int>(query, new
                    {
                        UserUuid = password.UserUuid,
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
                        [Uuid] TEXT NOT NULL PRIMARY KEY,
                        [EncryptedKey] TEXT NOT NULL,
                        [Username] TEXT NOT NULL,
                        [Hash] TEXT NOT NULL,
                        [FirstName] TEXT NOT NULL,
                        [LastName] TEXT NOT NULL,
                        [PhoneNumber] TEXT NOT NULL,
                        [Email] TEXT NOT NULL,
                        [SwVersion] TEXT NOT NULL
                    )");

                dbConn.Execute(@"
                    CREATE TABLE IF NOT EXISTS [Passwords] (
                        [UniqueID] INTEGER PRIMARY KEY,
                        [UserUuid] TEXT NOT NULL,                
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

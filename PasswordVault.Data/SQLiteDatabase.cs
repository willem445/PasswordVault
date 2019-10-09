using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Dapper;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private string DB_FILE = Environment.CurrentDirectory + "\\PasswordDb.sqlite";

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

            //User user = new User("1", "5567", "will", "1000", "sdf", "sdfsdf", "will", "hoffman", "222-222-2222", "email");
            //User user2 = new User("12", "5567", "will1", "1000", "sdf", "sdfsdf", "will", "hoffman", "222-222-2222", "email");
            //User user3 = new User("13", "5567", "will2", "1000", "sdf", "sdfsdf", "will", "hoffman", "222-222-2222", "email");

            //User modify = new User("13", "5", "will", "5", "5", "5", "5", "5", "5", "5");

            //AddUser(user);
            //AddUser(user2);
            //AddUser(user3);

            //GetAllUsers();

            //ModifyUser(user3, modify);

            //GetAllUsers();
        }

        /*=================================================================================================
		PUBLIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        public bool AddUser(User user)
        {
            bool result = false;

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

            return result;
        }

        /*************************************************************************************************/
        public bool DeleteUser(User user)
        {
            bool result = false;

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

            return result;
        }

        /*************************************************************************************************/
        public bool ModifyUser(User user, User modifiedUser)
        {
            bool result = false;

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

                var dbResult = dbConn.Execute(query, new { 
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

                if (user.Count() > 0)
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

                var user = dbConn.Query<User>(query, new { Guid = guid });

                if (user.Count() == 1)
                {
                    result = user.ElementAt(0);
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

                var user = dbConn.Query<User>(query, new { Username = username });

                if (user.Count() == 1)
                {
                    result = user.ElementAt(0);
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

                if (dbresult.Count() > 0)
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

                if (dbresult.Count() > 0)
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

                if (dbresult.Count() > 0)
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
                                    @Passphrase)";

                int dbresult = dbConn.Execute(query, new
                {
                    UserGUID = password.UserGUID,
                    Application = password.Application,
                    Username = password.Username,
                    Email = password.Email,
                    Description = password.Description,
                    Website = password.Website,
                    Passphrase = password.Passphrase,
                });

                if (dbresult != 0)
                {
                    result = true;
                }
            }

            return result;
        }

        /*************************************************************************************************/
        public bool ModifyPassword(DatabasePassword password, DatabasePassword modifiedPassword)
        {
            bool result = false;

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
                    Passphrase = modifiedPassword.Passphrase
                });

                if (dbResult > 0)
                {
                    result = true;
                }
            }

            return result;
        }

        /*************************************************************************************************/
        public bool DeletePassword(DatabasePassword password)
        {
            bool result = false;

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

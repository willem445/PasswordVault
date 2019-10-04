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
            CreateDatabase();
        }

        /*=================================================================================================
		PUBLIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        public bool AddPassword(DatabasePassword password)
        {
            throw new NotImplementedException();
        }

        public bool AddUser(User user)
        {
            throw new NotImplementedException();
        }

        public bool DeletePassword(DatabasePassword password)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser(User user)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public User GetUserByGUID(string guid)
        {
            throw new NotImplementedException();
        }

        public User GetUserByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public List<DatabasePassword> GetUserPasswordsByGUID(string guid)
        {
            throw new NotImplementedException();
        }

        public bool ModifyPassword(DatabasePassword password, DatabasePassword modifiedPassword)
        {
            throw new NotImplementedException();
        }

        public bool ModifyUser(User user, User modifiedUser)
        {
            throw new NotImplementedException();
        }

        public bool UserExistsByGUID(string guid)
        {
            throw new NotImplementedException();
        }

        public bool UserExistsByUsername(string username)
        {
            throw new NotImplementedException();
        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        private void VerifyDbExists()
        {
            if (!File.Exists(DbFile))
            {
                
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
                        [UserGUID] TEXT NOT NULL REFERENCES Users(GUID), 
                        [UniqueID] INTEGER NOT NULL,
                        [Appliction] TEXT NOT NULL,
                        [Username] TEXT NOT NULL,
                        [Email] TEXT NOT NULL,
                        [Description] TEXT NOT NULL,
                        [Website] TEXT NOT NULL,
                        [Passphrase] TEXT NOT NULL
                    )");

                dbConn.Execute(@"
                INSERT INTO Users
                    (GUID, EncryptedKey, Username, Iterations, Salt, Hash, FirstName, LastName, PhoneNumber, Email)
                VALUES
                    ('1234', 'fasdfdasf', 'willem445', '1000', 'adsg', 'sdgsdfg', 'Wille', 'hoff', '222-222-2222', 'will@test.com') 
                ");

                var user = dbConn.Query<User>(
                    "SELECT * FROM Users WHERE GUID = '1234'");
            }
        }

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // SQLiteDatabase CLASS
} // PasswordVault.Data NAMESPACE

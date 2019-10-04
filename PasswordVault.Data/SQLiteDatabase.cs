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
            bool result = false;

            using (var dbConn = DbConnection)
            {
                dbConn.Open();

                int dbresult = dbConn.Execute(string.Format(@"
                    INSERT INTO Users
                        (GUID, EncryptedKey, Username, Iterations, Salt, Hash, FirstName, LastName, PhoneNumber, Email)
                    VALUES
                        ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}') 
                    ", user.GUID, user.EncryptedKey, user.Username, user.Iterations, user.Salt, user.Hash, user.FirstName, user.LastName, user.PhoneNumber, user.Email));

                if (dbresult != 0)
                {
                    result = true;
                }
            }


            return result;
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
            User result = null;

            using (var dbConn = DbConnection)
            {
                dbConn.Open();

                var user = dbConn.Query<User>(string.Format(
                    "SELECT * FROM Users WHERE GUID = '{0}'", guid));

                if (user.Count() == 1)
                {
                    result = user.ElementAt(0);
                }
            }

            return result;
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
            }
        }

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // SQLiteDatabase CLASS
} // PasswordVault.Data NAMESPACE

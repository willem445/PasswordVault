
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/*=================================================================================================
DESCRIPTION
*================================================================================================*/
/* TODO - may need more error handling if table or entry does not exist
 ------------------------------------------------------------------------------------------------*/

namespace PasswordVault
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
    public sealed class CsvDatabase : IDatabase
    {
        /*=================================================================================================
		CONSTANTS
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/
        private const string BASE_PATH = @"..\..\CSV\";
        private const string USERS_CSV_PATH = BASE_PATH + @"users.csv";

        /*=================================================================================================
		FIELDS
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/
        private string _passwordFileName = "";
        private List<User> _encryptedUsers;
        private List<Password> _encryptedPasswords;
        private static CsvDatabase _instance = null;
        private static Object _mutex = new Object();
        private ICSVUserManager _csvUserManager;
        private ICSVPasswordManager _csvPasswordManager;
        private bool _validUserPasswordTable = false;


        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        private CsvDatabase(ICSVUserManager csvUserManager, ICSVPasswordManager csvPasswordManager)
        {
            _csvUserManager = csvUserManager;
            _csvPasswordManager = csvPasswordManager;

            if (!Directory.Exists(BASE_PATH))
            {
                Directory.CreateDirectory(BASE_PATH);
            }

            if (!File.Exists(USERS_CSV_PATH))
            {
                File.Create(USERS_CSV_PATH);
            }

            _csvUserManager.ParseUsersCSVFile(USERS_CSV_PATH);
            _encryptedUsers = _csvUserManager.GetEncryptedUsers();
          
            _encryptedPasswords = new List<Password>();
        }

        /*=================================================================================================
		PUBLIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        public static CsvDatabase GetInstance(ICSVUserManager parseUsers, ICSVPasswordManager parsePasswords)
        {
            if (_instance == null)
            {
                lock (_mutex) // now I can claim some form of thread safety...
                {
                    if (_instance == null)
                    {
                        _instance = new CsvDatabase(parseUsers, parsePasswords);
                    }
                }
            }

            return _instance;
        }

        /*************************************************************************************************/
        public void AddUser(string username, string salt, string hash)
        {
            _encryptedUsers.Add(new User(username, salt, hash));
            _csvUserManager.UpdateUsersCSVFile(USERS_CSV_PATH, _encryptedUsers);

        }

        /*************************************************************************************************/
        public void ModifyUser(User user, User modifiedUser)
        {
            int index = GetIndexOfUser(user);

            if (index != -1)
            {
                _encryptedUsers[index] = modifiedUser;
                _csvUserManager.UpdateUsersCSVFile(USERS_CSV_PATH, _encryptedUsers);
            }            
        }

        /*************************************************************************************************/
        public void DeleteUser(User user)
        {
            int index = GetIndexOfUser(user);

            if (index != -1)
            {
                _encryptedUsers.Remove(user);
                _csvUserManager.UpdateUsersCSVFile(USERS_CSV_PATH, _encryptedUsers);
            }
        }

        /*************************************************************************************************/
        public User GetUser(string username)
        {
            User user;

            user = _encryptedUsers.FirstOrDefault(x => x.UserID == username);

            return user;
        }

        /*************************************************************************************************/
        public List<User> GetUsers()
        {
            return _encryptedUsers;
        }

        /*************************************************************************************************/
        public bool UserExists(User user)
        {
            bool exists = _encryptedUsers.Exists(x => x.UserID == user.UserID);
            return exists;
        }

        /*************************************************************************************************/
        public bool UserPasswordTableExists(User user)
        {
            return File.Exists(CreateCSVFileNameFromTable(user.UserID));
        }

        /*************************************************************************************************/
        public bool SetUserPasswordTableName(string name)
        {
            bool tableExists = false;
            string userTableFileName = CreateCSVFileNameFromTable(name);

            if (File.Exists(userTableFileName))
            {
                tableExists = true;
                _validUserPasswordTable = true;
                _passwordFileName = userTableFileName;

                _csvPasswordManager.ParsePasswordCSVFile(_passwordFileName);
                _encryptedPasswords = _csvPasswordManager.GetEncryptedPasswords();
            }

            return tableExists;
        }

        /*************************************************************************************************/
        public void ClearUserPasswordTableName()
        {
            _passwordFileName = "";
            _encryptedPasswords = new List<Password>();
            _validUserPasswordTable = false;
        }

        /*************************************************************************************************/
        public void CreateUserPasswordTable(string name)
        {
            string userTableFileName = CreateCSVFileNameFromTable(name);

            File.Create(userTableFileName);
        }
            
        /*************************************************************************************************/
        public void AddPassword(Password password)
        {
            _encryptedPasswords.Add(password);
            _csvPasswordManager.UpdatePasswordCSVFile(_passwordFileName, _encryptedPasswords);
        }

        /*************************************************************************************************/
        public List<Password> GetPasswords()
        {
            return _encryptedPasswords;
        }

        /*************************************************************************************************/
        public void ModifyPassword(Password password, Password modifiedPassword)
        {
            int index = GetIndexOfPassword(password);

            if (index != -1)
            {
                _encryptedPasswords[index] = modifiedPassword;
                _csvPasswordManager.UpdatePasswordCSVFile(_passwordFileName, _encryptedPasswords);
            }
        }

        /*************************************************************************************************/
        public void DeletePassword(Password password)
        {
            int index = GetIndexOfPassword(password);

            if (index != -1)
            {
                _encryptedPasswords.Remove(password);
                _csvPasswordManager.UpdatePasswordCSVFile(_passwordFileName, _encryptedPasswords);
            }
        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        private int GetIndexOfUser(User user)
        {
            int index = -1;

            index = _encryptedUsers.IndexOf(user);

            return index;
        }

        /*************************************************************************************************/
        private int GetIndexOfPassword(Password pass)
        {
            int index = -1;

            index = _encryptedPasswords.IndexOf(pass);

            return index;
        }

        /*************************************************************************************************/
        private string CreateCSVFileNameFromTable(string name)
        {
            string userTableFileName = BASE_PATH + name + ".csv";

            return userTableFileName;
        }

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // CSV CLASS
} // PasswordHashTest NAMESPACE

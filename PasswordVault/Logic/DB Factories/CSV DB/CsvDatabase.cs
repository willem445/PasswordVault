
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

/*=================================================================================================
DESCRIPTION
*================================================================================================*/
/* TODO - may need more error handling if table or entry does not exist
 * TODO - Make these async Tasks
 * TODO - Make one big passwords table instead of table for each user
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
        private const string PASSWORDS_CSV_PATH = BASE_PATH + @"passwords.csv";

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

        private string _usersCsvPath = USERS_CSV_PATH;
        private string _passwordsCsvPath = PASSWORDS_CSV_PATH;

        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/
        public string UsersCsvPathOverride
        {
            set
            {
                _usersCsvPath = value;
                _csvUserManager.ParseUsersCSVFile(_usersCsvPath); 
                _encryptedUsers = _csvUserManager.GetEncryptedUsers();
            }
        }

        public string PasswordCsvPathOverride
        {
            set
            {
                _passwordsCsvPath = value;              
                _csvPasswordManager.ParsePasswordCSVFile(_passwordsCsvPath);
                _encryptedPasswords = _csvPasswordManager.GetEncryptedPasswords();
            }
        }

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        private CsvDatabase(ICSVUserManager csvUserManager, ICSVPasswordManager csvPasswordManager)
        {
            _csvUserManager = csvUserManager;
            _csvPasswordManager = csvPasswordManager;

            VerifyTablesExist();

            // Update each list on start up
            _csvUserManager.ParseUsersCSVFile(_usersCsvPath); // TODO - Make this async
            _encryptedUsers = _csvUserManager.GetEncryptedUsers();

            _csvPasswordManager.ParsePasswordCSVFile(_passwordsCsvPath);
            _encryptedPasswords = _csvPasswordManager.GetEncryptedPasswords();
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
            _csvUserManager.UpdateUsersCSVFile(_usersCsvPath, _encryptedUsers);
        }

        /*************************************************************************************************/
        public void ModifyUser(string username, User modifiedUser)
        {
            int index = GetIndexOfUser(username);

            if (index != -1)
            {
                _encryptedUsers[index] = modifiedUser;
                _csvUserManager.UpdateUsersCSVFile(_usersCsvPath, _encryptedUsers);
            }    
        }

        /*************************************************************************************************/
        public void DeleteUser(string username)
        {
            int index = GetIndexOfUser(username);

            if (index != -1)
            {
                _encryptedUsers.RemoveAt(index);
                _csvUserManager.UpdateUsersCSVFile(_usersCsvPath, _encryptedUsers);
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
        public List<User> GetAllUsers()
        {
            return _encryptedUsers;
        }

        /*************************************************************************************************/
        public bool UserExists(string username)
        {
            bool exists = _encryptedUsers.Exists(x => x.UserID == username);
            return exists;
        }
            
        /*************************************************************************************************/
        public void AddPassword(string username, Password password)
        {
            _encryptedPasswords.Add(password);
            _csvPasswordManager.UpdatePasswordCSVFile(_passwordFileName, _encryptedPasswords);
        }

        /*************************************************************************************************/
        public void ModifyPassword(string username, Password password, Password modifiedPassword)
        {
            int index = GetIndexOfPassword(password);

            if (index != -1)
            {
                _encryptedPasswords[index] = modifiedPassword;
                _csvPasswordManager.UpdatePasswordCSVFile(_passwordFileName, _encryptedPasswords);
            }
        }

        /*************************************************************************************************/
        public void DeletePassword(string username, Password password)
        {
            int index = GetIndexOfPassword(password);

            if (index != -1)
            {
                _encryptedPasswords.Remove(password);
                _csvPasswordManager.UpdatePasswordCSVFile(_passwordFileName, _encryptedPasswords);
            }
        }

        /*************************************************************************************************/
        public List<Password> GetUserPasswords(string username)
        {
            List<Password> result = (from Password password in _encryptedPasswords
                                    where password.ID == username
                                    select password).ToList<Password>();
            return result;      
        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        private int GetIndexOfUser(string username)
        {
            int index = -1;

            index = _encryptedUsers.FindIndex(x => x.UserID == username);

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

        /*************************************************************************************************/
        private void VerifyTablesExist()
        {
            if (!Directory.Exists(BASE_PATH))
            {
                Directory.CreateDirectory(BASE_PATH);
            }

            if (!File.Exists(_usersCsvPath))
            {
                File.Create(_usersCsvPath);
            }

            if (!File.Exists(_passwordsCsvPath))
            {
                File.Create(_passwordsCsvPath);
            }
        }

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // CSV CLASS
} // PasswordHashTest NAMESPACE

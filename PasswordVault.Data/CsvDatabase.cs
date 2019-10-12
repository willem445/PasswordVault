using System;
using System.Collections.Generic;
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
    public class CsvDatabase : IDatabase
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
        private List<User> _encryptedUsers;
        private List<DatabasePassword> _encryptedPasswords;
        private ICSVUserManager _csvUserManager;
        private ICSVPasswordManager _csvPasswordManager;

        private string _usersCsvPath = USERS_CSV_PATH;
        private string _passwordsCsvPath = PASSWORDS_CSV_PATH;

        private Int64 _lastUniqueId = 0;

        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/
#pragma warning disable CA1044 // Properties should not be write only
        public string UsersCsvPathOverride
#pragma warning restore CA1044 // Properties should not be write only
        {
            set
            {
                _usersCsvPath = value;
                _csvUserManager.ParseUsersCSVFile(_usersCsvPath);
                _encryptedUsers = _csvUserManager.GetEncryptedUsers();
            }
        }

#pragma warning disable CA1044 // Properties should not be write only
        public string PasswordCsvPathOverride
#pragma warning restore CA1044 // Properties should not be write only
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
        public CsvDatabase(ICSVUserManager csvUserManager, ICSVPasswordManager csvPasswordManager)
        {
            if (csvUserManager == null)
            {
                throw new ArgumentNullException(nameof(csvUserManager));
            }

            if (csvPasswordManager == null)
            {
                throw new ArgumentNullException(nameof(csvPasswordManager));
            }

            _csvUserManager = csvUserManager;
            _csvPasswordManager = csvPasswordManager;

            VerifyTablesExist();

            // Update each list on start up
            _csvUserManager.ParseUsersCSVFile(_usersCsvPath);
            _encryptedUsers = _csvUserManager.GetEncryptedUsers();

            _csvPasswordManager.ParsePasswordCSVFile(_passwordsCsvPath);
            _encryptedPasswords = _csvPasswordManager.GetEncryptedPasswords();
        }

        /*=================================================================================================
		PUBLIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

        /*************************************************************************************************/
        public bool AddUser(User user)
        {
            _encryptedUsers.Add(user);
            _csvUserManager.UpdateUsersCSVFile(_usersCsvPath, _encryptedUsers);

            return true;
        }

        /*************************************************************************************************/
        public bool ModifyUser(User user, User modifiedUser)
        {
            bool result = false;

            if (user != null && modifiedUser != null)
            {
                int index = GetIndexOfUser(user.GUID);

                if (index != -1)
                {
                    _encryptedUsers[index] = modifiedUser;
                    _csvUserManager.UpdateUsersCSVFile(_usersCsvPath, _encryptedUsers);
                    result = true;
                }
                else
                {
                    result = false;
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
                int index = GetIndexOfUser(user.GUID);

                if (index != -1)
                {
                    _encryptedUsers.RemoveAt(index);
                    _encryptedPasswords.RemoveAll(x => x.UserGUID == user.GUID);
                    _csvUserManager.UpdateUsersCSVFile(_usersCsvPath, _encryptedUsers);
                    _csvPasswordManager.UpdatePasswordCSVFile(_passwordsCsvPath, _encryptedPasswords);
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        /*************************************************************************************************/
        public User GetUserByUsername(string username)
        {
            User user;

            user = _encryptedUsers.FirstOrDefault(x => x.Username == username);

            return user;
        }

        /*************************************************************************************************/
        public User GetUserByGUID(string guid)
        {
            User user;

            user = _encryptedUsers.FirstOrDefault(x => x.GUID == guid);

            return user;
        }

        /*************************************************************************************************/
        public List<User> GetAllUsers()
        {
            return _encryptedUsers;
        }

        /*************************************************************************************************/
        public bool UserExistsByUsername(string username)
        {
            bool exists = _encryptedUsers.Exists(x => x.Username == username);
            return exists;
        }

        /*************************************************************************************************/
        public bool UserExistsByGUID(string guid)
        {
            bool exists = _encryptedUsers.Exists(x => x.GUID == guid);
            return exists;
        }

        /*************************************************************************************************/
        public bool AddPassword(DatabasePassword password)
        {
            Int64 uniqueID = 0;
            bool result = false;

            if (password != null)
            {
                if (_encryptedPasswords.Count != 0)
                {
                    uniqueID = _encryptedPasswords[_encryptedPasswords.Count - 1].UniqueID + 1;
                }

                DatabasePassword newPassword = new DatabasePassword(
                    uniqueID,
                    password.UserGUID,
                    password.Application,
                    password.Username,
                    password.Email,
                    password.Description,
                    password.Website,
                    password.Passphrase);

                _encryptedPasswords.Add(newPassword);
                _csvPasswordManager.UpdatePasswordCSVFile(_passwordsCsvPath, _encryptedPasswords);

                result = true;
            }

            _lastUniqueId = uniqueID;

            return result;
        }

        /*************************************************************************************************/
        public bool ModifyPassword(DatabasePassword password, DatabasePassword modifiedPassword)
        {
            bool result = false;

            if (password != null && modifiedPassword != null)
            {
                int index = GetIndexOfPassword(password.UniqueID);

                if (index != -1)
                {
                    _encryptedPasswords[index] = modifiedPassword;
                    _csvPasswordManager.UpdatePasswordCSVFile(_passwordsCsvPath, _encryptedPasswords);
                    result = true;
                }
                else
                {
                    result = false;
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
                int index = GetIndexOfPassword(password.UniqueID);

                if (index != -1)
                {
                    _encryptedPasswords.RemoveAt(index);
                    _csvPasswordManager.UpdatePasswordCSVFile(_passwordsCsvPath, _encryptedPasswords);
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        /*************************************************************************************************/
        public List<DatabasePassword> GetUserPasswordsByGUID(string guid)
        {
            List<DatabasePassword> result = (from DatabasePassword password in _encryptedPasswords
                                             where password.UserGUID == guid
                                             select password).ToList<DatabasePassword>();
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
        private int GetIndexOfUser(string guid)
        {
            int index = -1;

            index = _encryptedUsers.FindIndex(x => x.GUID == guid);

            return index;
        }

        /*************************************************************************************************/
        private int GetIndexOfPassword(Int64 uniqueID)
        {
            int index = -1;

            index = _encryptedPasswords.IndexOf(_encryptedPasswords.Where(x => x.UniqueID == uniqueID).FirstOrDefault());

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
                var myFile = File.Create(_usersCsvPath);
                myFile.Close();
            }

            if (!File.Exists(_passwordsCsvPath))
            {
                var myFile = File.Create(_passwordsCsvPath);
                myFile.Close();
            }
        }

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // CSV CLASS
} // PasswordVault.Data NAMESPACE

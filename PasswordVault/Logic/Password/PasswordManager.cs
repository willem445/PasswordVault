using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*=================================================================================================
DESCRIPTION
*================================================================================================*/
/* 
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
    class PasswordManager : IPasswordManager
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
        private User _currentUser;                           // Current user's username and password
        private List<Password> _passwordList; // stores the current users passwords and binds to datagridview
        private IStorage _storage;                    // Method of storing the passwords (ie. csv file or database)
        private IMasterPassword _masterPassword;
        private IEncryptDecrypt _encryptDecrypt;
        private IMessageWriter _messageWriter;

        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public PasswordManager(IStorage storage, IMasterPassword masterPassword, IEncryptDecrypt encryptDecrypt, IMessageWriter messageWriter)
        {
            _storage = storage;
            _masterPassword = masterPassword;
            _encryptDecrypt = encryptDecrypt;
            _messageWriter = messageWriter;

            _currentUser = new User(false);
        }

        /*=================================================================================================
		PUBLIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        public void Login(string username, string password)
        {
            User user = _storage.GetUser(username);

            if (user == null)
            {
                _messageWriter.Show("Username does not exist.");
            }
            else
            {
                bool valid = _masterPassword.VerifyPassword(password, user.Salt, user.Hash); // Hash password with user.Salt and compare to user.Hash

                if (valid)
                {
                    _currentUser = new User(username, user.Salt, user.Hash, password, true);
                }
                else
                {
                    _messageWriter.Show("Password is incorrect.");
                    _currentUser = new User(false);
                }
            }

            if (_currentUser.ValidKey)
            {
                _storage.SetUserTableName(_currentUser.UserID);
                UpdatePasswordList();
            }
        }

        /*************************************************************************************************/
        public void CreateNewUser(string username, string password)
        {
            User user = _storage.GetUser(username);

            if (user != null)
            {
                _messageWriter.Show("Username is taken.");
            }
            else
            {
                CryptData_S newPassword = _masterPassword.HashPassword(password);
                _storage.AddUser(username, newPassword.Salt, newPassword.Hash);
            }
        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        private void UpdatePasswordList()
        {
            _passwordList.Clear();
            foreach (var item in _storage.GetPasswords())
            {
                Password password = new Password();
                password.Application = _encryptDecrypt.Decrypt(item.Application, _currentUser.Key);
                password.Username = _encryptDecrypt.Decrypt(item.Username, _currentUser.Key);
                password.Description = _encryptDecrypt.Decrypt(item.Description, _currentUser.Key);
                password.Website = _encryptDecrypt.Decrypt(item.Website, _currentUser.Key);
                password.Passphrase = item.Passphrase; // Leave pass phase encrypted until it is needed
                _passwordList.Add(password);
            }
        }

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // PasswordWrapper CLASS
} // PasswordHashTest NAMESPACE

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*=================================================================================================
DESCRIPTION
*================================================================================================*/
/* Serves as the bridge between the database and the presenters
 ------------------------------------------------------------------------------------------------*/

namespace PasswordVault
{
    /*=================================================================================================
	ENUMERATIONS
	*================================================================================================*/
    public enum LoginResult
    {
        PasswordIncorrect, 
        UsernameDoesNotExist,
        Successful,
        UnSuccessful
    }

    public enum CreateUserResult
    {
        UsernameTaken,
        UsernameNotValid,
        PasswordNotValid,
        Successful,
        Unsuccessful,
    }
    /*=================================================================================================
	STRUCTS
	*================================================================================================*/

    /*=================================================================================================
	CLASSES
	*================================================================================================*/
    class PasswordService : IPasswordService
    {
        /*=================================================================================================
		CONSTANTS
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/
        private const int DEFAULT_PASSWORD_LENGTH = 15;
        private const int MINIMUM_PASSWORD_LENGTH = 8;

        /*=================================================================================================
		FIELDS
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/
        private User _currentUser;                       // Current user's username and password
        private List<Password> _passwordList;            // stores the current users passwords and binds to datagridview
        private IDatabase _dbcontext;                    // Method of storing the passwords (ie. csv file or database)
        private IMasterPassword _masterPassword;
        private IEncryptDecrypt _encryptDecrypt;

        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public PasswordService(IDatabase dbcontext, IMasterPassword masterPassword, IEncryptDecrypt encryptDecrypt)
        {
            _dbcontext = dbcontext;
            _masterPassword = masterPassword;
            _encryptDecrypt = encryptDecrypt;

            _currentUser = new User(false);
            _passwordList = new List<Password>();
        }

        /*=================================================================================================
		PUBLIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        public LoginResult Login(string username, string password)
        {
            LoginResult loginResult = LoginResult.UnSuccessful;

            // Perform user login verification
            if (!_dbcontext.UserExists(username))
            {
                loginResult = LoginResult.UsernameDoesNotExist;
            }
            else
            {
                User user = _dbcontext.GetUser(username);

                bool valid = _masterPassword.VerifyPassword(password, user.Salt, user.Hash); // Hash password with user.Salt and compare to user.Hash

                if (valid)
                {
                    _currentUser = new User(username, user.Salt, user.Hash, password, true);
                }
                else
                {
                    loginResult = LoginResult.PasswordIncorrect;
                    _currentUser = new User(false);
                }
            }

            // Set table name and read passwords
            if (_currentUser.ValidKey)
            {
                loginResult = LoginResult.Successful;
                UpdatePasswordListFromDB();
            }

            return loginResult;
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        public bool IsLoggedIn()
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(string username)
        {
            throw new NotImplementedException();
        }

        public void ChangeUserPassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public void AddPassword(Password encryptedPassword)
        {
            _passwordList.Add(encryptedPassword);
            _dbcontext.AddPassword(ConvertToDatabasePassword(encryptedPassword));
        }

        public void RemovePassword(Password encryptedPassword)
        {
            _passwordList.Remove(encryptedPassword);
            _dbcontext.DeletePassword(ConvertToDatabasePassword(encryptedPassword));
        }

        public void ModifyPassword()
        {
            throw new NotImplementedException();
        }

        public List<Password> GetPasswords()
        {
            return _passwordList;
        }


        /*************************************************************************************************/
        public CreateUserResult CreateNewUser(string username, string password)
        {
            CreateUserResult createUserResult = CreateUserResult.Unsuccessful;
            User user = _dbcontext.GetUser(username);

            if (user != null)
            {
                createUserResult = CreateUserResult.UsernameTaken;
            }
            else
            {
                // Verify that username and password pass requirements
                if (username == null || username == "")
                {
                    createUserResult = CreateUserResult.UsernameNotValid;
                }
                else if (password == null || password == "" || password.Length < MINIMUM_PASSWORD_LENGTH) // TODO - 1- Check if password contains special characters
                {
                    createUserResult = CreateUserResult.PasswordNotValid;
                }
                else
                {
                    createUserResult = CreateUserResult.Successful;
                    CryptData_S newPassword = _masterPassword.HashPassword(password);
                    _dbcontext.AddUser(username, newPassword.Salt, newPassword.Hash);
                }
            }

            return createUserResult;
        }

        /*************************************************************************************************/
        public int GetMinimumPasswordLength()
        {
            return MINIMUM_PASSWORD_LENGTH;
        }

        /*************************************************************************************************/
        public string GeneratePasswordKey()
        {
            return _encryptDecrypt.CreateKey(DEFAULT_PASSWORD_LENGTH);
        }

        /*************************************************************************************************/
        public string GetMasterUserKey()
        {
            return _currentUser.Key;
        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        private void UpdatePasswordListFromDB()
        {
            _passwordList.Clear();
            foreach (var item in _dbcontext.GetUserPasswords(_currentUser.UserID))
            {
                // Add encrypted password to _passwordList
                Password password = new Password(
                    item.Application,
                    item.Username, 
                    item.Description, 
                    item.Website, 
                    item.Passphrase
                    );

                _passwordList.Add(password);
            }
        }

        /*************************************************************************************************/
        private DatabasePassword ConvertToDatabasePassword(Password encryptedPassword)
        {
            return new DatabasePassword(
                _currentUser.UserID, // TODO - Change to unique ID - Use unencrypted username for now
                encryptedPassword.Application,
                encryptedPassword.Username,
                encryptedPassword.Description,
                encryptedPassword.Website,
                encryptedPassword.Passphrase
                );
        }


        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // PasswordWrapper CLASS
} // PasswordHashTest NAMESPACE

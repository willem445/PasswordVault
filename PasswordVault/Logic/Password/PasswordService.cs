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
                UpdatePasswordList();
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

        public void AddPassword(Password unencryptedPassword)
        {
            // Encrypt password
            Password encryptedPassword = new Password(
                _encryptDecrypt.Encrypt(unencryptedPassword.Application, _currentUser.Key),
                _encryptDecrypt.Encrypt(unencryptedPassword.Username, _currentUser.Key),
                _encryptDecrypt.Encrypt(unencryptedPassword.Description, _currentUser.Key),
                _encryptDecrypt.Encrypt(unencryptedPassword.Website, _currentUser.Key),
                _encryptDecrypt.Encrypt(unencryptedPassword.Passphrase, _currentUser.Key)
                );

            // Add password to password list
            _passwordList.Add(new Password(unencryptedPassword.Application,
                                            unencryptedPassword.Username, 
                                            unencryptedPassword.Description, 
                                            unencryptedPassword.Website, 
                                            encryptedPassword.Passphrase));

            // Add encrypted password to database
            _dbcontext.AddPassword(new DatabasePassword(
                _encryptDecrypt.Encrypt(_currentUser.UserID, _currentUser.Key),
                encryptedPassword.Application,
                encryptedPassword.Username,
                encryptedPassword.Description,
                encryptedPassword.Website,
                encryptedPassword.Passphrase
                ));
        }

        public void RemovePassword()
        {
            throw new NotImplementedException();
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

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        private void UpdatePasswordList()
        {
            _passwordList.Clear();
            foreach (var item in _dbcontext.GetUserPasswords(_currentUser.UserID))
            {
                Password password = new Password(
                    _encryptDecrypt.Decrypt(item.Application, _currentUser.Key),
                    _encryptDecrypt.Decrypt(item.Username, _currentUser.Key),
                    _encryptDecrypt.Decrypt(item.Description, _currentUser.Key),
                    _encryptDecrypt.Decrypt(item.Website, _currentUser.Key),
                    item.Passphrase
                    );

                _passwordList.Add(password);
            }
        }


        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // PasswordWrapper CLASS
} // PasswordHashTest NAMESPACE

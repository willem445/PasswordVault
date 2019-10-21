using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;
using PasswordVault.Data;
using PasswordVault.Models;

/*=================================================================================================
DESCRIPTION
*================================================================================================*/
/* 
 ------------------------------------------------------------------------------------------------*/

namespace PasswordVault.Services
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
    public class PasswordService : IPasswordService
    {
        /*=================================================================================================
		CONSTANTS
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/
        private const int DEFAULT_PASSWORD_LENGTH = 20;
        private const int MINIMUM_PASSWORD_LENGTH = 12;
        private const int MAXIMUM_PASSWORD_LENGTH = 200;
        private const int MAXIMUM_PASSWORD_FIELD_LENGTH = 500;

        /*=================================================================================================
		FIELDS
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/
        private User _currentUser;                       // Current user's username and password
        private List<Password> _passwordList;            // stores the current users passwords and binds to datagridview
        private IDatabase _dbcontext;                    // Method of storing the passwords (ie. csv file or database)
        private IMasterPassword _masterPassword;
        private IEncryptionService _encryptDecrypt;

        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public PasswordService(IDatabase dbcontext, IMasterPassword masterPassword, IEncryptionService encryptDecrypt)
        {
            _dbcontext = dbcontext ?? throw new ArgumentNullException(nameof(dbcontext));
            _masterPassword = masterPassword ?? throw new ArgumentNullException(nameof(masterPassword));
            _encryptDecrypt = encryptDecrypt ?? throw new ArgumentNullException(nameof(encryptDecrypt));

            _currentUser = new User(false);
            _passwordList = new List<Password>();
        }

        /*=================================================================================================
		PUBLIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        public LoginResult Login(string username, string password)
        {
            LoginResult loginResult = LoginResult.Failed;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return LoginResult.Failed;
            }

            if (!IsLoggedIn())
            {
                // Perform user login verification
                if (!_dbcontext.UserExistsByUsername(username))
                {
                    loginResult = LoginResult.UsernameDoesNotExist;
                }
                else
                {
                    User user = _dbcontext.GetUserByUsername(username);

                    bool valid = _masterPassword.VerifyPassword(password, user.Salt, user.Hash, Convert.ToInt32(user.Iterations, CultureInfo.CurrentCulture)); // Hash password with user.Salt and compare to user.Hash

                    if (valid)
                    {
                        string tempKey = _encryptDecrypt.Decrypt(user.EncryptedKey, password);
                        _currentUser = new User(user.GUID,
                                                user.Username,
                                                tempKey,
                                                _encryptDecrypt.Decrypt(user.FirstName, tempKey),
                                                _encryptDecrypt.Decrypt(user.LastName, tempKey),
                                                _encryptDecrypt.Decrypt(user.PhoneNumber, tempKey),
                                                _encryptDecrypt.Decrypt(user.Email, tempKey),
                                                true);
                    }
                    else
                    {
                        loginResult = LoginResult.PasswordIncorrect;
                        _currentUser = new User(false);
                    }
                }

                // Set table name and read passwords
                if (_currentUser.ValidUser)
                {
                    loginResult = LoginResult.Successful;
                    UpdatePasswordListFromDB();
                }
            }

            return loginResult;
        }

        /*************************************************************************************************/
        public LogOutResult Logout()
        {
            LogOutResult result = LogOutResult.Failed;

            if (IsLoggedIn())
            {
                _passwordList.Clear();
                _currentUser = new User(false);

                result = LogOutResult.Success;
            }

            return result;
        }

        /*************************************************************************************************/
        public bool IsLoggedIn()
        {
            if (_currentUser.ValidUser)
            {
                return true;
            }

            return false;
        }

        /*************************************************************************************************/
        public CreateUserResult CreateNewUser(User user)
        {
            CreateUserResult createUserResult = CreateUserResult.Failed;

            if (user == null)
            {
                return CreateUserResult.Failed;
            }

            User queryResult = _dbcontext.GetUserByUsername(user.Username);

            if (queryResult != null)
            {
                createUserResult = CreateUserResult.UsernameTaken;
            }
            else
            {
                UserInformationResult verifyUser = VerifyUserInformation(user);
                ChangeUserPasswordResult verifyPassword = VerifyPasswordRequirements(user.PlainTextPassword);

                // Verify that username and password pass requirements
                if (!VerifyUsernameRequirements(user.Username))
                {
                    createUserResult = CreateUserResult.UsernameNotValid;
                }
                else if (verifyPassword != ChangeUserPasswordResult.Success)
                {
                    switch (verifyPassword)
                    {
                        case ChangeUserPasswordResult.Failed:
                            createUserResult = CreateUserResult.PasswordNotValid;
                            break;

                        case ChangeUserPasswordResult.LengthRequirementNotMet:
                            createUserResult = CreateUserResult.LengthRequirementNotMet;
                            break;

                        case ChangeUserPasswordResult.NoLowerCaseCharacter:
                            createUserResult = CreateUserResult.NoLowerCaseCharacter;
                            break;

                        case ChangeUserPasswordResult.NoNumber:
                            createUserResult = CreateUserResult.NoNumber;
                            break;

                        case ChangeUserPasswordResult.NoSpecialCharacter:
                            createUserResult = CreateUserResult.NoSpecialCharacter;
                            break;

                        case ChangeUserPasswordResult.NoUpperCaseCharacter:
                            createUserResult = CreateUserResult.NoUpperCaseCharacter;
                            break;

                        case ChangeUserPasswordResult.PasswordsDoNotMatch:
                            createUserResult = CreateUserResult.PasswordNotValid;
                            break;

                        default:
                            createUserResult = CreateUserResult.PasswordNotValid;
                            break;
                    }
                }
                else if (verifyUser != UserInformationResult.Success)
                {
                    switch (verifyUser)
                    {
                        case UserInformationResult.InvalidEmail:
                            createUserResult = CreateUserResult.EmailNotValid;
                            break;

                        case UserInformationResult.InvalidFirstName:
                            createUserResult = CreateUserResult.FirstNameNotValid;
                            break;

                        case UserInformationResult.InvalidLastName:
                            createUserResult = CreateUserResult.LastNameNotValid;
                            break;

                        case UserInformationResult.InvalidPhoneNumber:
                            createUserResult = CreateUserResult.PhoneNumberNotValid;
                            break;

                        case UserInformationResult.Failed:
                            createUserResult = CreateUserResult.Failed;
                            break;
                    }
                }
                else
                {
                    createUserResult = CreateUserResult.Successful;
                    UserEncrypedData newEncryptedData = _masterPassword.GenerateNewUserEncryptedDataFromPassword(user.PlainTextPassword);

                    User newUser = new User(
                        newEncryptedData.UniqueGUID, // Leave unique guid in plaintext
                        _encryptDecrypt.Encrypt(newEncryptedData.RandomGeneratedKey, user.PlainTextPassword), // Encrypt the random key with the users password
                        user.Username, // Leave username in plaintext
                        newEncryptedData.Iterations.ToString(CultureInfo.CurrentCulture), // Leave iterations in plaintext
                        newEncryptedData.Salt,
                        newEncryptedData.Hash,
                        _encryptDecrypt.Encrypt(user.FirstName, newEncryptedData.RandomGeneratedKey), // Encrypt with decrypted random key
                        _encryptDecrypt.Encrypt(user.LastName, newEncryptedData.RandomGeneratedKey), // Encrypt with decrypted random key
                        _encryptDecrypt.Encrypt(user.PhoneNumber, newEncryptedData.RandomGeneratedKey), // Encrypt with decrypted random key
                        _encryptDecrypt.Encrypt(user.Email, newEncryptedData.RandomGeneratedKey) // Encrypt with decrypted random key
                        );

                    _dbcontext.AddUser(newUser);
                }
            }

            return createUserResult;
        }

        /*************************************************************************************************/
        public DeleteUserResult DeleteUser(User user)
        {
            DeleteUserResult result = DeleteUserResult.Failed;

            if (user == null)
            {
                return DeleteUserResult.Failed;
            }

            User getUser = _dbcontext.GetUserByUsername(user.Username);

            if (getUser != null)
            {
                bool success = _dbcontext.DeleteUser(getUser);

                if (success)
                {
                    result = DeleteUserResult.Success;
                }
            }
            else
            {
                result = DeleteUserResult.Failed;
            }

            return result;
        }

        /*************************************************************************************************/
        public UserInformationResult EditUser(User user)
        {
            UserInformationResult result = UserInformationResult.Failed;

            if (user == null)
            {
                return UserInformationResult.Failed;
            }

            if (IsLoggedIn())
            {
                var validation = VerifyUserInformation(user);

                if (validation == UserInformationResult.Success)
                {
                    User dbUser = _dbcontext.GetUserByGUID(_currentUser.GUID);

                    User newCurrentUser = new User(
                        _currentUser.GUID,
                        _currentUser.Username,
                        _currentUser.PlainTextRandomKey,
                        user.FirstName,
                        user.LastName,
                        user.PhoneNumber,
                        user.Email,
                        true);

                    _currentUser = newCurrentUser;

                    User modifiedUser = new User
                    (
                        dbUser.GUID,
                        dbUser.EncryptedKey,
                        dbUser.Username,
                        dbUser.Iterations,
                        dbUser.Salt,
                        dbUser.Hash,
                        _encryptDecrypt.Encrypt(user.FirstName, _currentUser.PlainTextRandomKey),
                        _encryptDecrypt.Encrypt(user.LastName, _currentUser.PlainTextRandomKey),
                        _encryptDecrypt.Encrypt(user.PhoneNumber, _currentUser.PlainTextRandomKey),
                        _encryptDecrypt.Encrypt(user.Email, _currentUser.PlainTextRandomKey)
                    );

                    bool success = _dbcontext.ModifyUser(dbUser, modifiedUser);

                    if (success)
                    {
                        result = UserInformationResult.Success; // TODO - return other results
                    }
                    else
                    {
                        result = UserInformationResult.Failed;
                    }
                }
                else
                {
                    result = validation;
                }
            }

            return result;
        }

        /*************************************************************************************************/
        public string GetCurrentUsername()
        {
            string user = "";

            if (IsLoggedIn())
            {
                user = _currentUser.Username;
            }

            return user;
        }

        /*************************************************************************************************/
        public User GetCurrentUser()
        {
            User user = new User();

            if (IsLoggedIn())
            {
                user = _currentUser;
            }

            return user;
        }

        /*************************************************************************************************/
        public ChangeUserPasswordResult ChangeUserPassword(string originalPassword, string newPassword, string confirmPassword)
        {
            ChangeUserPasswordResult result = ChangeUserPasswordResult.Failed;

            if (string.IsNullOrEmpty(originalPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                return ChangeUserPasswordResult.Failed;
            }

            if (IsLoggedIn())
            {
                if (newPassword == confirmPassword)
                {
                    bool validPassword = VerifyCurrentUserPassword(originalPassword);

                    if (validPassword)
                    {
                        ChangeUserPasswordResult verifyPass = VerifyPasswordRequirements(newPassword);
                        if (verifyPass != ChangeUserPasswordResult.Success)
                        {
                            result = verifyPass;
                        }
                        else
                        {
                            User user = _dbcontext.GetUserByGUID(_currentUser.GUID);
                            UserEncrypedData newEncryptedData = _masterPassword.GenerateNewUserEncryptedDataFromPassword(newPassword);

                            User newUser = new User(
                                user.GUID,
                                _encryptDecrypt.Encrypt(_currentUser.PlainTextRandomKey, newPassword), // Encrypt the random key with the users password
                                user.Username,
                                newEncryptedData.Iterations.ToString(CultureInfo.CurrentCulture),
                                newEncryptedData.Salt,
                                newEncryptedData.Hash,
                                user.FirstName,
                                user.LastName,
                                user.PhoneNumber,
                                user.Email
                            );

                            if (_dbcontext.ModifyUser(user, newUser))
                            {
                                result = ChangeUserPasswordResult.Success;
                            }
                            else
                            {
                                result = ChangeUserPasswordResult.Failed;
                            }
                        }
                    }
                    else
                    {
                        result = ChangeUserPasswordResult.InvalidPassword;
                    }
                }
                else
                {
                    result = ChangeUserPasswordResult.PasswordsDoNotMatch;
                }
            }

            return result;
        }

        /*************************************************************************************************/
        public bool VerifyCurrentUserPassword(string password)
        {
            bool result = false;

            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            if (IsLoggedIn())
            {
                User user = _dbcontext.GetUserByGUID(_currentUser.GUID);

                if (user != null)
                {
                    result = _masterPassword.VerifyPassword(password,
                                                        user.Salt, user.Hash,
                                                        Convert.ToInt32(user.Iterations, CultureInfo.CurrentCulture));
                }
            }

            return result;
        }

        /*************************************************************************************************/
        public AddPasswordResult AddPassword(Password password)
        {
            AddPasswordResult addResult = AddPasswordResult.Failed;

            if (password == null)
            {
                return AddPasswordResult.Failed;
            }

            if (IsLoggedIn())
            {
                AddPasswordResult verifyResult = VerifyAddPasswordFields(password);

                if (verifyResult == AddPasswordResult.Success)
                {
                    List<Password> queryResult = (from Password pass in _passwordList
                                                  where pass.Application == password.Application
                                                  select pass).ToList<Password>();

                    if (queryResult.Count <= 0) // Verify that new password is not a duplicate of an existing one
                    {
                        Password encryptPassword = ConvertPlaintextPasswordToEncryptedPassword(password); // Need to first encrypt the password
                        Int64 uniqueID = _dbcontext.AddPassword(ConvertToEncryptedDatabasePassword(encryptPassword)); // Add the encrypted password to the database

                        // Update the passwordservice list.
                        // This solves issue when deleting a newly added password where the unique ID hasn't been updated in the service.
                        // since the database autoincrements the unique ID.
                        UpdatePasswordListFromDB(encryptPassword, uniqueID);

                        addResult = AddPasswordResult.Success;
                    }
                    else
                    {
                        addResult = AddPasswordResult.DuplicatePassword;
                    }
                }
                else
                {
                    addResult = verifyResult;
                }
            }

            return addResult;
        }

        /*************************************************************************************************/
        public DeletePasswordResult DeletePassword(Password password)
        {
            DeletePasswordResult result = DeletePasswordResult.Failed;

            if (password == null)
            {
                return DeletePasswordResult.Failed;
            }

            if (IsLoggedIn())
            {
                Password queryResult = (from Password pass in _passwordList
                                        where pass.Application == password.Application
                                        where pass.Username == password.Username
                                        where pass.Description == password.Description
                                        where pass.Website == password.Website
                                        select pass).FirstOrDefault();

                if (queryResult != null)
                {
                    _passwordList.Remove(queryResult);
                    _dbcontext.DeletePassword(ConvertToEncryptedDatabasePassword(queryResult));
                    result = DeletePasswordResult.Success;
                }
                else
                {
                    result = DeletePasswordResult.PasswordDoesNotExist;
                }
            }

            return result;
        }

        /*************************************************************************************************/
        public AddPasswordResult ModifyPassword(Password originalPassword, Password modifiedPassword)
        {
            AddPasswordResult result = AddPasswordResult.Failed;

            if (originalPassword == null || modifiedPassword == null)
            {
                return AddPasswordResult.Failed;
            }

            if (IsLoggedIn())
            {
                AddPasswordResult verifyResult = VerifyAddPasswordFields(modifiedPassword);

                if (verifyResult == AddPasswordResult.Success)
                {
                    List<Password> modifiedPasswordResult = (from Password pass in _passwordList
                                                             where pass.Application == modifiedPassword.Application
                                                             select pass).ToList<Password>();

                    if ((modifiedPasswordResult.Count <= 0) || // verify that another password doesn't have the same application name
                        (modifiedPassword.Application == originalPassword.Application)) // if the application name of the original and modified match, continue as this is not actually a duplicate
                    {
                        int index = _passwordList.FindIndex(x => (x.Application == originalPassword.Application) &&
                                                                    (x.Username == originalPassword.Username) && (x.Description == originalPassword.Description) && (x.Website == originalPassword.Website));

                        if (index != -1)
                        {
                            Password modifiedWithUniqueID = new Password(_passwordList[index].UniqueID, modifiedPassword.Application, modifiedPassword.Username, modifiedPassword.Email, modifiedPassword.Description, modifiedPassword.Website, modifiedPassword.Passphrase);
                            Password originalWithUniqueID = new Password(_passwordList[index].UniqueID, originalPassword.Application, originalPassword.Username, originalPassword.Email, originalPassword.Description, originalPassword.Website, originalPassword.Passphrase);
                            Password modifiedEncryptedPassword = ConvertPlaintextPasswordToEncryptedPassword(modifiedWithUniqueID);

                            _passwordList[index] = modifiedEncryptedPassword;
                            _dbcontext.ModifyPassword(ConvertToEncryptedDatabasePassword(originalWithUniqueID), ConvertToEncryptedDatabasePassword(modifiedEncryptedPassword));
                            result = AddPasswordResult.Success;
                        }
                    }
                    else
                    {
                        result = AddPasswordResult.DuplicatePassword;
                    }
                }
                else
                {
                    result = verifyResult;
                }
            }

            return result;
        }

        /*************************************************************************************************/
        public List<Password> GetPasswords()
        {
            return _passwordList;
        }

        /*************************************************************************************************/
        public Password DecryptPassword(Password password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            Password decryptedPassword;

            decryptedPassword = DecryptEncryptedPassword(password);

            return decryptedPassword;
        }

        /*************************************************************************************************/
        public int GetMinimumPasswordLength()
        {
            return MINIMUM_PASSWORD_LENGTH;
        }

        /*************************************************************************************************/
        public string GeneratePasswordKey()
        {
            return _encryptDecrypt.CreateKey(DEFAULT_PASSWORD_LENGTH).Trim('=');
        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        private void UpdatePasswordListFromDB()
        {
            _passwordList.Clear();
            foreach (var item in _dbcontext.GetUserPasswordsByGUID(_currentUser.GUID))
            {
                // Add encrypted password to _passwordList
                Password password = new Password(
                    item.UniqueID,
                    _encryptDecrypt.Decrypt(item.Application, _currentUser.PlainTextRandomKey),
                    _encryptDecrypt.Decrypt(item.Username, _currentUser.PlainTextRandomKey),
                    _encryptDecrypt.Decrypt(item.Email, _currentUser.PlainTextRandomKey),
                    _encryptDecrypt.Decrypt(item.Description, _currentUser.PlainTextRandomKey),
                    _encryptDecrypt.Decrypt(item.Website, _currentUser.PlainTextRandomKey),
                    item.Passphrase // Leave the password encrypted
                    );

                _passwordList.Add(password);
            }
        }

        /*************************************************************************************************/
        private void UpdatePasswordListFromDB(Password password, Int64 uniqueID)
        {
            Password newPassword = new Password
            (
                uniqueID,
                password.Application,
                password.Username,
                password.Email,
                password.Description,
                password.Website,
                password.Passphrase
            );

            _passwordList.Add(newPassword);
        }

        /*************************************************************************************************/
        private DatabasePassword ConvertToEncryptedDatabasePassword(Password password)
        {
            return new DatabasePassword(
                password.UniqueID,
                _currentUser.GUID, // TODO - 7 - Change to unique ID - Use unencrypted username for now
                _encryptDecrypt.Encrypt(password.Application, _currentUser.PlainTextRandomKey),
                _encryptDecrypt.Encrypt(password.Username, _currentUser.PlainTextRandomKey),
                _encryptDecrypt.Encrypt(password.Email, _currentUser.PlainTextRandomKey),
                _encryptDecrypt.Encrypt(password.Description, _currentUser.PlainTextRandomKey),
                _encryptDecrypt.Encrypt(password.Website, _currentUser.PlainTextRandomKey),
                password.Passphrase // Password is already encrypted
                );
        }

        /*************************************************************************************************/
        private Password ConvertPlaintextPasswordToEncryptedPassword(Password password)
        {
            return new Password(
                password.UniqueID,
                password.Application,
                password.Username,
                password.Email,
                password.Description,
                password.Website,
                _encryptDecrypt.Encrypt(password.Passphrase, _currentUser.PlainTextRandomKey)
                );
        }

        /*************************************************************************************************/
        private Password DecryptEncryptedPassword(Password password)
        {
            return new Password(
                password.UniqueID,
                password.Application,
                password.Username,
                password.Email,
                password.Description,
                password.Website,
                _encryptDecrypt.Decrypt(password.Passphrase, _currentUser.PlainTextRandomKey));
        }

        /*************************************************************************************************/
        private bool VerifyUsernameRequirements(string username)
        {
            bool result = true;

            if (string.IsNullOrEmpty(username))
            {
                result = false;
            }

            return result;
        }

        /*************************************************************************************************/
        private UserInformationResult VerifyUserInformation(User user)
        {
            UserInformationResult result = UserInformationResult.Success;

            if (user != null)
            {
                if (string.IsNullOrEmpty(user.FirstName))
                {
                    result = UserInformationResult.InvalidFirstName;
                }

                if (string.IsNullOrEmpty(user.LastName))
                {
                    result = UserInformationResult.InvalidLastName;
                }

                if (string.IsNullOrEmpty(user.Email) || user.Email == "example@provider.com")
                {
                    result = UserInformationResult.InvalidEmail;
                }
                else
                {
                    var regex = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

                    Match match = Regex.Match(user.Email, regex, RegexOptions.IgnoreCase);

                    if (!match.Success)
                    {
                        result = UserInformationResult.InvalidEmail;
                    }
                }

                if (string.IsNullOrEmpty(user.PhoneNumber) || user.PhoneNumber == "xxx-xxx-xxxx")
                {
                    result = UserInformationResult.InvalidPhoneNumber;
                }
                else
                {
                    if (!IsValidUSPhoneNumber(user.PhoneNumber))
                    {
                        result = UserInformationResult.InvalidPhoneNumber;
                    }
                }
            }

            return result;
        }

        /*************************************************************************************************/
        private ChangeUserPasswordResult VerifyPasswordRequirements(string passphrase)
        {
            ChangeUserPasswordResult result = ChangeUserPasswordResult.Success;

            bool containsNumber = false;
            bool containsLowerCase = false;
            bool containsUpperCase = false;

            if (string.IsNullOrEmpty(passphrase))
            {
                return ChangeUserPasswordResult.Failed;
            }

            if (passphrase.Length >= MAXIMUM_PASSWORD_LENGTH)
            {
                result = ChangeUserPasswordResult.Failed;
            }

            if (passphrase.Length <= MINIMUM_PASSWORD_LENGTH)
            {
                result = ChangeUserPasswordResult.LengthRequirementNotMet;
            }

            foreach (var character in passphrase)
            {
                if (char.IsUpper(character))
                {
                    containsUpperCase = true;
                }
                else if (char.IsLower(character))
                {
                    containsLowerCase = true;
                }
                else if (char.IsDigit(character))
                {
                    containsNumber = true;
                }
            }

            if (!containsLowerCase)
            {
                result = ChangeUserPasswordResult.NoLowerCaseCharacter;
            }

            if (!containsUpperCase)
            {
                result = ChangeUserPasswordResult.NoUpperCaseCharacter;
            }

            if (!containsNumber)
            {
                result = ChangeUserPasswordResult.NoNumber;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(passphrase, @"[!@#$%^&*()_+=\[{\]};:<>|./?,-]"))
            {
                result = ChangeUserPasswordResult.NoSpecialCharacter;
            }

            return result;
        }

        /*************************************************************************************************/
        private AddPasswordResult VerifyAddPasswordFields(Password password)
        {
            AddPasswordResult result = AddPasswordResult.Success;

            if (password != null)
            {
                if (string.IsNullOrEmpty(password.Passphrase) || password.Passphrase.Length > MAXIMUM_PASSWORD_FIELD_LENGTH)
                {
                    result = AddPasswordResult.PassphraseError;
                }

                if (string.IsNullOrEmpty(password.Username) || password.Username.Length > MAXIMUM_PASSWORD_FIELD_LENGTH)
                {
                    result = AddPasswordResult.UsernameError;
                }

                if (string.IsNullOrEmpty(password.Application) || password.Application.Length > MAXIMUM_PASSWORD_FIELD_LENGTH)
                {
                    result = AddPasswordResult.ApplicationError;
                }

                if (password.Description == null || password.Description.Length > MAXIMUM_PASSWORD_FIELD_LENGTH)
                {
                    result = AddPasswordResult.DescriptionError;
                }

                if (password.Website == null || password.Website.Length > MAXIMUM_PASSWORD_FIELD_LENGTH)
                {
                    result = AddPasswordResult.WebsiteError;
                }
                else if (password.Website.Length != 0)
                {
                    if (!UriUtilities.IsValidUri(password.Website))
                    {
                        result = AddPasswordResult.WebsiteError;
                    }
                }

                if (password.Email == null || password.Email.Length > MAXIMUM_PASSWORD_FIELD_LENGTH)
                {
                    result = AddPasswordResult.EmailError;
                }
                else if (password.Email.Length != 0)
                {
                    if (!password.Email.Contains("@") || !password.Email.Contains("."))
                    {
                        result = AddPasswordResult.EmailError;
                    }
                }
            }

            return result;
        }

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        /// <summary>
        /// Allows phone number of the format: NPA = [2-9][0-8][0-9] Nxx = [2-9]      [0-9][0-9] Station = [0-9][0-9][0-9][0-9]
        /// </summary>
        /// <param name="strPhone"></param>
        /// <returns></returns>
        public static bool IsValidUSPhoneNumber(string strPhone)
        {
            string regExPattern = @"^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$";
            return MatchStringFromRegex(strPhone, regExPattern);
        }

        /*************************************************************************************************/
        public static bool MatchStringFromRegex(string str, string regexstr)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            str = str.Trim();
            System.Text.RegularExpressions.Regex pattern = new System.Text.RegularExpressions.Regex(regexstr);
            return pattern.IsMatch(str);
        }

    } // PasswordService CLASS
} // PasswordVault.Services.Standard NAMESPACE

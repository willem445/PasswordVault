using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordVault.Models;
using PasswordVault.Services;

namespace PasswordVault.Desktop.Winforms
{
    public enum LogOutResult
    {
        Success,
        Failed
    }

    public class DesktopServiceWrapper : IDesktopServiceWrapper
    {
        /*CONSTANTS********************************************************/
        private const EncryptionService ENCRYPTION_SERVICE_DEFAULT = EncryptionService.Aes;
        private const int GENERATED_PASSWORD_LENGTH = 20;

        /*FIELDS***********************************************************/
        private User _currentUser;                       // Current user's username and password
        private EncryptionServiceParameters _encryptionParameters;
        private List<Password> _passwordList;            // stores the current users passwords and binds to datagridview

        private IPasswordService _passwordService;
        private IUserService _userService;
        private IAuthenticationService _authenticationService;
        private IExportPasswords _exportPasswords;
        private IEncryptionConversionFactory _encryptionConversionFactory;

        /*PROPERTIES*******************************************************/

        /*CONSTRUCTORS*****************************************************/
        public DesktopServiceWrapper(
            IAuthenticationService authenticationService, 
            IPasswordService passwordService, 
            IUserService userService, 
            IExportPasswords exportPasswords, 
            IEncryptionConversionFactory encryptionConversionFactory)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
            _passwordService = passwordService ?? throw new ArgumentNullException(nameof(passwordService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _exportPasswords = exportPasswords ?? throw new ArgumentNullException(nameof(exportPasswords));
            _encryptionConversionFactory = encryptionConversionFactory ?? throw new ArgumentNullException(nameof(encryptionConversionFactory));

            _currentUser = new User(false);
            _passwordList = new List<Password>();
        }

        /*PUBLIC METHODS***************************************************/    
        public AuthenticateResult Login(string username, string password)
        {
            AuthenticateResult loginResult = AuthenticateResult.Failed;

            if (!IsLoggedIn())
            {
                User tempUser = _userService.GetUserByUsername(username);

                if (tempUser != null)
                {
                    // TODO - 9 - Need to handle if DB has null values
                    // TODO - 9 - Compare parameters to application default parameters to determine if a conversion needs to take place
                    _encryptionParameters = new EncryptionServiceParameters(
                        (EncryptionService)tempUser.PasswordEncryptionService.Value,
                        new EncryptionSizes(
                        tempUser.PasswordIterations.Value,
                        tempUser.PasswordBlockSize.Value,
                        tempUser.PasswordKeySize.Value));

                    AuthenticateReturn authenticateResult = _authenticationService.Authenticate(username, password, _encryptionParameters);

                    if (authenticateResult.Result == AuthenticateResult.Successful)
                    {
                        _currentUser = authenticateResult.User;

                        EncryptionSizes encryptionDefaults = new EncryptionServiceFactory().Get(_encryptionParameters).EncryptionSizeDefaults;

                        if (_encryptionParameters.EncryptionService != ENCRYPTION_SERVICE_DEFAULT)
                        {
                            IEncryptionConversion encryptionConversion = _encryptionConversionFactory.Get(_encryptionParameters.EncryptionService, ENCRYPTION_SERVICE_DEFAULT, _passwordService, _userService);
                            encryptionDefaults = new EncryptionServiceFactory().Get(new EncryptionServiceParameters(ENCRYPTION_SERVICE_DEFAULT, new EncryptionSizes())).EncryptionSizeDefaults;
                            encryptionConversion.Convert(_currentUser, password, _encryptionParameters.EncryptionSizes, encryptionDefaults);
                            _encryptionParameters = new EncryptionServiceParameters(ENCRYPTION_SERVICE_DEFAULT, encryptionDefaults);
                        }
                        else if (_encryptionParameters.EncryptionSizes.BlockSize != encryptionDefaults.BlockSize ||
                            _encryptionParameters.EncryptionSizes.KeySize != encryptionDefaults.KeySize ||
                            _encryptionParameters.EncryptionSizes.Iterations != encryptionDefaults.Iterations)
                        {
                            // Need to update old encrypted data to use latest defaults
                        }

                        UpdatePasswordListFromDB();                      
                    }
                    else
                    {
                        _currentUser = new User(false);
                        _encryptionParameters = new EncryptionServiceParameters();
                    }

                    loginResult = authenticateResult.Result;
                }            
            }
            
            return loginResult;
        }

        public bool VerifyCurrentUserPassword(string password)
        {
            bool result = false;

            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            if (IsLoggedIn())
            {
                result = _authenticationService.VerifyUserCredentials(_currentUser.Username, password);
            }

            return result;
        }

        public LogOutResult Logout()
        {
            LogOutResult result = LogOutResult.Failed;

            if (IsLoggedIn())
            {
                _passwordList.Clear();
                _currentUser = new User(false);
                _encryptionParameters = new EncryptionServiceParameters();

                result = LogOutResult.Success;
            }

            return result;
        }
  
        public bool IsLoggedIn()
        {
            if (_currentUser.ValidUser)
            {
                return true;
            }

            return false;
        }

        public AddUserResult CreateNewUser(User user)
        {
            AddUserResult result = _userService.AddUser(user, _encryptionParameters);
            return result;
        }

        public ValidateUserPasswordResult ChangeUserPassword(string originalPassword, string newPassword, string confirmPassword)
        {
            ValidateUserPasswordResult result = ValidateUserPasswordResult.Failed;

            if (IsLoggedIn())
            {
                result = _userService.ChangeUserPassword(_currentUser.GUID, originalPassword, newPassword, confirmPassword, _currentUser.PlainTextRandomKey, _encryptionParameters);
            }

            return result;
        }

        public DeleteUserResult DeleteUser(User user)
        {
            DeleteUserResult result = DeleteUserResult.Failed;

            if (user != null)
            {
                // presenter handles logging the user out
                result = _userService.DeleteUser(user.Username);
            }
                         
            return result;
        }

        public UserInformationResult EditUser(User user)
        {
            UserInformationResult result = UserInformationResult.Failed;

            if (IsLoggedIn())
            {
                if (user != null &&
                !string.IsNullOrEmpty(_currentUser.GUID) &&
                !string.IsNullOrEmpty(_currentUser.PlainTextRandomKey))
                {
                    result = _userService.ModifyUser(_currentUser.GUID, user, _currentUser.PlainTextRandomKey, _encryptionParameters);

                    if (result == UserInformationResult.Success)
                    {
                        _currentUser = new User(
                            _currentUser.GUID,
                            _currentUser.Username,
                            _currentUser.PlainTextRandomKey,
                            user.FirstName,
                            user.LastName,
                            user.PhoneNumber,
                            user.Email,
                            true);
                    }
                }
            }         
            
            return result;
        }

        public User GetCurrentUser()
        {
            User user = new User();

            if (IsLoggedIn())
            {
                user = _currentUser;
            }

            return user;
        }

        public string GetCurrentUsername()
        {
            string user = "";

            if (IsLoggedIn())
            {
                user = _currentUser.Username;
            }

            return user;
        }

        public AddModifyPasswordResult AddPassword(Password password)
        {
            AddModifyPasswordResult result = AddModifyPasswordResult.Failed;

            if (password == null)
            {
                return AddModifyPasswordResult.Failed;
            }

            if (IsLoggedIn())
            {
                List<Password> queryResult = (from Password pass in _passwordList
                                              where pass.Application == password.Application
                                              select pass).ToList<Password>();

                if (queryResult.Count <= 0)
                {
                    AddPasswordResult addResult = _passwordService.AddPassword(_currentUser.GUID, password, _currentUser.PlainTextRandomKey, _encryptionParameters);

                    if (addResult.Result == AddModifyPasswordResult.Success)
                    {
                        UpdatePasswordListFromDB(password, addResult.UniquePasswordID);
                        result = AddModifyPasswordResult.Success;
                    }
                    else
                    {
                        result = addResult.Result;
                    }
                }
                else
                {
                    result = AddModifyPasswordResult.DuplicatePassword;
                }
            }

            return result;
        }

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
                    result = _passwordService.DeletePassword(queryResult.UniqueID);
                }
                else
                {
                    result = DeletePasswordResult.PasswordDoesNotExist;
                }
                    
            }

            return result;
        }

        public AddModifyPasswordResult ModifyPassword(Password originalPassword, Password modifiedPassword)
        {
            AddModifyPasswordResult result = AddModifyPasswordResult.Failed;

            if (originalPassword == null || modifiedPassword == null)
            {
                return AddModifyPasswordResult.Failed;
            }

            if (IsLoggedIn())
            {
                List<Password> modifiedPasswordResult = (from Password pass in _passwordList
                                                         where pass.Application == modifiedPassword.Application
                                                         select pass).ToList<Password>();

                if ((modifiedPasswordResult.Count <= 0) || // verify that another password doesn't have the same application name
                        (modifiedPassword.Application == originalPassword.Application)) // if the application name of the original and modified match, continue as this is not actually a duplicate
                {
                    int index = _passwordList.FindIndex(x => (x.Application == originalPassword.Application) &&
                                                             (x.Username == originalPassword.Username) && 
                                                             (x.Description == originalPassword.Description) && 
                                                             (x.Website == originalPassword.Website));

                    if (index != -1)
                    {
                        Password modifiedWithUniqueID = new Password(_passwordList[index].UniqueID, 
                                                                     modifiedPassword.Application,
                                                                     modifiedPassword.Username, 
                                                                     modifiedPassword.Email,
                                                                     modifiedPassword.Description, 
                                                                     modifiedPassword.Website, 
                                                                     modifiedPassword.Passphrase);

                        AddModifyPasswordResult addResult = _passwordService.ModifyPassword(_currentUser.GUID, modifiedWithUniqueID, _currentUser.PlainTextRandomKey, _encryptionParameters);

                        if (addResult == AddModifyPasswordResult.Success)
                        {
                            _passwordList[index] = modifiedWithUniqueID;
                            result = AddModifyPasswordResult.Success;
                        }
                        else
                        {
                            result = addResult;
                        }                    
                    }
                    else
                    {
                        result = AddModifyPasswordResult.Failed;
                    }
                }
                else
                {
                    result = AddModifyPasswordResult.DuplicatePassword;
                }               
            }

            return result;
        }

        public List<Password> GetPasswords()
        {
            if (IsLoggedIn())
            {
                return _passwordList;
            }

            return null;
        }

        public string GeneratePasswordKey()
        {
            string result = "";

            result = _passwordService.GeneratePasswordKey(GENERATED_PASSWORD_LENGTH);

            return result;
        }

        public int GetMinimumPasswordLength()
        {
            int result = _userService.GetMinimumPasswordLength();
            return result;
        }

        public int GetPasswordCount()
        {
            if (IsLoggedIn())
            {
                return _passwordList.Count;
            }

            return -1;
        }

        public ExportResult ExportPasswords(ExportFileTypes fileType, string exportPath, string passwordProtection, bool passwordEnabled)
        {
            ExportResult result;

            result = _exportPasswords.Export(fileType, exportPath, _passwordList, passwordProtection, passwordEnabled);

            return result;
        }

        public List<SupportedFileTypes> GetSupportedFileTypes()
        {
            return _exportPasswords.GetSupportedFileTypes();
        }

        /*PRIVATE METHODS**************************************************/
        private void UpdatePasswordListFromDB()
        {
            _passwordList.Clear();
            _passwordList = _passwordService.GetPasswords(_currentUser.GUID, _currentUser.PlainTextRandomKey, _encryptionParameters);
        }

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

        /*STATIC METHODS***************************************************/

    } // DesktopServiceWrapper CLASS
}

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
        private const int GENERATED_PASSWORD_LENGTH = 20;

        /*FIELDS***********************************************************/
        private User _currentUser;                       // Current user's username and password
        private AppSettings _settings = AppSettings.Instance;
        private List<Password> _passwordList;            // stores the current users passwords and binds to datagridview

        private IPasswordService _passwordService;
        private IUserService _userService;
        private IAuthenticationService _authenticationService;
        private IImportExportPasswords _importExportPasswords;

        public event Action<AuthenticateResult> AuthenticationResultEvent;
        public event Action DoneLoadingPasswordsEvent;

        /*PROPERTIES*******************************************************/

        /*CONSTRUCTORS*****************************************************/
        public DesktopServiceWrapper(
            IAuthenticationService authenticationService, 
            IPasswordService passwordService, 
            IUserService userService, 
            IImportExportPasswords exportPasswords)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
            _passwordService = passwordService ?? throw new ArgumentNullException(nameof(passwordService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _importExportPasswords = exportPasswords ?? throw new ArgumentNullException(nameof(exportPasswords));

            _currentUser = new User(false);
            _passwordList = new List<Password>();
        }

        /*PUBLIC METHODS***************************************************/    
        public AuthenticateResult Login(string username, string password)
        {
            AuthenticateResult loginResult = AuthenticateResult.Failed;

            if (!IsLoggedIn())
            {
                AuthenticateReturn authenticateResult = _authenticationService.Authenticate(username, password, _settings.DefaultEncryptionParameters);
                AuthenticationResultEvent?.Invoke(authenticateResult.Result);

                if (authenticateResult.Result == AuthenticateResult.Successful)
                {
                    _currentUser = authenticateResult.User;

                    var t = Task.Run(() => UpdatePasswordListFromDB());
                    t.Wait();
                    DoneLoadingPasswordsEvent?.Invoke();
                }
                else
                {
                    // If user credentials are incorrect, clear user and parameters from memory
                    _currentUser = new User(false);
                }
                loginResult = authenticateResult.Result;
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
            AddUserResult result = _userService.AddUser(user, _settings.DefaultEncryptionParameters, _settings.DefaultMasterPasswordParameters);
            return result;
        }

        public ValidateUserPasswordResult ChangeUserPassword(string originalPassword, string newPassword, string confirmPassword)
        {
            ValidateUserPasswordResult result = ValidateUserPasswordResult.Failed;

            if (IsLoggedIn())
            {
                result = _userService.ChangeUserPassword(
                    _currentUser.Uuid, 
                    originalPassword, 
                    newPassword, 
                    confirmPassword, 
                    _currentUser.PlainTextRandomKey, 
                    _settings.DefaultEncryptionParameters, 
                    _settings.DefaultMasterPasswordParameters);
            }

            return result;
        }

        public DeleteUserResult DeleteUser(User user, int expectedPasswordCount)
        {
            DeleteUserResult result = DeleteUserResult.Failed;

            if (user != null)
            {
                // presenter handles logging the user out
                result = _userService.DeleteUser(user.Username, expectedPasswordCount);
            }
                         
            return result;
        }

        public UserInformationResult EditUser(User user)
        {
            UserInformationResult result = UserInformationResult.Failed;

            if (IsLoggedIn())
            {
                if (user != null &&
                !string.IsNullOrEmpty(_currentUser.Uuid) &&
                !string.IsNullOrEmpty(_currentUser.PlainTextRandomKey))
                {
                    result = _userService.ModifyUser(_currentUser.Uuid, user, _currentUser.PlainTextRandomKey, _settings.DefaultEncryptionParameters);

                    if (result == UserInformationResult.Success)
                    {
                        _currentUser = new User(
                            _currentUser.Uuid,
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
                    AddPasswordResult addResult = _passwordService.AddPassword(_currentUser.Uuid, password, _currentUser.PlainTextRandomKey, _settings.DefaultEncryptionParameters);

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

                        AddModifyPasswordResult addResult = _passwordService.ModifyPassword(_currentUser.Uuid, modifiedWithUniqueID, _currentUser.PlainTextRandomKey, _settings.DefaultEncryptionParameters);

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

        public ImportExportResult ExportPasswords(ImportExportFileType fileType, string exportPath, string passwordProtection, bool passwordEnabled)
        {
            ImportExportResult result;

            result = _importExportPasswords.Export(fileType, exportPath, _passwordList, passwordProtection, passwordEnabled);

            return result;
        }

        public ImportExportResult ImportPasswords(ImportExportFileType fileType, string importPath, string passphrase, bool passwordEnabled)
        {
            ImportExportResult result = ImportExportResult.Fail;
            List<Password> passwords;

            ImportResult importresult = _importExportPasswords.Import(fileType, importPath, passphrase, passwordEnabled);

            if (importresult.Result != ImportExportResult.Success)
            {
                return importresult.Result;
            }
            else
            {
                passwords = importresult.Passwords;
            }

            if (passwords == null || passwords.Count == 0)
            {
                result = ImportExportResult.Fail;
            }
            else
            {
                result = ImportExportResult.Success;

                foreach (var password in passwords)
                {
                    // check if password exists
                    int index = _passwordList.FindIndex(x => (x.Application == password.Application));
                    if (index == -1)
                    {
                        var addresult = AddPassword(password);

                        if (addresult != AddModifyPasswordResult.Success)
                        {
                            result = ImportExportResult.Fail;
                            break; // TODO - On failure, need to report which password had an issue
                        }
                    }
                }              
            }

            return result;
        }

        public List<SupportedFileTypes> GetSupportedFileTypes()
        {
            return _importExportPasswords.GetSupportedFileTypes();
        }

        /*PRIVATE METHODS**************************************************/
        private void UpdatePasswordListFromDB()
        {
            _passwordList.Clear();
            _passwordList = _passwordService.GetPasswords(_currentUser.Uuid, _currentUser.PlainTextRandomKey, _settings.DefaultEncryptionParameters);         
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

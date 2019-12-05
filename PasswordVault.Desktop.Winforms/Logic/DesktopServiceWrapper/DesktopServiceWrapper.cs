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
        private List<Password> _passwordList;            // stores the current users passwords and binds to datagridview

        private IPasswordService _passwordService;
        private IUserService _userService;
        private IAuthenticationService _authenticationService;
        private IExportPasswords _exportPasswords;

        /*PROPERTIES*******************************************************/

        /*CONSTRUCTORS*****************************************************/
        public DesktopServiceWrapper(IAuthenticationService authenticationService, IPasswordService passwordService, IUserService userService, IExportPasswords exportPasswords)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
            _passwordService = passwordService ?? throw new ArgumentNullException(nameof(passwordService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _exportPasswords = exportPasswords ?? throw new ArgumentNullException(nameof(exportPasswords));

            _currentUser = new User(false);
            _passwordList = new List<Password>();
        }

        /*PUBLIC METHODS***************************************************/    
        public AuthenticateResult Login(string username, string password)
        {
            AuthenticateResult loginResult = AuthenticateResult.Failed;

            if (!IsLoggedIn())
            {
                AuthenticateReturn authenticateResult = _authenticationService.Authenticate(username, password);

                if (authenticateResult.Result == AuthenticateResult.Successful)
                {
                    _currentUser = authenticateResult.User;
                    UpdatePasswordListFromDB();
                }
                else
                {
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
            AddUserResult result = _userService.AddUser(user);
            return result;
        }

        public ValidateUserPasswordResult ChangeUserPassword(string originalPassword, string newPassword, string confirmPassword)
        {
            ValidateUserPasswordResult result = ValidateUserPasswordResult.Failed;

            if (IsLoggedIn())
            {
                result = _userService.ChangeUserPassword(_currentUser.GUID, originalPassword, newPassword, confirmPassword, _currentUser.PlainTextRandomKey);
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
                    result = _userService.ModifyUser(_currentUser.GUID, user, _currentUser.PlainTextRandomKey);

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
                    AddPasswordResult addResult = _passwordService.AddPassword(_currentUser.GUID, password, _currentUser.PlainTextRandomKey);

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

                        AddModifyPasswordResult addResult = _passwordService.ModifyPassword(_currentUser.GUID, modifiedWithUniqueID, _currentUser.PlainTextRandomKey);

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

        public ExportResult ExportPasswords(ExportFileTypes fileType, string exportPath)
        {
            ExportResult result;

            result = _exportPasswords.Export(fileType, exportPath);

            return result;
        }

        /*PRIVATE METHODS**************************************************/
        private void UpdatePasswordListFromDB()
        {
            _passwordList.Clear();
            _passwordList = _passwordService.GetPasswords(_currentUser.GUID, _currentUser.PlainTextRandomKey);
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

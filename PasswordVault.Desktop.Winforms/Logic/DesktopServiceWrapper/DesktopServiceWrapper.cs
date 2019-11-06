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

    class DesktopServiceWrapper : IDesktopServiceWrapper
    {
        /*CONSTANTS********************************************************/

        /*FIELDS***********************************************************/
        private User _currentUser;                       // Current user's username and password
        private List<Password> _passwordList;            // stores the current users passwords and binds to datagridview

        private IPasswordService _passwordService;
        private IUserService _userService;
        private IAuthenticationService _authenticationService;

        /*PROPERTIES*******************************************************/

        /*CONSTRUCTORS*****************************************************/
        public DesktopServiceWrapper(IAuthenticationService authenticationService, IPasswordService passwordService, IUserService userService)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
            _passwordService = passwordService ?? throw new ArgumentNullException(nameof(passwordService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));

            _currentUser = new User(false);
            _passwordList = new List<Password>();
        }

        /*PUBLIC METHODS***************************************************/    
        public AuthenticateResult Login(string username, string password)
        {
            AuthenticateResult loginResult = AuthenticateResult.Failed;

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
            return loginResult;
        }

        public bool VerifyCurrentUserPassword(string password)
        {
            bool result = false;

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
            throw new NotImplementedException();
        }

        public AddModifyPasswordResult AddPassword(Password password)
        {
            throw new NotImplementedException();
        }

        public DeleteUserResult DeleteUser(User user)
        {
            DeleteUserResult result = DeleteUserResult.Failed;

            if (IsLoggedIn())
            {
                result = _userService.DeleteUser(user.GUID);
            }
            
            return result;
        }

        public UserInformationResult EditUser(User user)
        {
            throw new NotImplementedException();
        }

        public DeletePasswordResult DeletePassword(Password password)
        {
            throw new NotImplementedException();
        }     

        public string GeneratePasswordKey()
        {
            string result = "";

            result = _passwordService.GeneratePasswordKey(
                GetMinimumPasswordLength());

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

        public int GetMinimumPasswordLength()
        {
            int result = _userService.GetMinimumPasswordLength();
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

        public AddModifyPasswordResult ModifyPassword(Password originalPassword, Password modifiedPassword)
        {
            AddModifyPasswordResult result = AddModifyPasswordResult.Failed;

            if (IsLoggedIn())
            {
                _passwordService.ModifyPassword(_currentUser.GUID, modifiedPassword);
            }

            throw new NotImplementedException();

            return result;
        }

        /*PRIVATE METHODS**************************************************/
        private void UpdatePasswordListFromDB()
        {
            _passwordList.Clear();
            _passwordList = _passwordService.GetPasswords(_currentUser.GUID, _currentUser.PlainTextRandomKey);
        }

        /*STATIC METHODS***************************************************/

    } // DesktopServiceWrapper CLASS
}

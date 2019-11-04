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
            _authenticationService = authenticationService;
            _passwordService = passwordService;
            _userService = userService;
        }

        /*PUBLIC METHODS***************************************************/    
        public AuthenticateResult Login(string username, string password)
        {
            AuthenticateResult loginResult = AuthenticateResult.Failed;

            AuthenticateReturn authenticateResult = _authenticationService.Authenticate(username, password);

            if (authenticateResult.Result == AuthenticateResult.Successful)
            {
                _currentUser = authenticateResult.User;
            }
            else
            {
                _currentUser = new User(false);
            }

            loginResult = authenticateResult.Result;
            return loginResult;
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

        public AddModifyPasswordResult AddPassword(Password password)
        {
            throw new NotImplementedException();
        }

        public ValidateUserPasswordResult ChangeUserPassword(string originalPassword, string newPassword, string confirmPassword)
        {
            throw new NotImplementedException();
        }

        public Password DecryptPassword(Password password)
        {
            throw new NotImplementedException();
        }

        public DeletePasswordResult DeletePassword(Password password)
        {
            throw new NotImplementedException();
        }

        public DeleteUserResult DeleteUser(User user)
        {
            throw new NotImplementedException();
        }

        public UserInformationResult EditUser(User user)
        {
            throw new NotImplementedException();
        }

        public string GeneratePasswordKey()
        {
            throw new NotImplementedException();
        }

        public User GetCurrentUser()
        {
            throw new NotImplementedException();
        }

        public string GetCurrentUsername()
        {
            throw new NotImplementedException();
        }

        public int GetMinimumPasswordLength()
        {
            int result = _userService.GetMinimumPasswordLength();
            return result;
        }

        public List<Password> GetPasswords()
        {
            throw new NotImplementedException();
        }

        public AddModifyPasswordResult ModifyPassword(Password originalPassword, Password modifiedPassword)
        {
            throw new NotImplementedException();
        }

        public bool VerifyCurrentUserPassword(string password)
        {
            throw new NotImplementedException();
        }

        /*PRIVATE METHODS**************************************************/

        /*STATIC METHODS***************************************************/

    } // DesktopServiceWrapper CLASS
}

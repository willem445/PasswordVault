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

        /*PROPERTIES*******************************************************/

        /*CONSTRUCTORS*****************************************************/
        public DesktopServiceWrapper()
        {

        }

        /*PUBLIC METHODS***************************************************/
        public AddModifyPasswordResult AddPassword(Password password)
        {
            throw new NotImplementedException();
        }

        public ValidateUserPasswordResult ChangeUserPassword(string originalPassword, string newPassword, string confirmPassword)
        {
            throw new NotImplementedException();
        }

        public AddUserResult CreateNewUser(User user)
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
            throw new NotImplementedException();
        }

        public List<Password> GetPasswords()
        {
            throw new NotImplementedException();
        }

        public bool IsLoggedIn()
        {
            throw new NotImplementedException();
        }

        public AuthenticateResult Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public LogOutResult Logout()
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

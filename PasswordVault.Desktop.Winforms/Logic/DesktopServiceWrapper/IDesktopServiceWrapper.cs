using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordVault.Models;
using PasswordVault.Services;

namespace PasswordVault.Desktop.Winforms
{
    public interface IDesktopServiceWrapper
    {
        AuthenticateResult Login(string username, string password);
        LogOutResult Logout();
        bool IsLoggedIn();
        AddUserResult CreateNewUser(User user);
        string GetCurrentUsername();
        User GetCurrentUser();
        ValidateUserPasswordResult ChangeUserPassword(string originalPassword, string newPassword, string confirmPassword);
        bool VerifyCurrentUserPassword(string password);
        DeleteUserResult DeleteUser(User user);
        UserInformationResult EditUser(User user);
        AddModifyPasswordResult AddPassword(Password password);
        DeletePasswordResult DeletePassword(Password password);
        AddModifyPasswordResult ModifyPassword(Password originalPassword, Password modifiedPassword);
        List<Password> GetPasswords();
        Password DecryptPassword(Password password);
        int GetMinimumPasswordLength();
        string GeneratePasswordKey();
    }
}

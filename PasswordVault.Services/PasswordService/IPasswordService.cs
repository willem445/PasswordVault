using System.Collections.Generic;
using PasswordVault.Models;

namespace PasswordVault.Services
{
    public interface IPasswordService
    {
        LoginResult Login(string username, string password);
        LogOutResult Logout();
        bool IsLoggedIn();
        CreateUserResult CreateNewUser(User user);
        string GetCurrentUsername();
        User GetCurrentUser();
        ChangeUserPasswordResult ChangeUserPassword(string originalPassword, string newPassword, string confirmPassword);
        bool VerifyCurrentUserPassword(string password);
        DeleteUserResult DeleteUser(User user);
        UserInformationResult EditUser(User user);
        int GetMinimumPasswordLength();
        string GeneratePasswordKey();
        AddPasswordResult AddPassword(Password unencryptedPassword);
        DeletePasswordResult DeletePassword(Password encryptedPassword);
        AddPasswordResult ModifyPassword(Password originalPassword, Password modifiedPassword);
        List<Password> GetPasswords();
        Password DecryptPassword(Password password);
    }
}

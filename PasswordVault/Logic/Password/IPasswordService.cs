using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordVault
{
    interface IPasswordService
    {
        LoginResult Login(string username, string password);
        LogOutResult Logout();
        bool IsLoggedIn();
        CreateUserResult CreateNewUser(User user);
        string GetCurrentUserID();
        int GetMinimumPasswordLength();
        string GeneratePasswordKey();
        void DeleteUser();
        void ChangeUserPassword(string username, string oldPassword, string newPassword);
        void EditUser();
        AddPasswordResult AddPassword(Password unencryptedPassword);
        DeletePasswordResult DeletePassword(Password encryptedPassword);
        AddPasswordResult ModifyPassword(Password originalPassword, Password modifiedPassword);
        List<Password> GetPasswords();
        Password DecryptPassword(Password password);
    }
}

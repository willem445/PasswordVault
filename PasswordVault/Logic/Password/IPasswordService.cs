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
        void Logout();
        bool IsLoggedIn();
        CreateUserResult CreateNewUser(string username, string password);
        string GetCurrentUserID();
        int GetMinimumPasswordLength();
        string GeneratePasswordKey();
        void DeleteUser(string username);
        void ChangeUserPassword(string username, string oldPassword, string newPassword);
        void AddPassword(Password unencryptedPassword);
        void RemovePassword(Password encryptedPassword);
        void ModifyPassword();
        List<Password> GetPasswords();
    }
}

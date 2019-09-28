using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordVault
{
    public interface IPasswordService
    {
        LoginResult Login(string username, string password);
        LogOutResult Logout();
        bool IsLoggedIn();
        CreateUserResult CreateNewUser(User user);
        string GetCurrentUsername();
        User GetCurrentUser();
        void ChangeUserPassword(User user);
        DeleteUserResult DeleteUser(User user);
        void EditUser(User user);
        int GetMinimumPasswordLength();
        string GeneratePasswordKey();        
        AddPasswordResult AddPassword(Password unencryptedPassword);
        DeletePasswordResult DeletePassword(Password encryptedPassword);
        AddPasswordResult ModifyPassword(Password originalPassword, Password modifiedPassword);
        List<Password> GetPasswords();
        Password DecryptPassword(Password password);
    }
}

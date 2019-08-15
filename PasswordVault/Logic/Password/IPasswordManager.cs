using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordVault
{
    interface IPasswordManager
    {
        void Login(string username, string password);
        void Logout();
        bool IsLoggedIn();
        void CreateNewUser(string username, string password);
        void DeleteUser(string username);
        void ChangeUserPassword(string username, string oldPassword, string newPassword);
        void AddPassword();
        void RemovePassword();
        void ModifyPassword();      
    }
}

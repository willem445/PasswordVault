using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordVault
{
    public interface IDatabase
    {
        // Master users/password
        User GetUser(string username);
        List<User> GetAllUsers();
        void AddUser(string username, string salt, string hash);
        void ModifyUser(string username, User modifiedUser);
        void DeleteUser(string username);
        bool UserExists(string username);

        // User passwords
        void AddPassword(string username, Password password);
        void ModifyPassword(string username, Password password, Password modifiedPassword);
        void DeletePassword(string username, Password password);
        List<Password> GetUserPasswords(string username);
    }
}

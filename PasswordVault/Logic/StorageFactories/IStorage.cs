using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordVault
{
    public interface IStorage
    {
        // Master users/password
        User GetUser(string username);
        List<User> GetUsers();
        void AddUser(string userName, string salt, string hash);
        void ModifyUser(User user, User modifiedUser);
        void DeleteUser(User user);

        // User passwords
        bool SetUserTableName(string name);
        void ClearUserTableName();
        void CreateUserTable(string name);
        void AddPassword(Password password);
        void ModifyPassword(Password password, Password modifiedPassword);
        void DeletePassword(Password password);
        List<Password> GetPasswords();
    }
}

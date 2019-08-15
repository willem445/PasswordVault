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
        List<User> GetUsers();
        void AddUser(string userName, string salt, string hash);
        void ModifyUser(User user, User modifiedUser);
        void DeleteUser(User user);
        bool UserExists(User user);
        bool UserPasswordTableExists(User user);

        // User passwords
        bool SetUserPasswordTableName(string name);
        void ClearUserPasswordTableName();
        void CreateUserPasswordTable(string name);
        void AddPassword(Password password);
        void ModifyPassword(Password password, Password modifiedPassword);
        void DeletePassword(Password password);
        List<Password> GetPasswords();
    }
}

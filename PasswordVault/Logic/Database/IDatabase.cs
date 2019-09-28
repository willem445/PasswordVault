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
        void AddUser(User user);
        void ModifyUser(string username, User modifiedUser);
        void DeleteUser(User modifiedUser);
        bool UserExists(string username);

        // User passwords
        void AddPassword(DatabasePassword password);
        void ModifyPassword(DatabasePassword password, DatabasePassword modifiedPassword);
        void DeletePassword(DatabasePassword password);
        List<DatabasePassword> GetUserPasswords(string username);
    }
}

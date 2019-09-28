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
        User GetUserByUsername(string username);
        User GetUserByGUID(string guid);
        List<User> GetAllUsers();
        void AddUser(User user);
        void ModifyUser(User user, User modifiedUser);
        void DeleteUser(User user);
        bool UserExistsByUsername(string username);
        bool UserExistsByGUID(string guid);

        // User passwords
        void AddPassword(DatabasePassword password);
        void ModifyPassword(DatabasePassword password, DatabasePassword modifiedPassword);
        void DeletePassword(DatabasePassword password);
        List<DatabasePassword> GetUserPasswordsByGUID(string guid);
    }
}

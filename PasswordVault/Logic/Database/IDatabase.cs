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
        bool AddUser(User user);
        bool ModifyUser(User user, User modifiedUser);
        bool DeleteUser(User user);
        bool UserExistsByUsername(string username);
        bool UserExistsByGUID(string guid);

        // User passwords
        bool AddPassword(DatabasePassword password);
        bool ModifyPassword(DatabasePassword password, DatabasePassword modifiedPassword);
        bool DeletePassword(DatabasePassword password);
        List<DatabasePassword> GetUserPasswordsByGUID(string guid);
    }
}

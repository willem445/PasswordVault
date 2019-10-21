using System;
using System.Collections.Generic;
using PasswordVault.Models;

namespace PasswordVault.Data
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
        /// <summary>
        /// Add a password to a database.
        /// </summary>
        /// <param name="password">Database password object to add.</param>
        /// <returns></returns>
        Int64 AddPassword(DatabasePassword password);
        bool ModifyPassword(DatabasePassword password, DatabasePassword modifiedPassword);
        bool DeletePassword(DatabasePassword password);
        List<DatabasePassword> GetUserPasswordsByGUID(string guid);
    }
}

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
        Task<User> GetUser(string username);
        Task<List<User>> GetAllUsers();
        Task AddUser(string username, string salt, string hash);
        Task ModifyUser(string username, User modifiedUser);
        Task DeleteUser(string username);
        Task<bool> UserExists(string username);

        // User passwords
        Task AddPassword(string username, Password password);
        Task ModifyPassword(string username, Password password, Password modifiedPassword);
        Task DeletePassword(string username, Password password);
        Task<List<Password>> GetUserPasswords(string username);
    }
}

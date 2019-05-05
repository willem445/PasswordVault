using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordVault
{
    public interface IStorage
    {
        void AddUser(User user);
        void ModifyUser(User user, User modifiedUser);
        void DeleteUser(User user);
        User GetUser(string username);
        List<User> GetUsers();
        void AddPassword(Password password);
        void ModifyPassword(Password password, Password modifiedPassword);
        void DeletePassword(Password password);
        List<Password> GetPasswords();
    }
}

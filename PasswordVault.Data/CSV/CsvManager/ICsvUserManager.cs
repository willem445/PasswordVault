using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordVault.Models;

namespace PasswordVault.Data
{
    public interface ICSVUserManager
    {
        void ParseUsersCSVFile(string file);
        List<User> GetEncryptedUsers();
        void UpdateUsersCSVFile(string filename, List<User> encryptedUsers);
    }
}

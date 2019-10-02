using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordVault.Desktop.Winforms
{
    public interface ICSVUserManager
    {
        void ParseUsersCSVFile(string file);
        List<User> GetEncryptedUsers();
        void UpdateUsersCSVFile(string filename, List<User> encryptedUsers);
    }
}

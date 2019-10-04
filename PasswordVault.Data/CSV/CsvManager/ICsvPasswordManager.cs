using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordVault.Models;

namespace PasswordVault.Data
{
    public interface ICSVPasswordManager
    {
        void ParsePasswordCSVFile(string fileName);
        List<DatabasePassword> GetEncryptedPasswords();
        void UpdatePasswordCSVFile(string filename, List<DatabasePassword> encryptedUsers);
    }
}

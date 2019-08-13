using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordVault
{
    interface IMasterPassword
    {
        CryptData_S HashPassword(string password);
        string GetFormattedString();
        bool VerifyPassword(string password, string salt, string hash);
    }
}

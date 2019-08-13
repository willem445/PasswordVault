using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordVault
{
    interface IPasswordManager
    {
        void Login(string username, string password);
    }
}

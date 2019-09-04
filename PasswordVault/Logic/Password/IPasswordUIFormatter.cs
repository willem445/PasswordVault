using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordVault
{
    interface IPasswordUIFormatter
    {
        Password PasswordUIToService(Password uiPassword, string key);
        Password PasswordServiceToUI(Password servicePassword, string key);
    }
}

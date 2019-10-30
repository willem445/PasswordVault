using System;
using System.Collections.Generic;
using System.Text;
using PasswordVault.Models;

namespace PasswordVault.Services
{
    interface IUserAuthentication
    {
        User Authenticate(string username, string password);
    }
}

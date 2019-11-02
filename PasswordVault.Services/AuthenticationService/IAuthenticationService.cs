using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    interface IAuthenticationService
    {
        AuthenticateResult Authenticate(string username, string password);
    }
}

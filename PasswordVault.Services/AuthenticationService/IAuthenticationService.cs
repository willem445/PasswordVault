using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Authenticates username and password against database stored users.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>Returns AuthenticateReturn object containing AuthenticateResult enum and User object populated
        /// with UserUuid, username, RandomKey, FirstName, LastName, PhoneNumber and Email in plaintext.</returns>
        AuthenticateReturn Authenticate(string username, string password);
    }
}

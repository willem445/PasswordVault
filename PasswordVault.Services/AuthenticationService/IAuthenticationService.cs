using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    /// <summary>
    /// Methods to authenticate a user.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Authenticates username and password against database stored users.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>Returns AuthenticateReturn object containing AuthenticateResult enum and User object populated
        /// with UserUuid, username, RandomKey, FirstName, LastName, PhoneNumber and Email in plaintext.</returns>
        AuthenticateReturn Authenticate(string username, string password, EncryptionParameters parameters);

        /// <summary>
        /// Verifies user provided password against DB stored hash and salt if the
        /// user exists.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>Returns true if credentials are valid.</returns>
        bool VerifyUserCredentials(string username, string password);
    }
}

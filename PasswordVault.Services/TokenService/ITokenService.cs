using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    /// <summary>
    /// Generates tokens used for authentication.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Generates a JWT token with the users unique ID as a claim.
        /// </summary>
        /// <param name="userUuid">Users unique ID.</param>
        /// <returns>JWT token.</returns>
        string GenerateJwtToken(string userUuid);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    /// <summary>
    /// Methods for generated and verifying a users password using hash and salt.
    /// </summary>
    public interface IMasterPassword
    {
        /// <summary>
        /// Generates Hash, Salt, UUID, and Random Key given a user password.
        /// </summary>
        /// <param name="password">Password to generate new user data.</param>
        /// <returns>UserEncrypedData object.</returns>
        UserEncrypedData GenerateNewUserEncryptedDataFromPassword(string password);

        /// <summary>
        /// Verifies a password against given hash, salt, and iteration count.
        /// </summary>
        /// <param name="password">Password to verify.</param>
        /// <param name="salt">Salt generated.</param>
        /// <param name="hash">Hash generated.</param>
        /// <param name="iterationCount">Iterations to compute hash.</param>
        /// <returns>Returns true if password matches hash.</returns>
        bool VerifyPassword(string password, string salt, string hash, int iterationCount);

        /// <summary>
        /// Generates random key using RNGCryptoServiceProvider
        /// </summary>
        /// <returns>Random key string.</returns>
        string GenerateRandomKey(int sizeInBytes);
    }
}

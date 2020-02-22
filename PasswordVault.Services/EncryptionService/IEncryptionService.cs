using System;
using System.Collections.Generic;

namespace PasswordVault.Services
{
    /// <summary>
    /// Methods that encypt and decrypt user data.
    /// </summary>
    public interface IEncryptionService
    {
        /// <summary>
        /// Encrypts a plaintext string with given passphrase.
        /// </summary>
        /// <param name="plainText">Plaintext string to encrypt.</param>
        /// <param name="passPhrase">Passphrase used to encrypt string.</param>
        /// <returns>Ciphertext.</returns>
        string Encrypt(string plainText, string passPhrase);

        /// <summary>
        /// Decrypts ciphertext using a given passphrase.
        /// </summary>
        /// <param name="cipherText">Encrypted text to be decrypted.</param>
        /// <param name="passPhrase">Passphrase used to encrypt the ciphertext.</param>
        /// <returns>Decrypted ciphertext.</returns>
        string Decrypt(string cipherText, string passPhrase);
    }
}

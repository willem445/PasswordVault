using System;
using System.Collections.Generic;
using System.Text;
using PasswordVault.Models;

namespace PasswordVault.Services
{
    /// <summary>
    /// Methods for manipulating a users stored passwords. Interfaces with a given database 
    /// implementation. 
    /// </summary>
    public interface IPasswordService
    {
        /// <summary>
        /// Adds a password to the database. Encrypts user's data before pushing to database.
        /// </summary>
        /// <param name="userUuid">Unique ID of the user.</param>
        /// <param name="password">Password object to be stored. Should contain plaintext
        /// application, username, email, description, website, and passphrase.</param>
        /// <param name="key">Plaintext key used to encrypt data to be stored.</param>
        /// <returns>Result of adding password.</returns>
        AddPasswordResult AddPassword(string userUuid, Password password, string key, EncryptionParameters parameters);

        /// <summary>
        /// Modifies a users password entry in database.
        /// </summary>
        /// <param name="userUuid">Unique ID of the user.</param>
        /// <param name="modifiedPassword">Password object containing modified password.
        /// Should contain plaintext application, username, email, description, website,
        /// and passphrase.</param>
        /// <param name="key">Plaintext key used to encrypt data to be stored.</param>
        /// <returns>Result of modifing password.</returns>
        ValidatePassword ModifyPassword(string userUuid, Password modifiedPassword, string key, EncryptionParameters parameters);

        /// <summary>
        /// Deletes password from database
        /// </summary>
        /// <param name="passwordUuid">Unique ID of the password to be deleted.</param>
        /// <returns>Result of deleting password.</returns>
        DeletePasswordResult DeletePassword(Int64 passwordUuid);

        /// <summary>
        /// Gets all user password objects stored in database and decrypts the data.
        /// </summary>
        /// <param name="userUuid">Unique ID of the user.</param>
        /// <param name="decryptionKey">Plaintext key used to decrypt stored data.</param>
        /// <returns>List of plaintext password objects.</returns>
        List<Password> GetPasswords(string userUuid, string decryptionKey, EncryptionParameters parameters);

        /// <summary>
        /// Generates a random password key of given length.
        /// </summary>
        /// <param name="length">Desired length of password.</param>
        /// <returns>Password key.</returns>
        string GeneratePassword(int length);
    }
}

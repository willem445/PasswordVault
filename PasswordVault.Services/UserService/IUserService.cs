using System;
using System.Collections.Generic;
using System.Text;
using PasswordVault.Models;

namespace PasswordVault.Services
{
    /// <summary>
    /// Methods for manipulating users. Interfaces with a given database 
    /// implementation. 
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Validates user information and adds user to database if successful.
        /// </summary>
        /// <param name="user">User to add.</param>
        /// <returns>Result of adding user.</returns>
        AddUserResult AddUser(User user, EncryptionParameters parameters, MasterPasswordParameters hashParameters);

        /// <summary>
        /// Deletes user from database.
        /// </summary>
        /// <param name="userUuid">Unique ID of user to delete.</param>
        /// <param expectedNumPasswords="userUuid">Number of passwords that are expected to be deleted for the user.</param>
        /// <returns>Result of deleting user.</returns>
        DeleteUserResult DeleteUser(string userUuid, int expectedNumPasswords);

        /// <summary>
        /// Modifies a users information. Firstname, lastname, email and phone number 
        /// can be modified.
        /// </summary>
        /// <param name="userUuid">Unique ID of user to modify.</param>
        /// <param name="modifiedUser">User object containing modified fields. 
        /// Must contain valid firstname, lastname, phone number and email.</param>
        /// <param name="encryptionKey">Key used to encrypt users data.</param>
        /// <returns>Result of modifying user.</returns>
        UserInformationResult ModifyUser(string userUuid, User modifiedUser, string encryptionKey, EncryptionParameters parameters);

        /// <summary>
        /// Verifies a users password.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>Returns true is password is valid.</returns>
        bool VerifyUserPassword(string username, string password);

        /// <summary>
        /// Changes a users password. Encrypts the randomly generated key with the 
        /// new password provided.
        /// </summary>
        /// <param name="userUuid">Unique ID of the user.</param>
        /// <param name="originalPassword">Original password.</param>
        /// <param name="newPassword">New password.</param>
        /// <param name="confirmPassword">Confirm password.</param>
        /// <param name="encryptionKey">Key used for encryption.</param>
        /// <returns>Returns change password result.</returns>
        ValidateUserPasswordResult ChangeUserPassword(string userUuid, string originalPassword, string newPassword, string confirmPassword, string encryptionKey, EncryptionParameters parameters, MasterPasswordParameters hashParameters);

        /// <summary>
        /// Gets a user from the database by the users unique ID.
        /// </summary>
        /// <param name="userUuid">Users unique ID.</param>
        /// <returns>User obejct from database. Null if not found.</returns>
        User GetUser(string userUuid);

        User GetUserByUsername(string username);

        /// <summary>
        /// Gets the minimum password length for a users password.
        /// </summary>
        /// <returns>Minimum password length required.</returns>
        int GetMinimumPasswordLength();
    }
}

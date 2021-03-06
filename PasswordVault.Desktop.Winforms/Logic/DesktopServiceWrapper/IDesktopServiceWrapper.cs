﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordVault.Models;
using PasswordVault.Services;

namespace PasswordVault.Desktop.Winforms
{
    public interface IDesktopServiceWrapper
    {
        event Action<AuthenticateResult> AuthenticationResultEvent;
        event Action DoneLoadingPasswordsEvent;
        event Action PasswordReady;

        /// <summary>
        /// Verifies that a users password is correct, retrieves user information with
        /// generated JWT token.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>Returns AuthenticateResult enum.</returns>
        AuthenticateResult Login(string username, string password);

        Task<AuthenticateResult> LoginAsync(string username, string password);

        /// <summary>
        /// Logs the current user out and clears user data from memory.
        /// </summary>
        /// <returns>Returns LogOutResult enum.</returns>
        LogOutResult Logout();

        /// <summary>
        /// Checks if a user is currently logged into the application.
        /// </summary>
        /// <returns>Returns true if user is logged in.</returns>
        bool IsLoggedIn();

        /// <summary>
        /// Creates a new user and adds the user to the database.
        /// </summary>
        /// <param name="user">User object containing username, password, firstname, lastname, email and phone number.</param>
        /// <returns>AddUserResult</returns>
        AddUserResult CreateNewUser(User user);
        string GetCurrentUsername();
        User GetCurrentUser();
        ValidateUserPasswordResult ChangeUserPassword(string originalPassword, string newPassword, string confirmPassword);
        bool VerifyCurrentUserPassword(string password);
        DeleteUserResult DeleteUser(User user, int expectedPasswordCount);
        UserInformationResult EditUser(User user);
        ValidatePassword AddPassword(Password password);
        DeletePasswordResult DeletePassword(Password password);
        ValidatePassword ModifyPassword(Password originalPassword, Password modifiedPassword);
        List<Password> GetPasswords();

        /// <summary>
        /// Returns current number of logged in user's passwords.
        /// </summary>
        /// <returns>Number of passwords. -1 if error.</returns>
        int GetPasswordCount();
        int GetMinimumPasswordLength();
        string GeneratePassword();

        ImportExportResult ExportPasswords(ImportExportFileType fileType, string exportPath, string password = null, bool passwordEnabled = false);

        ImportExportResult ImportPasswords(ImportExportFileType fileType, string importPath, string password = null, bool passwordEnabled = false);

        List<SupportedFileTypes> GetSupportedFileTypes();
    }
}

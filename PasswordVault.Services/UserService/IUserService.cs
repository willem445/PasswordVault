using System;
using System.Collections.Generic;
using System.Text;
using PasswordVault.Models;

namespace PasswordVault.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Validates user information and adds user to database if successful.
        /// </summary>
        /// <param name="user">User to add.</param>
        /// <returns>AddUserResult</returns>
        AddUserResult AddUser(User user);
        DeleteUserResult DeleteUser(string userUuid);
        UserInformationResult ModifyUser(string userUuid, User user);
        bool VerifyUserPassword(string userUuid, string password);
        ValidateUserPasswordResult ChangeUserPassword(string userUuid, string originalPassword, string newPassword, string confirmPassword);
        User GetUser(string userUuid);
        int GetMinimumPasswordLength();
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using PasswordVault.Models;

namespace PasswordVault.Services
{
    public interface IPasswordService
    {
        AddModifyPasswordResult AddPassword(string userUuid, Password password);
        AddModifyPasswordResult ModifyPassword(string userUuid, Password modifiedPassword);
        DeletePasswordResult DeletePassword(string userUuid, string passwordUuid);
        List<Password> GetPasswords(string userUuid, string decryptionKey);
        string GeneratePasswordKey(int length);
    }
}

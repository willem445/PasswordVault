using System;
using System.Collections.Generic;
using System.Text;
using PasswordVault.Models;

namespace PasswordVault.Services
{
    public interface IPasswordService
    {
        AddPasswordResult AddPassword(string userUuid, Password password, string key);
        AddModifyPasswordResult ModifyPassword(string userUuid, Password modifiedPassword, string key);
        DeletePasswordResult DeletePassword(Int64 passwordUuid);
        List<Password> GetPasswords(string userUuid, string decryptionKey);
        string GeneratePasswordKey(int length);
    }
}

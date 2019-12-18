using System;
using System.Collections.Generic;
using System.Text;
using PasswordVault.Data;

namespace PasswordVault.Services
{
    public interface IEncryptionConversionFactory
    {
        IEncryptionConversion Get(EncryptionService from, EncryptionService to, IPasswordService passwordService, IUserService userService);
    }
}

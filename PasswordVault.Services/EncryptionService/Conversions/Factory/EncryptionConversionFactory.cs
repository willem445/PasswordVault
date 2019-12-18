using System;
using System.Collections.Generic;
using System.Text;
using PasswordVault.Data;

namespace PasswordVault.Services
{
    public class EncryptionConversionFactory : IEncryptionConversionFactory
    {
        public IEncryptionConversion Get(EncryptionService from, EncryptionService to, IPasswordService passwordService, IUserService userService)
        {
            IEncryptionConversion conversion = null;

            if (from == EncryptionService.RijndaelManaged && to == EncryptionService.Aes)
            {
                conversion = new RijndaelToAesConversion(passwordService, userService);
            }

            return conversion;
        }
    }
}

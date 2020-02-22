using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PasswordVault.Services
{
    class PBKDF2KeyDerivation : IKeyDerivation
    {
        public byte[] DeriveKey(string password, byte[] salt, KeyDerivationParameters parameters, int keySizeInBytes = 0)
        {
            byte[] key = new byte[parameters.KeySizeBytes];
            using (var derivation = new Rfc2898DeriveBytes(password, salt, parameters.Iterations))
            {
                key = derivation.GetBytes(parameters.KeySizeBytes); // multiply key size by 2 since we need two keys
            }
            return key;
        }
    }
}

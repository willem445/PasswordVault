using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace PasswordVault.Services
{
    class PBKDF2KeyDerivation : IKeyDerivation
    {
        public byte[] DeriveKey(string password, byte[] salt, KeyDerivationParameters parameters, int keySizeInBytes = 0)
        {
            int keySize = 0;

            if (keySizeInBytes != 0)
            {
                keySize = keySizeInBytes;
            }
            else
            {
                keySize = parameters.KeySizeBytes;
            }

            byte[] key = new byte[keySize];
            key = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, parameters.Iterations, keySize);

            return key;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    public class BcryptKeyDerivation : IKeyDerivation
    {
        public byte[] DeriveKey(string password, byte[] salt, KeyDerivationParameters parameters, int keySizeInBytes = 0)
        {
            byte[] bytes = null;

            throw new NotImplementedException();

            return bytes;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    interface IKeyDerivation
    {
        byte[] DeriveKey(string password, byte[] salt, int numBytes, KeyDerivationParameters parameters);
    }
}

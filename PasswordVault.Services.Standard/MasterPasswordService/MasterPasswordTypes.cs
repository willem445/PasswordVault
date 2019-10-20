using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PasswordVault.Services.Standard
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct UserEncrypedData
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        public UserEncrypedData(string salt, string hash, int iterations, string uniqueGUID, string randomGeneratedKey)
        {
            Salt = salt;
            Hash = hash;
            Iterations = iterations;
            UniqueGUID = uniqueGUID;
            RandomGeneratedKey = randomGeneratedKey;
        }

        public string Salt { get; }
        public string Hash { get; }
        public int Iterations { get; }
        public string UniqueGUID { get; }
        public string RandomGeneratedKey { get; }
    }
} // PasswordVault.Services.Standard NAMESPACE

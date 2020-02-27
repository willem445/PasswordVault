using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    public enum RecommendedParametersType
    {
        Hash = 0,
        Encryption = 1
    }

    interface IKeyDerivation
    {
        byte[] DeriveKey(string password, byte[] salt, KeyDerivationParameters parameters, int keySizeInBytes = 0);

        KeyDerivationParameters GetRecommendedParameters(RecommendedParametersType type);
    }
}

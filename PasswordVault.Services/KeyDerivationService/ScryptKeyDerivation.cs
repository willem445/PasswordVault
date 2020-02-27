using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Sodium;

namespace PasswordVault.Services
{
    class ScryptKeyDerivation : IKeyDerivation
    {
        KeyDerivationParameters _defaultKeyDerivationParameters = new KeyDerivationParameters(
            algorithm: KeyDerivationAlgorithm.Argon2Id,
            keysize: 32,
            saltsize: 32,
            iterations: 1000000,
            degreeofparallelism: -1,
            memorySizeKb: 1048576 // 1gb
            );

        KeyDerivationParameters _defaultEncryptionKeyDerivationParameters = new KeyDerivationParameters(
            algorithm: KeyDerivationAlgorithm.Argon2Id,
            keysize: 32,
            saltsize: 32,
            iterations: 10000,
            degreeofparallelism: -1,
            memorySizeKb: 1024 // 1mb
            );

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

            var memorySizeBytes = parameters.MemorySizeKb * 1024;

            if (memorySizeBytes < 16777216)
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "{0} must be greater than 16mb!.", (nameof(parameters))));

            if (parameters.Iterations < 32768)
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "{0} must be greater than 32768!.", (nameof(parameters))));

            if (salt.Length != 32)
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "{0} must be 32 bytes long!", (nameof(parameters))));

            byte[] passBytes = Encoding.ASCII.GetBytes(password);
            var hash = PasswordHash.ScryptHashBinary(passBytes, salt, ((long)parameters.Iterations), memorySizeBytes, keySize);

            return hash;
        }

        public KeyDerivationParameters GetRecommendedParameters(RecommendedParametersType type)
        {
            KeyDerivationParameters result = new KeyDerivationParameters();

            switch (type)
            {
                case RecommendedParametersType.Hash:
                    result = _defaultKeyDerivationParameters;
                    break;
                case RecommendedParametersType.Encryption:
                    result = _defaultEncryptionKeyDerivationParameters;
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}

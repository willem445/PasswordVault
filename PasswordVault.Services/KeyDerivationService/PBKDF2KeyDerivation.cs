using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using System.Globalization;

namespace PasswordVault.Services
{
    class PBKDF2KeyDerivation : IKeyDerivation
    {
        KeyDerivationParameters _defaultKeyDerivationParameters = new KeyDerivationParameters(
            algorithm: KeyDerivationAlgorithm.Argon2Id,
            keysize: 32,
            saltsize: 16,
            iterations: 10000000,
            degreeofparallelism: -1,
            memorySizeKb: -1 // 1gb
            );

        KeyDerivationParameters _defaultEncryptionKeyDerivationParameters = new KeyDerivationParameters(
            algorithm: KeyDerivationAlgorithm.Argon2Id,
            keysize: 32,
            saltsize: 16,
            iterations: 10000,
            degreeofparallelism: -1,
            memorySizeKb: -1 // 1mb
            );

        public byte[] DeriveKey(string password, byte[] salt, KeyDerivationParameters parameters, int keySizeInBytes = 0)
        {
            int keySize = 0;

            if (parameters.Iterations > int.MaxValue)
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "{0} must be less than {1}!", (nameof(parameters.Iterations)), int.MaxValue));

            if (keySizeInBytes != 0)
            {
                keySize = keySizeInBytes;
            }
            else
            {
                keySize = parameters.KeySizeBytes;
            }

            byte[] key = new byte[keySize];
            key = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, (int)parameters.Iterations, keySize);

            return key;
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

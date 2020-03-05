using System;
using System.Globalization;
using System.Text;
using Konscious.Security.Cryptography;

namespace PasswordVault.Services
{
    class Argon2IdKeyDerivation : IKeyDerivation
    {
        KeyDerivationParameters _defaultKeyDerivationParameters = new KeyDerivationParameters(
            algorithm:KeyDerivationAlgorithm.Argon2Id,
            keysize: 32, 
            saltsize: 16, 
            iterations: 10,
            degreeofparallelism: 8,
            memorySizeKb: 1048576 // 1gb
            );

        KeyDerivationParameters _defaultEncryptionKeyDerivationParameters = new KeyDerivationParameters(
            algorithm: KeyDerivationAlgorithm.Argon2Id,
            keysize: 32, 
            saltsize: 16, 
            iterations: 40,
            degreeofparallelism: 4,
            memorySizeKb: 1024 // 1mb
            );

        public byte[] DeriveKey(string password, byte[] salt, KeyDerivationParameters parameters, int keySizeInBytes=0)
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
            using (var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password)))
            {
                argon2.Salt = salt;
                argon2.DegreeOfParallelism = parameters.DegreeOfParallelism;
                argon2.Iterations = (int)parameters.Iterations;
                argon2.MemorySize = parameters.MemorySizeKb;
                key = argon2.GetBytes(keySize);
            }           

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

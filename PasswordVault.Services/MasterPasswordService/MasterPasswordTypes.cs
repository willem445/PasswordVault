using System;
using System.Globalization;

namespace PasswordVault.Services
{
    public enum PasswordHashAlgorithm
    {
        Pbkdf2 = 0,
        Argon2Id = 1,
    }

    public class MasterPasswordParameters
    {
        public MasterPasswordParameters(KeyDerivationParameters keyDerivationParameters, int randomKeySize)
        {
            KeyDerivationParameters = keyDerivationParameters;
            RandomKeySize = randomKeySize;
        }

        public KeyDerivationParameters KeyDerivationParameters { get; }
        public int RandomKeySize { get; }
    }

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct UserEncrypedData
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        public UserEncrypedData(KeyDerivationAlgorithm alg, int keysize, string salt, string hash, int iterations, string randomGeneratedKey)
        {
            KeyDevAlgorithm = alg;
            KeySize = keysize;
            Salt = salt;
            Hash = hash;
            Iterations = iterations;
            RandomGeneratedKey = randomGeneratedKey;
            DegreeOfParallelism = -1;
            MemorySize = -1;            
        }

        public UserEncrypedData(KeyDerivationAlgorithm alg, int keysize, string salt, string hash, int iterations, int degreeOfParallelism, int memorySize, string randomGeneratedKey)
        {
            KeyDevAlgorithm = alg;
            KeySize = keysize;
            Salt = salt;
            Hash = hash;
            Iterations = iterations;
            DegreeOfParallelism = degreeOfParallelism;
            MemorySize = memorySize;
            RandomGeneratedKey = randomGeneratedKey;
        }

        public string Salt { get; }
        public string Hash { get; }
        public int Iterations { get; }
        public int DegreeOfParallelism { get; }
        public int MemorySize { get; }
        public string RandomGeneratedKey { get; }
        public KeyDerivationAlgorithm KeyDevAlgorithm { get; }
        public int KeySize { get; }
    }
} // PasswordVault.Services.Standard NAMESPACE

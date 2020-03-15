using System;
using System.Globalization;

namespace PasswordVault.Services
{
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

    public struct UserEncrypedData
    {
        public UserEncrypedData(KeyDerivationAlgorithm alg, int keysize, string salt, int saltsize, string hash, UInt32 iterations, int degreeOfParallelism, int memorySize, string randomGeneratedKey)
        {
            KeyDevAlgorithm = alg;
            KeySize = keysize;
            SaltSize = saltsize;
            Salt = salt;
            Hash = hash;
            Iterations = iterations;
            DegreeOfParallelism = degreeOfParallelism;
            MemorySize = memorySize;
            RandomGeneratedKey = randomGeneratedKey;
        }

        public string Salt { get; }
        public string Hash { get; }
        public UInt32 Iterations { get; }
        public int DegreeOfParallelism { get; }
        public int MemorySize { get; }
        public string RandomGeneratedKey { get; }
        public KeyDerivationAlgorithm KeyDevAlgorithm { get; }
        public int KeySize { get; }
        public int SaltSize { get; }
    }
} // PasswordVault.Services.Standard NAMESPACE

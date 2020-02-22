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
        public UserEncrypedData(KeyDerivationAlgorithm alg, int keysize, string salt, string hash, int iterations, string uniqueGUID, string randomGeneratedKey)
        {
            KeyDevAlgorithm = alg;
            KeySize = keysize;
            Salt = salt;
            Hash = hash;
            Iterations = iterations;
            UserUUID = uniqueGUID;
            RandomGeneratedKey = randomGeneratedKey;
            DegreeOfParallelism = -1;
            MemorySize = -1;            
        }

        public UserEncrypedData(KeyDerivationAlgorithm alg, int keysize, string salt, string hash, int iterations, int degreeOfParallelism, int memorySize, string uuid, string randomGeneratedKey)
        {
            KeyDevAlgorithm = alg;
            KeySize = keysize;
            Salt = salt;
            Hash = hash;
            Iterations = iterations;
            DegreeOfParallelism = degreeOfParallelism;
            MemorySize = memorySize;
            UserUUID = uuid;
            RandomGeneratedKey = randomGeneratedKey;
        }

        public string Salt { get; }
        public string Hash { get; }
        public int Iterations { get; }
        public int DegreeOfParallelism { get; }
        public int MemorySize { get; }
        public string UserUUID { get; }
        public string RandomGeneratedKey { get; }
        public KeyDerivationAlgorithm KeyDevAlgorithm { get; }
        public int KeySize { get; }

        public string GetFormattedString()
        {
            string formatted = "";

            formatted = string.Format(
                CultureInfo.CurrentCulture, 
                "{0},{1},{2},{3},{4},{5},{6}",
                UserUUID, RandomGeneratedKey, Iterations, DegreeOfParallelism, MemorySize, Salt, Hash
            );

            return formatted;
        }

        public byte[] GetFormattedMasterPasswordParameters()
        {
            throw new NotImplementedException();
        }
    }
} // PasswordVault.Services.Standard NAMESPACE

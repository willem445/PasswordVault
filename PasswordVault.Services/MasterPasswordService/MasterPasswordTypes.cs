﻿using System;
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

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct UserEncrypedData
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        public UserEncrypedData(KeyDerivationAlgorithm alg, int keysize, string salt, string hash, UInt32 iterations, int degreeOfParallelism, int memorySize, string randomGeneratedKey)
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
        public UInt32 Iterations { get; }
        public int DegreeOfParallelism { get; }
        public int MemorySize { get; }
        public string RandomGeneratedKey { get; }
        public KeyDerivationAlgorithm KeyDevAlgorithm { get; }
        public int KeySize { get; }
    }
} // PasswordVault.Services.Standard NAMESPACE

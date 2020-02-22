using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    public enum KeyDerivationAlgorithm
    {
        Argon2Id = 0,
        Pbkdf2 = 1
    }

    public class KeyDerivationParameters
    {
        public int Iterations { get; set; }
        public int DegreeOfParallelism { get; set; }
        public int MemorySize { get; set; }
        public int SaltSizeBytes { get; set; }
        public int KeySizeBytes { get; set; }
        public KeyDerivationAlgorithm Algorithm { get; set; }

        public KeyDerivationParameters()
        {

        }

        public KeyDerivationParameters(int iterations)
        {
            Iterations = iterations;
        }

        public KeyDerivationParameters(KeyDerivationAlgorithm algorithm, int iterations, int degreeOfParallelism, int memorySize)
        {
            Algorithm = algorithm;
            Iterations = iterations;
            DegreeOfParallelism = degreeOfParallelism;
            MemorySize = memorySize;
        }

        public KeyDerivationParameters(KeyDerivationAlgorithm algorithm, int keysize, int saltsize, int iterations, int degreeofparallelism, int memorySize)
        {
            Algorithm = algorithm;
            KeySizeBytes = keysize;
            SaltSizeBytes = saltsize;
            Iterations = iterations;
            DegreeOfParallelism = degreeofparallelism;
            MemorySize = memorySize;
        }
    }
}

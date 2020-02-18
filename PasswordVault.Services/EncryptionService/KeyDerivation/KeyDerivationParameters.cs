using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    class KeyDerivationParameters
    {
        public int Iterations { get; set; }
        public int DegreeOfParallelism { get; set; }
        public int MemorySize { get; set; }

        public KeyDerivationParameters()
        {

        }

        public KeyDerivationParameters(int iterations)
        {
            Iterations = iterations;
        }

        public KeyDerivationParameters(int iterations, int degreeOfParallelism, int memorySize)
        {
            Iterations = iterations;
            DegreeOfParallelism = degreeOfParallelism;
            MemorySize = memorySize;
        }
    }
}

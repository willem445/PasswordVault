using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    public enum EncryptionService
    {
        RijndaelManaged = 0,
        Aes = 1,
    }

    public struct EncryptionServiceParameters
    {
        public EncryptionServiceParameters(EncryptionService encryptionService, int iterations, int blockSize, int keySize)
        {
            EncryptionService = encryptionService;
            Iterations = iterations;
            BlockSize = blockSize;
            KeySize = keySize;
        }

        public EncryptionService EncryptionService { get; }
        public int Iterations { get; }
        public int BlockSize { get; }
        public int KeySize { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    public enum CipherSuite
    {
        Unknown = 0,
        Aes256CfbPkcs7
    }

    public enum Mac : byte
    {
        Unknown = 0,
        HMACSHA256 = 1,
        HMACSHA512 = 2
    }

    public enum EncryptionService
    {
        RijndaelManaged = 0,
        Aes = 1,
    }

    public struct EncryptionSizes
    {
        public EncryptionSizes(int iterations, int blockSize, int keySize)
        {
            Iterations = iterations;
            BlockSize = blockSize;
            KeySize = keySize;
        }

        public int Iterations { get; }
        public int BlockSize { get; }
        public int KeySize { get; }
    }

    public struct EncryptionServiceParameters
    {
        public EncryptionServiceParameters(EncryptionService encryptionService, EncryptionSizes encryptionSizes)
        {
            EncryptionService = encryptionService;
            EncryptionSizes = encryptionSizes;
        }

        public EncryptionService EncryptionService { get; }
        public EncryptionSizes EncryptionSizes { get; }
    }
}

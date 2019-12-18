﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
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

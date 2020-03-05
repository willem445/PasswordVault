using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    public enum CipherSuiteIdx
    {
        EncryptionAlg = 0,
        KeyDevAlg=1,
        KeyDevItr=2,
        KeyDevMem=6,
        KeyDevParallel=10,
        MacAlg=11,
        NumCipherSuiteBytes=12
    }


#pragma warning disable CA1028 // Enum Storage should be Int32 - Ignore since we want this to be a byte
    public enum EncryptionAlgorithm : byte
#pragma warning restore CA1028 // Enum Storage should be Int32
    {
        Unknown = 0,
        Aes256CfbPkcs7 = 1,
        Aes128CfbPkcs7 = 2,
        Rijndael256CbcPkcs7 = 3,
        Rijndael128CbcPkcs7 = 4,
    }

#pragma warning disable CA1028 // Enum Storage should be Int32 - Ignore since we want this to be a byte
    public enum Mac : byte
#pragma warning restore CA1028 // Enum Storage should be Int32
    {
        Unknown = 0,
        HMACSHA256 = 1,
        HMACSHA512 = 2
    }

    public class EncryptionParameters
    {
        public EncryptionParameters(EncryptionAlgorithm algorithm, Mac mac, KeyDerivationParameters keyDerivationParameters, int blocksize, int ivSize)
        {
            Algorithm = algorithm;
            Mac = mac;
            KeyDerivationParameters = keyDerivationParameters;
            BlockSizeBytes = blocksize;
            IvSizeBytes = ivSize;
        }

        public EncryptionAlgorithm Algorithm { get; set; }
        public Mac Mac { get; set; }
        public KeyDerivationParameters KeyDerivationParameters { get; set; }
        public int BlockSizeBytes { get; set; }
        public int IvSizeBytes { get; set; }

    }
}

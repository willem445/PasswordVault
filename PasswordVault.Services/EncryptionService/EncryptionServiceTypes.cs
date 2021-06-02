namespace PasswordVault.Services
{
    public enum PackedCipherSuiteParametersIndex
    {
        EncryptionAlgorithmIndex = 0,
        KDFAlgorithmIndex=1,
        KDFInterationsIndex=2,
        KDFMemoryIndex=6,
        KDFParallelizationIndex=10,
        MACAlgorithmIndex=11,
        KDFSaltSizeIndex=12,
        IVSizeIndex=13,
        NumCipherSuiteParameterBytes=14
    }


#pragma warning disable CA1028 // Enum Storage should be Int32 - Ignore since we want this to be a byte
    public enum CipherSuite : byte
#pragma warning restore CA1028 // Enum Storage should be Int32
    {
        Unknown = 0,
        Aes256CfbPkcs7 = 1,
        Aes128CfbPkcs7 = 2,
        Rijndael256CbcPkcs7 = 3,
        Rijndael128CbcPkcs7 = 4,
        Aes256CbcPkcs7 = 5,
        Aes128CbcPkcs7 = 6,
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
        public EncryptionParameters(CipherSuite cipherSuite, Mac mac, KeyDerivationParameters keyDerivationParameters, int blocksize, int ivSize)
        {
            CipherSuite = cipherSuite;
            Mac = mac;
            KeyDerivationParameters = keyDerivationParameters;
            BlockSizeInBytes = blocksize;
            IvSizeInBytes = ivSize;
        }

        public CipherSuite CipherSuite { get; set; }
        public Mac Mac { get; set; }
        public KeyDerivationParameters KeyDerivationParameters { get; set; }
        public int BlockSizeInBytes { get; set; }
        public int IvSizeInBytes { get; set; }

    }
}

using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    public static class EncryptionHelpers
    {
        public static CipherMode GetCipherModeFromCipherSuite(CipherSuite suite)
        {
            CipherMode mode;

            switch (suite)
            {
                case CipherSuite.Aes128CfbPkcs7:
                case CipherSuite.Aes256CfbPkcs7:
                    mode = CipherMode.CFB;
                    break;
                case CipherSuite.Rijndael128CbcPkcs7:
                case CipherSuite.Rijndael256CbcPkcs7:
                case CipherSuite.Aes128CbcPkcs7:
                case CipherSuite.Aes256CbcPkcs7:
                    mode = CipherMode.CBC;
                    break;
                case CipherSuite.Unknown:
                default:
                    throw new Exception();
            }

            return mode;
        }

        public static int GetKeySizeFromCipherSuite(CipherSuite suite)
        {
            int size;

            switch (suite)
            {
                case CipherSuite.Aes128CfbPkcs7:
                case CipherSuite.Rijndael128CbcPkcs7:
                case CipherSuite.Aes128CbcPkcs7:
                    size = 16;
                    break;
                
                case CipherSuite.Aes256CfbPkcs7:
                case CipherSuite.Rijndael256CbcPkcs7:
                case CipherSuite.Aes256CbcPkcs7:
                    size = 32;
                    break;
                case CipherSuite.Unknown:
                default:
                    throw new Exception();
            }

            return size;
        }

        public static PaddingMode GetPaddingModeFromCipherSuite(CipherSuite suite)
        {
            PaddingMode mode;

            switch (suite)
            {
                case CipherSuite.Rijndael128CbcPkcs7:
                case CipherSuite.Rijndael256CbcPkcs7:
                case CipherSuite.Aes128CfbPkcs7:
                case CipherSuite.Aes256CfbPkcs7:
                case CipherSuite.Aes128CbcPkcs7:
                case CipherSuite.Aes256CbcPkcs7:
                    mode = PaddingMode.PKCS7;
                    break;
                case CipherSuite.Unknown:
                default:
                    throw new Exception();
            }

            return mode;
        }

        public static byte[] PackCipherParamsIntoBytes(EncryptionParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            byte[] bytes =
            {
                (byte)parameters.CipherSuite,
                (byte)parameters.KeyDerivationParameters.Algorithm,
                (byte)parameters.KeyDerivationParameters.Iterations,
                (byte)(parameters.KeyDerivationParameters.Iterations >> 8),
                (byte)(parameters.KeyDerivationParameters.Iterations >> 16),
                (byte)(parameters.KeyDerivationParameters.Iterations >> 24),
                (byte)parameters.KeyDerivationParameters.MemorySizeKb,
                (byte)(parameters.KeyDerivationParameters.MemorySizeKb >> 8),
                (byte)(parameters.KeyDerivationParameters.MemorySizeKb >> 16),
                (byte)(parameters.KeyDerivationParameters.MemorySizeKb >> 24),
                (byte)parameters.KeyDerivationParameters.DegreeOfParallelism,
                (byte)parameters.Mac,
                (byte)parameters.KeyDerivationParameters.SaltSizeBytes,
                (byte)parameters.IvSizeInBytes,

            };

            return bytes;
        }

        public static CipherSuite GetCipherSuiteFromPackedBytes(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            return (CipherSuite)bytes[(int)PackedCipherSuiteParametersIndex.EncryptionAlgorithmIndex];
        }

        public static Mac GetMacFromPackedBytes(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            return (Mac)bytes[(int)PackedCipherSuiteParametersIndex.MACAlgorithmIndex];
        }

        public static KeyDerivationAlgorithm GetKDFFromPackedBytes(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            return (KeyDerivationAlgorithm)bytes[(int)PackedCipherSuiteParametersIndex.KDFAlgorithmIndex];
        }

        public static UInt32 GetKDFIterationsFromPackedBytes(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            UInt32 keyderivationiterations = (UInt32)((bytes[((int)PackedCipherSuiteParametersIndex.KDFInterationsIndex) + 3] << 24) |
                (bytes[((int)PackedCipherSuiteParametersIndex.KDFInterationsIndex) + 2] << 16) |
                (bytes[((int)PackedCipherSuiteParametersIndex.KDFInterationsIndex) + 1] << 8) |
                bytes[(int)PackedCipherSuiteParametersIndex.KDFInterationsIndex]);

            return keyderivationiterations;
        }

        public static int GetKDFMemoryFromPackedBytes(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            int keyderivationmemory = (int)((bytes[((int)PackedCipherSuiteParametersIndex.KDFMemoryIndex) + 3] << 24) |
                (bytes[((int)PackedCipherSuiteParametersIndex.KDFMemoryIndex) + 2] << 16) |
                (bytes[((int)PackedCipherSuiteParametersIndex.KDFMemoryIndex) + 1] << 8) |
                bytes[(int)PackedCipherSuiteParametersIndex.KDFMemoryIndex]);

            return keyderivationmemory;
        }

        public static int GetKDFParallelizationFromPackedBytes(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            return (int)bytes[(int)PackedCipherSuiteParametersIndex.KDFParallelizationIndex];
        }

        public static int GetKDFSaltSizeInBytesFromPackedBytes(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            return (int)bytes[(int)PackedCipherSuiteParametersIndex.KDFSaltSizeIndex];
        }

        public static int GetIVSizeInBytesFromPackedBytes(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            return (int)bytes[(int)PackedCipherSuiteParametersIndex.IVSizeIndex];
        }
    }
}

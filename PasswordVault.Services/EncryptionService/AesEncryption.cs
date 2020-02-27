using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.CompilerServices;

/* Encryption/Decryption logic is based on recommended practice written 
 * by Microsoft.
 * https://docs.microsoft.com/en-us/dotnet/standard/security/vulnerabilities-cbc-mode 
 */

namespace PasswordVault.Services
{
    public class AesEncryption : IEncryptionService
    {
        /*CONSTANTS********************************************************/

        /*FIELDS***********************************************************/
        private EncryptionParameters _defaultParameters = new EncryptionParameters(
            EncryptionAlgorithm.Aes256CfbPkcs7,
            Mac.HMACSHA256,
            new KeyDerivationParameters(
                KeyDerivationAlgorithm.Argon2Id,
                32,
                16,
                10,
                8,
                1024 * 1024
            ),
            16,
            16
        );


        private EncryptionParameters _encryptionParameters;
        private IEncryptionIntegrity _integrityVerification;

        /*PROPERTIES*******************************************************/

        /*CONSTRUCTORS*****************************************************/
        public AesEncryption()
        {
            _encryptionParameters = _defaultParameters;
            _integrityVerification = new HmacIntegrity();
        }

        public AesEncryption(EncryptionParameters parameters)
        {
            _encryptionParameters = parameters;
            _integrityVerification = new HmacIntegrity();
        }

        /*PUBLIC METHODS***************************************************/
        public string Encrypt(string plainText, string passPhrase)
        {
            var salt = CryptographyHelper.GenerateRandomEntropy(_encryptionParameters.KeyDerivationParameters.SaltSizeBytes);
            var iv = CryptographyHelper.GenerateRandomEntropy(_encryptionParameters.IvSizeBytes);
            var plaintext = Encoding.UTF8.GetBytes(plainText);

            byte[] cipherSuite = 
            { 
                (byte)_encryptionParameters.Algorithm,
                (byte)_encryptionParameters.KeyDerivationParameters.Algorithm,
                (byte)_encryptionParameters.KeyDerivationParameters.Iterations,
                (byte)(_encryptionParameters.KeyDerivationParameters.Iterations >> 8),
                (byte)(_encryptionParameters.KeyDerivationParameters.Iterations >> 16),
                (byte)(_encryptionParameters.KeyDerivationParameters.Iterations >> 24),
                (byte)_encryptionParameters.KeyDerivationParameters.MemorySizeKb,
                (byte)(_encryptionParameters.KeyDerivationParameters.MemorySizeKb >> 8),
                (byte)(_encryptionParameters.KeyDerivationParameters.MemorySizeKb >> 16),
                (byte)(_encryptionParameters.KeyDerivationParameters.MemorySizeKb >> 24),
                (byte)_encryptionParameters.KeyDerivationParameters.DegreeOfParallelism,
                (byte)_encryptionParameters.Mac, 
                
            };
            byte[] authenticateHash;
            byte[] cipherBytes;
            byte[] cipherkey;
            byte[] hmackey;

            var keysize = _encryptionParameters.KeyDerivationParameters.KeySizeBytes;
            var hmacsize = _integrityVerification.GetHMACKeySizeInBits(_encryptionParameters.Mac).ToNumBytes();
            IKeyDerivation keyDerivation = KeyDerivationFactory.Get(_encryptionParameters.KeyDerivationParameters.Algorithm);
            var combinedKey = keyDerivation.DeriveKey(passPhrase, salt, _encryptionParameters.KeyDerivationParameters, keysize + hmacsize); // hash a key for both aes and hmac
            cipherkey = combinedKey.Take(keysize).ToArray();
            hmackey = combinedKey.Skip(keysize).Take(hmacsize).ToArray();
            
            using (SymmetricAlgorithm cipher = Aes.Create())
            {      
                cipher.BlockSize = _encryptionParameters.BlockSizeBytes.ToNumBits();

                switch(_encryptionParameters.Algorithm)
                {
                    case EncryptionAlgorithm.Aes128CfbPkcs7:
                    case EncryptionAlgorithm.Aes256CfbPkcs7:
                        cipher.Mode = CipherMode.CFB;
                        cipher.Padding = PaddingMode.PKCS7;
                        break;
                    case EncryptionAlgorithm.Rijndael128CbcPkcs7:
                    case EncryptionAlgorithm.Rijndael256CbcPkcs7:
                        cipher.Mode = CipherMode.CBC;
                        cipher.Padding = PaddingMode.PKCS7;
                        break;
                    case EncryptionAlgorithm.Unknown:
                    default:
                        throw new Exception();
                }
                
                using (ICryptoTransform encryptor = cipher.CreateEncryptor(cipherkey, iv))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(plaintext, 0, plaintext.Length);
                            cryptoStream.FlushFinalBlock();
                            cipherBytes = memoryStream.ToArray();
                        }
                    }                                      
                }

                authenticateHash = _integrityVerification.GenerateIntegrityHash((Mac)cipherSuite[(int)CipherSuiteIdx.MacAlg], hmackey, cipherSuite, salt, iv, cipherBytes);
            }

            int totalLength = cipherSuite.Length + authenticateHash.Length + salt.Length + iv.Length + cipherBytes.Length;
            byte[] combined = new byte[totalLength];
            int offset = 0;

            Buffer.BlockCopy(cipherSuite, 0, combined, offset, cipherSuite.Length);
            offset += cipherSuite.Length;
            Buffer.BlockCopy(authenticateHash, 0, combined, offset, authenticateHash.Length);
            offset += authenticateHash.Length;
            Buffer.BlockCopy(salt, 0, combined, offset, salt.Length);
            offset += salt.Length;
            Buffer.BlockCopy(iv, 0, combined, offset, iv.Length);
            offset += iv.Length;
            Buffer.BlockCopy(cipherBytes, 0, combined, offset, cipherBytes.Length);

            return Convert.ToBase64String(combined);           
        }

        public string Decrypt(string cipherText, string passPhrase)
        {
            string plaintext = "";
            var cipherRaw = Convert.FromBase64String(cipherText);

            // get parameters from cipher
            EncryptionAlgorithm encryptionalg = (EncryptionAlgorithm)cipherRaw[(int)CipherSuiteIdx.EncryptionAlg];
            Mac macalg = (Mac)cipherRaw[(int)CipherSuiteIdx.MacAlg];
            KeyDerivationAlgorithm keyderivationalg = (KeyDerivationAlgorithm)cipherRaw[(int)CipherSuiteIdx.KeyDevAlg];
            UInt32 keyderivationiterations = (UInt32)((cipherRaw[((int)CipherSuiteIdx.KeyDevItr)+3] << 24) | 
                (cipherRaw[((int)CipherSuiteIdx.KeyDevItr)+2] << 16) | 
                (cipherRaw[((int)CipherSuiteIdx.KeyDevItr)+1] << 8) | 
                cipherRaw[(int)CipherSuiteIdx.KeyDevItr]);
            int keyderivationmemory = (int)((cipherRaw[((int)CipherSuiteIdx.KeyDevMem) + 3] << 24) | 
                (cipherRaw[((int)CipherSuiteIdx.KeyDevMem) + 2] << 16) | 
                (cipherRaw[((int)CipherSuiteIdx.KeyDevMem) + 1] << 8) | 
                cipherRaw[(int)CipherSuiteIdx.KeyDevMem]);
            int keyderivationparallelism = (int)cipherRaw[(int)CipherSuiteIdx.KeyDevParallel];
  
            using (SymmetricAlgorithm cipher = Aes.Create())
            {
                int headerSizeInBytes = (int)CipherSuiteIdx.NumCipherSuiteBytes;
                int authSizeInBytes = _integrityVerification.GetHMACHashSizeInBits(macalg).ToNumBytes();
                int hmacKeySizeInBytes = _integrityVerification.GetHMACKeySizeInBits(macalg).ToNumBytes();
                int saltSizeInBytes = _encryptionParameters.KeyDerivationParameters.SaltSizeBytes; // use the configured salt size, should be 128 bits
                int ivSizeInBytes = cipher.BlockSize.ToNumBytes(); // iv should match blocksize in AES
                int blockSizeInBytes = cipher.BlockSize.ToNumBytes(); // 128 bits for AES               
                int keySizeInBytes = -1;
                switch (encryptionalg) // key size is based on the algorithm being used
                {
                    case EncryptionAlgorithm.Rijndael256CbcPkcs7:
                    case EncryptionAlgorithm.Aes256CfbPkcs7:
                        keySizeInBytes = 32;
                        break;
                    case EncryptionAlgorithm.Rijndael128CbcPkcs7:
                    case EncryptionAlgorithm.Aes128CfbPkcs7:
                        keySizeInBytes = 16;
                        break;
                    case EncryptionAlgorithm.Unknown:
                    default:
                        throw new CryptographicException();
                }

                int authOffset = headerSizeInBytes;
                int saltOffset = authOffset + authSizeInBytes;
                int ivOffset = saltOffset + saltSizeInBytes;
                int cipherOffset = ivOffset + ivSizeInBytes;
                int cipherLength = cipherRaw.Length - cipherOffset;
                int minLen = cipherOffset + blockSizeInBytes;

                byte[] salt = new byte[saltSizeInBytes];
                byte[] cipherkey;
                byte[] hmackey;
                Buffer.BlockCopy(cipherRaw, saltOffset, salt, 0, saltSizeInBytes);

                KeyDerivationParameters keyDevParams = new KeyDerivationParameters(keyderivationalg, 0, saltSizeInBytes, keyderivationiterations, keyderivationparallelism, keyderivationmemory);
                IKeyDerivation keyDerivation = KeyDerivationFactory.Get(keyderivationalg);
                var combinedKey = keyDerivation.DeriveKey(passPhrase, salt, keyDevParams, keySizeInBytes + hmacKeySizeInBytes);
                cipherkey = combinedKey.Take(keySizeInBytes).ToArray();
                hmackey = combinedKey.Skip(keySizeInBytes).Take(keySizeInBytes).ToArray();

                bool isVerified = _integrityVerification.VerifyIntegrity(macalg, hmackey, cipherRaw, saltSizeInBytes, ivSizeInBytes, blockSizeInBytes, headerSizeInBytes);

                if (!isVerified)
                {
                    throw new CryptographicException();
                }

                // Proceed with decryption
                byte[] iv = new byte[ivSizeInBytes];
                Buffer.BlockCopy(cipherRaw, ivOffset, iv, 0, ivSizeInBytes);
                cipher.BlockSize = blockSizeInBytes*8;

                switch (_encryptionParameters.Algorithm)
                {
                    case EncryptionAlgorithm.Aes128CfbPkcs7:
                    case EncryptionAlgorithm.Aes256CfbPkcs7:
                        cipher.Mode = CipherMode.CFB;
                        cipher.Padding = PaddingMode.PKCS7;
                        break;
                    case EncryptionAlgorithm.Rijndael128CbcPkcs7:
                    case EncryptionAlgorithm.Rijndael256CbcPkcs7:
                        cipher.Mode = CipherMode.CBC;
                        cipher.Padding = PaddingMode.PKCS7;
                        break;
                    case EncryptionAlgorithm.Unknown:
                    default:
                        throw new Exception();
                }

                using (ICryptoTransform decryptor = cipher.CreateDecryptor(cipherkey, iv))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write))
                        {
                            byte[] cipherBytes = new byte[cipherLength];
                            Buffer.BlockCopy(cipherRaw, cipherOffset, cipherBytes, 0, cipherLength);

                            cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);
                            cryptoStream.FlushFinalBlock();
                            byte[] plainbytes = memoryStream.ToArray();
                            plaintext  = Encoding.UTF8.GetString(plainbytes, 0, plainbytes.Length);
                        }
                    }
                }
            }
            return plaintext;
        }

        /*PRIVATE METHODS**************************************************/


        /*STATIC METHODS***************************************************/

    } // AesEncryption CLASS
}

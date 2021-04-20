using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.CompilerServices;
using PasswordVault.Utilities;

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
            CipherSuite.Aes256CbcPkcs7,
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
            var salt = CryptographyHelper.GenerateRandomEntropy(_encryptionParameters.KeyDerivationParameters.SaltSizeBytes); // dont allow configurable salt size for AES
            var iv = CryptographyHelper.GenerateRandomEntropy(_encryptionParameters.IvSizeInBytes); // dont allow configurable iv size for AES
            var plaintext = Encoding.UTF8.GetBytes(plainText);

            byte[] cipherSuite = EncryptionHelpers.PackCipherParamsIntoBytes(_encryptionParameters);
            byte[] authenticateHash;
            byte[] cipherBytes;
            byte[] cipherkey;
            byte[] hmackey;
            var keysize = _encryptionParameters.KeyDerivationParameters.KeySizeBytes; // dont allow configurable key size for AES
            var hmacsize = _integrityVerification.GetHMACKeySizeInBits(_encryptionParameters.Mac).ToNumBytes();

            // Derive keys for AES and HMAC
            IKeyDerivation keyDerivation = KeyDerivationFactory.Get(_encryptionParameters.KeyDerivationParameters.Algorithm);
            var combinedKey = keyDerivation.DeriveKey(passPhrase, salt, _encryptionParameters.KeyDerivationParameters, keysize + hmacsize);
            cipherkey = combinedKey.Take(keysize).ToArray();
            hmackey = combinedKey.Skip(keysize).Take(hmacsize).ToArray();
            
            // Encrypt data using AES
            using (SymmetricAlgorithm cipher = Aes.Create()) // Use Aes.Create to get the best implementation of AES
            {      
                cipher.BlockSize = _encryptionParameters.BlockSizeInBytes.ToNumBits();
                cipher.Mode = EncryptionHelpers.GetCipherModeFromCipherSuite(_encryptionParameters.CipherSuite);
                cipher.Padding = EncryptionHelpers.GetPaddingModeFromCipherSuite(_encryptionParameters.CipherSuite);
                
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

                // Perform MAC over all of the parameters & ciphertext to guarantee data is not tampered with at a later time
                // Encrypt + MAC recommended, especially for AES CBC mode, to prevent vulnerbilities
                authenticateHash = _integrityVerification.GenerateIntegrityHash((Mac)cipherSuite[(int)CipherSuiteParametersIndex.MACAlgorithmIndex], hmackey, cipherSuite, salt, iv, cipherBytes);
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
            CipherSuite encryptionalg = EncryptionHelpers.GetCipherSuiteFromPackedBytes(cipherRaw);
            Mac macalg = EncryptionHelpers.GetMacFromPackedBytes(cipherRaw);
            KeyDerivationAlgorithm keyderivationalg = EncryptionHelpers.GetKDFFromPackedBytes(cipherRaw);
            UInt32 keyderivationiterations = EncryptionHelpers.GetKDFIterationsFromPackedBytes(cipherRaw);
            int keyderivationmemory = EncryptionHelpers.GetKDFMemoryFromPackedBytes(cipherRaw);
            int keyderivationparallelism = EncryptionHelpers.GetKDFParallelizationFromPackedBytes(cipherRaw);


            using (SymmetricAlgorithm cipher = Aes.Create())
            {
                int headerSizeInBytes = (int)CipherSuiteParametersIndex.NumCipherSuiteParameterBytes;
                int authSizeInBytes = _integrityVerification.GetHMACHashSizeInBits(macalg).ToNumBytes();
                int hmacKeySizeInBytes = _integrityVerification.GetHMACKeySizeInBits(macalg).ToNumBytes();
                int saltSizeInBytes = EncryptionHelpers.GetKDFSaltSizeInBytesFromPackedBytes(cipherRaw);
                int ivSizeInBytes = EncryptionHelpers.GetIVSizeInBytesFromPackedBytes(cipherRaw);
                int blockSizeInBytes = cipher.BlockSize.ToNumBytes(); // 128 bits for AES               
                int keySizeInBytes = EncryptionHelpers.GetKeySizeFromCipherSuite(encryptionalg);

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
                cipher.Mode = EncryptionHelpers.GetCipherModeFromCipherSuite(encryptionalg);
                cipher.Padding = EncryptionHelpers.GetPaddingModeFromCipherSuite(encryptionalg);

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace PasswordVault.Services
{
    public class AesEncryption : IEncryptionService
    {
        /*CONSTANTS********************************************************/

        /*FIELDS***********************************************************/
        private EncryptionSizes _encryptionSizeDefaults = new EncryptionSizes(
            iterations: 2500,
            blockSize: 128,
            keySize: 256
        );

        private int _keySize;
        private int _ivSize;
        private int _blockSize;
        private int _derivationIterations;



        /*PROPERTIES*******************************************************/
        public EncryptionSizes EncryptionSizeDefaults
        {
            get
            {
                return _encryptionSizeDefaults;
            }
        }

        /*CONSTRUCTORS*****************************************************/
        public AesEncryption()
        {
            _keySize = _encryptionSizeDefaults.KeySize;
            _ivSize = _encryptionSizeDefaults.BlockSize;
            _blockSize = _encryptionSizeDefaults.BlockSize;
            _derivationIterations = _encryptionSizeDefaults.Iterations;
        }

        public AesEncryption(int keySize, int blockSize, int iterations)
        {
            _keySize = keySize;         
            _derivationIterations = iterations;
            _blockSize = blockSize;
            _ivSize = blockSize;
        }

        /*PUBLIC METHODS***************************************************/
        public string Encrypt(string plainText, string passPhrase)
        {
            var saltBytes = GenerateRandomEntropy(_ivSize);
            var ivBytes = GenerateRandomEntropy(_ivSize);
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltBytes, _derivationIterations))
            {
                var keyBytes = password.GetBytes(_keySize / 8);

                using (var aes = Aes.Create())
                {
                    aes.BlockSize = _blockSize;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    using (var encryptor = aes.CreateEncryptor(keyBytes, ivBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        public string Decrypt(string cipherText, string passPhrase)
        {
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(_ivSize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(_ivSize / 8).Take(_ivSize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((_ivSize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((_ivSize / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, _derivationIterations))
            {
                var keyBytes = password.GetBytes(_keySize / 8);

                using (var aes = Aes.Create())
                {
                    aes.BlockSize = _blockSize;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    using (var decryptor = aes.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }     

        /*PRIVATE METHODS**************************************************/
        private byte[] GenerateRandomEntropy(int bits)
        {
            if ((bits % 8) != 0)
            {
                throw new ArgumentException("Must be divisible by 8!", nameof(bits));
            }

            int numBytes = bits / 8;

            var randomBytes = new byte[numBytes]; 
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }

            return randomBytes;
        }

        /*STATIC METHODS***************************************************/

    } // AesEncryption CLASS
}

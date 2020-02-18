using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.CompilerServices;
using System.Diagnostics;

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
        private EncryptionSizes _encryptionSizeDefaults = new EncryptionSizes(
            iterations: 5000,
            blockSize: 128,
            keySize: 256
        );

        private int _keySize;
        private int _ivSize;
        private int _saltSize;
        private int _blockSize;
        private int _derivationIterations;

        private IEncryptionIntegrity _integrityVerification;

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
            _saltSize = _encryptionSizeDefaults.BlockSize;
            _blockSize = _encryptionSizeDefaults.BlockSize;
            _derivationIterations = _encryptionSizeDefaults.Iterations;

            _integrityVerification = new HmacIntegrity();
        }

        public AesEncryption(int keySize, int blockSize, int iterations)
        {
            _keySize = keySize;         
            _derivationIterations = iterations;
            _blockSize = blockSize;
            _ivSize = blockSize;
            _saltSize = blockSize;

            _integrityVerification = new HmacIntegrity();
        }

        /*PUBLIC METHODS***************************************************/
        public string Encrypt(string plainText, string passPhrase)
        {
            var salt = GenerateRandomEntropy(_ivSize);
            var iv = GenerateRandomEntropy(_ivSize);
            var plaintext = Encoding.UTF8.GetBytes(plainText);

            byte[] cipherSuite = { (byte)CipherSuite.Aes256CfbPkcs7, (byte)Mac.HMACSHA256 };
            byte[] authenticateHash;
            byte[] cipherBytes;
            byte[] cipherkey;
            byte[] hmackey;

            using (var password = new Rfc2898DeriveBytes(passPhrase, salt, _derivationIterations))
            {
                var keyBytes = password.GetBytes((_keySize * 2) / 8); // multiply key size by 2 since we need two keys

                cipherkey = keyBytes.Take(_keySize / 8).ToArray();
                hmackey = keyBytes.Skip(_keySize / 8).Take(_keySize / 8).ToArray();
            }

            /* It is advisable to use a key size that is at least the size of the hash method used, 
                * otherwise you may degrade the security margin provided by the HMAC method. There may 
                * be a minor performance penalty if the key size forces the hash algorithm to hash 
                * multiple blocks.
                * https://stackoverflow.com/questions/18080445/difference-between-hmacsha256-and-hmacsha512
            */
            using (SymmetricAlgorithm cipher = Aes.Create())
            {      
                cipher.BlockSize = _blockSize;
                cipher.Mode = CipherMode.CFB;
                cipher.Padding = PaddingMode.PKCS7;

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

                authenticateHash = _integrityVerification.GenerateIntegrityHash((Mac)cipherSuite[1], hmackey, cipherSuite, salt, iv, cipherBytes);
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
            var cipherRaw = Convert.FromBase64String(cipherText);
            CipherSuite cipherSuite = (CipherSuite)cipherRaw[0];
            Mac mac = (Mac)cipherRaw[1];

            string plaintext = "";

            using (SymmetricAlgorithm cipher = Aes.Create())
            {
                int headerSizeInBytes = 2;
                int authSizeInBytes = _integrityVerification.GetHMACHashSizeInBits(mac) / 8;
                int saltSizeInBytes = _saltSize / 8;
                int ivSizeInBytes = _ivSize / 8;
                int blockSizeInBytes = _blockSize / 8;

                int authOffset = headerSizeInBytes;
                int saltOffset = authOffset + authSizeInBytes;
                int ivOffset = saltOffset + saltSizeInBytes;
                int cipherOffset = ivOffset + ivSizeInBytes;
                int cipherLength = cipherRaw.Length - cipherOffset;
                int minLen = cipherOffset + blockSizeInBytes;

                byte[] salt = new byte[saltSizeInBytes];
                byte[] cipherkey;
                byte[] hmackey;
                Buffer.BlockCopy(cipherRaw, saltOffset, salt, 0, saltSizeInBytes); // Salt has an offset of 2 in raw cipher
                using (var password = new Rfc2898DeriveBytes(passPhrase, salt, _derivationIterations))
                {
                    var keyBytes = password.GetBytes((_keySize * 2) / 8); // multiply key size by 2 since we need two keys

                    cipherkey = keyBytes.Take(_keySize / 8).ToArray();
                    hmackey = keyBytes.Skip(_keySize / 8).Take(_keySize / 8).ToArray();
                }

                bool isVerified = _integrityVerification.VerifyIntegrity(mac, hmackey, cipherRaw, _saltSize, _ivSize, _blockSize);

                if (!isVerified)
                {
                    throw new CryptographicException();
                }

                // Proceed with decryption
                byte[] iv = new byte[_ivSize/8];
                Buffer.BlockCopy(cipherRaw, ivOffset, iv, 0, ivSizeInBytes);
                cipher.BlockSize = _blockSize;
                cipher.Mode = CipherMode.CFB;
                cipher.Padding = PaddingMode.PKCS7;

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

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static bool CryptographicEquals(
        byte[] a,
        int aOffset,
        byte[] b,
        int bOffset,
        int length)
        {
            Debug.Assert(a != null);
            Debug.Assert(b != null);
            Debug.Assert(length >= 0);

            int result = 0;

            if (a.Length - aOffset < length || b.Length - bOffset < length)
            {
                return false;
            }

            unchecked
            {
                for (int i = 0; i < length; i++)
                {
                    // Bitwise-OR of subtraction has been found to have the most
                    // stable execution time.
                    //
                    // This cannot overflow because bytes are 1 byte in length, and
                    // result is 4 bytes.
                    // The OR propagates all set bytes, so the differences are only
                    // present in the lowest byte.
                    result = result | (a[i + aOffset] - b[i + bOffset]);
                }
            }

            return result == 0;
        }

        /*STATIC METHODS***************************************************/

    } // AesEncryption CLASS
}

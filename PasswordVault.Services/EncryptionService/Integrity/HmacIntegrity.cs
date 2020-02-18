using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;

namespace PasswordVault.Services
{
    class HmacIntegrity : IEncryptionIntegrity
    {
        public byte[] GenerateIntegrityHash(Mac hmac, byte[] key, byte[] suiteBytes, byte[] saltBytes, byte[] ivBytes, byte[] cipherBytes)
        {
            byte[] output = null;

            HMAC auth = GetHMAC(hmac, key);

            auth.TransformBlock(suiteBytes, 0, suiteBytes.Length, null, 0);
            auth.TransformBlock(saltBytes, 0, saltBytes.Length, null, 0);
            auth.TransformBlock(ivBytes, 0, ivBytes.Length, null, 0);
            auth.TransformBlock(cipherBytes, 0, cipherBytes.Length, null, 0);
            auth.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
            output = auth.Hash;

            auth.Dispose();
            return output;
        }

        public bool VerifyIntegrity(Mac hmac, byte[] key, byte[] cipherBytes, int saltSizeInBits, int ivSizeInBits, int blockSizeInBits)
        {
            bool verified = false;

            HMAC auth = GetHMAC(hmac, key);

            int headerSizeInBytes = 2;
            int authSizeInBytes = auth.HashSize / 8;
            int saltSizeInBytes = saltSizeInBits / 8;
            int ivSizeInBytes = ivSizeInBits / 8;
            int blockSizeInBytes = blockSizeInBits / 8;

            int authOffset = headerSizeInBytes;
            int saltOffset = authOffset + authSizeInBytes;
            int ivOffset = saltOffset + saltSizeInBytes;
            int cipherOffset = ivOffset + ivSizeInBytes;
            int cipherLength = cipherBytes.Length - cipherOffset;
            int minLen = cipherOffset + blockSizeInBytes;

            if (cipherBytes.Length < minLen)
            {
                throw new CryptographicException();
            }

            auth.Key = key;
            auth.TransformBlock(cipherBytes, 0, headerSizeInBytes, null, 0); // suite
            auth.TransformBlock(cipherBytes, saltOffset, saltSizeInBytes, null, 0); // salt
            auth.TransformBlock(cipherBytes, ivOffset, ivSizeInBytes, null, 0); // iv
            auth.TransformBlock(cipherBytes, cipherOffset, cipherLength, null, 0); // cipher
            auth.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
            byte[] genAuth = auth.Hash;

            if (CryptographicEquals(genAuth, 0, cipherBytes, authOffset, authSizeInBytes))
            {
                verified = true;
            }
            else
            {
                verified = false;
            }

            auth.Dispose();

            return verified;
        }

        public int GetHMACHashSizeInBits(Mac mac)
        {
            int size = 0;
            var temp = GetHMAC(mac, Array.Empty<byte>());
            size = temp.HashSize;
            temp.Dispose();
            return size;
        }

        private HMAC GetHMAC(Mac hmac, byte[] hmacKey)
        {
            HMAC returnHmac;

            switch(hmac)
            {
                case Mac.HMACSHA256:
                    returnHmac = new HMACSHA256(hmacKey);
                    break;

                case Mac.HMACSHA512:
                    returnHmac = new HMACSHA512(hmacKey);
                    break;

                default:
                    throw new Exception("Not supported HMAC type!");
            }

            return returnHmac;
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static bool CryptographicEquals(byte[] a,
                                                int aOffset,
                                                byte[] b,
                                                int bOffset,
                                                int length)
        {
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
    }
}

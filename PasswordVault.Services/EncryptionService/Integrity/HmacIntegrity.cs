using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;
using System.Globalization;

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

        public bool VerifyIntegrity(Mac hmac, byte[] key, byte[] cipherBytes, int saltSizeInBytes, int ivSizeInBytes, int blockSizeInBytes, int headerSizeInBytes)
        {
            bool verified = false;

            HMAC auth = GetHMAC(hmac, key);

            int authSizeInBytes = auth.HashSize.ToNumBytes();

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

            if (CryptographyHelper.CryptographicEquals(genAuth, 0, cipherBytes, authOffset, authSizeInBytes))
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

        public int GetHMACKeySizeInBits(Mac mac)
        {
            /* It is advisable to use a key size that is at least the size of the hash method used, 
                * otherwise you may degrade the security margin provided by the HMAC method. There may 
                * be a minor performance penalty if the key size forces the hash algorithm to hash 
                * multiple blocks.
                * https://stackoverflow.com/questions/18080445/difference-between-hmacsha256-and-hmacsha512
            */
            int keysize = -1;

            switch (mac)
            {          
                case Mac.HMACSHA256:
                    keysize = 256;
                    break;
                case Mac.HMACSHA512:
                    keysize = 512;
                    break;
                case Mac.Unknown:
                default:
                    throw new Exception();
            }

            return keysize;
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
                    throw new CryptographicException();
            }

            return returnHmac;
        }
    }
}

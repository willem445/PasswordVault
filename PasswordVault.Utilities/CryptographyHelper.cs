using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace PasswordVault.Utilities
{
    public static class CryptographyHelper
    {
        public static byte[] GenerateRandomEntropy(int numBytes)
        {
            var randomBytes = new byte[numBytes];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }

        public static string ToBase64(this byte[] bytes)
        {
            string result = Convert.ToBase64String(bytes);
            return result;
        }

        public static byte[] ToBytes(this string base64String)
        {
            byte[] result = Convert.FromBase64String(base64String);
            return result;
        }

        public static int ToNumBytes(this int numbits)
        {
            int numbytes = numbits / 8;
            return numbytes;
        }

        public static int ToNumBits(this int numbytes)
        {
            int numbits = numbytes * 8;
            return numbits;
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static bool CryptographicEquals(
        byte[] a,
        int aOffset,
        byte[] b,
        int bOffset,
        int length)
        {
            if (a == null || b == null)
                throw new CryptographicException();

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

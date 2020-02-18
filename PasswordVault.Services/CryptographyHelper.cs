using System;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;

namespace PasswordVault.Services
{
    static class CryptographyHelper
    {
        static byte[] GenerateRandomEntropy(int bits)
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
        public static bool CryptographicEquals(
        byte[] a,
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

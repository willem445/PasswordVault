using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace PasswordVault.Services
{
    public static class KeyGenerator
    {
        /*CONSTANTS********************************************************/

        /*FIELDS***********************************************************/
        internal static readonly char[] chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*-_=+?".ToCharArray();

        /*PROPERTIES*******************************************************/

        /*CONSTRUCTORS*****************************************************/

        /*PUBLIC METHODS***************************************************/
        /// <summary>
        /// Generates a random key of given length.
        /// </summary>
        /// <param name="length">Desired length of key.</param>
        /// <returns>Random key string.</returns>
        public static string GetUniqueKey(int length)
        {
            byte[] data = new byte[4 * length];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % chars.Length;

                result.Append(chars[idx]);
            }

            return result.ToString();
        }

        /*PRIVATE METHODS**************************************************/

        /*STATIC METHODS***************************************************/

    } // KeyGenerator CLASS
}

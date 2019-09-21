using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

/*=================================================================================================
DESCRIPTION
*================================================================================================*/
/* 
 ------------------------------------------------------------------------------------------------*/

namespace PasswordVault
{
    /*=================================================================================================
	ENUMERATIONS
	*================================================================================================*/

    /*=================================================================================================
	STRUCTS
	*================================================================================================*/
    public struct CryptData_S
    {
        public CryptData_S(string salt, string hash, int iterations)
        {
            Salt = salt;
            Hash = hash;
            Iterations = iterations;
        }

        public string Salt { get; }
        public string Hash { get; }
        public int Iterations { get; }
    }

    /*=================================================================================================
	CLASSES
	*================================================================================================*/
    class MasterPassword : IMasterPassword
    {
        /*=================================================================================================
		CONSTANTS
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/
        

        /*=================================================================================================
		FIELDS
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/
        private CryptData_S _cryptData;

        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/
        public int _hashIterationCount { get; } = 1000;
        public int _saltArraySize { get; } = 24;
        public int _hashArraySize { get; } = 24;

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public MasterPassword()
        {
            // Use defaults
        }

        public MasterPassword(int iterations, int saltArraySize, int hashArraySize)
        {
            _hashIterationCount = iterations;
            _saltArraySize = saltArraySize;
            _hashArraySize = hashArraySize;
        }

        /*=================================================================================================
		PUBLIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        public CryptData_S HashPassword(string password)
        {
            // SALT
            RNGCryptoServiceProvider saltCellar = new RNGCryptoServiceProvider();
            byte[] salt = new byte[_saltArraySize];
            saltCellar.GetBytes(salt);

            string saltString = Convert.ToBase64String(salt);

            // HASH
            Rfc2898DeriveBytes hashTool = new Rfc2898DeriveBytes(password, salt);
            hashTool.IterationCount = _hashIterationCount;
            byte[] hash = hashTool.GetBytes(_hashArraySize);

            string hashString = Convert.ToBase64String(hash);
            int iterations = _hashIterationCount;

            return new CryptData_S(saltString, hashString, iterations);
        }

        /*************************************************************************************************/
        public string GetFormattedString()
        {
            string formatted = "";

            formatted = string.Format("{0}:{1}:{2}", _cryptData.Iterations, _cryptData.Salt, _cryptData.Hash);

            return formatted;
        }

        /*************************************************************************************************/
        public bool VerifyPassword(string password, string salt, string hash)
        {
            byte[] originalSalt = Convert.FromBase64String(salt);
            byte[] originalHash = Convert.FromBase64String(hash);
            Rfc2898DeriveBytes hashTool = new Rfc2898DeriveBytes(password, originalSalt);
            hashTool.IterationCount = _hashIterationCount;
            byte[] newHash = hashTool.GetBytes(_hashArraySize);

            uint differences = (uint)originalHash.Length ^ (uint)newHash.Length;
            for (int position = 0; position < Math.Min(originalHash.Length,
              newHash.Length); position++)
                differences |= (uint)(originalHash[position] ^ newHash[position]);
            bool passwordMatches = (differences == 0);

            return passwordMatches;
        }

        /*************************************************************************************************/
        public string GenerateRandomKey()
        {
            string token;

            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[32];
                rng.GetBytes(tokenData);

                token = Convert.ToBase64String(tokenData);
            }

            return token;
        }

        /*=================================================================================================
		PRIVATE METHODS
		*================================================================================================*/
        /*************************************************************************************************/

        /*=================================================================================================
		STATIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/

    } // UserPasswordHash CLASS
} // PasswordHashTest NAMESPACE

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
        public string Salt;
        public string Hash;
        public int Iterations;
    }

    /*=================================================================================================
	CLASSES
	*================================================================================================*/
    class UserPasswordHash
    {
        /*=================================================================================================
		CONSTANTS
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/
        const int HASH_ITERATION_COUNT = 1000;
        const int SALT_ARRAY_SIZE = 24;
        const int HASH_ARRAY_SIZE = 24;

        /*=================================================================================================
		FIELDS
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/
        CryptData_S _cryptData;

        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/

        /*PRIVATE*****************************************************************************************/

        /*=================================================================================================
		CONSTRUCTORS
		*================================================================================================*/
        public UserPasswordHash()
        {

        }

        /*=================================================================================================
		PUBLIC METHODS
		*================================================================================================*/
        /*************************************************************************************************/
        public CryptData_S HashPassword(string password)
        {
            _cryptData = new CryptData_S();

            // SALT
            RNGCryptoServiceProvider saltCellar = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SALT_ARRAY_SIZE];
            saltCellar.GetBytes(salt);

            _cryptData.Salt = Convert.ToBase64String(salt);

            // HASH
            Rfc2898DeriveBytes hashTool = new Rfc2898DeriveBytes(password, salt);
            hashTool.IterationCount = HASH_ITERATION_COUNT;
            byte[] hash = hashTool.GetBytes(HASH_ARRAY_SIZE);

            _cryptData.Hash = Convert.ToBase64String(hash);
            _cryptData.Iterations = HASH_ITERATION_COUNT;

            return _cryptData;
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
            hashTool.IterationCount = HASH_ITERATION_COUNT;
            byte[] newHash = hashTool.GetBytes(HASH_ARRAY_SIZE);

            uint differences = (uint)originalHash.Length ^ (uint)newHash.Length;
            for (int position = 0; position < Math.Min(originalHash.Length,
              newHash.Length); position++)
                differences |= (uint)(originalHash[position] ^ newHash[position]);
            bool passwordMatches = (differences == 0);

            return passwordMatches;
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

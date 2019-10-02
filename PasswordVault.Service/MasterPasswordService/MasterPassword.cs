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

namespace PasswordVault.Service
{
    /*=================================================================================================
	ENUMERATIONS
	*================================================================================================*/

    /*=================================================================================================
	STRUCTS
	*================================================================================================*/
    public struct UserEncrypedData
    {
        public UserEncrypedData(string salt, string hash, int iterations, string uniqueGUID, string randomGeneratedKey)
        {
            Salt = salt;
            Hash = hash;
            Iterations = iterations;
            UniqueGUID = uniqueGUID;
            RandomGeneratedKey = randomGeneratedKey;
        }

        public string Salt { get; }
        public string Hash { get; }
        public int Iterations { get; }
        public string UniqueGUID { get; }
        public string RandomGeneratedKey { get; }
    }

    /*=================================================================================================
	CLASSES
	*================================================================================================*/
    public class MasterPassword : IMasterPassword
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

        /*=================================================================================================
		PROPERTIES
		*================================================================================================*/
        /*PUBLIC******************************************************************************************/
        public int _hashIterationCount { get; } = 10000;
        public int _saltArraySize { get; } = 32;
        public int _hashArraySize { get; } = 32;

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
        public UserEncrypedData GenerateNewUserEncryptedDataFromPassword(string password)
        {
            // Salt
            RNGCryptoServiceProvider saltCellar = new RNGCryptoServiceProvider();
            byte[] salt = new byte[_saltArraySize];
            saltCellar.GetBytes(salt);
            string saltString = Convert.ToBase64String(salt);

            // Hash
            Rfc2898DeriveBytes hashTool = new Rfc2898DeriveBytes(password, salt);
            hashTool.IterationCount = _hashIterationCount;
            byte[] hash = hashTool.GetBytes(_hashArraySize);
            string hashString = Convert.ToBase64String(hash);

            // Iterations
            int iterations = _hashIterationCount;

            // Guid
            var uniqueID = Guid.NewGuid().ToString();

            // Random Key
            string randomGeneratedKey = GenerateRandomKey();

            return new UserEncrypedData(saltString, hashString, iterations, uniqueID, randomGeneratedKey);
        }

        /*************************************************************************************************/
        public string GetFormattedString(UserEncrypedData data)
        {
            string formatted = "";

            formatted = string.Format("{0},{1},{2},{3},{4}", data.UniqueGUID, data.RandomGeneratedKey, data.Iterations, data.Salt, data.Hash);

            return formatted;
        }

        /*************************************************************************************************/
        public bool VerifyPassword(string password, string salt, string hash, int iterationCount)
        {
            byte[] originalSalt = Convert.FromBase64String(salt);
            byte[] originalHash = Convert.FromBase64String(hash);
            Rfc2898DeriveBytes hashTool = new Rfc2898DeriveBytes(password, originalSalt);
            hashTool.IterationCount = iterationCount;
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
                byte[] tokenData = new byte[64];
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
} // PasswordVault.Service NAMESPACE

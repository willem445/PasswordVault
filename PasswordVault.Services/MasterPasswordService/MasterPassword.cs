using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using PasswordVault.Utilities;

namespace PasswordVault.Services
{
    public class MasterPassword : IMasterPassword
    {
        public MasterPassword()
        {

        }

        public UserEncrypedData GenerateMasterHash(string password, MasterPasswordParameters parameters)
        {
            if (parameters is null)
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "{0} cannot be null.", (nameof(parameters))));

            IKeyDerivation hasher = KeyDerivationFactory.Get(parameters.KeyDerivationParameters.Algorithm);

            var salt = CryptographyHelper.GenerateRandomEntropy(parameters.KeyDerivationParameters.SaltSizeBytes);
            var hash = hasher.DeriveKey(
                password,
                salt,
                parameters.KeyDerivationParameters
            );

            var saltString = salt.ToBase64();
            var hashString = hash.ToBase64();          
            var randomGeneratedKey = CryptographyHelper.GenerateRandomEntropy(parameters.RandomKeySize).ToBase64();

            return new UserEncrypedData(
                parameters.KeyDerivationParameters.Algorithm,
                parameters.KeyDerivationParameters.KeySizeBytes,
                saltString,
                parameters.KeyDerivationParameters.SaltSizeBytes,
                hashString,
                parameters.KeyDerivationParameters.Iterations,
                parameters.KeyDerivationParameters.DegreeOfParallelism,
                parameters.KeyDerivationParameters.MemorySizeKb,
                randomGeneratedKey
            );
        }

        public bool VerifyPassword(string password, string salt, string hash, MasterPasswordParameters parameters)
        {
            if (parameters is null)
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "{0} cannot be null.", (nameof(parameters))));

            IKeyDerivation hasher = KeyDerivationFactory.Get(parameters.KeyDerivationParameters.Algorithm);
            var saltBytes = salt.ToBytes();
            var hashBytes = hash.ToBytes();

            var verify = hasher.DeriveKey(
                password,
                saltBytes,
                parameters.KeyDerivationParameters
            );

            return CryptographyHelper.CryptographicEquals(hashBytes, 0, verify, 0, hashBytes.Length);
        }

        /// <summary>
        /// Flattens UserEncryptedData to be stored in a database.
        /// </summary>
        /// <param name="parameters">Masterpassword parameters to flatten.</param>
        /// <returns>flattened string</returns>
        public string FlattenHash(UserEncrypedData parameters)
        {
            string flattened = string.Format(CultureInfo.CurrentCulture, "{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}",
                ((byte)parameters.KeyDevAlgorithm).ToString(CultureInfo.CurrentCulture),
                parameters.KeySize.ToString(CultureInfo.CurrentCulture),
                parameters.SaltSize.ToString(CultureInfo.CurrentCulture),
                parameters.Iterations.ToString(CultureInfo.CurrentCulture),
                parameters.MemorySize.ToString(CultureInfo.CurrentCulture),
                parameters.DegreeOfParallelism.ToString(CultureInfo.CurrentCulture),
                parameters.Salt,
                parameters.Hash);
            return flattened;
        }

        /// <summary>
        /// Extracts UserEncryptedData from string stored in database. RandomGeneratedKey 
        /// are set to null since they are not stored in the database field and not needed for
        /// password validation.
        /// </summary>
        /// <param name="hash">String stored in database.</param>
        /// <returns>UserEncryptedData</returns>
        public UserEncrypedData UnFlattenHash(string hash)
        {
            if (string.IsNullOrEmpty(hash))
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "{0} cannot be null or empty!.", (nameof(hash))));

            var raw = hash.Split(':');
            UserEncrypedData data = new UserEncrypedData(
                alg :                 (KeyDerivationAlgorithm)Convert.ToInt32(raw[0], CultureInfo.CurrentCulture),
                keysize :             Convert.ToInt32(raw[1], CultureInfo.CurrentCulture),
                salt :                raw[6], 
                saltsize :            Convert.ToInt32(raw[2], CultureInfo.CurrentCulture),
                hash :                raw[7], 
                iterations :          Convert.ToUInt32(raw[3], CultureInfo.CurrentCulture), 
                degreeOfParallelism : Convert.ToInt32(raw[5], CultureInfo.CurrentCulture), 
                memorySize :          Convert.ToInt32(raw[4], CultureInfo.CurrentCulture), 
                randomGeneratedKey :  null);  

            return data;
        }
    }
}

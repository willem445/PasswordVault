using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PasswordVault.Services
{
    public class MasterPassword : IMasterPassword
    {
         public MasterPassword()
        {

        }

        public UserEncrypedData GenerateNewUserEncryptedDataFromPassword(string password, MasterPasswordParameters parameters)
        {
            IKeyDerivation hasher = KeyDerivationFactory.Get(parameters.KeyDerivationParameters.Algorithm);

            var salt = CryptographyHelper.GenerateRandomEntropy(parameters.KeyDerivationParameters.SaltSizeBytes);
            var hash = hasher.DeriveKey(
                password,
                salt,
                parameters.KeyDerivationParameters
            );

            var saltString = salt.ToBase64();
            var hashString = hash.ToBase64();
            var uuid = Guid.NewGuid().ToString();
            var randomGeneratedKey = CryptographyHelper.GenerateRandomEntropy(parameters.RandomKeySize).ToBase64();

            return new UserEncrypedData(
                parameters.KeyDerivationParameters.Algorithm,
                parameters.KeyDerivationParameters.KeySizeBytes,
                saltString,
                hashString,
                parameters.KeyDerivationParameters.Iterations,
                parameters.KeyDerivationParameters.DegreeOfParallelism,
                parameters.KeyDerivationParameters.MemorySize,
                uuid,
                randomGeneratedKey
            );
        }

        public bool VerifyPassword(string password, string salt, string hash, MasterPasswordParameters parameters)
        {
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

        public string FlattenHash(UserEncrypedData parameters)
        {
            string flattened = "";

            flattened = string.Format(CultureInfo.CurrentCulture, "{0}:{1}:{2}:{3}:{4}:{5}:{6}",
                            ((byte)parameters.KeyDevAlgorithm).ToString(CultureInfo.CurrentCulture),
                            parameters.KeySize.ToString(CultureInfo.CurrentCulture),
                            parameters.Iterations.ToString(CultureInfo.CurrentCulture),
                            parameters.MemorySize.ToString(CultureInfo.CurrentCulture),
                            parameters.DegreeOfParallelism.ToString(CultureInfo.CurrentCulture),
                            parameters.Salt,
                            parameters.Hash);

            return flattened;
        }

        public UserEncrypedData ExtractParameters(string hash)
        {
            var raw = hash.Split(':');
            UserEncrypedData data = new UserEncrypedData(
                (KeyDerivationAlgorithm)Convert.ToInt32(raw[0], CultureInfo.CurrentCulture),
                Convert.ToInt32(raw[1], CultureInfo.CurrentCulture),
                raw[5], 
                raw[6], 
                Convert.ToInt32(raw[2], CultureInfo.CurrentCulture), 
                Convert.ToInt32(raw[4], CultureInfo.CurrentCulture), 
                Convert.ToInt32(raw[3], CultureInfo.CurrentCulture), 
                null, 
                null);  

            return data;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
using System.Linq;

namespace PasswordVault.Services
{
    public class Argon2MasterPassword : IMasterPassword
    {
        public int _saltArraySize { get; } = 16;
        public int _hashArraySize { get; } = 32;
        public int _iterations { get; } = 50;
        public int _degreeOfParallelism { get; } = 8;
        public int _memorySize = 1024 * 128; // 128 mb


        public UserEncrypedData GenerateNewUserEncryptedDataFromPassword(string password)
        {
            var salt = CreateSalt();
            byte[] hash = HashPassword(password, salt);

            string hashString = Convert.ToBase64String(hash);
            string saltString = Convert.ToBase64String(salt);
            var uniqueID = Guid.NewGuid().ToString();
            string randomGeneratedKey = GenerateRandomKey();

            return new UserEncrypedData(saltString, hashString, _iterations, _degreeOfParallelism, _memorySize, uniqueID, randomGeneratedKey);
        }

        public bool VerifyPassword(string password, string salt, string hash, int iterationCount)
        {
            byte[] originalSalt = Convert.FromBase64String(salt);
            byte[] originalHash = Convert.FromBase64String(hash);

            byte[] verifyHash = HashPassword(password, originalSalt);

            return originalHash.SequenceEqual(verifyHash);
        }

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

        private byte[] HashPassword(string password, byte[] salt)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));
            argon2.Salt = salt;
            argon2.DegreeOfParallelism = _degreeOfParallelism;
            argon2.Iterations = _iterations;
            argon2.MemorySize = _memorySize;
            byte[] hash = argon2.GetBytes(_hashArraySize);
            return hash;
        }

        private byte[] CreateSalt()
        {
            // Salt
            RNGCryptoServiceProvider saltCellar = new RNGCryptoServiceProvider();
            byte[] salt = new byte[_saltArraySize];
            saltCellar.GetBytes(salt);
            string saltString = Convert.ToBase64String(salt);
            saltCellar.Dispose();

            return salt;
        }
    }
}

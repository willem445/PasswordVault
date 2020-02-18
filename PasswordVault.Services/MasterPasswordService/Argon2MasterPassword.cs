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
        public int SaltArraySize { get; } = 16;
        public int HashArraySize { get; } = 32;
        public int Iterations { get; } = 20;
        public int DegreeOfParallelism { get; } = 8;
        public int MemorySize { get; } = 1024 * 1024; // 1 gb

        private int _randomKeySize { get; } = 64;

        public Argon2MasterPassword()
        {
            // Use default settings
        }

        public Argon2MasterPassword(int iterations, int degreeOfParallelism, int memSize)
        {
            Iterations = iterations;
            DegreeOfParallelism = degreeOfParallelism;
            MemorySize = memSize;
        }

        public Argon2MasterPassword(int saltSize, int hashSize, int iterations, int degreeOfParallelism, int memSize)
        {
            SaltArraySize = saltSize;
            HashArraySize = hashSize;
            Iterations = iterations;
            DegreeOfParallelism = degreeOfParallelism;
            MemorySize = memSize;
        }

        public UserEncrypedData GenerateNewUserEncryptedDataFromPassword(string password)
        {
            var salt = CreateSalt();
            byte[] hash = HashPassword(password, salt);

            string hashString = Convert.ToBase64String(hash);
            string saltString = Convert.ToBase64String(salt);
            var uniqueID = Guid.NewGuid().ToString();
            string randomGeneratedKey = GenerateRandomKey(_randomKeySize);

            return new UserEncrypedData(saltString, hashString, Iterations, DegreeOfParallelism, MemorySize, uniqueID, randomGeneratedKey);
        }

        public bool VerifyPassword(string password, string salt, string hash, int iterationCount)
        {
            byte[] originalSalt = Convert.FromBase64String(salt);
            byte[] originalHash = Convert.FromBase64String(hash);

            byte[] verifyHash = HashPassword(password, originalSalt);

            return originalHash.SequenceEqual(verifyHash);
        }

        public string GenerateRandomKey(int sizeInBytes)
        {
            string token;

            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[sizeInBytes];
                rng.GetBytes(tokenData);

                token = Convert.ToBase64String(tokenData);
            }

            return token;
        }

        private byte[] HashPassword(string password, byte[] salt)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));
            argon2.Salt = salt;
            argon2.DegreeOfParallelism = DegreeOfParallelism;
            argon2.Iterations = Iterations;
            argon2.MemorySize = MemorySize;
            byte[] hash = argon2.GetBytes(HashArraySize);
            argon2.Reset();
            argon2.Dispose();
            GC.Collect();
            return hash;
        }

        private byte[] CreateSalt()
        {
            // Salt
            RNGCryptoServiceProvider saltCellar = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SaltArraySize];
            saltCellar.GetBytes(salt);
            string saltString = Convert.ToBase64String(salt);
            saltCellar.Dispose();

            return salt;
        }
    }
}

using System;
using System.Text;
using Konscious.Security.Cryptography;

namespace PasswordVault.Services
{
    class Argon2IdKeyDerivation : IKeyDerivation
    {
        public byte[] DeriveKey(string password, byte[] salt, KeyDerivationParameters parameters, int keySizeInBytes=0)
        {
            int keySize = 0;

            if (keySizeInBytes != 0)
            {
                keySize = keySizeInBytes;
            }
            else
            {
                keySize = parameters.KeySizeBytes;
            }

            byte[] key = new byte[keySize];
            using (var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password)))
            {
                argon2.Salt = salt;
                argon2.DegreeOfParallelism = parameters.DegreeOfParallelism;
                argon2.Iterations = parameters.Iterations;
                argon2.MemorySize = parameters.MemorySize;
                key = argon2.GetBytes(keySize);
            }           

            return key;
        }
    }
}

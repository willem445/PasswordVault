using System;
using System.Text;
using Konscious.Security.Cryptography;

namespace PasswordVault.Services
{
    class Argon2IdKeyDerivation : IKeyDerivation
    {
        public byte[] DeriveKey(string password, byte[] salt, int numBytes, KeyDerivationParameters parameters)
        {
            byte[] key = new byte[numBytes];

            using (var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password)))
            {
                argon2.Salt = salt;
                argon2.DegreeOfParallelism = parameters.DegreeOfParallelism;
                argon2.Iterations = parameters.Iterations;
                argon2.MemorySize = parameters.MemorySize;
                key = argon2.GetBytes(numBytes);
            }
                
            return key;
        }
    }
}

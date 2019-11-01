using System;
using System.Collections.Generic;

namespace PasswordVault.Services
{
    public interface IEncryptionService
    {
        int Iterations { get; }
        string Encrypt(string plainText, string passPhrase);
        string Decrypt(string cipherText, string passPhrase);
        string CreateKey(int keyLength);
    }
}

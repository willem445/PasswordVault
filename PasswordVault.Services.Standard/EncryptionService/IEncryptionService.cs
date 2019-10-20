using System;
using System.Collections.Generic;

namespace PasswordVault.Services.Standard
{
    public interface IEncryptionService
    {
        int Iterations { get; }
        string Encrypt(string plainText, string passPhrase);
        string Decrypt(string cipherText, string passPhrase);
        string CreateKey(int keyLength);
    }
}

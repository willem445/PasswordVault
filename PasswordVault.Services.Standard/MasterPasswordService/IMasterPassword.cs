using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services.Standard
{
    public interface IMasterPassword
    {
        UserEncrypedData GenerateNewUserEncryptedDataFromPassword(string password);
        string GetFormattedString(UserEncrypedData data);
        bool VerifyPassword(string password, string salt, string hash, int iterationCount);
        string GenerateRandomKey();
    }
}

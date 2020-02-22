using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    interface IEncryptionIntegrity
    {
        byte[] GenerateIntegrityHash(Mac hmac, byte[] key, byte[] suiteBytes, byte[] saltBytes, byte[] ivBytes, byte[] cipherBytes);
        bool VerifyIntegrity(Mac hmac, byte[] key, byte[] cipherBytes, int saltSizeInBits, int ivSizeInBits, int blockSizeInBits, int headerSizeInBytes);
        int GetHMACHashSizeInBits(Mac mac);

        int GetHMACKeySizeInBits(Mac mac);
    }
}

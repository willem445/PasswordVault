using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    public interface IEncryptionServiceFactory
    {
        IEncryptionService GetEncryptionService(EncryptionParameters parameters);
    }
}

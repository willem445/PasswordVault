using System;
using System.Collections.Generic;
using System.Text;
using PasswordVault.Models;

namespace PasswordVault.Services
{
    public interface IEncryptionConversion
    {
        void Convert(User user, string password, EncryptionServiceParameters originalParams, EncryptionServiceParameters newParams);
    }
}

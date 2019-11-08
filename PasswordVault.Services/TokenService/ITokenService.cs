using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    public interface ITokenService
    {
        string GenerateJwtToken(string userUuid);
    }
}

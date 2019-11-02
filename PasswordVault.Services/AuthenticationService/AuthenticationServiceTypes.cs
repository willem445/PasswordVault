using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PasswordVault.Models;

namespace PasswordVault.Services
{
    public enum AuthenticateResult
    {
        PasswordIncorrect,
        UsernameDoesNotExist,
        Successful,
        Failed
    }

    public struct AuthenticateReturn
    {
        public AuthenticateReturn(AuthenticateResult result, User user)
        {
            Result = result;
            User = user;
        }

        AuthenticateResult Result { get; }
        User User { get; }
    }
}

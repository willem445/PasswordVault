﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace PasswordVault.Services
{
    public enum AuthenticateResult
    {
        PasswordIncorrect,
        UsernameDoesNotExist,
        Successful,
        Failed
    }
}

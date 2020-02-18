using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    interface IKeyDerivationFactory
    {
        IKeyDerivation Get(CipherSuite suite);
    }
}

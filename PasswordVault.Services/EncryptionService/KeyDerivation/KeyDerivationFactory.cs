using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    static class KeyDerivationFactory
    {
        public static IKeyDerivation Get(CipherSuite suite)
        {
            IKeyDerivation keyDerivation = null;

            switch (suite)
            {           
                case CipherSuite.Aes256CfbPkcs7Pbkdf2:
                    keyDerivation = new PBKDF2KeyDerivation();
                    break;

                case CipherSuite.Aes128CfbPkcs7Pbkdf2:
                    keyDerivation = new PBKDF2KeyDerivation();
                    break;

                case CipherSuite.Aes128CfbPkcs7Argon2Id:
                    keyDerivation = new Argon2IdKeyDerivation();
                    break;

                case CipherSuite.Unknown:
                default:
                    throw new Exception();
            }

            return keyDerivation;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    static class KeyDerivationFactory
    {
        public static IKeyDerivation Get(KeyDerivationAlgorithm algorithm)
        {
            IKeyDerivation keyDerivation = null;

            switch (algorithm)
            {           
                case KeyDerivationAlgorithm.Argon2Id:
                    keyDerivation = new Argon2IdKeyDerivation();
                    break;

                case KeyDerivationAlgorithm.Pbkdf2:
                    keyDerivation = new PBKDF2KeyDerivation();
                    break;

                default:
                    throw new Exception();
            }

            return keyDerivation;
        }
    }
}

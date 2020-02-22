using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    public class EncryptionServiceFactory : IEncryptionServiceFactory
    {
        public IEncryptionService Get(EncryptionParameters parameters)
        {
            IEncryptionService encryptionService = null;

            switch (parameters.Algorithm)
            {
                case EncryptionAlgorithm.Rijndael256CbcPkcs7:
                case EncryptionAlgorithm.Rijndael128CbcPkcs7:
                    encryptionService = new RijndaelManagedEncryption();
                    break;

                case EncryptionAlgorithm.Aes256CfbPkcs7:
                case EncryptionAlgorithm.Aes128CfbPkcs7:
                    encryptionService = new AesEncryption(parameters);
                    break;

                default:
                    encryptionService = new AesEncryption(parameters);
                    break;
            }

            return encryptionService;
        }
    }
}

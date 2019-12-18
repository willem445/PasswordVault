using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    public class EncryptionServiceFactory : IEncryptionServiceFactory
    {
        public IEncryptionService Get(EncryptionServiceParameters parameters)
        {
            IEncryptionService encryptionService = null;

            switch (parameters.EncryptionService)
            {
                case EncryptionService.RijndaelManaged:
                    encryptionService = new RijndaelManagedEncryption(parameters.EncryptionSizes.KeySize, parameters.EncryptionSizes.BlockSize, parameters.EncryptionSizes.Iterations);
                    break;

                case EncryptionService.Aes:
                    encryptionService = new AesEncryption(parameters.EncryptionSizes.KeySize, parameters.EncryptionSizes.BlockSize, parameters.EncryptionSizes.Iterations);
                    break;

                default:
                    encryptionService = new AesEncryption(parameters.EncryptionSizes.KeySize, parameters.EncryptionSizes.BlockSize, parameters.EncryptionSizes.Iterations);
                    break;
            }

            return encryptionService;
        }
    }
}

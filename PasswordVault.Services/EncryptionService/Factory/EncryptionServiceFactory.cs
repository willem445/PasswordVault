using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PasswordVault.Services
{
    public class EncryptionServiceFactory : IEncryptionServiceFactory
    {
        public IEncryptionService GetEncryptionService(EncryptionParameters parameters)
        {
            if (parameters is null)
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "{0} cannot be null.", (nameof(parameters))));

            IEncryptionService encryptionService = null;

            switch (parameters.CipherSuite)
            {
                case CipherSuite.Rijndael256CbcPkcs7:
                case CipherSuite.Rijndael128CbcPkcs7:
                    encryptionService = new RijndaelManagedEncryption();
                    break;

                case CipherSuite.Aes256CfbPkcs7:
                case CipherSuite.Aes128CfbPkcs7:
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

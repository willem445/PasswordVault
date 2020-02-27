using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace PasswordVault.Services
{
    public sealed class AppSettings
    {
        private static readonly Lazy<AppSettings>
        lazy =
        new Lazy<AppSettings>
            (() => new AppSettings());

        public static AppSettings Instance { get { return lazy.Value; } }

        public string Secret { get; set; }

        private KeyDerivationParameters DefaultKeyDerivationParameters { get; }
        private KeyDerivationParameters DefaultEncryptionKeyDerivationParameters { get; }
        public MasterPasswordParameters DefaultMasterPasswordParameters { get; }
        public EncryptionParameters DefaultEncryptionParameters { get; }

        private AppSettings()
        {
#if DEBUG
            // For debugging, reduce the parameters for faster testing
            DefaultKeyDerivationParameters = new KeyDerivationParameters(
                algorithm: KeyDerivationAlgorithm.Argon2Id,
                keysize: 32, // 256 bit key for AES
                saltsize: 16, // 128 bits of salt is recommended for hashing (https://www.alexedwards.net/blog/how-to-hash-and-verify-passwords-with-argon2-in-go)
                iterations: 5,
                degreeofparallelism: 2,
                memorySizeKb: 1024
            );

            DefaultEncryptionKeyDerivationParameters = new KeyDerivationParameters(
                algorithm: KeyDerivationAlgorithm.Argon2Id,
                keysize: 32, // 256 bit key for AES
                saltsize: 16, // 128 bits of salt is recommended for hashing (https://www.alexedwards.net/blog/how-to-hash-and-verify-passwords-with-argon2-in-go)
                iterations: 1,
                degreeofparallelism: 2,
                memorySizeKb: 1024 
            );
#else
            // tuned for about 15s
            DefaultKeyDerivationParameters = new KeyDerivationParameters(
                algorithm:KeyDerivationAlgorithm.Argon2Id, 
                keysize:32, // 256 bit key for AES
                saltsize:16, // 128 bits of salt is recommended for hashing (https://www.alexedwards.net/blog/how-to-hash-and-verify-passwords-with-argon2-in-go)
                iterations:10,
                degreeofparallelism:8,
                memorySize:1048576 // 1gb
            );

            // tuned for about 100ms
            DefaultEncryptionKeyDerivationParameters = new KeyDerivationParameters(
                algorithm: KeyDerivationAlgorithm.Argon2Id,
                keysize: 32, // 256 bit key for AES
                saltsize: 16, // 128 bits of salt is recommended for hashing (https://www.alexedwards.net/blog/how-to-hash-and-verify-passwords-with-argon2-in-go)
                iterations: 40,
                degreeofparallelism: 4,
                memorySize: 1024 // 1mb
            );
#endif

            DefaultMasterPasswordParameters = new MasterPasswordParameters(
                keyDerivationParameters:DefaultKeyDerivationParameters,
                randomKeySize:64
            );

            DefaultEncryptionParameters = new EncryptionParameters(
                algorithm:EncryptionAlgorithm.Aes256CfbPkcs7,
                mac:Mac.HMACSHA256,
                keyDerivationParameters:DefaultEncryptionKeyDerivationParameters,
                blocksize:16,
                ivSize:16
            );
        }
    }
}

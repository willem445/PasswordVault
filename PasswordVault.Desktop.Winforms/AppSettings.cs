﻿using Newtonsoft.Json;
using PasswordVault.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PasswordVault.Desktop.Winforms
{
    public class Settings
    {
        public MasterPasswordParameters MasterPasswordParameters { get; set; }
        public EncryptionParameters EncryptionParameters { get; set; }
        public int TimeoutMinutes { get; set; }
    }

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
        public int DefaultTimeoutMinutes { get; }

#if DEBUG
        private string SETTINGS_FILE = Path.Combine(Environment.CurrentDirectory, @"..\..\..\PasswordVault.Data\TestDb") + "\\PasswordVaultSettings.json";
#else
        private string SETTINGS_FILE = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PasswordVault") + "\\PasswordVaultSettings.json";
#endif

        private AppSettings()
        {
#if DEBUG
            // For debugging, reduce the parameters for faster testing
            DefaultKeyDerivationParameters = new KeyDerivationParameters(
                algorithm: KeyDerivationAlgorithm.Argon2Id,
                keysize: 32, // 256 bit key for AES
                saltsize: 16, // 128 bits of salt is recommended for hashing (https://www.alexedwards.net/blog/how-to-hash-and-verify-passwords-with-argon2-in-go)
                iterations: 4,
                degreeofparallelism: 16,
                memorySizeKb: 1048576 // 1gb
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
            DefaultKeyDerivationParameters = new KeyDerivationParameters(
                algorithm:KeyDerivationAlgorithm.Argon2Id, 
                keysize:32, // 256 bit key for AES
                saltsize:16, // 128 bits of salt is recommended for hashing (https://www.alexedwards.net/blog/how-to-hash-and-verify-passwords-with-argon2-in-go)
                iterations:4,
                degreeofparallelism:16,
                memorySizeKb:1048576 // 1gb
            );

            DefaultEncryptionKeyDerivationParameters = new KeyDerivationParameters(
                algorithm: KeyDerivationAlgorithm.Argon2Id,
                keysize: 32, // 256 bit key for AES
                saltsize: 16, // 128 bits of salt is recommended for hashing (https://www.alexedwards.net/blog/how-to-hash-and-verify-passwords-with-argon2-in-go)
                iterations: 1,
                degreeofparallelism: 1,
                memorySizeKb: 1024 // 1mb
            );
#endif

            DefaultMasterPasswordParameters = new MasterPasswordParameters(
                keyDerivationParameters: DefaultKeyDerivationParameters,
                randomKeySize: 64
            );

            DefaultEncryptionParameters = new EncryptionParameters(
                cipherSuite: CipherSuite.Aes256CbcPkcs7,
                mac: Mac.HMACSHA256,
                keyDerivationParameters: DefaultEncryptionKeyDerivationParameters,
                blocksize: 16,
                ivSize: 16
            );

            DefaultTimeoutMinutes = 15;

            if (File.Exists(SETTINGS_FILE))
            {
                try
                {
                    Settings settingsOverride = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(SETTINGS_FILE)); // TODO - add verification for settings
                    DefaultMasterPasswordParameters = settingsOverride.MasterPasswordParameters;
                    DefaultEncryptionParameters = settingsOverride.EncryptionParameters;
                    DefaultTimeoutMinutes = settingsOverride.TimeoutMinutes;
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                }           
            }
        }
    }
}

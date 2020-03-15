using System;
using System.Collections.Generic;
using System.Text;
using PasswordVault.Models;

namespace PasswordVault.Services
{
    interface IImportExport
    {
        ImportExportResult Export(string exportPath, List<Password> passwords, string encryptionPassword = null, bool passwordEnabled = false);
        ImportResult Import(string path, string passphrase = null, bool passwordEnabled = false);
    }
}

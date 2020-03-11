using System;
using System.Collections.Generic;
using System.Text;
using PasswordVault.Models;

namespace PasswordVault.Services
{
    interface IImportExport
    {
        ImportExportResult Export(string exportPath, List<Password> passwords, string encryptionPassword, bool passwordEnabled);
        ImportResult Import(string path, string passphrase, bool passwordEnabled);
    }
}

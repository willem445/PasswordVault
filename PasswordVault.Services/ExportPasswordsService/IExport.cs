using System;
using System.Collections.Generic;
using System.Text;
using PasswordVault.Models;

namespace PasswordVault.Services
{
    interface IExport
    {
        ExportResult Export(string exportPath, List<Password> passwords, string encryptionPassword, bool passwordEnabled);
    }
}

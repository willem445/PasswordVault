using System;
using System.Collections.Generic;
using System.Text;
using PasswordVault.Models;

namespace PasswordVault.Services
{
    public interface IImportExportPasswords
    {
        ImportExportResult Export(ImportExportFileType fileType, string exportPath, List<Password> passwords, string passwordProtection, bool passwordEnabled);
        ImportResult Import(ImportExportFileType filetype, string importPath, string passphrase, bool passwordEnabled);
        List<SupportedFileTypes> GetSupportedFileTypes();
    }
}

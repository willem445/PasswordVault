using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    public interface IExportPasswords
    {
        ExportResult Export(ExportFileTypes fileType, string exportPath);
    }
}

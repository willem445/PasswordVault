using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    interface IImportExportFactory
    {
        IImportExport Get(ImportExportFileType type);
    }
}

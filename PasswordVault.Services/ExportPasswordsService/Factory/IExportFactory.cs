using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    interface IExportFactory
    {
        IExport Get(ExportFileType exportType);
    }
}

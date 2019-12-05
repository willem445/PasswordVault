using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    interface IExport
    {
        ExportResult Export(string exportPath);
    }
}

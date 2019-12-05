using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    public enum ExportResult
    {
        Success,
        Fail
    }

    public enum ExportFileTypes
    {
        Excel,
        Word,
        PDF
    }
}

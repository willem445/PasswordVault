﻿using System;
using System.Collections.Generic;
using System.Text;
using PasswordVault.Models;

namespace PasswordVault.Services
{
    public interface IExportPasswords
    {
        ExportResult Export(ExportFileTypes fileType, string exportPath, List<Password> passwords, string passwordProtection);
        List<SupportedFileTypes> GetSupportedFileTypes();
    }
}

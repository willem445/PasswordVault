using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordVault.Services;
using PasswordVault.Models;

namespace PasswordVault.Desktop.Winforms
{
    public interface IExportView
    {
        event Action InitializeEvent;
        event Action<ExportFileTypes, string, string> ExportPasswordsEvent;

        void DisplayFileTypes(List<SupportedFileTypes> fileTypes);
        void DisplayExportResult(ExportResult result);
        void ShowExportView();

    }
}

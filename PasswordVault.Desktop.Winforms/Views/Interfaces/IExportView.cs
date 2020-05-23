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
        event Action<ImportExportFileType, string, string, bool> ExportPasswordsEvent;
        event Action<string, string, bool> DataValidationEvent;

        void DisplayFileTypes(List<SupportedFileTypes> fileTypes);
        void DisplayExportResult(ImportExportResult result);
        void DisplayValidationResult(ExportValidationResult result, ImportExportFileType fileType);
        void ShowExportView();

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordVault.Services;
using PasswordVault.Models;

namespace PasswordVault.Desktop.Winforms
{
    public interface IImportView
    {
        event Action InitializeEvent;
        event Action<ImportExportFileType, string, string> ImportPasswordsEvent;
        event Action<string, string> DataValidationEvent;
        event Action<ImportExportResult> ImportPasswordsDoneEvent;

        void DisplayFileTypes(List<SupportedFileTypes> fileTypes);
        void DisplayImportResult(ImportExportResult result);
        void DisplayValidationResult(ExportValidationResult result, ImportExportFileType fileType);
        void ShowImportView();
        void CloseView();
    }
}

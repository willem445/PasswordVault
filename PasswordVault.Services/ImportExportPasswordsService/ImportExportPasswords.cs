using System;
using System.Collections.Generic;
using System.Text;
using PasswordVault.Models;

namespace PasswordVault.Services
{
    public class ImportExportPasswords : IImportExportPasswords
    {
        private List<SupportedFileTypes> _supportedFileTypes;

        public ImportExportPasswords()
        {
            _supportedFileTypes = new List<SupportedFileTypes>();
            _supportedFileTypes.Add(new SupportedFileTypes(ImportExportFileType.Excel, "Excel (*.xlsx)|*.xlsx", "Excel"));
            //_supportedFileTypes.Add(new SupportedFileTypes(ExportFileTypes.PDF, "PDF (*.pdf)|*.pdf", "PDF"));
            //_supportedFileTypes.Add(new SupportedFileTypes(ExportFileTypes.Word, "Word (*.docx)|*.docx", "Word"));
        }

        public ImportExportResult Export(ImportExportFileType fileType, string exportPath, List<Password> passwords, string passwordProtection, bool passwordEnabled)
        {
            ImportExportResult result;

            IImportExportFactory exportFactory = new ImportExportFactory();
            IImportExport exporter = exportFactory.Get(fileType);
            result = exporter.Export(exportPath, passwords, passwordProtection, passwordEnabled);

            return result;
        }

        public List<SupportedFileTypes> GetSupportedFileTypes()
        {
            return _supportedFileTypes;
        }

        public ImportResult Import(ImportExportFileType filetype, string importPath, string passphrase, bool passwordEnabled)
        {
            ImportResult result;

            IImportExportFactory importFactory = new ImportExportFactory();
            IImportExport importer = importFactory.Get(filetype);
            result = importer.Import(importPath, passphrase, passwordEnabled);

            return result;
        }
    }
}

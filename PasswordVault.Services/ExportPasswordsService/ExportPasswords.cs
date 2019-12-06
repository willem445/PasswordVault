using System;
using System.Collections.Generic;
using System.Text;
using PasswordVault.Models;

namespace PasswordVault.Services
{
    public class ExportPasswords : IExportPasswords
    {
        private List<SupportedFileTypes> _supportedFileTypes;

        public ExportPasswords()
        {
            _supportedFileTypes = new List<SupportedFileTypes>();
            _supportedFileTypes.Add(new SupportedFileTypes(ExportFileTypes.Excel, "Excel (*.xlsx)|*.xlsx", "Excel"));
            _supportedFileTypes.Add(new SupportedFileTypes(ExportFileTypes.PDF, "PDF (*.pdf)|*.pdf", "PDF"));
            _supportedFileTypes.Add(new SupportedFileTypes(ExportFileTypes.Word, "Word (*.docx)|*.docx", "Word"));
        }

        public ExportResult Export(ExportFileTypes fileType, string exportPath, List<Password> passwords)
        {
            ExportResult result;

            IExportFactory exportFactory = new ExportFactory();
            IExport exporter = exportFactory.Get(fileType);
            result = exporter.Export(exportPath, passwords);

            return result;
        }

        public List<SupportedFileTypes> GetSupportedFileTypes()
        {
            return _supportedFileTypes;
        }
    }
}

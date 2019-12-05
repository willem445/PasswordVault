using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    public class ExportPasswords : IExportPasswords
    {
        public ExportResult Export(ExportFileTypes fileType, string exportPath)
        {
            ExportResult result;

            IExportFactory exportFactory = new ExportFactory();
            IExport exporter = exportFactory.Get(fileType);
            result = exporter.Export(exportPath);

            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    class ImportExportFactory : IImportExportFactory
    {
        public IImportExport Get(ImportExportFileType exportType)
        {
            IImportExport export = null;

            switch(exportType)
            {
                case ImportExportFileType.Excel:
                    export = new ExcelImportExport();
                    break;

                default:
                    break;
            }

            return export;
        }
    }
}

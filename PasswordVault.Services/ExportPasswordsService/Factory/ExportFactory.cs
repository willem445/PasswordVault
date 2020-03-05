using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    class ExportFactory : IExportFactory
    {
        public IExport Get(ExportFileType exportType)
        {
            IExport export = null;

            switch(exportType)
            {
                case ExportFileType.Excel:
                    export = new ExcelExport();
                    break;

                default:
                    break;
            }

            return export;
        }
    }
}

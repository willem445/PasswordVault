using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    class ExportFactory : IExportFactory
    {
        public IExport Get(ExportFileTypes exportType)
        {
            IExport export = null;

            switch(exportType)
            {
                case ExportFileTypes.Excel:
                    export = new ExcelExport();
                    break;

                default:
                    break;
            }

            return export;
        }
    }
}

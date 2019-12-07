using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordVault.Services;
using PasswordVault.Models;

namespace PasswordVault.Desktop.Winforms
{
    class ExportPresenter
    {
        private IExportView _exportView;
        private IDesktopServiceWrapper _serviceWrapper;

        public ExportPresenter(IExportView exportView, IDesktopServiceWrapper serviceWrapper)
        {
            _exportView = exportView;
            _serviceWrapper = serviceWrapper;

            _exportView.ExportPasswordsEvent += ExportPasswords;
            _exportView.InitializeEvent += InitializeFileTypes;

        }

        private void InitializeFileTypes()
        {
            List<SupportedFileTypes> supportedFileTypes = _serviceWrapper.GetSupportedFileTypes();
            _exportView.DisplayFileTypes(supportedFileTypes);
        }

        private void ExportPasswords(ExportFileTypes fileType, string path, string password)
        {
            ExportResult result = _serviceWrapper.ExportPasswords(fileType, path, password);
            _exportView.DisplayExportResult(result);
        }
    }
}

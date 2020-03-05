using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordVault.Services;
using PasswordVault.Models;
using System.IO;

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
            _exportView.DataValidationEvent += DataValidation;

        }

        private void InitializeFileTypes()
        {
            List<SupportedFileTypes> supportedFileTypes = _serviceWrapper.GetSupportedFileTypes();
            _exportView.DisplayFileTypes(supportedFileTypes);
        }

        private void ExportPasswords(ExportFileType fileType, string path, string password, bool passwordEnabled)
        {
            ExportResult result = _serviceWrapper.ExportPasswords(fileType, path, password, passwordEnabled);
            _exportView.DisplayExportResult(result);
        }

        private void DataValidation(string path, string password)
        {
            ExportValidationResult result = ExportValidationResult.Invalid;
            ExportFileType fileType = ExportFileType.Unsupported;

            List<SupportedFileTypes> supportedFileTypes = _serviceWrapper.GetSupportedFileTypes();
            SupportedFileTypes supportedFile = null;

            if (path.Contains('.'))
            {
                supportedFile = supportedFileTypes.Where(x => x.Filter.Contains(path.Split('.')[1])).FirstOrDefault();
            }          

            if (supportedFile != null)
            {
                fileType = supportedFile.FileType;

                string dir = Path.GetDirectoryName(path);

                if (!Directory.Exists(dir))
                {
                    result = ExportValidationResult.PathDoesNotExist;
                }
                else
                {
                    result = ExportValidationResult.Valid;
                }
            }
            else
            {
                result = ExportValidationResult.FileNotSupported;
            }

            if (string.IsNullOrEmpty(password) || password == "Enter encryption password..")
            {
                result = ExportValidationResult.InvalidPassword;
            }

            _exportView.DisplayValidationResult(result, fileType);
        }
    }
}

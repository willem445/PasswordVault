using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordVault.Services;

namespace PasswordVault.Desktop.Winforms
{
    public class ImportPresenter
    {
        private IImportView _importView;
        private IDesktopServiceWrapper _serviceWrapper;

        public ImportPresenter(IImportView importView, IDesktopServiceWrapper serviceWrapper)
        {
            _importView = importView ?? throw new ArgumentNullException(nameof(importView));
            _serviceWrapper = serviceWrapper ?? throw new ArgumentNullException(nameof(serviceWrapper));

            _importView.InitializeEvent += InitializeFileTypes;
            _importView.ImportPasswordsEvent += ImportPasswords;
            _importView.DataValidationEvent += DataValidation;
        }

        private void InitializeFileTypes()
        {
            List<SupportedFileTypes> supportedFileTypes = _serviceWrapper.GetSupportedFileTypes();
            _importView.DisplayFileTypes(supportedFileTypes);
        }

        private void ImportPasswords(ImportExportFileType fileType, string path, string password)
        {
            bool passwordEnabled = true;

            if (string.IsNullOrEmpty(password) || password == "Enter password..")
            {
                passwordEnabled = false;
            }
            ImportExportResult result = _serviceWrapper.ImportPasswords(fileType, path, password, passwordEnabled);
            _importView.DisplayImportResult(result);
        }

        private void DataValidation(string path, string password)
        {
            ExportValidationResult result = ExportValidationResult.Invalid;
            ImportExportFileType fileType = ImportExportFileType.Unsupported;

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

            _importView.DisplayValidationResult(result, fileType);
        }
    }


}

using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    public enum ExportResult
    {
        Success,
        Fail
    }

    public enum ExportFileTypes
    {
        Unsupported,
        Excel,
        Word,
        PDF
    }

    public enum ExportValidationResult
    {
        InvalidPassword,
        FileNotSupported,
        PathDoesNotExist,
        Invalid,
        Valid
    }

    public class SupportedFileTypes
    {
        public SupportedFileTypes(ExportFileTypes fileType, string filter, string name)
        {
            FileType = fileType;
            Filter = filter;
            Name = name;
        }

        public ExportFileTypes FileType { get; }
        public string Filter { get; }
        public string Name { get; }
    }
}

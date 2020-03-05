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

    public enum ExportFileType
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
        public SupportedFileTypes(ExportFileType fileType, string filter, string name)
        {
            FileType = fileType;
            Filter = filter;
            Name = name;
        }

        public ExportFileType FileType { get; }
        public string Filter { get; }
        public string Name { get; }
    }
}

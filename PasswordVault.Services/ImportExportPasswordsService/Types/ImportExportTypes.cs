using PasswordVault.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault.Services
{
    public enum ImportExportResult
    {
        Success,
        Fail,
        PasswordInvalid,
        PasswordProtected,
    }

    public enum ImportExportFileType
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

    public struct ImportResult : IEquatable<ImportResult>
    {
        public ImportResult(ImportExportResult result, List<Password> passwords)
        {
            Result = result;
            Passwords = passwords;
        }

        public ImportExportResult Result { get; }
        public List<Password> Passwords { get; }

        public override bool Equals(object obj)
        {
            if (!(obj is ImportResult))
                return false;

            ImportResult mys = (ImportResult)obj;

            if (mys.Result != Result)
                return false;

            if (mys.Passwords != Passwords)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            // Suitable nullity checks etc, of course :)
            hash = hash * 23 + Result.GetHashCode();
            hash = hash * 23 + Passwords.GetHashCode();
            return hash;
        }

        public static bool operator ==(ImportResult left, ImportResult right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ImportResult left, ImportResult right)
        {
            return !(left == right);
        }

        public bool Equals(ImportResult other)
        {
            ImportResult mys = (ImportResult)other;

            if (mys.Result != Result)
                return false;

            if (mys.Passwords != Passwords)
                return false;

            return true;
        }
    }

    public class SupportedFileTypes
    {
        public SupportedFileTypes(ImportExportFileType fileType, string filter, string name)
        {
            FileType = fileType;
            Filter = filter;
            Name = name;
        }

        public ImportExportFileType FileType { get; }
        public string Filter { get; }
        public string Name { get; }
    }
}

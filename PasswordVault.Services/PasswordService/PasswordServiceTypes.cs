using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

/*=================================================================================================
DESCRIPTION
*================================================================================================*/
/* 
 ------------------------------------------------------------------------------------------------*/

namespace PasswordVault.Services
{
    public struct AddPasswordResult
    {
        public AddPasswordResult(AddModifyPasswordResult result, Int64 uniquePasswordID)
        {
            Result = result;
            UniquePasswordID = uniquePasswordID;
        }

        public AddModifyPasswordResult Result { get; }
        public Int64 UniquePasswordID { get; }
    }

    public enum AddModifyPasswordResult
    {
        ApplicationError,
        UsernameError,
        EmailError,
        DescriptionError,
        WebsiteError,
        PassphraseError,
        DuplicatePassword,
        Failed,
        Success,
    }

    public enum DeletePasswordResult
    {
        PasswordDoesNotExist,
        Success,
        Failed
    }
} // PasswordVault.Services.Standard.PasswordService NAMESPACE

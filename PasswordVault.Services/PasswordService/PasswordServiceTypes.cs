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

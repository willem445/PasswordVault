using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

/*=================================================================================================
DESCRIPTION
*================================================================================================*/
/* 
 ------------------------------------------------------------------------------------------------*/

namespace PasswordVault.Services.Standard
{
    public enum LoginResult
    {
        PasswordIncorrect,
        UsernameDoesNotExist,
        Successful,
        Failed
    }

    public enum CreateUserResult
    {
        UsernameTaken,
        UsernameNotValid,
        PasswordNotValid,
        NoSpecialCharacter,
        LengthRequirementNotMet,
        NoNumber,
        NoUpperCaseCharacter,
        NoLowerCaseCharacter,
        FirstNameNotValid,
        LastNameNotValid,
        PhoneNumberNotValid,
        EmailNotValid,
        Successful,
        Failed,
    }

    public enum DeleteUserResult
    {
        Success,
        Failed,
    }

    public enum UserInformationResult
    {
        Success,
        Failed,
        InvalidFirstName,
        InvalidLastName,
        InvalidPhoneNumber,
        InvalidEmail,
    }

    public enum ChangeUserPasswordResult
    {
        Failed,
        Success,
        NoSpecialCharacter,
        LengthRequirementNotMet,
        NoNumber,
        NoUpperCaseCharacter,
        NoLowerCaseCharacter,
        PasswordsDoNotMatch,
        InvalidPassword,
    }

    public enum AddPasswordResult
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

    public enum LogOutResult
    {
        Success,
        Failed
    }
} // PasswordVault.Services.Standard.PasswordService NAMESPACE

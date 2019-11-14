using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace PasswordVault.Services
{
    public enum AddUserResult
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
        IncorrectPassword,
        Failed,
    }
}

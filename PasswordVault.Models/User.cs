using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace PasswordVault.Models
{
    public enum ValidateUserPasswordResult
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

    public enum UserInformationResult
    {
        Success,
        Failed,
        InvalidFirstName,
        InvalidLastName,
        InvalidPhoneNumber,
        InvalidEmail,
    }

    public class User
    {
        private const int MINIMUM_PASSWORD_LENGTH = 15;

        public string PlainTextRandomKey { get; }
        public string PlainTextPassword { get; }
        public bool ValidUser { get; set; }
        public string Token { get; set; }

        // Properties stored in database
        public string GUID { get; } // unique Id assigned to each user, this unique id is the PK for password table
        public string EncryptedKey { get; } // encrypted version of randomly generated key, encrypted using the plaintext user password  
        public string Username { get; }
        public string Iterations { get; }
        public string Salt { get; }
        public string Hash { get; }
        public string FirstName { get; } // use randomly generated key to hash and store
        public string LastName { get; } // use randomly generated key to hash and store
        public string PhoneNumber { get; } // use randomly generated key to hash and store
        public string Email { get; } // use randomly generated key to hash and store

        public User(string uniqueID, string encryptedKey, string username, string iterations, string salt, string hash, string firstName, string lastName, string phoneNumber, string email, bool validUser = false)
        {
            GUID = uniqueID;
            EncryptedKey = encryptedKey;
            Username = username;
            Iterations = iterations;
            Salt = salt;
            Hash = hash;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;

            ValidUser = validUser;
        }

        public User(string uniqueID, string username, string plainTextRandomKey, string firstName, string lastName, string phoneNumber, string email, bool validUser = false)
        {
            GUID = uniqueID;
            Username = username;
            PlainTextRandomKey = plainTextRandomKey;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;

            ValidUser = validUser;
        }

        public User(string username, string password, string firstName, string lastName, string phoneNumber, string email, bool validUser = false)
        {
            Username = username;
            PlainTextPassword = password;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;

            ValidUser = validUser;
        }

        public User(bool validUser = false)
        {
            ValidUser = validUser;
        }

        public User(string username)
        {
            Username = username;
        }

        public User()
        {

        }

        public string GetUserString()
        {
            string userString = "";

            userString = string.Format(CultureInfo.CurrentCulture, "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", GUID, EncryptedKey, Username, Iterations, Salt, Hash, FirstName, LastName, PhoneNumber, Email);

            return userString;
        }

        public bool VerifyUsernameRequirements()
        {
            bool result = true;

            if (string.IsNullOrEmpty(this.Username))
            {
                result = false;
            }

            return result;
        }

        public UserInformationResult VerifyUserInformation()
        {
            UserInformationResult result = UserInformationResult.Success;

            if (this != null)
            {
                if (string.IsNullOrEmpty(this.FirstName))
                {
                    result = UserInformationResult.InvalidFirstName;
                }

                if (string.IsNullOrEmpty(this.LastName))
                {
                    result = UserInformationResult.InvalidLastName;
                }

                if (string.IsNullOrEmpty(this.Email) || this.Email == "example@provider.com")
                {
                    result = UserInformationResult.InvalidEmail;
                }
                else
                {
                    var regex = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

                    Match match = Regex.Match(this.Email, regex, RegexOptions.IgnoreCase);

                    if (!match.Success)
                    {
                        result = UserInformationResult.InvalidEmail;
                    }
                }

                if (string.IsNullOrEmpty(this.PhoneNumber) || this.PhoneNumber == "xxx-xxx-xxxx")
                {
                    result = UserInformationResult.InvalidPhoneNumber;
                }
                else
                {
                    if (!IsValidUSPhoneNumber(this.PhoneNumber))
                    {
                        result = UserInformationResult.InvalidPhoneNumber;
                    }
                }
            }

            return result;
        }

        public ValidateUserPasswordResult VerifyPlaintextPasswordRequirements()
        {
            ValidateUserPasswordResult result = ValidateUserPasswordResult.Failed;

            bool containsNumber = false;
            bool containsLowerCase = false;
            bool containsUpperCase = false;

            if (string.IsNullOrEmpty(this.PlainTextPassword))
            {
                return ValidateUserPasswordResult.Failed;
            }

            if (this.PlainTextPassword.Length <= MINIMUM_PASSWORD_LENGTH)
            {
                result = ValidateUserPasswordResult.LengthRequirementNotMet;
            }

            foreach (var character in this.PlainTextPassword)
            {
                if (char.IsUpper(character))
                {
                    containsUpperCase = true;
                }
                else if (char.IsLower(character))
                {
                    containsLowerCase = true;
                }
                else if (char.IsDigit(character))
                {
                    containsNumber = true;
                }
            }

            if (!containsLowerCase)
            {
                result = ValidateUserPasswordResult.NoLowerCaseCharacter;
            }

            if (!containsUpperCase)
            {
                result = ValidateUserPasswordResult.NoUpperCaseCharacter;
            }

            if (!containsNumber)
            {
                result = ValidateUserPasswordResult.NoNumber;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(this.PlainTextPassword, @"[!@#$%^&*()_+=\[{\]};:<>|./?,-]"))
            {
                result = ValidateUserPasswordResult.NoSpecialCharacter;
            }

            return result;
        }

        private static bool IsValidUSPhoneNumber(string strPhone)
        {
            string regExPattern = @"^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$";
            return MatchStringFromRegex(strPhone, regExPattern);
        }

        private static bool MatchStringFromRegex(string str, string regexstr)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            str = str.Trim();
            System.Text.RegularExpressions.Regex pattern = new System.Text.RegularExpressions.Regex(regexstr);
            return pattern.IsMatch(str);
        }

    } // User CLASS
} // PasswordVault.Models.Standard NAMESPACE

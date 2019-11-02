using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PasswordVault.Models;
using PasswordVault.Data;
using System.Globalization;

namespace PasswordVault.Services
{
    class UserService : IUserService
    {
        /*CONSTANTS********************************************************/

        /*FIELDS***********************************************************/
        private IDatabase _dbcontext;                   
        private IMasterPassword _masterPassword;
        private IEncryptionService _encryptDecrypt;

        /*PROPERTIES*******************************************************/

        /*CONSTRUCTORS*****************************************************/
        public UserService(IDatabase dbcontext, IMasterPassword masterPassword, IEncryptionService encryptDecrypt)
        {
            _dbcontext = dbcontext ?? throw new ArgumentNullException(nameof(dbcontext));
            _masterPassword = masterPassword ?? throw new ArgumentNullException(nameof(masterPassword));
            _encryptDecrypt = encryptDecrypt ?? throw new ArgumentNullException(nameof(encryptDecrypt));
        }

        /*PUBLIC METHODS***************************************************/
        public AddUserResult AddUser(User user)
        {
            AddUserResult addUserResult = AddUserResult.Failed;
            
            // Null check
            if (user == null) 
                return AddUserResult.Failed; 

            // Check if username exists
            User queryResult = _dbcontext.GetUserByUsername(user.Username);
            if (queryResult != null) 
                return AddUserResult.UsernameTaken;

            // Data validation
            UserInformationResult verifyUser = user.VerifyUserInformation();
            ValidateUserPasswordResult verifyPassword = user.VerifyPlaintextPasswordRequirements();

            if (!user.VerifyUsernameRequirements())
            {
                addUserResult = AddUserResult.UsernameNotValid;
            }
            else if (verifyPassword != ValidateUserPasswordResult.Success)
            {
                switch (verifyPassword)
                {
                    case ValidateUserPasswordResult.Failed:
                        addUserResult = AddUserResult.PasswordNotValid;
                        break;

                    case ValidateUserPasswordResult.LengthRequirementNotMet:
                        addUserResult = AddUserResult.LengthRequirementNotMet;
                        break;

                    case ValidateUserPasswordResult.NoLowerCaseCharacter:
                        addUserResult = AddUserResult.NoLowerCaseCharacter;
                        break;

                    case ValidateUserPasswordResult.NoNumber:
                        addUserResult = AddUserResult.NoNumber;
                        break;

                    case ValidateUserPasswordResult.NoSpecialCharacter:
                        addUserResult = AddUserResult.NoSpecialCharacter;
                        break;

                    case ValidateUserPasswordResult.NoUpperCaseCharacter:
                        addUserResult = AddUserResult.NoUpperCaseCharacter;
                        break;

                    case ValidateUserPasswordResult.PasswordsDoNotMatch:
                        addUserResult = AddUserResult.PasswordNotValid;
                        break;

                    default:
                        addUserResult = AddUserResult.PasswordNotValid;
                        break;
                }
            }
            else if(verifyUser != UserInformationResult.Success)
            {
                switch (verifyUser)
                {
                    case UserInformationResult.InvalidEmail:
                        addUserResult = AddUserResult.EmailNotValid;
                        break;

                    case UserInformationResult.InvalidFirstName:
                        addUserResult = AddUserResult.FirstNameNotValid;
                        break;

                    case UserInformationResult.InvalidLastName:
                        addUserResult = AddUserResult.LastNameNotValid;
                        break;

                    case UserInformationResult.InvalidPhoneNumber:
                        addUserResult = AddUserResult.PhoneNumberNotValid;
                        break;

                    case UserInformationResult.Failed:
                        addUserResult = AddUserResult.Failed;
                        break;
                }
            }
            else // Successful
            {
                UserEncrypedData newEncryptedData = _masterPassword.GenerateNewUserEncryptedDataFromPassword(user.PlainTextPassword);

                User newUser = new User(
                        newEncryptedData.UniqueGUID, // Leave unique guid in plaintext
                        _encryptDecrypt.Encrypt(newEncryptedData.RandomGeneratedKey, user.PlainTextPassword), // Encrypt the random key with the users password
                        user.Username, // Leave username in plaintext
                        newEncryptedData.Iterations.ToString(CultureInfo.CurrentCulture), // Leave iterations in plaintext
                        newEncryptedData.Salt,
                        newEncryptedData.Hash,
                        _encryptDecrypt.Encrypt(user.FirstName, newEncryptedData.RandomGeneratedKey), // Encrypt with decrypted random key
                        _encryptDecrypt.Encrypt(user.LastName, newEncryptedData.RandomGeneratedKey), // Encrypt with decrypted random key
                        _encryptDecrypt.Encrypt(user.PhoneNumber, newEncryptedData.RandomGeneratedKey), // Encrypt with decrypted random key
                        _encryptDecrypt.Encrypt(user.Email, newEncryptedData.RandomGeneratedKey) // Encrypt with decrypted random key
                        );

                _dbcontext.AddUser(newUser);

                addUserResult = AddUserResult.Successful;
            }

            return addUserResult;
        }

        /******************************************************************/
        public ValidateUserPasswordResult ChangeUserPassword(string userUuid, string originalPassword, string newPassword, string confirmPassword)
        {
            throw new NotImplementedException();
        }

        /******************************************************************/
        public DeleteUserResult DeleteUser(string userUuid)
        {
            throw new NotImplementedException();
        }

        /******************************************************************/
        public int GetMinimumPasswordLength()
        {
            throw new NotImplementedException();
        }

        /******************************************************************/
        public User GetUser(string userUuid)
        {
            throw new NotImplementedException();
        }

        /******************************************************************/
        public UserInformationResult ModifyUser(string userUuid, User user)
        {
            throw new NotImplementedException();
        }

        /******************************************************************/
        public bool VerifyUserPassword(string userUuid, string password)
        {
            throw new NotImplementedException();
        }

        /*PRIVATE METHODS**************************************************/

        /*STATIC METHODS***************************************************/

    } // UserService CLASS
}

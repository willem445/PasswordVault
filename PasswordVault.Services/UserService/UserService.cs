using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PasswordVault.Models;
using PasswordVault.Data;
using System.Globalization;

namespace PasswordVault.Services
{
    public class UserService : IUserService
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
        public DeleteUserResult DeleteUser(string userUuid)
        {
            DeleteUserResult result = DeleteUserResult.Failed;

            if (String.IsNullOrEmpty(userUuid))
                return DeleteUserResult.Failed;

            User getUser = _dbcontext.GetUserByGUID(userUuid);

            if (getUser != null)
            {
                bool success = _dbcontext.DeleteUser(getUser);

                if (success)
                {
                    result = DeleteUserResult.Success;
                }
            }
            else
            {
                result = DeleteUserResult.Failed;
            }

            return result;
        }

        /******************************************************************/
        public UserInformationResult ModifyUser(string userUuid, User user, string encryptionKey)
        {
            UserInformationResult result = UserInformationResult.Failed;

            //if (user == null)       
            //    return UserInformationResult.Failed;

            //var validation = user.VerifyUserInformation();

            //if (validation == UserInformationResult.Success)
            //{
            //    User dbUser = _dbcontext.GetUserByGUID(userUuid);

            //    User newCurrentUser = new User(
            //        dbUser.GUID,
            //        dbUser.Username,
            //        dbUser.PlainTextRandomKey,
            //        user.FirstName,
            //        user.LastName,
            //        user.PhoneNumber,
            //        user.Email,
            //        true);

            //    User modifiedUser = new User
            //    (
            //        dbUser.GUID,
            //        dbUser.EncryptedKey,
            //        dbUser.Username,
            //        dbUser.Iterations,
            //        dbUser.Salt,
            //        dbUser.Hash,
            //        _encryptDecrypt.Encrypt(user.FirstName, encryptionKey),
            //        _encryptDecrypt.Encrypt(user.LastName, encryptionKey),
            //        _encryptDecrypt.Encrypt(user.PhoneNumber, encryptionKey),
            //        _encryptDecrypt.Encrypt(user.Email, encryptionKey)
            //    );

            //    bool success = _dbcontext.ModifyUser(dbUser, modifiedUser);

            //    if (success)
            //    {
            //        result = UserInformationResult.Success; // TODO - return other results
            //    }
            //    else
            //    {
            //        result = UserInformationResult.Failed;
            //    }
            //}
            //else
            //{
            //    result = validation;
            //}

            return result;

        }

        /******************************************************************/
        public ValidateUserPasswordResult ChangeUserPassword(string userUuid, string originalPassword, string newPassword, string confirmPassword)
        {
            ValidateUserPasswordResult result = ValidateUserPasswordResult.Failed;

            //if (string.IsNullOrEmpty(originalPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            //{
            //    return ValidateUserPasswordResult.Failed;
            //}

            //if (newPassword == confirmPassword)
            //{
            //    bool validPassword = VerifyUserPassword(userUuid, originalPassword);

            //    if (validPassword)
            //    {
            //        ValidateUserPasswordResult verifyPass = new User() { PlainTextPassword = newPassword }.VerifyPlaintextPasswordRequirements();

            //        if (verifyPass != ValidateUserPasswordResult.Success)
            //        {
            //            result = verifyPass;
            //        }
            //        else
            //        {
            //            User user = _dbcontext.GetUserByGUID(userUuid);
            //            UserEncrypedData newEncryptedData = _masterPassword.GenerateNewUserEncryptedDataFromPassword(newPassword);

            //            User newUser = new User(
            //                user.GUID,
            //                _encryptDecrypt.Encrypt(_currentUser.PlainTextRandomKey, newPassword), // Encrypt the random key with the users password
            //                user.Username,
            //                newEncryptedData.Iterations.ToString(CultureInfo.CurrentCulture),
            //                newEncryptedData.Salt,
            //                newEncryptedData.Hash,
            //                user.FirstName,
            //                user.LastName,
            //                user.PhoneNumber,
            //                user.Email
            //            );

            //            if (_dbcontext.ModifyUser(user, newUser))
            //            {
            //                result = ValidateUserPasswordResult.Success;
            //            }
            //            else
            //            {
            //                result = ValidateUserPasswordResult.Failed;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        result = ValidateUserPasswordResult.InvalidPassword;
            //    }
            //}
            //else
            //{
            //    result = ValidateUserPasswordResult.PasswordsDoNotMatch;
            //}


            return result;
        }

        /******************************************************************/
        public bool VerifyUserPassword(string userUuid, string password)
        {
            throw new NotImplementedException();
        }

        /******************************************************************/
        public User GetUser(string userUuid)
        {
            User user = null;

            if (!String.IsNullOrEmpty(userUuid))
            {
                user = _dbcontext.GetUserByGUID(userUuid);
            }     

            return user;
        }

        /******************************************************************/
        public int GetMinimumPasswordLength()
        {
            return User.GetMinimumPasswordLength();
        }

        /*PRIVATE METHODS**************************************************/

        /*STATIC METHODS***************************************************/

    } // UserService CLASS
}

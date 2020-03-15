using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PasswordVault.Models;
using PasswordVault.Data;
using System.Globalization;

// TODO - Generate valid user password method

namespace PasswordVault.Services
{
    public class UserService : IUserService
    {
        /*CONSTANTS********************************************************/

        /*FIELDS***********************************************************/
        private IDatabase _dbcontext;                   
        private IMasterPassword _masterPassword;
        private IEncryptionServiceFactory _encryptDecryptFactory;
        private IAuthenticationService _authenticationService;

        /*PROPERTIES*******************************************************/

        /*CONSTRUCTORS*****************************************************/
        public UserService(IDatabase dbcontext, IMasterPassword masterPassword, IEncryptionServiceFactory encryptDecryptFactory, IAuthenticationService authenticationService)
        {
            _dbcontext = dbcontext ?? throw new ArgumentNullException(nameof(dbcontext));
            _masterPassword = masterPassword ?? throw new ArgumentNullException(nameof(masterPassword));
            _encryptDecryptFactory = encryptDecryptFactory ?? throw new ArgumentNullException(nameof(encryptDecryptFactory));
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }

        /*PUBLIC METHODS***************************************************/
        public AddUserResult AddUser(User user, EncryptionParameters encParameters, MasterPasswordParameters hashParameters)
        {
            AddUserResult addUserResult = AddUserResult.Failed;
            
            if (user == null || encParameters == null || hashParameters == null) 
                return AddUserResult.Failed; 

            // Check if username exists
            if (UserExists(user)) 
                return AddUserResult.UsernameTaken;

            // Data validation
            AddUserResult validation = ValidateNewUser(user);
            
            if (validation == AddUserResult.Successful)
            {
                UserEncrypedData masterHash = _masterPassword.GenerateMasterHash(user.PlainTextPassword, hashParameters);
                IEncryptionService encryptionService = _encryptDecryptFactory.GetEncryptionService(encParameters);

                User newUser = new User(
                        User.GenerateUserUuid(),
                        encryptionService.Encrypt(masterHash.RandomGeneratedKey, user.PlainTextPassword), // Encrypt the random key with the users password
                        user.Username,
                        _masterPassword.FlattenHash(masterHash), // Flaten the hash, salt, and parameters
                        encryptionService.Encrypt(user.FirstName, masterHash.RandomGeneratedKey), // Encrypt with plaintext random key
                        encryptionService.Encrypt(user.LastName, masterHash.RandomGeneratedKey), // Encrypt with plaintext random key
                        encryptionService.Encrypt(user.PhoneNumber, masterHash.RandomGeneratedKey), // Encrypt with plaintext random key
                        encryptionService.Encrypt(user.Email, masterHash.RandomGeneratedKey) // Encrypt with plaintext random key
                        );

                bool dbresult = _dbcontext.AddUser(newUser);
                addUserResult = dbresult ? AddUserResult.Successful : AddUserResult.Failed;
            }
            else
            {
                addUserResult = validation;
            }

            return addUserResult;
        }  

        /******************************************************************/
        public DeleteUserResult DeleteUser(string userUuid, int expectedNumPasswords)
        {
            DeleteUserResult result = DeleteUserResult.Failed;

            if (String.IsNullOrEmpty(userUuid) || expectedNumPasswords < 0)
                return DeleteUserResult.Failed;

            User user = _dbcontext.GetUserByUsername(userUuid);

            if (user != null)
            {
                bool success = _dbcontext.DeleteUser(user, expectedNumPasswords);

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
        public UserInformationResult ModifyUser(string userUuid, User modifiedUser, string encryptionKey, EncryptionParameters parameters)
        {
            UserInformationResult result = UserInformationResult.Failed;

            if (string.IsNullOrEmpty(userUuid) || modifiedUser == null || string.IsNullOrEmpty(encryptionKey) || parameters == null)
                return UserInformationResult.Failed;

            var validation = modifiedUser.VerifyUserInformation();

            if (validation == UserInformationResult.Success)
            {
                User dbUser = _dbcontext.GetUserByUuid(userUuid);
                IEncryptionService encryptionService = _encryptDecryptFactory.GetEncryptionService(parameters);

                User encryptedModifiedUser = new User
                (
                    uniqueID :     dbUser.Uuid,
                    encryptedKey : dbUser.EncryptedKey,
                    username :     dbUser.Username,                        
                    hash :         dbUser.Hash,
                    firstName :    encryptionService.Encrypt(modifiedUser.FirstName,   encryptionKey),
                    lastName :     encryptionService.Encrypt(modifiedUser.LastName,    encryptionKey),
                    phoneNumber :  encryptionService.Encrypt(modifiedUser.PhoneNumber, encryptionKey),
                    email :        encryptionService.Encrypt(modifiedUser.Email,       encryptionKey)
                );

                bool success = _dbcontext.ModifyUser(dbUser, encryptedModifiedUser);

                if (success)
                    result = UserInformationResult.Success; 
                else
                    result = UserInformationResult.Failed;
            }
            else
            {
                result = validation;
            }

            return result;
        }

        /******************************************************************/
        public ValidateUserPasswordResult ChangeUserPassword(
            string userUuid, 
            string originalPassword, 
            string newPassword, 
            string confirmPassword, 
            string encryptionKey, 
            EncryptionParameters encParameters,
            MasterPasswordParameters hashParameters)
        {
            ValidateUserPasswordResult result = ValidateUserPasswordResult.Failed;

            if (string.IsNullOrEmpty(userUuid) ||
                string.IsNullOrEmpty(originalPassword) || 
                string.IsNullOrEmpty(newPassword) || 
                string.IsNullOrEmpty(confirmPassword) || 
                string.IsNullOrEmpty(encryptionKey) ||
                encParameters == null ||
                hashParameters == null)
            {
                return ValidateUserPasswordResult.Failed;
            }

            if (string.Equals(newPassword, confirmPassword, StringComparison.CurrentCulture))
            {
                User dbUser = _dbcontext.GetUserByUuid(userUuid);

                if (VerifyUserPassword(dbUser.Username, originalPassword))
                {
                    ValidateUserPasswordResult verifyPass = User.VerifyPasswordRequirements(newPassword);

                    if (verifyPass != ValidateUserPasswordResult.Success)
                    {
                        result = verifyPass;
                    }
                    else
                    {
                        UserEncrypedData newEncryptedData = _masterPassword.GenerateMasterHash(newPassword, hashParameters);
                        IEncryptionService encryptionService = _encryptDecryptFactory.GetEncryptionService(encParameters);

                        User newUser = new User(
                            dbUser.Uuid,
                            encryptionService.Encrypt(encryptionKey, newPassword), // Encrypt the random key with the users password
                            dbUser.Username,
                            _masterPassword.FlattenHash(newEncryptedData),
                            dbUser.FirstName,
                            dbUser.LastName,
                            dbUser.PhoneNumber,
                            dbUser.Email
                        );

                        if (_dbcontext.ModifyUser(dbUser, newUser))
                            result = ValidateUserPasswordResult.Success;
                        else
                            result = ValidateUserPasswordResult.Failed;
                    }
                }
                else
                {
                    result = ValidateUserPasswordResult.InvalidPassword;
                }
            }
            else
            {
                result = ValidateUserPasswordResult.PasswordsDoNotMatch;
            }

            return result;
        }

        /******************************************************************/
        public bool VerifyUserPassword(string username, string password)
        {
            bool result = _authenticationService.VerifyUserCredentials(username, password);
            return result;
        }

        /******************************************************************/
        public User GetUser(string userUuid)
        {
            User user = null;

            if (!String.IsNullOrEmpty(userUuid))
            {
                user = _dbcontext.GetUserByUuid(userUuid);
            }     

            return user;
        }

        /******************************************************************/
        public User GetUserByUsername(string username)
        {
            User user = null;

            if (!String.IsNullOrEmpty(username))
            {
                user = _dbcontext.GetUserByUsername(username);
            }

            return user;
        }

        /******************************************************************/
        public int GetMinimumPasswordLength()
        {
            return User.GetMinimumPasswordLength();
        }

        /*PRIVATE METHODS**************************************************/
        private AddUserResult ValidateNewUser(User user)
        {
            AddUserResult dataValidationResult = AddUserResult.Successful;

            UserInformationResult verifyUser = user.VerifyUserInformation();
            ValidateUserPasswordResult verifyPassword = user.VerifyPlaintextPasswordRequirements();

            if (!user.VerifyUsernameRequirements())
            {
                dataValidationResult = AddUserResult.UsernameNotValid;
            }
            else if (verifyPassword != ValidateUserPasswordResult.Success)
            {
                switch (verifyPassword)
                {
                    case ValidateUserPasswordResult.Failed:
                        dataValidationResult = AddUserResult.PasswordNotValid;
                        break;

                    case ValidateUserPasswordResult.LengthRequirementNotMet:
                        dataValidationResult = AddUserResult.LengthRequirementNotMet;
                        break;

                    case ValidateUserPasswordResult.NoLowerCaseCharacter:
                        dataValidationResult = AddUserResult.NoLowerCaseCharacter;
                        break;

                    case ValidateUserPasswordResult.NoNumber:
                        dataValidationResult = AddUserResult.NoNumber;
                        break;

                    case ValidateUserPasswordResult.NoSpecialCharacter:
                        dataValidationResult = AddUserResult.NoSpecialCharacter;
                        break;

                    case ValidateUserPasswordResult.NoUpperCaseCharacter:
                        dataValidationResult = AddUserResult.NoUpperCaseCharacter;
                        break;

                    case ValidateUserPasswordResult.PasswordsDoNotMatch:
                        dataValidationResult = AddUserResult.PasswordNotValid;
                        break;

                    default:
                        dataValidationResult = AddUserResult.PasswordNotValid;
                        break;
                }
            }
            else if (verifyUser != UserInformationResult.Success)
            {
                switch (verifyUser)
                {
                    case UserInformationResult.InvalidEmail:
                        dataValidationResult = AddUserResult.EmailNotValid;
                        break;

                    case UserInformationResult.InvalidFirstName:
                        dataValidationResult = AddUserResult.FirstNameNotValid;
                        break;

                    case UserInformationResult.InvalidLastName:
                        dataValidationResult = AddUserResult.LastNameNotValid;
                        break;

                    case UserInformationResult.InvalidPhoneNumber:
                        dataValidationResult = AddUserResult.PhoneNumberNotValid;
                        break;

                    case UserInformationResult.Failed:
                        dataValidationResult = AddUserResult.Failed;
                        break;
                }
            }

            return dataValidationResult;
        }

        /******************************************************************/
        private bool UserExists(User user)
        {
            bool result = false;
            if (_dbcontext.GetUserByUsername(user.Username) != null)
                result = true;
            return result;
        }

        /*STATIC METHODS***************************************************/

    } // UserService CLASS
}

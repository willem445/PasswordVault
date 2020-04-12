using PasswordVault.Data;
using PasswordVault.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using PasswordVault.Utilities;

namespace PasswordVault.Services
{
    public class PasswordService : IPasswordService
    {
        /*CONSTANTS********************************************************/

        /*FIELDS***********************************************************/
        private IDatabase _dbContext;
        private IEncryptionServiceFactory _encryptionServiceFactory;

        /*PROPERTIES*******************************************************/

        /*CONSTRUCTORS*****************************************************/
        public PasswordService(IDatabase dbContext, IEncryptionServiceFactory encryptionServiceFactory)
        {
            _dbContext = dbContext;
            _encryptionServiceFactory = encryptionServiceFactory;
        }

        /*PUBLIC METHODS***************************************************/
        public AddPasswordResult AddPassword(string userUuid, Password password, string key, EncryptionParameters parameters)
        {
            AddPasswordResult result;
            ValidatePassword addResult = ValidatePassword.Failed;
            Int64 uniqueId = -1;

            if (password == null)
            {
                return new AddPasswordResult(ValidatePassword.Failed, uniqueId);
            }

            ValidatePassword verifyResult = VerifyAddPasswordFields(password);

            if (verifyResult == ValidatePassword.Success)
            {
                Password encryptPassword = ConvertPlaintextPasswordToEncryptedPassword(password, key, parameters); // Need to first encrypt the password
                uniqueId = _dbContext.AddPassword(ConvertToEncryptedDatabasePassword(userUuid, encryptPassword, key, parameters));
                addResult = ValidatePassword.Success;
            }
            else
            {
                addResult = verifyResult;
            }

            result = new AddPasswordResult(addResult, uniqueId);
            return result;
        }

        public DeletePasswordResult DeletePassword(Int64 passwordUuid)
        {
            DeletePasswordResult result = DeletePasswordResult.Failed;

            bool dbResult = _dbContext.DeletePassword(passwordUuid);

            if (dbResult)
            {
                result = DeletePasswordResult.Success;
            }

            return result;
        }

        public ValidatePassword ModifyPassword(string userUuid, Password modifiedPassword, string key, EncryptionParameters parameters)
        {
            if (string.IsNullOrEmpty(userUuid) || modifiedPassword == null || string.IsNullOrEmpty(key) || parameters == null)
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Arguments cannot be null or empty."));

            ValidatePassword result = ValidatePassword.Failed;

            ValidatePassword verifyResult = VerifyAddPasswordFields(modifiedPassword);

            if (verifyResult == ValidatePassword.Success)
            {
                Password encryptedPassword = ConvertPlaintextPasswordToEncryptedPassword(modifiedPassword, key, parameters);
                bool dbResult = _dbContext.ModifyPassword(ConvertToEncryptedDatabasePassword(userUuid, encryptedPassword, key, parameters));

                if (dbResult)
                {
                    result = ValidatePassword.Success;
                }
                else
                {
                    result = ValidatePassword.Failed;
                }
            }
            else
            {
                result = verifyResult;
            }

            return result;
        }

        public string GeneratePassword(int length)
        {
            string result = KeyGenerator.GenerateRandomPassword(length);

            return result;
        }

        public List<Password> GetPasswords(string userUuid, string key, EncryptionParameters parameters)
        {
            List<DatabasePassword> databasePasswords = null;
            List<Password> passwords = new List<Password>();

            databasePasswords = _dbContext.GetUserPasswordsByUuid(userUuid);
            IEncryptionService encryptionService = _encryptionServiceFactory.GetEncryptionService(parameters);

            foreach (var databasePassword in databasePasswords)
            {
                Password password = new Password(
                    databasePassword.UniqueID,
                    encryptionService.Decrypt(databasePassword.Application, key),
                    encryptionService.Decrypt(databasePassword.Username,    key),
                    encryptionService.Decrypt(databasePassword.Email,       key),
                    encryptionService.Decrypt(databasePassword.Description, key),
                    encryptionService.Decrypt(databasePassword.Website,     key),
                    encryptionService.Decrypt(databasePassword.Passphrase,  key),
                    encryptionService.Decrypt(databasePassword.Category,    key)
                    );

                passwords.Add(password);
            }

            return passwords;
        }

        /*PRIVATE METHODS**************************************************/
        private ValidatePassword VerifyAddPasswordFields(Password password)
        {
            return Password.Validate(password);
        }

        private Password ConvertPlaintextPasswordToEncryptedPassword(Password password, string key, EncryptionParameters parameters)
        {
            IEncryptionService encryptionService = _encryptionServiceFactory.GetEncryptionService(parameters);

            return new Password(
                password.UniqueID,
                password.Application,
                password.Username,
                password.Email,
                password.Description,
                password.Website,
                encryptionService.Encrypt(password.Passphrase, key)
                );
        }

        private DatabasePassword ConvertToEncryptedDatabasePassword(string uuid, Password password, string key, EncryptionParameters parameters)
        {
            IEncryptionService encryptionService = _encryptionServiceFactory.GetEncryptionService(parameters);

            return new DatabasePassword(
                uniqueID:    password.UniqueID,
                useruuid:    uuid,
                application: encryptionService.Encrypt(password.Application, key),
                username:    encryptionService.Encrypt(password.Username,    key),
                email:       encryptionService.Encrypt(password.Email,       key),
                description: encryptionService.Encrypt(password.Description, key),
                website:     encryptionService.Encrypt(password.Website,     key),
                passphrase:  password.Passphrase, // Password is already encrypted
                category:    encryptionService.Encrypt(password.Category,    key)
                );
        }

        /*STATIC METHODS***************************************************/

    } // PasswordService CLASS
}

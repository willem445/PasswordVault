using PasswordVault.Data;
using PasswordVault.Models;
using System;
using System.Collections.Generic;

namespace PasswordVault.Services
{
    public class PasswordService : IPasswordService
    {
        /*CONSTANTS********************************************************/

        /*FIELDS***********************************************************/
        private IDatabase _dbContext;
        private IEncryptionService _encryptionService;

        /*PROPERTIES*******************************************************/

        /*CONSTRUCTORS*****************************************************/
        public PasswordService(IDatabase dbContext, IEncryptionService encryptionService)
        {
            _dbContext = dbContext;
            _encryptionService = encryptionService;
        }

        /*PUBLIC METHODS***************************************************/
        public AddPasswordResult AddPassword(string userUuid, Password password, string key)
        {
            AddPasswordResult result;
            AddModifyPasswordResult addResult = AddModifyPasswordResult.Failed;
            Int64 uniqueId = -1;

            if (password == null)
            {
                return new AddPasswordResult(AddModifyPasswordResult.Failed, uniqueId);
            }

            AddModifyPasswordResult verifyResult = VerifyAddPasswordFields(password);

            if (verifyResult == AddModifyPasswordResult.Success)
            {
                Password encryptPassword = ConvertPlaintextPasswordToEncryptedPassword(password, key); // Need to first encrypt the password
                uniqueId = _dbContext.AddPassword(ConvertToEncryptedDatabasePassword(userUuid, encryptPassword, key));
                addResult = AddModifyPasswordResult.Success;
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

        public AddModifyPasswordResult ModifyPassword(string userUuid, Password modifiedPassword, string key)
        {
            AddModifyPasswordResult result = AddModifyPasswordResult.Failed;

            AddModifyPasswordResult verifyResult = VerifyAddPasswordFields(modifiedPassword);

            if (verifyResult == AddModifyPasswordResult.Success)
            {
                Password encryptedPassword = ConvertPlaintextPasswordToEncryptedPassword(modifiedPassword, key);
                bool dbResult = _dbContext.ModifyPassword(ConvertToEncryptedDatabasePassword(userUuid, encryptedPassword, key));

                if (dbResult)
                {
                    result = AddModifyPasswordResult.Success;
                }
                else
                {
                    result = AddModifyPasswordResult.Failed;
                }
            }
            else
            {
                result = verifyResult;
            }

            return result;
        }

        public string GeneratePasswordKey(int length)
        {
            string result = KeyGenerator.GetUniqueKey(length);

            return result;
        }

        public List<Password> GetPasswords(string userUuid, string key)
        {
            List<DatabasePassword> databasePasswords = null;
            List<Password> passwords = new List<Password>();

            databasePasswords = _dbContext.GetUserPasswordsByGUID(userUuid);

            foreach (var databasePassword in databasePasswords)
            {
                Password password = new Password(
                    databasePassword.UniqueID,
                    _encryptionService.Decrypt(databasePassword.Application, key),
                    _encryptionService.Decrypt(databasePassword.Username,    key),
                    _encryptionService.Decrypt(databasePassword.Email,       key),
                    _encryptionService.Decrypt(databasePassword.Description, key),
                    _encryptionService.Decrypt(databasePassword.Website,     key),
                    _encryptionService.Decrypt(databasePassword.Passphrase,  key)
                    );

                passwords.Add(password);
            }

            return passwords;
        }

        /*PRIVATE METHODS**************************************************/
        private AddModifyPasswordResult VerifyAddPasswordFields(Password password)
        {
            AddModifyPasswordResult result = AddModifyPasswordResult.Success;

            if (password != null)
            {
                if (string.IsNullOrEmpty(password.Passphrase))
                {
                    result = AddModifyPasswordResult.PassphraseError;
                }

                if (string.IsNullOrEmpty(password.Username))
                {
                    result = AddModifyPasswordResult.UsernameError;
                }

                if (string.IsNullOrEmpty(password.Application))
                {
                    result = AddModifyPasswordResult.ApplicationError;
                }

                if (password.Description == null)
                {
                    result = AddModifyPasswordResult.DescriptionError;
                }

                if (password.Website == null)
                {
                    result = AddModifyPasswordResult.WebsiteError;
                }
                else if (password.Website.Length != 0)
                {
                    if (!UriUtilities.IsValidUri(password.Website))
                    {
                        result = AddModifyPasswordResult.WebsiteError;
                    }
                }

                if (password.Email == null)
                {
                    result = AddModifyPasswordResult.EmailError;
                }
                else if (password.Email.Length != 0)
                {
                    if (!password.Email.Contains("@") || !password.Email.Contains("."))
                    {
                        result = AddModifyPasswordResult.EmailError;
                    }
                }
            }

            return result;
        }

        private Password ConvertPlaintextPasswordToEncryptedPassword(Password password, string key)
        {
            return new Password(
                password.UniqueID,
                password.Application,
                password.Username,
                password.Email,
                password.Description,
                password.Website,
                _encryptionService.Encrypt(password.Passphrase, key)
                );
        }

        private DatabasePassword ConvertToEncryptedDatabasePassword(string uuid, Password password, string key)
        {
            return new DatabasePassword(
                password.UniqueID,
                uuid, 
                _encryptionService.Encrypt(password.Application, key),
                _encryptionService.Encrypt(password.Username,    key),
                _encryptionService.Encrypt(password.Email,       key),
                _encryptionService.Encrypt(password.Description, key),
                _encryptionService.Encrypt(password.Website,     key),
                password.Passphrase // Password is already encrypted
                );
        }

        /*STATIC METHODS***************************************************/

    } // PasswordService CLASS
}

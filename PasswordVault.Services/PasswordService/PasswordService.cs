using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PasswordVault.Models;
using PasswordVault.Data;

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

        public AddModifyPasswordResult AddPassword(string userUuid, Password password)
        {
            throw new NotImplementedException();
        }

        public DeletePasswordResult DeletePassword(string userUuid, string passwordUuid)
        {
            throw new NotImplementedException();
        }

        public string GeneratePasswordKey(int length)
        {
            string result = KeyGenerator.GetUniqueKey(length);

            return result;
        }

        public List<Password> GetPasswords(string userUuid, string decryptionKey)
        {
            List<DatabasePassword> databasePasswords = null;
            List<Password> passwords = new List<Password>();

            databasePasswords = _dbContext.GetUserPasswordsByGUID(userUuid);

            foreach (var databasePassword in databasePasswords)
            {
                Password password = new Password(
                    databasePassword.UniqueID,
                    _encryptionService.Decrypt(databasePassword.Application, decryptionKey),
                    _encryptionService.Decrypt(databasePassword.Username,    decryptionKey),
                    _encryptionService.Decrypt(databasePassword.Email,       decryptionKey),
                    _encryptionService.Decrypt(databasePassword.Description, decryptionKey),
                    _encryptionService.Decrypt(databasePassword.Website,     decryptionKey),
                    _encryptionService.Decrypt(databasePassword.Passphrase,  decryptionKey)
                    );

                passwords.Add(password);
            }

            return passwords;
        }

        public AddModifyPasswordResult ModifyPassword(string userUuid, Password modifiedPassword)
        {
            throw new NotImplementedException();
        }

        /*PUBLIC METHODS***************************************************/

        /*PRIVATE METHODS**************************************************/

        /*STATIC METHODS***************************************************/

    } // PasswordService CLASS
}

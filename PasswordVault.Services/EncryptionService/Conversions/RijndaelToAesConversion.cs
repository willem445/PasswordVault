using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PasswordVault.Models;
using PasswordVault.Data;


namespace PasswordVault.Services
{
    public class RijndaelToAesConversion : IEncryptionConversion
    {
        /*CONSTANTS********************************************************/

        /*FIELDS***********************************************************/

        /*PROPERTIES*******************************************************/
        private IPasswordService _passwordService;

        /*CONSTRUCTORS*****************************************************/
        public RijndaelToAesConversion(IPasswordService passwordService, IUserService userService)
        {
            _passwordService = passwordService;
        }

        /*PUBLIC METHODS***************************************************/
        public void Convert(User user, EncryptionSizes originalSizes, EncryptionSizes newSizes)
        {
            EncryptionServiceParameters originalParameters = new EncryptionServiceParameters(EncryptionService.RijndaelManaged, originalSizes);
            EncryptionServiceParameters newParameters = new EncryptionServiceParameters(EncryptionService.Aes, newSizes);
            List<Password> passwords = _passwordService.GetPasswords(user.GUID, user.PlainTextRandomKey, originalParameters);

            foreach (var password in passwords)
            {
                _passwordService.ModifyPassword(user.GUID, password, user.PlainTextRandomKey, newParameters);
            }
        }

        public void Convert(string username, string passphrase)
        {
            //_passwordService.Login(username, passphrase);
            //User user = _passwordService.GetCurrentUser();
            //User dbUser = _dbContext.GetUserByUsername(username);
            //List<Password> rijndaelPasswords = _passwordService.GetPasswords();

            //List<Password> decryptedPasswords = new List<Password>();
            //foreach (var password in rijndaelPasswords)
            //{
            //    decryptedPasswords.Add(_passwordService.DecryptPassword(password));
            //}

            //List<DatabasePassword> orignalPasswords = _dbContext.GetUserPasswordsByGUID(user.GUID);

            //IEncryptionService aes = new AesEncryption();

            //var newKey = aes.Encrypt(user.PlainTextRandomKey, passphrase);
            //List<DatabasePassword> newPasswords = new List<DatabasePassword>();

            //foreach (var item in decryptedPasswords)
            //{
            //    DatabasePassword dbPassword = new DatabasePassword(
            //        item.UniqueID,
            //        user.GUID,
            //        aes.Encrypt(item.Application, user.PlainTextRandomKey),
            //        aes.Encrypt(item.Username, user.PlainTextRandomKey),
            //        aes.Encrypt(item.Email, user.PlainTextRandomKey),
            //        aes.Encrypt(item.Description, user.PlainTextRandomKey),
            //        aes.Encrypt(item.Website, user.PlainTextRandomKey),
            //        aes.Encrypt(item.Passphrase, user.PlainTextRandomKey));

            //    newPasswords.Add(dbPassword);
            //}

            //User newUser = new User(dbUser.GUID,
            //    newKey, user.Username,
            //    dbUser.Iterations,
            //    dbUser.Salt,
            //    dbUser.Hash,
            //    aes.Encrypt(user.FirstName, user.PlainTextRandomKey),
            //    aes.Encrypt(user.LastName, user.PlainTextRandomKey),
            //    aes.Encrypt(user.PhoneNumber, user.PlainTextRandomKey),
            //    aes.Encrypt(user.Email, user.PlainTextRandomKey));

            //_dbContext.ModifyUser(user, newUser);

            //int count = 0;
            //foreach (var item in newPasswords)
            //{
            //    _dbContext.ModifyPassword(orignalPasswords[count], item);
            //    count++;
            //}
        }

        

        /*PRIVATE METHODS**************************************************/

        /*STATIC METHODS***************************************************/

    } // RijndaelToAesConversion CLASS
}

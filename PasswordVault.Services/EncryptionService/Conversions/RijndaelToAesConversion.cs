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
        private IUserService _userService;

        /*CONSTRUCTORS*****************************************************/
        public RijndaelToAesConversion(IPasswordService passwordService, IUserService userService)
        {
            _passwordService = passwordService;
            _userService = userService;
        }

        /*PUBLIC METHODS***************************************************/
        public void Convert(User user, string password, EncryptionSizes originalSizes, EncryptionSizes newSizes)
        {
            EncryptionServiceParameters originalParameters = new EncryptionServiceParameters(EncryptionService.RijndaelManaged, originalSizes);
            EncryptionServiceParameters newParameters = new EncryptionServiceParameters(EncryptionService.Aes, newSizes);
            List<Password> passwords = _passwordService.GetPasswords(user.GUID, user.PlainTextRandomKey, originalParameters);

            //User newModifiedUser = new User
            //    (
            //        user.GUID,
            //        user.EncryptedKey,
            //        user.Username,
            //        user.Iterations,
            //        user.Salt,
            //        user.Hash,
            //        user.FirstName,
            //        user.LastName,
            //        user.PhoneNumber,
            //        user.Email,
            //        (int)EncryptionService.Aes,
            //        newSizes.BlockSize,
            //        newSizes.KeySize,
            //        newSizes.Iterations
            //    );
            _userService.ChangeUserPassword(user.GUID, password, password, password, user.PlainTextRandomKey, newParameters);
            _userService.ModifyUser(user.GUID, user, user.PlainTextRandomKey, newParameters);

            foreach (var pass in passwords)
            {
                _passwordService.ModifyPassword(user.GUID, pass, user.PlainTextRandomKey, newParameters);
            }         
        }

        /*PRIVATE METHODS**************************************************/

        /*STATIC METHODS***************************************************/

    } // RijndaelToAesConversion CLASS
}

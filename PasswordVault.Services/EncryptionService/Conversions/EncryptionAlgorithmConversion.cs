using PasswordVault.Models;
using System.Collections.Generic;


namespace PasswordVault.Services
{
    public class EncryptionAlgorithmConversion : IEncryptionConversion
    {
        /*CONSTANTS********************************************************/

        /*FIELDS***********************************************************/

        /*PROPERTIES*******************************************************/
        private IPasswordService _passwordService;
        private IUserService _userService;

        /*CONSTRUCTORS*****************************************************/
        public EncryptionAlgorithmConversion(IPasswordService passwordService, IUserService userService)
        {
            _passwordService = passwordService;
            _userService = userService;
        }

        /*PUBLIC METHODS***************************************************/
        public void Convert(User user, string password, EncryptionParameters originalParams, EncryptionParameters newParams)
        {
            //List<Password> passwords = _passwordService.GetPasswords(user.Uuid, user.PlainTextRandomKey, originalParams);

            //_userService.ChangeUserPassword(user.Uuid, password, password, password, user.PlainTextRandomKey, newParams);
            //_userService.ModifyUser(user.Uuid, user, user.PlainTextRandomKey, newParams);

            //foreach (var pass in passwords)
            //{
            //    _passwordService.ModifyPassword(user.Uuid, pass, user.PlainTextRandomKey, newParams);
            //}         
        }

        /*PRIVATE METHODS**************************************************/

        /*STATIC METHODS***************************************************/

    } // RijndaelToAesConversion CLASS
}

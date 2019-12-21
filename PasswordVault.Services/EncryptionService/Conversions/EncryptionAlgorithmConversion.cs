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
        public void Convert(User user, string password, EncryptionServiceParameters originalParams, EncryptionServiceParameters newParams)
        {
            List<Password> passwords = _passwordService.GetPasswords(user.GUID, user.PlainTextRandomKey, originalParams);

            _userService.ChangeUserPassword(user.GUID, password, password, password, user.PlainTextRandomKey, newParams);
            _userService.ModifyUser(user.GUID, user, user.PlainTextRandomKey, newParams);

            foreach (var pass in passwords)
            {
                _passwordService.ModifyPassword(user.GUID, pass, user.PlainTextRandomKey, newParams);
            }         
        }

        /*PRIVATE METHODS**************************************************/

        /*STATIC METHODS***************************************************/

    } // RijndaelToAesConversion CLASS
}

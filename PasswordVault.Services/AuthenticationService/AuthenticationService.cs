using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PasswordVault.Models;
using PasswordVault.Data;

namespace PasswordVault.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        /*CONSTANTS********************************************************/

        /*FIELDS***********************************************************/
        private IMasterPassword _masterPassword;
        private IEncryptionService _encryptionService;
        private IDatabase _dbContext;

        /*PROPERTIES*******************************************************/

        /*CONSTRUCTORS*****************************************************/
        public AuthenticationService(IMasterPassword masterPassword, IEncryptionService encryptionService, IDatabase dbContext)
        {
            _masterPassword = masterPassword;
            _encryptionService = encryptionService;
            _dbContext = dbContext;
        }



        /*PUBLIC METHODS***************************************************/
        public AuthenticateReturn Authenticate(string username, string password)
        {
            AuthenticateReturn result;

            User userResult = new User(false);
            AuthenticateResult authResult = AuthenticateResult.Failed;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                authResult = AuthenticateResult.Failed;
            }

            if (!_dbContext.UserExistsByUsername(username))
            {
                authResult = AuthenticateResult.UsernameDoesNotExist;
            }
            else
            {

            }

            result = new AuthenticateReturn(authResult, userResult);
            return result;
        }

        /*PRIVATE METHODS**************************************************/

        /*STATIC METHODS***************************************************/

    } // AuthenticationService CLASS
}

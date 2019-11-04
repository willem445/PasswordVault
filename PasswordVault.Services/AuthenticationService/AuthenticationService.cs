using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PasswordVault.Models;
using PasswordVault.Data;
using System.Globalization;

namespace PasswordVault.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        /*CONSTANTS********************************************************/

        /*FIELDS***********************************************************/
        private ITokenService _tokenService;
        private IMasterPassword _masterPassword;
        private IEncryptionService _encryptionService;
        private IDatabase _dbContext;

        /*PROPERTIES*******************************************************/

        /*CONSTRUCTORS*****************************************************/
        public AuthenticationService(ITokenService tokenService, IMasterPassword masterPassword, IEncryptionService encryptionService, IDatabase dbContext)
        {
            _tokenService = tokenService;
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
                User user = _dbContext.GetUserByUsername(username);

                // Hash password with user.Salt and compare to user.Hash
                bool valid = _masterPassword.VerifyPassword(password, 
                                                            user.Salt, 
                                                            user.Hash, 
                                                            Convert.ToInt32(user.Iterations, CultureInfo.CurrentCulture));

                if (valid)
                {
                    string randomKey = _encryptionService.Decrypt(user.EncryptedKey, password);

                    userResult = new User(user.GUID,
                                          user.Username,
                                          randomKey,
                                          _encryptionService.Decrypt(user.FirstName, randomKey),
                                          _encryptionService.Decrypt(user.LastName, randomKey),
                                          _encryptionService.Decrypt(user.PhoneNumber, randomKey),
                                          _encryptionService.Decrypt(user.Email, randomKey),
                                          true);

                    userResult.Token = _tokenService.GenerateJwtToken(user.GUID);

                    authResult = AuthenticateResult.Successful;
                }
                else
                {
                    authResult = AuthenticateResult.PasswordIncorrect;
                }
            }

            result = new AuthenticateReturn(authResult, userResult);
            return result;
        }

        /*PRIVATE METHODS**************************************************/

        /*STATIC METHODS***************************************************/

    } // AuthenticationService CLASS
}

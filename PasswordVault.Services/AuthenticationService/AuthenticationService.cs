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
        private IEncryptionServiceFactory _encryptionServiceFactory;
        private IDatabase _dbContext;

        /*PROPERTIES*******************************************************/

        /*CONSTRUCTORS*****************************************************/
        public AuthenticationService(ITokenService tokenService, IMasterPassword masterPassword, IEncryptionServiceFactory encryptionServiceFactory, IDatabase dbContext)
        {
            _tokenService = tokenService;
            _masterPassword = masterPassword;
            _encryptionServiceFactory = encryptionServiceFactory;
            _dbContext = dbContext;        
        }

        /*PUBLIC METHODS***************************************************/
        public AuthenticateReturn Authenticate(string username, string password, EncryptionServiceParameters parameters)
        {
            AuthenticateReturn result;

            User userResult = new User(false);
            AuthenticateResult authResult = AuthenticateResult.Failed;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return new AuthenticateReturn(AuthenticateResult.Failed, userResult);
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
                    IEncryptionService encryptionService = _encryptionServiceFactory.Get(parameters);

                    string randomKey = encryptionService.Decrypt(user.EncryptedKey, password);

                    userResult = new User(user.GUID,
                                          user.Username,
                                          randomKey,
                                          encryptionService.Decrypt(user.FirstName, randomKey),
                                          encryptionService.Decrypt(user.LastName, randomKey),
                                          encryptionService.Decrypt(user.PhoneNumber, randomKey),
                                          encryptionService.Decrypt(user.Email, randomKey),
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

        public bool VerifyUserCredentials(string username, string password)
        {
            bool result = false;

            if (!_dbContext.UserExistsByUsername(username))
            {
                result = false;
            }
            else
            {
                User user = _dbContext.GetUserByUsername(username);

                // Hash password with user.Salt and compare to user.Hash
                result = _masterPassword.VerifyPassword(password,
                                                            user.Salt,
                                                            user.Hash,
                                                            Convert.ToInt32(user.Iterations, CultureInfo.CurrentCulture));
            }

            return result;
        }

        /*PRIVATE METHODS**************************************************/

        /*STATIC METHODS***************************************************/

    } // AuthenticationService CLASS
}

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
        public AuthenticateReturn Authenticate(string username, string password, EncryptionParameters parameters)
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

                // Extract parameters from hash
                UserEncrypedData unflattened = _masterPassword.UnFlattenHash(user.Hash);
                MasterPasswordParameters hashParams = new MasterPasswordParameters(
                    new KeyDerivationParameters(
                        unflattened.KeyDevAlgorithm,
                        unflattened.KeySize,
                        unflattened.SaltSize,
                        unflattened.Iterations,
                        unflattened.DegreeOfParallelism,
                        unflattened.MemorySize),
                    -1);

                bool valid = _masterPassword.VerifyPassword(password, 
                                                            unflattened.Salt, 
                                                            unflattened.Hash, 
                                                            hashParams);

                if (valid)
                {
                    IEncryptionService encryptionService = _encryptionServiceFactory.GetEncryptionService(parameters);

                    string randomKey = encryptionService.Decrypt(user.EncryptedKey, password);

                    userResult = new User(user.Uuid,
                                          user.Username,
                                          randomKey,
                                          encryptionService.Decrypt(user.FirstName, randomKey),
                                          encryptionService.Decrypt(user.LastName, randomKey),
                                          encryptionService.Decrypt(user.PhoneNumber, randomKey),
                                          encryptionService.Decrypt(user.Email, randomKey),
                                          true);

                    userResult.Token = _tokenService.GenerateJwtToken(user.Uuid);

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

                // Extract parameters from hash
                UserEncrypedData unflattened = _masterPassword.UnFlattenHash(user.Hash);
                MasterPasswordParameters hashParams = new MasterPasswordParameters(
                    new KeyDerivationParameters(
                        unflattened.KeyDevAlgorithm,
                        unflattened.KeySize,
                        -1,
                        unflattened.Iterations,
                        unflattened.DegreeOfParallelism,
                        unflattened.MemorySize),
                    -1);

                // Hash password with user.Salt and compare to user.Hash
                bool valid = _masterPassword.VerifyPassword(password,
                                                            unflattened.Salt,
                                                            unflattened.Hash,
                                                            hashParams);
                result = valid;
            }
            return result;
        }

        /*PRIVATE METHODS**************************************************/

        /*STATIC METHODS***************************************************/

    } // AuthenticationService CLASS
}

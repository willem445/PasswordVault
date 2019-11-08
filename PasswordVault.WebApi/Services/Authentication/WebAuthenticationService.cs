using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using PasswordVault.Models;
using PasswordVault.Data;
using PasswordVault.Services;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace PasswordVault.WebApi
{
    public class WebAuthenticationService
    {
        //private IDatabase _dbContext;
        //private IMasterPassword _masterPassword;
        //private IEncryptionService _encryptionService;
        //private readonly AppSettings _appsettings;

        //public UserService(IOptions<AppSettings> appSettings, IDatabase dbContext, IMasterPassword masterPassword, IEncryptionService encryptionService)
        //{
        //    _dbContext = dbContext;
        //    _masterPassword = masterPassword;
        //    _encryptionService = encryptionService;
        //    _appsettings = appSettings.Value;
        //}

        //public User Authenticate(string username, string password)
        //{
        //    User result = new User(false);

        //    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        //    {
        //        return result;
        //    }

        //    if (!_dbContext.UserExistsByUsername(username))
        //    {
        //        return result;
        //    }
        //    else
        //    {
        //        User user = _dbContext.GetUserByUsername(username);

        //        bool valid = _masterPassword.VerifyPassword(password, user.Salt, user.Hash, Convert.ToInt32(user.Iterations, CultureInfo.CurrentCulture));

        //        if (valid)
        //        {
        //            string tempKey = _encryptionService.Decrypt(user.EncryptedKey, password);
        //            result = new User(user.GUID,
        //                                        user.Username,
        //                                        tempKey,
        //                                        _encryptionService.Decrypt(user.FirstName, tempKey),
        //                                        _encryptionService.Decrypt(user.LastName, tempKey),
        //                                        _encryptionService.Decrypt(user.PhoneNumber, tempKey),
        //                                        _encryptionService.Decrypt(user.Email, tempKey),
        //                                        true);

        //            var tokenHandler = new JwtSecurityTokenHandler();
        //            var key = Encoding.ASCII.GetBytes(_appsettings.Secret);
        //            var tokenDescriptor = new SecurityTokenDescriptor
        //            {
        //                Subject = new ClaimsIdentity(new Claim[]
        //                {
        //                    new Claim(ClaimTypes.Name, user.GUID.ToString())
        //                }),
        //                Expires = DateTime.UtcNow.AddDays(7),
        //                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //            };

        //            var token = tokenHandler.CreateToken(tokenDescriptor);
        //            result.Token = tokenHandler.WriteToken(token);
        //        }
        //    }

        //    return result;
        //}
    }
}

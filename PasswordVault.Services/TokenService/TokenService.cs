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

namespace PasswordVault.Services
{
    public class TokenService : ITokenService
    {
        /*CONSTANTS********************************************************/

        /*FIELDS***********************************************************/
        private readonly AppSettings _appsettings;

        /*PROPERTIES*******************************************************/

        /*CONSTRUCTORS*****************************************************/
        public TokenService()
        {
            _appsettings = AppSettings.Instance;
            _appsettings.Secret = "THIS IS USED TO SIGN AND VERIFY JWT TOKENS, REPLACE IT WITH YOUR OWN SECRET, IT CAN BE ANY STRING"; // TODO - 9 - Store this in app settings
        }

        /*PUBLIC METHODS***************************************************/
        public string GenerateJwtToken(string userUuid)
        {
            string resultToken = null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appsettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userUuid)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            resultToken = tokenHandler.WriteToken(token);

            return resultToken;
        }

        /*PRIVATE METHODS**************************************************/

        /*STATIC METHODS***************************************************/

    } // TokenService CLASS
}

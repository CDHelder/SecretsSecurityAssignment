using Microsoft.IdentityModel.Tokens;
using SecretsSecurityAssignment.Core.Data.Service;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSecurityAssignment.Service
{
    public class JWTService : IJWTService
    {
        public string ComputeHash(string password, string salt)
        {
            var bytePassword = Encoding.ASCII.GetBytes(password);
            var byteSalt = Encoding.ASCII.GetBytes(salt);
            var byteResult = new Rfc2898DeriveBytes(bytePassword, byteSalt, 10000);
            return Convert.ToBase64String(byteResult.GetBytes(24));
        }

        public string GenerateJWT(List<Claim> claims, string userSecurityKey)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(userSecurityKey));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = "traktor.nl",
                Audience = "aud",
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(token);
        }

        public string GenerateSalt()
        {
            var bytes = new byte[128 / 8];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}

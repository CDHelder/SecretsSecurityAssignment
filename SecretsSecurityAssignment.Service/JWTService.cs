using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
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

        public string GenerateJWT(List<Claim> claims)
        {
            //var key = new SymmetricSecurityKey
            //TODO: Maak deze (tip check LesSnippets)
            return string.Empty;
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

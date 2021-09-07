using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using SecretsSecurityAssignment.Core.Data.Service;
using SecretsSecurityAssignment.Core.UserEntities;
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
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor httpContextAccessor;
        private JwtSecurityTokenHandler tokenHandler;

        public JWTService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.httpContextAccessor = httpContextAccessor;
            tokenHandler = new();
        }

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

        public string GetSecurityKeyFromTokenInHttpContext()
        {
            var token = httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

            var userId = Int32.Parse(jwtSecurityToken.Claims.FirstOrDefault(a => a.Type == "certserialnumber").Value);
            var userSecurityKey = unitOfWork.UserRepository.GetById(userId).SecurityKey;

            return userSecurityKey;
        }

        public int GetUserIdFromTokenInHttpContext()
        {
            var token = httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

            var userId = Int32.Parse(jwtSecurityToken.Claims.FirstOrDefault(a => a.Type == "certserialnumber").Value);

            return userId;
        }

        public int GetUserIdFromTokenInHttpContext(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

            var userId = Int32.Parse(jwtSecurityToken.Claims.FirstOrDefault(a => a.Type == "certserialnumber").Value);

            return userId;
        }

        public string GetUserTypeFromTokenInHttpContext(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

            var userRole = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "role").Value;

            return userRole;
        }

        public SymmetricSecurityKey CreateSymmetricSecurityKey(string token)
        {
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

            var userId = Int32.Parse(jwtSecurityToken.Claims.FirstOrDefault(a => a.Type == "certserialnumber").Value);
            var userSecurityKey = unitOfWork.UserRepository.GetById(userId).SecurityKey;
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(userSecurityKey));

            return securityKey;
        }

        public string GetUserTypeFromTokenInHttpContext()
        {
            var token = httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

            var userRole = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "role").Value;

            return userRole;
        }
    }
}

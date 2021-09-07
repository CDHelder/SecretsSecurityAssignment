using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using SecretsSecurityAssignment.Core.UserEntities;
using System.Collections.Generic;
using System.Security.Claims;

namespace SecretsSecurityAssignment.Service
{
    public interface IJWTService
    {
        public string GenerateJWT(List<Claim> claims, string userSecurityKey);
        public string GenerateSalt();
        public string ComputeHash(string password, string salt);
        public int GetUserIdFromTokenInHttpContext();
        public int GetUserIdFromTokenInHttpContext(HttpContext context);
        public string GetSecurityKeyFromTokenInHttpContext();
        public string GetUserTypeFromTokenInHttpContext(HttpContext context);
        public SymmetricSecurityKey CreateSymmetricSecurityKey(string token);
    }
}
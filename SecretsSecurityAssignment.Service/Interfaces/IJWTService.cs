using System.Collections.Generic;
using System.Security.Claims;

namespace SecretsSecurityAssignment.Service
{
    public interface IJWTService
    {
        public string GenerateJWT(List<Claim> claims, string userSecurityKey);
        public string GenerateSalt();
        public string ComputeHash(string password, string salt);
    }
}
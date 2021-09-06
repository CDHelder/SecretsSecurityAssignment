using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using SecretsSecurityAssignment.Core.Data.Service;
using SecretsSecurityAssignment.Data.Service;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSecurityAssignment.Service.Middleware
{
    public class JWTMiddleware
    {
        private readonly RequestDelegate next;

        public JWTMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, IUnitOfWork unitOfWork)
        {
            //TODO: Middleware dependecy injection c#
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token == null)
            {
                await next(context);
            }
            else
            {
                await AttachUserToContext(context, token, unitOfWork);
                await next(context);
            }
        }

        public async Task AttachUserToContext(HttpContext context, string token, IUnitOfWork unitOfWork)
        {
            try
            {
                //var userSecurityKey = "dsfgsdfg";
                
                //TODO: get de userId en daarmee de related userSecurityKey
                //Token decrypten in de claims list de claim met type SerialNumber pakken ??

                var userId = Int32.Parse(context.User.Claims.Where(a => a.Type == ClaimTypes.SerialNumber).FirstOrDefault().Value);
                var userSecurityKey = unitOfWork.UserRepository.GetById(userId).SecurityKey;

                var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(userSecurityKey));

                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                if (context.User != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim("UserId", jwtToken.Claims.FirstOrDefault(x => x.Type == "UserId").Value),
                        new Claim(ClaimTypes.Name, jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value),
                        new Claim(ClaimTypes.Role, jwtToken.Claims.First(x => x.Type == ClaimTypes.Role).Value)
                    };

                    var identity = new ClaimsIdentity(claims);
                    context.User.AddIdentity(identity);
                }
            }
            catch (Exception ex)
            {
                //Account is not attached.
            }
        }
    }
}

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

        public async Task Invoke(HttpContext context, IJWTService jWTService)
        {
            //TODO: Middleware dependecy injection c#
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token == null)
            {
                await next(context);
            }
            else
            {
                await AttachUserToContext(context, token, jWTService);
                await next(context);
            }
        }

        public async Task AttachUserToContext(HttpContext context, string token , IJWTService jWTService)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                //var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
                //var stringUserId = jwtSecurityToken.Claims.FirstOrDefault(a => a.Type == "certserialnumber").Value;
                //var userId = Convert.ToInt32(stringUserId);

                //if (unitOfWork.UserRepository.GetById(userId).Blocked == true)
                //{
                //    return;
                //}

                var securityKey = jWTService.CreateSymmetricSecurityKey(token);
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
                        new Claim(ClaimTypes.SerialNumber, jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber).Value),
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

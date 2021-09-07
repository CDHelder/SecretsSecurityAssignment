using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SecretsSecurityAssignment.Core.UserEntities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace SecretsSecurityAssignment.Service.CustomAttributes
{
    public class AuthorizeUser : Attribute, IAuthorizationFilter
    {
        private readonly UserType userType;

        public AuthorizeUser(UserType userType)
        {
            this.userType = userType;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token == null)
            {
                context.Result = new JsonResult(string.Empty) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

            var stringUserId = jwtSecurityToken.Claims.FirstOrDefault(a => a.Type == "certserialnumber").Value;
            var userId = Convert.ToInt32(stringUserId);
            if (userId <= 0)
            {
                context.Result = new JsonResult(string.Empty) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }

            var userRole = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "role").Value;
            switch (userType)
            {
                case UserType.GovermentEmployee:
                    if (userRole == UserType.Civilian.ToString())
                    {
                        context.Result = new JsonResult(string.Empty) { StatusCode = StatusCodes.Status401Unauthorized };
                        return;
                    }
                    break;
                case UserType.SecretAgent:
                    if (userRole == UserType.Civilian.ToString() || userRole == UserType.GovermentEmployee.ToString())
                    {
                        context.Result = new JsonResult(string.Empty) { StatusCode = StatusCodes.Status401Unauthorized };
                        return;
                    }
                    break;
                default:
                    context.Result = new JsonResult(string.Empty) { StatusCode = StatusCodes.Status401Unauthorized };
                    return;
            }
        }
    }
}

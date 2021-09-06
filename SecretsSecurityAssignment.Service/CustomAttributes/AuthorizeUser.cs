using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SecretsSecurityAssignment.Core.UserEntities;
using System;
using System.Linq;
using System.Security.Claims;

namespace SecretsSecurityAssignment.Service.CustomAttributes
{
    public class AuthorizeUser : Attribute, IAuthorizationFilter
    {
        public UserType UserType { get; set; }

        //TODO: Moet ik die context steeds meegeven of kan ik deze ook in de relevante plekken aanspreken met IHttpContextAccessor
        //TODO: Maak Custom Attribute 
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //context.Result = new JsonResult(string.Empty) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}

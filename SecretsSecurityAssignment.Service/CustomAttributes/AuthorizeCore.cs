using Microsoft.AspNetCore.Http;
using SecretsSecurityAssignment.Core.UserEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSecurityAssignment.Service.CustomAttributes
{
    public class AuthorizeCore : IAuthorizeCore
    {
        private readonly IJWTService jWTService;

        public AuthorizeCore(IJWTService jWTService)
        {
            this.jWTService = jWTService;
        }

        public bool CheckUserId(HttpContext context)
        {
            var contextUserId = jWTService.GetUserIdFromTokenInHttpContext(context);

            if (contextUserId <= 0 || contextUserId == null)
            {
                return false;
            }

            return true;
        }

        public bool CheckUserType(HttpContext context, UserType userType)
        {
            var contextUserType = jWTService.GetUserTypeFromTokenInHttpContext(context);

            switch (userType)
            {
                case UserType.GovermentEmployee:
                    if (contextUserType == UserType.Civilian.ToString())
                    {
                        return false;
                    }
                    break;
                case UserType.SecretAgent:
                    if (contextUserType == UserType.Civilian.ToString() || contextUserType == UserType.GovermentEmployee.ToString())
                    {
                        return false;
                    }
                    break;
                default:
                    return false;
            }

            return true;
        }
    }
}

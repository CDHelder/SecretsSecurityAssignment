using Microsoft.AspNetCore.Http;
using SecretsSecurityAssignment.Core.UserEntities;

namespace SecretsSecurityAssignment.Service.CustomAttributes
{
    public interface IAuthorizeCore
    {
        public bool CheckUserId(HttpContext context);
        public bool CheckUserType(HttpContext context, UserType userType);

    }
}
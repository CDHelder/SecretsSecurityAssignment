using SecretsSecurityAssignment.Core.UserEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretsSecurityAssignment.WebApi.ViewModels
{
    public class CreateCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public UserType UserType { get; set; }
    }
}

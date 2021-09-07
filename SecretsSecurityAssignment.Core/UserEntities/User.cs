using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSecurityAssignment.Core.UserEntities
{
    public enum UserType
    {
        Civilian,
        GovermentEmployee,
        SecretAgent,
        Admin
    }

    public class User
    {
        public int Id { get; set; }
        public UserType UserType { get; set; }
        public string UserName { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        public string SecurityKey { get; set; }
        public bool Blocked { get; set; }
        public List<SensitiveSecret> SensitiveSecrets { get; set; }
        public List<StateSecret> StateSecrets { get; set; }
        public List<TopSecret> TopSecrets { get; set; }
    }
}

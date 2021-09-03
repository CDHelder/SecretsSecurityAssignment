using SecretsSecurityAssignment.Core.UserEntities;

namespace SecretsSecurityAssignment.Core
{
    public class SensitiveSecret : Secret
    {
        public int? UserId { get; set; }
        public User User { get; set; }
    }
}

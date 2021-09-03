using SecretsSecurityAssignment.Core.UserEntities;

namespace SecretsSecurityAssignment.Core
{
    public class Secret
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
    }
}

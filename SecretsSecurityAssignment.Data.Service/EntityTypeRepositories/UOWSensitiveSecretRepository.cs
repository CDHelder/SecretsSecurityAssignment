using SecretsSecurityAssignment.Core;

namespace SecretsSecurityAssignment.Data.Service.EntityTypeRepositories
{
    public class UOWSensitiveSecretRepository : GenericRepository<SensitiveSecret>
    {
        private readonly ApplicationDbContext dbContext;

        public UOWSensitiveSecretRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }


    }
}

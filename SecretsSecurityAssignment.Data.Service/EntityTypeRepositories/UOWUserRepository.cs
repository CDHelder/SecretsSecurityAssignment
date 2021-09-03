using SecretsSecurityAssignment.Core.UserEntities;

namespace SecretsSecurityAssignment.Data.Service.EntityTypeRepositories
{
    public class UOWUserRepository : GenericRepository<User>
    {
        private readonly ApplicationDbContext dbContext;

        public UOWUserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }


    }
}

using SecretsSecurityAssignment.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSecurityAssignment.Data.Service.EntityTypeRepositories
{
    public class UOWStateSecretRepository : GenericRepository<StateSecret>
    {
        private readonly ApplicationDbContext dbContext;

        public UOWStateSecretRepository(ApplicationDbContext dbContext): base(dbContext)
        {
            this.dbContext = dbContext;
        }


    }
}

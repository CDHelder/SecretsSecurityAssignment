using SecretsSecurityAssignment.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSecurityAssignment.Data.Service.EntityTypeRepositories
{
    public class UOWTopSecretRepository : GenericRepository<TopSecret>
    {
        private readonly ApplicationDbContext dbContext;

        public UOWTopSecretRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }


    }
}

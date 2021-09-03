using SecretsSecurityAssignment.Core.Data.Service;
using SecretsSecurityAssignment.Data.Service.EntityTypeRepositories;

namespace SecretsSecurityAssignment.Data.Service
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private UOWUserRepository userRepository;
        private UOWTopSecretRepository topSecretRepository;
        private UOWStateSecretRepository stateSecretRepository;
        private UOWSensitiveSecretRepository sensitiveSecretRepository;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public UOWSensitiveSecretRepository SensitiveSecretRepository
        {
            get => sensitiveSecretRepository ?? new UOWSensitiveSecretRepository(_dbContext);
            private set => sensitiveSecretRepository = value;
        }

        public UOWStateSecretRepository StateSecretRepository
        {
            get => stateSecretRepository ?? new UOWStateSecretRepository(_dbContext);
            private set => stateSecretRepository = value;
        }

        public UOWTopSecretRepository TopSecretRepository
        {
            get => topSecretRepository ?? new UOWTopSecretRepository(_dbContext);
            private set => topSecretRepository = value;
        }

        public UOWUserRepository UserRepository
        {
            get => userRepository ?? new UOWUserRepository(_dbContext);
            private set => userRepository = value;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public bool SaveChanges(int changes)
        {
            var rowsChanged = _dbContext.SaveChanges();
            if (rowsChanged == changes)
                return true;

            return false;
        }
    }
}

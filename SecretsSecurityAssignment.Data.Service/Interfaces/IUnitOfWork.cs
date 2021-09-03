using SecretsSecurityAssignment.Data.Service.EntityTypeRepositories;
using System;

namespace SecretsSecurityAssignment.Core.Data.Service
{
    public interface IUnitOfWork : IDisposable
    {
        public UOWSensitiveSecretRepository SensitiveSecretRepository { get; }
        public UOWStateSecretRepository StateSecretRepository { get; }
        public UOWTopSecretRepository TopSecretRepository { get; }
        public UOWUserRepository UserRepository { get; }
        bool SaveChanges(int changes);
    }
}

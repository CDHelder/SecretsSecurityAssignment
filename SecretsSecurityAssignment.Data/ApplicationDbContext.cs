using Microsoft.EntityFrameworkCore;
using SecretsSecurityAssignment.Core;
using SecretsSecurityAssignment.Core.UserEntities;
using System.Reflection;

namespace SecretsSecurityAssignment.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<SensitiveSecret> SensitiveSecrets { get; set; }
        public DbSet<StateSecret> StateSecrets { get; set; }
        public DbSet<TopSecret> TopSecrets { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}

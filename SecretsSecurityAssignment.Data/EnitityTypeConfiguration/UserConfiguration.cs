using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretsSecurityAssignment.Core.UserEntities;

namespace SecretsSecurityAssignment.Data.EnitityTypeConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasMany(u => u.SensitiveSecrets).WithOne(s => s.User).HasForeignKey(u => u.UserId);
            builder.HasMany(u => u.StateSecrets).WithOne(s => s.User).HasForeignKey(u => u.UserId);
            builder.HasMany(u => u.TopSecrets).WithOne(s => s.User).HasForeignKey(u => u.UserId);
        }
    }
}

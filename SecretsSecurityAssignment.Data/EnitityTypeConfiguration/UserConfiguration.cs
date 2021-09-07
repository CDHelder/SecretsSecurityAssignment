using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretsSecurityAssignment.Core.UserEntities;
using System.Collections.Generic;

namespace SecretsSecurityAssignment.Data.EnitityTypeConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            var data = new List<User>
            {
                new User
                {
                    Id = 1,
                    Blocked = false,
                    UserName = "IncludeTest 1",
                    UserType = UserType.GovermentEmployee
                },
                new User
                {
                    Id = 2,
                    Blocked = false,
                    UserName = "IncludeTest 2",
                    UserType = UserType.GovermentEmployee
                }
            };

            builder.ToTable("Users");
            builder.HasMany(u => u.SensitiveSecrets).WithOne(s => s.User).HasForeignKey(u => u.UserId);
            builder.HasMany(u => u.StateSecrets).WithOne(s => s.User).HasForeignKey(u => u.UserId);
            builder.HasMany(u => u.TopSecrets).WithOne(s => s.User).HasForeignKey(u => u.UserId);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretsSecurityAssignment.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSecurityAssignment.Data.EnitityTypeConfiguration
{
    public class SensitiveSecretConfiguration : IEntityTypeConfiguration<SensitiveSecret>
    {
        public void Configure(EntityTypeBuilder<SensitiveSecret> builder)
        {
            builder.ToTable("SensitiveSecrets");

            var data = new List<SensitiveSecret>
            {
                new SensitiveSecret
                {
                    Content = "SensitiveSecret 1",
                    Id = 1,
                    Name = "Roddel 1",
                    IncludeUserName = true
                },
                new SensitiveSecret
                {
                    Content = "SensitiveSecret 2",
                    Id = 2,
                    Name = "Roddel 2",
                    IncludeUserName = false
                }
            };

            builder.HasData(data);
        }
    }
}

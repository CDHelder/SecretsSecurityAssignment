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
    public class TopSecretConfiguration : IEntityTypeConfiguration<TopSecret>
    {
        public void Configure(EntityTypeBuilder<TopSecret> builder)
        {
            builder.ToTable("TopSecrets");

            var data = new List<TopSecret>
            {
                new TopSecret
                {
                    Content = "TopSecret 1",
                    Id = 1,
                    Name = "Topgeheim 1"
                },
                new TopSecret
                {
                    Content = "TopSecret 2",
                    Id = 2,
                    Name = "Topgeheim 2"
                }
            };
            builder.HasData(data);
        }
    }
}

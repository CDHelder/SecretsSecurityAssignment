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
    public class StateSecretConfiguration : IEntityTypeConfiguration<StateSecret>
    {
        public void Configure(EntityTypeBuilder<StateSecret> builder)
        {
            builder.ToTable("StateSecrets");

            var data = new List<StateSecret>
            {
                new StateSecret
                {
                    Content = "StateSecret 1",
                    Id = 1,
                    Name = "Staatsgeheim 1"
                },
                new StateSecret
                {
                    Content = "StateSecret 2",
                    Id = 2,
                    Name = "Staatsgeheim 2"
                }
            };
            builder.HasData(data);
        }
    }
}

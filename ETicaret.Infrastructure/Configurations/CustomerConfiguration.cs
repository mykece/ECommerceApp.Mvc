using ETicaret.Domain.Core.BaseEntityConfigurations;
using ETicaret.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure.Configurations
{
    public class CustomerConfiguration : BaseUserConfiguration<Customer>
    {
        public override void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(x => x.DateOfBirth).IsRequired().HasColumnType("date");
            builder.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(11);
            builder.Property(x => x.Gender).IsRequired();
            base.Configure(builder);
        }
    }
}

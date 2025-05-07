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
    public class CustomerAddressConfiguration : BaseEntityConfiguration<CustomerAddress>
    {
        public override void Configure(EntityTypeBuilder<CustomerAddress> builder)
        {
            builder.Property(x => x.AddressName).IsRequired();
            builder.Property(x => x.Address).IsRequired();

            base.Configure(builder);
        }
    }
}

using ETicaret.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure.Configurations;

public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.Property(x => x.Quantity).IsRequired();
        builder.Property(x=>x.OrderId).IsRequired();
        builder.Property(x=>x.UnitPrice).IsRequired();
    }
}

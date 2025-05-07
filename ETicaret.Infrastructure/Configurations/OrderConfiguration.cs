using ETicaret.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(x=>x.CustomerId).IsRequired();
        builder.Property(x=>x.ShipAdress).IsRequired().HasMaxLength(4000);
        builder.Property(x=>x.ShipVia).IsRequired(false);
        builder.Property(x => x.ShipCity).IsRequired();
        builder.Property(x=>x.ShipCountry).IsRequired();
        builder.Property(x=>x.ShipPostalCode).IsRequired();

    }
}

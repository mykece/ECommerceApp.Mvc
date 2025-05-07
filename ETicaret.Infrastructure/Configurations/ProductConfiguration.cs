using ETicaret.Domain.Core.BaseEntityConfigurations;
using ETicaret.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ETicaret.Infrastructure.Configurations
{
    public class ProductConfiguration : AudiTableEntityConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(b => b.Name).IsRequired().HasMaxLength(180);
            builder.Property(b => b.Description).IsRequired(false).HasMaxLength(500);
            builder.Property(b => b.UnitPrice).IsRequired();
            builder.Property(b=> b.Gender).IsRequired();
            builder.Property(x => x.Image).IsRequired(false);
            base.Configure(builder);
        }
    }
}

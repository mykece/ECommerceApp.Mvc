using ETicaret.Domain.Core.BaseEntityConfigurations;
using ETicaret.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ETicaret.Infrastructure.Configurations;

public class SizeConfiguration : AudiTableEntityConfiguration<Size>
{
    public override void Configure(EntityTypeBuilder<Size> builder)
    {
        builder.Property(x => x.SizeName).IsRequired();
        builder.Property(x => x.SizeTypeName).IsRequired(false);

        base.Configure(builder);
    }
}

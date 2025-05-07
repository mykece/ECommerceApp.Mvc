using ETicaret.Domain.Core.BaseEntityConfigurations;
using ETicaret.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ETicaret.Infrastructure.Configurations
{
    public class CampaignConfiguration : AudiTableEntityConfiguration<Campaign>
    {
        public override void Configure(EntityTypeBuilder<Campaign> builder)
        {
            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.StartDate)
                  .IsRequired();

            builder.Property(x => x.EndDate)
                   .IsRequired();

            builder.Property(x => x.IsActive)
                   .IsRequired();
            builder.Property(x => x.CampaignStatus)
                  .IsRequired();
            builder.Property(x => x.DiscountPercentage)
                  .IsRequired();

            base.Configure(builder);
        }
    }
}

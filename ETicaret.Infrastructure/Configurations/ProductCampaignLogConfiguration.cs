using ETicaret.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure.Configurations;

public class ProductCampaignLogConfiguration : IEntityTypeConfiguration<ProductCampaignLog>
{
    public void Configure(EntityTypeBuilder<ProductCampaignLog> builder)
    {
        //builder.Property(x => x.IsActive).IsRequired();
        builder.Property(x => x.ProductId).IsRequired();
        builder.Property(x => x.CampaignId).IsRequired();
        builder.Property(x => x.CampaignName).IsRequired();
        builder.Property(x => x.StartDate).IsRequired();
        builder.Property(x => x.EndDate).IsRequired();
    }
}

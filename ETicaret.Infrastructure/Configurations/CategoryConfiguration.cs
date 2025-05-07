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
    public class CategoryConfiguration : AudiTableEntityConfiguration<Category>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Image).IsRequired(false);
            base.Configure(builder);
        }
    }
}

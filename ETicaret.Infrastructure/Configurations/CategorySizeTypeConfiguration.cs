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
    public class CategorySizeTypeConfiguration : BaseEntityConfiguration<CategorySizeType>
    {
        public override void Configure(EntityTypeBuilder<CategorySizeType> builder)
        {
            base.Configure(builder);
        }
    }
}

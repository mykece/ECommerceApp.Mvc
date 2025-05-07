using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Domain.Core.BaseEntityConfigurations
{
    public class BaseUserConfiguration<TEntity> : AudiTableEntityConfiguration<TEntity> where TEntity : BaseUser
    {
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(x=>x.FirstName).IsRequired().HasMaxLength(128);
            builder.Property(x=>x.LastName).IsRequired().HasMaxLength(128);
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x=>x.Image).IsRequired(false);
            base.Configure(builder);
        }
    }
    
}

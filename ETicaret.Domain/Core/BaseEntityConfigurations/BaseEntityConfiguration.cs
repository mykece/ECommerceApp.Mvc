namespace ETicaret.Domain.Core.BaseEntityConfigurations
{
    public class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.CreatedBy).IsRequired();
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.UpdatedBy).IsRequired(false);
            builder.Property(x => x.UpdatedDate).IsRequired(false);
        }
    }
}

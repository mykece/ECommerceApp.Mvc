namespace ETicaret.Infrastructure.DataAccess.Interfaces
{
    public interface IAsyncDeletableRepository<TEntity> : IAsyncRepository where TEntity : BaseEntity
    {
        Task DeleteAsync(TEntity entity);
    }
}

namespace ETicaret.Infrastructure.DataAccess.Interfaces
{
    public interface IAsyncUpdatableRepository<TEntity> : IAsyncRepository where TEntity : BaseEntity
    {
        Task<TEntity> UpdateAsync(TEntity entity);
    }
}

using ETicaret.Domain.Entities;
using System.Linq.Expressions;

namespace ETicaret.Infrastructure.Repositories.ProductRepositories
{
    public interface IProductRepository : IAsyncRepository, IAsyncInsertableRepository<Product>, IAsyncFindableRepository<Product>,
        IAsyncQueryableRepository<Product>, IAsyncDeletableRepository<Product>, IAsyncUpdatableRepository<Product>, IAsyncTransactionRepository
    {
        Task<List<Product>> GetProductsByCategoryIdAsync(Guid categoryId);
        Task<List<Product>> GetLatestProductsAsync(int count);
        Task<int> CountAsync(Expression<Func<Product, bool>> predicate);
    }
}

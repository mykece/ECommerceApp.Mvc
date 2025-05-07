using ETicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure.Repositories.CategoryRepositories
{
    public interface ICategoryRepository : IAsyncRepository, IAsyncInsertableRepository<Category>, IAsyncFindableRepository<Category>,
          IAsyncQueryableRepository<Category>, IAsyncDeletableRepository<Category>, IAsyncUpdatableRepository<Category>, IAsyncTransactionRepository
    {
        Task<List<Category>> GetCategoriesByIdsAsync(List<Guid> categoryIds);
    }
}

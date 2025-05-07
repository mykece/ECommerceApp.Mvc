using ETicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure.Repositories.CategorySizeTypeProductRepositories;

public interface ICategorySizeTypeProductRepository : IAsyncRepository, IAsyncInsertableRepository<CategorySizeTypeProduct>, IAsyncFindableRepository<CategorySizeTypeProduct>,
        IAsyncQueryableRepository<CategorySizeTypeProduct>, IAsyncDeletableRepository<CategorySizeTypeProduct>, IAsyncUpdatableRepository<CategorySizeTypeProduct>
{
    Task<List<CategorySizeTypeProduct>> GetByProductIdAsync(Guid productId);
}

using ETicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure.Repositories.CategorySizeTypeRepositories
{
    public interface ICategorySizeTypeRepository : IAsyncRepository, IAsyncInsertableRepository<CategorySizeType>, IAsyncFindableRepository<CategorySizeType>,
        IAsyncQueryableRepository<CategorySizeType>, IAsyncDeletableRepository<CategorySizeType>, IAsyncUpdatableRepository<CategorySizeType>
    {
    }
}

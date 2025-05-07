using ETicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure.Repositories.SizeTypeRepositories
{
    public interface ISizeTypeRepository : IAsyncRepository, IAsyncInsertableRepository<SizeType>, IAsyncFindableRepository<SizeType>,
        IAsyncQueryableRepository<SizeType>, IAsyncDeletableRepository<SizeType>, IAsyncUpdatableRepository<SizeType>, IAsyncTransactionRepository
    {
    }
}

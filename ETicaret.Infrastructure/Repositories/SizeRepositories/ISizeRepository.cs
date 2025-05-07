using ETicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure.Repositories.SizeRepositories;

public interface ISizeRepository : IAsyncRepository, IAsyncInsertableRepository<Size>, IAsyncFindableRepository<Size>,
        IAsyncQueryableRepository<Size>, IAsyncDeletableRepository<Size>, IAsyncUpdatableRepository<Size>
{
}

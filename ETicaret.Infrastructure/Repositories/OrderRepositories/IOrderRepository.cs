using ETicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure.Repositories.OrderRepositories;

public interface IOrderRepository : IAsyncRepository, IAsyncInsertableRepository<Order>, IAsyncFindableRepository<Order>,
          IAsyncQueryableRepository<Order>, IAsyncDeletableRepository<Order>, IAsyncUpdatableRepository<Order>, IAsyncTransactionRepository
{
}

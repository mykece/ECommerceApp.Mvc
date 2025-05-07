using ETicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure.Repositories.OrderDetailRepositories;

public interface IOrderDetailRepository : IAsyncRepository, IAsyncInsertableRepository<OrderDetail>, IAsyncFindableRepository<OrderDetail>,
          IAsyncQueryableRepository<OrderDetail>, IAsyncDeletableRepository<OrderDetail>, IAsyncUpdatableRepository<OrderDetail>, IAsyncTransactionRepository
{
}

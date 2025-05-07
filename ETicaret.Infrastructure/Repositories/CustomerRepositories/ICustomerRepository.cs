using ETicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure.Repositories.CustomerRepositories
{
    public interface ICustomerRepository : IAsyncRepository, IAsyncInsertableRepository<Customer>, IAsyncFindableRepository<Customer>,
          IAsyncQueryableRepository<Customer>, IAsyncDeletableRepository<Customer>, IAsyncUpdatableRepository<Customer>, IAsyncTransactionRepository
    {
        Task<Customer?> GetByIdentityId(string identityId);
    }
}

using ETicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure.Repositories.CustomerAddressRepositories
{
    public interface ICustomerAddressRepository: IAsyncRepository, IAsyncInsertableRepository<CustomerAddress>, IAsyncFindableRepository<CustomerAddress>,
        IAsyncQueryableRepository<CustomerAddress>, IAsyncDeletableRepository<CustomerAddress>, IAsyncUpdatableRepository<CustomerAddress>
    {
    }
}

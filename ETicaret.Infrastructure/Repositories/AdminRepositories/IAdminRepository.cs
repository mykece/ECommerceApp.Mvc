using ETicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure.Repositories.AdminRepositories
{
    public interface IAdminRepository : IAsyncRepository, IAsyncInsertableRepository<Admin>, IAsyncFindableRepository<Admin>, IAsyncQueryableRepository<Admin>, IAsyncUpdatableRepository<Admin>, IAsyncDeletableRepository<Admin>, IAsyncTransactionRepository
    {
        Task<Admin?> GetByIdentityId(string identityId);
    }
}

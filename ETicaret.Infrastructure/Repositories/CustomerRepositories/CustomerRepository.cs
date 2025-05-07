using ETicaret.Domain.Entities;
using ETicaret.Infrastructure.AppContext;
using ETicaret.Infrastructure.DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure.Repositories.CustomerRepositories
{
    public class CustomerRepository : EFBaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(AppDbContext context) : base(context)
        {
        }

        public Task<Customer?> GetByIdentityId(string identityId)
        {
            return _table.FirstOrDefaultAsync(x => x.IdentityId == identityId);
        }
    }
}

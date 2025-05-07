using ETicaret.Domain.Entities;
using ETicaret.Infrastructure.AppContext;
using ETicaret.Infrastructure.DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure.Repositories.CategorySizeTypeRepositories
{
    public class CategorySizeTypeRepository : EFBaseRepository<CategorySizeType>, ICategorySizeTypeRepository
    {
        public CategorySizeTypeRepository(AppDbContext context) : base(context)
        {
        }
    }
}

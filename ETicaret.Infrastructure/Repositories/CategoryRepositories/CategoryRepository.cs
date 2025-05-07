using ETicaret.Domain.Entities;
using ETicaret.Infrastructure.AppContext;
using ETicaret.Infrastructure.DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure.Repositories.CategoryRepositories
{
    public class CategoryRepository : EFBaseRepository<Category>, ICategoryRepository
    {
        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<Category>> GetCategoriesByIdsAsync(List<Guid> categoryIds)
        {
            return await _context.Categories
                .Where(c => categoryIds.Contains(c.Id))
                .ToListAsync();
        }
    }
}

using ETicaret.Domain.Entities;
using ETicaret.Infrastructure.AppContext;
using ETicaret.Infrastructure.DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure.Repositories.CategorySizeTypeProductRepositories;

public class CategorySizeTypeProductRepository : EFBaseRepository<CategorySizeTypeProduct> , ICategorySizeTypeProductRepository
{
    private readonly AppDbContext _context;
    public CategorySizeTypeProductRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<CategorySizeTypeProduct>> GetByProductIdAsync(Guid productId)
    {
        return await _context.CategorySizeTypeProducts
                .Where(cstp => cstp.ProductId == productId)
                .ToListAsync();
    }
}

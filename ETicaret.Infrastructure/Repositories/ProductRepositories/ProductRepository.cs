using ETicaret.Domain.Entities;
using ETicaret.Domain.Enums;
using ETicaret.Infrastructure.AppContext;
using ETicaret.Infrastructure.DataAccess.EntityFramework;
using System.Linq.Expressions;

namespace ETicaret.Infrastructure.Repositories.ProductRepositories
{
    public class ProductRepository : EFBaseRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetLatestProductsAsync(int count)
        {
            return await _context.Products
                      .Where(p => p.Status != Status.Deleted)
.                     OrderByDescending(p => p.CreatedDate)
                       .Take(count)
                       .ToListAsync();
        }

        public async Task<List<Product>> GetProductsByCategoryIdAsync(Guid categoryId)
        {
            return await _context.CategorySizeTypeProducts
                .Include(cstp => cstp.Product)
                .Where(cstp => cstp.CategorySizeType.CategoryId == categoryId)
                .Select(cstp => cstp.Product)
                .Distinct()
                .ToListAsync();
        }

        public async Task<int> CountAsync(Expression<Func<Product, bool>> predicate)
        {
            return await _context.Products.CountAsync(predicate);
        }

    }
}

using ETicaret.Domain.Entities;
using ETicaret.Infrastructure.AppContext;
using ETicaret.Infrastructure.DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure.Repositories.ProductCampaignLogRepositories;

public class ProductCampaignLogRepository : EFBaseRepository<ProductCampaignLog>, IProductCampaignLogRepository
{
    public ProductCampaignLogRepository(AppDbContext context)  : base(context)
    {
        
    }
}

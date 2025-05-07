using ETicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure.Repositories.ProductCampaignLogRepositories
{
    public interface IProductCampaignLogRepository : IAsyncRepository, IAsyncInsertableRepository<ProductCampaignLog>, IAsyncFindableRepository<ProductCampaignLog>,
          IAsyncQueryableRepository<ProductCampaignLog>, IAsyncDeletableRepository<ProductCampaignLog>, IAsyncUpdatableRepository<ProductCampaignLog>, IAsyncTransactionRepository
    {
    }
}

using ETicaret.Domain.Entities;

namespace ETicaret.Infrastructure.Repositories.CampaignRepositories
{
    public interface ICampaignRepository : IAsyncRepository, IAsyncInsertableRepository<Campaign>, IAsyncFindableRepository<Campaign>,
          IAsyncQueryableRepository<Campaign>, IAsyncDeletableRepository<Campaign>, IAsyncUpdatableRepository<Campaign>, IAsyncTransactionRepository
    {
    }
}

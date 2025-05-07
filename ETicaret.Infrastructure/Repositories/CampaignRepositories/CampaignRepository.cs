using ETicaret.Domain.Entities;
using ETicaret.Infrastructure.AppContext;
using ETicaret.Infrastructure.DataAccess.EntityFramework;

namespace ETicaret.Infrastructure.Repositories.CampaignRepositories
{
    public class CampaignRepository : EFBaseRepository<Campaign>, ICampaignRepository
    {
        public CampaignRepository(AppDbContext context) : base(context)
        {
        }
    }
}

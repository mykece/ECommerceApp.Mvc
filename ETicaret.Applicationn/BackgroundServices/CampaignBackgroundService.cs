using ETicaret.Applicationn.Services.CampaignServices;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace ETicaret.Applicationn.BackgroundServices
{
    //Hangfire ile planlamak için background metodu
    public class CampaignBackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public CampaignBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        //Belirli aralıklarla kampanyaların süresini kontrol eder ve süresi dolan kampanyaları pasif hale getirir.
        [AutomaticRetry(Attempts = 3)]                                 // Hata durumunda otomatik olarak yeniden deneme  yapmasını sağlar (maksimum 3 kez yeniden deneme)
        public async Task DeactivateExpiredCampaigns()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                //throw new NotImplementedException();
                var campaignService = scope.ServiceProvider.GetRequiredService<ICampaignService>();
                await campaignService.DeactivateExpiredCampaignsAsync();
            }
        }
    }
}

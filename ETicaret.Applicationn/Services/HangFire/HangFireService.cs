using ETicaret.Applicationn.BackgroundServices;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.Services.HangFire;

public class HangFireService
{
    private readonly IRecurringJobManager _recurringJobManager;
    private readonly CampaignBackgroundService _backgroundService;

    public HangFireService(IRecurringJobManager recurringJobManager, CampaignBackgroundService backgroundService)
    {
        _recurringJobManager = recurringJobManager;
        _backgroundService = backgroundService;
    }
    public void ConfigureHangfireJobs()
    {
        _recurringJobManager.AddOrUpdate(
            "deactivate-expired-campaigns",
            () => _backgroundService.DeactivateExpiredCampaigns(),
             //Cron.Minutely
             Cron.Daily(23, 59)
        );
    }

}

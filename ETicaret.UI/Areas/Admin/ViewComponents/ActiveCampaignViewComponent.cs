using ETicaret.Applicationn.Services.CampaignServices;
using ETicaret.UI.Areas.Admin.Models.CampaignVMs;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.UI.Areas.Admin.ViewComponents
{
    public class ActiveCampaignViewComponent : ViewComponent
    {
        private readonly ICampaignService _campaignService;

        public ActiveCampaignViewComponent(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var activeCampaigns = await _campaignService.GetActiveCampaignsAsync();

            if (!activeCampaigns.IsSuccess)
            {
                return View("Error", activeCampaigns.Messages); 
            }

            var activeCampaignVMs = activeCampaigns.Data.Adapt<List<CampaignListVM>>();

            return View(activeCampaignVMs);
        }
    }
}

using ETicaret.Applicationn.Services.CampaignServices;
using ETicaret.UI.Areas.Admin.Models.CampaignVMs;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.UI.ViewComponents
{
    public class SliderViewComponent : ViewComponent
    {
        private readonly ICampaignService _campaignService;

        public SliderViewComponent(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var activeCampaigns = (await _campaignService.GetAllAsync()).Data.FindAll(x => x.IsActive == true);
            var activeCampaignListVMs = activeCampaigns.Adapt<List<CampaignListVM>>();


            return View("Default",activeCampaignListVMs);
        }
    }
}

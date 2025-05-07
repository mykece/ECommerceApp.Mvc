using ETicaret.Applicationn.DTOs.CampaignDTOs;
using ETicaret.Domain.Utilities.Interfaces;

namespace ETicaret.Applicationn.Services.CampaignServices
{
    public interface ICampaignService
    {
        Task<IDataResult<CampaignDTO>> CreateAsync(CampaignCreateDTO campaignCreateDTO);
        Task<IDataResult<List<CampaignListDTO>>> GetAllAsync();
        Task<IResult> DeleteAsync(Guid id);
        Task<IDataResult<CampaignDTO>> GetByIdAsync(Guid id);
        Task<IDataResult<CampaignDTO>> UpdateAsync(CampaignUpdateDTO campaignUpdateDTO);
        Task DeactivateExpiredCampaignsAsync();
        Task<IDataResult<List<CampaignListDTO>>> GetActiveCampaignsAsync();  // Aktif kampanyalar dashboard için 
    }
}

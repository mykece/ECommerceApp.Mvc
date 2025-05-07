using ETicaret.Applicationn.DTOs.OrderDTO;
using ETicaret.Applicationn.DTOs.ProductCampaignLogDTOs;
using ETicaret.Domain.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.Services.ProductCampaignLogServices;

public interface IProductCampaignLogService
{
    Task<IDataResult<ProductCampaignLogDTO>> CreateAsync(ProductCampaignLogCreateDTO productCampaignCreateDTO);
    Task<IDataResult<List<ProductCampaignLogListDTO>>> GetAllAsync();
    Task<IResult> DeleteAsync(Guid id);
    Task<IDataResult<ProductCampaignLogDTO>> GetByIdAsync(Guid id);
    Task<IDataResult<ProductCampaignLogDTO>> UpdateAsync(ProductCampaignLogUpdateDTO productCampaignUpdateDTO);
    Task<IDataResult<List<ProductCampaignLogListDTO>>> GetAllByCampaignIdAsync(Guid id);
    Task<IDataResult<List<ProductCampaignLogListDTO>>> GetAllByProductIdAsync(Guid id);
}

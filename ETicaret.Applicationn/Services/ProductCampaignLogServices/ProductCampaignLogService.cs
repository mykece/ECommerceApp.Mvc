using ETicaret.Applicationn.DTOs.ProductCampaignLogDTOs;
using ETicaret.Domain.Entities;
using ETicaret.Domain.Utilities.Concretes;
using ETicaret.Domain.Utilities.Interfaces;
using ETicaret.Infrastructure.Repositories.ProductCampaignLogRepositories;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.Services.ProductCampaignLogServices;

public class ProductCampaignLogService : IProductCampaignLogService
{
    private readonly IProductCampaignLogRepository _productCampaignLogRepository;

    public ProductCampaignLogService(IProductCampaignLogRepository productCampaignLogRepository)
    {
        _productCampaignLogRepository = productCampaignLogRepository;
    }

    public async Task<IDataResult<ProductCampaignLogDTO>> CreateAsync(ProductCampaignLogCreateDTO productCampaignCreateDTO)
    {
        var productCampaignLog = await _productCampaignLogRepository.AddAsync(productCampaignCreateDTO.Adapt<ProductCampaignLog>());
        if(productCampaignLog is null)
        {
            return new ErrorDataResult<ProductCampaignLogDTO>(productCampaignLog.Adapt<ProductCampaignLogDTO>(),"Loglama başarısız.");
            
        }
        await _productCampaignLogRepository.SaveChangesAsync();
        return new SuccessDataResult<ProductCampaignLogDTO>(productCampaignLog.Adapt<ProductCampaignLogDTO>(), "Loglama başarılı.");

    }

    public async Task<IResult> DeleteAsync(Guid id)
    {
        var deletedProductCampaignLog = await _productCampaignLogRepository.GetByIdAsync(id);
         
        if(deletedProductCampaignLog is null)
        {
            return new ErrorResult("Silinecek log bulunamadı.");
            
        }
        await _productCampaignLogRepository.DeleteAsync(deletedProductCampaignLog);
        await _productCampaignLogRepository.SaveChangesAsync();
        return new SuccessResult("Log başarıyla silindi.");
    }

    public async Task<IDataResult<List<ProductCampaignLogListDTO>>> GetAllAsync()
    {
        var productCampaignLogs = await _productCampaignLogRepository.GetAllAsync();
        if (productCampaignLogs is null)
        {
            return new ErrorDataResult<List<ProductCampaignLogListDTO>>(new List<ProductCampaignLogListDTO>(), "Listelenecek kampanya bulunamadı.");
        }
        else return new SuccessDataResult<List<ProductCampaignLogListDTO>>(productCampaignLogs.Adapt<List<ProductCampaignLogListDTO>>(), "Loglar başarıyla getirildi.");
    }

    public async Task<IDataResult<List<ProductCampaignLogListDTO>>> GetAllByCampaignIdAsync(Guid id)
    {
        var productCampaignLogs = await _productCampaignLogRepository.GetAllAsync(x=>x.CampaignId == id);
        if(productCampaignLogs is null)
        {
            return new ErrorDataResult<List<ProductCampaignLogListDTO>>(new List<ProductCampaignLogListDTO>(), "Kampanyaya ait product logları bulunamadı."); ;
        }
        return new SuccessDataResult<List<ProductCampaignLogListDTO>>(productCampaignLogs.Adapt<List<ProductCampaignLogListDTO>>(), "Kampanyaya ait loglar başarıyla listelendi.");
    }

    public async Task<IDataResult<List<ProductCampaignLogListDTO>>> GetAllByProductIdAsync(Guid id)
    {
        var productCampaignLogs = await _productCampaignLogRepository.GetAllAsync(x => x.ProductId == id, false);
        if (productCampaignLogs is null)
        {
            return new ErrorDataResult<List<ProductCampaignLogListDTO>>(new List<ProductCampaignLogListDTO>(), "Ürüne ait kampanya logları bulunamadı."); ;
        }
        return new SuccessDataResult<List<ProductCampaignLogListDTO>>(productCampaignLogs.Adapt<List<ProductCampaignLogListDTO>>(), "Ürüne ait loglar başarıyla listelendi.");
    }

    public async Task<IDataResult<ProductCampaignLogDTO>> GetByIdAsync(Guid id)
    {
        var productCampaignLog = _productCampaignLogRepository.GetByIdAsync(id);
        if(productCampaignLog is null)
        {
            return new ErrorDataResult<ProductCampaignLogDTO>("Log bulunamadı.");
        }
        return new SuccessDataResult<ProductCampaignLogDTO>(productCampaignLog.Adapt<ProductCampaignLogDTO>(),"Log getirme başarılı.");
    }

    public async Task<IDataResult<ProductCampaignLogDTO>> UpdateAsync(ProductCampaignLogUpdateDTO productCampaignUpdateDTO)
    {
        var updatedProductCampaignLog = await _productCampaignLogRepository.UpdateAsync(productCampaignUpdateDTO.Adapt<ProductCampaignLog>());
        if (updatedProductCampaignLog is null)
        {
            return new ErrorDataResult<ProductCampaignLogDTO>("Log güncelleme başarısız.");
        }
        await _productCampaignLogRepository.SaveChangesAsync();
        return new SuccessDataResult<ProductCampaignLogDTO>(updatedProductCampaignLog.Adapt<ProductCampaignLogDTO>(),"Log güncelleme başarılı");
    }
}

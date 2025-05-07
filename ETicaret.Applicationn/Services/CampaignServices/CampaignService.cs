using ETicaret.Applicationn.DTOs.CampaignDTOs;
using ETicaret.Applicationn.DTOs.ProductCampaignLogDTOs;
using ETicaret.Domain.Entities;
using ETicaret.Domain.Enums;
using ETicaret.Domain.Utilities.Concretes;
using ETicaret.Domain.Utilities.Interfaces;
using ETicaret.Infrastructure.Repositories.CampaignRepositories;
using ETicaret.Infrastructure.Repositories.CategoryRepositories;
using ETicaret.Infrastructure.Repositories.ProductCampaignLogRepositories;
using ETicaret.Infrastructure.Repositories.ProductRepositories;
using Mapster;

namespace ETicaret.Applicationn.Services.CampaignServices;

public class CampaignService : ICampaignService
{
    private readonly ICampaignRepository _campaignRepository;
    private readonly IProductRepository _productRepository;
    private readonly IProductCampaignLogRepository _productCampaignLogRepository;
    private readonly ICategoryRepository _categoryRepository;
    public CampaignService(ICampaignRepository campaignRepository, IProductRepository productRepository, IProductCampaignLogRepository productCampaignLogRepository, ICategoryRepository categoryRepository)
    {
        _campaignRepository = campaignRepository;
        _productRepository = productRepository;
        _productCampaignLogRepository = productCampaignLogRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IDataResult<CampaignDTO>> CreateAsync(CampaignCreateDTO campaignCreateDTO)
    {
        if (await _campaignRepository.AnyAsync(x => x.Name.ToLower() == campaignCreateDTO.Name.ToLower()))
        {
            return new ErrorDataResult<CampaignDTO>("Current Campaign Registered in the System!");
        }
        var newCampaign = campaignCreateDTO.Adapt<Campaign>();

        var now = DateTime.Now;
        if (newCampaign.StartDate > now)
        {
            newCampaign.CampaignStatus = CampaignStatus.CampaignPreparing;
            newCampaign.IsActive = false;
        }
        else if (newCampaign.StartDate <= now && newCampaign.EndDate > now)
        {
            newCampaign.CampaignStatus = CampaignStatus.CampaignActive;
            newCampaign.IsActive = true;
        }
        else if (newCampaign.EndDate <= now)
        {
            newCampaign.CampaignStatus = CampaignStatus.CampaignPassive;
            newCampaign.IsActive = false;
        }

        newCampaign.Products = new List<Product>();

        if (campaignCreateDTO.ProductIds != null && campaignCreateDTO.ProductIds.Any())
        {
            foreach (var productId in campaignCreateDTO.ProductIds)
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product != null)
                {
                    if (newCampaign.CampaignStatus == CampaignStatus.CampaignActive)
                    {
                        product.OriginalPrice = product.UnitPrice;
                        product.UnitPrice -= product.UnitPrice * (decimal)(campaignCreateDTO.DiscountPercentage / 100);
                    }
                    newCampaign.Products.Add(product);
                }
            }
        }
        else if (campaignCreateDTO.CategoryIds != null && campaignCreateDTO.CategoryIds.Any())
        {
            foreach (var categoryId in campaignCreateDTO.CategoryIds)
            {
                var categoryProducts = await _productRepository.GetProductsByCategoryIdAsync(categoryId);
                foreach (var product in categoryProducts)
                {
                    if (newCampaign.CampaignStatus == CampaignStatus.CampaignActive)
                    {
                        product.OriginalPrice = product.UnitPrice;
                        product.UnitPrice -= product.UnitPrice * (decimal)(campaignCreateDTO.DiscountPercentage / 100);
                    }
                    newCampaign.Products.Add(product);
                }
            }
        }

        var campaign = await _campaignRepository.AddAsync(newCampaign);

        //Product'a ait Campaign'lerin loglama Create işleminin yapıldığı yer.
        if (campaign != null)
        {
            foreach (var item in campaign.Products)
            {
                var newProductCampaignLog = new ProductCampaignLog()
                {
                    CampaignId = campaign.Id,
                    ProductId = item.Id,
                    CampaignName = campaign.Name,
                    DiscountPercentage = campaign.DiscountPercentage,
                    EndDate = campaign.EndDate,
                    StartDate = campaign.StartDate,
                };
                await _productCampaignLogRepository.AddAsync(newProductCampaignLog);
                await _productCampaignLogRepository.SaveChangesAsync();
            }

        };

        await _campaignRepository.SaveChangesAsync();

        var campaignDto = newCampaign.Adapt<CampaignDTO>();
        return new SuccessDataResult<CampaignDTO>(campaignDto, "Campaign added successfully!");
    }
    // Kategori isimlerini almak için yardımcı bir metot 
    private async Task<List<string>> GetCategoryNamesByIdsAsync(List<Guid> categoryIds)
    {
        // Burada kategori isimlerini almak için ilgili repository'den veya servisten verileri çek
        var categories = await _categoryRepository.GetCategoriesByIdsAsync(categoryIds);
        return categories.Select(c => c.Name).ToList();
    }
    public async Task<IDataResult<List<CampaignListDTO>>> GetAllAsync()
    {
        var campaigns = await _campaignRepository.GetAllAsync();
        var campaignListDTOs = campaigns.Adapt<List<CampaignListDTO>>();
        foreach (var campaign in campaigns)
        {
            var campaignDTO = campaignListDTOs.FirstOrDefault(c => c.Id == campaign.Id);

            if (campaignDTO != null)
            {// Kampanyanın ürün isimlerini ve ID'lerini al
                var productNames = campaign.Products.Select(p => p.Name).ToList();
                var productIds = campaign.Products.Select(p => p.Id).ToList();
                campaignDTO.ProductNames = productNames;
                campaignDTO.ProductIds = productIds;

                // Kampanyanın ürünleri aracılığıyla kategorilere erişim
                var categoryIds = campaign.Products
                                          .SelectMany(p => p.CategorySizeTypeProducts.Select(cstp => cstp.CategorySizeType.CategoryId))
                                          .Distinct()
                                          .ToList();

                // Kategori isimlerini al (burada, GetCategoryNamesByIdsAsync metodunu kullanarak isimleri alıyoruz)
                var categoryNames = await GetCategoryNamesByIdsAsync(categoryIds);

                campaignDTO.CategoryIds = categoryIds;
                campaignDTO.CategoryNames = categoryNames;
            }
        }
        if (!campaigns.Any())
        {
            return new ErrorDataResult<List<CampaignListDTO>>(campaignListDTOs, "Campaigns could not be found!");
        }
        return new SuccessDataResult<List<CampaignListDTO>>(campaignListDTOs, "Campaigns listed successfully!");
    }

    public async Task<IResult> DeleteAsync(Guid id)
    {
        var campaign = await _campaignRepository.GetByIdAsync(id);
        if (campaign == null)
        {
            return new ErrorResult("Campaign not found");
        }
        // Eğer kampanya aktifse, kullanıcıya kampanyayı pasif hale getirmesi gerektiğini söyle
        if (campaign.IsActive)
        {
            return new ErrorResult("You must deactivate the campaign before deleting it.");
        }


        // Kampanya silindiğinde ürünlerin fiyatlarını orijinal haline dönerrr
        foreach (var product in campaign.Products)
        {
            if (product.OriginalPrice != null)
            {
                product.UnitPrice = product.OriginalPrice.Value;
                product.OriginalPrice = null;
            }
        }
        await _campaignRepository.DeleteAsync(campaign);
        await _campaignRepository.SaveChangesAsync();
        return new SuccessResult("Campaign deleted successfully!");

    }

    public async Task<IDataResult<CampaignDTO>> GetByIdAsync(Guid id)
    {
        var campaign = await _campaignRepository.GetByIdAsync(id);
        if (campaign is null)
        {
            return new ErrorDataResult<CampaignDTO>("Campaign could not be found!");
        }
        // Kampanyanın DTO'ya dönüştürülmesi
        var campaignDTO = campaign.Adapt<CampaignDTO>();
        // Kampanyanın ürün isimlerini ve ID'lerini dahil etme
        campaignDTO.ProductNames = campaign.Products.Select(p => p.Name).ToList();
        campaignDTO.ProductIds = campaign.Products.Select(p => p.Id).ToList();

        // Kampanyanın kategorilerinin ID ve isimlerini almak için
        var categoryIds = await _categoryRepository.GetCategoriesByIdsAsync(
            campaign.Products
                .SelectMany(p => p.CategorySizeTypeProducts)
                .Select(cstp => cstp.CategorySizeType.CategoryId)
                .Distinct()
                .ToList()
        );

        campaignDTO.CategoryIds = categoryIds.Select(c => c.Id).ToList();
        campaignDTO.CategoryNames = categoryIds.Select(c => c.Name).ToList();
        return new SuccessDataResult<CampaignDTO>(campaignDTO, "Campaign found successfully!");
    }

    // Kampanya güncelleme metodu burada da aynı şekilde güncellenmelidir
    public async Task<IDataResult<CampaignDTO>> UpdateAsync(CampaignUpdateDTO campaignUpdateDTO)
    {
        var updatingCampaign = await _campaignRepository.GetByIdAsync(campaignUpdateDTO.Id);
        if (updatingCampaign is null)
        {
            return new ErrorDataResult<CampaignDTO>("Campaign could not be found!");
        }
        // Varsayılan olarak kampanyayı aktif olarak ayarla
        bool isActive = campaignUpdateDTO.IsActive;
        // Eğer kampanya aktifse ve IsActive false olarak güncellenmişse, kampanya durumunu pasif yap
        if (!isActive && updatingCampaign.IsActive)
        {
            updatingCampaign.CampaignStatus = CampaignStatus.CampaignPassive;
            updatingCampaign.EndDate = DateTime.Now; // Kampanya bitiş tarihini güncel tarih olarak ayarla
            updatingCampaign.IsActive = false;

            // Ürünlerin fiyatlarını orijinal haline döndür
            foreach (var product in updatingCampaign.Products)
            {
                if (product.OriginalPrice != null)
                {
                    product.UnitPrice = product.OriginalPrice.Value;
                    product.OriginalPrice = null;
                }
            }

            await _campaignRepository.UpdateAsync(updatingCampaign);
            await _campaignRepository.SaveChangesAsync();

            // Kampanya güncellenmiş olarak dön
            return new SuccessDataResult<CampaignDTO>(updatingCampaign.Adapt<CampaignDTO>(), "Campaign updated successfully!");
        }

        // Eğer kampanya pasif değilse veya aktif değilse, mevcut kampanyayı güncelle
        // Mevcut kampanyanın ürünlerinin fiyatlarını orijinal haline döndür
        foreach (var product in updatingCampaign.Products)
        {
            if (product.OriginalPrice != null)
            {
                product.UnitPrice = product.OriginalPrice.Value;
                product.OriginalPrice = null;
            }
        }

        //// Mevcut ürün ve kategori bilgilerini temizle
        //updatingCampaign.Products.Clear();

        // Kampanya güncelleme işlemi
        campaignUpdateDTO.Adapt(updatingCampaign);

        var now = DateTime.Now;
        if (campaignUpdateDTO.StartDate > now)
        {
            updatingCampaign.CampaignStatus = CampaignStatus.CampaignPreparing;
            updatingCampaign.IsActive = false;
        }
        else if (campaignUpdateDTO.StartDate <= now && campaignUpdateDTO.EndDate > now)
        {
            updatingCampaign.CampaignStatus = CampaignStatus.CampaignActive;
            updatingCampaign.IsActive = true;

            // Ürünler bazında güncelleme
            if (campaignUpdateDTO.ProductIds != null && campaignUpdateDTO.ProductIds.Any())
            {
                foreach (var productId in campaignUpdateDTO.ProductIds)
                {
                    var product = await _productRepository.GetByIdAsync(productId);
                    if (product != null)
                    {
                        if (product.OriginalPrice == null)
                        {
                            product.OriginalPrice = product.UnitPrice;
                        }
                        else
                        {
                            product.UnitPrice = product.OriginalPrice.Value;
                        }
                        product.UnitPrice -= product.UnitPrice * (decimal)(campaignUpdateDTO.DiscountPercentage / 100);
                        updatingCampaign.Products.Add(product);
                    }
                }
            }

            // Kategoriler bazında güncelleme
            if (campaignUpdateDTO.CategoryIds != null && campaignUpdateDTO.CategoryIds.Any())
            {
                foreach (var categoryId in campaignUpdateDTO.CategoryIds)
                {
                    var categoryProducts = await _productRepository.GetProductsByCategoryIdAsync(categoryId);
                    foreach (var product in categoryProducts)
                    {
                        if (product.OriginalPrice == null)
                        {
                            product.OriginalPrice = product.UnitPrice;
                        }
                        else
                        {
                            product.UnitPrice = product.OriginalPrice.Value;
                        }
                        product.UnitPrice -= product.UnitPrice * (decimal)(campaignUpdateDTO.DiscountPercentage / 100);
                        updatingCampaign.Products.Add(product);
                    }
                }
            }
        }
        else if (campaignUpdateDTO.EndDate <= now)
        {
            updatingCampaign.CampaignStatus = CampaignStatus.CampaignPassive;
            updatingCampaign.IsActive = false;
            updatingCampaign.EndDate = now; // Kampanya bitiş tarihini güncel tarih olarak ayarla
        }

        // Product Campaign Loglama işlemleri. Mevcutta kampanya logu varsa günceller, yoksa yenisini oluşturur.
        var productCampaignLogs = await _productCampaignLogRepository.GetAllAsync(x => x.CampaignId == campaignUpdateDTO.Id);
        if (campaignUpdateDTO.ProductIds != null) { 
            foreach (var id in campaignUpdateDTO.ProductIds)
            {
                var logs = productCampaignLogs.Where(x => x.ProductId == id).ToList();
                if (logs.Any())
                {
                    // Mevcut logları güncelle
                    foreach (var log in logs)
                    {
                        log.StartDate = campaignUpdateDTO.StartDate;
                        log.EndDate = campaignUpdateDTO.EndDate;
                        log.CampaignName = campaignUpdateDTO.Name;
                        log.DiscountPercentage = campaignUpdateDTO.DiscountPercentage;
                        await _productCampaignLogRepository.UpdateAsync(log);
                    }
                }
                else
                {
                    // Yeni log oluştur
                    var newLog = new ProductCampaignLog()
                    {
                        CampaignId = campaignUpdateDTO.Id,
                        CampaignName = campaignUpdateDTO.Name,
                        ProductId = id,
                        StartDate = campaignUpdateDTO.StartDate,
                        EndDate = campaignUpdateDTO.EndDate,
                        DiscountPercentage = campaignUpdateDTO.DiscountPercentage,
                    };
                    await _productCampaignLogRepository.AddAsync(newLog);
                }
            }

        }
        await _productCampaignLogRepository.SaveChangesAsync();
        await _campaignRepository.UpdateAsync(updatingCampaign);
        await _campaignRepository.SaveChangesAsync();

        // Kampanya güncellenmiş olarak dön
        return new SuccessDataResult<CampaignDTO>(updatingCampaign.Adapt<CampaignDTO>(), "Campaign updated successfully!");
    }






    //kampanyaların bitiş tarihlerini kontrol eden ve süresi dolan kampanyaları otomatik olarak pasif hale getiren bir işlem.Background jobs çağrıldı
    // Kampanya bitiş kontrolü ve fiyat geri döndürme metodu
    public async Task DeactivateExpiredCampaignsAsync()
    {
        var campaigns = await _campaignRepository.GetAllAsync();
        var now = DateTime.Now;
        foreach (var campaign in campaigns)
        {
            if (campaign.EndDate < now && campaign.CampaignStatus != CampaignStatus.CampaignPassive)
            {
                campaign.CampaignStatus = CampaignStatus.CampaignPassive;
                campaign.IsActive = false;

                // Ürünlerin fiyatlarını orijinal haline döndür
                foreach (var product in campaign.Products)
                {
                    if (product.OriginalPrice != null)
                    {
                        product.UnitPrice = product.OriginalPrice.Value;
                        product.OriginalPrice = null; // Orijinal fiyatı sıfırla
                    }
                }
                await _campaignRepository.UpdateAsync(campaign);
            }

            else if (campaign.StartDate > now && campaign.CampaignStatus != CampaignStatus.CampaignPreparing)
            {
                campaign.CampaignStatus = CampaignStatus.CampaignPreparing;
                campaign.IsActive = false;
                await _campaignRepository.UpdateAsync(campaign);
            }
            else if (campaign.StartDate <= now && campaign.EndDate > now && campaign.CampaignStatus != CampaignStatus.CampaignActive)
            {
                campaign.CampaignStatus = CampaignStatus.CampaignActive;
                campaign.IsActive = true;
                foreach (var product in campaign.Products)
                {
                    if (product.OriginalPrice == null)
                    {
                        product.OriginalPrice = product.UnitPrice;
                    }
                    product.UnitPrice -= product.UnitPrice * (decimal)(campaign.DiscountPercentage / 100);
                }
                await _campaignRepository.UpdateAsync(campaign);
            }
        }
        await _campaignRepository.SaveChangesAsync();
    }
    // Aktif kampanyalar dashboard için 
    public async Task<IDataResult<List<CampaignListDTO>>> GetActiveCampaignsAsync()
    {
        var activeCampaigns = await _campaignRepository.GetAllAsync(c => c.CampaignStatus == CampaignStatus.CampaignActive);

        if (!activeCampaigns.Any())
        {
            return new ErrorDataResult<List<CampaignListDTO>>("Aktif kampanya bulunamadı!");
        }

        var activeCampaignListDTOs = activeCampaigns.Adapt<List<CampaignListDTO>>();

        // Ekstra çekileeck bilgiler
        foreach (var campaign in activeCampaigns)
        {
            var campaignDTO = activeCampaignListDTOs.FirstOrDefault(c => c.Id == campaign.Id);
            if (campaignDTO != null)
            {
                campaignDTO.ProductNames = campaign.Products.Select(p => p.Name).ToList();
                campaignDTO.ProductIds = campaign.Products.Select(p => p.Id).ToList();

                var categoryIds = campaign.Products
                    .SelectMany(p => p.CategorySizeTypeProducts.Select(cstp => cstp.CategorySizeType.CategoryId))
                    .Distinct()
                    .ToList();

                var categoryNames = await GetCategoryNamesByIdsAsync(categoryIds);

                campaignDTO.CategoryIds = categoryIds;
                campaignDTO.CategoryNames = categoryNames;
            }
        }

        return new SuccessDataResult<List<CampaignListDTO>>(activeCampaignListDTOs, "Aktif kampanyalar başarıyla listelendi!");

    }
}


using ETicaret.Applicationn.DTOs.AdminDTOs;
using ETicaret.Applicationn.DTOs.CampaignDTOs;
using ETicaret.Applicationn.Services.CampaignServices;
using ETicaret.Applicationn.Services.CategoryServices;
using ETicaret.Applicationn.Services.ProductServices;
using ETicaret.UI.Areas.Admin.Models.AdminVMs;
using ETicaret.UI.Areas.Admin.Models.CampaignVMs;
using ETicaret.UI.Extentions;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;

namespace ETicaret.UI.Areas.Admin.Controllers
{
    public class CampaignController : AdminBaseController
    {
        private readonly ICampaignService _campaignService;
        private readonly IStringLocalizer<ModelResource> _stringLocalizer;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public CampaignController(ICampaignService campaignService, IStringLocalizer<ModelResource> stringLocalizer, IProductService productService, ICategoryService categoryService)
        {
            _campaignService = campaignService;
            _stringLocalizer = stringLocalizer;
            _productService = productService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _campaignService.GetAllAsync();
            var campaignListVMs = result.Data.Adapt<List<CampaignListVM>>();

            if (!result.IsSuccess)
            {

                NotifyError(_stringLocalizer["Campaigns could not be found!"]);
                return View(campaignListVMs);
            }
            NotifySucces(_stringLocalizer["Campaigns listed successfully!"]);
            return View(campaignListVMs);
        }

        public async Task<IActionResult> Create()
        {
            var model = new CampaignCreateVM();
            model.ProductList = await GetProductsAsync();
            model.CategoryList = await GetCategoriesAsync();
            //return View(model);
            return PartialView("_CampaignCreatePartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CampaignCreateVM campaignCreateVM)
        {
            if (!ModelState.IsValid)
            {
                campaignCreateVM.ProductList = await GetProductsAsync();
                campaignCreateVM.CategoryList = await GetCategoriesAsync();
                return View(campaignCreateVM);
            }

            //// JSON formatında gelen `ProductIds` ve `CategoryIds` değerlerini ayrıştır
            //campaignCreateVM.ProductIds = !string.IsNullOrEmpty(campaignCreateVM.ProductIdsJson)
            //    ? Newtonsoft.Json.JsonConvert.DeserializeObject<List<Guid>>(campaignCreateVM.ProductIdsJson)
            //    : new List<Guid>();
            //campaignCreateVM.CategoryIds = !string.IsNullOrEmpty(campaignCreateVM.CategoryIdsJson)
            //    ? Newtonsoft.Json.JsonConvert.DeserializeObject<List<Guid>>(campaignCreateVM.CategoryIdsJson)
            //    : new List<Guid>();

            var campaignCreateDto = campaignCreateVM.Adapt<CampaignCreateDTO>();
            campaignCreateVM.ProductList = await GetProductsAsync();
            if (campaignCreateVM.NewImage != null && campaignCreateVM.NewImage.Length > 0)
            {
                campaignCreateDto.Image = await campaignCreateVM.NewImage.StringToByteArrayAsync();
            }

            if (campaignCreateVM.CategoryIds != null && campaignCreateVM.CategoryIds.Any())
            {
                // Kategorilere göre kampanya ekleme işlemleri burada yapılacak
                campaignCreateDto.CategoryIds = campaignCreateVM.CategoryIds;
            }
            else if (campaignCreateVM.ProductIds != null && campaignCreateVM.ProductIds.Any())
            {
                // Ürünlere göre kampanya ekleme işlemleri burada yapılacak
                campaignCreateDto.ProductIds = campaignCreateVM.ProductIds;
            }

            var result = await _campaignService.CreateAsync(campaignCreateDto);

            if (!result.IsSuccess)
            {
                NotifyError(_stringLocalizer["Campaign already added!"]);
                campaignCreateVM.ProductList = await GetProductsAsync();
                campaignCreateVM.CategoryList = await GetCategoriesAsync();
                return View(campaignCreateVM);
            }

            NotifySucces(_stringLocalizer["Campaign added successfully!"]);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(Guid id)
        {
            var result = await _campaignService.GetByIdAsync(id);
            if(!result.IsSuccess)
            {
                NotifyError(_stringLocalizer["No data found to update!"]);
                return RedirectToAction("Index");
            }
            var campaignUpdateVM = result.Data.Adapt<CampaignUpdateVM>();
            campaignUpdateVM.ExistingImage = result.Data.Image;
            campaignUpdateVM.ProductList = await GetProductsAsync();
            campaignUpdateVM.CategoryList = await GetCategoriesAsync();
            //return View(campaignUpdateVM);
            return PartialView("_CampaignUpdatePartial", campaignUpdateVM);

        }

        [HttpPost]
        public async Task<IActionResult> Update(CampaignUpdateVM campaignUpdateVM)
        {
            var campaing = await _campaignService.GetByIdAsync(campaignUpdateVM.Id);
            if (!ModelState.IsValid)
            {
                campaignUpdateVM.ProductList = await GetProductsAsync();
                campaignUpdateVM.CategoryList = await GetCategoriesAsync();
                NotifyError(_stringLocalizer["Please fill in all fields as requested!"]);
                return View(campaignUpdateVM);
            }

            //// JSON formatında gelen `ProductIds` ve `CategoryIds` değerlerini ayrıştır
            //campaignUpdateVM.ProductIds = !string.IsNullOrEmpty(campaignUpdateVM.ProductIdsJson)
            //    ? Newtonsoft.Json.JsonConvert.DeserializeObject<List<Guid>>(campaignUpdateVM.ProductIdsJson)
            //    : new List<Guid>();
            //campaignUpdateVM.CategoryIds = !string.IsNullOrEmpty(campaignUpdateVM.CategoryIdsJson)
            //    ? Newtonsoft.Json.JsonConvert.DeserializeObject<List<Guid>>(campaignUpdateVM.CategoryIdsJson)
            //    : new List<Guid>();

            var campaignDto = campaignUpdateVM.Adapt<CampaignUpdateDTO>();

            if (campaignUpdateVM.NewImage != null && campaignUpdateVM.NewImage.Length > 0)
            {
                campaignDto.Image = await campaignUpdateVM.NewImage.StringToByteArrayAsync();
            }
            else
            {
                // Mevcut resmi kullan
                campaignDto.Image = campaing.Data.Image;
            }
            // Kategorilere göre kampanya uygulanıyorsa, ilgili ürün ID'lerini ProductIds listesine ekleyin
            if (campaignUpdateVM.CategoryIds != null && campaignUpdateVM.CategoryIds.Any())
            {
                var categoryProducts = new List<Guid>();
                foreach (var categoryId in campaignUpdateVM.CategoryIds)
                {
                    var productsResult = await _productService.GetProductsByCategoryIdAsync(categoryId);
                    if (productsResult.IsSuccess && productsResult.Data != null)
                    {
                        categoryProducts.AddRange(productsResult.Data.Select(p => p.Id));
                    }
                }
                campaignDto.ProductIds = categoryProducts;
                // Kategorilere göre kampanya güncelleme işlemleri burada yapılacak
                campaignDto.CategoryIds = campaignUpdateVM.CategoryIds;
            }
            else if (campaignUpdateVM.ProductIds != null && campaignUpdateVM.ProductIds.Any())
            {
                // Ürünlere göre kampanya güncelleme işlemleri burada yapılacak
                campaignDto.ProductIds = campaignUpdateVM.ProductIds;
            }

            var result = await _campaignService.UpdateAsync(campaignDto);

            if (!result.IsSuccess)
            {
                NotifyError(_stringLocalizer["Campaign could not be updated!"]);
                campaignUpdateVM.ProductList = await GetProductsAsync();
                campaignUpdateVM.CategoryList = await GetCategoriesAsync();
                return View(campaignUpdateVM);
            }

            NotifySucces(_stringLocalizer["Campaign updated successfully!"]);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _campaignService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                NotifyError(_stringLocalizer["Delete failed!"]);
                return Json(new { success = false, message = result.Messages });
            }
            NotifySucces(_stringLocalizer["Successfully deleted!"]);
            return Json(new { success = true });

        }
        private async Task<SelectList> GetProductsAsync()
        {
            var products = (await _productService.GetAllAsync()).Data;
            return new SelectList(products.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).OrderBy(x => x.Text), "Value", "Text");
        }

        //New for Category
        private async Task<SelectList> GetCategoriesAsync()
        {
            var categories = (await _categoryService.GetAllAsync()).Data;
            return new SelectList(categories.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).OrderBy(x => x.Text), "Value", "Text");
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _campaignService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                NotifyError(_stringLocalizer["Campaign could not be found!"]);
                return RedirectToAction("Index");
            }


            var campaignDetailVM = result.Data.Adapt<CampaignDetailVM>();

            //return View(campaignDetailVM);
            return PartialView("_CampaignDetailsPartial", campaignDetailVM);
        }

        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _campaignService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                return Json(new { success = false, message = result.Messages });
            }

            return Json(new { success = true, data = result.Data });
        }

    }
}

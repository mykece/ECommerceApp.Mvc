using ETicaret.Applicationn.DTOs.AdminDTOs;
using ETicaret.Applicationn.DTOs.CampaignDTOs;
using ETicaret.Applicationn.DTOs.CategoryDTOs;
using ETicaret.Applicationn.Services.CategoryServices;
using ETicaret.Applicationn.Services.ProductServices;
using ETicaret.Applicationn.Services.SizeServices;
using ETicaret.Applicationn.Services.SizeTypeServices;
using ETicaret.Infrastructure.Repositories.CategorySizeTypeProductRepositories;
using ETicaret.UI.Areas.Admin.Models.AdminVMs;
using ETicaret.UI.Areas.Admin.Models.CampaignVMs;
using ETicaret.UI.Areas.Admin.Models.CategorySizeTypeProductVMs;
using ETicaret.UI.Areas.Admin.Models.CategoryVMs;
using ETicaret.UI.Areas.Admin.Models.ProductCampaignLogVMs;
using ETicaret.UI.Extentions;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;

namespace ETicaret.UI.Areas.Admin.Controllers
{
    //[Area("Admin")]
    public class CategoryController : AdminBaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly ISizeTypeService _sizeTypeService;
        private readonly IStringLocalizer<ModelResource> _stringLocalizer;
        private readonly ICategorySizeTypeProductRepository _categorySizeTypeProductRepository;
        private readonly IProductService _productService;
        private readonly ISizeServices _sizeServices;


        public CategoryController(ICategoryService categoryService, ISizeTypeService sizeTypeService, IStringLocalizer<ModelResource> stringLocalizer, ICategorySizeTypeProductRepository categorySizeTypeProductRepository, IProductService productService, ISizeServices sizeServices)
        {
            _categoryService = categoryService;
            _sizeTypeService = sizeTypeService;
            _stringLocalizer = stringLocalizer;
            _categorySizeTypeProductRepository = categorySizeTypeProductRepository;
            _productService = productService;
            _sizeServices = sizeServices;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _categoryService.GetAllAsync();
            var categoryVMs = result.Data.Adapt<List<CategoryListVM>>();

            foreach (var category in categoryVMs)
            {
                category.ProductCount = await _categoryService.GetProductCountByCategoryIdAsync(category.Id);
            }
            if (!result.IsSuccess)
            {
                //await Console.Out.WriteLineAsync(result.Messages);
                NotifyError(_stringLocalizer["Category could not be found!"]);

                return View(categoryVMs);
            }
            //await Console.Out.WriteLineAsync(result.Messages);
            NotifySucces(_stringLocalizer["Category  listed succesfully!"]);

            return View(categoryVMs);
        }

        public async Task<IActionResult> Create()
        {
            var categoryCreateVM = new CategoryCreateVM();
            categoryCreateVM.SizeTypes = await GetSizeTypes();
            //return View(categoryCreateVM);
            return PartialView("_CategoryCreatePartial", categoryCreateVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateVM categoryCreateVM)
        {
            categoryCreateVM.SizeTypes = await GetSizeTypes();
            var categoryCreateDto = categoryCreateVM.Adapt<CategoryCreateDTO>();

            if(categoryCreateVM.NewImage !=null && categoryCreateVM.NewImage.Length > 0)
            {
                categoryCreateDto.Image = await categoryCreateVM.NewImage.StringToByteArrayAsync();
            }
            var result = await _categoryService.CreateAsync(categoryCreateDto);
            if (!result.IsSuccess)
            {
                //await Console.Out.WriteLineAsync(result.Messages);
                NotifyError(_stringLocalizer["Category already added!"]);

                return View(categoryCreateVM);
            }
            //await Console.Out.WriteLineAsync(result.Messages);
            NotifySucces(_stringLocalizer["Category added successfully!"]);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(Guid id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                NotifyError(_stringLocalizer["No data found to update!"]);
                RedirectToAction("Index");
            }
            var categoryUpdateVM = result.Data.Adapt<CategoryUpdateVM>();
            categoryUpdateVM.ExistingImage = result.Data.Image;
            categoryUpdateVM.SizeTypes = await GetSizeTypes();

            //return View(categoryUpdateVM);
            return PartialView("_CategoryUpdatePartial", categoryUpdateVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CategoryUpdateVM categoryUpdateVM)
        {
            var category = await _categoryService.GetByIdAsync(categoryUpdateVM.Id);
            categoryUpdateVM.SizeTypes = await GetSizeTypes();
            if (!ModelState.IsValid)
            {
                NotifyError(_stringLocalizer["Please fill in all fields as requested!"]);
                return View(categoryUpdateVM);
            }
            var categoryDto = categoryUpdateVM.Adapt<CategoryUpdateDTO>();
            if (categoryUpdateVM.NewImage != null && categoryUpdateVM.NewImage.Length > 0)
            {
                categoryDto.Image = await categoryUpdateVM.NewImage.StringToByteArrayAsync();
            }
            else
            {
                // Mevcut resmi kullan..
                categoryDto.Image = category.Data.Image;
            }

            var result = await _categoryService.UpdateAsync(categoryDto);
            if (!result.IsSuccess)
            {
                //await Console.Out.WriteLineAsync(result.Messages);
                NotifyError(_stringLocalizer["Category could not be updated!"]);

            }
            //await Console.Out.WriteLineAsync(result.Messages);
            NotifySucces(_stringLocalizer["Category updated successfully!"]);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _categoryService.DeleteAsync(id);
            if (!result.IsSuccess) 
            { 
                NotifyError(_stringLocalizer["Delete failed!"]);
                return Json(new { success = false, message = result.Messages });
            }
            NotifySucces(_stringLocalizer["Successfully deleted!"]);
            return Json(new { success = true });
        }
        public async Task<IActionResult> CategoryProducts(Guid id)
        {
            var categorySizeTypeProductsresult = await _categoryService.GetAllProductByCategoryId(id);
            if (!categorySizeTypeProductsresult.IsSuccess)
            {
                NotifyError(_stringLocalizer["Product could not be found!"]);
            }
            var categoryProductsList = new List<CategorySizeTypeProductListVM>();
            foreach (var item in categorySizeTypeProductsresult.Data)
            {
                var productResult = await _productService.GetByIdAsync(item.ProductId);
                var sizeResult = await _sizeServices.GetByIdAsync(item.SizeId);
                if(productResult.IsSuccess && sizeResult.IsSuccess && item.Quantity>0)
                {
                    var newCategoryProduct = new CategorySizeTypeProductListVM()
                    {
                        Id = item.ProductId,
                        Price = productResult.Data.UnitPrice,
                        ProductName=productResult.Data.Name,
                        Quantity=item.Quantity,
                        Size=sizeResult.Data.SizeName,
                        SizeType=sizeResult.Data.SizeTypeName,
                        Image=productResult.Data.Image,
                       
                    };
                    categoryProductsList.Add(newCategoryProduct);
                }
                continue;

            }
            NotifySucces(_stringLocalizer["Product listed successfully!"]);
            return PartialView("_CategoryProductsListPartial", categoryProductsList);
        }


        //private async Task<SelectList> GetSizeTypes(Guid? sizeTypeId = null)
        //{
        //    var sizeTypes = (await _sizeTypeService.GetAllAsync()).Data;
        //    return new SelectList(sizeTypes.Select(x => new SelectListItem
        //    {
        //        Value = x.Id.ToString(),
        //        Text = x.SizeTypeName,
        //        Selected = x.Id == (sizeTypeId != null ? sizeTypeId.Value : sizeTypeId)
        //    }).OrderBy(x => x.Text), "Value", "Text");
        //}

        private async Task<List<SelectListItem>> GetSizeTypes(Guid? sizeTypeId = null)
        {
            var sizeTypes = (await _sizeTypeService.GetAllAsync()).Data;
            return sizeTypes.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.SizeTypeName,
                Selected = sizeTypeId.HasValue && x.Id == sizeTypeId.Value
            }).OrderBy(x => x.Text).ToList();
        }


    }
}

using ETicaret.Applicationn.DTOs.CategoryDTOs;
using ETicaret.Applicationn.DTOs.CategorySizeTypeProductDTOs;
using ETicaret.Applicationn.DTOs.ProductDTOs;
using ETicaret.Applicationn.DTOs.SizeDTOs;
using ETicaret.Applicationn.Services.CategoryServices;
using ETicaret.Applicationn.Services.ProductCampaignLogServices;
using ETicaret.Applicationn.Services.ProductServices;
using ETicaret.Applicationn.Services.SizeServices;
using ETicaret.UI.Areas.Admin.Models.CategoryVMs;
using ETicaret.UI.Areas.Admin.Models.ProductCampaignLogVMs;
using ETicaret.UI.Areas.Admin.Models.ProductVMs;
using ETicaret.UI.Extentions;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System.Text;

namespace ETicaret.UI.Areas.Admin.Controllers
{
    //[Area("Admin")]
    public class ProductController : AdminBaseController
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ISizeServices _sizeService;
        private readonly IStringLocalizer<ModelResource> _stringLocalizer;
        private readonly IProductCampaignLogService _productCampaignLogService;

        public ProductController(IProductService productService, ICategoryService categoryService, ISizeServices sizeService, IStringLocalizer<ModelResource> stringLocalizer, IProductCampaignLogService productCampaignLogService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _sizeService = sizeService;
            _stringLocalizer = stringLocalizer;
            _productCampaignLogService = productCampaignLogService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _productService.GetAllAsync();
            var productListVMs = result.Data.Adapt<List<ProductListVM>>();
            foreach (var product in productListVMs)
            {
                product.Description = ConvertToHtmlDescription(product.Description);
            }

            if (!result.IsSuccess)
            {
                //await Console.Out.WriteLineAsync(result.Messages);
                NotifyError(_stringLocalizer["Product could not be found!"]);

                return View(productListVMs);
            }
            //await Console.Out.WriteLineAsync(result.Messages);
            NotifySucces(_stringLocalizer["Product  listed succesfully!"]);

            return View(productListVMs);
        }
        private string ConvertToHtmlDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                return description;
            }

            var sb = new StringBuilder();
            var lines = description.Split(new[] { "\n" }, StringSplitOptions.None);
            foreach (var line in lines)
            {
                sb.Append(line).Append("<br/>");
            }

            return sb.ToString();
        }
        public async Task<IActionResult> Create()
        {
            var productCreateVM = new ProductCreateVM();
            productCreateVM.Categories = await GetCategories();
            productCreateVM.Sizes = await GetSizes(/*productCreateVM.CategoryId*/);
            //return View(productCreateVM);
            return PartialView("_ProductCreatePartial", productCreateVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductRequest request)
        {

            var productCreateDto = new ProductCreateDTO()
            {
                CategoryIds = request.CategoryIds.Select(id => Guid.Parse(id)).ToList(),
                Description = request.ProductCreateVM.Description,
                Name = request.ProductCreateVM.Name,
                UnitPrice = request.ProductCreateVM.UnitPrice,
                Gender = request.ProductCreateVM.Gender,
                CategorySizeTypeProductCreateDTOs = request.CategorySizeTypeProductCreateDTOs
                
                

            };

            if (request.ProductCreateVM.NewImage != null && request.ProductCreateVM.NewImage.Length > 0)
            {
                productCreateDto.Image = await request.ProductCreateVM.NewImage.StringToByteArrayAsync();
            }

            if (request.CategoryIds == null || !request.CategoryIds.Any())
            {
                NotifyError(_stringLocalizer["Please select at least one category!"]);
                return View(request.ProductCreateVM);
            }
            var result = await _productService.CreateAsync(productCreateDto);
            if (!result.IsSuccess)
            {

                NotifyError(_stringLocalizer["Product already added!"]);

                return View(productCreateDto);
            }
            NotifySucces(_stringLocalizer["Product added successfully!"]);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(Guid id)
        {
            var result = await _productService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                NotifyError(_stringLocalizer["No data found to update!"]);
                return RedirectToAction("Index");
            }
            var productUpdateVM = result.Data.Adapt<ProductUpdateVM>();
            productUpdateVM.ExistingImage = result.Data.Image;

            // CategoryIds ve SizeIds değerlerini ViewModel'e ata
            productUpdateVM.CategoryIds = result.Data.CategoryIds;
            productUpdateVM.SizeIds = result.Data.SizeIds;
            // CategorySizeTypeProductUpdateDTOs listesinin null olmadığından emin olun
            productUpdateVM.CategorySizeTypeProductUpdateDTOs = new List<CategorySizeTypeProductUpdateDTO>();
            // Dropdownlar için kategorileri ve boyutları getir
            productUpdateVM.Categories = await GetCategories();
            productUpdateVM.Sizes = await GetSizes(productUpdateVM.CategoryIds.FirstOrDefault());

            var categorySizeTypeProductResult = await _productService.GetByProductId(id);
            if (categorySizeTypeProductResult.IsSuccess)
            {
                productUpdateVM.CategorySizeTypeProductUpdateDTOs = categorySizeTypeProductResult.Data
                    .Adapt<List<CategorySizeTypeProductUpdateDTO>>();
            }
            else
            {
                NotifyError(_stringLocalizer["Error loading product details!"]);
            }

            return PartialView("_ProductUpdatePartial", productUpdateVM);
        }



        [HttpPost]
        public async Task<IActionResult> Update([FromForm] UpdateProductRequest request)
        {
            var categoriesIds = request.CategorySizeTypeProductUpdateDTOs.Select(x => x.CategoryId).ToList();


            var productUpdateDto = new ProductUpdateDTO()
            {
                Id=request.ProductUpdateVM.Id,
                CategoryIds = categoriesIds,
                Description = request.ProductUpdateVM.Description,
                Name = request.ProductUpdateVM.Name,
                UnitPrice = request.ProductUpdateVM.UnitPrice,
                Gender = request.ProductUpdateVM.Gender,
                CategorySizeTypeProductUpdateDTOs = request.CategorySizeTypeProductUpdateDTOs
            };


            if (request.ProductUpdateVM.NewImage != null && request.ProductUpdateVM.NewImage.Length > 0)
            {
                
                productUpdateDto.Image = await request.ProductUpdateVM.NewImage.StringToByteArrayAsync();
                
            }
            if (categoriesIds == null || !categoriesIds.Any())
            {
                NotifyError(_stringLocalizer["Please select at least one category!"]);
                return View(request.ProductUpdateVM);
            }

            var result = await _productService.UpdateAsync(productUpdateDto);
            if (!result.IsSuccess)
            {
                NotifyError(_stringLocalizer["Product already updated!"]);

                return View(productUpdateDto);
            }
            //await Console.Out.WriteLineAsync(result.Messages);
            NotifySucces(_stringLocalizer["Product updated successfully!"]);

            return RedirectToAction("Index", "Product");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _productService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                NotifyError(_stringLocalizer["Delete failed!"]);
                return Json(new { success = false, message = result.Messages });
            }
            NotifySucces(_stringLocalizer["Successfully deleted!"]);
            return Json(new { success = true });

        }
        private async Task<SelectList> GetCategories(Guid? categoryId = null)
        {
            var categories = (await _categoryService.GetAllAsync()).Data;
            return new SelectList(categories.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
                Selected = x.Id == (categoryId != null ? categoryId.Value : categoryId)
            }).OrderBy(x => x.Text), "Value", "Text");
        }
        private async Task<SelectList> GetSizes(Guid? categoryId = null)
        {
            var sizes = (await _sizeService.GetAllAsync()).Data;
            return new SelectList(sizes.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.SizeName,
                Selected = x.Id == (categoryId != null ? categoryId.Value : categoryId)
            }).OrderBy(x => x.Text), "Value", "Text");
        }


        [HttpPost]
        public async Task<IActionResult> GetSizesByCategoryIds([FromBody] List<CategoryDataVM> categories)
        {
            if (categories == null || !categories.Any())
            {
                return Json(new List<SelectListItem>());
            }

            var categoryIds = categories.Select(c => Guid.Parse(c.Value)).ToList();

            var sizes = new List<SizeDTO>();

            // İlk kategorinin bedenlerini alın
            var initialCategorySizes = (await _sizeService.GetByCategoryIdAsync(categoryIds.First())).Data;
            var commonSizes = new HashSet<Guid>(initialCategorySizes.Select(s => s.Id));

            // Diğer kategorilerin bedenleriyle kesiştirin
            foreach (var categoryId in categoryIds.Skip(1))
            {
                var categorySizes = (await _sizeService.GetByCategoryIdAsync(categoryId)).Data;
                commonSizes.IntersectWith(categorySizes.Select(s => s.Id));
            }

            // Kesişimdeki bedenleri alın
            var finalSizes = initialCategorySizes.Where(s => commonSizes.Contains(s.Id)).ToList();

            var sizeSelectList = finalSizes.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.SizeName
            }).OrderBy(x => x.Text).ToList();

            return Json(sizeSelectList);


        }

        [HttpGet]
        public async Task<IActionResult> GetSizeNamesByIds([FromQuery] List<Guid> sizeIds)
        {
            var sizes = new List<SizeDTO>();
            foreach (var item in sizeIds)
            {
                var size = await _sizeService.GetByIdAsync(item);
                sizes.Add(size.Data);
            }
            
            var sizeNames = sizes.Select(s => new { s.Id, s.SizeName }).ToList();
            return Json(sizeNames);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryNamesByIds(List<Guid> categoryIds)
        {
            var categories = new List<CategoryDTO>();
            foreach (var item in categoryIds)
            {
                var category = _categoryService.GetByIdAsync(item).Result.Data;
                categories.Add(category);

            }
            var result = categories.Select(c => new { id = c.Id, name = c.Name }).ToList();
            return Json(result);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetSizeNamesByIds(List<Guid> sizeIds)
        //{
        //    var sizes = new List<SizeDTO>();
        //    foreach(var item in sizeIds)
        //    {
        //        var size = _sizeService.GetByIdAsync(item).Result.Data;
        //        sizes.Add(size);
        //    }
        //    var result = sizes.Select(s => new { id = s.Id, name = s.SizeName }).ToList();
        //    return Json(result);
        //}

        [HttpGet]
        public async Task<IActionResult> GetSizeName(Guid id)
        {
            var sizeResult = await _sizeService.GetByIdAsync(id);
            if (sizeResult.IsSuccess)
            {
                var size = sizeResult.Data;
                return Json(size.SizeName);
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryName(Guid id)
        {
            
            var categoryResult = await _categoryService.GetCategoryByCategorySizeTypeId(id);
            if (categoryResult.IsSuccess)
            {
                var category = categoryResult.Data;
                return Json(category.Name);
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetCategorySizeType(Guid sizeId, Guid categoryId)
        {
            var categorySizeTypeResult= _productService.GetCategorySizeType(sizeId, categoryId);
            return Json(categorySizeTypeResult.Result.Data.Id);
        }

        public async Task<IActionResult> GetProductCampaign(Guid id)
        {
            var result = await _productCampaignLogService.GetAllByProductIdAsync(id);
            return PartialView("_ProductCampaignLogCreatePartial", result.Data.Adapt<List<ProductCampaignLogListVM>>());
        }

    }
}

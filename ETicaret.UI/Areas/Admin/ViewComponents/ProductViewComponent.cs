using ETicaret.Applicationn.Services.ProductServices;
using ETicaret.Domain.Enums;
using ETicaret.UI.Areas.Admin.Models.ProductVMs;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.UI.Areas.Admin.ViewComponents
{
    public class ProductViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public ProductViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Tüm ürünlerin sayısını al
            var totalProducts = await _productService.GetProductCountByGenderAsync();

            var womenProducts = await _productService.GetProductCountByGenderAsync(Gender.Woman);

            var menProducts = await _productService.GetProductCountByGenderAsync(Gender.Man);

            var unisexProducts = await _productService.GetProductCountByGenderAsync(Gender.Unisex);


            var model = new ProductCountVM
            {
                TotalProducts = totalProducts,
                WomenProducts = womenProducts,
                MenProducts = menProducts,
                UnisexProducts = unisexProducts

            };


            return View(model);
        }
    }
}

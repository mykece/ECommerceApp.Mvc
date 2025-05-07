using ETicaret.Applicationn.DTOs.ProductDTOs;
using ETicaret.Applicationn.Services.ProductServices;
using ETicaret.UI.Areas.Admin.Models.ProductVMs;
using ETicaret.UI.Areas.Admin.Models.ProfileVMs;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.UI.ViewComponents
{
    public class NewProductsViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public NewProductsViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _productService.GetLatestProductsAsync(10);
            var isAuthenticated = HttpContext.User.Identity.IsAuthenticated;

            if (result.IsSuccess)
            {
                var lastProductsVM = new LastProductsVM()
                {
                    Products = result.Data.Adapt<List<ProductListVM>>(),
                    IsAuthenticated = isAuthenticated
                };
                return View("Default", lastProductsVM);
            }
            else
            {
                return View("Default", new List<LastProductsVM>());
            }
        }
    }
}

using ETicaret.Applicationn.DTOs.CategorySizeTypeProductDTOs;
using ETicaret.Applicationn.Services.CampaignServices;
using ETicaret.Applicationn.Services.CategoryServices;
using ETicaret.Applicationn.Services.CharServices;
using ETicaret.Applicationn.Services.ProductServices;
using ETicaret.Applicationn.Services.SizeServices;
using ETicaret.Domain.Enums;
using ETicaret.Domain.Utilities.Concretes;
using ETicaret.UI.Areas.Admin.Models.CategorySizeTypeProductVMs;
using ETicaret.UI.Areas.Admin.Models.ProductVMs;
using ETicaret.UI.Areas.Admin.Models.SizeVMs;
using ETicaret.UI.Models;
using ETicaret.UI.Models.CategoryVMs;
using ETicaret.UI.Models.ProductVMs;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers; // Bu satýrý ekleyin

namespace ETicaret.UI.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ICartService _cartService;
        private readonly ISizeServices _sizeServices;
        private readonly ICampaignService _campaignService;

        public HomeController(ILogger<HomeController> logger, IProductService productService, ICategoryService categoryService, ICartService cartService, ISizeServices sizeServices, ICampaignService campaignService)
        {
            _logger = logger;
            _productService = productService;
            _categoryService = categoryService;
            _cartService = cartService;
            _sizeServices = sizeServices;
            _campaignService=campaignService;
        }
        public async Task<IActionResult> CategoryProducts(Guid id)
        {
            // Öncelikle, exchange oranýný alýn
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://currency-exchange.p.rapidapi.com/exchange?from=USD&to=TRY&q=1.0"),
                Headers =
        {
            { "x-rapidapi-key", "630ce9cc86msh271c60cffe62d5ep1b514djsn0fe292593744" },
            { "x-rapidapi-host", "currency-exchange.p.rapidapi.com" },
        },
            };

            decimal exchangeRate = 0;
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var exchangeRateString = await response.Content.ReadAsStringAsync();

                // Exchange oranýný decimal'e dönüþtürün
                if (decimal.TryParse(exchangeRateString, out var parsedRate))
                {
                    exchangeRate = parsedRate;
                }
            }

            // Kategoriye ait ürünleri al
            var productsResult = await _productService.GetProductsByCategoryIdAsync(id);
            if (!productsResult.IsSuccess)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            // Kategori bilgilerini al
            var categoryResult = await _categoryService.GetByIdAsync(id);
            if (!categoryResult.IsSuccess)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            // ViewModel oluþtur ve exchange oranýný ekle
            var viewModel = new CategoryProductListVm
            {
                CategoryId = id,
                CategoryName = categoryResult.Data.Name,
                Products = productsResult.Data,
                Exchange = exchangeRate // Exchange oranýný decimal olarak ekliyoruz
            };

            return View(viewModel);
        }


        //public async Task<IActionResult> AddProductToCard(Guid id)
        //{

        //}

        public async Task<IActionResult> Product(Guid id)
        {
            var result = await _productService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            var productCardVm = new ProductCardVM()
            {
                Description = result.Data.Description,
                Id = id,
                Image = result.Data.Image,
                Name = result.Data.Name,
                UnitPrice = result.Data.UnitPrice,
                OriginalPrice = result.Data.OriginalPrice,
            };
            var categorySizeTypeProductResult = await _productService.GetByProductId(id);
            if (!categorySizeTypeProductResult.IsSuccess)
            {
                productCardVm.Sizes = new List<SizeListVM>();
                return View(productCardVm);
            }
            var sizes = new List<SizeListVM>();
            foreach (var item in categorySizeTypeProductResult.Data)
            {
                var sizeResult = await _sizeServices.GetByIdAsync(item.SizeId);
                if (sizeResult.IsSuccess)
                {
                    sizes.Add(sizeResult.Data.Adapt<SizeListVM>());
                }
                continue;
            }
            if (sizes is null)
            {
                productCardVm.Sizes = new List<SizeListVM>();
                return View(productCardVm);
            }
            productCardVm.Sizes =sizes;
            return View(productCardVm);
        }


        public async Task<IActionResult>Index()
        {
            return View();
        }

        // PrivacyViewModel'i kullanarak sayfayý ilk kez yükler
        [HttpGet]
        public IActionResult Privacy()
        {
            var model = new PrivacyViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Privacy(PrivacyViewModel model)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://chatgpt-42.p.rapidapi.com/gpt4"),
                Headers =
        {
            { "x-rapidapi-key", "a06b4308a0mshef46508cb4efda2p15f724jsn19e024d85458" },
            { "x-rapidapi-host", "chatgpt-42.p.rapidapi.com" },
        },
                Content = new StringContent($"{{\"messages\":[{{\"role\":\"user\",\"content\":\"Boyum {model.Height} cm ve kilom {model.Weight} kg. Bu bilgilere göre {model.ClothingType} için hangi beden bana uygundur?\"}}],\"web_access\":false}}")
                {
                    Headers =
            {
                ContentType = new MediaTypeHeaderValue("application/json")
            }
                }
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();

                // JSON yanýtýný deserialize et
                var responseModel = JsonConvert.DeserializeObject<ResponseModel>(body);

                // Sadece "Result" kýsmýný alýyoruz
                model.ResponseMessage = responseModel?.Result;
            }

            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> ProductDetails(Guid id)
        {
            var productResult = await _productService.GetByIdAsync(id);

            if (!productResult.IsSuccess)
            {
                // Handle error case
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            var viewModel = new ProductDetailVM
            {

                Name = productResult.Data.Name,
                Description = productResult.Data.Description,
                UnitPrice = productResult.Data.UnitPrice,
                Image = productResult.Data.Image,
                Quantity = productResult.Data.Quantity,
            };

            return View(viewModel);
        }

        public async Task<IActionResult> AllProducts(Gender? gender)
        {
            
            var productsResult = await _productService.GetAllAsync();

            if (!productsResult.IsSuccess)
            {
                
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            // Cinsiyete göre ürünleri filtrele veya tüm ürünleri getirme
            var filteredProducts = gender.HasValue
                ? productsResult.Data.Where(product => product.Gender == gender.Value)
                : productsResult.Data;

            
            var allProductsVM = filteredProducts
                .Select(product => new ProductListVM
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    UnitPrice = product.UnitPrice,
                    Image = product.Image,
                    Quantity = (int)product.Quantity,
                    Gender = product.Gender
                })
                .ToList();
            
            return View(allProductsVM);
        }


        //Kampanya ait ürünlerin getilmesi
        public async Task<IActionResult> ActiveCampaignProducts(Guid campaignId)
        {
            var campaignResult = await _campaignService.GetByIdAsync(campaignId);
            if (!campaignResult.IsSuccess)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            var campaign = campaignResult.Data;

            var campaignProducts = new List<ProductListVM>();

            foreach (var productId in campaign.ProductIds)
            {
                var productResult = await _productService.GetByIdAsync(productId);
                if (productResult.IsSuccess)
                {
                    campaignProducts.Add(new ProductListVM
                    {
                        Id = productResult.Data.Id,
                        Name = productResult.Data.Name,
                        Description = productResult.Data.Description,
                        UnitPrice = productResult.Data.UnitPrice,
                        Image = productResult.Data.Image,
                        Gender = productResult.Data.Gender,
                        OriginalPrice = productResult.Data.OriginalPrice,
                    });
                }
            }

            return View(campaignProducts);
        }

    }
}


using ETicaret.Applicationn.DTOs.OrderDetailDTOs;
using ETicaret.Applicationn.DTOs.OrderDTO;
using ETicaret.Applicationn.Services.AccountServices;
using ETicaret.Applicationn.Services.OrderServices;
using ETicaret.Domain.Entities;
using ETicaret.Infrastructure.Repositories.CategorySizeTypeProductRepositories;
using ETicaret.UI.Models.OrderVMs;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class OrderController : Controller
{
    private readonly ICategorySizeTypeProductRepository _categorySizeTypeProductRepository;
    private readonly IAccountService _accountService;
    private readonly IOrderService _orderService;

    public OrderController(ICategorySizeTypeProductRepository categorySizeTypeProductRepository, IAccountService accountService, IOrderService orderService)
    {
        _categorySizeTypeProductRepository = categorySizeTypeProductRepository;
        _accountService = accountService;
        _orderService = orderService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(List<OrderCreateVM> orderCreateVMs)
    {
        // Kullanıcının giriş yapıp yapmadığını kontrol edin
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }

        // Giriş yapan kullanıcının ID'sini alın
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("Kullanıcı ID'si bulunamadı.");
        }

        var identityUser = await _accountService.FindByIdAsync(userId);
        if (identityUser == null)
        {
            return Unauthorized("Kullanıcı bulunamadı.");
        }
        var order = new OrderCreateDTO() 
        { 
            CustomerId= _accountService.GetUserIdAsync(userId,"Customer").Result,
            
        };

        var orderDetailCreateDTOs = new List<OrderDetailCreateDTO>();
        // Devam eden işlemler (orderCreateVMs ile işlem yapma vb.)
        foreach (var item in orderCreateVMs)
        {
            var sizeId = Guid.Parse(item.SizeId);
            var categorySizeTypeProduct = await _categorySizeTypeProductRepository.GetAsync(x => x.SizeId == sizeId && x.Product.Name ==item.ProductName);
            var orderItem = new OrderDetailCreateDTO() 
            { 
                CategorySizeTypeProductId=categorySizeTypeProduct.Id,
                Quantity=Double.Parse(item.Quantity),
                UnitPrice=Decimal.Parse(item.UnitPrice),
            };
            orderDetailCreateDTOs.Add(orderItem);

        }
        order.OrderDetailCreateDTOs=orderDetailCreateDTOs;
        var result = await _orderService.CreateAsync(order);
        if (result.IsSuccess)
        {
            return RedirectToAction("OrderConfirmation");
        }


        return RedirectToAction("Index","Cart");
    }


    public IActionResult OrderConfirmation()
    {
        // Sipariş onay sayfası
        return View();
    }
}



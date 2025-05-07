using ETicaret.Applicationn.Services.CharServices;
using ETicaret.Applicationn.Services.ProductServices;
using ETicaret.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.UI.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly IProductService _productService;


    public CartController(ICartService cartService, IProductService productService)
    {
        _cartService = cartService;
        _productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        var cart = await _cartService.GetCartAsync();
        return View(cart);
    }

    public async Task<IActionResult> AddToCart(Guid productId, int quantity)
    {
        var categorySizeTypeProductResult = await _productService.GetCategorySizeTypeProduct(productId);
        if(categorySizeTypeProductResult.IsSuccess)
        {
            await _cartService.AddItemAsync(categorySizeTypeProductResult.Data, quantity);
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> RemoveFromCart(Guid productId)
    {
        await _cartService.RemoveItemAsync(productId);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> ClearCart()
    {
        await _cartService.ClearCartAsync();
        return RedirectToAction("Index");
    }
}

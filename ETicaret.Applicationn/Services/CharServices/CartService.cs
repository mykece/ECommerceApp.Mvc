using ETicaret.Applicationn.DTOs.CategorySizeTypeProductDTOs;
using ETicaret.Applicationn.Services.ProductServices;
using ETicaret.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.Services.CharServices;

public class CartService : ICartService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProductService _productService;

    public CartService(IHttpContextAccessor httpContextAccessor, IProductService productService)
    {
        _httpContextAccessor = httpContextAccessor;
        _productService = productService;
    }

    public async Task<Cart> GetCartAsync()
    {
        var session = _httpContextAccessor.HttpContext.Session;
        var cartJson = session.GetString("Cart");
        return string.IsNullOrEmpty(cartJson) ? new Cart() : JsonConvert.DeserializeObject<Cart>(cartJson);
    }

    public async Task SaveCartAsync(Cart cart)
    {
        var session = _httpContextAccessor.HttpContext.Session;
        var cartJson = JsonConvert.SerializeObject(cart);
        session.SetString("Cart", cartJson);
    }

    public async Task ClearCartAsync()
    {
        var session = _httpContextAccessor.HttpContext.Session;
        session.Remove("Cart");
    }

    public async Task AddItemAsync(CategorySizeTypeProductDTO product, int quantity)
    {
        var cart = await GetCartAsync();
        var existingItem = cart.Items.FirstOrDefault(i => i.CategorySizeTypeProductId == product.Id);
        var existingProductResult = await _productService.GetByIdAsync(product.ProductId);
        if (existingItem == null)
        {
            cart.Items.Add(new CartItem
            {
                CategorySizeTypeProductId = product.Id,
                ProductName = existingProductResult.Data.Name,
                Quantity = quantity,
                UnitPrice = existingProductResult.Data.UnitPrice
            });
        }
        else
        {
            existingItem.Quantity += quantity;
        }

        await SaveCartAsync(cart);
    }

    public async Task RemoveItemAsync(Guid productId)
    {
        var cart = await GetCartAsync();
        cart.Items.RemoveAll(i => i.CategorySizeTypeProductId == productId);
        await SaveCartAsync(cart);
    }

    public async Task<decimal> ComputeTotalValueAsync()
    {
        var cart = await GetCartAsync();
        return cart.Items.Sum(i => i.UnitPrice * i.Quantity);
    }
}


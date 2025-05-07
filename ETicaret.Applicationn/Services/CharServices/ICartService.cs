using ETicaret.Applicationn.DTOs.CategorySizeTypeProductDTOs;
using ETicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.Services.CharServices;

public interface ICartService
{
    Task<Cart> GetCartAsync();
    Task SaveCartAsync(Cart cart);
    Task ClearCartAsync();
    Task AddItemAsync(CategorySizeTypeProductDTO product, int quantity);
    Task RemoveItemAsync(Guid productId);
    Task<decimal> ComputeTotalValueAsync();
}

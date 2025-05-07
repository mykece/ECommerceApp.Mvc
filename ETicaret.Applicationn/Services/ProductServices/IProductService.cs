using ETicaret.Applicationn.DTOs.CategorySizeTypeDTOs;
using ETicaret.Applicationn.DTOs.CategorySizeTypeProductDTOs;
using ETicaret.Applicationn.DTOs.ProductDTOs;
using ETicaret.Domain.Entities;
using ETicaret.Domain.Enums;
using ETicaret.Domain.Utilities.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.Applicationn.Services.ProductServices
{
    public interface IProductService
    {
        Task<IDataResult<ProductDTO>> CreateAsync(ProductCreateDTO productCreateDTO);
        Task<IDataResult<List<ProductListDTO>>> GetAllAsync();
        Task<IResult> DeleteAsync(Guid id);
        Task<IDataResult<ProductDTO>> GetByIdAsync(Guid id);
        Task<IDataResult<ProductDTO>> UpdateAsync(ProductUpdateDTO productUpdateDTO);
        Task<IDataResult<List<CategorySizeTypeProductListDTO>>> GetByProductId(Guid productId);
        Task<IDataResult<CategorySizeTypeDTO>> GetCategorySizeType(Guid sizeId, Guid categoryId);
        Task<IDataResult<List<ProductListDTO>>> GetLatestProductsAsync(int count);
        Task<IDataResult<List<ProductListDTO>>> GetProductsByCategoryIdAsync(Guid categoryId);
        Task<IDataResult<CategorySizeTypeProductDTO>> GetCategorySizeTypeProduct(Guid categorySizeTypeProductId);
        Task<int> GetProductCountByGenderAsync(Gender? gender = null); // Product Count dashboard için 
    }
}

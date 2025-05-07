using ETicaret.Applicationn.DTOs.CategoryDTOs;
using ETicaret.Applicationn.DTOs.CategorySizeTypeProductDTOs;
using ETicaret.Applicationn.DTOs.ProductDTOs;
using ETicaret.Applicationn.DTOs.SizeTypeDTOs;
using ETicaret.Domain.Enums;
using ETicaret.Domain.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<IDataResult<CategoryDTO>> CreateAsync(CategoryCreateDTO categoryCreateDTO);
        Task<IDataResult<List<CategoryListDTO>>> GetAllAsync();
        Task<IResult> DeleteAsync(Guid id);
        Task<IDataResult<CategoryDTO>> GetByIdAsync(Guid id);
        Task<IDataResult<CategoryDTO>> UpdateAsync(CategoryUpdateDTO categoryUpdateDTO);
        Task<IDataResult<CategoryDTO>> GetCategoryByCategorySizeTypeId(Guid id);
        Task<IDataResult<List<CategorySizeTypeProductListDTO>>> GetAllProductByCategoryId(Guid id);
        Task<int> GetProductCountByCategoryIdAsync(Guid categoryId);
        Task<IDataResult<List<CategoryListDTO>>> GetCategoriesByProductGenderAsync(Gender gender);  //ürünlerin Gender özelliklerine göre kategorileri getiren bir metod

    }
}

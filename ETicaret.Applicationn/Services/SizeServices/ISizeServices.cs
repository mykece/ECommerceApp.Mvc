using ETicaret.Applicationn.DTOs.ProductDTOs;
using ETicaret.Applicationn.DTOs.SizeDTOs;
using ETicaret.Domain.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.Services.SizeServices;

public interface ISizeServices
{
    Task<IDataResult<SizeDTO>> CreateAsync(SizeCreateDTO sizeCreateDTO);
    Task<IDataResult<List<SizeListDTO>>> GetAllAsync();
    Task<IResult> DeleteAsync(Guid id);
    Task<IDataResult<SizeDTO>> GetByIdAsync(Guid id);
    Task<IDataResult<SizeDTO>> UpdateAsync(SizeUpdateDTO sizeUpdateDTO);
    Task<IDataResult<List<SizeDTO>>> GetByCategoryIdAsync(Guid categoryId);
}

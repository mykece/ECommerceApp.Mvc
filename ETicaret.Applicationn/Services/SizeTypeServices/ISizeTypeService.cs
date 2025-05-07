using ETicaret.Applicationn.DTOs.ProductDTOs;
using ETicaret.Applicationn.DTOs.SizeTypeDTOs;
using ETicaret.Domain.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.Services.SizeTypeServices
{
    public interface ISizeTypeService
    {
        Task<IDataResult<SizeTypeDTO>> CreateAsync(SizeTypeCreateDTO sizeTypeCreateDTO);
        Task<IDataResult<List<SizeTypeListDTO>>> GetAllAsync();
        Task<IResult> DeleteAsync(Guid id);
        Task<IDataResult<SizeTypeDTO>> GetByIdAsync(Guid id);
        Task<IDataResult<SizeTypeDTO>> UpdateAsync(SizeTypeUpdateDTO sizeTypeUpdateDTO);
    }
}

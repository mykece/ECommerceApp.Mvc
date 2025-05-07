using ETicaret.Applicationn.DTOs.CategoryDTOs;
using ETicaret.Applicationn.DTOs.OrderDTO;
using ETicaret.Domain.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.Services.OrderServices
{
    public interface IOrderService
    {
        Task<IDataResult<OrderDTO>> CreateAsync(OrderCreateDTO orderCreateDTO);
        Task<IDataResult<List<OrderListDTO>>> GetAllAsync();
        Task<IResult> DeleteAsync(Guid id);
        Task<IDataResult<OrderDTO>> GetByIdAsync(Guid id);
        Task<IDataResult<OrderDTO>> UpdateAsync(OrderUpdateDTO orderUpdateDTO);
        
    }
}

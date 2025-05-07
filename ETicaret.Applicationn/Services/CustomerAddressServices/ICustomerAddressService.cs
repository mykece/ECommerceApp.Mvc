using ETicaret.Applicationn.DTOs.CategoryDTOs;
using ETicaret.Applicationn.DTOs.CustomerAddressDTOs;
using ETicaret.Domain.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.Services.CustomerAddressServices
{
    public interface ICustomerAddressService
    {
        Task<IDataResult<CustomerAddressDTO>> CreateAsync(CustomerAddressCreateDTO customerAddressCreateDTO);
        Task<IDataResult<List<CustomerAddressListDTO>>> GetAllAsync();
        Task<IResult> DeleteAsync(Guid id);
        Task<IDataResult<CustomerAddressDTO>> GetByIdAsync(Guid id);
        Task<IDataResult<CustomerAddressDTO>> UpdateAsync(CustomerAddressUpdateDTO customerAddressUpdateDTO);
    }
}

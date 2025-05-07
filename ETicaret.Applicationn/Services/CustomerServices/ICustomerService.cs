using ETicaret.Applicationn.DTOs.AdminDTOs;
using ETicaret.Applicationn.DTOs.CustomerDTOs;
using ETicaret.Domain.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.Services.CustomerServices
{
    public interface ICustomerService
    {
        Task<IDataResult<List<CustomerListDTO>>> GetAllAsync();
        Task<IDataResult<CustomerDTO>> CreateAsync(CustomerCreateDTO customerCreateDTO);
        Task<IResult> DeleteAsync(Guid id);
        Task<IDataResult<CustomerDTO>> GetByIdAsync(Guid id);
        Task<IDataResult<CustomerDTO>> UpdateAsync(CustomerUpdateDTO customerUpdateDTO);
        Task<IDataResult<CustomerDTO>> GetByIdentityUserIdAsync(string identityUserId);
        Task<IDataResult<CustomerDTO>> GetCustomerWithAccessor();
        Task<IDataResult<CustomerDTO>> CreateAsyncByAdmin(CustomerCreateDTO customerCreateDTO);
    }
}

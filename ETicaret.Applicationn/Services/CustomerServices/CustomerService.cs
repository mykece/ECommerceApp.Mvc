using ETicaret.Applicationn.DTOs.AdminDTOs;
using ETicaret.Applicationn.DTOs.CustomerDTOs;
using ETicaret.Applicationn.Services.AccountServices;
using ETicaret.Domain.Entities;
using ETicaret.Domain.Utilities.Concretes;
using ETicaret.Domain.Utilities.Interfaces;
using ETicaret.Infrastructure.Repositories.CustomerRepositories;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IResult = ETicaret.Domain.Utilities.Interfaces.IResult;

namespace ETicaret.Applicationn.Services.CustomerServices
{
    public class CustomerService : ICustomerService
    {
        private readonly IAccountService _accountService;
        private readonly ICustomerRepository _customerRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        public CustomerService(IAccountService accountService, ICustomerRepository customerRepository, IHttpContextAccessor contextAccessor)
        {
            _accountService = accountService;
            _customerRepository = customerRepository;
            _contextAccessor = contextAccessor;
        }

        public async Task<IDataResult<CustomerDTO>> CreateAsync(CustomerCreateDTO customerCreateDTO) // Transaction Controllerda yapıldığı için burada yapılmadı.
        {

            var newCustomer = customerCreateDTO.Adapt<Customer>();
            await _customerRepository.AddAsync(newCustomer);
            await _customerRepository.SaveChangesAsync();
            return new SuccessDataResult<CustomerDTO>("Customer Added Succesfully!");
        }

        public async Task<IDataResult<CustomerDTO>> UpdateAsync(CustomerUpdateDTO customerUpdateDTO)
        {
            var updatingCustomer = await _customerRepository.GetByIdAsync(customerUpdateDTO.Id);
            if (updatingCustomer == null)
            {
                return new ErrorDataResult<CustomerDTO>("Customer could not be found");
            }
            if (updatingCustomer.Email != customerUpdateDTO.Email && await _accountService.AnyAsync(x => x.Email == customerUpdateDTO.Email))
            {
                return new ErrorDataResult<CustomerDTO>("This Email is already taken");
            }

            var identityUser = await _accountService.FindByIdAsync(updatingCustomer.IdentityId);
            identityUser.Email = customerUpdateDTO.Email;
            identityUser.NormalizedEmail = customerUpdateDTO.Email.ToUpperInvariant();
            identityUser.UserName = customerUpdateDTO.Email;
            identityUser.NormalizedUserName = customerUpdateDTO.Email.ToUpperInvariant();

            DataResult<CustomerDTO> result = new ErrorDataResult<CustomerDTO>();
            var strategy = await _customerRepository.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                var transactionScope = await _customerRepository.BeginTransactionAsync().ConfigureAwait(false);
                try
                {
                    var identityResult = await _accountService.UpdateUserAsync(identityUser);
                    if (!identityResult.Succeeded)
                    {
                        result = new ErrorDataResult<CustomerDTO>(identityResult.ToString());
                        transactionScope.Rollback();
                        return;
                    }
                    var updatedCustomer = customerUpdateDTO.Adapt(updatingCustomer);

                    await _customerRepository.UpdateAsync(updatedCustomer);
                    await _customerRepository.SaveChangesAsync();
                    result = new SuccessDataResult<CustomerDTO>("Customer updated successfully!");
                    transactionScope.Commit();
                }
                catch (Exception ex)
                {
                    result = new ErrorDataResult<CustomerDTO>("Customer could not be updated" + ex.Message);
                    transactionScope.Rollback();
                }
                finally
                {
                    transactionScope.Dispose();
                }
            });
            return result;
        }


        public async Task<IResult> DeleteAsync(Guid id)
        {
            var deletingCustomer = await _customerRepository.GetByIdAsync(id);
            if (deletingCustomer == null)
            {
                return new ErrorDataResult<IResult>("Customer could not be found!");
            }

            var strategy = await _customerRepository.CreateExecutionStrategy();
            Result result = new ErrorResult();

            await strategy.ExecuteAsync(async () =>
            {
                var transactionScope = await _customerRepository.BeginTransactionAsync().ConfigureAwait(false);
                try
                {
                    var identityResult = await _accountService.DeleteUserAsync(deletingCustomer.IdentityId);
                    if (!identityResult.Succeeded)
                    {
                        result = new ErrorResult(identityResult.ToString());
                        transactionScope.Rollback();
                        return;
                    }
                    await _customerRepository.DeleteAsync(deletingCustomer);
                    await _customerRepository.SaveChangesAsync();
                    result = new SuccessResult("Customer deleted successfully!");
                    transactionScope.Commit();
                }
                catch (Exception ex)
                {
                    result = new ErrorResult("Customer could not be deleted" + ex.Message);
                    transactionScope.Rollback();
                }
                finally
                {
                    transactionScope.Dispose();
                }
            });

            return result;

        }

        public async Task<IDataResult<List<CustomerListDTO>>> GetAllAsync()
        {
            var customers = await _customerRepository.GetAllAsync();
            var customerDTOs = customers.Adapt<List<CustomerListDTO>>();
            if (customerDTOs.Count <= 0)
            {
                return new ErrorDataResult<List<CustomerListDTO>>(customerDTOs, "Customer could not be found!");
            }
            return new SuccessDataResult<List<CustomerListDTO>>(customerDTOs, " Customer listed successfully!");
        }

        public async Task<IDataResult<CustomerDTO>> GetByIdAsync(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return new ErrorDataResult<CustomerDTO>("Customer could not be found!");
            }
            var customerDTO = customer.Adapt<CustomerDTO>();
            return new SuccessDataResult<CustomerDTO>(customerDTO, "Customer found successfully!");
        }

        public async Task<IDataResult<CustomerDTO>> GetByIdentityUserIdAsync(string identityUserId)
        {
            var customer = await _customerRepository.GetByIdentityId(identityUserId);
            if (customer == null)
            {
                return new ErrorDataResult<CustomerDTO>("Customer bulunamadı!");
            }
            var customerDTO = customer.Adapt<CustomerDTO>();
            return new SuccessDataResult<CustomerDTO>(customerDTO, "Customer başarıyla bulundu!");
        }

        public async Task<IDataResult<CustomerDTO>> GetCustomerWithAccessor()
        {
            var user = _contextAccessor.HttpContext.User;
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var loginCustomer = await _customerRepository.GetByIdentityId(userId);
            if (loginCustomer == null)
            {
                return new ErrorDataResult<CustomerDTO>("Login olan kullanıcı bulunamadı.");
            }
            return new SuccessDataResult<CustomerDTO>(loginCustomer.Adapt<CustomerDTO>(), "Login olan kullanıcı bulundu.");
        }

        public async Task<IDataResult<CustomerDTO>> CreateAsyncByAdmin(CustomerCreateDTO customerCreateDTO)
        {
            if (await _accountService.AnyAsync(x => x.Email == customerCreateDTO.Email))
            {
                return new ErrorDataResult<CustomerDTO>("Email taken. Customer is already exist!");
            }
            IdentityUser identityUser = new()
            {
                Email = customerCreateDTO.Email,
                NormalizedEmail = customerCreateDTO.Email.ToUpperInvariant(),
                UserName = customerCreateDTO.Email,
                NormalizedUserName = customerCreateDTO.Email.ToUpperInvariant(),
                EmailConfirmed = true
            };

            DataResult<CustomerDTO> result = new ErrorDataResult<CustomerDTO>();

            var strategy = await _customerRepository.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                var transactionScope = await _customerRepository.BeginTransactionAsync().ConfigureAwait(false);
                try
                {
                    var identityResult = await _accountService.CreateUserAsync(identityUser, Domain.Enums.Roles.Customer);
                    if (!identityResult.Succeeded)
                    {
                        result = new ErrorDataResult<CustomerDTO>(identityResult.ToString());
                        transactionScope.Rollback();
                        return;
                    }
                    var customer = customerCreateDTO.Adapt<Customer>();
                    customer.IdentityId = identityUser.Id;
                    await _customerRepository.AddAsync(customer);
                    await _customerRepository.SaveChangesAsync();
                    result = new SuccessDataResult<CustomerDTO>("Customer added successfully!");
                    transactionScope.Commit();
                }
                catch (Exception ex)
                {
                    result = new ErrorDataResult<CustomerDTO>("Customer could not be added!" + ex.Message);
                    transactionScope.Rollback();
                }
                finally
                {
                    transactionScope.Dispose();
                }
            });
                return result;
        }
    }
}

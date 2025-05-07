using ETicaret.Applicationn.DTOs.CustomerAddressDTOs;
using ETicaret.Applicationn.DTOs.SizeDTOs;
using ETicaret.Domain.Entities;
using ETicaret.Domain.Utilities.Concretes;
using ETicaret.Domain.Utilities.Interfaces;
using ETicaret.Infrastructure.Repositories.CustomerAddressRepositories;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.Services.CustomerAddressServices
{
    public class CustomerAddressService : ICustomerAddressService
    {
        private readonly ICustomerAddressRepository _customerAddressRepository;

        public CustomerAddressService(ICustomerAddressRepository customerAddressRepository)
        {
            _customerAddressRepository = customerAddressRepository;
        }

        public async Task<IDataResult<CustomerAddressDTO>> CreateAsync(CustomerAddressCreateDTO customerAddressCreateDTO)
        {
            var customerAddress = customerAddressCreateDTO.Adapt<CustomerAddress>();
            await _customerAddressRepository.AddAsync(customerAddress);
            await _customerAddressRepository.SaveChangesAsync();
            var customerAddressDTO = customerAddress.Adapt<CustomerAddressDTO>();
            return new SuccessDataResult<CustomerAddressDTO>(customerAddressDTO, "Address Ekleme Başarılı");
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
           var deletingAddress = await _customerAddressRepository.GetByIdAsync(id);
            if (deletingAddress == null)
            {
                new ErrorResult("Silinecek Adres Bulunamadı.");
            }

            await _customerAddressRepository.DeleteAsync(deletingAddress);
            await _customerAddressRepository.SaveChangesAsync();
            return new SuccessResult("Adres Başarıyla Silindi");
        }

        public async Task<IDataResult<List<CustomerAddressListDTO>>> GetAllAsync()
        {
            var addresses = await _customerAddressRepository.GetAllAsync();
            var addressListDTOs = addresses.Adapt<List<CustomerAddressListDTO>>();
            if (addresses.Count() <= 0)
            {
                return new ErrorDataResult<List<CustomerAddressListDTO>>(addressListDTOs, "Listelenecek Adres Bulunamadı!");
            }
                return new SuccessDataResult<List<CustomerAddressListDTO>>(addressListDTOs, " Adres Listeleme  Başarılı!");
        }

        public async Task<IDataResult<CustomerAddressDTO>> GetByIdAsync(Guid id)
        {
            var address = await _customerAddressRepository.GetByIdAsync(id);
            if (address is null)
            {
                return new ErrorDataResult<CustomerAddressDTO>("Gösterilecek Adres Bulunamadı");
            }
            var addressDTO = address.Adapt<CustomerAddressDTO>();
            return new SuccessDataResult<CustomerAddressDTO>(addressDTO, "Adres Gösterme Başarılı");
        }

        public async Task<IDataResult<CustomerAddressDTO>> UpdateAsync(CustomerAddressUpdateDTO customerAddressUpdateDTO)
        {
            var updatingAddress = await _customerAddressRepository.GetByIdAsync(customerAddressUpdateDTO.Id);
            if (updatingAddress is null)
            {
                return new ErrorDataResult<CustomerAddressDTO>("Güncellenecek Adres Bulunamadı");
            }
            var updatedAddress = customerAddressUpdateDTO.Adapt(updatingAddress);
            await _customerAddressRepository.UpdateAsync(updatedAddress);
            await _customerAddressRepository.SaveChangesAsync();
            return new SuccessDataResult<CustomerAddressDTO>(updatedAddress.Adapt<CustomerAddressDTO>(), "Adres Güncelleme Başarılı");
        }
    }
}

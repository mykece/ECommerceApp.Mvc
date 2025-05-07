using ETicaret.Applicationn.DTOs.AdminDTOs;
using ETicaret.Applicationn.DTOs.CustomerAddressDTOs;
using ETicaret.Applicationn.DTOs.CustomerDTOs;
using ETicaret.Applicationn.Services.CustomerAddressServices;
using ETicaret.Applicationn.Services.CustomerServices;
using ETicaret.UI.Areas.Admin.Models.AdminVMs;
using ETicaret.UI.Areas.Admin.Models.CustomerVMs;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System.Security.Policy;

namespace ETicaret.UI.Areas.Admin.Controllers
{
    public class CustomerController : AdminBaseController
    {
        private readonly ICustomerService _customerService;
        private readonly ICustomerAddressService _customerAddressService;
        private readonly IStringLocalizer<ModelResource> _stringLocalizer;

        public CustomerController(ICustomerService customerService, IStringLocalizer<ModelResource> stringLocalizer, ICustomerAddressService customerAddressService)
        {
            _customerService = customerService;
            _stringLocalizer = stringLocalizer;
            _customerAddressService = customerAddressService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _customerService.GetAllAsync();
            var customerVMs = result.Data.Adapt<List<CustomerListVM>>();
            if (!result.IsSuccess)
            {
                NotifyError(_stringLocalizer["Customer could not be found"]);
                return View(customerVMs);
            }
            NotifySucces(_stringLocalizer["Customer listed successfully!"]);
            return View(customerVMs);
        }

        public async Task<IActionResult> Create()
        {
            var model = new CustomerCreateVM();
            return PartialView("_CustomerCreatePartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CustomerCreateVM customerCreateVM)
        {
            if (!ModelState.IsValid)
            {
                return View(customerCreateVM);
            }

            var customerCreateDTO = customerCreateVM.Adapt<CustomerCreateDTO>();
            var result = await _customerService.CreateAsyncByAdmin(customerCreateDTO);
            if (!result.IsSuccess)
            {
                NotifyError(_stringLocalizer["Customer already added"]);
                return RedirectToAction("Index");
            }
            NotifySucces(_stringLocalizer["Customer added successfully!"]);
            return RedirectToAction("Index", "Customer");
        }


        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _customerService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                NotifyError(_stringLocalizer["Customer could not be deleted"]);
                return RedirectToAction("Index", "Customer");
            }
            NotifySucces(_stringLocalizer["Customer deleted successfully!"]);
            return RedirectToAction("Index", "Customer");
        }


        public async Task<IActionResult> Update(Guid id)
        {
            var result = await _customerService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                return View("Error");
            }
            var customerUpdateVM = result.Data.Adapt<CustomerUpdateVM>();
            return PartialView("_CustomerUpdatePartial", customerUpdateVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CustomerUpdateVM customerUpdateVM)
        {
            if (!ModelState.IsValid)
            {
                return View(customerUpdateVM);
            }
            var customerDTO = customerUpdateVM.Adapt<CustomerUpdateDTO>();

            var result = await _customerService.UpdateAsync(customerDTO);
            if (!result.IsSuccess)
            {
                NotifyError(_stringLocalizer["Customer could not be updated!"]);
                return RedirectToAction("Index");
            }

            NotifySucces(_stringLocalizer["Customer updated successfully!"]);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> CreateAddress(Guid id)
        {
            var result = await _customerService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                return View("Error");
            }
            var createAddressVM = new CreateAddressVM();
            createAddressVM.CustomerId = result.Data.Id;
            return PartialView("_CreateAddressPartial", createAddressVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddress(CreateAddressVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var customerAddressResult = await _customerAddressService.CreateAsync(model.Adapt<CustomerAddressCreateDTO>());
            if (!customerAddressResult.IsSuccess)
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetCustomerAddress(Guid id)
        {
            var result = await _customerAddressService.GetAllAsync();
            var customerAddressVMs = result.Data.Adapt<List<CustomerAddressListVM>>();
            if (!result.IsSuccess)
            {
                NotifyError(_stringLocalizer["Customer Address could not be found"]);
                return View(customerAddressVMs);
            }
            var selectedCustomerAddress = customerAddressVMs.Where(x => x.CustomerId == id);
            NotifySucces(_stringLocalizer["Customer Address listed successfully!"]);
            return PartialView("_GetCustomerAddress", selectedCustomerAddress);
        }




        public async Task<IActionResult> DeleteCustomerAddress(Guid id)
        {
            var result = await _customerAddressService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                NotifyError(_stringLocalizer["Customer address could not be deleted"]);
                return RedirectToAction("Index");
            }
            NotifySucces(_stringLocalizer["Customer address could not be deleted"]);
            return RedirectToAction("Index");
        }


        private async Task<SelectList> GetCustomers(Guid? customerId = null)
        {
            var customers = (await _customerService.GetAllAsync()).Data;
            return new SelectList(customers.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.FirstName + x.LastName,
                Selected = x.Id == (customerId != null ? customerId.Value : customerId)
            }).OrderBy(x => x.Text), "Value", "Text");
        }
    }
}

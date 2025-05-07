using ETicaret.Applicationn.DTOs.CustomerAddressDTOs;
using ETicaret.Applicationn.DTOs.CustomerDTOs;
using ETicaret.Applicationn.DTOs.MailDTOs;
using ETicaret.Applicationn.Services.CustomerAddressServices;
using ETicaret.Applicationn.Services.CustomerServices;
using ETicaret.Applicationn.Services.MailServices;
using ETicaret.Domain.Entities;
using ETicaret.Domain.Enums;
using ETicaret.Domain.Utilities.Concretes;
using ETicaret.Infrastructure.Repositories.CustomerRepositories;
using ETicaret.UI.Models.AccountVMs;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MimeKit;
using System.Security.Claims;


namespace ETicaret.UI.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMailService _mailService;
        private readonly ICustomerService _customerService;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerAddressService _customerAddressService;
        private readonly IStringLocalizer<ModelResource> _stringLocalizer;



        private const string customerPassword = "Password.1";

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IMailService mailService, ICustomerService customerService, ICustomerRepository customerRepository, ICustomerAddressService customerAddressService, IStringLocalizer<ModelResource> stringLocalizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
            _customerService = customerService;
            _customerRepository = customerRepository;
            _customerAddressService = customerAddressService;
            _stringLocalizer = stringLocalizer;
        }


        public async Task<IActionResult> Register()
        {
            var registerVM = new RegisterVM();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model) //Customer Transaction'u Controller'da yapıldı.
        { // Transaction Yazılacak
            if (!ModelState.IsValid)
            {
                NotifyError(_stringLocalizer["Please fill in all fields as requested!"]);
                Console.Out.WriteLineAsync("Üyelik İşlemleri sırasında bir hata oluştu.");
                return View(model);
            }
            if((await _userManager.FindByEmailAsync(model.Email)) != null)
            {
                NotifyError(_stringLocalizer["This email has already been registered."]);
                return View(model);
            }
            DataResult<CustomerDTO> result = new ErrorDataResult<CustomerDTO>();

            var strategy = await _customerRepository.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                var transactionScope = await _customerRepository.BeginTransactionAsync().ConfigureAwait(false);
                try
                {
                    var customer = new IdentityUser { UserName = model.Email, Email = model.Email };
                    var identityResult = await _userManager.CreateAsync(customer, customerPassword);
                    if (!identityResult.Succeeded)
                    {
                        result = new ErrorDataResult<CustomerDTO>(identityResult.ToString());
                        transactionScope.Rollback();
                        return;

                    }
                    await _userManager.AddToRoleAsync(customer, Roles.Customer.ToString());
                    var confirmCode = await _userManager.GenerateEmailConfirmationTokenAsync(customer);
                    var url = Url.Action("ConfirmEmail", "Account", new { customerId = customer.Id, code = confirmCode }, protocol: Request.Scheme);
                    var mailDTO = new SendMailDTO();
                    mailDTO.Email = model.Email;
                    mailDTO.Subject = "Üyelik İşlemleri";
                    mailDTO.Message = $"Hesabınızın onaylanması için <a href='{url}'>link</a>e tıklayınız!..<br/> Şifreniz = {customerPassword}";

                    await _mailService.SendMailAsync(mailDTO);
                    var customerCreateDTO = model.Adapt<CustomerCreateDTO>();
                    customerCreateDTO.IdentityId = customer.Id;
                    await _customerService.CreateAsync(customerCreateDTO);
                    Console.Out.WriteLineAsync("Üye işlemleriniz tamamlanması için mailinizi kontrol etmelisiniz");
                    transactionScope.Commit();
                }
                catch (Exception ex)
                {
                    result = new ErrorDataResult<CustomerDTO>("Customer Ekleme Başarısız" + ex.Message);
                    transactionScope.Rollback();
                }
                finally
                {
                    transactionScope.Dispose();
                }
            });
            return RedirectToAction("Login");

        }

        public async Task<IActionResult> ConfirmEmail(string customerId, string code)
        {
            var customer = await _userManager.FindByIdAsync(customerId);
            await _userManager.ConfirmEmailAsync(customer, code);
            return RedirectToAction("Login");
        }


        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(CustomerLoginVM model) // Buraya Bakılacak
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var customer = await _userManager.FindByEmailAsync(model.Email);
            if (customer == null)
            {
                Console.Out.WriteLineAsync("Kullanıcı veya şifre hatalı");
                return View(model);
            }

            var checkPassword = await _signInManager.PasswordSignInAsync(customer, model.Password, false, false);
            if (!checkPassword.Succeeded)
            {
                Console.Out.WriteLineAsync("Kullanıcı veya şifre hatalı");
                return View(model);
            }

            var userRole = await _userManager.GetRolesAsync(customer);
            if (userRole == null)
            {
                Console.Out.WriteLineAsync("Kullanıcı veya şifre hatalı");
                return View(model);
            }

            return RedirectToAction("Index", "Home");

        }


        public async Task<IActionResult> Profile()
        {
            var customer = await _userManager.GetUserAsync(User);
            if (customer == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı oturumu bulunamadı. Lütfen giriş yapın.");
                return RedirectToAction("Login");
            }

            var result = await _customerService.GetByIdentityUserIdAsync(customer.Id);
            var customerDTO = result.Data as CustomerDTO;
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, "Customer bilgisi bulunamadı. Lütfen tekrar deneyin.");
                return RedirectToAction("Index", "Home");
            }

            var customerProfileVM = result.Data.Adapt<CustomerProfileVM>();
            customerProfileVM.Addresses= (await _customerAddressService.GetAllAsync()).Data.Where(x=>x.CustomerId==customerDTO.Id).Adapt<List<CustomerAddressVM>>();
            return View(customerProfileVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddress(CustomerAddressVM model)
        {
            var loginCustomerResult = await _customerService.GetCustomerWithAccessor();
            model.CustomerId = loginCustomerResult.Data.Id;
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var customerAddressResult = await _customerAddressService.CreateAsync(model.Adapt<CustomerAddressCreateDTO>());
            if (!customerAddressResult.IsSuccess)
            {
                return View(model);
            }
             return RedirectToAction("Profile", "Account");
            
        }

        public async Task<IActionResult> CreateAddress()
        {
            var customerAddressVM = new CustomerAddressVM();
            return PartialView("_CreateAddressPartial", customerAddressVM);
            //return View(customerAddressVM);
        }

        public async Task<IActionResult> DeleteAddress(Guid id)
        {
            var result = await _customerAddressService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                return RedirectToAction("Profile", "Account");
            }
            return RedirectToAction("Profile", "Account");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


    }
}

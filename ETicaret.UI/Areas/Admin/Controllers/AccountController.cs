using ETicaret.Applicationn.DTOs.AdminDTOs;
using ETicaret.Applicationn.DTOs.MailDTOs;
using ETicaret.Applicationn.Services.AdminService;
using ETicaret.Applicationn.Services.MailServices;
using ETicaret.Domain.Entities;
using ETicaret.Domain.Enums;
using ETicaret.Infrastructure.Repositories.AdminRepositories;
using ETicaret.UI.Areas.Admin.Models.AccountVMs;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;

namespace ETicaret.UI.Areas.Admin.Controllers
{
    //[Area("Admin")]
    public class AccountController : AdminBaseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IAdminService _adminService;
        private readonly IMailService _emailService;
        private readonly IStringLocalizer<ModelResource> _stringLocalizer;
        private readonly IAdminRepository _adminRepository;



        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IAdminService adminService, IMailService emailService, IStringLocalizer<ModelResource> stringLocalizer, IAdminRepository adminRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _adminService = adminService;
            _emailService = emailService;
            _stringLocalizer = stringLocalizer;
            _adminRepository = adminRepository;
        }
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                //ModelState.AddModelError(string.Empty, "Kullanıcı oturumu bulunamadı. Lütfen giriş yapın.");
                NotifyError(_stringLocalizer["User session not found. Please log in."]);
                return RedirectToAction("Login");
            }

            var result = await _adminService.GetByIdentityUserIdAsync(user.Id);
            if (!result.IsSuccess)
            {
                //ModelState.AddModelError(string.Empty, "Admin bilgisi bulunamadı. Lütfen tekrar deneyin.");
                NotifyError(_stringLocalizer["Admin information not found. Please try again."]);
                return RedirectToAction("Index", "Home");
            }

            var adminDto = result.Data;
            return PartialView("_ProfilePartial", adminDto);
        }


        public async Task<IActionResult> Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["ErrorMessage"] = _stringLocalizer["Email or password incorrect!"];
                return View(model);
            }

            var checkPass = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!checkPass.Succeeded)
            {
                //Console.WriteLine("Email veya şifre hatalı");
                TempData["ErrorMessage"] = _stringLocalizer["Email or password incorrect!"];
                NotifyError(_stringLocalizer["Email or password incorrect!"]);
                return View(model);
            }
            var userRole = await _userManager.GetRolesAsync(user);
            if (userRole == null)
            {
                TempData["ErrorMessage"] = _stringLocalizer["Email or password incorrect!"];
                NotifyError(_stringLocalizer["Email or password incorrect!"]);
                return View(model);
            }
            NotifySucces(_stringLocalizer["Login successfully."]);
            return RedirectToAction("Index", "Home", new { Area = "Admin" });    //userRole[0].ToString() 'da kullanılabilir.
                                                                                 //return RedirectToAction("Index", "Category", new { Area = "Admin" });
        }



        //      {
        //          if (!ModelState.IsValid)
        //          {
        //              return View(model);
        //          }
        //          var user = await _userManager.FindByEmailAsync(model.Email);
        //          if (user == null)
        //          {
        //              //Console.WriteLine("Email veya şifre hatalı!");
        //              NotifyError(_stringLocalizer["Email or password incorrect!"]);

        //              return View(model);
        //          }

        //          var checkPass = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

        //	if (!checkPass.Succeeded)
        //	{
        //              //Console.WriteLine("Email veya şifre hatalı");
        //              NotifyError(_stringLocalizer["Email or password incorrect!"]);

        //              return View(model);
        //	}

        //	var userRole = await _userManager.GetRolesAsync(user);
        //	if (userRole == null)
        //	{
        //		//Console.WriteLine("Email veya şifre hatalı");
        //              NotifyError(_stringLocalizer["Email or password incorrect!"]);

        //              return View(model);
        //	}

        //	return RedirectToAction("Index", "Home", new { Area = "Admin" });  //userRole[0].ToString() 'da kullanılabilir.
        //																	   //return RedirectToAction("Index", "Category", new { Area = "Admin" });

        //}

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account", new { Area = "Admin" });
        }

        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {

            if (ModelState.IsValid)
            {
                var result = await _adminService.ResetPasswordRequestAsync(model.Email);

                if (result.IsSuccess == false)
                {
                    //Console.WriteLine("Your e-mail is not found. Please check your e-mail.");
                    NotifyError(_stringLocalizer["Your e-mail is not found. Please check your e-mail."]);
                    return View(model);
                }

                //Mail gönder işlemleri
                var conformationLink = Url.Action("ResetPassword", "Account", new { Code = result.Data.Token, email = result.Data.Email }, Request.Scheme);
                var message = new SendMailDTO();
                message.Email = result.Data.Email;
                message.Message = $"To reset your password, please click the link below:  {conformationLink}";
                message.Subject = "Reset Password";

                await _emailService.SendMailAsync(message);

                Console.WriteLine("Please check your mailbox and verify your email!");
                return RedirectToAction("ResetPasswordConfirmation", "Account", new { area = "Admin" });

            }
            return View(model);


        }

        public async Task<IActionResult> ResetPassword(string email, string Code)
        {
            var model = new ResetPasswordVM();

            if (email != null && Code != null)
            {
                model.Code = Code;
                model.Email = email;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _adminService.ResetPasswordAsync(new AdminResetPasswordDTO() { Code = model.Code, Email = model.Email, Password = model.Password });

            if (!result.IsSuccess)
            {
                //Console.WriteLine("Your account could not confirmed. Please register again.");
                NotifyError(_stringLocalizer["Your account could not confirmed. Please register again."]);
                return View(model);
            }
            //Console.WriteLine("Password is updated!");
            NotifySucces(_stringLocalizer["Password is updated!"]);

            var response = Request.Form["g-recaptcha-response"];
            const string secretKey = "6LdmYAQqAAAAAOzVq7SE4UykEGfAbzG33hZt6C5w";

            var client = new WebClient();
            var reply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            var captchaResponse = JsonConvert.DeserializeObject<GoogleCaptcha>(reply); //GoogleCaptcha UI'da açtıgımız class adı.

            if (!captchaResponse.Success)
            {
                TempData["Message"] = "Lütfen güvenliği doğrulayın";
                return View(model);
            }
            else
            {
                TempData["Message"] = "Doğrulandı";
            }
            return RedirectToAction("Login", "Account");
        }




        [HttpGet]
        public async Task<IActionResult> ChangePassword(Guid id)
        {
            var model = new ChangePasswordVM { Id = id };
            return PartialView("_ChangePasswordPartial", model);
            //return View("ChangePassword");

        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            // Kullanıcı ID'sini claim'lerden al
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!ModelState.IsValid)
            {
                NotifyError(_stringLocalizer["Validation failed. Please correct the errors and try again."]);
                return RedirectToAction("Profile", "Account", new { Area = "Admin" });
                //return View(model);
            }

            var response = Request.Form["g-recaptcha-response"];
            const string secretKey = "6LdmYAQqAAAAAOzVq7SE4UykEGfAbzG33hZt6C5w";

            var client = new WebClient();
            var reply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            var captchaResponse = JsonConvert.DeserializeObject<GoogleCaptcha>(reply); //GoogleCaptcha UI'da açtıgımız class adı.

            if (!captchaResponse.Success)
            {
                TempData["Message"] = "Lütfen güvenliği doğrulayın";
                return View(model);
            }
            else
            {
                TempData["Message"] = "Doğrulandı";
            }

            var dto = model.Adapt<AdminChangePasswordDTO>();
             
            var adminId = await  _adminService.GetByIdentityUserIdAsync(userId);
            dto.Id = adminId.Data.Id;
            var result = await _adminService.ChangePasswordAsync(dto);

            if (!result.IsSuccess)
            {
                //Console.WriteLine("Password change failed. Please try again."); 
                //return View(model);
                NotifyError(_stringLocalizer["Password change failed. Please try again."]);
                return RedirectToAction("Profile", "Account", new { Area = "Admin" });
            }
            //Console.WriteLine("Password change was successful.");
            NotifySucces(_stringLocalizer["Password change was successful."]);
            return RedirectToAction("Index", "Home", new { Area = "Admin" });
        }

        public IActionResult ResetPasswordConfirmation()

        {

            return View();
        }
    }
}




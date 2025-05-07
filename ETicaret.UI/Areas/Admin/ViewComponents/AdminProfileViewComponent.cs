using ETicaret.Applicationn.Services.AdminService;
using ETicaret.UI.Areas.Admin.Models.ProfileVMs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ETicaret.UI.Areas.Admin.Components
{
    public class AdminProfileViewComponent : ViewComponent
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAdminService _adminService;

        public AdminProfileViewComponent(UserManager<IdentityUser> userManager, IAdminService adminService)
        {
            _userManager = userManager;
            _adminService = adminService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(UserClaimsPrincipal);
            if (user == null)
            {
                return View<AdminProfileViewModel>("Default", null);
            }

            var result = await _adminService.GetByIdentityUserIdAsync(user.Id);
            if (!result.IsSuccess)
            {
                return View<AdminProfileViewModel>("Default", null);
            }

            var adminDto = result.Data;
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();
            var profileInfo = new AdminProfileViewModel
            {
                FullName = $"{adminDto.FirstName} {adminDto.LastName}",
                Email = adminDto.Email,
                Role = role,
                Image = adminDto.Image != null ? $"data:image/jpeg;base64,{Convert.ToBase64String(adminDto.Image)}" : "https://www.gravatar.com/avatar/00000000000000000000000000000000?d=mp&f=y"
            };

            return View("Default", profileInfo);
        }
    }
}

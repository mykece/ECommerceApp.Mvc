using ETicaret.Domain.Core.BaseEntities;
using ETicaret.Domain.Enums;
using ETicaret.Infrastructure.Repositories.AdminRepositories;
using ETicaret.Infrastructure.Repositories.CustomerRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.Services.AccountServices
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAdminRepository _adminRepository;
        private readonly ICustomerRepository _customerRepository;


        public AccountService(UserManager<IdentityUser> userManager, IAdminRepository adminRepository, ICustomerRepository customerRepository)
        {
            _userManager = userManager;
            _adminRepository = adminRepository;
            _customerRepository = customerRepository;
        }
        public async Task<IdentityResult> CreateUserAsync(IdentityUser user, Roles role)
        {
            var result = await _userManager.CreateAsync(user, "Password.1");
            if (!result.Succeeded)
            {
                return result;
            }
            return await _userManager.AddToRoleAsync(user, role.ToString());
        }

        public async Task<bool> AnyAsync(Expression<Func<IdentityUser, bool>> expression)
        {
            return await _userManager.Users.AnyAsync(expression);
        }


        public async Task<IdentityResult> DeleteUserAsync(string identityId)
        {
            var user = await _userManager.FindByIdAsync(identityId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError()
                {
                    Code = "Kullanıcı Bulunamadı",
                    Description = "Kullanıcı Bulunamadı"
                });
            }
            return await _userManager.DeleteAsync(user);
        }

        public async Task<IdentityUser?> FindByIdAsync(string identityId)
        {
            return await _userManager.FindByIdAsync(identityId);
        }

        public async Task<Guid> GetUserIdAsync(string identityId, string role)
        {
            BaseUser? user = role switch
            {
                "Admin" => await _adminRepository.GetByIdentityId(identityId),
                "Customer" => await _customerRepository.GetByIdentityId(identityId),
                _ => null,
            };
            return user is null ? Guid.NewGuid() : user.Id;
        }

        public async Task<IdentityResult> UpdateUserAsync(IdentityUser user)
        {
            var updatingUser = await _userManager.FindByIdAsync(user.Id);
            if (updatingUser == null)
            {
                return IdentityResult.Failed(new IdentityError()
                {
                    Code = "Güncellenecek User Bulunamadı",
                    Description = "Güncellenecek User Bulunamadı"
                });
            }

            return await _userManager.UpdateAsync(user);
        }
		

		public async Task<IdentityResult> ChangePasswordAsync(IdentityUser user, string currentPassword, string newPassword)
		{
			return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
		}

		public async Task<IdentityUser?> FindByEmailAsync(string email)  //Email karşılık bir kullanıcı durumu sorgulama
		{
			return await _userManager.FindByEmailAsync(email);
		}

		public async Task<string> GeneratePasswordResetTokenAsync(IdentityUser user)  //usera özel reset token üretme 
		{
			return await _userManager.GeneratePasswordResetTokenAsync(user);
		}

		public async Task<IdentityResult> ResetPasswordAsync(IdentityUser user, string token, string newPassword)   //token şifreleme
		{
			return await _userManager.ResetPasswordAsync(user, token, newPassword);
		}

        
    }
}

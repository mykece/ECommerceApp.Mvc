using ETicaret.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.Services.AccountServices
{
    public interface IAccountService
    {
        Task<bool> AnyAsync(Expression<Func<IdentityUser, bool>> expression);
        Task<IdentityUser>? FindByIdAsync(string identityId); 
        Task<IdentityResult> CreateUserAsync(IdentityUser user, Roles role);
        Task<IdentityResult> DeleteUserAsync(string identityId);
        Task<Guid> GetUserIdAsync(string identityID, string role);
        Task<IdentityResult> UpdateUserAsync(IdentityUser user);
        //Şifre İşlemleri
        Task<IdentityResult> ChangePasswordAsync(IdentityUser user, string currentPassword, string newPassword);
        Task<IdentityUser?> FindByEmailAsync(string email);
        Task<string> GeneratePasswordResetTokenAsync(IdentityUser user);
        Task<IdentityResult> ResetPasswordAsync(IdentityUser user, string token, string newPassword);




	}
}

using ETicaret.Applicationn.DTOs.AdminDTOs;
using ETicaret.Applicationn.Services.AccountServices;
using ETicaret.Domain.Entities;
using ETicaret.Domain.Utilities.Concretes;
using ETicaret.Domain.Utilities.Interfaces;
using ETicaret.Infrastructure.Repositories.AdminRepositories;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.Services.AdminService
{
    public class AdminService : IAdminService
    {
        private readonly IAccountService _accountService;
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAccountService accountService, IAdminRepository adminRepository)
        {
            _accountService = accountService;
            _adminRepository = adminRepository;
        }


		public async Task<IDataResult<AdminDTO>> CreateAsync(AdminCreateDTO adminCreateDTO)
        {
            if (await _accountService.AnyAsync(x => x.Email == adminCreateDTO.Email))
            {
                return new ErrorDataResult<AdminDTO>("Email is already taken!");
            }
            IdentityUser identityUser = new()
            {
                Email = adminCreateDTO.Email,
                NormalizedEmail = adminCreateDTO.Email.ToUpperInvariant(),
                UserName = adminCreateDTO.Email,
                NormalizedUserName = adminCreateDTO.Email.ToUpperInvariant(),
                EmailConfirmed = true
            };

            DataResult<AdminDTO> result = new ErrorDataResult<AdminDTO>();

            var strategy = await _adminRepository.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                var transactionScope = await _adminRepository.BeginTransactionAsync().ConfigureAwait(false);
                try
                {
                    var identityResult = await _accountService.CreateUserAsync(identityUser, Domain.Enums.Roles.Admin);
                    if (!identityResult.Succeeded)
                    {
                        result = new ErrorDataResult<AdminDTO>(identityResult.ToString());
                        transactionScope.Rollback();
                        return;
                    }
                    var profileUser = adminCreateDTO.Adapt<Admin>();
                    profileUser.IdentityId = identityUser.Id;
                    await _adminRepository.AddAsync(profileUser);
                    await _adminRepository.SaveChangesAsync();
                    result = new SuccessDataResult<AdminDTO>("Admin added successfully!");
                    transactionScope.Commit();
                }
                catch (Exception ex)
                {
                    result = new ErrorDataResult<AdminDTO>("Admin could not be added!" + ex.Message);
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
            var deletingAdmin = await _adminRepository.GetByIdAsync(id);
            if (deletingAdmin == null)
            {
                return new ErrorDataResult<IResult>("Admin could not be found!");
            }

            var strategy = await _adminRepository.CreateExecutionStrategy();
            Result result = new ErrorResult();

            await strategy.ExecuteAsync(async () =>
            {
                var transactionScope = await _adminRepository.BeginTransactionAsync().ConfigureAwait(false);
                try
                {
                    var identityResult = await _accountService.DeleteUserAsync(deletingAdmin.IdentityId);
                    if (!identityResult.Succeeded)
                    {
                        result = new ErrorResult(identityResult.ToString());
                        transactionScope.Rollback();
                        return;
                    }
                    await _adminRepository.DeleteAsync(deletingAdmin);
                    await _adminRepository.SaveChangesAsync();
                    result = new SuccessResult("Admin deleted successfully!");
                    transactionScope.Commit();
                }
                catch (Exception ex)
                {
                    result = new ErrorResult("Admin could not be deleted" + ex.Message);
                    transactionScope.Rollback();
                }
                finally
                {
                    transactionScope.Dispose();
                }
            });
            return result;
        }

        public async Task<IDataResult<List<AdminListDTO>>> GetAllAsync()
        {
            var admins = await _adminRepository.GetAllAsync();
            var adminDTOs = admins.Adapt<List<AdminListDTO>>();
            if (admins.Count() <= 0)
            {
                return new ErrorDataResult<List<AdminListDTO>>(adminDTOs, "Admin could not be found!");
            }
            return new SuccessDataResult<List<AdminListDTO>>(adminDTOs, "Admin listed successfully!");

        }

        public async Task<IDataResult<AdminDTO>> GetByIdAsync(Guid id)
        {
            var admin = await _adminRepository.GetByIdAsync(id);
            if (admin == null)
            {
                return new ErrorDataResult<AdminDTO>("Admin could not be found!");
            }
            var adminDTO = admin.Adapt<AdminDTO>();
            return new SuccessDataResult<AdminDTO>(adminDTO, "Admin found successfully!");
        }

        public async Task<IDataResult<AdminDTO>> UpdateAsync(AdminUpdateDTO adminUpdateDTO)
        {
            var updatingAdmin = await _adminRepository.GetByIdAsync(adminUpdateDTO.Id);
            if (updatingAdmin == null)
            {
                return new ErrorDataResult<AdminDTO>("Admin could not be found!");
            }
            if (updatingAdmin.Email != adminUpdateDTO.Email && await _accountService.AnyAsync(x => x.Email == adminUpdateDTO.Email))
            {
                return new ErrorDataResult<AdminDTO>("Email is already taken!");
            }

            var identityUser = await _accountService.FindByIdAsync(updatingAdmin.IdentityId);
            identityUser.Email = adminUpdateDTO.Email;
            identityUser.NormalizedEmail = adminUpdateDTO.Email.ToUpperInvariant();
            identityUser.UserName = adminUpdateDTO.Email;
            identityUser.NormalizedUserName = adminUpdateDTO.Email.ToUpperInvariant();

            DataResult<AdminDTO> result = new ErrorDataResult<AdminDTO>();

            var strategy = await _adminRepository.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                var transectionScope = await _adminRepository.BeginTransactionAsync().ConfigureAwait(false);
                try
                {
                    var identityResult = await _accountService.UpdateUserAsync(identityUser);
                    if (!identityResult.Succeeded)
                    {
                        result = new ErrorDataResult<AdminDTO>(identityResult.ToString());
                        transectionScope.Rollback();
                        return;
                    }
                    var updatedAdmin = adminUpdateDTO.Adapt(updatingAdmin);

                    await _adminRepository.UpdateAsync(updatedAdmin);
                    await _adminRepository.SaveChangesAsync();
                    result = new SuccessDataResult<AdminDTO>("Admin updated successfully!");
                    transectionScope.Commit();
                }
                catch (Exception ex)
                {
                    result = new ErrorDataResult<AdminDTO>("Admin could not be updated!" + ex.Message);
                    transectionScope.Rollback();
                }
                finally
                {
                    transectionScope.Dispose();
                }
            });
            return result;
        }
		public async Task<IResult> ChangePasswordAsync(AdminChangePasswordDTO adminChangePasswordDTO)
		{
			var admin = await _adminRepository.GetByIdAsync(adminChangePasswordDTO.Id);
			if (admin == null)
			{
				return new ErrorResult("Admin bulunamadı!");
			}

			var identityUser = await _accountService.FindByIdAsync(admin.IdentityId);
			if (identityUser == null)
			{
				return new ErrorResult("Kimlik kullanıcısı bulunamadı!");
			}

			var result = await _accountService.ChangePasswordAsync(identityUser, adminChangePasswordDTO.CurrentPassword, adminChangePasswordDTO.NewPassword);
			if (!result.Succeeded)
			{
				return new ErrorResult("Şifre değiştirilemedi!");
			}

			return new SuccessResult("Şifre başarıyla değiştirildi!");
		}

		public async Task<IDataResult<AdminForgotPasswordDTO>> ResetPasswordRequestAsync(string email)
		{
			
			var identityUser = await _accountService.FindByEmailAsync(email);
			if (identityUser == null)
			{
				return new ErrorDataResult<AdminForgotPasswordDTO>("Admin could not be found!");
			}
			var code = await _accountService.GeneratePasswordResetTokenAsync(identityUser);
           
            var adminForgotPasswordDTO = new AdminForgotPasswordDTO();
            adminForgotPasswordDTO.Token = code;
            adminForgotPasswordDTO.Result = IdentityResult.Success;
            adminForgotPasswordDTO.Email = identityUser.Email;
           
			return new SuccessDataResult<AdminForgotPasswordDTO>(adminForgotPasswordDTO,"Şifre sıfırlama talebi başarıyla alındı!");
		}

		public async Task<IResult> ResetPasswordAsync(AdminResetPasswordDTO adminResetPasswordDTO)
		{
			var identityUser = await _accountService.FindByEmailAsync(adminResetPasswordDTO.Email);
			if (identityUser == null)
			{
				return new ErrorResult("Kullanıcı bulunamadı!");
			}

			var result = await _accountService.ResetPasswordAsync(identityUser, adminResetPasswordDTO.Code, adminResetPasswordDTO.Password);
			if (!result.Succeeded)
			{
				return new ErrorResult("Şifre sıfırlama başarısız!");
			}

			return new SuccessResult("Şifre başarıyla sıfırlandı!");
		}

		public async Task<IDataResult<AdminDTO>> GetByIdentityUserIdAsync(string identityUserId)
		{
			var admin = await _adminRepository.GetByIdentityId(identityUserId);
			if (admin == null)
			{
				return new ErrorDataResult<AdminDTO>("Admin bulunamadı!");
			}
			var adminDTO = admin.Adapt<AdminDTO>();
			return new SuccessDataResult<AdminDTO>(adminDTO, "Admin başarıyla bulundu!");
		}

	}
}

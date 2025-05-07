using ETicaret.Applicationn.DTOs.AdminDTOs;
using ETicaret.Domain.Utilities.Concretes;
using ETicaret.Domain.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.Services.AdminService
{
    public interface IAdminService
    {
        Task<IDataResult<List<AdminListDTO>>> GetAllAsync();
        Task<IDataResult<AdminDTO>> CreateAsync(AdminCreateDTO adminCreateDTO);
        Task<IResult> DeleteAsync(Guid id);
        Task<IDataResult<AdminDTO>> GetByIdAsync(Guid id);
        Task<IDataResult<AdminDTO>> UpdateAsync(AdminUpdateDTO adminUpdateDTO);
        //Change
        Task<IResult> ChangePasswordAsync(AdminChangePasswordDTO adminChangePasswordDTO);
		//Reset
        Task<IDataResult<AdminForgotPasswordDTO>> ResetPasswordRequestAsync(string email);
        Task<IResult> ResetPasswordAsync(AdminResetPasswordDTO adminResetPasswordDTO);

        Task<IDataResult<AdminDTO>> GetByIdentityUserIdAsync(string identityUserId);


	}
}

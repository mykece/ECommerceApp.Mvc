using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.DTOs.AdminDTOs
{
	public class AdminForgotPasswordDTO
	{
        public IdentityResult Result { get; set; }
        public string Email { get; set; }
		[ValidateNever]
		public string Token { get; set; }
	}
}

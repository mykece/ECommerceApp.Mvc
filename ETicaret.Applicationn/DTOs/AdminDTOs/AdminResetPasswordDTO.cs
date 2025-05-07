using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Applicationn.DTOs.AdminDTOs
{
	public class AdminResetPasswordDTO
	{
		public string Password { get; set; }
		public string Email { get; set; }
		public string Code { get; set; }
	}
}

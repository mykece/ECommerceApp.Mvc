using ETicaret.Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ETicaret.UI.Models.AccountVMs
{
    public class RegisterVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}




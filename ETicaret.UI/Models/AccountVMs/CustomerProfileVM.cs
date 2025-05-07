using ETicaret.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ETicaret.UI.Models.AccountVMs
{
    public class CustomerProfileVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateOfBirth { get; set; }
        public List<CustomerAddressVM> Addresses { get; set; }
    }
}

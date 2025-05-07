using Microsoft.AspNetCore.Mvc.Rendering;

namespace ETicaret.UI.Areas.Admin.Models.CustomerVMs
{
    public class CreateAddressVM
    {
        public Guid Id { get; set; }
        public string AddressName { get; set; }
        public string Address { get; set; }
        public Guid CustomerId { get; set; }
        //public SelectList? Customers { get; set; }
    }
}
